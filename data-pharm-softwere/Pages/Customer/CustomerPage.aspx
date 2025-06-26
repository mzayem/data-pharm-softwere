<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="CustomerPage.aspx.cs" Inherits="data_pharm_softwere.Pages.Customer.CustomerPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
<div class="container mt-4">
    <h2 class="mb-3">Customer List</h2>

    <asp:HyperLink 
        ID="btnCreateProduct" 
        runat="server" 
        NavigateUrl="/customer/create"
        CssClass="btn btn-primary mb-3">
        + Add New Customer
    </asp:HyperLink>
    <asp:Button 
        ID="btnExportExcel" 
        runat="server" 
        CssClass="btn btn-success mb-3 ms-2" 
        Text="Export to Excel" 
        OnClick="btnExportExcel_Click" />
    <asp:Button 
        ID="btnExportPdf" 
        runat="server" 
        CssClass="btn btn-danger mb-3 ms-2" 
        Text="Export to PDF" 
        OnClick="btnExportPdf_Click" />


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:TextBox 
                ID="txtSearch" 
                runat="server" 
                CssClass="form-control mb-3"
                Placeholder="Search by Customer name, code, Ntn, " 
                AutoPostBack="true" 
                OnTextChanged="txtSearch_TextChanged" />
            <asp:Panel ID="pnlFilters" runat="server" CssClass="row g-3 mb-3">
                <!-- City Route Dropdown -->
                <div class="col-md-3">
                    <asp:DropDownList 
                        ID="ddlCityRoute" 
                        runat="server" 
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlCityRoute_SelectedIndexChanged" />
                </div>

                <!-- Town Dropdown -->
                <div class="col-md-3">
                    <asp:DropDownList 
                        ID="ddlTown" 
                        runat="server" 
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlTown_SelectedIndexChanged" />
                </div>

                <!-- PartyType Dropdown -->
                <div class="col-md-3">
                    <asp:DropDownList 
                        ID="ddlPartyType" 
                        runat="server" 
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlPartyType_SelectedIndexChanged" />
                </div>
                <div class="col-md-3">
                    <asp:DropDownList 
                        ID="ddlStatus" 
                        runat="server" 
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">

                        <asp:ListItem Value="">-- Status --</asp:ListItem>
                        <asp:ListItem Text="Active" Value="Active" />
                        <asp:ListItem Text="Dead" Value="Dead" />
                    </asp:DropDownList>
                </div>
                

            </asp:Panel>


            <asp:GridView 
                ID="gvCustomers" 
                runat="server" 
                AutoGenerateColumns="False"
                CssClass="table table-bordered table-hover" 
                OnRowCommand="gvCustomers_RowCommand" 
                OnRowDataBound="gvCustomers_RowDataBound"
                EmptyDataText="No Towns Found" >

                <Columns>
                    <asp:BoundField DataField="CustomerID" HeaderText="Code" DataFormatString="{0:D4}" HtmlEncode="false" />
                    <asp:BoundField DataField="Name" HeaderText="Customer Name" />
                    <asp:BoundField DataField="Contact" HeaderText="Contact" />
                    <asp:BoundField DataField="CNIC" HeaderText="CNIC" />
                    <asp:BoundField DataField="NtnNo" HeaderText="NtnNo" />
                    <asp:BoundField DataField="CityRouteName" HeaderText="Route" />
                    <asp:BoundField DataField="TownName" HeaderText="Town" />
                    <asp:BoundField DataField="PartyType" HeaderText="Party Type" />

                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:DropDownList 
                                ID="ddlActions" 
                                runat="server"
                                CssClass="form-select form-select-sm border-0 bg-transparent custom-dropdown"
                                AutoPostBack="true" 
                                OnSelectedIndexChanged="ddlActions_SelectedIndexChanged">
                                
                                <asp:ListItem Value="">• • •</asp:ListItem>
                                <asp:ListItem Text="Edit" Value="Edit" />
                                <asp:ListItem Text="Delete" Value="Delete" />
                            </asp:DropDownList>

                            <asp:HiddenField 
                                ID="hfCustomerId" 
                                runat="server" 
                                Value='<%# Eval("CustomerId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    

</div>
</asp:Content>
