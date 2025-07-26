<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="VendorPage.aspx.cs" Inherits="data_pharm_softwere.Pages.Vendor.VendorPage" %>
<%@ Register TagPrefix="uc" TagName="ImportInfo" Src="~/Components/Control/ImportInfo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <uc:ImportInfo ID="ImportInfoControl" runat="server" ImportContext="vendor" />

    <div class="container my-4">
        <h2 class="mb-3">Vendor List</h2>
        <asp:Label ID="lblImportStatus" runat="server" CssClass="d-none" EnableViewState="false" />

        <div class="d-flex justify-content-between mb-3">
            <!-- Add New Batch -->
            <div>
                <asp:HyperLink 
                    ID="btnCreateVendor" 
                    runat="server" 
                    NavigateUrl="/vendor/create"
                    CssClass="btn btn-primary mb-3">
                    + Add New Vendor
                </asp:HyperLink>

                <asp:Button 
                    ID="btnExportExcel" 
                    runat="server"  
                    CssClass="btn btn-success mb-3 ms-2" 
                    Text="Export as Excel" 
                    OnClick="btnExportExcel_Click" />
                <asp:Button 
                    ID="btnExportPdf" 
                    runat="server" 
                    CssClass="btn btn-danger mb-3 ms-2"
                    Text="Export as PDF" 
                    OnClick="btnExportPdf_Click"/>
            </div>

            <div class="d-flex gap-2 align-items-start">
                <!-- Help Icon with Tooltip -->
                <button type="button" 
                        class="btn btn-link p-0 m-0 border-0 text-dark" 
                        data-bs-toggle="modal" 
                        data-bs-target="#ImportInfo">
                    <i class="bi bi-info-circle fs-4" data-bs-toggle="tooltip" title="Need Help!"></i>
                </button>

                <asp:FileUpload ID="fuCSV" runat="server" CssClass="d-none" onchange="submitImport()" accept=".csv"/>
                <asp:Button ID="btnImport" runat="server" Text="Import CSV" CssClass="btn btn-success" OnClientClick="triggerFileInput(); return false;" />
                <asp:Button ID="btnHiddenSubmit" runat="server" CssClass="d-none" OnClick="btnImport_Click" />

            </div>
        </div>

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
                        <asp:BoundField DataField="AccountId" HeaderText="ID" />
                        <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <%# Eval("Account.AccountName") %>
                        </ItemTemplate>
                    </asp:TemplateField>


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
                                    ID="hfAccountId" 
                                    runat="server" 
                                    Value='<%# Eval("AccountId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            </ContentTemplate>

            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportPdf" />
                <asp:PostBackTrigger ControlID="btnExportExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <script>
        function triggerFileInput() {
            document.getElementById('<%= fuCSV.ClientID %>').click();
        }

        function submitImport() {
            document.getElementById('<%= btnHiddenSubmit.ClientID %>').click();
         }

         // Enable Bootstrap Tooltip
         document.addEventListener('DOMContentLoaded', function () {
             var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
             tooltipTriggerList.forEach(function (tooltipTriggerEl) {
              new bootstrap.Tooltip(tooltipTriggerEl);
             });
         });
    </script>
</asp:Content>
