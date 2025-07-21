<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="CreateVendor.aspx.cs" Inherits="data_pharm_softwere.Pages.Vendor.CreateVendor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-5">
        <div class="card shadow-sm rounded-4 p-4 mx-auto" style="max-width: 900px;">
            <h3 class="mb-4 text-center fw-semibold">Add New Vendor</h3>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server"
                CssClass="alert alert-danger"
                HeaderText="Please fix the following errors:"
                DisplayMode="BulletList"
                ShowSummary="true"
                ValidationGroup="VendorForm" />

            <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-semibold"></asp:Label>

            <div id="vendorForm" class="needs-validation" novalidate="true">
                <div class="row g-3">
                    <!-- Left Column -->
                    <div class="col-md-6">
                        <div>
                            <label for="txtName" class="form-label fw-semibold">Vendor Name</label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control rounded-pill" />
                            <asp:RequiredFieldValidator ID="rfvName" runat="server"
                                ControlToValidate="txtName"
                                ErrorMessage="Vendor Name is required"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="VendorForm" />
                        </div>

                        <div>
                            <label for="txtEmail" class="form-label fw-semibold mt-3">Email</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control rounded-pill" TextMode="Email" />
                             <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                ControlToValidate="txtEmail"
                                ErrorMessage="Invalid email format"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w{2,4}$"
                                ValidationGroup="VendorForm" />
                        </div>

                        <div>
                            <label for="txtAddress" class="form-label fw-semibold mt-3">Address</label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control rounded-pill" />
                            <asp:RequiredFieldValidator ID="rfvAddress" runat="server"
                                ControlToValidate="txtAddress"
                                ErrorMessage="Address is required"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="VendorForm" />
                        </div>

                        <div>
                            <label for="txtTown" class="form-label fw-semibold mt-3">Town</label>
                            <asp:TextBox ID="txtTown" runat="server" CssClass="form-control rounded-pill" />
                            <asp:RequiredFieldValidator ID="rfvTown" runat="server"
                                ControlToValidate="txtTown"
                                ErrorMessage="Town is required"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="VendorForm" />
                        </div>

                        <div>
                            <label for="txtCity" class="form-label fw-semibold mt-3">City</label>
                            <asp:TextBox ID="txtCity" runat="server" CssClass="form-control rounded-pill" />
                            <asp:RequiredFieldValidator ID="rfvCity" runat="server"
                                ControlToValidate="txtCity"
                                ErrorMessage="City is required"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="VendorForm" />
                        </div>

                        <div>
                            <label for="txtLicenceNo" class="form-label fw-semibold mt-3">Licence No</label>
                            <asp:TextBox ID="txtLicenceNo" runat="server" CssClass="form-control rounded-pill" />
                        </div>

                        <div>
                            <label for="txtExpiryDate" class="form-label fw-semibold mt-3">Expiry Date</label>
                            <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control rounded-pill" TextMode="Date" />
                            <asp:RequiredFieldValidator ID="rfvExpiryDate" runat="server"
                                ControlToValidate="txtExpiryDate"
                                ErrorMessage="Expiry Date is required"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="VendorForm" />
                        </div>
                    </div>

                    <!-- Right Column -->
                    <div class="col-md-6">
                        <div>
                            <label for="txtSRACode" class="form-label fw-semibold">SRA Code</label>
                            <asp:TextBox ID="txtSRACode" runat="server" CssClass="form-control rounded-pill" />
                            <asp:RequiredFieldValidator ID="rfvSRACode" runat="server"
                                ControlToValidate="txtSRACode"
                                ErrorMessage="SRA Code is required"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="VendorForm" />
                        </div>

                        <div>
                            <label for="txtContact" class="form-label fw-semibold mt-3">Contact</label>
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

                        <div>
                            <label for="txtGstNo" class="form-label fw-semibold mt-3">GST No</label>
                            <asp:TextBox ID="txtGstNo" runat="server" CssClass="form-control rounded-pill" />
                            <asp:RequiredFieldValidator ID="rfvGstNo" runat="server"
                                ControlToValidate="txtGstNo"
                                ErrorMessage="GST No is required"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="VendorForm" />
                        </div>

                        <div>
                            <label for="txtNtnNo" class="form-label fw-semibold mt-3">NTN No</label>
                            <asp:TextBox ID="txtNtnNo" runat="server" CssClass="form-control rounded-pill" />
                            <asp:RequiredFieldValidator ID="rfvNtnNo" runat="server"
                                ControlToValidate="txtNtnNo"
                                ErrorMessage="NTN No is required"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="VendorForm" />
                        </div>

                        <div>
                            <label for="txtCompanyCode" class="form-label fw-semibold mt-3">Company Code</label>
                            <asp:TextBox ID="txtCompanyCode" runat="server" CssClass="form-control rounded-pill" />
                            <asp:RequiredFieldValidator ID="rfvCompanyCode" runat="server"
                                ControlToValidate="txtCompanyCode"
                                ErrorMessage="Company Code is required"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ValidationGroup="VendorForm" />
                        </div>

                       <div>
                        <label for="txtMaxDiscount" class="form-label fw-semibold mt-3">Max Discount Allowed (%)</label>
                        <asp:TextBox ID="txtMaxDiscount" runat="server" CssClass="form-control rounded-pill" TextMode="SingleLine" />
                        <asp:RequiredFieldValidator ID="rfvMaxDiscount" runat="server"
                            ControlToValidate="txtMaxDiscount"
                            ErrorMessage="Max Discount is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="VendorForm" />
                        <asp:RegularExpressionValidator 
                            ID="revDiscount" 
                            runat="server"
                            ControlToValidate="txtMaxDiscount"
                            ValidationExpression="^(100(\.0{1,2})?|[0-9]{1,2}(\.[0-9]{1,2})?)$"
                            ErrorMessage="Enter a valid discount (0–100%, up to 2 decimals)"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="VendorForm" />
                    </div>


                        <div>
                            <label for="txtRemarks" class="form-label fw-semibold mt-3">Remarks</label>
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control rounded-pill" />
                        </div>
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
        </div>
    </div>
</asp:Content>
