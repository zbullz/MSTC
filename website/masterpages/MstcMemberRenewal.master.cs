using System;
using System.Collections.Generic;
using cFront.Umbraco;
using GoCardlessSdk.Connect;
using Mstc.Core.Domain;
using Mstc.Core.Providers;
using Newtonsoft.Json;
using umbraco.BusinessLogic;

public partial class masterpages_MstcMemberRenewal : System.Web.UI.MasterPage
{
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

		var sessionProvider = new SessionProvider();
		MembershipOptions membershipOptions = membershipOptionsControl.GetMembershipOptions();
		sessionProvider.RenewalOptions = membershipOptions;

		Log.Add(LogTypes.Custom, - 1,
			string.Format("Membership {0} request: {1}, {2}", IsRenewing(currentmemdata) ? "renewal" : "upgrade",
				currentmemdata[MemberProperty.Email],
				JsonConvert.SerializeObject(membershipOptions)));

		decimal cost = (new MembershipCostCalculator()).Calculate(membershipOptions, DateTime.Now);
		string memberEmail = currentmemdata[MemberProperty.Email] as string;

		string billName = string.Format("MSTC Membership {0}", IsRenewing(currentmemdata) ? "Renewal" : "Upgrade");
		var memberProvider = new MemberProvider();
		RedirectToGocardless(billName, memberEmail, cost, memberProvider.GetPaymentDescription(membershipOptions));
		//RedirectToCompletePage(); //Can use this for local testing
	}

	private void RedirectToCompletePage()
	{
		string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
			Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
		string redirectUrl = string.Format("{0}/members-area/membership-renewal-complete", rootUrl);
		Response.Redirect(redirectUrl);
	}

	private void RedirectToGocardless(string billName, string memberEmail, decimal cost, string description)
	{
		var goCardlessProvider = new GoCardlessProvider();
		var billRequest = new BillRequest(goCardlessProvider.MerchantId, cost)
		{
			Name = billName,
			Description = description,
			User = new UserRequest()
			{
				Email = memberEmail
			},
		};

		//Could wrap this in a provider
		string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
			Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
		string redirectUrl = string.Format("{0}/members-area/membership-renewal-complete", rootUrl);
		string cancelUrl = string.Format("{0}/members-area/membership-renewal-cancelled", rootUrl);

		string paymentGatewayUrl = goCardlessProvider.CreateBill(billRequest, redirectUrl, cancelUrl);
		Response.Redirect(paymentGatewayUrl);
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