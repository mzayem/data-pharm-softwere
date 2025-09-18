<%@ Page Title="Discount Policy" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="DiscountPolicy.aspx.cs" Inherits="data_pharm_softwere.Pages.Sales.DiscountPolicy.DiscountPolicy" %>

<%@ Register TagPrefix="uc" TagName="ImportInfo" Src="~/Components/Control/ImportInfo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc:ImportInfo ID="ImportInfoControl" runat="server" ImportContext="discountpolicy" />
   
   <div class="container my-5">
   <div class="card shadow-sm rounded-4 p-4 mx-auto" style="max-width: 1200px;">
       <div class="d-flex justify-content-between align-content-between">
            <h3 class="mb-4 text-center fw-semibold">Discount Policy</h3>
            <div class="d-flex gap-2 align-items-start">
              <button type="button" 
                class="btn btn-link p-0 m-0 border-0 text-dark" 
                data-bs-toggle="modal" 
                data-bs-target="#ImportInfo">
                <i class="bi bi-info-circle fs-4" data-bs-toggle="tooltip" title="Need Help!"></i>
                </button>

               <asp:FileUpload ID="fuCSV" runat="server" CssClass="d-none" onchange="submitImport()" accept=".csv"/>
               <asp:Button ID="btnImport" runat="server" Text="Import CSV" CssClass="btn btn-success" OnClientClick="triggerFileInput(); return false;" />
               <asp:Button ID="btnHiddenSubmit" runat="server" CssClass="d-none" OnClick="btnImport_Click" />

            </div>

       </div>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" />

        <asp:UpdatePanel ID="upAllControls" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlForm" runat="server" >
                <div class="row g-3">
                    <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-semibold" EnableViewState="false" />
                        <div class="col-md-12 ">
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
                                <!-- readonly town textbox -->
                                 <asp:TextBox ID="txtTown" runat="server"
                                     CssClass="form-control rounded-pill bg-light text-secondary"
                                     ReadOnly="true"
                                     placeholder="Town" />

                                 
                            </div>
                           

                            <asp:RequiredFieldValidator ID="rfvCustomer" runat="server"
                               ControlToValidate="ddlCustomer"
                               InitialValue=""
                               ErrorMessage="Customer is required"
                               CssClass="text-danger"
                               ValidationGroup="DiscountForm"
                               Display="Dynamic" />
                        </div>

                     <!-- Input Row for adding new discount -->
                        <div class="row g-3 align-items-end mb-3">
                            <!-- Product Selection -->
                            <div class="col-md-4">
                                <label class="form-label fw-semibold">Product</label>
                                <div class="d-flex gap-2">
                                    <asp:TextBox ID="txtProductId" runat="server" CssClass="form-control rounded-pill"
                                        AutoPostBack="true" OnTextChanged="txtProductId_TextChanged"
                                        placeholder="Enter Product Id" />
                                    <asp:DropDownList ID="ddlProduct" runat="server"
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged"
                                        CssClass="form-select rounded-pill"></asp:DropDownList>
                                </div>
                            </div>

                            <!-- Flat Discount -->
                            <div class="col-md-2">
                                <label for="txtFlatDiscountAdd" class="form-label fw-semibold">Flat Discount</label>
                                <asp:TextBox ID="txtFlatDiscountAdd" runat="server" CssClass="form-control rounded-pill" TextMode="Number" />
                            </div>

                            <!-- Flat Expiry -->
                            <div class="col-md-2">
                                <label for="txtFlatExpiryDateAdd" class="form-label fw-semibold">Flat Expiry</label>
                                <asp:TextBox ID="txtFlatExpiryDateAdd" runat="server" CssClass="form-control rounded-pill" TextMode="Date" />
                            </div>

                            <!-- Credit Discount -->
                            <div class="col-md-2">
                                <label for="txtCreditDiscountAdd" class="form-label fw-semibold">Credit Discount</label>
                                <asp:TextBox ID="txtCreditDiscountAdd" runat="server" CssClass="form-control rounded-pill" TextMode="Number" />
                            </div>

                            <!-- Credit Expiry -->
                            <div class="col-md-2">
                                <label for="txtCreditExpiryDateAdd" class="form-label fw-semibold">Credit Expiry</label>
                                <asp:TextBox ID="txtCreditExpiryDateAdd" runat="server" CssClass="form-control rounded-pill" TextMode="Date" />
                            </div>

                            <!-- Add Button -->
                            <div class="col-md-2">
                                <asp:Button ID="btnAddDiscount" runat="server"
                                    CssClass="btn btn-success rounded-pill px-4"
                                    Text="+ Add Discount" OnClick="btnAddDiscount_Click" />
                            </div>
                        </div>


                    <!-- Repeater to list added discounts -->
                    <div class="mt-4 d-flex flex-column align-content-center">
                        <table class="table table-borderless text-center" style="min-height:200px;">
                            <thead class="table-light text-black">
                                <tr>
                                    <th style="width: 30%;">Product name</th>
                                    <th style="width: 15%;">Flat Discount</th>
                                    <th style="width: 15%;">Flat Expiry</th>
                                    <th style="width: 15%;">Credit Discount</th>
                                    <th style="width: 15%;">Credit Expiry</th>
                                    <th style="width: 10%;">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptBonuses" runat="server" 
                                    OnItemDataBound="rptBonuses_ItemDataBound" 
                                    OnItemCommand="rptBonuses_ItemCommand">

                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtProductName" runat="server" 
                                                    CssClass="form-control rounded-pill text-center" 
                                                    Text='<%# Eval("Product.Name") %>' 
                                                    ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFlatDiscount" runat="server" 
                                                    CssClass="form-control rounded-pill text-center" 
                                                    Text='<%# Eval("FlatDiscount") %>' 
                                                    AutoPostBack="true" 
                                                    OnTextChanged="BonusFieldChanged" 
                                                    EnableViewState="true"
                                                    CausesValidation="false"
                                                    TextMode="Number" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFlatExpiry" runat="server"
                                                    CssClass="form-control rounded-pill text-center"
                                                    Text='<%# Eval("FlatDiscountExpiry", "{0:yyyy-MM-dd}") %>'
                                                    TextMode="Date"
                                                    AutoPostBack="true"
                                                    OnTextChanged="BonusFieldChanged" 
                                                    EnableViewState="true"
                                                    CausesValidation="false" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCreditDiscount" runat="server" 
                                                    CssClass="form-control rounded-pill text-center" 
                                                    Text='<%# Eval("CreditDiscount") %>' 
                                                    OnTextChanged="BonusFieldChanged" 
                                                    AutoPostBack="true"
                                                    TextMode="Number" 
                                                    EnableViewState="true"
                                                    CausesValidation="false" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCreditExpiry" runat="server" 
                                                    CssClass="form-control rounded-pill text-center" 
                                                    Text='<%# Eval("CreditDiscountExpiry", "{0:yyyy-MM-dd}") %>' 
                                                    TextMode="Date"
                                                    AutoPostBack="true"
                                                    OnTextChanged="BonusFieldChanged" 
                                                    EnableViewState="true"
                                                    CausesValidation="false" />
                                            </td>
                                            <td class="text-center">
                                                <asp:LinkButton ID="btnRemove" runat="server" CommandName="Remove" 
                                                    CommandArgument='<%# Container.ItemIndex %>' 
                                                    CssClass="btn btn-sm btn-outline-danger rounded-pill"
                                                    CausesValidation="false">
                                                    <i class="bi bi-trash"></i>
                                                </asp:LinkButton>

                                            </td>
                                        </tr>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <asp:PlaceHolder ID="phEmpty" runat="server" Visible="false">
                                            <tr>
                                                <td colspan="6" class="text-muted py-5 text-center">
                                                    No items added yet.
                                                </td>
                                            </tr>
                                        </asp:PlaceHolder>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>

                    </div>

                <div class="mt-8 d-flex align-items-center justify-content-between">
                    <asp:Button ID="btnSave" runat="server"
                        Text="Save Discount"
                        CssClass="btn btn-primary px-5 py-2 rounded-pill"
                        ValidationGroup="DiscountForm"
                        OnClick="btnSave_Click" />
                      <asp:Button ID="btnDeleteAll" runat="server"
                        Text="Delete All Discount"
                        CssClass="btn btn-danger px-5 py-2 rounded-pill"
                        OnClientClick="return confirm('⚠ Are you sure you want to delete ALL discount policies for this customer?');"
                        OnClick="btnDeleteAll_Click" />

                </div>
            </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />                
                <asp:AsyncPostBackTrigger ControlID="rptBonuses" EventName="ItemCommand" />
                <asp:AsyncPostBackTrigger ControlID="btnAddDiscount" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
     </div>
   </div>
        <script>
                function triggerFileInput() {
                    document.getElementById('<%= fuCSV.ClientID %>').click();
        }

                function submitImport() {
                    document.getElementById('<%= btnHiddenSubmit.ClientID %>').click();
                        }

                // Enable Bootstrap Tooltip
                document.addEventListener('DOMContentLoaded', function () {
                    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
                    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
                        new bootstrap.Tooltip(tooltipTriggerEl);
                    });
                });
        </script>
</asp:Content>
