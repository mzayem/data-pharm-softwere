<%@ Page Title="Create Customers" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="CreateCustomer.aspx.cs" Inherits="data_pharm_softwere.Pages.Customer.CreateCustomer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid my-5">

            <asp:ValidationSummary ID="ValidationSummary1" runat="server"
                CssClass="alert alert-danger"
                HeaderText="Please fix the following errors:"
                DisplayMode="BulletList"
                ShowSummary="true"
                ValidationGroup="CustomerForm" />
            <asp:UpdatePanel ID="upForm" runat="server" UpdateMode="Conditional">

                <ContentTemplate>
                    <asp:Panel ID="pnlForm" runat="server" CssClass="card shadow-sm p-4 rounded-4">
                         <h3 class="mb-4 text-center fw-semibold">Add New Customer</h3>
                         <asp:Label ID="lblMessage" runat="server" CssClass="fw-semibold d-block mt-2"></asp:Label>
                         <div id="CustomerForm" class="needs-validation" novalidate="true">
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
                                ValidationGroup="CustomerForm" />
                        </div>

                            <!-- Name -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Customer Name</label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control rounded-pill bg-light text-secondary" ReadOnly="true" />
                            </div>

                            <!-- Email -->
                            <div class="col-md-4">
                                <label class="form-label fw-semibold">Email</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control rounded-pill" TextMode="Email" />
                                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                                                ControlToValidate="txtEmail"
                                                                ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"
                                                                ErrorMessage="Invalid email address"
                                                                CssClass="text-danger"
                                                                Display="Dynamic"
                                                                ValidationGroup="CustomerForm" />
                            </div>

                            <!-- Contact -->
                            <div class="col-md-4">
                                <label class="form-label fw-semibold">Contact</label>
                                <asp:TextBox ID="txtContact" runat="server" CssClass="form-control rounded-pill" />
                                <asp:RequiredFieldValidator ID="rfvContactreq" runat="server"
                                                            ControlToValidate="txtContact"
                                                            ErrorMessage="Contact No is required"
                                                            CssClass="text-danger"
                                                            Display="Dynamic"
                                                            ValidationGroup="CustomerForm" />

                                <asp:RegularExpressionValidator 
                                    ID="revContact" 
                                    runat="server"
                                    ControlToValidate="txtContact"
                                    ValidationExpression="^(\+?\d{1,4}[\s-]?)?(\(?\d{3,4}\)?[\s-]?)?\d{3,4}[\s-]?\d{3,4}$"
                                    ErrorMessage="Invalid phone number format"
                                    CssClass="text-danger"
                                    Display="Dynamic"
                                    ValidationGroup="CustomerForm" />
                            </div>

                            <!-- CNIC -->
                            <div class="col-md-4">
                                <label class="form-label fw-semibold">CNIC</label>
                                <asp:TextBox ID="txtCNIC" runat="server" CssClass="form-control rounded-pill" />
                                <asp:RequiredFieldValidator ID="rfvCNIC" runat="server"
                                                            ControlToValidate="txtCNIC"
                                                            ErrorMessage="CNIC is required"
                                                            CssClass="text-danger"
                                                            Display="Dynamic"
                                                            ValidationGroup="CustomerForm" />
                                <asp:RegularExpressionValidator ID="revCNIC" runat="server"
                                                                ControlToValidate="txtCNIC"
                                                                ValidationExpression="^\d{5}-\d{7}-\d{1}$"
                                                                ErrorMessage="CNIC format should be like 31102-1573501-3"
                                                                CssClass="text-danger"
                                                                Display="Dynamic"
                                                                ValidationGroup="CustomerForm" />

                            </div>

                            <!-- Address -->
                            <div class="col-md-12">
                                <label class="form-label fw-semibold">Address</label>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control rounded-pill" />
                                <asp:RequiredFieldValidator ID="rfvAddress" runat="server"
                                                            ControlToValidate="txtAddress"
                                                            ErrorMessage="Address is required"
                                                            CssClass="text-danger"
                                                            Display="Dynamic"
                                                            ValidationGroup="CustomerForm" />

                            </div>

                            <!-- Filters: CityRoute and Town -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">City Route</label>
                                <asp:DropDownList ID="ddlCityRoute" runat="server" CssClass="form-select rounded-pill"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlCityRoute_SelectedIndexChanged" />
                            </div>

                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Town</label>
                                <asp:DropDownList ID="ddlTown" runat="server" CssClass="form-select rounded-pill" />
                                <asp:RequiredFieldValidator ID="rfvTown" runat="server"
                                                            ControlToValidate="ddlTown"
                                                            InitialValue=""
                                                            ErrorMessage="Town is required"
                                                            CssClass="text-danger"
                                                            Display="Dynamic"
                                                            ValidationGroup="CustomerForm" />

                            </div>

                            <!-- Licence No -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Licence No</label>
                                <asp:TextBox ID="txtLicenceNo" runat="server" CssClass="form-control rounded-pill" />
                                <asp:RequiredFieldValidator ID="rfvLicenceNo" runat="server"
                                                            ControlToValidate="txtLicenceNo"
                                                            ErrorMessage="Licence No is required"
                                                            CssClass="text-danger"
                                                            Display="Dynamic"
                                                            ValidationGroup="CustomerForm" />

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
                                    ValidationGroup="CustomerForm" />
                            </div>

                            <!-- NTN -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">NTN No</label>
                                <asp:TextBox ID="txtNtnNo" runat="server" CssClass="form-control rounded-pill" />
                                <asp:RequiredFieldValidator ID="rfvNtnNo" runat="server"
                                                            ControlToValidate="txtNtnNo"
                                                            ErrorMessage="NTN No is required"
                                                            CssClass="text-danger"
                                                            Display="Dynamic"
                                                            ValidationGroup="CustomerForm" />
                            </div>

                            <!-- Customer Type -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Customer Type</label>
                                <asp:DropDownList ID="ddlCustomerType" runat="server" CssClass="form-select rounded-pill" />
                                <asp:RequiredFieldValidator ID="rfvCustomerType" runat="server"
                                                            ControlToValidate="ddlCustomerType"
                                                            InitialValue=""
                                                            ErrorMessage="Customer Type is required"
                                                            CssClass="text-danger"
                                                            Display="Dynamic"
                                                            ValidationGroup="CustomerForm" />

                            </div>

                            <!-- Checkboxes -->
                        <div class="row g-3">
                            <div class="col-md-4 mt-2">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkNorcoticsSaleAllowed" runat="server" CssClass="form-check-input border-0" />
                                    <label class="form-check-label ms-2">Narcotics Sale Allowed</label>
                                </div>
                            </div>
                            <div class="col-md-4 mt-2">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkInActive" runat="server" CssClass="form-check-input border-0" />
                                    <label class="form-check-label ms-2">Inactive</label>
                                </div>
                            </div>
                            <div class="col-md-4 mt-2">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkAdvTaxExempted" runat="server" CssClass="form-check-input border-0" />
                                    <label class="form-check-label ms-2">Advance Tax Exempted</label>
                                </div>
                            </div>
                            <div class="col-md-4 mt-2">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkFbrInActiveGST" runat="server" CssClass="form-check-input border-0" />
                                    <label class="form-check-label ms-2">FBR Inactive GST</label>
                                </div>
                            </div>
                            <div class="col-md-4 mt-2">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkFBRInActiveTax236H" runat="server" CssClass="form-check-input border-0" />
                                    <label class="form-check-label ms-2">FBR Inactive Tax 236H</label>
                                </div>
                            </div>
                        </div>


                        <!-- Buttons -->
                        <div class="d-flex flex-wrap align-items-center justify-content-center gap-2 text-center mt-4">
                            <asp:Button ID="btnSave" runat="server"
                                Text="Save Customer"
                                CssClass="btn btn-primary px-5 py-2 rounded-pill"
                                ValidationGroup="CustomerForm"
                                OnClick="btnSave_Click" />
                            <asp:HyperLink NavigateUrl="/customer" Text="Back to List"
                                CssClass="btn btn-secondary px-5 py-2 rounded-pill" runat="server" />
                        </div>
                    </div>
                  </asp:Panel>
                </ContentTemplate>

               <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnFetchAccount" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ddlCityRoute" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    
        <script type="text/javascript">
            function attachCnicFormatter() {
                var $cnicInput = $("#<%= txtCNIC.ClientID %>");
                $cnicInput.off("input").on("input", function () {
                    let value = this.value.replace(/\D/g, '');
                    if (value.length > 5)
                        value = value.slice(0, 5) + '-' + value.slice(5);
                    if (value.length > 13)
                        value = value.slice(0, 13) + '-' + value.slice(13);
                    if (value.length > 15)
                        value = value.slice(0, 15);
                    this.value = value;
                });
            }

            // Handles both full load and UpdatePanel partial updates
            Sys.Application.add_load(attachCnicFormatter);
        </script>

</asp:Content>
