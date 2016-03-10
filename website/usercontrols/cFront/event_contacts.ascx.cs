using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mstc.Core.configuration;

public partial class usercontrols_cFront_event_contacts : System.Web.UI.UserControl
{
	protected void Page_Load(object sender, EventArgs e)
	{
		ViewState["ViewContactForm"] = 0;

		int intViewState = (int)ViewState["ViewContactForm"];

		if (intViewState == 0)
		{
			phMessageSent.Visible = false;
			phMessageError.Visible = false;
			phMessageForm.Visible = true;
		}
		else if (intViewState == 1)
		{
			phMessageError.Visible = false;
			phMessageSent.Visible = true;
			phMessageForm.Visible = false;
		}

	}

	protected void SendMessage(Object s, EventArgs e)
	{
		if (txtEmail.Text.Contains("@") == false)
		{
			phMessageError.Visible = true;
			return;
		}
		else
		{
			phMessageError.Visible = false;
		}

		string RecipientAddress = "";
		if (ddlRecipient.SelectedItem.Value == "1")
			RecipientAddress = "info@midsussextriclub.com";
		else if (ddlRecipient.SelectedItem.Value == "3")
			RecipientAddress = "sponsorship@midsussextriclub.com";
		else if (ddlRecipient.SelectedItem.Value == "5")
			RecipientAddress = "press@midsussextriclub.com";

		MailMessage objMail = new MailMessage();
		objMail.To.Add(RecipientAddress);
		objMail.From = new MailAddress("noreply@midsussextriclub.com");
		objMail.Subject = "[Event Website Enquiry]";

		objMail.IsBodyHtml = true;

		objMail.Body = "<p>" + txtMsg.Text + "</p><p>Message from: " + txtName.Text + "</p><p>Email: " + txtEmail.Text + "</p>";

		GmailSmtpClient GmailSmtpClient = new GmailSmtpClient();
		GmailSmtpClient.Send(objMail);

		ViewState["ViewContactForm"] = 1;

		phMessageSent.Visible = true;
		phMessageForm.Visible = false;

	}

}