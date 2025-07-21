<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="EditDivision.aspx.cs" Inherits="data_pharm_softwere.Pages.Division.EditDivision" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <div class="container my-5">
     <h2 class="mb-4">Edit Division</h2>

     <asp:ValidationSummary ID="ValidationSummary1" runat="server"
                            CssClass="alert alert-danger"
                            HeaderText="Please fix the following errors:"
                            DisplayMode="BulletList"
                            ShowSummary="true"
                            ValidationGroup="DivisionForm" />

     <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-semibold"></asp:Label>

     <asp:Panel ID="pnlForm" runat="server" CssClass="card shadow-sm p-4 rounded-4">
         <div class="row g-3">
             <div>
                 <label for="txtName" class="form-label fw-semibold">Division Name</label>
                 <asp:TextBox ID="txtName" runat="server" CssClass="form-control rounded-pill" />
                 <asp:RequiredFieldValidator ID="rfvName" runat="server"
                                             ControlToValidate="txtName"
                                             ErrorMessage="Division Name is required"
                                             CssClass="text-danger"
                                             Display="Dynamic"
                                             ValidationGroup="DivisionForm" />
             </div>
             <div>
                 <label for="ddlVendor" class="form-label fw-semibold">Vendor</label>
                 <asp:DropDownList ID="ddlVendor" runat="server" CssClass="form-select rounded-pill" />
                 <asp:RequiredFieldValidator ID="rfvVendor" runat="server"
                                             ControlToValidate="ddlVendor"
                                             InitialValue=""
                                             ErrorMessage="Vendor is required"
                                             CssClass="text-danger"
                                             Display="Dynamic"
                                             ValidationGroup="DivisionForm" />
             </div>

         </div>

         <div class="d-flex flex-wrap gap-2 justify-content-between mt-4">
             <asp:Button ID="btnUpdate" runat="server" Text="Update Division" CssClass="btn btn-primary px-5 py-2 rounded-pill" OnClick="btnUpdate_Click" ValidationGroup="DivisionForm" />
             <asp:HyperLink  runat="server" NavigateUrl="/division/create"  CssClass="btn btn-primary px-5 py-2 rounded-pill">+ Add New Division</asp:HyperLink>
             <asp:HyperLink NavigateUrl="/division" Text="Back to List" CssClass="btn btn-secondary px-5 py-2 rounded-pill" runat="server" />
         </div>
     </asp:Panel>
 </div>
</asp:Content>
