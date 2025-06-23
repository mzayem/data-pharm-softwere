    <%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Layout.Master" AutoEventWireup="true" CodeBehind="BatchPage.aspx.cs" Inherits="data_pharm_softwere.Pages.Batch.BatchPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <div class="container mt-4">
        <h2 class="mb-3">Batch List</h2>

        <asp:HyperLink 
            ID="btnCreateBatch" 
            runat="server" 
            NavigateUrl="/batch/create"
            CssClass="btn btn-primary mb-3">
            + Add New Batch
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
                    <div class="col-md-3">
                        <asp:DropDownList 
                            ID="ddlVendor" 
                            runat="server" 
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged" />
                    </div>

                    <div class="col-md-3">
                        <asp:DropDownList 
                            ID="ddlGroup" 
                            runat="server" 
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" />
                    </div>

                    <div class="col-md-3">
                        <asp:DropDownList 
                            ID="ddlSubGroup" 
                            runat="server" 
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlSubGroup_SelectedIndexChanged" />
                    </div>

                    <div class="col-md-3">
                        <asp:DropDownList 
                            ID="ddlProduct" 
                            runat="server" 
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged" />
                    </div>
                </asp:Panel>

                <asp:GridView 
                    ID="gvBatches" 
                    runat="server" 
                    AutoGenerateColumns="False"
                    CssClass="table table-bordered table-hover" 
                    OnRowCommand="gvBatches_RowCommand"
                    OnRowDataBound="gvBatches_RowDataBound">

                    <Columns>
                        <asp:BoundField DataField="BatchNo" HeaderText="Batch No" />
                        <asp:BoundField DataField="ProductName" HeaderText="Product" />
                        <asp:BoundField DataField="DP" HeaderText="DP" DataFormatString='Rs. {0:N2}' HtmlEncode="false" />
                        <asp:BoundField DataField="TP" HeaderText="TP" DataFormatString='Rs. {0:N2}' HtmlEncode="false" />
                        <asp:BoundField DataField="MRP" HeaderText="MRP" DataFormatString='Rs. {0:N2}' HtmlEncode="false" />
                        <asp:BoundField DataField="MFGDate" HeaderText="MFG Date" DataFormatString="{0:yyyy-MM-dd}" />
                        <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" DataFormatString="{0:yyyy-MM-dd}" />

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
                                    ID="hfBatchID" 
                                    runat="server" 
                                    Value='<%# Eval("BatchID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
