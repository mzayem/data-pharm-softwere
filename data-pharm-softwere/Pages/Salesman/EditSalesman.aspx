<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="EditSalesman.aspx.cs" Inherits="data_pharm_softwere.Pages.Salesman.EditSalesman" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
   <div class="container my-5">
   <div class="card shadow-sm rounded-4 p-4 mx-auto" style="max-width: 900px;">

    <h3 class="mb-4 text-center fw-semibold">Update Salesman Details</h3>

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
                     <div class="mt-4 d-flex flex-column align-content-center align-items-center ">
                        <table class="table table-borderless align-middle text-center">
                            <thead class="table-light text-black ">
                                <tr>
                                    <th style="width: 25%;">Town</th>
                                    <th style="width: 25%;">Type</th>
                                    <th style="width: 25%;">Percentage</th>
                                    <th style="width: 15%;">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptAssignedTowns" runat="server" 
                                    OnItemDataBound="rptAssignedTowns_ItemDataBound" 
                                    OnItemCommand="rptAssignedTowns_ItemCommand">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfTownID" runat="server" Value='<%# Eval("TownID") %>' />
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtTownName" runat="server" 
                                                    CssClass="form-control rounded-pill text-center bg-light text-secondary" 
                                                    Text='<%# Eval("TownName") %>' ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlAssignmentType" runat="server" 
                                                    CssClass="form-select rounded-pill text-center" 
                                                    AutoPostBack="true" 
                                                    OnSelectedIndexChanged="BonusFieldChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPercentage" runat="server" 
                                                    CssClass="form-control rounded-pill text-center" 
                                                    Text='<%# Eval("Percentage") %>' AutoPostBack="true" 
                                                    OnTextChanged="BonusFieldChanged" TextMode="Number" />
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
                </div>

                <div class="mt-4 d-flex align-items-center justify-content-between">
                    <asp:Button ID="btnSave" runat="server"
                        Text="Update Salesman"
                        CssClass="btn btn-primary px-5 py-2 rounded-pill"
                        ValidationGroup="SalesmanForm"
                        OnClick="btnSave_Click" />
                    <asp:HyperLink runat="server" NavigateUrl="/salesman/create"
               CssClass="btn btn-primary px-5 py-2 rounded-pill">+ Add New Salesman</asp:HyperLink>
                    <asp:HyperLink NavigateUrl="/salesman" Text="Back to List"
                                   CssClass="btn btn-secondary px-5 py-2 rounded-pill" runat="server" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlTown" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="rptAssignedTowns" EventName="ItemCommand" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
   </div>
   </div>
</asp:Content>
