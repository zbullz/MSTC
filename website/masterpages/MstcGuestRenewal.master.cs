using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using cFront.Umbraco;
using Mstc.Core.Domain;
using Mstc.Core.Providers;
using umbraco.cms.businesslogic.member;

public partial class masterpages_MstcGuestRenewal : System.Web.UI.MasterPage
{
    private MemberProvider _memberProvider;

    private const string AcceptIndemnity = "Accept";

    public masterpages_MstcGuestRenewal()
    {
        _memberProvider = new MemberProvider();
    }

    protected void Page_Load(object sender, EventArgs e)
	{
		var indemnityOptionsList = new List<ListItem>()
	    {
		    new ListItem(
			    @"I have read and understand the open water swimming indemnity document.<br />I agree to and accept the terms without qualification and agree to be included in the duty rota.",
			    AcceptIndemnity),
		    new ListItem(
			    @"I do not accept the terms in the open water swimming indemnity document.<br />I understand I will not be elligible to take part in club open water swim sessions for this membership year.",
			    "NotAccepted")
	    };

		indemnityOptions.Items.AddRange(indemnityOptionsList.ToArray());
	}

	protected void RenewMember_OnClick(object sender, EventArgs e)
	{
		if (Page.IsValid == false)
		{
			return;
		}

		IDictionary<String, object> currentmemdata = MemberHelper.Get();
		if (currentmemdata == null)
		{
			return; //Ensure the form is behind a login form
		}
	
		MembershipOptions membershipOptions = new MembershipOptions()
		{
			MembershipType = MembershipType.Guest,
			SwimSubs1 = "",
			SwimSubs2 = "",
			OpenWaterIndemnityAcceptance = indemnityOptions.SelectedValue == AcceptIndemnity,
			Volunteering = true //Hardcode to true as can't renew unless this is selected :)
		};
        var member = Member.GetCurrentMember();
        bool resetEventEntries = true;
        _memberProvider.UpdateMemberOptions(member, membershipOptions, resetEventEntries);

		RedirectToCompletePage(); //Can use this for local testing
	}

	private void RedirectToCompletePage()
	{
		string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
			Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
		string redirectUrl = string.Format("{0}/members-area/my-details", rootUrl);
		Response.Redirect(redirectUrl);
	}
}