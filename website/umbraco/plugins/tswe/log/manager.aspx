<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manager.aspx.cs" Inherits="tswe.log.manager" MasterPageFile="/umbraco/masterpages/umbracoPage.Master"%>
<%@ Register TagPrefix="cc1" Namespace="umbraco.uicontrols" Assembly="controls" %>
<%@ Register src="viewLog.ascx" tagname="viewLog" tagprefix="vl1" %>
<%@ Register src="deleteLog.ascx" tagname="deleteLog" tagprefix="dl1" %>
<asp:Content ID="Content" ContentPlaceHolderID="body" runat="server">
    <cc1:TabView ID="TabView1" runat="server" Width="552px" Height="392px" />
    <vl1:viewLog ID="viewLog" runat="server" />
    <dl1:deleteLog ID="deleteLog" runat="server" />
</asp:Content>    
