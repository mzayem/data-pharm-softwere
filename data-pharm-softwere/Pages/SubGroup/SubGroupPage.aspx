<%@ Page Title="Sub Group" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="SubGroupPage.aspx.cs" Inherits="data_pharm_softwere.Pages.SubGroup.SubGroupPage" %>
 <%@ Register TagPrefix="uc" TagName="ImportInfo" Src="~/Components/Control/ImportInfo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <uc:ImportInfo ID="ImportInfoControl" runat="server" ImportContext="subgroup" />

    <div class="container mt-4">
        <h2 class="mb-3">Sub Group List</h2>
        <asp:Label ID="lblImportStatus" runat="server" CssClass="d-none" EnableViewState="false" />

        <div class="d-flex justify-content-between mb-3">
            <!-- Add New Batch -->
                    <asp:HyperLink 
                        ID="btnCreateSubGroup" 
                        runat="server" 
                        NavigateUrl="/subgroup/create"
                        CssClass="btn btn-primary mb-3">
                        + Add New Sub Group
                    </asp:HyperLink>

            <div class="d-flex gap-2 align-items-start">
                <!-- Help Icon with Tooltip -->
                <button type="button" 
                        class="btn btn-link p-0 m-0 border-0 text-dark" 
                        data-bs-toggle="modal" 
                        data-bs-target="#ImportInfo">
                    <i class="bi bi-info-circle fs-4" data-bs-toggle="tooltip" title="Need Help!"></i>
                </button>

                <asp:FileUpload ID="fuCSV" runat="server" CssClass="d-none" onchange="submitImport()" />
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
                    Placeholder="Search subgroup by name, group or vendor" 
                    AutoPostBack="true" 
                    OnTextChanged="txtSearch_TextChanged" />

                <asp:GridView 
                    ID="gvSubGroups" 
                    runat="server" 
                    AutoGenerateColumns="False"
                    CssClass="table table-bordered table-hover" 
                    OnRowCommand="gvSubGroups_RowCommand" 
                    OnRowDataBound="gvSubGroups_RowDataBound"
                    EmptyDataText="No Sub Group Found">

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
