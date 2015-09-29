using System;
using System.Data;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net.Mail;
using System.Collections.Generic;
using Mstc.Core.Providers;

namespace cFront.Projects.CFSL.Web.UI.UserControls
{
	// ===============================================================================================================================================================
    public class ContactForm : UserControl
	{
		protected TextBox 		txtName, txtEmail, txtMsg;
		protected DropDownList topicSelect;
		protected PlaceHolder 	phMessageForm, phMessageSent;
		protected int			intViewState;
		protected Label			lblResult;
		
		void Page_Load()
		{
			ViewState["ViewContactForm"] = 0;
			
			intViewState = (int)ViewState["ViewContactForm"];
			
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
			if(Page.IsValid)
			{
				var emailProvider = new EmailProvider();

				string toAddress;
				if (emailProvider.EmailLookup.TryGetValue(topicSelect.SelectedValue, out toAddress) == false)
					toAddress = ConfigurationManager.AppSettings["contactFormEmailTo"] ?? "info@midsussextriclub.com";
				
				string content = "<p>" + txtMsg.Text + "</p><p>Message from: " + txtName.Text + "</p><p>Email: " + txtEmail.Text +
				                 "</p>";

				emailProvider.SendEmail(toAddress, txtEmail.Text, "[Website Enquiry]", content);
					
				ViewState["ViewContactForm"] = 1;
				
				phMessageSent.Visible = true;
				phMessageForm.Visible = false;
			}
			else
			{
				lblResult.Text = "The words you entered were incorrect. Please try again";
				lblResult.Visible = true;
			}
		}
	}
}	