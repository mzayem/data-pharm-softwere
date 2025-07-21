<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="CreateSubGroup.aspx.cs" Inherits="data_pharm_softwere.Pages.SubGroup.CreateSubGroup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <div class="container my-5">
        <div class="card shadow-sm rounded-4 p-4 mx-auto" style="max-width: 900px;">
            <h3 class="mb-4 text-center fw-semibold">Add New Sub Group</h3>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server"
                CssClass="alert alert-danger"
                HeaderText="Please fix the following errors:"
                DisplayMode="BulletList"
                ShowSummary="true"
                ValidationGroup="SubGroupForm" />

            <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-semibold"></asp:Label>

            <div class="needs-validation" novalidate="true">
                 <div>
                        <label for="txtName" class="form-label fw-semibold">Sub Group Name</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ID="rfvName" runat="server"
                            ControlToValidate="txtName"
                            ErrorMessage="Sub Group Name is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="SubGroupForm" />
                    </div>

                <div class="row g-3 mt-1">
                    <div class="col-md-6">
                        <div>
                            <label for="ddlVendor" class="form-label fw-semibold">Vendor</label>
                            <asp:DropDownList ID="ddlVendor" runat="server"
                                CssClass="form-select rounded-pill"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    
                    <div class="col-md-6">
                    <div>
                        <label for="ddlGroup" class="form-label fw-semibold">Group</label>
                        <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-select rounded-pill">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvGroup" runat="server"
                            ControlToValidate="ddlGroup"
                            InitialValue=""
                            ErrorMessage="Group is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="SubGroupForm" />
                    </div>
                    </div>
                </div>

                <div class="d-flex flex-wrap align-items-center justify-content-center gap-2 text-center mt-4">
                    <asp:Button ID="btnSave" runat="server"
                        Text="Save Sub Group"
                        CssClass="btn btn-primary px-5 py-2 rounded-pill"
                        OnClick="btnSave_Click"
                        ValidationGroup="SubGroupForm" />

                    <asp:HyperLink NavigateUrl="/subgroup" Text="Back to List"
                        CssClass="btn btn-secondary px-5 py-2 rounded-pill" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
