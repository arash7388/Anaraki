<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.Master" CodeBehind="WorkLine.aspx.cs" Inherits="MehranPack.WorkLine" ClientIDMode="Static"%>

<asp:Content runat="server" ContentPlaceHolderID="head">
    <title>صف کارهای در حال تولید</title>
    <meta content="Automation" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="main">
    <div class="row">
        <div class="col-md-10" style="padding-right: 75px;">
            <h3 style="margin-top: 35px;">صف کارهای در حال تولید &nbsp; 
            
                 <asp:Label runat="server" ID="lblCurrentDate"></asp:Label>
                 -
                 <asp:Label runat="server" ID="lblCurrentTime"></asp:Label>
                </h3>
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
                OnRowCommand="gridWorkLine_RowCommand" AllowPaging="True"  
                PageSize="10" OnPageIndexChanging="gridWorkLine_OnPageIndexChanging">
                <PagerStyle CssClass="gridPagerStyle" HorizontalAlign="Center" Wrap="False"/>

                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="شناسه">
                        <ControlStyle BorderColor="#FFFF99" BorderStyle="Solid" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField DataField="WorksheetId" HeaderText="شناسه برگه کار">
                        <ControlStyle BorderColor="#FFFF99" BorderStyle="Solid" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField DataField="OperatorName" HeaderText="اوپراتور">
                        <ControlStyle BorderColor="#FFFF99" BorderStyle="Solid" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <%--<asp:BoundField DataField="ProductCode" HeaderText="کد محصول">
                        <ControlStyle BorderColor="#FFFF99" BorderStyle="Solid" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField DataField="ProductName" HeaderText="نام محصول">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>--%>

                    <asp:BoundField DataField="ProcessName" HeaderText="فرآیند">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField DataField="PersianDateTime" HeaderText="زمان ایجاد">
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

            <asp:Button runat="server" ID="btnAdd" Text="جدید" CssClass="btn btn-black btn-standard" OnClick="btnAdd_Click" Visible="false"/>
            <asp:TextBox runat="server" ID="txtBarcodeInput" placeholder="بارکد را اسکن کنید ..." AutoCompleteType="None" autocomplete="off"></asp:TextBox>
        </div>
    </div>
    <script type="text/javascript">

        toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-top-center",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }


        $(document).ready(function () {
            if ($("#gridWorkLine") != undefined) {
                var gridWidth = $("#gridWorkLine").width();
                var gridOffset = $("#gridWorkLine").offset();

                if (gridWidth == null)
                    $("#txtBarcodeInput").width("95%")
                else
                    $("#txtBarcodeInput").width(gridWidth);

                if (gridOffset != undefined)
                  $("#txtBarcodeInput").offset({ left: gridOffset.left })
            };

            $("#txtBarcodeInput").on("keypress", function (e) {
                debugger;
                if (e.keyCode == 35) {
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
                                $("#txtBarcodeInput").val('');
                                toastr["success"]("ردیف با موفقیت اضافه شد")

                                setTimeout(function () { window.location.href = window.location.origin + "/workline.aspx"; }, 1200);
                            }
                            else {
                                toastr["error"](data.d);
                                $("#txtBarcodeInput").val('');
                            };
                        },
                        error: function (data) {
                            toastr["error"]("خطا در سیستم " + data.Message);
                        }
                    });
                }
            });

            var myVar = setInterval(timer, 1000);

            function timer() {
                
                var d = new Date();
                var time = d.toLocaleTimeString().replace(" ", " ").replace("AM", "").replace("PM", "")
                $("#lblCurrentTime").text(time.toFaDigit());
            }

            ConvertNumberToPersion();

            String.prototype.toEnDigit = function () {
                return this.replace(/[\u06F0-\u06F9]+/g, function (digit) {
                    var ret = '';
                    for (var i = 0, len = digit.length; i < len; i++) {
                        ret += String.fromCharCode(digit.charCodeAt(i) - 1728);
                    }

                    return ret;
                });
            };

            String.prototype.toFaDigit = function () {
                return this.replace(/\d+/g, function (digit) {
                    var ret = '';
                    for (var i = 0, len = digit.length; i < len; i++) {
                        ret += String.fromCharCode(digit.charCodeAt(i) + 1728);
                    }

                    return ret;
                });
            };

            function ConvertNumberToPersion() {
                persian = { 0: '۰', 1: '۱', 2: '۲', 3: '۳', 4: '۴', 5: '۵', 6: '۶', 7: '۷', 8: '۸', 9: '۹' };
                function traverse(el) {
                    if (el.nodeType == 3) {
                        var list = el.data.match(/[0-9]/g);
                        if (list != null && list.length != 0) {
                            for (var i = 0; i < list.length; i++)
                                el.data = el.data.replace(list[i], persian[list[i]]);
                        }
                    }
                    for (var i = 0; i < el.childNodes.length; i++) {
                        traverse(el.childNodes[i]);
                    }
                }
                traverse(document.body);
            }
        });

        
    </script>
</asp:Content>
