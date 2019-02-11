using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls;
using Mstc.Core.Domain;
using Mstc.Core.Providers;
using Newtonsoft.Json;

public partial class masterpages_MstcEventGuestRegistration : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

	protected void Enter_OnClick(object sender, EventArgs e)
	{
		if (Page.IsValid == false)
		{
			return;
		}

		var regDetails = registrationDetailsControl.GetRegistrationDetails();

		var memberProvider = new MemberProvider();
		var member = memberProvider.CreateMember(regDetails, new string[] { "Guest" });

		var registrationFullDetails = new RegistrationFullDetails()
		{
			MembershipOptions = new MembershipOptions()
			{
				MembershipType = MembershipType.Guest,
				OpenWaterIndemnityAcceptance = false,
				SwimSubs1 = "",
				SwimSubs2 = "",
				Volunteering = false,
				GuestCode = tbSecretCode.Text
			},
			RegistrationDetails = regDetails
		};
		memberProvider.UpdateMemberDetails(member, registrationFullDetails);

		//Login the member
		FormsAuthentication.SetAuthCookie(member.LoginName, true);

		var emailProvider = new EmailProvider();
		string content = string.Format("<p>A new event guest has registered with the club</p><p>Guest details: {0}</p>",
			JsonConvert.SerializeObject(registrationFullDetails, Formatting.Indented));
        var passwordObfuscator = new PasswordObfuscator();
        content = passwordObfuscator.ObfuscateString(content);

        emailProvider.SendEmail(ConfigurationManager.AppSettings["newRegistrationEmailTo"], EmailProvider.SupportEmail, "New MSTC Guest registration", content);

		Response.Redirect("/members-area/my-details");
	}

	protected void tbSecretCodeValidator_ServerValidate(object source, ServerValidateEventArgs args)
	{
		string code = tbSecretCode.Text.ToLower();
		if (string.IsNullOrWhiteSpace(code) == false)
		{
			args.IsValid = (code == "festguest");
			return;
		}

		args.IsValid = false;
	}

}
