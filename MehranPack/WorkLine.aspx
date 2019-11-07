<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkLine.aspx.cs" Inherits="MehranPack.WorkLine" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>صف کارهای در حال تولید</title>
    <meta content="Automation" />
    <link href="/Content/css/bootstrap-rtl.css" rel="stylesheet" />
    <link href="/Content/css/custom.css" rel="stylesheet" />

    <script src="/Scripts/jquery-1.10.2.js"></script>
    <script src="/Scripts/bootstrap-rtl.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="row">
            <div class="col-md-7" style="padding-right: 75px;">
                <h3 style="margin-top: 35px;">صف کارهای در حال تولید</h3>
            </div>
            <div class="col-md-3 text-left" style="margin-top:20px;">
                <h4>
                    <asp:Label runat="server" ID="lblCurrentDate"></asp:Label> &nbsp;
                    <asp:Label runat="server" ID="lblCurrentTime"></asp:Label>
                </h4>
            </div>
            <div class="col-md-2 text-left">
                <img src="Images/logo.png" class="img-responsive" style="width: 110px; height: 70px; margin-top: 10px" />
            </div>
        </div>

        <div class="row">

            <div class="col-md-12 text-center" style="padding-right: 45px;">
                <hr class="hrBlue" style="width: 95%; margin-top: 5px !important" />
                <br />
                <asp:GridView runat="server" AutoGenerateColumns="False" Width="95%" ID="gridWorkLine" CssClass="table table-bordered table-striped" DataKeyNames="Id"
                    OnRowCommand="gridWorkLine_RowCommand" AllowPaging="True" PageSize="10" OnPageIndexChanging="gridWorkLine_OnPageIndexChanging">
                    <PagerStyle CssClass="gridPagerStyle" HorizontalAlign="Center" Wrap="False" />

                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="شناسه">
                            <ControlStyle BorderColor="#FFFF99" BorderStyle="Solid" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="OperatorName" HeaderText="اوپراتور">
                            <ControlStyle BorderColor="#FFFF99" BorderStyle="Solid" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="ProductCode" HeaderText="کد محصول">
                            <ControlStyle BorderColor="#FFFF99" BorderStyle="Solid" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="ProductName" HeaderText="نام محصول">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="ProcessName" HeaderText="فرآیند">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="PersianInsertDateTime" HeaderText="زمان ایجاد">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="false"
                                    CommandName="Edit"
                                    CommandArgument='<%# Eval("Id") %>'
                                    Text="ویرایش" />
                                <asp:Image runat="server" ImageUrl="Images/Edit16.png" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="false"
                                    CommandName="Delete"
                                    CommandArgument='<%# Eval("Id") %>'
                                    Text="حذف" />
                                <asp:Image runat="server" ImageUrl="Images/Delete16.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:Button runat="server" ID="btnAdd" Text="جدید" CssClass="btn btn-black btn-standard" OnClick="btnAdd_Click" />
                <asp:TextBox runat="server" ID="txtBarcodeInput"></asp:TextBox>
            </div>
        </div>
        <script type="text/javascript">
            $(document).ready(function () {
                $("#txtBarcodeInput").on("keypress", function (e) {
                    if (e.keyCode == 13) {
                        debugger;
                        var inputTxt = $("#txtBarcodeInput").val();
                        var paramss = '{input:"' + inputTxt + '"}'
                        $.ajax({
                            url: '<%= ResolveUrl("~/workline.aspx/AddRow") %>',
                            type: "POST",
                            data: paramss,
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data.d == "OK") {
                                    debugger;
                                    $("#txtBarcodeInput").val('');
                                    window.location.href = window.location.origin + "/workline.aspx";

                                    //if (buttonpressed == "btnSave")
                                    //    window.location.href = window.location.origin + "/worksheetlist";
                                    //else if (buttonpressed == "btnSaveAndPrint")
                                    //    window.location.href = window.location.origin + "/worksheetprint.aspx?id=" + id;

                                }
                                else alert(data.d);
                            },
                            error: function (data) {
                                alert("an error occured! " + data.Message);
                            }
                        });
                    }
                });
            });

            debugger;
            //var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
            //$("#lblCurrentDateTime").text(dateTime);

            var myVar = setInterval(timer, 1000);

            function timer() {
                var d = new Date();
                $("#lblCurrentTime").text(d.toLocaleTimeString());
            }
        </script>
    </form>
</body>

</html>
