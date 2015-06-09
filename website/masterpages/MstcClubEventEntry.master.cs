using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco.Membership;
using GoCardlessSdk;
using GoCardlessSdk.Connect;

public partial class masterpages_MstcClubEventEntry : System.Web.UI.MasterPage
{
	protected SessionProvider _sessionProvider;
	protected SessionProvider SessionProvider
	{
		get
		{
			if (_sessionProvider == null)
			{
				_sessionProvider = new SessionProvider();
			}
			return _sessionProvider;
		}
	}

    protected void Page_Load(object sender, EventArgs e)
    {
	    if (IsPostBack)
		    return;

	    string clubEvent = Request.QueryString["e"];

	    duathlonEntryForm.Visible = clubEvent == "d";
    }

	protected void DualthonEnter_OnClick(object sender, EventArgs e)
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

		SessionProvider.CanProcessPaymentCompletion = true;

		decimal cost = 10m;
		string memberEmail = currentmemdata[MemberProperty.Email] as string;
		//RedirectToGocardless(memberEmail, cost, "MSTC Duathlon Entry", "Mid Sussex Tri Club Duathlon Entry", PaymentStates.E00D101C);
		RedirectToCompletePage(PaymentStates.E00D101C.ToString()); //Can use this for local testing
	}

	private void RedirectToCompletePage(string state)
	{
		string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
			Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
		string redirectUrl = string.Format("{0}/payment-complete?state={1}", rootUrl, state);
		Response.Redirect(redirectUrl);
	}

	private void RedirectToGocardless(string memberEmail, decimal cost, string name, string description, PaymentStates paymentState)
	{
		var goCardlessProvider = new GoCardlessProvider();
		var redirectUrl = goCardlessProvider.CreateSimpleBill(memberEmail, cost, name, description, paymentState, Request.Url);
		Response.Redirect(redirectUrl);
	}

	protected void CheckBoxRequired_ServerValidate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = waiverAgreement.Checked;
	}
}
