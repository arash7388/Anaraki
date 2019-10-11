using Common;
using Repository.DAL;
using Repository.Entity.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace MehranPack
{
    public partial class Worksheet : SystemUI.Page
    {
        [WebMethod]
        public static string GetProductCodeAndName(int productId) //it sets p id too
        {
            var p = new ProductRepository().GetById(productId);
            if (p != null)
            {
                HttpContext.Current.Session["ProductId"] = p.Id;
                return p.Code + "," + p.Name;
            }

            return "";
        }

        [WebMethod]
        public static string GetCategoryName(int catId)
        {

            return new CategoryRepository().GetById(catId)?.Name;
        }

        [WebMethod]
        public static string AddProduct(int productId)
        {
            return "";
            //((List<WorksheetDetailHelper>)Session["GridSource"]).Add();
            //return new CategoryRepository().GetById(catId)?.Name;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Page.RouteData.Values["Id"].ToSafeInt() != 0)
                {
                    var repo = new WorksheetRepository();
                    var tobeEditedWorksheet = repo.GetByIdWithDetails(Page.RouteData.Values["Id"].ToSafeInt());

                    dtWorksheet.Date = tobeEditedWorksheet.InsertDateTime.ToString();
                    txtPart.Text = tobeEditedWorksheet.PartNo.ToString();

                    var details = new List<WorksheetDetailHelper>();

                    foreach (WorksheetDetail d in tobeEditedWorksheet.WorksheetDetails)
                    {
                        details.Add(new WorksheetDetailHelper()
                        {
                            Id = d.Id,
                            ProductId = d.ProductId,
                            ProductName = d.Product.Name,
                            CategoryId = d.Product.ProductCategoryId,
                            CategoryName = new CategoryRepository().GetById(d.Product.ProductCategoryId).Name,
                            //InsertDateTime = ((DateTime)d.InsertDateTime).ToFaDate(),
                        });
                    }

                    Session["GridSource"] = details;
                }
                else
                    Session["GridSource"] = new List<InputHelper>();

                dtWorksheet.LoadCurrentDateTime = true;
                
                BindDrpUsers();
                BindDrpColors();
            }

            BindTreeview();
            tv1.ExpandAll();

            if (Session["GridSource"] == null)
                Session["GridSource"] = new List<WorksheetDetailHelper>();

            gridInput.DataSource = Session["GridSource"];
            gridInput.DataBind();
        }

        private void BindDrpColors()
        {
            var source = new BaseRepository<Color>().GetAll().ToList();
            drpColor.DataSource = source;
            drpColor.DataValueField = "Id";
            drpColor.DataTextField = "Name";
            drpColor.DataBind(); ;
        }

        private void BindDrpUsers()
        {
            var source = new UserRepository().GetAll().ToList();
            drpOperator.DataSource = source;
            drpOperator.DataValueField = "Id";
            drpOperator.DataTextField = "FriendlyName";
            drpOperator.DataBind();
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
        }

        protected void Save(string modelmodel)
        {
            var typedModel = JsonConvert.DeserializeObject<List<WorksheetDetailHelper>>(model);
            var source = typedModel;

            if (source.Count == 0)
            {
                ((Main)Page.Master).SetGeneralMessage("اطلاعاتی برای ذخیره کردن یافت نشد", MessageType.Error);
                return;
            }
            var uow = new UnitOfWork();

            if (Page.RouteData.Values["Id"].ToSafeInt() == 0)
            {
                var w = new Repository.Entity.Domain.Worksheet();
                
                var insDateTime = Utility.AdjustTimeOfDate(dtWorksheet.Date.ToEnDate());
                w.InsertDateTime = insDateTime;
                w.UserId = ((User)Session["User"]).Id;
                w.Status = -1;
                w.WorksheetDetails = CastToIODetail((List<InputHelper>)Session["GridSource"]);

                uow.Worksheets.Create(w);
            }
            else
            {
                var tobeEdited = uow.InputOutputs.GetById(Page.RouteData.Values["Id"].ToSafeInt());

                uow.InputOutputDetails.Delete(a => a.InputOutputId == tobeEdited.Id);

                tobeEdited.InsertDateTime = Utility.AdjustTimeOfDate(dtInputOutput.Date.ToEnDate());

                foreach (InputHelper item in source)
                {
                    uow.InputOutputDetails.Create(new InputOutputDetail()
                    {
                        Count = item.Count,
                        CustomerId = item.CustomerId,
                        InsertDateTime = Utility.AdjustTimeOfDate(item.InsertDateTime.ToEnDate()),
                        Status = -1,
                        ProductId = item.ProductId,
                        UpdateDateTime = DateTime.Now,
                        InputOutputId = tobeEdited.Id,
                        ProductionQuality = item.ProductionQualityId
                    });
                }
            }

            var result = uow.SaveChanges();
            if (result.IsSuccess)
            {
                RedirectFactorListActionResultWithMessage();
            }
            else
            {
                ((Main)Page.Master).SetGeneralMessage("خطا در ذخیره اطلاعات", MessageType.Error);
                Debuging.Error(result.Message);
            }
        }

        protected gridSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                var s = (List<WorksheetDetailHelper>)Session["GridSource"];
                var tobeDeleted = s.SingleOrDefault(a => a.ProductId == e.CommandArgument.ToSafeInt());
                if (tobeDeleted != null)
                    s.Remove(tobeDeleted);
                BindGrid();
            }
        }

        private void BindGrid()
        {
            gridInput.DataSource = Session["GridSource"];
            gridInput.DataBind();
        }

        protected void b1_OnClick(object sender, EventArgs e)
        {
            BindTreeview();
            tv1.ExpandAll();
        }

        private void BindTreeview()
        {
            var hierarchicalData = new CategoryRepository().GethierarchicalTree();
            tv1.Nodes.Clear();
            var root = new CustomTreeNode("گروه ها", "0", "", "", "");
            tv1.Nodes.Add(root);
            BindTreeRecursive(hierarchicalData, root);
        }

        protected void tv1_OnSelectedNodeChanged(object sender, EventArgs e)
        {

        }

        private void BindTreeRecursive(List<Category> hierarchicalData, TreeNode node)
        {
            foreach (Category category in hierarchicalData)
            {
                if (category.Children.Any())
                {
                    var n = new CustomTreeNode(TreeNodeSelectAction.None, category.Name + "(" + category.Code + ")", "", "", category.Id.ToString(), category.Name);
                    node.ChildNodes.Add(n);
                    BindTreeRecursive(category.Children.ToList(), n);
                }
                else
                {
                    var n = new CustomTreeNode(category.Name, "", "", category.Id.ToString(), category.Name);
                    node.ChildNodes.Add(n);
                    if (n.Parent != null) n.Parent.SelectAction = TreeNodeSelectAction.None;

                    if (new ProductRepository().Get(a => a.ProductCategoryId == category.Id).Any())
                    {
                        var catRelatedProducts = new ProductRepository().Get(a => a.ProductCategoryId == category.Id).ToList();
                        n.SelectAction = TreeNodeSelectAction.None;

                        foreach (Product product in catRelatedProducts)
                        {
                            if (string.IsNullOrWhiteSpace(txtSearchTree.Text))
                                n.ChildNodes.Add(new CustomTreeNode(product.Name + "(" + product.Code + ")", product.Id.ToString(), product.Name, product.ProductCategoryId.ToString(), product.ProductCategory?.Name));
                            else if (product.Name.Contains(txtSearchTree.Text) || product.Code.Contains(txtSearchTree.Text))
                                n.ChildNodes.Add(new CustomTreeNode(product.Name + "(" + product.Code + ")", product.Id.ToString(), product.Name, product.ProductCategoryId.ToString(), product.ProductCategory?.Name));
                        }
                    }
                }
            }
        }
  }
    }
}