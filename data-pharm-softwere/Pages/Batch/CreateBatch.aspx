﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="CreateBatch.aspx.cs" Inherits="data_pharm_softwere.Pages.Batch.CreateBatch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <div class="container my-4">
        <h3>Create Batch</h3>

        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" />

        <asp:Panel ID="pnlForm" runat="server" CssClass="card p-4 shadow-sm rounded-4">
            <asp:UpdatePanel ID="upAllControls" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row g-3">
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />

                        <!-- Filter Section -->
                        <div class="col-md-12 row g-3">
                            <div class="col-md-4">
                                <label class="form-label fw-semibold">Vendor</label>
                                <asp:DropDownList ID="ddlVendor" runat="server" CssClass="form-select rounded-pill"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged" />
                            </div>
                            <div class="col-md-4">
                                <label class="form-label fw-semibold">Group</label>
                                <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-select rounded-pill"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" />
                            </div>
                            <div class="col-md-4">
                                <label class="form-label fw-semibold">Sub Group</label>
                                <asp:DropDownList ID="ddlSubGroup" runat="server" CssClass="form-select rounded-pill"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlSubGroup_SelectedIndexChanged" />
                            </div>
                        </div>

                        <div class="col-md-6">
                            <label class="form-label fw-semibold">Product</label>
                            <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-select rounded-pill"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged" />
                            <asp:RequiredFieldValidator ID="rfvProduct" runat="server"
                                ControlToValidate="ddlProduct"
                                InitialValue=""
                                ErrorMessage="Product is required"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="BatchForm" />
                        </div>

                        <div class="col-md-6">
                            <label class="form-label fw-semibold">Batch No</label>
                            <asp:TextBox ID="txtBatchNo" runat="server" CssClass="form-control rounded-pill"
                                AutoPostBack="true" OnTextChanged="txtBatchNo_TextChanged" />
                        </div>

                        <div class="col-md-6">
                            <label class="form-label fw-semibold">MFG Date</label>
                            <asp:TextBox ID="txtMFGDate" runat="server" TextMode="Date" CssClass="form-control rounded-pill" />
                            <asp:RequiredFieldValidator ID="rfvMFGDate" runat="server"
                                ControlToValidate="txtMFGDate"
                                ErrorMessage="MFG Date is required"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="BatchForm" />
                        </div>

                        <div class="col-md-6">
                            <label class="form-label fw-semibold">Expiry Date</label>
                            <asp:TextBox ID="txtExpiryDate" runat="server" TextMode="Date" CssClass="form-control rounded-pill" />
                            <asp:RequiredFieldValidator ID="rfvExpiryDate" runat="server"
                                ControlToValidate="txtExpiryDate"
                                ErrorMessage="Expiry Date is required"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="BatchForm" />
                        </div>

                        <div class="col-md-4">
                            <label class="form-label fw-semibold">DP (Rs.)</label>
                            <asp:TextBox ID="txtDP" runat="server" CssClass="form-control rounded-pill" TextMode="Number"
                                AutoPostBack="true" OnTextChanged="txtDP_TextChanged" />
                            <asp:RequiredFieldValidator ID="rfvDP" runat="server"
                                ControlToValidate="txtDP"
                                ErrorMessage="DP is required"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="BatchForm" />
                        </div>

                        <div class="col-md-4">
                            <label class="form-label fw-semibold">TP (Rs.)</label>
                            <asp:TextBox ID="txtTP" runat="server" CssClass="form-control rounded-pill" TextMode="Number" />
                            <asp:RequiredFieldValidator ID="rfvTP" runat="server"
                                ControlToValidate="txtTP"
                                ErrorMessage="TP is required"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="BatchForm" />
                        </div>

                        <div class="col-md-4">
                            <label class="form-label fw-semibold">MRP (Rs.)</label>
                            <asp:TextBox ID="txtMRP" runat="server" CssClass="form-control rounded-pill" TextMode="Number" />
                            <asp:RequiredFieldValidator ID="rfvMRP" runat="server"
                                ControlToValidate="txtMRP"
                                ErrorMessage="MRP is required"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="BatchForm" />
                        </div>

                        <div class="col-md-6">
                            <label class="form-label fw-semibold">Carton Quantity</label>
                            <asp:TextBox ID="txtCartonQty" runat="server" CssClass="form-control rounded-pill" TextMode="Number" />
                            <asp:RequiredFieldValidator ID="rfvCartonQty" runat="server"
                                ControlToValidate="txtCartonQty"
                                ErrorMessage="Carton Quantity is required"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="BatchForm" />
                        </div>

                        <div class="col-md-6">
                            <label class="form-label fw-semibold">Carton Price</label>
                            <asp:TextBox ID="txtCartonPrice" runat="server" CssClass="form-control rounded-pill"
                                TextMode="Number" ReadOnly="true" />
                        </div>
                    </div>

                    <div class="mt-4">
                        <asp:Button ID="btnSave" runat="server"
                            Text="Save Batch"
                            CssClass="btn btn-primary px-5 py-2 rounded-pill"
                            ValidationGroup="BatchForm"
                            OnClick="btnSave_Click" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlVendor" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlGroup" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlSubGroup" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlProduct" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="txtDP" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="txtBatchNo" EventName="TextChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
</asp:Content>
