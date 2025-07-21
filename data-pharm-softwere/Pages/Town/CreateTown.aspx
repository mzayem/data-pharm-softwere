<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="CreateTown.aspx.cs" Inherits="data_pharm_softwere.Pages.Town.CreateTown" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
    <div class="container my-5">
    <div class="card shadow-sm rounded-4 p-4 mx-auto" style="max-width: 900px;">

     <h3 class="mb-4 text-center fw-semibold">Create Town</h3>

     <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" />

     <asp:Panel ID="pnlForm" runat="server" >
         <asp:UpdatePanel ID="upAllControls" runat="server" UpdateMode="Conditional">
             <ContentTemplate>
                 <div class="row g-3">
                     <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-semibold" EnableViewState="false" />


                     <div class="col-md-12">
                         <label class="form-label fw-semibold">Town Name</label>
                         <asp:TextBox ID="txtName" runat="server" CssClass="form-control rounded-pill" />
                         <asp:RequiredFieldValidator ID="rfvName" runat="server"
                             ControlToValidate="txtName"
                             ErrorMessage="Town Name is required"
                             CssClass="text-danger"
                             Display="Dynamic"
                             ValidationGroup="TownForm" />
                     </div> 
                     <div class="col-md-12">
                             <label class="form-label fw-semibold">City Routes</label>
                             <asp:DropDownList ID="ddlCityRoute" runat="server" CssClass="form-select rounded-pill"
                                 AutoPostBack="true" OnSelectedIndexChanged="ddlCityRoute_SelectedIndexChanged" />
                         </div>
                 </div>

                 <div class="mt-4 d-flex align-items-center justify-content-between">
                     <asp:Button ID="btnSave" runat="server"
                         Text="Save Town"
                         CssClass="btn btn-primary px-5 py-2 rounded-pill"
                         ValidationGroup="TownForm"
                         OnClick="btnSave_Click" />
                     <asp:HyperLink NavigateUrl="/town" Text="Back to List"
                                    CssClass="btn btn-secondary px-5 py-2 rounded-pill" runat="server" />
                 </div>


             </ContentTemplate>
             <Triggers>
                 <asp:AsyncPostBackTrigger ControlID="ddlCityRoute" EventName="SelectedIndexChanged" />
             </Triggers>
         </asp:UpdatePanel>
     </asp:Panel>
 </div>
    </div>
</asp:Content>
