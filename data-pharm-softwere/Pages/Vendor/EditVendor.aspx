<%@ Page Title="Edit Vendor" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="EditVendor.aspx.cs" Inherits="data_pharm_softwere.Pages.Vendor.EditVendor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-5">
        <h2 class="mb-4">Edit Vendor</h2>

        <asp:ValidationSummary runat="server" CssClass="text-danger" />

        <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-semibold"></asp:Label>

        <asp:Panel ID="pnlForm" runat="server" CssClass="card shadow-sm p-4 rounded-4">
            <div class="row g-3">

                <div class="col-md-6">
                    <label for="txtID" class="form-label fw-semibold">Account ID</label>
                   <div class="d-flex gap-2 align-items-end">
                        <asp:TextBox ID="txtID" runat="server" CssClass="form-control rounded-pill" />
                        <asp:Button ID="btnFetchAccount" runat="server" Text="Get Account" OnClick="btnFetchAccount_Click" CssClass="btn btn-outline-primary" />
                        <asp:RequiredFieldValidator ID="revID" runat="server"
                            ControlToValidate="txtID"
                            ErrorMessage="ID is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="VendorForm" 
                            />
                        </div>
                    </div>

                    <div class="col-md-6">
                        <label for="txtName" class="form-label fw-semibold">Vendor Name</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control rounded-pill bg-light text-secondary" ReadOnly="true" />
                    </div>
        

                    <div class="col-md-6">
                        <label for="txtEmail" class="form-label fw-semibold mt-3">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control rounded-pill" TextMode="Email" />
                    </div>

                    <div class="col-md-6">
                        <label for="txtContact" class="form-label fw-semibold mt-3">Contact</label>
                        <asp:TextBox ID="txtContact" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RegularExpressionValidator 
                            ID="revContact" 
                            runat="server"
                            ControlToValidate="txtContact"
                            ValidationExpression="^$|^(\+?\d{1,4}[\s-]?)?(\(?\d{3,4}\)?[\s-]?)?\d{3,4}[\s-]?\d{3,4}$"
                            ErrorMessage="Invalid phone number format"
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <div class="col-md-6">
                        <label for="txtAddress" class="form-label fw-semibold mt-3">Address</label>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ControlToValidate="txtAddress" runat="server" ErrorMessage="Address is required" CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <div class="col-md-6">
                        <label for="txtTown" class="form-label fw-semibold mt-3">Town</label>
                        <asp:TextBox ID="txtTown" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ControlToValidate="txtTown" runat="server" ErrorMessage="Town is required" CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <div class="col-md-6">
                        <label for="txtCity" class="form-label fw-semibold mt-3">City</label>
                        <asp:TextBox ID="txtCity" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ControlToValidate="txtCity" runat="server" ErrorMessage="City is required" CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <div class="col-md-6">
                        <label for="txtCompanyCode" class="form-label fw-semibold mt-3">Company Code</label>
                        <asp:TextBox ID="txtCompanyCode" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ControlToValidate="txtCompanyCode" runat="server" ErrorMessage="Company Code is required" CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <div class="col-md-6">
                        <label for="txtLicenceNo" class="form-label fw-semibold mt-3">Licence No</label>
                        <asp:TextBox ID="txtLicenceNo" runat="server" CssClass="form-control rounded-pill" />
                    </div>

                    <div class="col-md-6">
                        <label for="txtExpiryDate" class="form-label fw-semibold mt-3">Expiry Date</label>
                        <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control rounded-pill" TextMode="Date" />
                        <asp:RequiredFieldValidator ControlToValidate="txtExpiryDate" runat="server" ErrorMessage="Expiry Date is required" CssClass="text-danger" Display="Dynamic" />
                    </div>


                    <div class="col-md-4">
                        <label for="txtGstNo" class="form-label fw-semibold mt-3">GST No</label>
                        <asp:TextBox ID="txtGstNo" runat="server" CssClass="form-control rounded-pill" />
                    </div>

                    <div class="col-md-4">
                        <label for="txtNtnNo" class="form-label fw-semibold mt-3">NTN No</label>
                        <asp:TextBox ID="txtNtnNo" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ControlToValidate="txtNtnNo" runat="server" ErrorMessage="NTN No is required" CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <!-- Adv Tax Rate -->
                     <div class="col-md-4">
                         <label class="form-label fw-semibold mt-3">Adv Tax Rate(%)</label>
                         <asp:TextBox ID="txtAdvTax" runat="server"
                             CssClass="form-control rounded-pill"
                             TextMode="Number" />
                         <asp:RequiredFieldValidator ID="rfvAdvTax" runat="server"
                             ControlToValidate="txtAdvTax"
                             ErrorMessage="AdvTax is required"
                             CssClass="text-danger"
                             Display="Dynamic"
                             ValidationGroup="VendorForm" />
                         <asp:RegularExpressionValidator ID="revAdvTax" runat="server"
                             ControlToValidate="txtAdvTax"
                             ValidationExpression="^(100(\.0{1,2})?|[0-9]{1,2}(\.[0-9]{1,2})?)$"
                             ErrorMessage="Enter valid AdvTax (0–100%)"
                             CssClass="text-danger"
                             Display="Dynamic"
                             ValidationGroup="VendorForm" />
                     </div>


                     <div class="col-md-6">
                        <label for="txtSraCode" class="form-label fw-semibold">SRA Code</label>
                        <asp:TextBox ID="txtSraCode" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ControlToValidate="txtSraCode" runat="server" ErrorMessage="SRA Code is required" CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <div class="col-md-6">
                        <label for="txtRemarks" class="form-label fw-semibold">Remarks</label>
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control rounded-pill"/>
                    </div>
            </div>

            <div class="d-flex flex-wrap gap-2 justify-content-between mt-4">
                <asp:Button ID="btnUpdate" runat="server" Text="Update Vendor" CssClass="btn btn-primary px-5 py-2 rounded-pill" OnClick="btnUpdate_Click" />
                <asp:HyperLink ID="btnCreateVendor" runat="server" NavigateUrl="/vendor/create" CssClass="btn btn-primary px-5 py-2 rounded-pill">+ Add New Vendor</asp:HyperLink>
                <asp:HyperLink NavigateUrl="/vendor" Text="Back to List" CssClass="btn btn-secondary px-5 py-2 rounded-pill" runat="server" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
