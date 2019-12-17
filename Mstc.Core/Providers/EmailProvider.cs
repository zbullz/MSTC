using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using Mstc.Core.configuration;

namespace Mstc.Core.Providers
{
	public class EmailProvider
	{
		public EmailProvider()
		{

		}

		public const string MembersEmail = "members@midsussextriclub.com";
		public const string OwsEmail = "openwaterswim@midsussextriclub.com";
        public const string SponsorsEmail = "sponsorship@midsussextriclub.com";
		public const string JuniorsEmail = "juniors@midsussextriclub.com";
		public const string SupportEmail = "support@midsussextriclub.com";
		public const string CoachingEmail = "coaching@midsussextriclub.com";


		public Dictionary<string, string> EmailLookup = new Dictionary<string, string>()
		{
			{"Membership", MembersEmail},
			{"OWS", OwsEmail},
            {"Sponsorship", SponsorsEmail},
			{"Juniors", JuniorsEmail},
			{"Website", SupportEmail},
			{"Coaching", CoachingEmail},
		};

		public void SendEmail(string toAddress, string fromAddress, string subject, string htmlContent)
		{
			MailMessage objMail = new MailMessage();

			objMail.To.Add(toAddress);
			objMail.From = new MailAddress(fromAddress);
			objMail.Subject = subject;
			objMail.IsBodyHtml = true;
			objMail.Body = htmlContent;

		    string gmailUserName = ConfigurationManager.AppSettings["gmailUserName"];
		    if (string.IsNullOrWhiteSpace(gmailUserName) == false)
		    {
		        var GmailSmtpClient = new GmailSmtpClient();
		        GmailSmtpClient.Send(objMail);
		    }
		}
	}
}