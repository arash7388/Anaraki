<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Worksheet.aspx.cs" Inherits="MehranPack.Worksheet" ClientIDMode="Static" %>

<%@ Register Src="UserControls/PersianCalender.ascx" TagName="PersianCalender" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/css/datatables.min.css" rel="stylesheet" />
    <script src="/Content/js/datatables.min.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="row">
        <div class="col-md-12 text-center">
            <h3>برگه کار</h3>
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
        <div class="col-md-10">
            <table id="table1" class="display table table-bordered table-striped" style="width: 100%">
                <thead>
                    <tr>
                        <th>شناسه محصول</th>
                        <th>کد محصول</th>
                        <th>گروه محصول</th>
                        <th>نام محصول</th>
                        <th>ACode</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                </tbody>

            </table>
        </div>
    </div>
    <hr class="hrGray" />



    <div class="row">
        <div class="col-md-6">
            <button type="button" class="btn btn-info btn-standard" data-toggle="modal" data-target="#myModal">اضافه کردن کالا</button>
            &nbsp;
            <asp:Button Style="min-width: 100px !important" runat="server" ID="btnSave" Text="ذخیره" CssClass="btn btn-black btn-standard btnSubmit" ></asp:Button>
            &nbsp;
            <asp:Button Style="min-width: 100px !important" runat="server" ID="btnSaveAndPrint" Text="ذخیره و چاپ" CssClass="btn btn-black btn-standard btnSubmit" ></asp:Button>
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
                        <div class="col-md-10">
                            <label>جستجو</label>
                            <asp:TextBox runat="server" ID="txtSearchTree" placeholder="نام کالا"></asp:TextBox>
                            <asp:Button runat="server" ID="b1" OnClick="b1_OnClick" Text="جستجو" UseSubmitBehavior="false"/>
                            <br />
                            <asp:TextBox runat="server" ID="txtACode" placeholder="ACode"></asp:TextBox>
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

            tbl = $('#table1').DataTable({
                "bFilter": false,
                "paging": false,
                "ordering": false,
                "info": false,
                "processing": true,
                "serverSide": false,

                "columns": [
                    { 'title': 'شناسه محصول' },
                    { 'title': 'کد محصول' },
                    { 'title': 'نام محصول' },
                    { 'title': 'ACode' },
                    { 'title':"" }
                ]
            });

            var url = $(location).attr('href'),
                parts = url.split("/"),
                id = parts[parts.length - 1];

            var adr = "/DataHandler.ashx?id=" + id
            $.ajax({
                type: "POST",
                url: adr,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    jQuery.each(data, function (i, row) {
                       
                        tbl.row.add([
                            row["ProductId"],
                            row["ProductCode"],
                            row["ProductName"],
                            row["ACode"],
                            '<span><button type="button" onclick="onDeleteClicked();">حذف</button></span>'
                        ]).draw(false);
                    });
                },
                error: function (data) {
                    alert("Error" + data);
                }

            });



            $('#table1 tbody').on('click', 'tr', function () {
                if ($(this).hasClass('selected')) {
                    $(this).removeClass('selected');
                }
                else {
                    tbl.$('tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                }
            });

        });

        var buttonpressed;
        $('.btnSubmit').click(function () {
            buttonpressed = $(this).attr('id')
        });


        $('#myModal').on('shown', function (e) {
            debugger;
            $("#txtACode").val('');
            $("#txtSearchTree").val('');
        });


        $('#form1').on('submit', function (e) {
            debugger;
            var operatorId = $("#drpOperator").val();
            var colorId = $("#drpColor").val();
            var partNo = $("#txtPart").val();
            var date = $("#year").val() + "/" + $("#mounth").val() + "/" + $("#day").val()

            e.preventDefault();

            var allRows = tbl.rows().data();
            var model0 = '{"OperatorId":' + operatorId + ',"ColorId":' + colorId + ',"PartNo":"' + partNo + '","Date":"' + date + '","WorksheetDetails": ['
            var model = model0;

            jQuery.each(allRows, function (i, row) {

                if (model != model0)
                    model = model + ",";

                model = model +
                    '{"ProductId" : "' + row[0] + '",' +
                    '"ACode" : "' + row[3] + '",' +
                    '"Status" : "-1"}';
            });

            model += "]}";

            var userId = $("#hfUserId").val();

            var url = $(location).attr('href'),
                parts = url.split("/"),
                id = parts[parts.length - 1];


            var methodParams = '{userId:"' + userId + '",date:"' + date + '",id:"' + id + '",model:' + model + '}'
            $.ajax({
                url: '<%= ResolveUrl("~/worksheet.aspx/Save") %>',
                type: "POST",
                data: methodParams,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d == "OK") {
                        debugger;
                        if (buttonpressed=="btnSave")
                            window.location.href = window.location.origin + "/worksheetlist";
                        else if (buttonpressed == "btnSaveAndPrint")
                            window.location.href = window.location.origin + "/worksheetprint.aspx?id="+id;

                    }
                    else alert(data.d);
                },
                error: function (data) {
                    alert("an error occured! " + data.Message);
                }
            });

        });

        function onDeleteClicked() {
           tbl.row('.selected').remove().draw(false);
        }


        function onNodeClicked(productId, catId) {
            $('#myModal').modal('hide');
            var t = $('#table1').DataTable();

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
                var prCode = response.d.split(',')[0];
                var prName = response.d.split(',')[1];

                t.row.add([
                    productId,
                    prCode,
                    prName,
                    'A' + $("#year").val().substr(2,2) + $("#mounth").val() + $("#txtACode").val(),
                    '<span><button type="button" onclick="onDeleteClicked();">حذف</button></span>'
                ]).draw(false);
            };
        }

    </script>
</asp:Content>
