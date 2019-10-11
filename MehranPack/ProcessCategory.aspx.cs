using Common;
using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MehranPack
{
    public partial class ProcessCategory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Page.RouteData.Values["Id"].ToSafeInt() != 0)
                {
                    drpCat.Enabled = false;
                    var repo = new ProcessCategoryRepository();
                    var tobeEditedPC = repo.GetByCatIdWithDetails(Page.RouteData.Values["Id"].ToSafeInt());

                    var details = new List<ProcessCategoryHelper>();

                    foreach (ProcessCategoryHelper pc in tobeEditedPC)
                    {
                        details.Add(new ProcessCategoryHelper()
                        {
                            Id = pc.Id,
                            CategoryId = pc.CategoryId,
                            CategoryName = pc.CategoryName,
                            ProcessId = pc.ProcessId,
                            ProcessName = pc.ProcessName,
                            Order = pc.Order
                        });
                    }

                    Session["GridSource"] = details;
                }
                else
                    Session["GridSource"] = new List<ProcessCategoryHelper>();

                BindDrpCat();
                BindDrpProcess();
            }

            if (Session["GridSource"] == null)
                Session["GridSource"] = new List<ProcessCategoryHelper>();

            gridInput.DataSource = Session["GridSource"];
            gridInput.DataBind();
        }

        protected void gridSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                var s = (List<ProcessCategoryHelper>)Session["GridSource"];
                var tobeDeleted = s.SingleOrDefault(a => a.ProcessId == e.CommandArgument.ToSafeInt());
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

        private void BindDrpProcess()
        {
            var source = new ProcessRepository().GetAll().ToList();
            drpProcesses.DataSource = source;
            drpProcesses.DataValueField = "Id";
            drpProcesses.DataTextField = "Name";
            drpProcesses.DataBind();
        }

        private void BindDrpCat()
        {
            var source = new CategoryRepository().GetAll().ToList();
            drpCat.DataSource = source;
            drpCat.DataValueField = "Id";
            drpCat.DataTextField = "Name";
            drpCat.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var catId = drpCat.SelectedValue.ToSafeInt();
                var existingCatPr = new ProcessCategoryRepository().Get(a => a.CategoryId == catId).FirstOrDefault();
                if (Page.RouteData.Values["Id"].ToSafeInt() == 0 && existingCatPr !=null) throw new LocalException("duplicate cat", " فرآیندهای این گروه محصول قبلا ثبت شده اند");

                UnitOfWork uow = new UnitOfWork();

                if (Page.RouteData.Values["Id"].ToSafeInt() == 0)
                {
                    foreach (var item in ((List<ProcessCategoryHelper>)Session["GridSource"]))
                    {
                        var newPC = new Repository.Entity.Domain.ProcessCategory()
                        {
                            CategoryId = item.CategoryId,
                            ProcessId = item.ProcessId,
                            Order = txtOrder.Text.ToSafeInt()
                        };

                        uow.ProcessCategories.Create(newPC);
                    }
                }
                else
                {
                    var repo = uow.ProcessCategories;
                    var exsitedPCs = repo.Get(a => a.CategoryId == catId).ToList();

                    foreach (Repository.Entity.Domain.ProcessCategory item in exsitedPCs)
                    {
                        repo.Delete(item.Id);
                    }

                    foreach (var item in ((List<ProcessCategoryHelper>)Session["GridSource"]))
                    {
                        var newPC = new Repository.Entity.Domain.ProcessCategory()
                        {
                            CategoryId = item.CategoryId,
                            ProcessId = item.ProcessId,
                            Order = item.Order
                        };

                        uow.ProcessCategories.Create(newPC);
                    }
                }

                var result = uow.SaveChanges();
                if (result.IsSuccess)
                  ((Main)Page.Master).SetGeneralMessage("اطلاعات با موفقیت ذخیره شد", MessageType.Success);
                else
                    ((Main)Page.Master).SetGeneralMessage(result.ResultMessage, MessageType.Error);

                ClearControls();
            }
            catch (LocalException ex)
            {
                ((Main)Page.Master).SetGeneralMessage("خطا در دخیره سازی -" + ex.ResultMessage, MessageType.Error);
            }
        }

        private void ClearControls()
        {
            drpCat.Enabled = drpProcesses.Enabled = txtOrder.Enabled = false;
            gridInput.Enabled = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.RedirectToRoute("Home");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtOrder.Text)) throw new LocalException("order is empty", "ترتیب  را وارد نمایید");
                if (txtOrder.Text.ToSafeInt()==0) throw new LocalException("order is empty", "ترتیب  باید عددی مثبت باشد ");
                var gridSource = (List<ProcessCategoryHelper>)Session["GridSource"];

                if (gridSource != null && gridSource.Any() && gridSource.Any(a => a.Order == txtOrder.Text.ToSafeInt()))
                    throw new LocalException("duplicate order", "ترتیب نباید تکراری باشد");

                if (gridSource != null && gridSource.Any() && gridSource.Any(a => a.ProcessId == drpProcesses.SelectedValue.ToSafeInt()))
                    throw new LocalException("duplicate process", "فرآیند نباید تکراری باشد");

                var addedInput = new ProcessCategoryHelper()
                {
                    CategoryId = drpCat.SelectedValue.ToSafeInt(),
                    ProcessId = drpProcesses.SelectedValue.ToSafeInt(),
                    ProcessName = new ProcessRepository().GetById(drpProcesses.SelectedValue.ToSafeInt()).Name,
                    Order = txtOrder.Text.ToSafeInt()
                };

                if (Session["GridSource"] == null)
                    Session["GridSource"] = new List<ProcessCategoryHelper>();

                ((List<ProcessCategoryHelper>)Session["GridSource"]).Add(addedInput);
                gridInput.DataSource = Session["GridSource"];
                gridInput.DataBind();
            }
            catch (LocalException ex)
            {
                ((Main)Page.Master).SetGeneralMessage(ex.ResultMessage, MessageType.Error);
            }
        }

        protected void gridInput_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gridInput_OnRowDeleted(object sender, GridViewDeletedEventArgs e)
        {

        }
    }
}