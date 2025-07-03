<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="VendorPage.aspx.cs" Inherits="data_pharm_softwere.Pages.Vendor.VendorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <div class="container mt-4">
        <h2 class="mb-3">Vendor List</h2>

        <asp:HyperLink 
            ID="btnCreateVendor" 
            runat="server" 
            NavigateUrl="/vendor/create"
            CssClass="btn btn-primary mb-3">
            + Add New Vendor
        </asp:HyperLink>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:TextBox 
                    ID="txtSearch" 
                    runat="server" 
                    CssClass="form-control mb-3"
                    Placeholder="Search by any keyword" 
                    AutoPostBack="true" 
                    OnTextChanged="txtSearch_TextChanged" />

                <asp:GridView 
                    ID="gvVendors" 
                    runat="server" 
                    AutoGenerateColumns="False"
                    CssClass="table table-bordered table-hover" 
                    OnRowCommand="gvVendors_RowCommand" 
                    OnRowDataBound="gvVendors_RowDataBound"
                    EmptyDataText="No Vendor Found">

                    <Columns>
                        <asp:BoundField DataField="VendorID" HeaderText="ID" />
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="City" HeaderText="City" />
                        <asp:BoundField DataField="CompanyCode" HeaderText="Company Code" />
                        <asp:BoundField DataField="GstNo" HeaderText="GST No" />
                        <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" DataFormatString="{0:yyyy-MM-dd}" />

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
                                    ID="hfVendorID" 
                                    runat="server" 
                                    Value='<%# Eval("VendorID") %>' />
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
