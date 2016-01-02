using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco.Membership;
using GoCardlessSdk;
using GoCardlessSdk.Connect;
using Mstc.Core.Domain;
using Mstc.Core.Providers;
using Newtonsoft.Json;

public partial class masterpages_MstcGuestRegistration : System.Web.UI.MasterPage
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
				GuestCode = tbSecretCode.Text,
				ReferredByMember = tbMemberReferred.Text
			},
			RegistrationDetails = regDetails
		};
		memberProvider.UpdateMemberDetails(member, registrationFullDetails);

		//Login the member
		FormsAuthentication.SetAuthCookie(member.LoginName, true);

		var emailProvider = new EmailProvider();
		string content = string.Format("<p>A new guest has registered with the club</p><p>Guest details: {0}</p>",
			JsonConvert.SerializeObject(registrationFullDetails, Formatting.Indented));
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
			    @"I do not accept the terms in the open water swimming indemnity document.<br />I understand I will not be elligible to take part in club open water swim sessions.",
			    "NotAccepted")
	    };

		indemnityOptions.Items.AddRange(indemnityOptionsList.ToArray());
	}

	protected void tbSecretCodeValidator_ServerValidate(object source, ServerValidateEventArgs args)
	{
		string code = tbSecretCode.Text.ToLower();
		if (string.IsNullOrWhiteSpace(code) == false)
		{
			args.IsValid = (code == "egaffiliate" || code == "btrslegend" || code == "fishyfriends" || code == "leic-affiliate" || code == "hogwarts");
			return;
		}

		if (string.IsNullOrWhiteSpace(tbMemberReferred.Text) == false)
		{
			args.IsValid = true;
			return;
		}

		args.IsValid = false;
	}

}
