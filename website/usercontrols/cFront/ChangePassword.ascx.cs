using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic.member;

public partial class usercontrols_cFront_ChangePassword : UserControl
{
	// Error format
	private const string ErrorFormat = "<p class=\"formerror\">{0}</p>";

	protected void Page_Load(object sender, EventArgs e)
	{

	}

	protected void BtnSubmitClick(object sender, EventArgs e)
	{
		// Try and get the member from the email address entered
		Member cMember = Member.GetCurrentMember();

		// Check we have a member with that email address
		if (cMember == null)
		{
			litError.Text = string.Format(ErrorFormat, "You don't appear to be logged in, go away!");
			return;
		}

		MembershipUser member = Membership.GetUser(cMember.LoginName);
		if (member.ChangePassword(tbCurrentPassword.Text, tbNewPassword.Text) == false)
		{
			litError.Text = string.Format(ErrorFormat,
										  "Your current password does not match to your stored password");
			return;
		}

		// Save the password/member
		cMember.Save();

		// update the XML cache 
		cMember.XmlGenerate(new System.Xml.XmlDocument());

		// Disable the button to stop them pressing it again
		btnSubmit.Enabled = false;

		// Show a message to the user
		litError.Text = string.Format(ErrorFormat, "Password changed");

	}
}