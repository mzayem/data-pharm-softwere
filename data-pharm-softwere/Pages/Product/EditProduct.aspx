<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="EditProduct.aspx.cs" Inherits="data_pharm_softwere.Pages.Product.EditProduct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <div class="container mt-5">
   <h3 class="mb-4 ">Edit Product</h3>
        

        <asp:ValidationSummary ID="ValidationSummary1" runat="server"
            CssClass="alert alert-danger"
            HeaderText="Please fix the following errors:"
            DisplayMode="BulletList"
            ShowSummary="true"
            ValidationGroup="ProductForm" />

        <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-semibold"></asp:Label>
    
    <asp:Panel ID="pnlForm" runat="server" CssClass="card shadow-sm p-4 rounded-4 mx-auto" style="max-width: 1200px;">

        <div class="row g-3">
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

        <!-- Submit Buttons -->
    <div class="d-flex flex-wrap gap-2 justify-content-between mt-4">
        <asp:Button ID="btnUpdate" runat="server"
                    Text="Update Product"
                    CssClass="btn btn-primary px-5 py-2 rounded-pill"
                    OnClick="btnUpdate_Click"
                    ValidationGroup="SubGroupForm" />

        <asp:HyperLink runat="server"
                       NavigateUrl="/product/create"
                       CssClass="btn btn-primary px-5 py-2 rounded-pill">+ Add New Product</asp:HyperLink>

        <asp:HyperLink runat="server"
                       NavigateUrl="/product"
                       Text="Back to List"
                       CssClass="btn btn-secondary px-5 py-2 rounded-pill" />
        </div>
    </asp:Panel>
    </div>


</asp:Content>
