<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="SalesmanPage.aspx.cs" Inherits="data_pharm_softwere.Pages.Salesman.SalesmanPage" %>
<%@ Register TagPrefix="uc" TagName="ImportInfo" Src="~/Components/Control/ImportInfo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <uc:ImportInfo ID="ImportInfoControl" runat="server" ImportContext="town" />

    <div class="container my-4">
        <h2 class="mb-3">Salesman List</h2>
        <asp:Label ID="lblImportStatus" runat="server" CssClass="d-none" EnableViewState="false" />

        <!-- Header actions -->
        <div class="d-flex justify-content-between align-items-center mb-3">
            <div>
                <asp:HyperLink 
                    ID="btnCreateSalesman" 
                    runat="server" 
                    NavigateUrl="/salesman/create"
                    CssClass="btn btn-primary">
                    + Add New Salesman
                </asp:HyperLink>
                <asp:Button 
                    ID="btnExportExcel" 
                    runat="server" 
                    CssClass="btn btn-success ms-2" 
                    Text="Export to Excel" 
                    OnClick="btnExportExcel_Click" />
                <asp:Button 
                    ID="btnExportPdf" 
                    runat="server" 
                    CssClass="btn btn-danger ms-2" 
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

        <!-- Filters and grid -->
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:TextBox 
                    ID="txtSearch" 
                    runat="server" 
                    CssClass="form-control mb-3"
                    Placeholder="Search by name, code, NTN..." 
                    AutoPostBack="true" 
                    OnTextChanged="txtSearch_TextChanged" />

                <div class="row mb-3">
                    <div class="col-md-3">
                        <asp:DropDownList 
                            ID="ddlTown" 
                            runat="server" 
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlTown_SelectedIndexChanged" />
                    </div>
                </div>

                <asp:GridView 
                    ID="gvSalesmen" 
                    runat="server" 
                    AutoGenerateColumns="False"
                    CssClass="table table-bordered table-hover"
                    EmptyDataText="No Salesman Found"
                    OnRowCommand="gvSalesmen_RowCommand"
                    OnRowDataBound="gvSalesmen_RowDataBound">

                    <Columns>
                        <asp:BoundField DataField="SalesmanID" HeaderText="Code" DataFormatString="{0:D4}" HtmlEncode="false" />
                        <asp:BoundField DataField="Name" HeaderText="Salesman Name" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="Contact" HeaderText="Contact" />

   
                        <asp:TemplateField HeaderText="Towns">
                            <ItemTemplate>
                                <%# GetLimitedTownNames((int)Eval("SalesmanID")) %>
                                <span class="text-muted"><%# GetTownOverflowText((int)Eval("SalesmanID")) %></span>
                            </ItemTemplate>
                        </asp:TemplateField>


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
                                    ID="hfSalesmanId" 
                                    runat="server" 
                                    Value='<%# Eval("SalesmanID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
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
