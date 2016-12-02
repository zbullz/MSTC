<%@ Control Language="C#" AutoEventWireup="true" CodeFile="entry.ascx.cs" Inherits="usercontrols_triclub_entry" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<asp:PlaceHolder id="containerEntryFull" runat="server" Visible="False" >
	<p class="entry_sf">
		The event is now full!
	</p>
	<p class="entry_sf">
		If you would like to be added to the waiting list then please <a href="mailto:MSTCwebguys@gmail.com">contact us</a>
	</p>
	<p class="entry_sf">
		The waiting list is organised on a first come first served basis. If someone pulls out the first person on the list will be offered the place. 
	</p>
	<p class="entry_sf">
		&nbsp;
	</p>
</asp:PlaceHolder>
<asp:PlaceHolder id="containerEntry" runat="server" Visible="True" >
	<h3>
		About You
	</h3>
	<p class="entry top">
	    <label>First Name:<span class="entry_reqd">*</span></label><asp:TextBox id="txtFirstName" runat="server" CssClass="txtbox" />
		<asp:RequiredFieldValidator id="rfvFname" ControlToValidate="txtFirstName" runat="server" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> Please enter your first name</span>" Display="Dynamic" />
	</p>
	<p class="entry">
		<label>Last Name:<span class="entry_reqd">*</span></label><asp:TextBox id="txtLastName" runat="server" CssClass="txtbox" />
		<asp:RequiredFieldValidator id="rfvSname" ControlToValidate="txtLastName" runat="server" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> Please enter your last name</span>" Display="Dynamic" />
	</p>
	<p class="entry">
		<label>Date of Birth:<span class="entry_reqd">*</span></label>
		<asp:DropDownList id="ddlDateDay" runat="server" CssClass="ddlbox" />
		<asp:DropDownList id="ddlDateMonth" runat="server" CssClass="ddlbox" />
		<asp:DropDownList id="ddlDateYear" runat="server" CssClass="ddlbox" /><span class="entry_note">day / month / year</span>
	</p>
	<p class="entry">
		<label>Gender:<span class="entry_reqd">*</span></label>
		<asp:DropDownList id="ddlGender" runat="server" CssClass="ddlbox">
			<asp:ListItem Selected="True" Value="M" Text="MALE"/>
			<asp:ListItem Value="F" Text="FEMALE"/>
		</asp:DropDownList>
	</p>
	<h3>
		Your Contact Details
	</h3>
	<p class="entry">
		<label>Address:<span class="entry_reqd">*</span></label><asp:TextBox id="txtAddr1" runat="server" CssClass="txtboxw" />
		<asp:RequiredFieldValidator id="rfvAddr1" ControlToValidate="txtAddr1" runat="server" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> Please enter your house number/name and street</span>" Display="Dynamic" />
		<span class="entry_note">House number/name and street</span>
	</p>
	<p class="entry">
		<label>&nbsp;</label><asp:TextBox id="txtAddr2" runat="server" CssClass="txtboxw" />
		<span class="entry_note">(Optional)</span>
	</p>
	<p class="entry">
		<label>Town/City:<span class="entry_reqd">*</span></label><asp:TextBox id="txtAddr3" runat="server" CssClass="txtbox" />
		<asp:RequiredFieldValidator id="rfvAddr3" ControlToValidate="txtAddr3" runat="server" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> Please enter your town/city</span>" Display="Dynamic" />
	</p>
	<p class="entry">
		<label>County:</label><asp:TextBox id="txtAddr4" runat="server" CssClass="txtbox" />
	</p>
	<p class="entry">
		<label>Postcode:<span class="entry_reqd">*</span></label><asp:TextBox id="txtPostcode" runat="server" CssClass="txtboxs" />
		<asp:RequiredFieldValidator id="rfvPostcode" ControlToValidate="txtPostcode" runat="server" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> Please enter your postcode</span>" Display="Dynamic" />
	</p>
	<p class="entry">
		<label>Phone:</label><asp:TextBox id="txtPhone" runat="server" CssClass="txtbox" />
	</p>
	<p class="entry">
		<label>Email:<span class="entry_reqd">*</span></label><asp:TextBox id="txtEmail" runat="server" CssClass="txtboxw" />
		<asp:RequiredFieldValidator id="rfvtxtEmail" ControlToValidate="txtEmail" runat="server" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> Please enter your email address</span>" Display="Dynamic" />
        
        
	</p>
    <p class="entry email-confirm">
        <label>Email confirm:<span class="entry_reqd">*</span></label>
        <asp:TextBox id="txtEmailConfirm" runat="server" CssClass="txtbox" AutoCompleteType="None" />
    </p>
	<h3>
		Event Stuff
	</h3>
	<h4>
		Which race would you like to enter?
	</h4>
	<ul class="event-entry-type">
		<li>
			<asp:RadioButton id="rbFull" Text="Full Triathlon" Checked="True" GroupName="eventType" runat="server" />
		</li>
		<li>
			<asp:RadioButton id="rbAquabike" Text="Aquabike" Checked="False" GroupName="eventType" runat="server" />
		</li>
	</ul>
	<p class="entry">
		<label>BTF Number:</label><asp:TextBox id="txtBTF" runat="server" CssClass="txtboxs" />
		<span class="entry_note">If you're a member.</span>
	</p>
	<p class="entry">
		<label>Club:</label><asp:TextBox id="txtClub" runat="server" CssClass="txtboxw" />
		<span class="entry_note">If you belong to one.</span>
	</p>
	<p class="entry">
		<label>Est. 400m swim time:<span class="entry_reqd">*</span></label>
		<asp:DropDownList id="ddlSwimMins" runat="server">
			<asp:ListItem>04</asp:ListItem>
			<asp:ListItem>05</asp:ListItem>
			<asp:ListItem>06</asp:ListItem>
			<asp:ListItem>07</asp:ListItem>
			<asp:ListItem>08</asp:ListItem>
			<asp:ListItem>09</asp:ListItem>
			<asp:ListItem>10</asp:ListItem>
			<asp:ListItem>11</asp:ListItem>
			<asp:ListItem>12</asp:ListItem>
			<asp:ListItem>13</asp:ListItem>
			<asp:ListItem>14</asp:ListItem>
			<asp:ListItem>15</asp:ListItem>
			<asp:ListItem>16</asp:ListItem>
			<asp:ListItem>17</asp:ListItem>
			<asp:ListItem>18</asp:ListItem>
			<asp:ListItem>19</asp:ListItem>
			<asp:ListItem>20</asp:ListItem>
		</asp:DropDownList>&nbsp;<span class="swim">Mins</span>&nbsp;
		<asp:DropDownList id="ddlSwimSecs" runat="server">
			<asp:ListItem>00</asp:ListItem>
			<asp:ListItem>15</asp:ListItem>
			<asp:ListItem>30</asp:ListItem>
			<asp:ListItem>45</asp:ListItem>
		</asp:DropDownList>&nbsp;<span class="swim">Secs</span>
		<span class="entry_note">Please be as accurate as you can with this.</span>
	</p>
	<h3>
		Terms &#038; Conditions
	</h3>
	<p class="entry">
		<asp:CheckBox id="cbAgree" runat="server" /> I have read and agree to the <a href="race-entry/terms-conditions.aspx" target="_blank">race terms and conditions</a>.
		<asp:Label id="lblTerms" runat="server" Display="Dynamic" />
	</p>
	<p class="entry_button">
		<asp:Button id="btnEntry" OnClick="addEntry" Text="Proceed to payment >" Runat="server" CssClass="entry_button" />
	</p>
</asp:PlaceHolder>