<%@ Page Title="Group" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="GroupPage.aspx.cs" Inherits="data_pharm_softwere.Pages.Group.GroupPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <div class="container mt-4">
        <h2 class="mb-3">Group List</h2>

        <asp:HyperLink 
            ID="btnCreateGroup" 
            runat="server" 
            NavigateUrl="/group/create"
            CssClass="btn btn-primary mb-3">
            + Add New Group
        </asp:HyperLink>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:TextBox 
                    ID="txtSearch" 
                    runat="server" 
                    CssClass="form-control mb-3"
                    Placeholder="Search group by name or vendor" 
                    AutoPostBack="true" 
                    OnTextChanged="txtSearch_TextChanged" />

                <asp:GridView 
                    ID="gvGroups" 
                    runat="server" 
                    AutoGenerateColumns="False"
                    CssClass="table table-bordered table-hover" 
                    OnRowCommand="gvGroups_RowCommand" 
                    OnRowDataBound="gvGroups_RowDataBound">

                    <Columns>
                        <asp:BoundField DataField="GroupID" HeaderText="ID" />
                        <asp:BoundField DataField="Name" HeaderText="Group Name" />
                        <asp:BoundField DataField="VendorName" HeaderText="Vendor" />
                        <asp:BoundField DataField="CreatedAt" HeaderText="Created At" DataFormatString="{0:yyyy-MM-dd}" />

                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:DropDownList 
                                    ID="ddlActions" 
                                    runat="server"
                                    CssClass="form-select form-select-sm border-0 bg-transparent custom-dropdown"
                                    AutoPostBack="true" 
                                    OnSelectedIndexChanged="ddlActions_SelectedIndexChanged">
                                    
                                    <asp:ListItem Value="">• • •</asp:ListItem>
                                    <asp:ListItem Text="Edit" Value="Edit" />
                                    <asp:ListItem Text="Delete" Value="Delete" />
                                </asp:DropDownList>

                                <asp:HiddenField 
                                    ID="hfGroupID" 
                                    runat="server" 
                                    Value='<%# Eval("GroupID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:Button 
                    ID="btnExportPdf" 
                    runat="server" 
                    CssClass="btn btn-dark mt-3"
                    Text="Export as PDF" 
                    OnClick="btnExportPdf_Click" />

                <asp:Button 
                    ID="btnExportExcel" 
                    runat="server" 
                    CssClass="btn btn-success mt-3 ms-2"
                    Text="Export as Excel" 
                    OnClick="btnExportExcel_Click" />
            </ContentTemplate>

            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportPdf" />
                <asp:PostBackTrigger ControlID="btnExportExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
