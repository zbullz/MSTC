<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UDT_adminAddEvents.ascx.cs" Inherits="usercontrols.triclub.usercontrols_triclub_UDT_adminAddEvents" %>
<script>
    $(document).ready(function () {
        $("input[name*='txtEventDate']").datepicker({ dateFormat: 'yy-mm-dd' });
    });
</script>
<asp:PlaceHolder id="containerEntry" runat="server" Visible="True" >	
	<p class="entry top">
		<label>Event Name:<span class="entry_reqd">*</span></label><asp:TextBox id="txtEventTitle" runat="server" CssClass="txtbox" />
		<asp:RequiredFieldValidator id="rfvEventTitle" ControlToValidate="txtEventTitle" runat="server" ValidationGroup="valAddEntry" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> Please enter the event name</span>" Display="Dynamic" />
	</p>
	<p class="entry">
		<label>Event Date:<span class="entry_reqd">*</span></label><asp:TextBox id="txtEventDate" runat="server" CssClass="txtbox" />
		<asp:RequiredFieldValidator id="rfvEventDate" ControlToValidate="txtEventDate" runat="server" ValidationGroup="valAddEntry"
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
		<label>Description:</label><asp:TextBox id="txtEventDescription" TextMode="MultiLine" runat="server" CssClass="txtboxw" />
	</p>
	<p class="entry">
		<label>Link:</label><asp:TextBox id="txtEventLink" runat="server" CssClass="txtboxw" />
	</p>
	<p class="entry">
		<label>Results:</label><asp:TextBox id="txtResultsLink" runat="server" CssClass="txtboxw" />
	</p>
	<p class="entry">
		<label>News Report:</label><asp:TextBox id="txtNewsLink" runat="server" CssClass="txtboxw" />
	</p>
	<p class="entry">
		<label>Photos:</label><asp:TextBox id="txtPhotoLink" runat="server" CssClass="txtboxw" />
	</p>
	<p class="entry_button">
		<asp:Button id="btnAddEvent" ValidationGroup="valAddEntry" OnClick="addEvent" Text="Add Event" Runat="server" CssClass="submit_button" />
	</p>
</asp:PlaceHolder>
