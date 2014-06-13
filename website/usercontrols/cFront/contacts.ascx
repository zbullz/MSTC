<%@ Control Inherits="cFront.Projects.CFSL.Web.UI.UserControls.ContactForm" AutoEventWireup="true" Src="code/contacts.cs" %>
<asp:PlaceHolder id="phMessageForm" runat="server">
	<form runat="server">
		<p class="contacts_form">
			<label>Your name:</label> <asp:TextBox id="txtName" runat="Server" CssClass="contact_tb_n" />
			<asp:RequiredFieldValidator id="rfvName" runat="server" ControlToValidate="txtName"
				ErrorMessage="<span class='contact_fielderror'><span class='ERROR'>Error!</span> Name required</span>" Display="Dynamic" />
		</p>
		<p class="contacts_form">
			<label>Your email:</label> <asp:TextBox id="txtEmail" runat="Server" CssClass="contact_tb_l" />
			<asp:RequiredFieldValidator id="rfvEmail" runat="server" ControlToValidate="txtEmail"
				ErrorMessage="<span class='contact_fielderror'><span class='ERROR'>Error!</span> Email required</span>" Display="Dynamic" />
				<asp:RegularExpressionValidator id="revEmail" ControlToValidate="txtEmail" runat="server" ValidationExpression="^\S+@\S+\.\S+$" 
				ErrorMessage="<span class='contact_fielderror'><span class='ERROR'>Error!</span> Invalid email address</span>" Display="Dynamic" />
		</p>
		<p class="contacts_form">
			<label>Message:</label> <asp:TextBox id="txtMsg" TextMode="MultiLine" runat="Server" CssClass="contact_tb_ml" />
		</p>
		<p class="contacts_form_button">
			<asp:Label id="lblResult" visible="false" runat="server" CssClass="contact-error" />
			<asp:Button id="btnSend" Text="Send message" Runat="server" onClick="SendMessage" CssClass="contact_btn_submit" />
		</p>
	</form>
</asp:PlaceHolder>
<asp:PlaceHolder id="phMessageSent" runat="server">
			<p class="sent_title">
				Message sent!
			</p>
			<p class="sent">
				We will get back to you as soon as possible.
			</p>
</asp:PlaceHolder>