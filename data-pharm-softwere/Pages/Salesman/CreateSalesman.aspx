<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="CreateSalesman.aspx.cs" Inherits="data_pharm_softwere.Pages.Salesman.CreateSalesman" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
   
   <div class="container my-5">
   <div class="card shadow-sm rounded-4 p-4 mx-auto" style="max-width: 900px;">

    <h3 class="mb-4 text-center fw-semibold">Add Salesman</h3>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" />

    <asp:Panel ID="pnlForm" runat="server" >
        <asp:UpdatePanel ID="upAllControls" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row g-3">
                    <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-semibold" EnableViewState="false" />

                    <!-- Name -->
                    <div class="col-md-12">
                        <label class="form-label fw-semibold">Salesman Name</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control rounded-pill" />
                        <asp:RequiredFieldValidator ID="rfvName" runat="server"
                            ControlToValidate="txtName"
                            ErrorMessage="Town Name is required"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="SalesmanForm" />
                    </div> 

                    <!-- Email -->
                    <div class="col-md-6">
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
                    <div class="col-md-6">
                        <label class="form-label fw-semibold">Contact Number</label>
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

                    <!-- Assign Town -->
                    <div class="col-md-8">
                        <label class="form-label fw-semibold">Select Town</label>
                       <asp:DropDownList ID="ddlTown" 
                           runat="server" 
                           AutoPostBack="true" 
                           CssClass="form-select rounded-pill"
                           OnSelectedIndexChanged="ddlTown_SelectedIndexChanged" 
                           >
                       </asp:DropDownList>

                    </div>

                    <!-- Assigned Towns -->
                    <div class="col-md-12">
                        <label class="form-label fw-semibold">Assigned Towns</label>
                        <div class="d-flex flex-wrap gap-2 mt-1">
                            <asp:Repeater ID="rptAssignedTowns" runat="server">
                                <ItemTemplate>
                                    <div class="assigned-town badge text-dark bg-light px-3 py-2 rounded-pill">
                                        <%# Eval("Name") %>
                                        <asp:LinkButton ID="btnRemoveTown" runat="server"
                                            CssClass="btn-close ms-2 small border-0 shadow-none focus-ring-0"
                                            CommandArgument='<%# Eval("TownID") %>'
                                            OnClick="btnRemoveTown_Click"
                                            ToolTip="Remove Town">
                                        </asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>

                <div class="mt-4 d-flex align-items-center justify-content-between">
                    <asp:Button ID="btnSave" runat="server"
                        Text="Save Salesman"
                        CssClass="btn btn-primary px-5 py-2 rounded-pill"
                        ValidationGroup="SalesmanForm"
                        OnClick="btnSave_Click" />
                    <asp:HyperLink NavigateUrl="/salesman" Text="Back to List"
                                   CssClass="btn btn-secondary px-5 py-2 rounded-pill" runat="server" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlTown" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
   </div>
   </div>
</asp:Content>
