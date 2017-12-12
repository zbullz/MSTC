using System;
using System.Collections.Generic;
using cFront.Umbraco.Membership;
using Mstc.Core.Domain;
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

            var paymentSucceeded = CreatePayment(currentmemdata, membershipOptions);

		    if (paymentSucceeded)
		    {
		        _memberProvider.UpdateMemberOptions(member, membershipOptions);
		    }
		    else
		    {
                //Show failure message
		        ShowPaymentFailed = true;
		    }

		    _sessionProvider.RenewalOptions = null;
		}
	}

    private bool CreatePayment(IDictionary<String, object> currentmemdata, MembershipOptions membershipOptions)
    {
        string mandateId = currentmemdata[MemberProperty.directDebitMandateId] as string;
        string email = currentmemdata[MemberProperty.Email] as string;
        int costInPence = MembershipCostCalculator.Calculate(membershipOptions, DateTime.Now);
        string description = Request.QueryString["state"];

        bool paymentSucceeded = _goCardlessProvider.CreatePayment(mandateId, email, costInPence, description);

        return paymentSucceeded;
    }


}
