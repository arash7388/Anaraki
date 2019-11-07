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
                var repo = new WorkLineRepository();
                gridWorkLine.DataSource = repo.GetTodayWorkLine();
                gridWorkLine.DataBind();
                lblCurrentDate.Text = Common.Utility.CastToFaDate(DateTime.Now);
                Session["Result"] = gridWorkLine.DataSource;
                txtBarcodeInput.Focus();
            }
        }

        [WebMethod]
        public static string AddRow(string input)
        {
            //if (HttpContext.Current.Session["Result"] == null)
            //    HttpContext.Current.Session["Result"] = new List<WorkLineHelper>();

            //WID,OperatorID,ProductID,ProcessID
            var parts = input.Split(',');

            var uow = new UnitOfWork();
            uow.WorkLines.Create(new Repository.Entity.Domain.WorkLine()
            {
                InsertDateTime = DateTime.Now,
                OperatorId= parts[1].ToSafeInt(),
                ProductId=parts[2].ToSafeInt(),
                ProcessId=parts[3].ToSafeInt()

            }
            ) ;

            //((List<WorkLineHelper>)HttpContext.Current.Session["Result"]).Add(new WorkLineHelper()
            //{

            //});

            var result = uow.SaveChanges();
            if (result.IsSuccess)
            {
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
                data.RedirectRoute = "Worklines.aspx";

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