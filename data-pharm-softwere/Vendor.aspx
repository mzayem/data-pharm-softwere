<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Vendor.aspx.cs" Inherits="data_pharm_softwere.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="card shadow-sm">
            <div class="card-header bg-primary text-white">
                <h2 class="h5 mb-0">Add New Vendor</h2>
            </div>
            <div class="card-body">
                <asp:Label ID="lblMessage" runat="server" CssClass="text-success mb-3 d-block"></asp:Label>

                <div class="mb-3">
                    <label for="txtName" class="form-label">Vendor Name</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Placeholder="Enter vendor name" />
                </div>

                <div class="mb-3">
                    <label for="txtEmail" class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="Enter email address" />
                </div>

                <asp:Button ID="btnSave" runat="server" Text="Save Vendor"
                    CssClass="btn btn-primary" OnClick="btnSave_Click" />
            </div>
        </div>
    </div>
</asp:Content>
