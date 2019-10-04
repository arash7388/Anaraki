﻿using Common;
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
               // if (string.IsNullOrEmpty(txtName.Text)) throw new LocalException("Name is empty", "نام  را وارد نمایید");

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
                    var tobeEditedPC = repo.GetById(Page.RouteData.Values["Id"].ToSafeInt());
                   
                    //tobeEditedPr.Name = txtName.Text;
                    
                    //tobeEditedPr.Type = drpKind.SelectedValue.ToSafeInt();
                }

                uow.SaveChanges();
                ((Main)Page.Master).SetGeneralMessage("اطلاعات با موفقیت ذخیره شد", MessageType.Success);
                ClearControls();
            }
            catch (LocalException ex)
            {
                ((Main)Page.Master).SetGeneralMessage("خطا در دخیره سازی -" + ex.ResultMessage, MessageType.Error);
            }
        }

        private void ClearControls()
        {
            //txtName.Text = "";
        }
        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.RedirectToRoute("Home");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var addedInput = new ProcessCategoryHelper()
            {
                CategoryId = drpCat.SelectedValue.ToSafeInt(),
                ProcessId = drpProcesses.SelectedValue.ToSafeInt(),
                ProcessName = new ProcessRepository().GetById(drpProcesses.SelectedValue.ToSafeInt()).Name,
                Order = txtOrder.ToSafeInt()
            };

            if (Session["GridSource"] == null)
                Session["GridSource"] = new List<ProcessCategoryHelper>();

            ((List<ProcessCategoryHelper>)Session["GridSource"]).Add(addedInput);
            gridInput.DataSource = Session["GridSource"];
            gridInput.DataBind();
        }

        protected void gridInput_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gridInput_OnRowDeleted(object sender, GridViewDeletedEventArgs e)
        {

        }
    }
}