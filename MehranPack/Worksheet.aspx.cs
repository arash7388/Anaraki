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
using System.Web.Script.Serialization;
using System.Web.Script.Services;

namespace MehranPack
{
    public partial class Worksheet : Page
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

        [WebMethod]
        public static string GetDetails2(int id)
        {
            var repo = new WorksheetDetailRepository();
            var js = new JavaScriptSerializer();
            return js.Serialize(repo.Get(a=>a.WorksheetId==id).ToList());
            //return new
        }

        [WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object GetDetails()
        {
            var repo = new WorksheetDetailRepository();
            var js = new JavaScriptSerializer();
            return js.Serialize(repo.GetAll().ToList());
            //return new
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

            //gridInput.DataSource = Session["GridSource"];
            //gridInput.DataBind();
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


        [WebMethod]
        public static string Save(int userId,string date, int id, Repository.Entity.Domain.Worksheet model)
        {
            if (model == null)
            {
                //((Main)Page.Master).SetGeneralMessage("اطلاعاتی برای ذخیره کردن یافت نشد", MessageType.Error);
                return "اطلاعاتی برای ذخیره کردن یافت نشد";
            }

            if(!model.WorksheetDetails.Any())
                return "هیچ ردیفی ثبت نشده است";

            if(model.WorksheetDetails.GroupBy(a=>a.ProductId).Where(a => a.Count() > 1).Count()>0)
                return "ردیف با کالای تکراری ثبت شده است";

            var uow = new UnitOfWork();

            if (id.ToSafeInt() == 0)
            {
                var w = new Repository.Entity.Domain.Worksheet();

                var insDateTime = model.InsertDateTime;
                w.Date = Utility.AdjustTimeOfDate(date.ToEnDate());
                w.PartNo = model.PartNo;
                w.InsertDateTime = insDateTime;
                w.OperatorId = model.OperatorId;
                w.ColorId = model.ColorId;
                w.UserId = userId;
                w.Status = -1;
                w.WorksheetDetails = model.WorksheetDetails;

                uow.Worksheets.Create(w);
            }
            else
            {
                var tobeEdited = uow.Worksheets.GetById(id.ToSafeInt());

                uow.WorksheetDetails.Delete(a => a.WorksheetId == tobeEdited.Id);

                tobeEdited.UpdateDateTime = DateTime.Now;
                tobeEdited.Date = Utility.AdjustTimeOfDate(date.ToEnDate());
                tobeEdited.UserId = userId;
                tobeEdited.ColorId = model.ColorId;
                tobeEdited.PartNo = model.PartNo;

                foreach (WorksheetDetail item in model.WorksheetDetails)
                {
                    uow.WorksheetDetails.Create(new WorksheetDetail()
                    {
                        Status = -1,
                        ProductId = item.ProductId,
                        UpdateDateTime = DateTime.Now,
                        WorksheetId = tobeEdited.Id,
                    });
                }
            }

            var result = uow.SaveChanges();
            if (result.IsSuccess)
            {
                //RedirectToWorksheetListActionResultWithMessage();
                return "OK";
            }
            else
            {
                //((Main)Page.Master).SetGeneralMessage("خطا در ذخیره اطلاعات", MessageType.Error);
                Debuging.Error(result.ResultCode + "," + result.Message + "," + result.Message);
                return "خطا در ذخیره اطلاعات";
            }
        }


        private void RedirectToWorksheetListActionResultWithMessage()
        {
            throw new NotImplementedException();
        }

        private ICollection<WorksheetDetail> CastToWorksheetDetails(List<WorksheetDetailHelper> detailHelpers)
        {
            var result = new List<WorksheetDetail>();

            foreach (WorksheetDetailHelper detailHelper in detailHelpers)
            {
                result.Add(new WorksheetDetail()
                {
                    Id = detailHelper.Id,
                    //WorksheetId=detailHelper.WorksheetId,
                    ProductId = detailHelper.ProductId,
                    //InsertDateTime = Utility.AdjustTimeOfDate(detailHelper.InsertDateTime.ToEnDate()),
                    Status = -1,
                });
            }

            return result;
        }

        protected void gridSource_RowCommand(object sender, GridViewCommandEventArgs e)
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
            //gridInput.DataSource = Session["GridSource"];
            //gridInput.DataBind();
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

        private void BindTreeRecursive(List<Repository.Entity.Domain.Category> hierarchicalData, TreeNode node)
        {
            foreach (Repository.Entity.Domain.Category category in hierarchicalData)
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

                        foreach (Repository.Entity.Domain.Product product in catRelatedProducts)
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
