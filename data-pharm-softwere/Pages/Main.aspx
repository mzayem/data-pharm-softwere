<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="data_pharm_softwere.Pages.Main" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <div class="text-center mt-5">
        <h1 class="display-4 fw-bold">Welcome to Data Pharma Software</h1>
        <p class="lead mt-3">Manage your pharmaceutical vendors, inventory, and more—all from a single platform.</p>
            
            <div class="d-flex flex-wrap align-items-center justify-content-between">
                <a href="/vendor" class="btn btn-primary rounded-pill px-4 mt-4">
                    Go to Vendor Management
                </a>
                <a href="/group" class="btn btn-primary rounded-pill px-4 mt-4">
                    Go to Group Management
                </a>
                <a href="/subgroup" class="btn btn-primary rounded-pill px-4 mt-4">
                    Go to SubGroup Management
                </a>
                <a href="/product" class="btn btn-primary rounded-pill px-4 mt-4">
                    Go to Product Management
                </a>

            </div>

</div>
</asp:Content>
