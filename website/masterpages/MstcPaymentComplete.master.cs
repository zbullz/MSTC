using System;
using System.Collections.Generic;
using System.ComponentModel;
using cFront.Umbraco.Membership;
using Mstc.Core.Domain;
using Mstc.Core.Providers;

public partial class masterpages_MstcPaymentComplete : System.Web.UI.MasterPage
{
    private SessionProvider _sessionProvider;
    private GoCardlessProvider _goCardlessProvider;

    protected bool ShowPaymentFailed = false;
    protected bool ShowSwimCreditsConfirmation = false;
    protected bool ShowEventConfirmation = false;
    protected bool ShowSwimSubsConfirmation = false;

    public masterpages_MstcPaymentComplete()
    {
        _sessionProvider = new SessionProvider();
        _goCardlessProvider = new GoCardlessProvider();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
		if (IsPostBack == false)
		{
			IDictionary<String, object> currentmemdata = MemberHelper.Get();
			if (currentmemdata == null || _sessionProvider.CanProcessPaymentCompletion == false || Request.QueryString["state"] == null)
			{
				return; //Ensure the form is behind a login form and not a duplicate request
			}
            
		    if (string.IsNullOrWhiteSpace(_sessionProvider.GoCardlessRedirectFlowId) == false)
		    {
                //Complete the mandate request if required
		        string mandateId = _goCardlessProvider.CompleteRedirectRequest(_sessionProvider.GoCardlessRedirectFlowId,
		            _sessionProvider.SessionId);
                currentmemdata[MemberProperty.directDebitMandateId] = mandateId;
                MemberHelper.Update(currentmemdata);
		        _sessionProvider.GoCardlessRedirectFlowId = null;
		    }

		    string state = Request.QueryString["state"];
            var paymentState = (PaymentStates)Enum.Parse(typeof(PaymentStates), state);

            var paymentSucceeded = CreatePayment(currentmemdata, paymentState);

		    if (paymentSucceeded)
		    {
		        ProcessPaymentState(currentmemdata, paymentState);
		    }
		    else
		    {
		        ShowPaymentFailed = true;
		    }

		    _sessionProvider.CanProcessPaymentCompletion = false;
		}
    }

    private void ProcessPaymentState(IDictionary<String, object> currentmemdata, PaymentStates paymentState)
    {
        switch (paymentState)
        {
            case PaymentStates.S00599C:
            case PaymentStates.S001099C:
            case PaymentStates.S001599C:
            case PaymentStates.S002499C:
            {
                UpdateMemberSwimCredits(currentmemdata, (int) paymentState);
                DisplaySwimCreditConfirmationMessage((int) paymentState);
                break;
            }
            case PaymentStates.E00D101C:
            {
                EnterMemberInDuathlon(currentmemdata);
                DisplayEventEnteredMessage(currentmemdata, paymentState);
                break;
            }
            case PaymentStates.E00TRIOI201C:
            case PaymentStates.E00TRIOR202C:
            case PaymentStates.E00TRIMI203C:
            case PaymentStates.E00TRIMR204C:
            case PaymentStates.E00TRISI205C:
            {
                EnterMemberInTriFest(currentmemdata, paymentState);
                DisplayEventEnteredMessage(currentmemdata, paymentState);
                break;
            }
            case PaymentStates.E00S1KM301C:
            case PaymentStates.E00S3KM302C:
            case PaymentStates.E00S5KM303C:
            case PaymentStates.E00S1KM3KM304C:
            case PaymentStates.E00S1KM5KM305C:
            case PaymentStates.E00S3KM5KM306C:
            case PaymentStates.E00S1KM3KM5KM307C:
            {
                //decimal cost = 20m;
                EnterMemberInCharitySwim(currentmemdata, paymentState);
                DisplayEventEnteredMessage(currentmemdata, paymentState);
                break;
            }
            case PaymentStates.SS05991:
            case PaymentStates.SS05992:
            case PaymentStates.SS05996:
            {
                UpdateMemberSwimSubs(currentmemdata, paymentState);
                DisplaySwimSubsConfirmationMessage(paymentState);
                break;
            }
        }
    }

    private bool CreatePayment(IDictionary<String, object> currentmemdata, PaymentStates paymentState)
    {
        string mandateId = currentmemdata[MemberProperty.directDebitMandateId] as string;
        string email = currentmemdata[MemberProperty.Email] as string;
        bool hasBTFNumber = string.IsNullOrWhiteSpace(currentmemdata[MemberProperty.BTFNumber] as string) == false; 
        int costInPence = MembershipCostCalculator.EventCost(paymentState, hasBTFNumber);
        string description = paymentState.GetAttributeOfType<DescriptionAttribute>().Description;

        bool paymentSucceeded = _goCardlessProvider.CreatePayment(mandateId, email, costInPence, description);

        return paymentSucceeded;
    }

    private void DisplaySwimCreditConfirmationMessage(int credits)
	{
		ShowSwimCreditsConfirmation = true;
		litSwimCredits.Text = credits.ToString();
	}

	private void DisplayEventEnteredMessage(IDictionary<String, object> currentmemdata, PaymentStates paymentState)
	{
		ShowEventConfirmation = true;
		litEventEntered.Text = paymentState.GetAttributeOfType<DescriptionAttribute>().Description;

        bool hasBTFNumber = string.IsNullOrWhiteSpace(currentmemdata[MemberProperty.BTFNumber] as string) == false;
        int costInPence = MembershipCostCalculator.EventCost(paymentState, hasBTFNumber);

	    litEventCost.Text = (costInPence / 100).ToString();
	}

	private void DisplaySwimSubsConfirmationMessage(PaymentStates paymentState)
	{
		ShowSwimSubsConfirmation = true;
		litSwimSubs.Text = paymentState.GetAttributeOfType<DescriptionAttribute>().Description;
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

	private void EnterMemberInTriFest(IDictionary<String, object> currentmemdata, PaymentStates paymentState)
	{
		currentmemdata[MemberProperty.TriFestEntry] = paymentState.GetAttributeOfType<DescriptionAttribute>().Description;
		MemberHelper.Update(currentmemdata);
	}

	private void EnterMemberInCharitySwim(IDictionary<String, object> currentmemdata, PaymentStates paymentState)
	{
		currentmemdata[MemberProperty.CharitySwimEntry] = paymentState.GetAttributeOfType<DescriptionAttribute>().Description;
		MemberHelper.Update(currentmemdata);
	}

	private void UpdateMemberSwimSubs(IDictionary<String, object> currentmemdata, PaymentStates paymentState)
	{
		if (paymentState == PaymentStates.SS05991)
		{
			currentmemdata[MemberProperty.swimSubsAprToSept] = true;
		}
		if (paymentState == PaymentStates.SS05992)
		{
			currentmemdata[MemberProperty.SwimSubsOctToMar] = true;
		}

		MemberHelper.Update(currentmemdata);
	}

}
