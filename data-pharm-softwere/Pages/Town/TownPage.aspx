<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="TownPage.aspx.cs" Inherits="data_pharm_softwere.Pages.Town.TownPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server" />

<div class="container mt-4">
    <h2 class="mb-3">Route List</h2>

    <asp:HyperLink 
        ID="btnCreateBatch" 
        runat="server" 
        NavigateUrl="/town/create"
        CssClass="btn btn-primary mb-3">
        + Add New Town
    </asp:HyperLink>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:TextBox 
                ID="txtSearch" 
                runat="server" 
                CssClass="form-control mb-3"
                Placeholder="Search by product, vendor, or group" 
                AutoPostBack="true" 
                OnTextChanged="txtSearch_TextChanged" />
            <asp:Panel ID="pnlFilters" runat="server" CssClass="row g-3 mb-3">
                <!-- cityRoutes Dropdown -->
                <div class="col-md-4">
                    <asp:DropDownList 
                        ID="ddlCityRoute" 
                        runat="server" 
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlCityRoute_SelectedIndexChanged" />
                </div>
            </asp:Panel>

            <asp:GridView 
                ID="gvBatches" 
                runat="server" 
                AutoGenerateColumns="False"
                CssClass="table table-bordered table-hover" 
                OnRowCommand="gvBatches_RowCommand"
                OnRowDataBound="gvBatches_RowDataBound"
                EmptyDataText="No Towns Found">

                <Columns>
                    <asp:BoundField DataField="TownID" HeaderText="Town No" />
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="CityRouteName" HeaderText="Route" />
                    <asp:BoundField DataField="CreatedAt" HeaderText="Created At" DataFormatString="{0:yyyy-MM-dd}" />

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
                                ID="hfTownID"
                                runat="server" 
                                Value='<%# Eval("TownID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
