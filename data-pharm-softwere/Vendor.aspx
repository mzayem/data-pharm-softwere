<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Vendor.aspx.cs" Inherits="data_pharm_softwere.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="max-w-xl mx-auto mt-10 bg-white shadow-md p-6 rounded-xl">
        <h2 class="text-lg font-semibold mb-4">Add New Vendor</h2>

        <asp:Label ID="lblMessage" runat="server" CssClass="text-green-600"></asp:Label>

        <asp:TextBox ID="txtName" runat="server" CssClass="border p-2 w-full mb-2" Placeholder="Vendor Name" />
        <asp:TextBox ID="txtEmail" runat="server" CssClass="border p-2 w-full mb-2" Placeholder="Email" />
        
        
        <asp:Button ID="btnSave" runat="server" Text="Save Vendor" CssClass="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded"
            OnClick="btnSave_Click" />
    </div>
</asp:Content>

