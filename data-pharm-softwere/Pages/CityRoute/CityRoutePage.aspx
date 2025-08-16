<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="CityRoutePage.aspx.cs" Inherits="data_pharm_softwere.Pages.CityRoute.CityRoutePage" %>
<%@ Register TagPrefix="uc" TagName="ImportInfo" Src="~/Components/Control/ImportInfo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc:ImportInfo ID="ImportInfoControl" runat="server" ImportContext="cityroute" />

    <div class="container my-4">
        <h2 class="mb-3">Route List</h2>
        <asp:Label ID="lblImportStatus" runat="server" CssClass="d-none" EnableViewState="false" />
     
        <div class="d-flex justify-content-between mb-3">
            <asp:HyperLink 
                ID="btnCreateBatch" 
                runat="server" 
                NavigateUrl="/city-route/create"
                CssClass="btn btn-primary mb-3">
                + Add New Route
            </asp:HyperLink>

            <div class="d-flex gap-2 align-items-start">
                <!-- Help Icon with Tooltip -->
                <button type="button" 
                        class="btn btn-link p-0 m-0 border-0 text-dark" 
                        data-bs-toggle="modal" 
                        data-bs-target="#ImportInfo">
                    <i class="bi bi-info-circle fs-4" data-bs-toggle="tooltip" title="Need Help!"></i>
                </button>


                <!-- Hidden FileUpload and Styled Import Button -->
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
                Placeholder="Search by product, vendor, or group" 
                AutoPostBack="true" 
                OnTextChanged="txtSearch_TextChanged" />

            <asp:GridView 
                ID="gvBatches" 
                runat="server" 
                AutoGenerateColumns="False"
                CssClass="table table-bordered table-hover" 
                OnRowCommand="gvBatches_RowCommand"
                OnRowDataBound="gvBatches_RowDataBound"
                EmptyDataText="No Route Found">

                <Columns>
                    <asp:BoundField DataField="CityRouteID" HeaderText="Route No" />
                    <asp:BoundField DataField="Name" HeaderText="Name" />
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
                                ID="hfCityRouteId" 
                                runat="server" 
                                Value='<%# Eval("CityRouteId") %>' />
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
