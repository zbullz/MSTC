<%@ Control Inherits="cFront.Projects.CFSL.Web.UI.UserControls.EntryForm" AutoEventWireup="true" Src="code/gotri_entry.cs" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<form id="juniorEntry" runat="server">
<asp:PlaceHolder id="containerEntryClosed" runat="server" Visible="False" >
	<p class="entry_sf">
		Entry is now closed
	</p>
</asp:PlaceHolder>
<asp:PlaceHolder id="containerEntry" runat="server" Visible="True" >
	<h1>Registration Form – GoTri! Junior Training Camp</h1>
	<h2>
		Young Person / Triathlete details:
	</h2>
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
		<label>Age:<span class="entry_reqd">*</span></label><asp:TextBox id="txtAge" runat="server" CssClass="txtboxS" />
		<asp:RequiredFieldValidator id="rfvAge" ControlToValidate="txtAge" runat="server" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> Please enter age</span>" Display="Dynamic" />
	</p>
	
	<p class="entry">
		<label>Gender:<span class="entry_reqd">*</span></label>
		<asp:DropDownList id="ddlGender" runat="server" CssClass="ddlbox">
			<asp:ListItem Selected="True" Value="M" Text="MALE"/>
			<asp:ListItem Value="F" Text="FEMALE"/>
		</asp:DropDownList>
	</p>
	
	<p class="entry">
		<label>School attended:<span class="entry_reqd">*</span></label><asp:TextBox id="txtSchool" runat="server" CssClass="txtbox" />
		<asp:RequiredFieldValidator id="rfvSchool" ControlToValidate="txtSchool" runat="server" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> Please enter school</span>" Display="Dynamic" />
	</p>
	
	<p class="entry">
		<label>School year:<span class="entry_reqd">*</span></label><asp:TextBox id="txtSchoolYear" runat="server" CssClass="txtboxS" />
		<asp:RequiredFieldValidator id="rfvSchoolYear" ControlToValidate="txtSchoolYear" runat="server" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> Please enter school year</span>" Display="Dynamic" />
	</p>

	<p class="entry">
		<label>Email:<span class="entry_reqd">*</span></label><asp:TextBox id="txtYPEmail" runat="server" CssClass="txtboxW" />
		<asp:RequiredFieldValidator id="rfvYPEmail" ControlToValidate="txtYPEmail" runat="server" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> Please enter an email address</span>" Display="Dynamic" />
	</p>
	
	<div class="entry_form_note">
		<h3>Medical condition of participant</h3>
		<p>
			Please mention any current medical condition that the organisers and coaches should be aware of. Please also list any medication that is carried 
			by the participant. <strong>Note:</strong> The Mid Sussex Tri Club cannot accept responsibility for giving any medication to any participant 
			at the TriHub training.
		</p>
		<asp:TextBox id="txtMedical" runat="server" CssClass="txtboxw" TextMode="MultiLine" />
	</div>
	
	<div class="entry_form_note">
		<h3>Dietry Information</h3>
		<p>
			Please list any allergies or information we should know pertaining to diet
		</p>
		<asp:TextBox id="txtDiet" runat="server" CssClass="txtboxw" TextMode="MultiLine" />
	</div>
	
	<div class="entry_form_note">
		<h3>Triathlon Experience</h3>
		<p>
			Are you a member of any clubs or/and have you taken part in a triathlon? Please give details
		</p>
		<asp:TextBox id="txtExperience" runat="server" CssClass="txtboxw" TextMode="MultiLine" />
	</div>
	
	<h2>
		Parent details and emergency contact
	</h2>
	<p class="entry top">
		<label>First Name:<span class="entry_reqd">*</span></label><asp:TextBox id="txtParentFirstName" runat="server" CssClass="txtbox" />
		<asp:RequiredFieldValidator id="rfvParentFirstName" ControlToValidate="txtParentFirstName" runat="server" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> Please enter your first name</span>" Display="Dynamic" />
	</p>
	<p class="entry">
		<label>Last Name:<span class="entry_reqd">*</span></label><asp:TextBox id="txtParentLastName" runat="server" CssClass="txtbox" />
		<asp:RequiredFieldValidator id="rfvParentLastName" ControlToValidate="txtParentLastName" runat="server" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> Please enter your last name</span>" Display="Dynamic" />
	</p>
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
		<label>Email:<span class="entry_reqd">*</span></label><asp:TextBox id="txtEmail" runat="server" CssClass="txtboxW" />
		<asp:RequiredFieldValidator id="rfvtxtEmail" ControlToValidate="txtEmail" runat="server" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> Please enter your email address</span>" Display="Dynamic" />
		<asp:RegularExpressionValidator ID="regexVEmail" runat="server" ControlToValidate="txtEmail" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> Please enter a valid email address</span>" Display="Dynamic" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
	</p>
	
	<div class="entry_form_note">
		<p>
			Please pay online or make cheques payable to Mid Sussex Triathlon Club and send to:
			MSTC TriHub 12,Brading Road, Brighton BN2 3PD
			Please enter as soon as possible as places are limited.  
		</p>
		<h3>Conditions:</h3>
		<p>
			Please note the young person must be able to swim at least 25m in a recognised stroke on their front.
			The young person should bring swimwear/cap/goggles/towel, cycle (any model that is mechanically sound) and cycle helmet, 
			suitable footwear and clothing for running and cycling and a drink. The club committee may take photos / video for marketing purposes.
		</p>
		<h3>Disclaimer:</h3>
		<p>
			A more detailed participation pack will be sent out once entry is accepted and confirmed.  However, please note that the Mid Sussex 
			Tri Club, its members or agents will accept no liability in respect of any personal injury to participants or spectators, or any 
			loss or damage to property of any participant or spectator that may occur during or as a result of attending and/or participating 
			in the junior training.  The Mid Sussex Tri Club holds public liability insurance.
		</p>
	</div>
	<h3>
		Terms &#038; Conditions
	</h3>
	<p class="entry">
		<label>Your Name:<span class="entry_reqd">*</span></label><asp:TextBox id="txtName" runat="server" CssClass="txtboxw" />
		<asp:RequiredFieldValidator id="rfvtxtName" ControlToValidate="txtName" runat="server" 
		ErrorMessage="<span class='mst_fielderror'><span class='ERROR'>Error!</span> Please enter your name</span>" Display="Dynamic" />
	</p>
	<p class="entry">
		<asp:CheckBox id="cbAgree" runat="server" /> By ticking the box you are bound by the Conditions and Disclaimer above.</a>.
		<asp:Label id="lblTerms" runat="server" Display="Dynamic" />
	</p>
	<p class="Captcha clearfix">
		<recaptcha:RecaptchaControl
		ID="recaptcha"
		runat="server"
		theme="white"
		PublicKey="6LdNrt4SAAAAADwamubmUbsuZ6t0v_m37HzONoGf"
		PrivateKey="6LdNrt4SAAAAAKF1fmWSNpTEQFvWd0k_E3Aum7-b"
		/>
		<asp:Label id="lblCaptcha" runat="server" Display="Dynamic" />
	</p>
	<p class="entry_button">
		<asp:Button id="btnEntry" OnClick="submitEntry" Text="Submit entry" Runat="server" CssClass="entry_button" />
	</p>
</asp:PlaceHolder>
</form>