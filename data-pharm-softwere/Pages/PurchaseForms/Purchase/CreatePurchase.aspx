<%@ Page Title="Create Purchase" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="CreatePurchase.aspx.cs" Inherits="data_pharm_softwere.Pages.PurchaseForms.Purchase.CreatePurchase" %>
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
                    <div class="row mb-3">
                        <div class="col-12">
                            <h3 class="fw-semibold mb-2">Create Purchase</h3>
                        </div>
                    </div>

                    <!-- Row 1 -->
                    <div class="row g-3 align-items-end d-flex justify-content-between align-content-between">
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                        
                        <div class="col-md-3">
                            <label class="form-label fw-semibold">Invoice #</label>
                            <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control rounded-pill bg-light text-secondary" ReadOnly="true" />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label fw-semibold">Purchase Date</label>
                            <asp:TextBox ID="txtPurchaseDate" runat="server" TextMode="Date" CssClass="form-control rounded-pill" />
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
                                ValidationGroup="PurchaseForm"
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
                                    ValidationGroup="PurchaseForm"
                                    Display="Dynamic" />
                    </div>
                    </div>

                    <!-- Row 2 -->
                    <div class="row g-3 align-items-end mt-1 ">
                        <div class="col-md-5 ">
                            <label class="form-label fw-semibold">Vendor</label>
                            <div class="input-group d-flex justify-content-between align-content-between gap-3">
                                 <asp:TextBox ID="txtVendorCode" runat="server" 
                                     CssClass="form-control rounded-pill" 
                                     placeholder="Enter Code" 
                                     AutoPostBack="true" 
                                     OnTextChanged="txtVendorCode_TextChanged" />

                                <asp:DropDownList ID="ddlVendor" runat="server" 
                                  CssClass="form-select rounded-pill"
                                  AutoPostBack="true" 
                                  OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged" />
                                 
                            </div>
                            <asp:RequiredFieldValidator ID="rfvVendor" runat="server"
                               ControlToValidate="ddlVendor"
                               InitialValue=""
                               ErrorMessage="Vendor is required"
                               CssClass="text-danger"
                               ValidationGroup="PurchaseForm"
                               Display="Dynamic" />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label fw-semibold">PO Number</label>
                            <asp:TextBox ID="txtPoNumber" runat="server" placeholder="Enter Po#" CssClass="form-control rounded-pill" />
                            <asp:RequiredFieldValidator ID="rfvPoNumber" runat="server"
                                ControlToValidate="txtPoNumber"
                                ErrorMessage="PO Number is required"
                                CssClass="text-danger"
                                ValidationGroup="PurchaseForm"
                                Display="Dynamic" />
                        </div>
                        <div class="col-md-4">
                            <label class="form-label fw-semibold">Reference</label>
                            <asp:TextBox ID="txtReference" runat="server" placeholder="Enter Ref#" CssClass="form-control rounded-pill" />
                            <asp:RequiredFieldValidator ID="rfvReference" runat="server"
                                ControlToValidate="txtReference"
                                ErrorMessage="Reference is required"
                                CssClass="text-danger"
                                ValidationGroup="PurchaseForm"
                                Display="Dynamic" />
                        </div>
                    </div>

                    <hr class="my-4" />

                   <!-- Add Record Row -->
                    <div class="row g-3 mb-3 align-items-end">
                        <!-- Product Code -->
                        <div class="col-md-4">
                            <label class="form-label fw-semibold">Product Id</label>
                            <div class=" d-flex gap-2">
                            <asp:TextBox ID="txtProductId" runat="server" CssClass="form-control rounded-pill"
                                         AutoPostBack="true" OnTextChanged="txtProductId_TextChanged"
                                         placeholder="Enter Product Id" />
                            <asp:DropDownList ID="ddlProduct" 
                                        runat="server" 
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged" 
                                        CssClass="form-select rounded-pill"></asp:DropDownList>
                                </div>
                        </div>
                        <div class="col-md-1">
                            <label class="form-label fw-semibold">Qty</label>
                            <asp:TextBox ID="txtQty" runat="server" CssClass="form-control rounded-pill"
                                         placeholder="0"
                                         TextMode="Number"
                                />
                        </div>

                        <!-- Batch Dropdown -->
                        <div class="col-md-5 ">
                        <label class="form-label fw-semibold">Batches</label>
                            <div class=" d-flex gap-2">
                        <asp:TextBox ID="txtBatchNo" runat="server" CssClass="form-control rounded-pill" AutoPostBack="true"
                            placeholder="Enter Batch no"
                            OnTextChanged="txtBatchNo_TextChanged"></asp:TextBox>

                        <asp:DropDownList ID="ddlBatch" 
                            runat="server" 
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged" 
                            CssClass="form-select rounded-pill"></asp:DropDownList></div>

                        </div>
                        <!-- Add Button -->
                        <div class="col-md-2">
                            <asp:Button ID="btnAddBatch" runat="server" Text="Add" 
                                        CssClass="btn btn-outline-primary w-100 rounded-pill"
                                        OnClick="btnAddBatch_Click" />
                        </div>
                    </div>

                    <!-- Grid -->
                     <div style="min-height:250px; overflow-x:auto; white-space:nowrap;">
                        <asp:GridView ID="gvPurchaseDetails" runat="server"
                            AutoGenerateColumns="False"
                            CssClass="table table-bordered table-hover light-grid"
                            EmptyDataText="No Details Found"
                            DataKeyNames="BatchStockID"
                            OnRowDeleting="gvPurchaseDetails_RowDeleting"
                            OnRowDataBound="gvPurchaseDetails_RowDataBound">
                            <RowStyle CssClass="grid-row" />
                            <AlternatingRowStyle CssClass="grid-row-alt" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sr#">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="40px" />
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Code" DataField="ProductID" />
                                <asp:BoundField HeaderText="Product Name" DataField="ProductName" />
                                <asp:BoundField HeaderText="Batch#" DataField="BatchNo" />
                                <asp:BoundField HeaderText="Expiry" DataField="ExpiryDate" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:TemplateField HeaderText="Qty">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQtyList" runat="server"
                                            CssClass="form-control form-control-sm qty-box"
                                            style="width:70px;"
                                            Text='<%# Bind("CartonQty") %>'
                                            AutoPostBack="true"
                                            TextMode="Number"
                                            OnTextChanged="Qty_TextChanged" 
                                            />
                                    </ItemTemplate>
                                    <ItemStyle Width="90px" />
                                </asp:TemplateField>  
                                <asp:BoundField HeaderText="Rate" DataField="CartonPrice" DataFormatString="{0:N2}" />
                                <asp:BoundField HeaderText="Dist%" DataField="DiscountPercent" />
                                <asp:BoundField HeaderText="GST%" DataField="GSTPercent" />
                                 <asp:TemplateField HeaderText="Bonus Qty">
                                     <ItemTemplate>
                                         <asp:TextBox ID="txtBonusQty" runat="server"
                                             CssClass="form-control form-control-sm qty-box"
                                             style="width:70px;"
                                             Text='<%# Bind("BonusQty") %>'
                                             AutoPostBack="true"
                                             TextMode="Number"
                                             OnTextChanged="BonusQty_TextChanged" 
                                             />
                                     </ItemTemplate>
                                     <ItemStyle Width="90px" />
                                 </asp:TemplateField>
                                <asp:BoundField HeaderText="Gross Amount" DataField="GrossAmount" DataFormatString="{0:N2}" />
                                <asp:BoundField HeaderText="Net Amount" DataField="NetAmount" DataFormatString="{0:N2}" />
                                <asp:BoundField HeaderText="Dist Amnt" DataField="DiscountAmount" DataFormatString="{0:N2}" />
                                <asp:BoundField HeaderText="GST Amnt" DataField="GSTAmount" DataFormatString="{0:N2}" />

                                   <asp:CommandField ShowDeleteButton="True" />
                            </Columns>
                        </asp:GridView>
                    </div>


                    <!-- Footer Totals -->
                    <div class="mt-4 row g-3">
                        <div class="col-md-2">
                            <label class="form-label fw-semibold">Gross</label>
                            <asp:Label ID="lblGross" runat="server" CssClass="form-control rounded-pill bg-light" />
                        </div>
                        <div class="col-md-2">
                            <label class="form-label fw-semibold">Discount</label>
                            <asp:Label ID="lblDiscount" runat="server" CssClass="form-control rounded-pill bg-light" />
                        </div>
                         <div class="col-md-1">
                            <label class="form-label fw-semibold">AdvTax%</label>
                            <asp:TextBox ID="txtAdvTaxRate" runat="server"
                                AutoPostBack="true" 
                                OnTextChanged="TotalValuesChanged"
                                CssClass="form-control rounded-pill"
                                TextMode="Number" />
                        </div>
                        <div class="col-md-2">
                            <label class="form-label fw-semibold">Advance Tax</label>
                            <asp:Label ID="lblAdvTaxAmount" runat="server" CssClass="form-control rounded-pill bg-light" />
                        </div>
                        <div class="col-md-2">
                            <label class="form-label fw-semibold">Additional Charges</label>
                            <asp:TextBox ID="txtAdditionalCharges" runat="server"
                                AutoPostBack="true" 
                                OnTextChanged="TotalValuesChanged"
                                CssClass="form-control rounded-pill" TextMode="Number" />
                        </div>

                        <div class="col-md-3">
                            <label class="form-label fw-semibold">Net Amount</label>
                            <asp:Label ID="lblNetAmount" runat="server" CssClass="form-control rounded-pill bg-light fw-bold" />
                        </div>
                    </div>

                    <!-- Save Button -->
                    <div class="mt-4 d-flex justify-content-between">
                        <asp:Button ID="btnSave" runat="server" Text="Save Purchase" CssClass="btn btn-primary px-5 py-2 rounded-pill" OnClick="btnSave_Click" ValidationGroup="PurchaseForm"/>
                        <asp:HyperLink NavigateUrl="/purchase" Text="Back to List" CssClass="btn btn-secondary px-5 py-2 rounded-pill" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <script>
        function checkBatchExists() {
            var typedBatch = $("#txtBatchNo").val().trim();
            var exists = $("#batchList option").filter(function () {
                return $(this).val() == typedBatch;
            }).length > 0;

            if (!exists && typedBatch !== "") {
                $("#newBatchModal").modal("show"); // Bootstrap modal
            }
        }

    </script>
</asp:Content>
