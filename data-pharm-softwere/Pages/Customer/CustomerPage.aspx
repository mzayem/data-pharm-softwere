<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="CustomerPage.aspx.cs" Inherits="data_pharm_softwere.Pages.Customer.CustomerPage" %>
<%@ Register TagPrefix="uc" TagName="ImportInfo" Src="~/Components/Control/ImportInfo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <uc:ImportInfo ID="ImportInfoControl" runat="server" ImportContext="customer" />

    <div class="container mt-4">
        <h2 class="mb-3">Customer List</h2>
        <asp:Label ID="lblImportStatus" runat="server" CssClass="d-none" EnableViewState="false" />

        <div class="d-flex justify-content-between mb-3">
            <!-- Add New Batch -->
            <div>
                <asp:HyperLink 
                    ID="btnCreateProduct" 
                    runat="server" 
                    NavigateUrl="/customer/create"
                    CssClass="btn btn-primary mb-3">
                    + Add New Customer
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
                Placeholder="Search by Customer name, code, Ntn, " 
                AutoPostBack="true" 
                OnTextChanged="txtSearch_TextChanged" />
            <asp:Panel ID="pnlFilters" runat="server" CssClass="row g-3 mb-3">
                <!-- City Route Dropdown -->
                <div class="col-md-3">
                    <asp:DropDownList 
                        ID="ddlCityRoute" 
                        runat="server" 
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlCityRoute_SelectedIndexChanged" />
                </div>

                <!-- Town Dropdown -->
                <div class="col-md-3">
                    <asp:DropDownList 
                        ID="ddlTown" 
                        runat="server" 
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlTown_SelectedIndexChanged" />
                </div>

                <!-- CustomerType Dropdown -->
                <div class="col-md-3">
                    <asp:DropDownList 
                        ID="ddlCustomerType" 
                        runat="server" 
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlCustomerType_SelectedIndexChanged" />
                </div>
                <div class="col-md-3">
                    <asp:DropDownList 
                        ID="ddlStatus" 
                        runat="server" 
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">

                        <asp:ListItem Value="">-- Status --</asp:ListItem>
                        <asp:ListItem Text="Active" Value="Active" />
                        <asp:ListItem Text="Dead" Value="Dead" />
                    </asp:DropDownList>
                </div>
                

            </asp:Panel>


            <asp:GridView 
                ID="gvCustomers" 
                runat="server" 
                AutoGenerateColumns="False"
                CssClass="table table-bordered table-hover" 
                OnRowCommand="gvCustomers_RowCommand" 
                OnRowDataBound="gvCustomers_RowDataBound"
                EmptyDataText="No Customer Found" >

                <Columns>
                    <asp:BoundField DataField="CustomerID" HeaderText="Code" DataFormatString="{0:D4}" HtmlEncode="false" />
                    <asp:BoundField DataField="Name" HeaderText="Customer Name" />
                    <asp:BoundField DataField="Contact" HeaderText="Contact" />
                    <asp:BoundField DataField="CNIC" HeaderText="CNIC" />
                    <asp:BoundField DataField="NtnNo" HeaderText="NtnNo" />
                    <asp:BoundField DataField="CityRouteName" HeaderText="Route" />
                    <asp:BoundField DataField="TownName" HeaderText="Town" />
                    <asp:BoundField DataField="CustomerType" HeaderText="Customer Type" />

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
                                ID="hfCustomerId" 
                                runat="server" 
                                Value='<%# Eval("CustomerId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
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

</div>
</asp:Content>
 