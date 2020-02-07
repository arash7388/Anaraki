﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Repository.DAL;
using Telerik.Web.UI;

namespace MehranPack
{
    public partial class WorkLineSummaryReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //RadGridLocalizationProvider.CurrentProvider = new CustomLocalizationProvider();
                BindDrpOp();
                BindDrpReportType();
                Session["Result"] = null;
                //List<Filter> filters = new List<Filter>();
                //filters.Add(new Filter("InsertDateTime", OperationType.GreaterThanOrEqual, DateTime.Now.AddDays(-7)));
                //filters.Add(new Filter("InsertDateTime", OperationType.LessThanOrEqual, DateTime.Now));

                //var whereClause = ExpressionBuilder.GetExpression<WorkLineHelper>(filters);

                //Session["Result"] = new WorkLineRepository().GetAllForSummaryReport(1,whereClause);
                //BindGrid();
            }

            if (Session["Result"] != null && ((List<WorkLineSummary>)Session["Result"]).Any())
                BindGrid();
        }

        private void BindDrpOp()
        {
            var opSource = new UserRepository().GetAll().OrderBy(a => a.FriendlyName).ToList();

            var emptyUser = new Repository.Entity.Domain.User();
            emptyUser.Id = -1;
            emptyUser.FriendlyName = "انتخاب کنید...";
            opSource.Insert(0, emptyUser);
            drpOperator.DataSource = opSource;
            drpOperator.DataValueField = "Id";
            drpOperator.DataTextField = "FriendlyName";
            drpOperator.DataBind();
        }

        private void BindDrpReportType()
        {
            var source = new List<KeyValuePair<int, string>>();
            source.Add(new KeyValuePair<int, string>(1, "براساس فرآیند"));
            source.Add(new KeyValuePair<int, string>(2, "بر اساس محصول-فرآیند"));
            source.Add(new KeyValuePair<int, string>(3, "بر اساس  زمان تولید "));
            source.Add(new KeyValuePair<int, string>(4, "بر اساس  زمان تولید روزانه"));
            //source.Add(new KeyValuePair<int, string>(5, "بر اساس  زمان تولید هفتگی"));
            source.Add(new KeyValuePair<int, string>(6, "بر اساس  زمان تولید ماهانه"));
            source.Add(new KeyValuePair<int, string>(7, "بر اساس  تاخیر تولید روزانه"));
            source.Add(new KeyValuePair<int, string>(8, "بر اساس  تعجیل تولید روزانه"));
            source.Add(new KeyValuePair<int, string>(9, "بر اساس  تاخیر تولید ماهانه"));
            source.Add(new KeyValuePair<int, string>(10, "بر اساس  تعجیل تولید ماهانه"));

            drpReportType.DataSource = source;
            drpReportType.DataValueField = "Key";
            drpReportType.DataTextField = "Value";
            drpReportType.DataBind();
        }

        private void BindGrid()
        {
            RadGridReport.DataSource = Session["Result"];
            RadGridReport.DataBind();
        }

        protected void btnRun_OnClick(object sender, EventArgs e)
        {
            List<Filter> filters = new List<Filter>();
            var selectedOp = drpOperator.SelectedValue.ToSafeInt();
            if (selectedOp != -1 && selectedOp != 0) filters.Add(new Filter("OperatorId", OperationType.Equals, drpOperator.SelectedValue.ToSafeInt()));
            if (dtFrom.Date != "") filters.Add(new Filter("InsertDateTime", OperationType.GreaterThanOrEqual, dtFrom.Date.ToEnDate()));
            if (dtTo.Date != "") filters.Add(new Filter("InsertDateTime", OperationType.LessThanOrEqual, dtTo.Date.ToEnDate().AddDays(1).AddSeconds(-1)));

            var whereClause = filters.Count > 0 ? ExpressionBuilder.GetExpression<WorkLineHelper>(filters) : null;

            var repType = drpReportType.SelectedValue.ToSafeInt();
            //0 FriendlyName
            //1 ProductName
            //2 ProcessName
            //3 PersianDate
            //4 Year
            //5 Month
            //6 Day
            //7 Count
            //8 ProcessTime
            //9 ProcessDuration

            RadGridReport.Columns[1].Visible = repType == 2;
            RadGridReport.Columns[2].Visible = repType == 2 || repType == 7 || repType == 8 || repType == 9 || repType == 10;
            RadGridReport.Columns[3].Visible = repType == 1 || repType == 2;
            RadGridReport.Columns[7].Visible = repType == 1;
            RadGridReport.Columns[4].Visible = RadGridReport.Columns[5].Visible = repType == 1 || repType == 4 || repType == 6 || repType == 7 || repType == 8 || repType == 9 || repType == 10 ;
            RadGridReport.Columns[6].Visible = repType == 1 || repType == 4 || repType == 7 || repType == 8;
            RadGridReport.Columns[8].Visible = repType == 4 || repType == 3 || repType == 6 || repType == 7 || repType == 8 || repType == 9 || repType == 10;
            RadGridReport.Columns[9].Visible = repType == 4 || repType == 3 || repType == 6 || repType == 7 || repType == 8 || repType == 9 || repType == 10;
            Session["Result"] = new WorkLineRepository().GetAllForSummaryReport(repType, whereClause);
            BindGrid();
        }

        protected void btnExportToExcel_OnClick(object sender, ImageClickEventArgs e)
        {
            RadGridReport.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
            RadGridReport.ExportSettings.IgnorePaging = true;
            RadGridReport.ExportSettings.ExportOnlyData = true;
            RadGridReport.ExportSettings.OpenInNewWindow = true;
            RadGridReport.ExportSettings.FileName = "WorkLineSummaryReport-" + DateTime.Now.ToFaDateTime();
            RadGridReport.MasterTableView.ExportToExcel();
        }

        protected void btnExportToPdf_OnClick(object sender, ImageClickEventArgs e)
        {
            RadGridReport.ExportSettings.Pdf.Title = "خلاصه گزارش کارهای تولیدی";
            RadGridReport.ExportSettings.Pdf.DefaultFontFamily = "Arial Unicode MS";
            RadGridReport.MasterTableView.ExportToPdf();
        }

        protected void RadGridReport_OnItemCreated(object sender, GridItemEventArgs e)
        {
            //if (e.Item is GridDataItem)
            //{
            //    GridDataItem item = (GridDataItem)e.Item;
            //    foreach (TableCell cell in item.Cells)
            //    {
            //        cell.Style["font-family"] = "tahoma";
            //        cell.Style["text-align"] = "center";
            //        //cell.Style["font-size"] = (4 + e.Item.ItemIndex * 0.8) + "pt";

            //    }
            //}
            //else if (e.Item is GridHeaderItem)
            //{
            //    GridHeaderItem item = (GridHeaderItem)e.Item;
            //    foreach (TableCell cell in item.Cells)
            //    {
            //        cell.Style["font-family"] = "tahoma";
            //        cell.Style["text-align"] = "center";
            //        //cell.Style["font-size"] = (4 + e.Item.ItemIndex * 0.8) + "pt";
            //    }
            //}
        }

        protected void drpReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpReportType.SelectedValue.ToSafeInt() == 4 || drpReportType.SelectedValue.ToSafeInt() == 6)
            {
                lblHint.Visible = true;
            }
            else
            {
                lblHint.Visible = false;
            }
        }

        //public class CustomLocalizationProvider : RadGridLocalizationProvider
        //{
        //    public override string GetLocalizedString(string id)
        //    {
        //        switch (id)
        //        {
        //            case RadGridStringId.GroupByThisColumnMenuItem:
        //                return "My custom group by this column text";
        //        }

        //        return base.GetLocalizedString(id);
        //    }
        //}
    }
}