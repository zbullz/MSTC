<%@ Control Inherits="cFront.Projects.CFSL.Web.UI.UserControls.displayResults" AutoEventWireup="true" Src="code/displayResults.cs" %>
<script>
	$(document).ready(function() {
		$("input[name*='txtEventDate']").datepicker({dateFormat: 'yy-mm-dd'});		
		});
</script>
<asp:PlaceHolder id="phEventsList" runat="server" >
<table id="events-list">
	<tr>
		<th>
			Date
		</th>
		<th>
			Event Name
		</th>
		<th>
			Type - Distance
		</th>
	</tr>
<asp:Repeater id="rpEvents" runat="server" >
<ItemTemplate>
	<tr>
		<td class="event-date">
			<%# DataBinder.Eval(Container.DataItem, "eventDate", "{0: dd MMM yyyy}") %>
		</td>
		<td>
			<h4><a href='results.aspx?eventID=<%# DataBinder.Eval(Container.DataItem, "IID") %>' title='Click for more info on the <%# DataBinder.Eval(Container.DataItem, "eventTitle") %>'><%# DataBinder.Eval(Container.DataItem, "eventTitle") %></a></h4>
			<span class="event-location"><%# DataBinder.Eval(Container.DataItem, "eventLocation") %></span>
		</td>
		<td class="event-links">
			<a href=""><%# DataBinder.Eval(Container.DataItem, "eventType") %></a> - <a href=""><%# DataBinder.Eval(Container.DataItem, "eventDistance") %></a>
		</td>
	</tr>
</ItemTemplate>
</asp:Repeater>
</table>
</asp:PlaceHolder>
<asp:PlaceHolder id="phEventDetail" runat="server">
	<div class="event-detail">
		<h3><%= strEventTitle %>
		</h3>
		<span class="event-date"><%= strEventDate %></span>
		<hr />
		<label>Type:</label> <%= strEventType %><br />
		<label>Distance:</label> <%= strEventDistance %><br />
		<label>Location:</label> <%= strEventLocation %><br />
		<hr />
		<label>Website:</label> <%= strEventWebsite %><br />
		<hr />
		<label>Results:</label> <%= strResults %><br />
		<hr />
		<asp:Label id="lblNoEvent" runat="server" Visible="false" />
	</div>
</asp:PlaceHolder>