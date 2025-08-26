<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="CreateProduct.aspx.cs" Inherits="data_pharm_softwere.Pages.Product.CreateProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <div class="container my-5">
    <div class="card shadow-sm rounded-4 p-4 mx-auto" style="max-width: 1200px;">
        <h3 class="mb-4 text-center fw-semibold">Add New Product</h3>

        <asp:ValidationSummary ID="ValidationSummary1" runat="server"
            CssClass="alert alert-danger"
            HeaderText="Please fix the following errors:"
            DisplayMode="BulletList"
            ShowSummary="true"
            ValidationGroup="ProductForm" />

        <asp:UpdatePanel ID="upProduct" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false" >
            <ContentTemplate>
                <div class="row g-3">
                    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                    <!-- Product Code (readonly) -->
                    <div class="col-md-4">
                        <label class="form-label fw-semibold">Code</label>
                        <asp:TextBox ID="lblProductId" runat="server"
                            CssClass="form-control rounded-pill bg-light text-secondary"
                            ReadOnly="true" />
                    </div>

                    <!-- Packing Type -->
                    <div class="col-md-4">
                        <label class="form-label fw-semibold">Packing Type</label>
                        <asp:DropDownList ID="ddlPackingType" runat="server" CssClass="form-select rounded-pill">
                            <asp:ListItem Text="Select" Value="" />
                            <asp:ListItem Text="Tablet" Value="Tablet" />
                            <asp:ListItem Text="Capsule" Value="Capsule" />
                            <asp:ListItem Text="Syrup" Value="Syrup" />
                            <asp:ListItem Text="Injection" Value="Injection" />
                            <asp:ListItem Text="Cream" Value="Cream" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvPackingType" runat="server"
                            ControlToValidate="ddlPackingType"
                            InitialValue=""
                            ErrorMessage="Packing Type is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                    </div>

                    <!-- Product Type -->
                    <div class="col-md-4">
                        <label class="form-label fw-semibold">Product Type</label>
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select rounded-pill">
                            <asp:ListItem Text="Select" Value="" />
                            <asp:ListItem Text="Medicine" Value="Medicine" />
                            <asp:ListItem Text="Neutra" Value="Neutra" />
                            <asp:ListItem Text="NonWare" Value="NonWare" />
                            <asp:ListItem Text="Narcotics" Value="Narcotics" />
                            <asp:ListItem Text="Cosmetic" Value="Cosmetic" />
                            <asp:ListItem Text="Consumer" Value="Consumer" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvType" runat="server"
                            ControlToValidate="ddlType"
                            InitialValue=""
                            ErrorMessage="Product Type is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                    </div>

                    <!-- Product Name -->
                    <div class="col-md-12">
                        <label class="form-label fw-semibold">Product Name</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ID="rfvName" runat="server"
                            ControlToValidate="txtName"
                            ErrorMessage="Product Name is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                    </div>

                    <!-- Product Code -->
                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Product Code</label>
                        <asp:TextBox ID="txtProductCode" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ID="rfvProductCode" runat="server"
                            ControlToValidate="txtProductCode"
                            ErrorMessage="Product Code is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                        <asp:RegularExpressionValidator ID="revProductCode" runat="server"
                            ControlToValidate="txtProductCode"
                            ValidationExpression="^\d{1,50}$"
                            ErrorMessage="Enter a valid Product Code (up to 50 digits)"
                            CssClass="text-danger"
                            ValidationGroup="ProductForm" />
                    </div>

                    <!-- HS Code -->
                    <div class="col-md-6">
                        <label class="form-label fw-semibold">HS Code</label>
                        <asp:TextBox ID="txtHSCode" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ID="rfvHSCode" runat="server"
                            ControlToValidate="txtHSCode"
                            ErrorMessage="HS Code is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                    </div>

                    <!-- Packing Size -->  
                    <div class="col-md-4">
                        <label class="form-label fw-semibold">Packing Size</label>
                        <asp:TextBox ID="txtPackingSize" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ID="rfvPackingSize" runat="server"
                            ControlToValidate="txtPackingSize"
                            ErrorMessage="Packing Size is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                    </div>

                    <!-- Carton Size -->
                    <div class="col-md-4">
                        <label class="form-label fw-semibold">Carton Size(Units)</label>
                        <asp:TextBox ID="txtCartonSize" runat="server" CssClass="form-control rounded-pill" TextMode="Number"/>
                        <asp:RequiredFieldValidator ID="rfvCartonSize" runat="server"
                            ControlToValidate="txtCartonSize"
                            ErrorMessage="Carton Size is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                    </div>

                    <!-- UOM -->
                    <div class="col-md-4">
                        <label class="form-label fw-semibold">UOM</label>
                        <asp:TextBox ID="txtUom" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ID="rfvUom" runat="server"
                            ControlToValidate="txtUom"
                            InitialValue=""
                            ErrorMessage="UOM is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                    </div>

                    <!-- Discount -->
                    <div class="col-md-4">
                        <label class="form-label fw-semibold">Purchase Discount (%)</label>
                        <asp:TextBox ID="txtPurchaseDiscount" runat="server"
                            CssClass="form-control rounded-pill"
                            TextMode="Number" />
                        <asp:RequiredFieldValidator ID="rfvPurchaseDiscount" runat="server"
                            ControlToValidate="txtPurchaseDiscount"
                            ErrorMessage="Discount is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                        <asp:RegularExpressionValidator ID="revPurchaseDiscount" runat="server"
                            ControlToValidate="txtPurchaseDiscount"
                            ValidationExpression="^(100(\.0{1,2})?|[0-9]{1,2}(\.[0-9]{1,2})?)$"
                            ErrorMessage="Enter valid discount (0–100%)"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                    </div>

                    <!-- Required GST -->
                    <div class="col-md-4">
                        <label class="form-label fw-semibold">Required GST (%)</label>
                        <asp:TextBox ID="txtReqGST" runat="server"
                            CssClass="form-control rounded-pill"
                            TextMode="Number" />
                        <asp:RequiredFieldValidator ID="rfvReqGST" runat="server"
                            ControlToValidate="txtReqGST"
                            ErrorMessage="Required GST is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                        <asp:RegularExpressionValidator ID="revReqGST" runat="server"
                            ControlToValidate="txtReqGST"
                            ValidationExpression="^(100(\.0{1,2})?|[0-9]{1,2}(\.[0-9]{1,2})?)$"
                            ErrorMessage="Enter valid GST (0–100%)"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                    </div>

                    <!-- Unrequired GST -->
                    <div class="col-md-4">
                        <label class="form-label fw-semibold">Unrequired GST (%)</label>
                        <asp:TextBox ID="txtUnReqGST" runat="server"
                            CssClass="form-control rounded-pill"
                            TextMode="Number" />
                        <asp:RequiredFieldValidator ID="rfvUnReqGST" runat="server"
                            ControlToValidate="txtUnReqGST"
                            ErrorMessage="Unrequired GST is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                        <asp:RegularExpressionValidator ID="revUnReqGST" runat="server"
                            ControlToValidate="txtUnReqGST"
                            ValidationExpression="^(100(\.0{1,2})?|[0-9]{1,2}(\.[0-9]{1,2})?)$"
                            ErrorMessage="Enter valid GST (0–100%)"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                    </div>

                    <!-- Vendor, Group, SubGroup -->
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
                            <asp:DropDownList ID="ddlSubGroup" runat="server" CssClass="form-select rounded-pill" />
                            <asp:RequiredFieldValidator ID="rfvSubGroup" runat="server"
                                ControlToValidate="ddlSubGroup"
                                InitialValue=""
                                ErrorMessage="Sub Group is required"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="ProductForm" />
                        </div>
                    </div>

                    <!-- Division -->
                    <div class="col-md-8">
                        <label class="form-label fw-semibold">Division</label>
                        <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-select rounded-pill" />
                        <asp:RequiredFieldValidator ID="rfvDivision" runat="server"
                            ControlToValidate="ddlDivision"
                            InitialValue=""
                            ErrorMessage="Division is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                    </div>

                    <!-- Checkboxes -->
                    <div class="col-md-6 mt-2">
                        <div class="form-check">
                            <asp:CheckBox ID="chkAdvTaxExempted" runat="server" CssClass="form-check-input border-0" />
                            <label class="form-check-label ms-2">Advance Tax Exempted</label>
                        </div>
                    </div>
                    <div class="col-md-6 mt-2">
                        <div class="form-check">
                            <asp:CheckBox ID="chkGSTExempted" runat="server" CssClass="form-check-input border-0" />
                            <label class="form-check-label ms-2">GST Exempted</label>
                        </div>
                    </div>
                </div>
              </ContentTemplate>

            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlVendor" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlGroup" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
        <hr class="my-4" />
        <h5 class="fw-semibold mb-4">Product Bonuses and Discounts</h5>

        <asp:UpdatePanel ID="upBonuses" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblBonuses" runat="server" EnableViewState="false" />
                <div class="row g-3">
                    <!--Bonus Type -->
                     <div class="col-md-3">
                        <label class="form-label fw-semibold">Bonus Type </label>
                        <asp:DropDownList ID="ddlBonusType" runat="server" CssClass="form-select rounded-pill">
                            <asp:ListItem Text="Select" Value="" />
                            <asp:ListItem Text="No Bonus" Value="NoBonus" />
                            <asp:ListItem Text="Lock" Value="Lock" />
                            <asp:ListItem Text="Unlocked" Value="Unlocked" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvBonusType" runat="server"
                            ControlToValidate="ddlBonusType"
                            InitialValue=""
                            ErrorMessage="Bnous type is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                    </div>
                     <div class="col-md-3">
                        <label class="form-label fw-semibold">is Dicounted?</label>
                        <asp:DropDownList ID="ddlIsDiscounted" runat="server" CssClass="form-select rounded-pill">
                            <asp:ListItem Text="No" Value="false" />
                            <asp:ListItem Text="Yes" Value="true" />
                        </asp:DropDownList>
                    </div>
                    <!--Discount1 -->
                    <div class="col-md-3">
                        <label class="form-label fw-semibold"> Discount1 (%)</label>
                        <asp:TextBox ID="txtDist1" runat="server"
                            CssClass="form-control rounded-pill"
                            TextMode="Number" />
                        <asp:RequiredFieldValidator ID="rfvDist1" runat="server"
                            ControlToValidate="txtPurchaseDiscount"
                            ErrorMessage="Discount1 is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                        <asp:RegularExpressionValidator ID="revDist1" runat="server"
                            ControlToValidate="txtPurchaseDiscount"
                            ValidationExpression="^(100(\.0{1,2})?|[0-9]{1,2}(\.[0-9]{1,2})?)$"
                            ErrorMessage="Enter valid discount (0–100%)"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                    </div>
                    <!--Discount2 -->
                    <div class="col-md-3">
                        <label class="form-label fw-semibold"> Discount2 (%)</label>
                        <asp:TextBox ID="txtDist2" runat="server"
                            CssClass="form-control rounded-pill"
                            TextMode="Number" />
                        <asp:RequiredFieldValidator ID="rfvDist2" runat="server"
                            ControlToValidate="txtPurchaseDiscount"
                            ErrorMessage="Discount2 is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                        <asp:RegularExpressionValidator ID="revDist2" runat="server"
                            ControlToValidate="txtPurchaseDiscount"
                            ValidationExpression="^(100(\.0{1,2})?|[0-9]{1,2}(\.[0-9]{1,2})?)$"
                            ErrorMessage="Enter valid discount (0–100%)"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="ProductForm" />
                    </div>
                </div>
                <hr class="my-4" />
                <!-- Input Row for adding new bonus -->
                <div class="row g-3 align-items-end">
                    <div class="col-md-3">
                        <label class="form-label">Min Qty</label>
                        <asp:TextBox ID="txtBonusMinQty" runat="server" CssClass="form-control rounded-pill" TextMode="Number" />
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Bonus Items</label>
                        <asp:TextBox ID="txtBonusItems" runat="server" CssClass="form-control rounded-pill" TextMode="Number" />
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Status</label>
                        <asp:DropDownList ID="ddlBonusStatus" runat="server" CssClass="form-select rounded-pill">
                            <asp:ListItem Text="Active" Value="true" Selected="True" />
                            <asp:ListItem Text="Inactive" Value="false" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="btnAddBonus" runat="server" CssClass="btn btn-success rounded-pill px-4"
                            Text="+ Add Bonus" OnClick="btnAddBonus_Click" />
                    </div>
                </div>

                <!-- Repeater to list added bonuses -->
                 <div class="mt-4 d-flex flex-column align-content-center align-items-center ">
                    <table class="table table-borderless align-middle text-center">
                        <thead class="table-light text-black ">
                            <tr>
                                <th style="width: 25%;">Min Qty</th>
                                <th style="width: 25%;">Bonus Qty</th>
                                <th style="width: 25%;">Status</th>
                                <th style="width: 15%;">Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptBonuses" runat="server" 
                                OnItemDataBound="rptBonuses_ItemDataBound" 
                                OnItemCommand="rptBonuses_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtMinQty" runat="server" 
                                                CssClass="form-control rounded-pill text-center" 
                                                Text='<%# Eval("MinQty") %>' AutoPostBack="true" 
                                                OnTextChanged="BonusFieldChanged" TextMode="Number" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBonusItemsRow" runat="server" 
                                                CssClass="form-control rounded-pill text-center" 
                                                Text='<%# Eval("BonusItems") %>' AutoPostBack="true" 
                                                OnTextChanged="BonusFieldChanged" TextMode="Number" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlStatus" runat="server" 
                                                CssClass="form-select rounded-pill text-center" AutoPostBack="true"
                                                OnSelectedIndexChanged="BonusFieldChanged">
                                                <asp:ListItem Value="true" Text="Active"></asp:ListItem>
                                                <asp:ListItem Value="false" Text="Inactive"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="text-center">
                                            <asp:LinkButton ID="btnRemove" runat="server" CommandName="Remove" 
                                                CommandArgument='<%# Container.ItemIndex %>' 
                                                CssClass="btn btn-sm btn-outline-danger rounded-pill">
                                                <i class="bi bi-trash"></i>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
              </ContentTemplate>

            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAddBonus" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="rptBonuses" EventName="ItemCommand" />

            </Triggers>
        </asp:UpdatePanel>


                <!-- Submit Buttons -->
                <div class="d-flex flex-wrap align-items-center justify-content-center gap-2 text-center mt-4">
                    <asp:Button ID="btnSave" runat="server"
                        Text="Save Product"
                        CssClass="btn btn-primary px-5 py-2 rounded-pill"
                        OnClick="btnSave_Click"
                        ValidationGroup="ProductForm" />
                    <asp:HyperLink NavigateUrl="/product" Text="Back to List"
                        CssClass="btn btn-secondary px-5 py-2 rounded-pill" runat="server" />
                </div>

        </div>
    </div>
</asp:Content>