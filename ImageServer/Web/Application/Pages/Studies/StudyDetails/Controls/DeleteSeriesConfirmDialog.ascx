<%@ Import namespace="Microsoft.JScript"%>
<%@ Import namespace="ClearCanvas.ImageServer.Web.Common.Utilities"%>

<%@ Import Namespace="ClearCanvas.ImageServer.Web.Application.Helpers" %>
<%@ Control Language="C#" AutoEventWireup="true" Codebehind="DeleteSeriesConfirmDialog.ascx.cs"
    Inherits="ClearCanvas.ImageServer.Web.Application.Pages.Studies.StudyDetails.Controls.DeleteSeriesConfirmDialog" %>

<ccAsp:ModalDialog ID="ModalDialog" runat="server" Title="<%$ Resources:Titles, DeleteSeriesConfirmDialogTitle %>" Width="800px">
    <ContentTemplate>
    
        <script language="javascript" type="text/javascript">
            Sys.Application.add_load(page_load);
            function page_load()
            {            
                var listbox = $get('<%= ReasonListBox.ClientID %>');
                if (document.all) //IE6
                {
                    listbox.attachEvent('onchange', reasonSelectionChanged);
                }
                else //Firefox
                {
                    listbox.addEventListener('onchange', reasonSelectionChanged, false);
                }
            }
            
            function reasonSelectionChanged()
            {
                var listbox = $get('<%= ReasonListBox.ClientID %>');
                var textbox = $get('<%= Reason.ClientID %>');
                textbox.value = listbox.options[listbox.selectedIndex].value;
                
            }
        </script>
    
        <div class="ContentPanel">
        <div class="DialogPanelContent">
            <div id="StudyList">
                <table border="0" cellspacing="5" width="100%">
                    <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="SeriesListingLabel" runat="server" CssClass="DialogTextBoxLabel" 
                                                    Text='<%$ Resources:Labels, DeleteSeriesConfirmDialog_SeriesListingLabel %>'></asp:Label>
                                </td>
                                <td align="right">                                                    
                                    <asp:Label ID="DeleteEntireStudyLabel" runat="server" CssClass="DialogTextBoxLabel" Text='<%$ Resources:Labels, DeleteSeriesConfirmDialog_DeleteEntireStudy %>' Visible="false" ForeColor="red"></asp:Label>                                                    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="DeleteStudiesTableContainer">
                                    <asp:Repeater runat="server" ID="SeriesListing">
                                        <HeaderTemplate>
                                            <table  cellspacing="0" width="100%" class="DeleteStudiesConfirmTable">
                                                <tr>
                                                    <th style="white-space:nowrap" class="GlobalGridViewHeader" align="Center">
                                                        Series #</th>
                                                    <th style="white-space:nowrap" class="GlobalGridViewHeader" align="center">
                                                        Modality</th>
                                                    <th style="white-space:nowrap" class="GlobalGridViewHeader">
                                                        Description</th>
                                                    <th style="white-space:nowrap" class="GlobalGridViewHeader" align="Center">
                                                        Instances</th>
                                                    <th style="white-space:nowrap" class="GlobalGridViewHeader">
                                                        Series Instance UID</th>
                                                    <th style="white-space:nowrap" class="GlobalGridViewHeader">
                                                        Performed On</th>
                                                </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="GlobalGridViewRow">
                                                <td align="center">
                                                    <%# Eval("SeriesNumber") %>
                                                </td>
                                                <td align="center">
                                                    <%# Eval("Modality") %>
                                                </td>
                                                <td>
                                                    <%# Eval("Description") %>&nbsp;
                                                </td>
                                                <td align="Center">
                                                    <%# Eval("NumberOfSeriesRelatedInstances") %>
                                                </td>
                                                <td>
                                                    <%# Eval("SeriesInstanceUID") %>
                                                </td>
                                                <td>
                                                    <ccUI:DALabel ID="PerformedDate" runat="server" Text="{0}" Value='<%# Eval("PerformedProcedureStepStartDate") %>' InvalidValueText="<i style='color:red'>[Invalid date:{0}]</i>" EmptyValueText="&nbsp;"></ccUI:DALabel>
                                                    <ccUI:TMLabel ID="PerformedTime" runat="server" Text="{0}" Value='<%# Eval("PerformedProcedureStepStartTime") %>' InvalidValueText="<i style='color:red'>[Invalid time:{0}]</i>" EmptyValueText="&nbsp;"></ccUI:TMLabel>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                </table>
            </div>
            <div id="ReasonPanel">
                <table border="0">
                    
                    <tr valign="top">
                        <td>
                            <asp:Label ID="Label3" runat="server" CssClass="DialogTextBoxLabel" 
                                            Text='<%$ Resources:Labels, DeleteSeriesConfirmReasonLabel %>'></asp:Label> 
                             
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr valign="top">
                                    <td>
                                        <asp:TextBox  Width="400px" Rows="3" ID="Reason" runat="server" TextMode="MultiLine" style="font-family: Arial, Sans-Serif; font-size: 14px;" />                                            
                                    </td>
                                    <td>
                                        <ccAsp:InvalidInputIndicator ID="InvalidReasonIndicator" runat="server" SkinID="InvalidInputIndicator" />
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ID="PredefinedReasonButton" OnClientClick="return false;" Text="..." ></asp:Button>
                                        <asp:Panel runat="server" ID="ReasonForDeletionDropDownPanel" CssClass="ReasonForDeletionDropDownPanel">
                                        <asp:Label ID="Label2" runat="server" ForeColor="#16425D" style="font-family: Arial, Sans-Serif; font-size: 14px;"
                                                Text="Pre-defined Reasons:"></asp:Label><br />                            
                                        <asp:DropDownList runat="server" ID="ReasonListBox" style="font-family: Arial, Sans-Serif; font-size: 14px;"/>                                        
                                        </asp:Panel>                                        
                                    </td>
                                </tr>
                            </table>
                            
                            
                        </td>
                    </tr>
                    <tr id="ReasonSavePanel">
                        <td>
                            <asp:Label ID="Label4" runat="server" CssClass="DialogTextBoxLabel" 
                                                Text="Save this reason as"></asp:Label> 
                                 
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="SaveReasonAsName" style="font-family: Arial, Sans-Serif; font-size: 14px;"/>
                        </td>
                </tr>
                </table>
            </div>                
        </div>
        
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr align="right">
                <td>
                    <asp:Panel ID="Panel1" runat="server" CssClass="DefaultModalDialogButtonPanel">
                        <ccUI:ToolbarButton ID="DeleteSeriesButton" runat="server" SkinID="OKButton" 
                           OnClick="DeleteSeriesButton_Clicked"  />
                        <ccUI:ToolbarButton ID="CancelButton" runat="server" SkinID="CancelButton"
                            OnClick="CancelButton_Clicked" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
        </div>
       <ccValidator:ConditionalRequiredFieldValidator ID="ReasonValidator" runat="server"
                                                ControlToValidate="Reason" InvalidInputIndicatorID="InvalidReasonIndicator" 
                                                ValidationGroup="<%= ClientID %>"
                                                Text="You must specify the reason for deleting the studies for future auditing purpose." Display="None" InvalidInputColor="#FAFFB5"></ccValidator:ConditionalRequiredFieldValidator>
       
       <aspAjax:PopupControlExtender ID="PopEx" runat="server"
                        TargetControlID="PredefinedReasonButton"
                        PopupControlID="ReasonForDeletionDropDownPanel"
                        Position="Right"  />
    
    </ContentTemplate>
</ccAsp:ModalDialog>