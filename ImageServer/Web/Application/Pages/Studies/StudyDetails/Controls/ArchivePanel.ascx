<%-- License

Copyright (c) 2011, ClearCanvas Inc.
All rights reserved.
http://www.clearcanvas.ca

This software is licensed under the Open Software License v3.0.
For the complete license, see http://www.clearcanvas.ca/OSLv3.0
--%>

<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ArchivePanel.ascx.cs"
	Inherits="ClearCanvas.ImageServer.Web.Application.Pages.Studies.StudyDetails.Controls.ArchivePanel" %>
<%@ Import Namespace="ClearCanvas.ImageServer.Web.Application.App_GlobalResources" %>

<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>
<table border="0" cellspacing="0" width="100%">
	<tr>
		<td>
			<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<div class="StudyDetailsSubTitle"><%= Labels.StudyDetails_ArchivePanel_ArchiveQueue%></div>
<asp:Table runat="server" ID="ContainerTable" Height="100%" CellPadding="0" CellSpacing="0"
	Width="100%" style="border: solid 1px #3D98D1;">
	<asp:TableRow VerticalAlign="top">
		<asp:TableCell VerticalAlign="top">     

					<ccUI:GridView ID="ArchiveQueueGridView" runat="server" 
						OnPageIndexChanged="ArchiveQueueGridView_PageIndexChanged"
						OnPageIndexChanging="ArchiveQueueGridView_PageIndexChanging" SelectionMode="Disabled"
						MouseHoverRowHighlightEnabled="false" GridLines="Horizontal" BackColor="White">
						<Columns>
							<asp:BoundField DataField="ArchiveQueueStatusEnum" HeaderText="<%$Resources: ColumnHeaders, ArchiveQueueStatus %>">
								<headerstyle wrap="False" />
							</asp:BoundField>
							<asp:TemplateField HeaderText="<%$Resources: ColumnHeaders, ArchiveQueueScheduledTime %>">
								<itemtemplate>
                                    <ccUI:DateTimeLabel ID="ScheduledTime" runat="server" Value='<%# Eval("ScheduledTime") %>' ></ccUI:DateTimeLabel>
                                </itemtemplate>
							</asp:TemplateField>
							<asp:BoundField DataField="ProcessorID" HeaderText="<%$Resources: ColumnHeaders, ArchiveQueueProcessorID %>">
								<headerstyle wrap="False" />
							</asp:BoundField>
						</Columns>
						<EmptyDataTemplate>
							<asp:Table ID="Table1" runat="server" Width="100%" CellPadding="0" CellSpacing="0">
								<asp:TableHeaderRow CssClass="GlobalGridViewHeader">
									<asp:TableHeaderCell><%= ColumnHeaders.ArchiveQueueStatus %></asp:TableHeaderCell>
									<asp:TableHeaderCell><%= ColumnHeaders.ArchiveQueueScheduledTime%></asp:TableHeaderCell>
									<asp:TableHeaderCell><%= ColumnHeaders.ArchiveQueueProcessorID %></asp:TableHeaderCell>
								</asp:TableHeaderRow>
								<asp:TableRow>
									<asp:TableCell ColumnSpan="3" Height="50" HorizontalAlign="Center">
										<asp:Panel ID="Panel1" runat="server" CssClass="GlobalGridViewEmptyText">
											<%= SR.StudyDetails_NoArchiveQueueItemForThisStudy %></asp:Panel>
									</asp:TableCell>
								</asp:TableRow>
							</asp:Table>
						</EmptyDataTemplate>
						<RowStyle CssClass="GlobalGridViewRow" />
						<HeaderStyle CssClass="GlobalGridViewHeader" />
						<AlternatingRowStyle CssClass="GlobalGridViewAlternatingRow" />
						<SelectedRowStyle CssClass="GlobalGridViewSelectedRow" />
					</ccUI:GridView>
                    		</asp:TableCell>
	</asp:TableRow>
</asp:Table>
				</ContentTemplate>
			</asp:UpdatePanel>
		</td>
	</tr>
	<tr>
		<td>
			<div class="StudyDetailsSubTitle" style="margin-top: 9px;"><%= Labels.StudyDetails_ArchivePanel_ArchiveStudyStorage%></div>
			<asp:Table runat="server" ID="Table2" Height="100%" CellPadding="0" CellSpacing="0"
	Width="100%" style="border: solid 1px #3D98D1;">
	<asp:TableRow VerticalAlign="top">
		<asp:TableCell VerticalAlign="top">
			<ccUI:GridView ID="ArchiveStudyStorageGridView" runat="server" 
				SelectionMode="Disabled"
				MouseHoverRowHighlightEnabled="false" GridLines="Horizontal" BackColor="White"
				OnRowDataBound="ArchiveStudyStorageGridView_RowDataBound">
				<Columns>
					<asp:TemplateField HeaderText="<%$Resources: ColumnHeaders, ArchiveStorageTransferSyntax %>">
						<itemtemplate>
                <asp:Label ID="ServerTranseferSyntax" runat="server"></asp:Label>
            </itemtemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="<%$Resources: ColumnHeaders, ArchiveStorageArchiveTime%>">
						<itemtemplate>
                <ccUI:DateTimeLabel ID="ArchiveTime" runat="server" Value='<%# Eval("ArchiveTime") %>' ></ccUI:DateTimeLabel>
            </itemtemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="<%$Resources: ColumnHeaders, ArchiveStorageArchiveXML %>">
						<itemtemplate>
                <asp:Label ID="XmlText" runat="server"></asp:Label>
            </itemtemplate>
					</asp:TemplateField>
				</Columns>
				<EmptyDataTemplate>
					<asp:Table ID="Table1" runat="server" Width="100%" CellPadding="0" CellSpacing="0">
						<asp:TableHeaderRow CssClass="GlobalGridViewHeader">
							<asp:TableHeaderCell><%= ColumnHeaders.ArchiveStorageTransferSyntax%></asp:TableHeaderCell>
							<asp:TableHeaderCell><%= ColumnHeaders.ArchiveStorageArchiveTime%></asp:TableHeaderCell>
							<asp:TableHeaderCell><%= ColumnHeaders.ArchiveStorageArchiveXML%></asp:TableHeaderCell>
						</asp:TableHeaderRow>
						<asp:TableRow>
							<asp:TableCell ColumnSpan="3" Height="50" HorizontalAlign="Center">
								<asp:Panel ID="Panel1" runat="server" CssClass="GlobalGridViewEmptyText">
									<%= SR.StudyDetails_NoArchiveStudyStorageForThisStudy %></asp:Panel>
							</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
				</EmptyDataTemplate>
				<RowStyle CssClass="GlobalGridViewRow" />
				<HeaderStyle CssClass="GlobalGridViewHeader" />
				<AlternatingRowStyle CssClass="GlobalGridViewAlternatingRow" />
				<SelectedRowStyle CssClass="GlobalGridViewSelectedRow" />
			</ccUI:GridView>
                    		</asp:TableCell>
	</asp:TableRow>
</asp:Table>
		</td>
	</tr>
</table>
