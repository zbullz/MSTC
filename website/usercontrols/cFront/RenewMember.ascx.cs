using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using cFront.Umbraco.Membership;
using GoCardlessSdk;
using GoCardlessSdk.Connect;

public partial class usercontrols_cFront_RenewMember : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

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

		decimal cost = (new MembershipCostCalcualtor()).Calculate(membershipOptions);
		string memberEmail = currentmemdata[MemberProperty.Email] as string;
		RedirectToGocardless(memberEmail, cost, GetPaymentDescription(membershipOptions));
		//RedirectToCompletePage(); //Can use this for local testing
	}

	private void RedirectToCompletePage()
	{
		string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
			Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
		string redirectUrl = string.Format("{0}/members-area/membership-renewal-complete", rootUrl);
		Response.Redirect(redirectUrl);
	}

	private void RedirectToGocardless(string memberEmail, decimal cost, string description)
	{
		var goCardlessProvider = new GoCardlessProvider();
		var billRequest = new BillRequest(goCardlessProvider.MerchantId, cost)
		{
			Name = "MSTC Membership Renewal",
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

	private string GetPaymentDescription(MembershipOptions membershipOptions)
	{
		List<string> descriptionList = new List<string>() {membershipOptions.MembershipType.ToString()};
		if (membershipOptions.SwimSubsJanToJune)
		{
			descriptionList.Add("Swim subs Jan to June");
		}
		if (membershipOptions.SwimSubsJulyToDec)
		{
			descriptionList.Add("Swim subs July to Dec");
		}
		if (membershipOptions.CoreSubsAprilToSept)
		{
			descriptionList.Add("Core subs April to Sept");
		}
		if (membershipOptions.CoreSubsOctToMarch)
		{
			descriptionList.Add("Core subs Oct to March");
		}

		return string.Join(", ", descriptionList);
	}
}