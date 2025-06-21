<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="CreateBatch.aspx.cs" Inherits="data_pharm_softwere.Pages.Batch.CreateBatch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <div class="container mt-4">
        <h3>Create Batch</h3>

        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" />

        <asp:Panel ID="pnlForm" runat="server" CssClass="card p-4 shadow-sm rounded-4">
            <div class="row g-3">
                <div class="col-md-6">
                    <label class="form-label">Product</label>
                    <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-select rounded-pill" />
                </div>
                <div class="col-md-6">
                    <label class="form-label">Batch No</label>
                    <asp:TextBox ID="txtBatchNo" runat="server" CssClass="form-control rounded-pill" />
                </div>
                <div class="col-md-6">
                    <label class="form-label">MFG Date</label>
                    <asp:TextBox ID="txtMFGDate" runat="server" TextMode="Date" CssClass="form-control rounded-pill" />
                </div>
                <div class="col-md-6">
                    <label class="form-label">Expiry Date</label>
                    <asp:TextBox ID="txtExpiryDate" runat="server" TextMode="Date" CssClass="form-control rounded-pill" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">DP (Rs.)</label>
                    <asp:TextBox ID="txtDP" runat="server" CssClass="form-control rounded-pill" TextMode="Number" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">TP (Rs.)</label>
                    <asp:TextBox ID="txtTP" runat="server" CssClass="form-control rounded-pill" TextMode="Number" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">MRP (Rs.)</label>
                    <asp:TextBox ID="txtMRP" runat="server" CssClass="form-control rounded-pill" TextMode="Number" />
                </div>
                <div class="col-md-6">
                    <label class="form-label">Carton Quantity</label>
                    <asp:TextBox ID="txtCartonQty" runat="server" CssClass="form-control rounded-pill" TextMode="Number" />
                </div>
                <div class="col-md-6">
                    <label class="form-label">Carton Price</label>
                    <asp:TextBox ID="txtCartonPrice" runat="server" CssClass="form-control rounded-pill" TextMode="Number" />
                </div>
            </div>

            <div class="mt-4">
                <asp:Button ID="btnSave" runat="server" Text="Save Batch" CssClass="btn btn-primary px-5 py-2 rounded-pill" OnClick="btnSave_Click" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
