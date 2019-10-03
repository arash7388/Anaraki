﻿<%@ Page Title="" Language="C#" MasterPageFile="~/AdminPanel.Master" AutoEventWireup="true" CodeBehind="PostList.aspx.cs" Inherits="AdminPanel.PostList" %>
<%@ Import Namespace="Common" %>
<%@ Import Namespace="MTO" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
     <div class="row">
        <div class="col-md-12">
            <h3>مطالب</h3>
        </div>
    </div>
    
                <div class="row">
                    <div class="col-md-10">
                        <asp:GridView runat="server" AutoGenerateColumns="False" Width="512px" ID="gridPosts" CssClass="table table-bordered table-striped"
                            OnRowCommand="gridPost_OnRowCommand" DataKeyNames="Id">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="شناسه">
                                    <ControlStyle BorderColor="#FFFF99" BorderStyle="Solid" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Code" HeaderText="کد">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Title" HeaderText="عنوان">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FriendlyName" HeaderText="نویسنده">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="تاریخ ایجاد">
                                    <ItemTemplate>
                                        <%# ((DateTime)Eval("InsertDateTime")).ToFaDateTime() %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="EditPost"
                                            Text="ویرایش" CommandArgument='<%# Eval("Id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="DeletePost"
                                            Text="حذف" CommandArgument='<%# Eval("Id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:Button runat="server" ID="btnAddPost" Text="جدید" CssClass="btn btn-black" OnClick="btnAddPost_Click" />
                    </div>
                </div>
</asp:Content>
