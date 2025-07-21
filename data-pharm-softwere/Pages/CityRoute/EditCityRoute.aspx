<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="EditCityRoute.aspx.cs" Inherits="data_pharm_softwere.Pages.CityRoute.EditCityRoute" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <div class="container my-5">
     <div class="card shadow-sm rounded-4 p-4 mx-auto" style="max-width: 900px;">
         <h3 class="mb-4 text-center fw-semibold">Add New Route</h3>

         <asp:ValidationSummary ID="ValidationSummary1" runat="server"
                                CssClass="alert alert-danger"
                                HeaderText="Please fix the following errors:"
                                DisplayMode="BulletList"
                                ShowSummary="true"
                                ValidationGroup="CityRouteForm" />

         <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-semibold"  EnableViewState="false"></asp:Label>

         <div class="needs-validation" novalidate="true">
             <div class="row g-3">
                 <div>
                     <label for="txtName" class="form-label fw-semibold">Route Name</label>
                     <asp:TextBox ID="txtName" runat="server" CssClass="form-control rounded-pill" />
                     <asp:RequiredFieldValidator ID="rfvName" runat="server"
                                                 ControlToValidate="txtName"
                                                 ErrorMessage="Route Name is required"
                                                 CssClass="text-danger"
                                                 Display="Dynamic"
                                                 ValidationGroup="CityRouteForm" />
                 </div>

             </div>

             <div class="d-flex flex-wrap align-items-center justify-content-center gap-2 text-center mt-4">
                 <asp:Button ID="btnSave" runat="server"
                             Text="Save Route"
                             CssClass="btn btn-success px-5 py-2 rounded-pill"
                             OnClick="btnUpdate_Click"
                             ValidationGroup="CityRouteForm" />
                 <asp:HyperLink runat="server"
                                NavigateUrl="/city-route/create"
                                CssClass="btn btn-primary px-5 py-2 rounded-pill">+ Add New Route</asp:HyperLink>
             
                 <asp:HyperLink NavigateUrl="/city-route" Text="Back to List"
                                CssClass="btn btn-secondary px-5 py-2 rounded-pill" runat="server" />
             </div>
            
         </div>
     </div>
 </div>
</asp:Content>
