using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco.Membership;
using GoCardlessSdk;
using GoCardlessSdk.Connect;

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
				SwimSubsJanToJune = false,
				SwimSubsJulyToDec = false,
				Volunteering = false,
				GuestCode = tbSecretCode.Text
			},
			RegistrationDetails = regDetails
		};
		memberProvider.UpdateMemberDetails(member, registrationFullDetails, new DateTime(2099, 1, 1));

		//Login the member
		FormsAuthentication.SetAuthCookie(member.LoginName, true);

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
		if (tbSecretCode.Text == "EGAffiliate" || tbSecretCode.Text == "BTRSLegend")
			args.IsValid = true;
		else
			args.IsValid = false;
	}

}
