<%@ Control Inherits="cFront.Projects.CFSL.Web.UI.UserControls.displayEventsHome" AutoEventWireup="true" Src="code/displayEventsHome.cs" %>
<asp:Repeater id="rpEventsHome" runat="server" >
<ItemTemplate>
<div class="home-event-list">
	<h3 class="home-event-title"><a href='events.aspx?eventID=<%# DataBinder.Eval(Container.DataItem, "eventID") %>'><%# DataBinder.Eval(Container.DataItem, "eventTitle") %></a></h3>
	<span class="home-event-date"><%# DataBinder.Eval(Container.DataItem, "eventDate", "{0: dd MMM yyyy}") %></span>
	<!--
	<span class="home-event-tags"><a href=""><%# DataBinder.Eval(Container.DataItem, "eventType") %></a> | 
	<a href=""><%# DataBinder.Eval(Container.DataItem, "eventDistance") %></a></span>-->
</div>
</ItemTemplate>
</asp:Repeater>