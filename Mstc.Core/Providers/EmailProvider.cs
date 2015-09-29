using System.Collections.Generic;
using System.Net.Mail;
using Mstc.Core.configuration;

namespace Mstc.Core.Providers
{
	/// <summary>
	/// Summary description for EmailProvider
	/// </summary>
	public class EmailProvider
	{
		public EmailProvider()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public const string MembersEmail = "members@midsussextriclub.com";
		public const string SponsorsEmail = "sponsorship@midsussextriclub.com";
		public const string JuniorsEmail = "juniors@midsussextriclub.com";
		public const string SupportEmail = "support@midsussextriclub.com";


		public Dictionary<string, string> EmailLookup = new Dictionary<string, string>()
		{
			{"Membership", MembersEmail},
			{"Sponsorship", SponsorsEmail},
			{"Juniors", JuniorsEmail},
			{"Website", SupportEmail},
		};

		public void SendEmail(string toAddress, string fromAddress, string subject, string htmlContent)
		{
			MailMessage objMail = new MailMessage();

			objMail.To.Add(toAddress);
			objMail.From = new MailAddress(fromAddress);
			objMail.Subject = subject;
			objMail.IsBodyHtml = true;
			objMail.Body = htmlContent;

			var GmailSmtpClient = new GmailSmtpClient();
			GmailSmtpClient.Send(objMail);
		}
	}
}