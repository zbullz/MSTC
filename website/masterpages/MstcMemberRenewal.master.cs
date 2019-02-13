using System;
using System.Collections.Generic;
using cFront.Umbraco;
using Mstc.Core.Domain;
using Mstc.Core.Providers;
using Newtonsoft.Json;
using umbraco.BusinessLogic;

public partial class masterpages_MstcMemberRenewal : System.Web.UI.MasterPage
{
    private SessionProvider _sessionProvider;

    public masterpages_MstcMemberRenewal()
    {
        _sessionProvider = new SessionProvider();
    }

	protected void Page_Load(object sender, EventArgs e)
	{
		IDictionary<String, object> currentmemdata = MemberHelper.Get();
		litActionType.Text = IsRenewing(currentmemdata) ? "renew" : "upgrade";
		litTitleActionType.Text = IsRenewing(currentmemdata) ? "Renew" : "Upgrade";
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

		
		MembershipOptions membershipOptions = membershipOptionsControl.GetMembershipOptions();
		_sessionProvider.RenewalOptions = membershipOptions;

		Log.Add(LogTypes.Custom, - 1,
			string.Format("Membership {0} request: {1}, {2}", IsRenewing(currentmemdata) ? "renewal" : "upgrade",
				currentmemdata[MemberProperty.Email],
				JsonConvert.SerializeObject(membershipOptions)));

        PaymentStates state =  IsRenewing(currentmemdata) ? PaymentStates.MemberRenewal : PaymentStates.MemberUpgrade;
        RedirectToPaymentPages(currentmemdata, state); 
	}

    private void RedirectToPaymentPages(IDictionary<String, object> currentmemdata, PaymentStates state)
    {
        _sessionProvider.MandateSuccessPage = "payment-confirmation";

        bool hasMandate = string.IsNullOrWhiteSpace(currentmemdata[MemberProperty.directDebitMandateId] as string) == false;
        string page = hasMandate ? "payment-confirmation" : "mandate-request";

        _sessionProvider.CanProcessPaymentCompletion = true;

        string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
            Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
        string redirectUrl = string.Format("{0}/{1}?state={2}", rootUrl, page, state);
        Response.Redirect(redirectUrl);
    }

	private bool IsRenewing(IDictionary<String, object> currentmemdata)
	{
		string membershipTypeValue = currentmemdata[MemberProperty.membershipType] as string;
		if (string.IsNullOrWhiteSpace(membershipTypeValue) == false)
		{
			//Sadly there is no nicer way to do this as umbraco gives us an int as a string object! 
			int membershipTypeInt;
			if (int.TryParse(membershipTypeValue, out membershipTypeInt))
			{
				return (MembershipType)membershipTypeInt != MembershipType.Guest;
			}
		}

		return true;
	}
}