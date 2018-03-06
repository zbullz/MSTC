using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls;
using Mstc.Core.Domain;
using Mstc.Core.Providers;
using Newtonsoft.Json;

public partial class masterpages_MstcOwsTasterRegistration : System.Web.UI.MasterPage
{
	private const string AcceptIndemnity = "Accept";

    protected void Page_Load(object sender, EventArgs e)
    {
	    if (Page.IsPostBack == false)
	    {
		    BindControls();
	    }
    }

	protected void Enter_OnClick(object sender, EventArgs e)
	{
		if (Page.IsValid == false)
		{
			return;
		}

		var regDetails = registrationDetailsControl.GetRegistrationDetails();
		bool openWaterSwimAccepted = indemnityOptions.SelectedValue == AcceptIndemnity;

		var memberProvider = new MemberProvider();
		var member = memberProvider.CreateMember(regDetails, new string[] { "Guest" });

		var registrationFullDetails = new RegistrationFullDetails()
		{
			MembershipOptions = new MembershipOptions()
			{
				MembershipType = MembershipType.Guest,
				OpenWaterIndemnityAcceptance = openWaterSwimAccepted,
				SwimSubsAprToSept = false,
				SwimSubsOctToMar = false,
				Volunteering = false,
			},
			RegistrationDetails = regDetails
		};
		memberProvider.UpdateMemberDetails(member, registrationFullDetails);

		//Login the member
		FormsAuthentication.SetAuthCookie(member.LoginName, true);

		var emailProvider = new EmailProvider();
		string content = string.Format("<p>A new guest has registered with the club</p><p>Guest details: {0}</p>",
			JsonConvert.SerializeObject(registrationFullDetails, Formatting.Indented));
        var passwordObfuscator = new PasswordObfuscator();
        content = passwordObfuscator.ObfuscateString(content);

        emailProvider.SendEmail(ConfigurationManager.AppSettings["newRegistrationEmailTo"], EmailProvider.SupportEmail, "New MSTC Guest registration", content);

		Response.Redirect("/members-area/my-details");
	}

	private void BindControls()
	{
		var indemnityOptionsList = new List<ListItem>()
	    {
		    new ListItem(
			    @"I have read and understand the open water swimming indemnity document.<br />I agree to and accept the terms without qualification.",
			    AcceptIndemnity),
		    new ListItem(
                @"I do not wish to take part in open water swimming.",
			    "NotAccepted")
	    };

		indemnityOptions.Items.AddRange(indemnityOptionsList.ToArray());
	}
}
