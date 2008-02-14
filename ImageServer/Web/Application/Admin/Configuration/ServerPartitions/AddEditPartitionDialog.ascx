<%@ Control Language="C#" AutoEventWireup="true" Inherits="ClearCanvas.ImageServer.Web.Application.Admin.Configuration.ServerPartitions.AddEditPartitionDialog"
    Codebehind="AddEditPartitionDialog.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="ClearCanvas.ImageServer.Web.Common" Namespace="ClearCanvas.ImageServer.Web.Common.WebControls.Validators"
    TagPrefix="CCValidators" %>
<%@ Register Src="~/Common/InvalidInputIndicator.ascx" TagName="InvalidInputIndicator"
    TagPrefix="CCCommon" %>
    
<asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="DialogPanel" runat="server" CssClass="CSSPopupWindow" Style="display: none"
            Width="440px">
            <asp:Panel ID="TitleBarPanel" runat="server" CssClass="CSSPopupWindowTitleBar">
                <table width="100%">
                    <tr>
                        <td valign="middle">
                            <asp:Label ID="TitleLabel" runat="server" EnableViewState="False" Text="Add Partition"></asp:Label></td>
                    </tr>
                </table>
            </asp:Panel>
            <div class="CSSPopupWindowBody">
                <asp:Panel ID="Panel3" runat="server" Height="100%" CssClass="CSSDialogTabPanelContent">
                    <ajaxToolkit:TabContainer ID="ServerPartitionTabContainer" runat="server" ActiveTabIndex="0"
                        CssClass="CSSDialogTabControl">
                        <ajaxToolkit:TabPanel ID="GeneralTabPanel" runat="server" HeaderText="GeneralTabPanel"
                            CssClass="CSSTabPanel">
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server" CssClass="CSSDialogTabPanelContent">
                                    <table id="GeneralTabTable" runat="server">
                                        <tr runat="server" align="left">
                                            <td runat="server">
                                                <table width="100%">
                                                    <tr align="left">
                                                        <td width="100%">
                                                            <asp:Label ID="Label4" runat="server" Text="AE Title" CssClass="CSSTextLabel" /><br />
                                                            <asp:TextBox ID="AETitleTextBox" runat="server" MaxLength="16" ValidationGroup="vg1"
                                                                ToolTip="The DICOM Application Entity Title for the partition."></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <CCCommon:InvalidInputIndicator ID="AETitleHelp" runat="server" ImageUrl="~/images/icons/HelpSmall.png">
                                                            </CCCommon:InvalidInputIndicator>
                                                            <CCValidators:ConditionalRequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                                ControlToValidate="AETitleTextBox" Display="None" EnableClientScript="true" ErrorMessage="AE Title is required"
                                                                InvalidInputColor="#FAFFB5" ValidationGroup="vg1" InvalidInputIndicatorID="AETitleHelp"></CCValidators:ConditionalRequiredFieldValidator>
                                                            <CCValidators:RegularExpressionFieldValidator ID="RegularExpressionFieldValidator2"
                                                                runat="server" ControlToValidate="AETitleTextBox" Display="None" ErrorMessage="The AE Title is not valid."
                                                                InvalidInputColor="#FAFFB5" ValidationExpression="^([^\\]){1,16}$" ValidationGroup="vg1"
                                                                InvalidInputIndicatorID="AETitleHelp"></CCValidators:RegularExpressionFieldValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td runat="server" align="left">
                                                <table width="100%">
                                                    <tr align="left">
                                                        <td width="100%">
                                                            <asp:Label ID="Label1" runat="server" Text="Description" CssClass="CSSTextLabel" /><br />
                                                            <asp:TextBox ID="DescriptionTextBox" runat="server" ToolTip="A textual description of the partition."></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr runat="server" align="left">
                                            <td runat="server">
                                                <table width="100%">
                                                    <tr align="left">
                                                        <td width="100%">
                                                            <asp:Label ID="Label2" runat="server" Text="Port" CssClass="CSSTextLabel" /><br />
                                                            <asp:TextBox ID="PortTextBox" runat="server"></asp:TextBox>
                                                            <CCValidators:RangeValidator ID="PortValidator1" runat="server" ControlToValidate="PortTextBox"
                                                                InvalidInputColor="#FAFFB5" ValidationGroup="vg1" MinValue="1" MaxValue="65535"
                                                                ErrorMessage="Partition Port must be between 1 and 65535" Display="None" InvalidInputIndicatorID="PortHelp"></CCValidators:RangeValidator>
                                                        </td>
                                                        <td>
                                                            <CCCommon:InvalidInputIndicator ID="PortHelp" runat="server" ImageUrl="~/images/icons/HelpSmall.png">
                                                            </CCCommon:InvalidInputIndicator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td runat="server">
                                                <table width="100%">
                                                    <tr align="left">
                                                        <td>
                                                            <asp:Label ID="Label3" runat="server" Text="Folder Name" CssClass="CSSTextLabel" /><br />
                                                            <asp:TextBox ID="PartitionFolderTextBox" runat="server" CausesValidation="true" ValidationGroup="vg1"
                                                                ToolTip="A unique folder name to store images within for the partition."></asp:TextBox>
                                                            <CCValidators:ConditionalRequiredFieldValidator ID="Conditionalrequiredfieldvalidator1"
                                                                runat="server" ControlToValidate="PartitionFolderTextBox" Display="None" EnableClientScript="true"
                                                                ErrorMessage="Folder Name is required" InvalidInputColor="#FAFFB5" ValidationGroup="vg1"
                                                                InvalidInputIndicatorID="FolderHelp"></CCValidators:ConditionalRequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                            <CCCommon:InvalidInputIndicator ID="FolderHelp" runat="server" ImageUrl="~/images/icons/HelpSmall.png">
                                                            </CCCommon:InvalidInputIndicator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr runat="server" align="left">
                                            <td runat="server">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="100%">
                                                            <asp:CheckBox ID="EnabledCheckBox" runat="server" Checked="True" Text="Enabled" ToolTip="Enable or Disable DICOM connections to the partition." />
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td runat="server" valign="top">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </ContentTemplate>
                            <HeaderTemplate>
                                General
                            </HeaderTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" runat="server" CssClass="CSSDialogTabPanelContent">
                                    <table width="100%">
                                        <tr>
                                            <td align="left">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="AcceptAnyDeviceCheckBox" runat="server" Text="Accept Any Device"
                                                                ToolTip="Accept DICOM Associations from any device to this partition." />
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="AutoInsertDeviceCheckBox" runat="server" Text="Auto Insert Devices"
                                                                ToolTip="Automatically add devices when they connect to this partition." />
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label5" runat="server" Text="Default Remote Port" CssClass="CSSTextLabel" /><br />
                                                            <asp:TextBox ID="DefaultRemotePortTextBox" runat="server"></asp:TextBox>
                                                            <td>
                                                                <CCCommon:InvalidInputIndicator ID="DefaultPortHelp" runat="server" ImageUrl="~/images/icons/HelpSmall.png">
                                                                </CCCommon:InvalidInputIndicator>
                                                                <CCValidators:RangeValidator ID="DefaultRemotePortRangeValidator" runat="server" ControlToValidate="DefaultRemotePortTextBox"
                                                                    InvalidInputColor="#FAFFB5" ValidationGroup="vg1" MinValue="1" MaxValue="65535"
                                                                    ErrorMessage="Remote device default port must be between 1 and 65535" Display="None"
                                                                    InvalidInputIndicatorID="DefaultPortHelp" />
                                                            </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </ContentTemplate>
                            <HeaderTemplate>
                                Remote Devices
                            </HeaderTemplate>
                        </ajaxToolkit:TabPanel>
                    </ajaxToolkit:TabContainer>
                </asp:Panel>
                <center>
                    <br />
                    <table width="80%">
                        <tr align="center">
                            <td>
                                <asp:Button ID="OKButton" runat="server" Text="Add" Width="77px" OnClick="OKButton_Click"
                                    ValidationGroup="vg1" />
                            </td>
                            <td>
                                <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
                            </td>
                        </tr>
                    </table>
                    <br />
                </center>
                <asp:Panel ID="DummyPanel" runat="server" Height="1px" Style="z-index: 101; left: 522px;
                    position: absolute; top: 53px" Width="36px">
                </asp:Panel>
            </div>
        </asp:Panel>
        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="CSSModalBackground"
            BehaviorID="MyStupidExtender" Enabled="true" PopupControlID="DialogPanel" TargetControlID="DummyPanel">
        </ajaxToolkit:ModalPopupExtender>
    </ContentTemplate>
</asp:UpdatePanel>
