<%@ Page Title="Create Sales" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="CreateSales.aspx.cs" Inherits="data_pharm_softwere.Pages.Sales.CreateSales" %>
<%@ Register Src="~/Pages/Batch/Controls/BatchModal.ascx" TagName="BatchModal" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc:BatchModal ID="BatchModalControl" runat="server" />

     <div class="container my-5">
         <div class="card shadow-sm rounded-4 p-4">
             <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" />

             <asp:UpdatePanel ID="upPurchase" runat="server">
                 <ContentTemplate>
                     <asp:HiddenField ID="hiddenProductId" runat="server" />

                    <!-- Header -->
                    <div class="row align-items-center mb-4">
                        <div class="col-md-8">
                            <h3 class="fw-bold mb-0">Create Sales Invoice</h3>
                            <small class="text-muted">Manage transactions efficiently</small>
                        </div>
                        <div class="col-md-4 text-end">
                            <div class="d-flex align-items-center justify-content-between gap-3">
                                <asp:Label ID="lblACBal" runat="server"
                                    CssClass="badge bg-light text-dark rounded-pill fs-6 px-3 py-2 shadow-sm w-50 " />
                                <asp:LinkButton ID="btnACBal" runat="server"
                                    CssClass="btn btn-outline-primary btn-sm rounded-pill px-4 shadow-sm w-50"
                                    OnClick="Load_ACBalance">
                                    <i class="bi bi-cash-coin me-1"></i> Show Balance
                                </asp:LinkButton>

                                
                            </div>
                        </div>
                    </div>

                    <!-- Row 1 -->
                    <div class="row g-3 align-items-end d-flex justify-content-between align-content-between">
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                        
                        <div class="col-md-3">
                            <label class="form-label fw-semibold">Voucher #</label>
                            <asp:TextBox ID="txtVoucherNo" runat="server" CssClass="form-control rounded-pill bg-light text-secondary" ReadOnly="true" />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label fw-semibold">Sale Date</label>
                            <asp:TextBox ID="txtPurchaseDate" runat="server" TextMode="Date" CssClass="form-control rounded-pill bg-light text-secondary" ReadOnly="true"/>
                        </div>
                        <div class="col-md-3">
                            <label class="form-label fw-semibold">Advance Tax Type</label>
                            <asp:DropDownList ID="ddlAdvTaxType" runat="server"
                                AutoPostBack="true" 
                               OnSelectedIndexChanged="TotalValuesChanged"
                               CssClass="form-select rounded-pill">
                                <asp:ListItem Text="Net" Value="Net" />
                                <asp:ListItem Text="Gross" Value="Gross" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvAdvTaxType" runat="server"
                                ControlToValidate="ddlAdvTaxType"
                                InitialValue=""
                                ErrorMessage="Advance Tax Type is required"
                                CssClass="text-danger"
                                ValidationGroup="SalesForm"
                                Display="Dynamic" />
                        </div>

                        <div class="col-md-3">
                            <label class="form-label fw-semibold">Gst Tax Type</label>
                            <asp:DropDownList ID="ddlGstType" runat="server" 
                                AutoPostBack="true" 
                                OnSelectedIndexChanged="TotalValuesChanged"
                                CssClass="form-select rounded-pill">
                                <asp:ListItem Text="Net" Value="Net" />
                                <asp:ListItem Text="Gross" Value="Gross" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvGstType" runat="server"
                                    ControlToValidate="ddlGstType"
                                    InitialValue=""
                                    ErrorMessage="GST Tax Type is required"
                                    CssClass="text-danger"
                                    ValidationGroup="SalesForm"
                                    Display="Dynamic" />
                    </div>
                    </div>

                    <!-- Row 2 -->
                    <div class="row g-3 align-items-end mt-1 ">
                        <div class="col-md-6 ">
                            <label class="form-label fw-semibold">Customer</label>
                            <div class="input-group d-flex justify-content-between align-content-between gap-3">
                                 <asp:TextBox ID="txtCustomerCode" runat="server" 
                                     CssClass="form-control rounded-pill" 
                                     placeholder="Enter Code" 
                                     AutoPostBack="true" 
                                     OnTextChanged="txtCustomerCode_TextChanged" />

                                <asp:DropDownList ID="ddlCustomer" runat="server" 
                                  CssClass="form-select rounded-pill"
                                  AutoPostBack="true" 
                                  OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" />
                                 
                            </div>
                            <asp:RequiredFieldValidator ID="rfvCustomer" runat="server"
                               ControlToValidate="ddlCustomer"
                               ErrorMessage="Customer is required"
                               CssClass="text-danger"
                               ValidationGroup="SalesForm"
                               Display="Dynamic" />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label fw-semibold">Town</label>
                            <asp:TextBox ID="txtTown" runat="server" CssClass="form-control rounded-pill bg-light text-secondary" ReadOnly="true" />
                        </div>
                            <div class="col-md-3">
                                <label class="form-label fw-semibold">Bill Type</label>
                                <asp:DropDownList ID="ddlBillType" runat="server" 
                                    AutoPostBack="true"
                                    CssClass="form-select rounded-pill">
                                    <asp:ListItem Text="Normal" Value="Normal" />
                                    <asp:ListItem Text="Availability" Value="Availability" />
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvBillType" runat="server"
                                        ControlToValidate="ddlBillType"
                                        InitialValue=""
                                        ErrorMessage="Bill Type is required"
                                        CssClass="text-danger"
                                        ValidationGroup="SalesForm"
                                        Display="Dynamic" />
                        </div>
                        <div class="col-md-4">
                            <label class="form-label fw-semibold">Salesman</label>
                            <asp:DropDownList ID="ddlSalesmanBooker" runat="server" 
                              CssClass="form-select rounded-pill"
                              AutoPostBack="true" />
                        </div>
                         <div class="col-md-4">
                             <label class="form-label fw-semibold">Supplier</label>
                             <asp:DropDownList ID="ddlSalesmanSupplier" runat="server" 
                               CssClass="form-select rounded-pill"
                               AutoPostBack="true" />
                         </div>
                         <div class="col-md-4">
                             <label class="form-label fw-semibold">Driver</label>
                             <asp:DropDownList ID="ddlSalesmanDriver" runat="server" 
                               CssClass="form-select rounded-pill"
                               AutoPostBack="true" />
                         </div>
                    </div>
                    <hr class="my-4" />

                 </ContentTemplate>
             </asp:UpdatePanel>
         </div>
     </div>
</asp:Content>
