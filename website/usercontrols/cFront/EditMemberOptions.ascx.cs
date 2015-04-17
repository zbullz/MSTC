using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Security;
using GoCardlessSdk.Connect;

public partial class usercontrols_cFront_EditMemberOptions : System.Web.UI.UserControl
{
	protected MembershipCostCalcualtor _membershipCostCalcualtor;

	public bool EnableRenewal { get; set; }
	public bool EnableOpenWater { get; set; }
	public bool ShowMemberAdminLink { get; set; }

	protected void Page_Load(object sender, EventArgs e)
	{
		_membershipCostCalcualtor = new MembershipCostCalcualtor();
	}

	public void LoadOptions(IDictionary<String, object> memberData)
	{

		if (memberData != null)
		{
			var membershipExpiryDate = memberData[MemberProperty.MembershipExpiry] as DateTime?;
			bool hasExpired = membershipExpiryDate.HasValue == false || membershipExpiryDate.Value < DateTime.Now;
			membershipExpiry.Text = hasExpired ? "Expired" : membershipExpiryDate.Value.ToString("dd MMM yyyy");

			bool renewalsEnabled = bool.Parse(ConfigurationManager.AppSettings["renewalsEnabled"]);
			if (renewalsEnabled && (membershipExpiryDate.HasValue == false || membershipExpiryDate.Value.Year <= DateTime.Now.Year))
			{
				EnableRenewal = true;
			}
			else
			{
				EnableRenewal = false;
			}

			membershipType.Text = memberData[MemberProperty.membershipType] as string;

			membershipOptionalExtras.Text = string.Join("<br/>", OptionalExtras(memberData));
			
		    EnableOpenWater = GetMemberBool(memberData, MemberProperty.OpenWaterIndemnityAcceptance);
			if (EnableOpenWater)
			{
				object swimAuthObj = memberData[MemberProperty.SwimAuthNumber];
				if (swimAuthObj != null && string.IsNullOrEmpty(swimAuthObj.ToString()) == false)
				{
					openWaterAuthNumber.Text = ((int)swimAuthObj).ToString("D3");
				}
				litSwimCredits.Text = memberData[MemberProperty.SwimCredits].ToString();
				hiddenEmail.Value = memberData[MemberProperty.Email].ToString();
			}
		}

		string[] roles = Roles.GetRolesForUser();
		ShowMemberAdminLink = roles.Contains("MemberAdmin");
	}

	private List<string> OptionalExtras(IDictionary<String, object> memberData)
	{
		var extras = new List<string>();
		
        if (GetMemberBool(memberData, MemberProperty.swimSubsJanToJune))
		{
			extras.Add("Pool swim Jan - June.");
		}
		if (GetMemberBool(memberData, MemberProperty.SwimSubsJulyToDec))
		{
			extras.Add("Pool swim July - Dec.");
		}

        if (extras.Any() == false)
        {
            extras.Add("None");
        }

		return extras;
	}

    private bool GetMemberBool(IDictionary<String, object> memberData, string memberPropertyName)
    {
        bool value = false;
        object propertyValue = memberData[memberPropertyName];
        if (propertyValue != null)
        {
            int valueInt;
            if (int.TryParse(propertyValue.ToString(), out valueInt))
            {
                value = valueInt == 1;
            }
        }
        return value;
    }

	public void btn_5SwimCreditsClick(object sender, EventArgs e)
	{
		MakeSwimPayment(PaymentStates.S00599C);
	}

	public void btn_10SwimCreditsClick(object sender, EventArgs e)
	{
		MakeSwimPayment(PaymentStates.S001099C);
	}

	public void btn_15SwimCreditsClick(object sender, EventArgs e)
	{
		MakeSwimPayment(PaymentStates.S001599C);
	}

	private void MakeSwimPayment(PaymentStates paymentState)
	{
		var goCardlessProvider = new GoCardlessProvider();
		var redirectUrl = goCardlessProvider.CreateSimpleBill(hiddenEmail.Value, _membershipCostCalcualtor.SwimCreditsCost(paymentState),
			"Open water swim credits",
			string.Format("Open water swim, {0} credits", (int)paymentState), paymentState, Request.Url);

		var sessionProvider = new SessionProvider();
		sessionProvider.CanProcessPaymentCompletion = true;
		//RedirectToCompletePage(paymentState.ToString()); //Can be used for testing
		Response.Redirect(redirectUrl);
	}

	private void RedirectToCompletePage(string state)
	{
		string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
			Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
		string redirectUrl = string.Format("{0}/payment-complete?state={1}", rootUrl, state);
		Response.Redirect(redirectUrl);
	}

	
}