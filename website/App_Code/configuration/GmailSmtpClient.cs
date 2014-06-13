using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Configuration;

public class GmailSmtpClient : SmtpClient
{
	public GmailSmtpClient()
	{
		DeliveryMethod = SmtpDeliveryMethod.Network;
		Host = "smtp.gmail.com";
		Port = 587;
		UseDefaultCredentials = false;
		EnableSsl = true;
		Credentials = new NetworkCredential(ConfigurationManager.AppSettings["gmailUserName"], ConfigurationManager.AppSettings["gmailPassword"]);
	}
}