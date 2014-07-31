using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic.member;

namespace cFront.Projects.CFSL.Web.UI.UserControls
{

	/// <summary>
	/// Summary description for ChangePassword
	/// </summary>
	/// <summary>
	/// Summary description for changePassword1
	/// </summary>
	public class ChangePassword : UserControl
	{
		protected TextBox tbCurrentPassword, tbNewPassword;
		protected Literal litError;
		protected Button btnSubmit;

		// Error format
		private const string ErrorFormat = "<p class=\"formerror\">{0}</p>";

		protected void Page_Load(object sender, EventArgs e)
		{
			Member cMember = Member.GetCurrentMember();
			litError.Text = string.Format(ErrorFormat, cMember.Password);
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

			//TODO - Fix this to un-hash the password before comparing
			if (cMember.Password != tbCurrentPassword.Text)
			{
				litError.Text = string.Format(ErrorFormat,
											  "Your current password does not match to your stored password");
				return;
			}

			cMember.ChangePassword(tbNewPassword.Text);
			// Save the password/member
			cMember.Save();

			// Disable the button to stop them pressing it again
			btnSubmit.Enabled = false;

			// Show a message to the user
			litError.Text = string.Format(ErrorFormat, "Password changed");

		}
	}
}