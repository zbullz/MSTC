<%@ Control Inherits="cFront.Projects.CFSL.Web.UI.UserControls.EntriesSoFar" AutoEventWireup="true" Src="code/entriesSoFar.cs" %>
<table class="entries-so-far">
	<tr>
		<th class="entry-name">
			Name
		</th>
		<th>
			Club
		</th>
		<th>
			Est. Swim Time (400m)
		</th>
		<th>
			Event Entered
		</th>
	</tr>
<asp:Repeater id="rpEntries" runat="server" >
<ItemTemplate>
	<tr>
		<td class="entry-name">
			<%# DataBinder.Eval(Container.DataItem, "ULastName") %>, <%# DataBinder.Eval(Container.DataItem, "FirstName") %>
		</td>
		<td>
			<%# DataBinder.Eval(Container.DataItem, "Club") %>
		</td>
		<td>
			<%# DataBinder.Eval(Container.DataItem, "Mins") %>:<%# DataBinder.Eval(Container.DataItem, "Secs") %>
		</td>
		<td>
			<%# DataBinder.Eval(Container.DataItem, "EventType") %>
		</td>
	</tr>
</ItemTemplate>
</asp:Repeater>
</table>
<p>
	Total: <%= intEntries %>
</p
