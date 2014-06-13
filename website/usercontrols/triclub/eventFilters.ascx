<%@ Control Inherits="cFront.Projects.CFSL.Web.UI.UserControls.eventFilters" AutoEventWireup="true" Src="code/eventFilters.cs" %>
<p class="event-filter-title">
	Filter by type
</p>
<ul class="event-filter-list">
<asp:Repeater id="rpEventFilterType" runat="server" >
<ItemTemplate>
	<li>
		<a href='?eventType=<%# DataBinder.Eval(Container.DataItem, "eventTypeID") %>' title='Click to view <%# DataBinder.Eval(Container.DataItem, "eventTypeDefinition") %> events'><%# DataBinder.Eval(Container.DataItem, "eventTypeDefinition") %></a>
	</li>
</ItemTemplate>
</asp:Repeater>
</ul>
<p class="event-filter-title">
	...or distance
</p>
<ul class="event-filter-list">
<asp:Repeater id="rpEventFilterDistance" runat="server">
<ItemTemplate>
	<li>
		<a href='?eventDistance=<%# DataBinder.Eval(Container.DataItem, "eventDistanceID") %>' title='Click to view <%# DataBinder.Eval(Container.DataItem, "eventDistanceDefinition") %> events'><%# DataBinder.Eval(Container.DataItem, "eventDistanceDefinition") %></a>
	</li>
</ItemTemplate>
</asp:Repeater>
</ul>
<p class="event-filter-link">
	<a href='?eventIsOld=1'>Get past events</a>
</p>