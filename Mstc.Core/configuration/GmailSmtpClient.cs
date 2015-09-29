using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace Mstc.Core.configuration
{
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
}