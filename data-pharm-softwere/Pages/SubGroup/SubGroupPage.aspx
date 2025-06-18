<%@ Page Title="Sub Group" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="SubGroupPage.aspx.cs" Inherits="data_pharm_softwere.Pages.SubGroup.SubGroupPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="container mt-4">
        <h2 class="mb-3">Sub Group List</h2>

        <asp:HyperLink 
            ID="btnCreateSubGroup" 
            runat="server" 
            NavigateUrl="/subgroup/create"
            CssClass="btn btn-primary mb-3">
            + Add New Sub Group
        </asp:HyperLink>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:TextBox 
                    ID="txtSearch" 
                    runat="server" 
                    CssClass="form-control mb-3"
                    Placeholder="Search subgroup by name, group or vendor" 
                    AutoPostBack="true" 
                    OnTextChanged="txtSearch_TextChanged" />

                <asp:GridView 
                    ID="gvSubGroups" 
                    runat="server" 
                    AutoGenerateColumns="False"
                    CssClass="table table-bordered table-hover" 
                    OnRowCommand="gvSubGroups_RowCommand" 
                    OnRowDataBound="gvSubGroups_RowDataBound">

                    <Columns>
                        <asp:BoundField DataField="SubGroupID" HeaderText="ID" />
                        <asp:BoundField DataField="Name" HeaderText="SubGroup Name" />
                        <asp:BoundField DataField="GroupName" HeaderText="Group" />
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
                                    ID="hfSubGroupID" 
                                    runat="server" 
                                    Value='<%# Eval("SubGroupID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
