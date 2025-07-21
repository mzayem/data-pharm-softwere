<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="EditSubGroup.aspx.cs" Inherits="data_pharm_softwere.Pages.SubGroup.EditSubGroup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <div class="container my-5">
        <h2 class="mb-4">Edit Sub Group</h2>

        <asp:ValidationSummary ID="ValidationSummary1" runat="server"
                               CssClass="alert alert-danger"
                               HeaderText="Please fix the following errors:"
                               DisplayMode="BulletList"
                               ShowSummary="true"
                               ValidationGroup="SubGroupForm" />

        <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-semibold"></asp:Label>

        <asp:Panel ID="pnlForm" runat="server" CssClass="card shadow-sm p-4 rounded-4">
            <div class="row g-3">
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

                <div>
                    <label for="ddlVendor" class="form-label fw-semibold">Vendor</label>
                    <asp:DropDownList ID="ddlVendor" runat="server"
                                      CssClass="form-select rounded-pill"
                                      AutoPostBack="true"
                                      OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvVendor" runat="server"
                                                ControlToValidate="ddlVendor"
                                                InitialValue=""
                                                ErrorMessage="Vendor is required"
                                                CssClass="text-danger"
                                                Display="Dynamic"
                                                ValidationGroup="SubGroupForm" />
                </div>

                <div>
                    <label for="ddlGroup" class="form-label fw-semibold">Group</label>
                    <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-select rounded-pill" />
                    <asp:RequiredFieldValidator ID="rfvGroup" runat="server"
                                                ControlToValidate="ddlGroup"
                                                InitialValue=""
                                                ErrorMessage="Group is required"
                                                CssClass="text-danger"
                                                Display="Dynamic"
                                                ValidationGroup="SubGroupForm" />
                </div>
            </div>

            <div class="d-flex flex-wrap gap-2 justify-content-between mt-4">
                <asp:Button ID="btnUpdate" runat="server"
                            Text="Update Sub Group"
                            CssClass="btn btn-success px-5 py-2 rounded-pill"
                            OnClick="btnUpdate_Click"
                            ValidationGroup="SubGroupForm" />

                <asp:HyperLink runat="server"
                               NavigateUrl="/subgroup/create"
                               CssClass="btn btn-primary px-5 py-2 rounded-pill">+ Add New Sub Group</asp:HyperLink>

                <asp:HyperLink runat="server"
                               NavigateUrl="/subgroup"
                               Text="Back to List"
                               CssClass="btn btn-secondary px-5 py-2 rounded-pill" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
