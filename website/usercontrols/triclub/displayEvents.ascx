<%@ Control Inherits="cFront.Projects.CFSL.Web.UI.UserControls.displayEvents" AutoEventWireup="true" Src="code/displayEvents.cs" %>
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
			<h4><a href='events.aspx?eventID=<%# DataBinder.Eval(Container.DataItem, "IID") %>' title='Click for more info on the <%# DataBinder.Eval(Container.DataItem, "eventTitle") %>'><%# DataBinder.Eval(Container.DataItem, "eventTitle") %></a></h4>
			<span class="event-location"><%# DataBinder.Eval(Container.DataItem, "eventLocation") %></span>
		</td>
		<td class="event-links">
			<a href=""><%# DataBinder.Eval(Container.DataItem, "eventType") %></a> - <a href=""><%# DataBinder.Eval(Container.DataItem, "eventDistance") %></a>
		</td>
	</tr>
</ItemTemplate>
</asp:Repeater>
</table>
<asp:LoginView ID="MemberEventAddView" runat="server">
<LoggedInTemplate>
<div class="member-add-event">
	<p class="title">
		Add a new event
	</p>
		<p class="entry top">
		<label>Event Name:<span class="entry_reqd">*</span></label><asp:TextBox id="txtEventTitle" runat="server" CssClass="txtboxw" />
		<asp:RequiredFieldValidator id="rfvEventTitle" ControlToValidate="txtEventTitle" runat="server" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> Please enter the event name</span>" Display="Dynamic" />
	</p>
	<p class="entry">
		<label>Event Date:<span class="entry_reqd">*</span></label><asp:TextBox id="txtEventDate" runat="server" CssClass="txtbox" />
		<asp:RequiredFieldValidator id="rfvEventDate" ControlToValidate="txtEventDate" runat="server" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> You must enter a date</span>" Display="Dynamic" />
	</p>
	<p class="entry">
		<label>Location:</label><asp:TextBox id="txtEventLocation" runat="server" CssClass="txtboxw" />
	</p>
	<p class="entry">
		<label>Event Type:</label>
		<asp:DropDownList id="ddlEventType" runat="server" CssClass="ddlbox">
			<asp:ListItem Value="1" Text="Triathlon"/>
			<asp:ListItem Value="2" Text="Duathlon"/>
			<asp:ListItem Value="3" Text="Aquathlon"/>
			<asp:ListItem Value="4" Text="Run"/>
			<asp:ListItem Value="5" Text="Swim"/>
			<asp:ListItem Value="6" Text="Cycling - Time Trial"/>
			<asp:ListItem Value="7" Text="Cycling - Road Race"/>
			<asp:ListItem Value="8" Text="Cycling - Cyclocross"/>
			<asp:ListItem Value="9" Text="Other"/>
		</asp:DropDownList>
	</p>
	<p class="entry">
		<label>Event Distance:</label>
		<asp:DropDownList id="ddlEventDistance" runat="server" CssClass="ddlbox">
			<asp:ListItem Value="1" Text="Sprint"/>
			<asp:ListItem Value="2" Text="Olympic"/>
			<asp:ListItem Value="3" Text="Half Ironman"/>
			<asp:ListItem Value="4" Text="Ironman"/>
			<asp:ListItem Value="5" Text="5k"/>
			<asp:ListItem Value="6" Text="10k"/>
			<asp:ListItem Value="7" Text="Half Marathon"/>
			<asp:ListItem Value="8" Text="Marathon"/>
			<asp:ListItem Value="9" Text="25 mile"/>
			<asp:ListItem Value="10" Text="50 mile"/>
			<asp:ListItem Value="11" Text="100 mile"/>
			<asp:ListItem Value="12" Text="Other"/>
		</asp:DropDownList>
	</p>
	<p class="entry">
		<label class="event_description">Description:</label><asp:TextBox id="txtEventDescription" TextMode="MultiLine" runat="server" CssClass="txtboxw" />
	</p>
	<p class="entry">
		<label>Link:</label><asp:TextBox id="txtEventLink" runat="server" CssClass="txtboxw" />
	</p>
	<p class="entry_button">
		<asp:Button id="btnAddEvent" OnClick="addEvent" Text="Add Event" Runat="server" CssClass="submit_button" />
	</p>
</div>
</LoggedInTemplate>
</asp:LoginView>
</asp:PlaceHolder>
<asp:PlaceHolder id="phEventDetail" runat="server">
	<div class="event-detail">
		<h3><%= strEventTitle %>
		<asp:LoginView ID="MemberDoingEvent" runat="server">
			<LoggedInTemplate>
				<asp:Button id="btnAddMemberToEvent" OnClick="addMemberToEvent" Text="I am doing this event!" Runat="server" CssClass="submit_button" CausesValidation="false" />
				<asp:Button id="btnRemoveMemberFromEvent" OnClick="removeMemberFromEvent" Text="I'm not doing this event :-(" Runat="server" Visible="false" CssClass="submit_button" CausesValidation="false" />
			</LoggedInTemplate>
		</asp:LoginView>
		</h3>
		<span class="event-date"><%= strEventDate %></span>
		<hr />
		<label>Type:</label> <%= strEventType %><br />
		<label>Distance:</label> <%= strEventDistance %><br />
		<label>Location:</label> <%= strEventLocation %><br />
		<hr />
		<label>Website:</label> <%= strEventWebsite %><br />
		<hr />
		<label class="event_description">Description:</label> <%= strEventDescription %><br />
		<hr />
		<label>Results:</label> <%= strResults %><br />
		<hr />
		<label class="long-label">Who's doing it?</label><br />
		<asp:Repeater id="WhoElseRepeater" runat="server">
			<HeaderTemplate><ul class="user-doing-event"></HeaderTemplate>
			<ItemTemplate>
				<li><a href='/about-mstc/athletes.aspx?memberID=<%# DataBinder.Eval(Container.DataItem, "[ID]") %>'><%# DataBinder.Eval(Container.DataItem, "[Name]") %></a></li>
			</ItemTemplate>
			<FooterTemplate></ul></FooterTemplate>
		</asp:Repeater>
		<asp:Label id="NooneElseLabel" runat="server" Visible="false">No one has signed up so far!</asp:Label>
		<asp:Label id="lblNoEvent" runat="server" Visible="false" />
	</div>
	<div class="facebook-like">
		<script src="http://connect.facebook.net/en_US/all.js#xfbml=1"></script><fb:like href="" show_faces="false" width="450" font=""></fb:like>
	</div>
	<asp:LoginView ID="MemberAddResultsView" runat="server">
		<LoggedInTemplate>
		<div class="member-add-event">
			<p class="title">
				Add results link to this event
			</p>
			<p class="entry">
				<label>Results Link:</label><asp:TextBox id="txtResultsLink" runat="server" CssClass="txtboxw" />
			</p>
			<p class="entry_button">
				<asp:Button id="btnAddResults" OnClick="addResults" Text="Add Results" Runat="server" CssClass="submit_button" />
			</p>
		</div>
		</LoggedInTemplate>
	</asp:LoginView>
</asp:PlaceHolder>
<asp:Literal id="DebugText" runat="server"/>
