<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ProductPage.aspx.cs" Inherits="data_pharm_softwere.Pages.Product.ProductPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="container mt-4">
        <h2 class="mb-3">Product List</h2>

        <asp:HyperLink 
            ID="btnCreateProduct" 
            runat="server" 
            NavigateUrl="/product/create"
            CssClass="btn btn-primary mb-3">
            + Add New Product
        </asp:HyperLink>
        
        <asp:Button 
            ID="btnExportExcel" 
            runat="server" 
            CssClass="btn btn-success mb-3 ms-2" 
            Text="Export to Excel" 
            OnClick="btnExportExcel_Click" />
        <asp:Button 
            ID="btnExportPdf" 
            runat="server" 
            CssClass="btn btn-danger mb-3 ms-2" 
            Text="Export to PDF" 
            OnClick="btnExportPdf_Click" />



        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:TextBox 
                    ID="txtSearch" 
                    runat="server" 
                    CssClass="form-control mb-3"
                    Placeholder="Search by product name, code, vendor, or group" 
                    AutoPostBack="true" 
                    OnTextChanged="txtSearch_TextChanged" />
                <asp:Panel ID="pnlFilters" runat="server" CssClass="row g-3 mb-3">
                    <!-- Vendor Dropdown -->
                    <div class="col-md-4">
                        <asp:DropDownList 
                            ID="ddlVendor" 
                            runat="server" 
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged" />
                    </div>

                    <!-- Group Dropdown -->
                    <div class="col-md-4">
                        <asp:DropDownList 
                            ID="ddlGroup" 
                            runat="server" 
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" />
                    </div>

                    <!-- SubGroup Dropdown -->
                    <div class="col-md-4">
                        <asp:DropDownList 
                            ID="ddlSubGroup" 
                            runat="server" 
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlSubGroup_SelectedIndexChanged" />
                    </div>
                </asp:Panel>


                <asp:GridView 
                    ID="gvProducts" 
                    runat="server" 
                    AutoGenerateColumns="False"
                    CssClass="table table-bordered table-hover" 
                    OnRowCommand="gvProducts_RowCommand" 
                    OnRowDataBound="gvProducts_RowDataBound"
                    EmptyDataText="No Product Found">

                    <Columns>
                        <asp:BoundField DataField="ProductID" HeaderText="ID" />
                        <asp:BoundField DataField="Name" HeaderText="Product Name" />
                        <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                        <asp:BoundField DataField="VendorName" HeaderText="Vendor" />
                        <asp:BoundField DataField="GroupName" HeaderText="Group" />
                        <asp:BoundField DataField="SubGroupName" HeaderText="SubGroup" />
                        <asp:BoundField DataField="PackingType" HeaderText="Packing Type" />
                        <asp:BoundField DataField="Type" HeaderText="Type" />
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
                                    ID="hfProductID" 
                                    runat="server" 
                                    Value='<%# Eval("ProductID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        

    </div>
</asp:Content>
