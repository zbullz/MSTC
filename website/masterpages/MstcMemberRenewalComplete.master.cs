using System;
using System.Collections.Generic;
using cFront.Umbraco.Membership;
using Mstc.Core.Domain;
using Mstc.Core.Dto;
using Mstc.Core.Providers;
using umbraco.cms.businesslogic.member;

public partial class masterpages_MstcMemberRenewalComplete : System.Web.UI.MasterPage
{
    protected SessionProvider _sessionProvider;
    private GoCardlessProvider _goCardlessProvider;
    private MemberProvider _memberProvider;

    protected bool ShowPaymentFailed = false;

    public masterpages_MstcMemberRenewalComplete()
    {
        _sessionProvider = new SessionProvider();
        _goCardlessProvider = new GoCardlessProvider();
        _memberProvider = new MemberProvider();
    }


	protected void Page_Load(object sender, EventArgs e)
	{
		if (IsPostBack == false)
		{
			var member = Member.GetCurrentMember();

            MembershipOptions membershipOptions = _sessionProvider.RenewalOptions;
			if (member == null || membershipOptions == null)
			{
				return; //Ensure user is logged in and request hasn't been duplicated
			}

            IDictionary<String, object> currentmemdata = MemberHelper.Get();
            if (string.IsNullOrWhiteSpace(_sessionProvider.GoCardlessRedirectFlowId) == false)
            {
                //Complete the mandate request if required
                string mandateId = _goCardlessProvider.CompleteRedirectRequest(_sessionProvider.GoCardlessRedirectFlowId,
                    _sessionProvider.SessionId);
                currentmemdata[MemberProperty.directDebitMandateId] = mandateId;
                MemberHelper.Update(currentmemdata);
                _sessionProvider.GoCardlessRedirectFlowId = null;
            }

            var paymentResponse = CreatePayment(currentmemdata, membershipOptions);

		    if (paymentResponse == PaymentResponseDto.Success)
		    {
		        _memberProvider.UpdateMemberOptions(member, membershipOptions);
                int costInPence = MembershipCostCalculator.Calculate(membershipOptions, DateTime.Now);
                litCost.Text = (costInPence / 100m).ToString("N2");
            }
            else if (paymentResponse == PaymentResponseDto.MandateError)
		    {
		        RedirectToMandatePage(Request.QueryString["state"]);
		    }
		    else
		    {
                //Show failure message
		        ShowPaymentFailed = true;
		    }

		    _sessionProvider.RenewalOptions = null;
		}
	}

    private PaymentResponseDto CreatePayment(IDictionary<String, object> currentmemdata, MembershipOptions membershipOptions)
    {
        string mandateId = currentmemdata[MemberProperty.directDebitMandateId] as string;
        string email = currentmemdata[MemberProperty.Email] as string;
        int costInPence = MembershipCostCalculator.Calculate(membershipOptions, DateTime.Now);
        string description = Request.QueryString["state"];

        return _goCardlessProvider.CreatePayment(mandateId, email, costInPence, description);
    }

    private void RedirectToMandatePage(string state)
    {
        _sessionProvider.MandateSuccessPage = "members-area/membership-renewal-complete";
        string page = "mandate-request";

        _sessionProvider.CanProcessPaymentCompletion = true;

        string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
            Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
        string redirectUrl = string.Format("{0}/{1}?state={2}", rootUrl, page, state);
        Response.Redirect(redirectUrl);
    }


}
