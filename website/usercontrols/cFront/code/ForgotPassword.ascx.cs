using System;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;
using Mstc.Core.configuration;
using umbraco.cms.businesslogic.member;

namespace usercontrols.cFront.code
{
    public partial class usercontrols_cFront_ForgotPassword : System.Web.UI.UserControl
    {
        // Error format
        private const string ErrorFormat = "<p class=\"formerror\">{0}</p>";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnSubmitClick(object sender, EventArgs e)
        {
            // Try and get the member from the email address entered
            var cMember = Member.GetMemberFromEmail(tbEmail.Text);

            // Check we have a member with that email address
            if (cMember != null)
            {
                // Found the user
                // Generate a password which we'll email the member
                var password = Membership.GeneratePassword(10, 1);
                password = Regex.Replace(password, @"[^a-zA-Z0-9]", m => "9");

                // Change the password to the new generated one above
                var member = Membership.GetUser(cMember.LoginName);
                member.ChangePassword(member.ResetPassword(), password);

                // Save the password/member
                cMember.Save();

                // update the XML cache 
                cMember.XmlGenerate(new System.Xml.XmlDocument());

                // Now email the member their password
                MailMessage objMail = new MailMessage();

                objMail.From = new MailAddress("noreply@midsussextriclub.com");
                objMail.To.Add(tbEmail.Text);
                //objMail.From = new MailAddress(txtEmail.Text);
                objMail.Subject = "Mid Sussex Tri Club password reset";

                objMail.IsBodyHtml = true;

                var sb = new StringBuilder();
                sb.AppendFormat(string.Format("<p>Please find your new password below to access the site</p>"));
                sb.AppendFormat("<p><b>{0}</b></p>", password);
                objMail.Body = sb.ToString();

                var GmailSmtpClient = new GmailSmtpClient();
                GmailSmtpClient.Send(objMail);


                // Disable the button to stop them pressing it again
                btnSubmit.Enabled = false;

                // Show a message to the user
                litError.Text = string.Format(ErrorFormat, "Password Sent");
            }
            else
            {
                // Can't find a user with that email
                litError.Text = string.Format(ErrorFormat, "Can't find a user with that email address");
            }
        }
    }
}