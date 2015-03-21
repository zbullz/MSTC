using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco.Membership;
using GoCardlessSdk;
using GoCardlessSdk.Connect;

public partial class masterpages_MstcDuathlonEntry : System.Web.UI.MasterPage
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

		IDictionary<String, object> currentmemdata = MemberHelper.Get();
		if (currentmemdata == null)
		{
			return; //Ensure the form is behind a login form
		}

		decimal cost = 10m;
		string memberEmail = currentmemdata[MemberProperty.Email] as string;
		RedirectToGocardless(memberEmail, cost, "MSTC Duathlon Entry");
		//RedirectToCompletePage(); //Can use this for local testing
	}

	private void RedirectToCompletePage()
	{
		string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
			Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
		string redirectUrl = string.Format("{0}/club-events/seasons-events/duathlon-entry-complete", rootUrl);
		Response.Redirect(redirectUrl);
	}

	private void RedirectToGocardless(string memberEmail, decimal cost, string description)
	{
		var goCardlessProvider = new GoCardlessProvider();
		var billRequest = new BillRequest(goCardlessProvider.MerchantId, cost)
		{
			Name = "MSTC Duathlon Entry",
			Description = description,
			User = new UserRequest()
			{
				Email = memberEmail
			},
		};

		//Could wrap this in a provider
		string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
			Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
		string redirectUrl = string.Format("{0}/club-events/seasons-events/duathlon-entry-complete", rootUrl);
		string cancelUrl = string.Format("{0}", rootUrl);

		string paymentGatewayUrl = goCardlessProvider.CreateBill(billRequest, redirectUrl, cancelUrl);
		Response.Redirect(paymentGatewayUrl);
	}

	protected void CheckBoxRequired_ServerValidate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = waiverAgreement.Checked;
	}
}
