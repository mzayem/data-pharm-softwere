<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BatchModal.ascx.cs" Inherits="data_pharm_softwere.Pages.Batch.Controls.BatchModal" %>

<div class="modal fade" id="batchModal" tabindex="-1" aria-labelledby="batchModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Create Batch</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <asp:Panel ID="pnlForm" runat="server" CssClass="row g-3">
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
                </asp:Panel>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnSave" runat="server" Text="Save Batch" CssClass="btn btn-primary" OnClick="btnSave_Click" />
            </div>
        </div>
    </div>
</div>
