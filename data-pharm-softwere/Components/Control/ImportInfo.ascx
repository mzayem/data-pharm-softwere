<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportInfo.ascx.cs" Inherits="data_pharm_softwere.Components.Control.ImportInfo" %>

<div class="modal fade" id="ImportInfo" tabindex="-1" aria-labelledby="ImportInfoLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title fw-bold" id="ImportInfoLabel">
                    <asp:Literal ID="litHeader" runat="server" />
                </h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>

            <asp:UpdatePanel ID="upMain" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        <asp:Literal ID="litBody" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <asp:Button 
                            ID="btnSave" 
                            runat="server" 
                            Text="Download Sample CSV" 
                            CssClass="btn btn-primary" 
                            OnClick="btnSave_Click"
                            UseSubmitBehavior="true"
                            CausesValidation="false" />

                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnSave" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>