<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Worksheet.aspx.cs" Inherits="MehranPack.Worksheet" %>

<%@ Register Src="UserControls/PersianCalender.ascx" TagName="PersianCalender" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/css/datatables.min.css" rel="stylesheet" />
    <script src="Content/js/datatables.min.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="row">
        <div class="col-md-12 text-center">
            <h3>Worksheet</h3>
        </div>
    </div>
    <hr class="hrBlue" />
    <div class="row">
        <div class="col-sm-1" align="left">
            اپراتور:
        </div>

        <div class="col-sm-4">
            <asp:DropDownList runat="server" ID="drpOperator"></asp:DropDownList>
        </div>


        <div class="col-sm-1" align="left">
            تاریخ:
        </div>

        <div class="col-sm-4">
            <uc:PersianCalender runat="server" ID="dtWorksheet" AllowEmpty="False" timepanel-visible="False" />
        </div>
    </div>

    <div class="row">
        <div class="col-sm-1" align="left">
            پارت:
        </div>

        <div class="col-sm-4">
            <asp:TextBox runat="server" ID="txtPart"></asp:TextBox>
        </div>


        <div class="col-sm-1" align="left">
            رنگ:
        </div>

        <div class="col-sm-4">
            <asp:DropDownList runat="server" ID="drpColor"></asp:DropDownList>
        </div>
    </div>

    <hr />

    <div class="row">
        <div class="col-md-8">
            <asp:GridView runat="server" AutoGenerateColumns="False" Width="500px" ID="gridInput" CssClass="table table-bordered table-striped" DataKeyNames="Id"
                OnRowCommand="gridSource_RowCommand" AllowPaging="false" PageSize="10"
                EmptyDataText="اطلاعاتی جهت نمایش وجود ندارد">

                <PagerStyle CssClass="gridPagerStyle" HorizontalAlign="Center" Wrap="False" />

                <Columns>
                    <asp:BoundField DataField="ProductId" HeaderText="محصول" Visible="false">
                        <ControlStyle BorderColor="#FFFF99" BorderStyle="Solid" />
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField DataField="ProductName" HeaderText="محصول" Visible="true">
                        <ControlStyle BorderColor="#FFFF99" BorderStyle="Solid" />
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                Text="حذف" CommandArgument='<%# Eval("ProductId") %>' />
                            <asp:Image runat="server" ImageUrl="Images/Delete16.png" />
                        </ItemTemplate>
                        <ItemStyle Width="70"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="row">
        <div class="col-md-10">
            <table id="table1" class="display" style="width: 100%">
                <thead>
                    <tr>
                        <th>ProductId</th>
                        <th>ProductCode</th>
                        <th>ProductName</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>1</td>
                        <td>01</td>
                        <td>محصول 1</td>
                        <td><span><button type="button" onclick="onDeleteClicked();">حذف</button></span></td>
                    </tr>
                </tbody>
                <%--<tfoot>
            <tr>
                <th>ProductId</th>
                <th>ProductCode</th>
                <th>ProductName</th>
            </tr>
        </tfoot>--%>
            </table>
        </div>
    </div>
    <hr class="hrGray" />



    <div class="row">
        <div class="col-md-4">
            <button type="button" class="btn btn-info btn-standard" data-toggle="modal" data-target="#myModal">اضافه کردن کالا</button>
            &nbsp;
            <asp:Button Style="min-width: 100px !important" runat="server" ID="btnSave" Text="ذخیره" CssClass="btn btn-black btn-standard" OnClick="btnSave_Click"></asp:Button>
            <%--<asp:Button runat="server" ID="Button1" Text="اضافه" CssClass="btn btn-info btn-standard" OnClick="btnAdd_Click"></asp:Button>--%>
        </div>
    </div>


    <div class="row">
        <div class="col-md-10 col-md-offset-2">
            <div class="label label-info lblResult" runat="server" id="lblResult"></div>
        </div>
    </div>

    <!-- Modal -->
    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">انتخاب کالا</h4>
                </div>

                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <label>جستجو</label>
                            <asp:TextBox runat="server" ID="txtSearchTree" placeholder="نام کالا"></asp:TextBox>
                            <asp:Button runat="server" ID="b1" OnClick="b1_OnClick" Text="جستجو" />
                            <%--<asp:Button runat="server" ID="btnClearSearch" OnClick="btnClearSearch_OnClick" Text="جستجو" />--%>
                            <br />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">

                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TreeView runat="server" ID="tv1" OnSelectedNodeChanged="tv1_OnSelectedNodeChanged"></asp:TreeView>
                                </ContentTemplate>

                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="b1" EventName="Click" />
                                </Triggers>

                            </asp:UpdatePanel>

                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">انصراف</button>
                </div>
            </div>

        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#table1').DataTable({
                "paging": false,
                "ordering": false,
                "info": false
            });

            $('#table1 tbody').on('click', 'tr', function () {
                if ($(this).hasClass('selected')) {
                    $(this).removeClass('selected');
                }
                else {
                    t.$('tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                }
            });
        });

        $('#form1').on('submit', function (e) {
            debugger;

            e.preventDefault();

            var allRows = t.rows().data();
            var model = "[";

            jQuery.each(allRows, function (i, row) {

                if (model != "[")
                    model = model + ",";

                model = model +
                    '{"InsertDateTime" : "' + row[0] + '",' +
                    '"BranchId" : ' + row[1] + ',' +
                    '"VisaExpDate" : "' + row[7] + '",' +
                    '"IsDeleted" : "false"}';
            });

            model += "]";

            $.ajax({
                url: '@Url.Action("Save","Home")',
                type: "POST",
                data: model,
                dataType: "json",
                traditional: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.IsSuccess == true) {
                        alert('Row added successfuly.');
                        $("#table1").html(data.Data);
                    }
                },
                error: function (data) {
                    debugger;
                    alert("an error occured! " + data.Message);
                }
            });

        });

        function onDeleteClicked() {
            t.row('.selected').remove().draw(false);
        }


        function onNodeClicked(productId, catId) {
            debugger;
            $('#myModal').modal('hide');
            t = $('#table1').DataTable();

            //var checkState = $("#activeCheckBox")[0].checked == true ? "checked=\"true\"" : "";

            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("~/worksheet.aspx/GetProductCodeAndName") %>',
                data: '{productId:' + productId + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: onSuccess,
                failure: function (response) {
                    alert('error in GetProductName!');
                }
            });

            function onSuccess(response) {
                debugger;
                var prCode = response.d.split(',')[0];
                var prName = response.d.split(',')[1];

                t.row.add([
                    productId,
                    prCode,
                    prName,
                    '<span><button type="button" onclick="onDeleteClicked();">حذف</button></span>'
                ]).draw(false);
            };
        }

    </script>
</asp:Content>
