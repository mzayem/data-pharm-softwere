﻿<%@ Page Title="Edit Vendor" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="EditVendor.aspx.cs" Inherits="data_pharm_softwere.Pages.Vendor.EditVendor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <h2 class="mb-4">Edit Vendor</h2>

        <asp:ValidationSummary runat="server" CssClass="text-danger" />

        <asp:Panel ID="pnlForm" runat="server" CssClass="card shadow-sm p-4 rounded-4">
            <div class="row g-3">

                <!-- Left Column -->
                <div class="col-md-6">
                    <div>
                        <label for="txtName" class="form-label fw-semibold">Vendor Name</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ControlToValidate="txtName" runat="server" ErrorMessage="Vendor Name is required" CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <div>
                        <label for="txtEmail" class="form-label fw-semibold mt-3">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control rounded-pill" TextMode="Email" />
                    </div>

                    <div>
                        <label for="txtAddress" class="form-label fw-semibold mt-3">Address</label>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ControlToValidate="txtAddress" runat="server" ErrorMessage="Address is required" CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <div>
                        <label for="txtTown" class="form-label fw-semibold mt-3">Town</label>
                        <asp:TextBox ID="txtTown" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ControlToValidate="txtTown" runat="server" ErrorMessage="Town is required" CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <div>
                        <label for="txtCity" class="form-label fw-semibold mt-3">City</label>
                        <asp:TextBox ID="txtCity" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ControlToValidate="txtCity" runat="server" ErrorMessage="City is required" CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <div>
                        <label for="txtLicenceNo" class="form-label fw-semibold mt-3">Licence No</label>
                        <asp:TextBox ID="txtLicenceNo" runat="server" CssClass="form-control rounded-pill" />
                    </div>

                    <div>
                        <label for="txtExpiryDate" class="form-label fw-semibold mt-3">Expiry Date</label>
                        <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control rounded-pill" TextMode="Date" />
                        <asp:RequiredFieldValidator ControlToValidate="txtExpiryDate" runat="server" ErrorMessage="Expiry Date is required" CssClass="text-danger" Display="Dynamic" />
                    </div>
                </div>

                <!-- Right Column -->
                <div class="col-md-6">
                    <div>
                        <label for="txtSraCode" class="form-label fw-semibold">SRA Code</label>
                        <asp:TextBox ID="txtSraCode" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ControlToValidate="txtSraCode" runat="server" ErrorMessage="SRA Code is required" CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <div>
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

                    <div>
                        <label for="txtGstNo" class="form-label fw-semibold mt-3">GST No</label>
                        <asp:TextBox ID="txtGstNo" runat="server" CssClass="form-control rounded-pill" />
                    </div>

                    <div>
                        <label for="txtNtnNo" class="form-label fw-semibold mt-3">NTN No</label>
                        <asp:TextBox ID="txtNtnNo" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ControlToValidate="txtNtnNo" runat="server" ErrorMessage="NTN No is required" CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <div>
                        <label for="txtCompanyCode" class="form-label fw-semibold mt-3">Company Code</label>
                        <asp:TextBox ID="txtCompanyCode" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ControlToValidate="txtCompanyCode" runat="server" ErrorMessage="Company Code is required" CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <div>
                        <label for="txtDiscount" class="form-label fw-semibold mt-3">Max Discount Allowed (%)</label>
                        <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control rounded-pill" TextMode="Number" />
                        <asp:RequiredFieldValidator 
                            ControlToValidate="txtDiscount" 
                            runat="server" 
                            ErrorMessage="Max Discount is required" 
                            CssClass="text-danger" 
                            Display="Dynamic" />
                        <asp:RegularExpressionValidator 
                            ID="revDiscount" 
                            runat="server" 
                            ControlToValidate="txtDiscount"
                            ValidationExpression="^(100(\.0{1,2})?|[0-9]{1,2}(\.[0-9]{1,2})?)$"
                            ErrorMessage="Enter a valid discount (0 to 100%)"
                            CssClass="text-danger"
                            Display="Dynamic" />
                    </div>

                    <div>
                        <label for="txtRemarks" class="form-label fw-semibold mt-3">Remarks</label>
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control rounded-pill"/>
                    </div>
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
