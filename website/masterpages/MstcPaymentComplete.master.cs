using System;
using System.Collections.Generic;
using System.ComponentModel;
using cFront.Umbraco.Membership;
using Mstc.Core.Domain;
using Mstc.Core.Dto;
using Mstc.Core.Providers;
using System.Linq;
using umbraco.cms.businesslogic.member;

public partial class masterpages_MstcPaymentComplete : System.Web.UI.MasterPage
{
    private SessionProvider _sessionProvider;
    private GoCardlessProvider _goCardlessProvider;
    private MemberProvider _memberProvider;

    protected bool HasPaymentDetails= true;
    protected bool PaymentConfirmed = false;
    protected bool ShowPaymentFailed = false;
    protected bool ShowSwimCreditsConfirmation = false;
    protected bool ShowEventConfirmation = false;
    protected bool ShowSwimSubsConfirmation = false;
    protected bool ShowRenewed = false;

    protected string PaymentDescription = "";
    protected string Cost = "";

    public masterpages_MstcPaymentComplete()
    {
        _sessionProvider = new SessionProvider();
        _goCardlessProvider = new GoCardlessProvider();
        _memberProvider = new MemberProvider();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        IDictionary<String, object> currentmemdata = MemberHelper.Get();
        string state = Request.QueryString["state"];
        if (currentmemdata == null || _sessionProvider.CanProcessPaymentCompletion == false || string.IsNullOrEmpty(state))
        {
            HasPaymentDetails = false;
            return; //Ensure the form is behind a login form and not a duplicate request
        }

        var paymentState = (PaymentStates)Enum.Parse(typeof(PaymentStates), state);

        string mandateId = currentmemdata[MemberProperty.directDebitMandateId] as string;
        if (string.IsNullOrWhiteSpace(_sessionProvider.GoCardlessRedirectFlowId) == false)
        {
            //Complete the mandate request if required
            mandateId = _goCardlessProvider.CompleteRedirectRequest(_sessionProvider.GoCardlessRedirectFlowId,
                _sessionProvider.SessionId);
            currentmemdata[MemberProperty.directDebitMandateId] = mandateId;
            MemberHelper.Update(currentmemdata);
            _sessionProvider.GoCardlessRedirectFlowId = null;
        } else if (string.IsNullOrEmpty(mandateId)) {
            RedirectToMandatePage(paymentState);
        }
        
        PaymentDescription = paymentState.GetAttributeOfType<DescriptionAttribute>().Description;

        MembershipType membershipType;
        Enum.TryParse(currentmemdata[MemberProperty.membershipType] as string, out membershipType);
        //bool hasBTFNumber = string.IsNullOrWhiteSpace(currentmemdata[MemberProperty.BTFNumber] as string) == false;    
        bool hasBTFNumber = true; //Hardcode this as BTF registration is no longer required
        int costInPence = (paymentState == PaymentStates.MemberRenewal || paymentState == PaymentStates.MemberUpgrade) 
            ? MembershipCostCalculator.Calculate(_sessionProvider.RenewalOptions, DateTime.Now) 
            : MembershipCostCalculator.PaymentStateCost(paymentState, hasBTFNumber, membershipType);

        Cost = (costInPence / 100m).ToString("N2");
    }

    protected void Confirm_OnClick(object sender, EventArgs e)
    {
        IDictionary<String, object> currentmemdata = MemberHelper.Get();
        if (currentmemdata == null || _sessionProvider.CanProcessPaymentCompletion == false || Request.QueryString["state"] == null)
        {
            return; //Ensure the form is behind a login form and not a duplicate request
        }        

        string state = Request.QueryString["state"];
        var paymentState = (PaymentStates)Enum.Parse(typeof(PaymentStates), state);

        var paymentResponse = CreatePayment(currentmemdata, paymentState);

        if (paymentResponse == PaymentResponseDto.Success)
        {
            ProcessPaymentState(currentmemdata, paymentState);
            PaymentConfirmed = true;

        }
        else if (paymentResponse == PaymentResponseDto.MandateError)
        {
            RedirectToMandatePage(paymentState);
        }
        else
        {
            ShowPaymentFailed = true;
        }

        _sessionProvider.CanProcessPaymentCompletion = false;
    }

    private void RedirectToMandatePage(PaymentStates state)
    {
        _sessionProvider.MandateSuccessPage = "payment-confirmation";
        string page = "mandate-request";

        string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
            Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
        string redirectUrl = string.Format("{0}/{1}?state={2}", rootUrl, page, state);
        Response.Redirect(redirectUrl);
    }

    private void ProcessPaymentState(IDictionary<String, object> currentmemdata, PaymentStates paymentState)
    {
        switch (paymentState)
        {
            case PaymentStates.S00199C:
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
            case PaymentStates.E00D102C:
            case PaymentStates.E00D103C:
                {
                EnterMemberInDuathlon(currentmemdata, paymentState);
                DisplayEventEnteredMessage(currentmemdata, paymentState);
                break;
            }
            case PaymentStates.E00TRIOI201C:
            case PaymentStates.E00TRIOR202C:
            case PaymentStates.E00TRIMI203C:
            case PaymentStates.E00TRIMR204C:
            case PaymentStates.E00TRISI205C:
            case PaymentStates.E00TRISR206C:
            case PaymentStates.E00AOI207C:
            case PaymentStates.E00AOR208C:
            case PaymentStates.E00AMI209C:
            case PaymentStates.E00AMR210C:
            case PaymentStates.E00ASI211C:
            case PaymentStates.E00ASR212C:
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
                EnterMemberInCharitySwim(currentmemdata, paymentState);
                DisplayEventEnteredMessage(currentmemdata, paymentState);
                break;
            }
            case PaymentStates.SS05991:
            case PaymentStates.SS05992:
            case PaymentStates.SS05996:
            {
                UpdateMemberSwimSubs(currentmemdata, paymentState);
                DisplaySwimSubsConfirmationMessage(currentmemdata, paymentState);
                break;
            }
            case PaymentStates.MemberRenewal:
            case PaymentStates.MemberUpgrade:
                {
                RenewOrUpgradeMember(paymentState);
                break;
            }
        }
    }

    private PaymentResponseDto CreatePayment(IDictionary<String, object> currentmemdata, PaymentStates paymentState)
    {
        MembershipType membershipType;
        Enum.TryParse(currentmemdata[MemberProperty.membershipType] as string, out membershipType);

        string mandateId = currentmemdata[MemberProperty.directDebitMandateId] as string;
        string email = currentmemdata[MemberProperty.Email] as string;
        bool hasBTFNumber = string.IsNullOrWhiteSpace(currentmemdata[MemberProperty.BTFNumber] as string) == false; 
        int costInPence = (paymentState == PaymentStates.MemberRenewal || paymentState == PaymentStates.MemberUpgrade)
            ? MembershipCostCalculator.Calculate(_sessionProvider.RenewalOptions, DateTime.Now) 
            : MembershipCostCalculator.PaymentStateCost(paymentState, hasBTFNumber, membershipType);
        string description = paymentState.GetAttributeOfType<DescriptionAttribute>().Description;

        return _goCardlessProvider.CreatePayment(mandateId, email, costInPence, description);
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

        MembershipType membershipType;
        Enum.TryParse(currentmemdata[MemberProperty.membershipType] as string, out membershipType);
        int costInPence = MembershipCostCalculator.PaymentStateCost(paymentState, hasBTFNumber, membershipType);

	    litEventCost.Text = (costInPence / 100m).ToString("N2");
	}

	private void DisplaySwimSubsConfirmationMessage(IDictionary<String, object> currentmemdata, PaymentStates paymentState)
	{
		ShowSwimSubsConfirmation = true;
		litSwimSubs.Text = paymentState.GetAttributeOfType<DescriptionAttribute>().Description;

        MembershipType membershipType;
        Enum.TryParse(currentmemdata[MemberProperty.membershipType] as string, out membershipType);

	    litSwimSubsCost.Text = (((decimal) MembershipCostCalculator.SwimsSubsCostInPence(membershipType) ) / 100).ToString();
	}

	private void UpdateMemberSwimCredits(IDictionary<String, object> currentmemdata, int credits)
	{
		int memberCredits = 0;
		int.TryParse(currentmemdata[MemberProperty.SwimCreditsBought].ToString(), out memberCredits);
	
		memberCredits += credits;
		currentmemdata[MemberProperty.SwimCreditsBought] = memberCredits;

		MemberHelper.Update(currentmemdata);
	}

	private void EnterMemberInDuathlon(IDictionary<String, object> currentmemdata, PaymentStates paymentState)
	{	
		currentmemdata[MemberProperty.DuathlonEntry] = string.Format("{0} - {1}", paymentState.GetAttributeOfType<DescriptionAttribute>().Description, DateTime.Now.Year);
        MemberHelper.Update(currentmemdata);
	}

	private void EnterMemberInTriFest(IDictionary<String, object> currentmemdata, PaymentStates paymentState)
	{
		currentmemdata[MemberProperty.TriFestEntry] = string.Format("{0} - {1}", paymentState.GetAttributeOfType<DescriptionAttribute>().Description, DateTime.Now.Year);
		MemberHelper.Update(currentmemdata);
	}

	private void EnterMemberInCharitySwim(IDictionary<String, object> currentmemdata, PaymentStates paymentState)
	{
		currentmemdata[MemberProperty.CharitySwimEntry] = string.Format("{0} - {1}", paymentState.GetAttributeOfType<DescriptionAttribute>().Description, DateTime.Now.Year);
		MemberHelper.Update(currentmemdata);
	}

	private void UpdateMemberSwimSubs(IDictionary<String, object> currentmemdata, PaymentStates paymentState)
	{
		if (paymentState == PaymentStates.SS05991)
		{
			currentmemdata[MemberProperty.swimSubs1] = string.Format("Swim Subs Apr - Sept {0}", DateTime.Now.Year);
		}
		if (paymentState == PaymentStates.SS05992)
		{
            var janToMarch = new List<int>() { 1,2,3};            
            int year1 = janToMarch.Any(m => m == DateTime.Now.Month) ? DateTime.Now.Year - 1: DateTime.Now.Year;
            currentmemdata[MemberProperty.swimSubs2] = string.Format("Swim Subs Oct {0} - Mar {1}", year1, year1 + 1);
		}

		MemberHelper.Update(currentmemdata);
	}

    private void RenewOrUpgradeMember(PaymentStates paymentState)
    {
        ShowRenewed = true;
        var member = Member.GetCurrentMember();
        _memberProvider.UpdateMemberOptions(member, _sessionProvider.RenewalOptions, resetEventEntries: paymentState == PaymentStates.MemberRenewal, isUpgrade: paymentState == PaymentStates.MemberUpgrade);
    }

}
