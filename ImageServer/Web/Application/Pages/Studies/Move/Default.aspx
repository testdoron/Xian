<%-- License

Copyright (c) 2011, ClearCanvas Inc.
All rights reserved.
http://www.clearcanvas.ca

This software is licensed under the Open Software License v3.0.
For the complete license, see http://www.clearcanvas.ca/OSLv3.0
--%>

<%@ Page Language="C#" MasterPageFile="~/Pages/Common/MainContentSection.master" AutoEventWireup="true" 
    EnableEventValidation="false" CodeBehind="Default.aspx.cs" Inherits="ClearCanvas.ImageServer.Web.Application.Pages.Studies.Move.Default" 
    Title="Move Studies | ClearCanvas ImageServer" %>

<%@ Register Src="MovePanel.ascx" TagName="MovePanel" TagPrefix="localAsp" %>

<asp:Content runat="server" ID="ContentTitle" ContentPlaceHolderID="TitlePlaceHolder">Move Studies</asp:Content>

<asp:Content runat="server" ID="MainMenuContent" contentplaceholderID="MainMenuPlaceHolder">
    <asp:Table ID="Table1" runat="server" Width="100%" ><asp:TableRow><asp:TableCell HorizontalAlign="right" style="padding-top: 12px;"><asp:LinkButton ID="LinkButton1" runat="server" SkinId="CloseButton" Text="<%$Resources: Labels, Close %>" OnClientClick="javascript: window.close(); return false;" /></asp:TableCell></asp:TableRow></asp:Table>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentSectionPlaceHolder" runat="server">
    <localAsp:MovePanel ID="Move" runat="server"/>
</asp:Content>
