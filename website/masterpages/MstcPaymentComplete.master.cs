using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco.Membership;
using GoCardlessSdk;
using GoCardlessSdk.Connect;
using Lucene.Net.Search.Function;

public partial class masterpages_MstcPaymentComplete : System.Web.UI.MasterPage
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
	
	protected bool ShowSwimCreditsConfirmation = false;
	protected bool ShowEventConfirmation = false;

    protected void Page_Load(object sender, EventArgs e)
    {
		if (IsPostBack == false)
		{
			IDictionary<String, object> currentmemdata = MemberHelper.Get();
			if (currentmemdata == null || SessionProvider.CanProcessPaymentCompletion == false)
			{
				return; //Ensure the form is behind a login form and not a duplicate request
			}

			//lblQueryString.Text = Request.QueryString["resource_uri"];
			if (Request.QueryString["resource_uri"] != null)
			{
				ConfirmPaymentRequest();
			}
			if (Request.QueryString["state"] != null)
			{
				ProcessPaymentState(currentmemdata, Request.QueryString["state"]);
			}

			SessionProvider.CanProcessPaymentCompletion = false;
		}
    }

	private void ConfirmPaymentRequest()
	{
		var goCardlessProvider = new GoCardlessProvider();
		goCardlessProvider.ConfirmBill(Request.QueryString);
	}

	private void ProcessPaymentState(IDictionary<String, object> currentmemdata, string state)
	{
		var paymentState = (PaymentStates) Enum.Parse(typeof(PaymentStates), state);

		switch (paymentState)
		{
			case PaymentStates.S00599C:
			case PaymentStates.S001099C:
			case PaymentStates.S001599C:
			{
				UpdateMemberSwimCredits(currentmemdata, (int) paymentState);
				DisplaySwimCreditConfirmationMessage((int) paymentState);
				break;
			}
			case PaymentStates.E00D101C:
			{
				EnterMemberInDuathlon(currentmemdata);
				DisplayEventEnteredMessage(paymentState);
				break;
			}
		}
	}

	private void DisplaySwimCreditConfirmationMessage(int credits)
	{
		ShowSwimCreditsConfirmation = true;
		litSwimCredits.Text = credits.ToString();
	}

	private void DisplayEventEnteredMessage(PaymentStates paymentState)
	{
		ShowEventConfirmation = true;
		litEventEntered.Text = paymentState.GetAttributeOfType<DescriptionAttribute>().Description;
	}

	private void UpdateMemberSwimCredits(IDictionary<String, object> currentmemdata, int credits)
	{
		int memberCredits = 0;
		int.TryParse(currentmemdata[MemberProperty.SwimCreditsBought].ToString(), out memberCredits);
	
		memberCredits += credits;
		currentmemdata[MemberProperty.SwimCreditsBought] = memberCredits;

		MemberHelper.Update(currentmemdata);
	}

	private void EnterMemberInDuathlon(IDictionary<String, object> currentmemdata)
	{
		currentmemdata[MemberProperty.DuathlonEntered] = true;
		MemberHelper.Update(currentmemdata);
	}
}
