<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="CreateDivision.aspx.cs" Inherits="data_pharm_softwere.Pages.Division.CreateDivision" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <div class="container mt-5">
        <div class="card shadow-sm rounded-4 p-4 mx-auto" style="max-width: 900px;">
            <h3 class="mb-4 text-center fw-semibold">Add New Division</h3>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server"
                CssClass="alert alert-danger"
                HeaderText="Please fix the following errors:"
                DisplayMode="BulletList"
                ShowSummary="true"
                ValidationGroup="DivisionForm" />

            <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-semibold"></asp:Label>

            <div class="needs-validation" novalidate="true">
                <div class="row g-3">
                    <div>
                        <label for="txtName" class="form-label fw-semibold">Division Name</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ID="rfvName" runat="server"
                            ControlToValidate="txtName"
                            ErrorMessage="Division Name is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="DivisionForm" />
                    </div>

                    <div>
                        <label for="ddlVendor" class="form-label fw-semibold mt-3">Vendor</label>
                        <asp:DropDownList ID="ddlVendor" runat="server" CssClass="form-select rounded-pill">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvVendor" runat="server"
                            ControlToValidate="ddlVendor"
                            InitialValue=""
                            ErrorMessage="Vendor is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="DivisionForm" />
                    </div>
                </div>

                <div class="d-flex flex-wrap align-items-center justify-content-center gap-2 text-center mt-4">
                    <asp:Button ID="btnSave" runat="server"
                        Text="Save Division"
                        CssClass="btn btn-primary px-5 py-2 rounded-pill"
                        OnClick="btnSave_Click"
                        ValidationGroup="DivisionForm" />
                    
                    <asp:HyperLink NavigateUrl="/division" Text="Back to List"
                        CssClass="btn btn-secondary px-5 py-2 rounded-pill" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
