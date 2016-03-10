<%@ Control Language="C#" AutoEventWireup="true" CodeFile="event_contacts.ascx.cs" Inherits="usercontrols_cFront_event_contacts" %>
<asp:PlaceHolder id="phMessageError" runat="server">
    <p style="color:red;">Please enter an email address</p>
</asp:PlaceHolder>


<asp:PlaceHolder id="phMessageForm" runat="server">
		<p> 
			To contact us about the event select the appropriate person and then complete the form below and click "Send Message"
		</p>
		<p class="contact">
			<label>Send To:</label>
			<asp:DropDownList id="ddlRecipient" runat="server" CssClass="ddlbox">
				<asp:ListItem Value="1">Steve McMenamin (Race Director)</asp:ListItem>
				<asp:ListItem Value="3">Emma (Sponsorship)</asp:ListItem>
				<asp:ListItem Value="5">Roger (Press)</asp:ListItem>
			</asp:DropDownList>
		</p>
		<p class="contacts_form">
			<label>Your name:</label> <asp:TextBox id="txtName" runat="Server" CssClass="contact_tb_n" />
		</p>
		<p class="contacts_form">
			<label>Your email:</label> <asp:TextBox id="txtEmail" runat="Server" CssClass="contact_tb_l" />
		</p>
		<p class="contacts_form">
			<label>Message:</label> <asp:TextBox id="txtMsg" TextMode="MultiLine" runat="Server" CssClass="contact_tb_ml" />
		</p>
		<p class="contacts_form_button">
			<asp:Button id="btnSend" Text="Send message" Runat="server" onClick="SendMessage" CssClass="contact_btn_submit" />
		</p>
</asp:PlaceHolder>
<asp:PlaceHolder id="phMessageSent" runat="server">
			<p class="sent_title">
				Message sent!
			</p>
			<p class="sent">
				We will get back to you as soon as possible.
			</p>
</asp:PlaceHolder>