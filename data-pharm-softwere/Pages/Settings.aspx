<%@ Page Title="Settings" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="data_pharm_softwere.Pages.Settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-4">
    <asp:Panel ID="pnlSettings" runat="server" CssClass="card p-4 shadow-sm rounded-4">
        <asp:UpdatePanel ID="upSettings" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <h3 class="mb-4">System Settings</h3>

                <div class="col-md-12 mb-4">
                    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="d-block" />
                </div>

                <div class="row g-3">
                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Company Name</label>
                        <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server"
                            ControlToValidate="txtCompanyName"
                            ErrorMessage="Company Name Account is required"
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Default Currency</label>
                        <asp:TextBox ID="txtCurrency" runat="server" CssClass="form-control rounded-pill" />
                    </div>

                    <div class="col-md-12">
                        <label class="form-label fw-semibold">Address</label>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control rounded-pill" TextMode="MultiLine" Rows="2" />
                    </div>

                    <!-- Stock In Hand -->
                    <div class="col-md-12">
                        <label class="form-label fw-semibold">Stock In Hand Account</label>
                        <div class="input-group d-flex gap-2">
                            <asp:TextBox ID="txtStockInHand" runat="server" 
                                         CssClass="form-control rounded-pill" 
                                         placeholder="Enter Account No" 
                                         AutoPostBack="true" 
                                         OnTextChanged="txtStockInHand_TextChanged" />
                        
                            <asp:DropDownList ID="ddlStockAccounts" runat="server" CssClass="form-select rounded-pill"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlStockAccounts_SelectedIndexChanged" />
                        </div>
                        <asp:RequiredFieldValidator ID="rfvStockInHand" runat="server"
                            ControlToValidate="txtStockInHand"
                            ErrorMessage="Stock In Hand Account is required"
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>
                    
                    <div class="col-md-2">
                        <label class="form-label fw-semibold">Purchase Head</label>
                        <asp:TextBox ID="txtPurchaseHead" runat="server" CssClass="form-control rounded-pill" placeholder ="P" />
                    </div>
                    <div class="col-md-2">
                        <label class="form-label fw-semibold">PurchaseReturn Head</label>
                        <asp:TextBox ID="txtPurchaseReturnHead" runat="server" CssClass="form-control rounded-pill" placeholder="PR" />
                    </div>
                    <div class="col-md-2">
                        <label class="form-label fw-semibold">TransferIn Head</label>
                        <asp:TextBox ID="txtTransferInHead" runat="server" CssClass="form-control rounded-pill" placeholder="TI" />
                    </div>
                    <div class="col-md-2">
                        <label class="form-label fw-semibold">TransferOut Head</label>
                        <asp:TextBox ID="txtTransferOutHead" runat="server" CssClass="form-control rounded-pill" placeholder="TO" />
                    </div>
                    <div class="col-md-2">
                        <label class="form-label fw-semibold">Sales Head</label>
                        <asp:TextBox ID="txtSalesHead" runat="server" CssClass="form-control rounded-pill" placeholder="S" />
                    </div>
                    <div class="col-md-2">
                        <label class="form-label fw-semibold">SalesReturn Head</label>
                        <asp:TextBox ID="txtSalesReturnHead" runat="server" CssClass="form-control rounded-pill" placeholder="SR" />
                    </div>
                </div>

                <div class="d-flex justify-content-end mt-4">
                    <asp:Button ID="btnSave" runat="server" 
                                Text="Update Settings" 
                                CssClass="btn btn-primary px-5 py-2 rounded-pill"
                                OnClick="btnSave_Click" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtStockInHand" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlStockAccounts" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
</div>
</asp:Content>
