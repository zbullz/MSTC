using System;
using System.Data;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net.Mail;

namespace cFront.Projects.CFSL.Web.UI.UserControls
{
	// ===============================================================================================================================================================
    public class ContactForm : UserControl
	{
		protected TextBox 		txtName, txtEmail, txtMsg;
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
				MailMessage objMail = new MailMessage();
				objMail.To.Add("pete081259@aol.com");
				objMail.To.Add("stephen.mcmenamin@domesticandgeneral.com");
				objMail.To.Add("info@midsussextriclub.com");
				objMail.From = new MailAddress(txtEmail.Text);
				objMail.Subject = "[Website Enquiry]";
				
				objMail.IsBodyHtml = true;
				
				objMail.Body = "<p>" + txtMsg.Text + "</p><p>Message from: " + txtName.Text + "</p><p>Email: " + txtEmail.Text + "</p>";
				
				SmtpClient smtpClient = new SmtpClient();
				smtpClient.Send(objMail);
					
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