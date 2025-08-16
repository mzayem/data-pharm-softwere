<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="CreateVendor.aspx.cs" Inherits="data_pharm_softwere.Pages.Vendor.CreateVendor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid my-5">
            <h3 class="mb-4 text-center fw-semibold">Add New Vendor</h3>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server"
                CssClass="alert alert-danger"
                HeaderText="Please fix the following errors:"
                DisplayMode="BulletList"
                ShowSummary="true"
                ValidationGroup="VendorForm" />
            

            <asp:UpdatePanel ID="UpdatePanelForm" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                
                <asp:Panel ID="pnlForm" runat="server" CssClass="card shadow-sm p-4 rounded-4">
                    <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-semibold"></asp:Label>

                <div id="vendorForm" class="needs-validation" novalidate="true">
                <div class="row g-3">

                    <!-- ID + Button -->
                    <div class="col-md-6">
                        <label for="txtID" class="form-label fw-semibold">Account ID</label>
                        <div class="d-flex gap-2">
                            <asp:TextBox ID="txtID" runat="server" CssClass="form-control rounded-pill " />
                            <asp:Button ID="btnFetchAccount" runat="server" Text="Get Account" OnClick="btnFetchAccount_Click" CssClass="btn btn-outline-primary" />
                        </div>
                        <asp:RequiredFieldValidator ID="revID" runat="server"
                            ControlToValidate="txtID"
                            ErrorMessage="ID is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="VendorForm" />
                    </div>

                    <!-- Vendor Name -->
                    <div class="col-md-6">
                        <label for="txtName" class="form-label fw-semibold">Vendor Name</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control rounded-pill bg-light text-secondary" ReadOnly="true" />
                    </div>

                    <!-- Email -->
                    <div class="col-md-6">
                        <label for="txtEmail" class="form-label fw-semibold">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control rounded-pill" TextMode="Email" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server"
                            ControlToValidate="txtEmail"
                            ErrorMessage="Invalid email format"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w{2,4}$"
                            ValidationGroup="VendorForm" />
                    </div>

                    <!-- Address -->
                    <div class="col-md-6">
                        <label for="txtAddress" class="form-label fw-semibold">Address</label>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ID="rfvAddress" runat="server"
                            ControlToValidate="txtAddress"
                            ErrorMessage="Address is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="VendorForm" />
                    </div>

                    <!-- Town -->
                    <div class="col-md-6">
                        <label for="txtTown" class="form-label fw-semibold">Town</label>
                        <asp:TextBox ID="txtTown" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ID="rfvTown" runat="server"
                            ControlToValidate="txtTown"
                            ErrorMessage="Town is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="VendorForm" />
                    </div>

                    <!-- City -->
                    <div class="col-md-6">
                        <label for="txtCity" class="form-label fw-semibold">City</label>
                        <asp:TextBox ID="txtCity" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ID="rfvCity" runat="server"
                            ControlToValidate="txtCity"
                            ErrorMessage="City is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="VendorForm" />
                    </div>

                    <!-- Licence No -->
                    <div class="col-md-6">
                        <label for="txtLicenceNo" class="form-label fw-semibold">Licence No</label>
                        <asp:TextBox ID="txtLicenceNo" runat="server" CssClass="form-control rounded-pill" />
                    </div>

                    <!-- Expiry Date -->
                    <div class="col-md-6">
                        <label for="txtExpiryDate" class="form-label fw-semibold">Expiry Date</label>
                        <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control rounded-pill" TextMode="Date" />
                        <asp:RequiredFieldValidator ID="rfvExpiryDate" runat="server"
                            ControlToValidate="txtExpiryDate"
                            ErrorMessage="Expiry Date is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="VendorForm" />
                    </div>

                    <!-- Contact -->
                    <div class="col-md-6">
                        <label for="txtContact" class="form-label fw-semibold">Contact</label>
                        <asp:TextBox ID="txtContact" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RegularExpressionValidator 
                            ID="revContact" 
                            runat="server"
                            ControlToValidate="txtContact"
                            ValidationExpression="^(\+?\d{1,4}[\s-]?)?(\(?\d{3,4}\)?[\s-]?)?\d{3,4}[\s-]?\d{3,4}$"
                            ErrorMessage="Invalid phone number format"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="VendorForm" />
                    </div>

                    <!-- GST No -->
                    <div class="col-md-6">
                        <label for="txtGstNo" class="form-label fw-semibold">GST No</label>
                        <asp:TextBox ID="txtGstNo" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ID="rfvGstNo" runat="server"
                            ControlToValidate="txtGstNo"
                            ErrorMessage="GST No is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="VendorForm" />
                    </div>

                    <!-- NTN No -->
                    <div class="col-md-6">
                        <label for="txtNtnNo" class="form-label fw-semibold">NTN No</label>
                        <asp:TextBox ID="txtNtnNo" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ID="rfvNtnNo" runat="server"
                            ControlToValidate="txtNtnNo"
                            ErrorMessage="NTN No is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="VendorForm" />
                    </div>

                    <!-- Company Code -->
                    <div class="col-md-6">
                        <label for="txtCompanyCode" class="form-label fw-semibold">Company Code</label>
                        <asp:TextBox ID="txtCompanyCode" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ID="rfvCompanyCode" runat="server"
                            ControlToValidate="txtCompanyCode"
                            ErrorMessage="Company Code is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="VendorForm" />
                    </div>

                    <!-- SRA Code -->
                    <div class="col-md-6">
                        <label for="txtSRACode" class="form-label fw-semibold">SRA Code</label>
                        <asp:TextBox ID="txtSRACode" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ID="rfvSRACode" runat="server"
                            ControlToValidate="txtSRACode"
                            ErrorMessage="SRA Code is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="VendorForm" />
                    </div>

                    <!-- Remarks -->
                    <div class="col-md-6">
                        <label for="txtRemarks" class="form-label fw-semibold">Remarks</label>
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control rounded-pill" />
                    </div>

                </div>

                <div class="d-flex flex-wrap align-items-center justify-content-center gap-2 text-center mt-4">
                    <asp:Button ID="btnSave" runat="server"
                        Text="Save Vendor"
                        CssClass="btn btn-primary px-5 py-2 rounded-pill"
                        OnClick="btnSave_Click"
                        ValidationGroup="VendorForm" />
                    <asp:HyperLink NavigateUrl="/vendor" Text="Back to List" CssClass="btn btn-secondary px-5 py-2 rounded-pill" runat="server" />
                </div>
            </div>
             </asp:Panel>
            </ContentTemplate>

            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnFetchAccount" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
