<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="WorkLineReport.aspx.cs" Inherits="MehranPack.WorkLineReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2014.2.724.45, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register TagPrefix="uc" TagName="PersianCalender" Src="~/UserControls/PersianCalender.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>گزارش کارهای تولیدی</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
        </Scripts>
    </telerik:RadScriptManager>

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGridReport">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridReport" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1">
    </telerik:RadAjaxLoadingPanel>

    <div class="row">
        <div class="col-sm-12">
            <h3>گزارش کارهای تولیدی</h3>
            <hr class="hrBlue" />
        </div>
    </div>

    <div class="row">
        <div class="col-xs-3 col-sm-1" align="left">
            اوپراتور:
        </div>
        <div class="col-xs-6 col-sm-3">
            <asp:DropDownList runat="server" ID="drpOperator" CssClass="form-control paddingTop0" Height="29"></asp:DropDownList>
        </div>
        <div class="col-xs-6 col-sm-3 col-sm-offset-5" align="left">
            <span class="input-group-btn">
                <asp:ImageButton runat="server" ID="btnExportToExcel" CssClass="btn btn-default btn50" ImageUrl="Images/excel16.png" OnClick="btnExportToExcel_OnClick"></asp:ImageButton>
                <asp:ImageButton runat="server" ID="btnExportToPdf" CssClass="btn btn-default btn50" ImageUrl="Images/pdf16.png" OnClick="btnExportToPdf_OnClick"></asp:ImageButton>
            </span>

            <%--<asp:Button  runat="server" ID="btnExportToExcel" OnClick="btnExportToExcel_OnClick"  CssClass="btn btn-info btn-standard" Height="33" Text="ارسال به اکسل"/>--%>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-1" align="left">
            از تاریخ:
        </div>
        <div class="col-sm-3">
            <uc:PersianCalender runat="server" ID="dtFrom" AllowEmpty="True" TimePanel-Visible="False" />
        </div>

        <div class="col-sm-1" align="left">
            تا تاریخ:
        </div>
        <div class="col-sm-3">
            <uc:PersianCalender runat="server" ID="dtTo" AllowEmpty="True" TimePanel-Visible="False" />
        </div>
        <div class="col-sm-2 col-sm-offset-2" align="left">
            <asp:Button runat="server" ID="btnRun" CssClass="btn btn-info btn-standard" OnClick="btnRun_OnClick" Text="اجرا" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-sm-12">


            <telerik:RadGrid ID="RadGridReport" runat="server"
                AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                CellSpacing="-1" GridLines="Both" PageSize="20" Height="605px"
                OnItemCreated="RadGridReport_OnItemCreated">
                <ClientSettings AllowColumnsReorder="True">
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                </ClientSettings>
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="ProductCode" FilterControlAltText="Filter column column" HeaderText="کد محصول" ReadOnly="True" SortExpression="ProductCode" UniqueName="column" AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo" DataType="System.Int32" FilterDelay="1200" FilterImageToolTip="فیلتر" MaxLength="50">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                            <HeaderStyle Font-Names="bkoodak" Font-Bold="True" Font-Size="Medium" Width="120px" />
                            <ItemStyle Font-Names="bkoodak" Font-Size="10" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="ProductName" FilterControlAltText="Filter column1 column" HeaderText="محصول" ReadOnly="True" UniqueName="column1" AllowFiltering="true" AutoPostBackOnFilter="True" FilterImageToolTip="فیلتر" CurrentFilterFunction="EqualTo" MaxLength="200" DataType="System.string">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                            <HeaderStyle Font-Names="bkoodak" Font-Bold="True" Font-Size="Medium" />
                            <ItemStyle Font-Names="bkoodak" Font-Size="10" />
                        </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn DataField="OperatorName" FilterControlAltText="Filter column2 column" HeaderText="اوپراتور" ReadOnly="True" SortExpression="OperatorName" UniqueName="column2" AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo" FilterDelay="1500" DataType="System.string" FilterImageToolTip="فیلتر" MaxLength="150" ">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                            <HeaderStyle Font-Names="bkoodak" Font-Bold="True" Font-Size="Medium" />
                            <ItemStyle Font-Names="bkoodak" Font-Size="10" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="PersianInsertDateTime" FilterControlAltText="Filter column3 column" HeaderText="تاریخ" ReadOnly="True" SortExpression="PersianInsertDateTime" UniqueName="column3" AllowFiltering="False" FilterImageToolTip="فیلتر" MaxLength="150">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                            <HeaderStyle Font-Names="bkoodak" Font-Bold="True" Font-Size="Medium" Width="110px" />
                            <ItemStyle HorizontalAlign="Center" Font-Names="bkoodak" Font-Size="10"></ItemStyle>
                        </telerik:GridBoundColumn>

         <%--               <telerik:GridBoundColumn AllowSorting="False" AutoPostBackOnFilter="True" CurrentFilterFunction="Contains" DataField="Description" FilterControlAltText="Filter column4 column" FilterDelay="1200" FilterImageToolTip="فیلتر" HeaderText="توضیحات" ReadOnly="True" UniqueName="column4">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                            <HeaderStyle Font-Names="bkoodak" Font-Bold="True" Font-Size="Medium" Width="120px" />
                            <ItemStyle Font-Names="bkoodak" Font-Size="10" />
                        </telerik:GridBoundColumn>--%>
                    </Columns>
                </MasterTableView>
                <PagerStyle PageSizes="20,30,50" PagerTextFormat="{4}<strong>{5}</strong> مورد"
                    PageSizeLabelText="تعداد ردیف ها:" />
            </telerik:RadGrid>

        </div>
    </div>
</asp:Content>
