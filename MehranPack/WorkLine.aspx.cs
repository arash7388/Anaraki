using Common;
using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MehranPack
{
    public partial class WorkLine : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            {
                //this.Master.Visible = false;
                var repo = new WorkLineRepository();
                gridWorkLine.DataSource = repo.GetTodayWorkLine();
                gridWorkLine.DataBind();
                lblCurrentDate.Text = Common.Utility.CastToFaDate(DateTime.Now).ToPersianNumber();
                Session["Result"] = gridWorkLine.DataSource;
                txtBarcodeInput.Focus();
            }
        }

        [WebMethod]
        public static string AddRow(string input)
        {
            //WID,OperatorID,ProductID,ProcessID,Order
            var parts = input.Split(',');

            var worksheetId = parts[0].ToSafeInt();
            var operatorId = parts[1].ToSafeInt();
            var productId = parts[2].ToSafeInt();
            var processId = parts[3].ToSafeInt();
            var order = parts[4].ToSafeInt();

            var prevOrder = HttpContext.Current.Session[worksheetId + "#" + operatorId + "#" + productId].ToSafeInt();

            if (prevOrder > order ||  order - prevOrder != 1)
                return "عدم رعایت ترتیب فرآیند";

            //var repo = new WorkLineRepository();
            //if (repo.Get(a => a.WorksheetId == worksheetId && a.OperatorId == operatorId
            //    && a.ProductId == productId).Any())
            //{
            //    return "ردیف تکراری";
            //}

            var uow = new UnitOfWork();
            uow.WorkLines.Create(new Repository.Entity.Domain.WorkLine()
            {
                InsertDateTime = DateTime.Now,
                WorksheetId = worksheetId,
                OperatorId = operatorId,
                ProductId = productId,
                ProcessId = processId
            }
            );

            var result = uow.SaveChanges();
            if (result.IsSuccess)
            {
                HttpContext.Current.Session[worksheetId + "#" + operatorId + "#" + productId] = order;
                return "OK";
            }
            else
            {
                //((Main)Page.Master).SetGeneralMessage("خطا در ذخیره اطلاعات", MessageType.Error);
                Debuging.Error(result.ResultCode + "," + result.Message + "," + result.Message);
                return "خطا در اضافه کردن ردیف";
            }
        }

        protected void gridWorkLine_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                //Response.Redirect("Category.aspx?Id=" + e.CommandArgument);
            }
            else
            if (e.CommandName == "Delete")
            {
                var data = new ConfirmData();

                data.Command = "Delete";
                data.Id = e.CommandArgument.ToSafeInt();
                data.Msg = "آیا از حذف اطمینان دارید؟";
                data.Table = "Worklines";
                data.RedirectAdr = "Workline.aspx";

                Session["ConfirmData"] = data;
                Response.RedirectToRoute("Confirmation");
                Response.End();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //Response.Redirect("Category.aspx");
        }

        protected void gridWorkLine_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridWorkLine.PageIndex = e.NewPageIndex;
            gridWorkLine.DataSource = Session["Result"];
            gridWorkLine.DataBind();
        }


    }
}