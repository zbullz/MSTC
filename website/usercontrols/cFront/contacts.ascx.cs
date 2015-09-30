using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mstc.Core.Providers;

public partial class usercontrols_cFront_contacts : System.Web.UI.UserControl
{
	void Page_Load()
	{
		ViewState["ViewContactForm"] = 0;

		int intViewState = (int)ViewState["ViewContactForm"];

		if (intViewState == 0)
		{
			phMessageSent.Visible = false;
			phMessageForm.Visible = true;
		}
		else if (intViewState == 1)
		{
			phMessageSent.Visible = true;
			phMessageForm.Visible = false;
		}

	}

	protected void SendMessage(Object s, EventArgs e)
	{
		if (Page.IsValid)
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