<%@ Control Language="C#" AutoEventWireup="true" CodeFile="adminEntries.ascx.cs" Inherits="usercontrols_triclub_adminEntries" %>
<p class="entry_sf_excl">
	Number of confirmed entries: <span class="red"><%= intEntries %></span>
</p>
<table>
	<tr>
		<th>
			Name
		</th>
		<th>
			Club
		</th>
	</tr>
<asp:Repeater id="rpEntries" runat="server" >
<ItemTemplate>
	<tr>
		<td>
			<%# DataBinder.Eval(Container.DataItem, "strLastName") %>, <%# DataBinder.Eval(Container.DataItem, "strFirstName") %>
		</td>
		<td>
			<%# DataBinder.Eval(Container.DataItem, "strClub") %>
		</td>
	</tr>
</ItemTemplate>
</asp:Repeater>
</table>