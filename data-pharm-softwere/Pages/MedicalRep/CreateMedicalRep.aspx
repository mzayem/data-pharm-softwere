<%@ Page Title="Create Medical Rep" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="CreateMedicalRep.aspx.cs" Inherits="data_pharm_softwere.Pages.MedicalRep.CreateMedicalRep" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container my-5">
    <div class="card shadow-sm rounded-4 p-4 mx-auto" style="max-width: 900px;">

     <h3 class="mb-4 text-center fw-semibold">Add Medical Rep</h3>

     <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" />

     <asp:Panel ID="pnlForm" runat="server" >
         <asp:UpdatePanel ID="upAllControls" runat="server" UpdateMode="Conditional">
             <ContentTemplate>
                 <div class="row g-3">
                     <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-semibold" EnableViewState="false" />

                     <!-- Name -->
                     <div class="col-md-12">
                         <label class="form-label fw-semibold">Medical Rep Name</label>
                         <asp:TextBox ID="txtName" runat="server" CssClass="form-control rounded-pill" />
                         <asp:RequiredFieldValidator ID="rfvName" runat="server"
                             ControlToValidate="txtName"
                             ErrorMessage="Town Name is required"
                             CssClass="text-danger"
                             Display="Dynamic"
                             ValidationGroup="MedicalRepForm" />
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
                                                         ValidationGroup="MedicalRepForm" />
                 
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
                                                         ValidationGroup="MedicalRepForm" />

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

                       <!-- MedicalRep Type -->
                       <div class="col-md-6">
                            <label class="form-label fw-semibold">Medical Rep Type</label>
                            <asp:DropDownList ID="ddlMedicalRepType" runat="server" CssClass="form-select rounded-pill" />
                            <asp:RequiredFieldValidator ID="rfvMedicalRepType" runat="server"
                                                        ControlToValidate="ddlMedicalRepType"
                                                        InitialValue=""
                                                        ErrorMessage="Medical Rep Type is required"
                                                        CssClass="text-danger"
                                                        Display="Dynamic"
                                                        ValidationGroup="MedicalRepForm" />

                        </div>

                     <!-- Assign SubGroup -->
                     <div class="col-md-6">
                         <label class="form-label fw-semibold">Select SubGroup</label>
                        <asp:DropDownList ID="ddlSubGroup" 
                            runat="server" 
                            AutoPostBack="true" 
                            CssClass="form-select rounded-pill"
                            OnSelectedIndexChanged="ddlSubGroup_SelectedIndexChanged" 
                            >
                        </asp:DropDownList>

                     </div>

                     <!-- Assigned SubGroup -->
                     <div class="col-md-12">
                         <label class="form-label fw-semibold">Medical Rep SubGroups</label>
                         <div class="d-flex flex-wrap gap-2 mt-1">
                             <asp:Repeater ID="rptAssignedSubGroups" runat="server">
                                 <ItemTemplate>
                                     <div class="badge text-dark bg-light px-3 py-2 rounded-pill">
                                         <%# Eval("Name") %>
                                         <asp:LinkButton ID="btnRemoveSubGroup" runat="server"
                                             CssClass="btn-close ms-2 small border-0 shadow-none focus-ring-0"
                                             CommandArgument='<%# Eval("SubGroupID") %>'
                                             OnClick="btnRemoveSubGroup_Click"
                                             ToolTip="Remove SubGroup">
                                         </asp:LinkButton>
                                     </div>
                                 </ItemTemplate>
                             </asp:Repeater>
                         </div>
                     </div>
                 </div>

                 <div class="mt-4 d-flex align-items-center justify-content-between">
                     <asp:Button ID="btnSave" runat="server"
                         Text="Save Medical Rep"
                         CssClass="btn btn-primary px-5 py-2 rounded-pill"
                         ValidationGroup="MedicalRepForm"
                         OnClick="btnSave_Click" />
                     <asp:HyperLink NavigateUrl="/medicalrep" Text="Back to List"
                                    CssClass="btn btn-secondary px-5 py-2 rounded-pill" runat="server" />
                 </div>
             </ContentTemplate>
             <Triggers>
                 <asp:AsyncPostBackTrigger ControlID="ddlSubGroup" EventName="SelectedIndexChanged" />
             </Triggers>
         </asp:UpdatePanel>
     </asp:Panel>
    </div>
    </div>
</asp:Content>