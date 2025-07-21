<%@ Page Title="Edit Group" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="EditGroup.aspx.cs" Inherits="data_pharm_softwere.Pages.Group.EditGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-5">
        <h2 class="mb-4">Edit Group</h2>

        <asp:ValidationSummary ID="ValidationSummary1" runat="server"
                               CssClass="alert alert-danger"
                               HeaderText="Please fix the following errors:"
                               DisplayMode="BulletList"
                               ShowSummary="true"
                               ValidationGroup="GroupForm" />

        <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-semibold"></asp:Label>

        <asp:Panel ID="pnlForm" runat="server" CssClass="card shadow-sm p-4 rounded-4">
            <div class="row g-3">
                <div>
                    <label for="txtName" class="form-label fw-semibold">Group Name</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control rounded-pill" />
                    <asp:RequiredFieldValidator ID="rfvName" runat="server"
                                                ControlToValidate="txtName"
                                                ErrorMessage="Group Name is required"
                                                CssClass="text-danger"
                                                Display="Dynamic"
                                                ValidationGroup="GroupForm" />
                </div>
                <div>
                    <label for="ddlDivision" class="form-label fw-semibold">Division</label>
                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-select rounded-pill" />
                    <asp:RequiredFieldValidator ID="rfvDivision" runat="server"
                                                ControlToValidate="ddlDivision"
                                                InitialValue=""
                                                ErrorMessage="Division is required"
                                                CssClass="text-danger"
                                                Display="Dynamic"
                                                ValidationGroup="GroupForm" />
                </div>

            </div>

            <div class="d-flex flex-wrap gap-2 justify-content-between mt-4">
                <asp:Button ID="btnUpdate" runat="server" Text="Update Group" CssClass="btn btn-primary px-5 py-2 rounded-pill" OnClick="btnUpdate_Click" ValidationGroup="GroupForm" />
                <asp:HyperLink  runat="server" NavigateUrl="/group/create"  CssClass="btn btn-primary px-5 py-2 rounded-pill">+ Add New Group</asp:HyperLink>
                <asp:HyperLink NavigateUrl="/group" Text="Back to List" CssClass="btn btn-secondary px-5 py-2 rounded-pill" runat="server" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
