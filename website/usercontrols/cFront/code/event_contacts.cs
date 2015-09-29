using System;
using System.Data;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net.Mail;
using Mstc.Core.configuration;

namespace cFront.Projects.CFSL.Web.UI.UserControls
{
	// ===============================================================================================================================================================
    public class EventContactForm : UserControl
	{
		protected TextBox 		txtName, txtEmail, txtMsg;
		protected PlaceHolder 	phMessageForm, phMessageSent;
		protected int			intViewState;
		protected string 		RecipientAddress;
		protected DropDownList	ddlRecipient;
		
		void Page_Load()
		{
			ViewState["ViewContactForm"] = 0;
			
			intViewState = (int) ViewState["ViewContactForm"];
			
			if(intViewState == 0)
			{
				phMessageSent.Visible = false;
				phMessageForm.Visible = true;
			}
			else if(intViewState == 1)
			{
				phMessageSent.Visible = true;
				phMessageForm.Visible = false;
			}
			
		}
			
		protected void SendMessage(Object s, EventArgs e)
		{
			
			if(ddlRecipient.SelectedItem.Value == "1")
				RecipientAddress = "info@midsussextriclub.com";
			else if(ddlRecipient.SelectedItem.Value == "3")
				RecipientAddress = "sponsorship@midsussextriclub.com";
		
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
}