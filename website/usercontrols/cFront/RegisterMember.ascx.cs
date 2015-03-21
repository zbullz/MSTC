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
using umbraco.BusinessLogic;

public partial class usercontrols_cFront_RegisterMember : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

	protected void RegisterMember_OnClick(object sender, EventArgs e)
	{
		if (Page.IsValid == false)
		{
			return;
		}

		var registrationFullDetails = new RegistrationFullDetails()
		{
			MembershipOptions = membershipOptionsControl.GetMembershipOptions(),
			RegistrationDetails = registrationDetailsControl.GetRegistrationDetails()
		};

		var sessionProvider = new SessionProvider();
		sessionProvider.RegistrationFullDetails = registrationFullDetails;

		Log.Add(LogTypes.Custom, -1, string.Format("New member registration request: {0}",
			JsonConvert.SerializeObject(registrationFullDetails)));

		decimal cost = (new MembershipCostCalcualtor()).Calculate(registrationFullDetails.MembershipOptions);
		RedirectToGocardless(registrationFullDetails.RegistrationDetails.Email, cost, GetPaymentDescription(registrationFullDetails.MembershipOptions));
		//RedirectToCompletePage(); //Can use this for local testing
	}

	private void RedirectToCompletePage()
	{
		string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
			Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
		string redirectUrl = string.Format("{0}/the-club/membership-registration-complete", rootUrl);
		Response.Redirect(redirectUrl);
	}

	private void RedirectToGocardless(string memberEmail, decimal cost, string description)
	{
		var goCardlessProvider = new GoCardlessProvider();
		var billRequest = new BillRequest(goCardlessProvider.MerchantId, cost)
		{
			Name = "MSTC Membership Registration",
			Description = description,
			User = new UserRequest()
			{
				Email = memberEmail
			},
		};

		//Could wrap this in a provider
		string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
			Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
		string redirectUrl = string.Format("{0}/the-club/membership-registration-complete", rootUrl);
		string cancelUrl = string.Format("{0}", rootUrl);

		string paymentGatewayUrl = goCardlessProvider.CreateBill(billRequest, redirectUrl, cancelUrl);
		Response.Redirect(paymentGatewayUrl);
	}

	private string GetPaymentDescription(MembershipOptions membershipOptions)
	{
		List<string> descriptionList = new List<string>() { string.Format("New member - {0}", membershipOptions.MembershipType.ToString())};
		if (membershipOptions.SwimSubsJanToJune)
		{
			descriptionList.Add("Swim subs Jan to June");
		}
		if (membershipOptions.SwimSubsJulyToDec)
		{
			descriptionList.Add("Swim subs July to Dec");
		}

		return string.Join(", ", descriptionList);
	}
}