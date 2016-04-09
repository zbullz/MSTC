using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Security;
using GoCardlessSdk.Connect;
using Mstc.Core.Domain;
using Mstc.Core.Providers;

public partial class usercontrols_cFront_EditMemberOptions : System.Web.UI.UserControl
{
	protected MembershipCostCalculator _membershipCostCalcualtor;

	public bool EnableMemberRenewal { get; set; }
	public bool EnableOpenWater { get; set; }
	public bool ShowMemberAdminLink { get; set; }
	public bool ShowSwimAdminLink { get; set; }
	public bool ShowBuySwimSubsSpecial { get; set; }
	public bool ShowBuySwimSubs1 { get; set; }
	public bool ShowBuySwimSubs2 { get; set; }
	public bool IsGuest { get; set; }

	protected void Page_Load(object sender, EventArgs e)
	{
		_membershipCostCalcualtor = new MembershipCostCalculator();
	}

	public void LoadOptions(IDictionary<String, object> memberData)
	{

		if (memberData != null)
		{
			var membershipExpiryDate = memberData[MemberProperty.MembershipExpiry] as DateTime?;
			bool hasExpired = membershipExpiryDate.HasValue == false || membershipExpiryDate.Value < DateTime.Now;
			membershipExpiry.Text = hasExpired ? "Expired" : membershipExpiryDate.Value.ToString("dd MMM yyyy");

			string membershipTypeValue = memberData[MemberProperty.membershipType] as string;
			var memberType = MembershipType.Individual;
			if (string.IsNullOrWhiteSpace(membershipTypeValue) == false)
			{
				//Sadly there is no nicer way to do this as umbraco gives us an int as a string object! 
				int membershipTypeInt;
				if (int.TryParse(membershipTypeValue, out membershipTypeInt))
				{
					memberType = (MembershipType) membershipTypeInt;
					membershipType.Text = ((MembershipType) membershipTypeInt).ToString();
				}
			}

			bool renewalsEnabled = bool.Parse(ConfigurationManager.AppSettings["renewalsEnabled"]);
			if (renewalsEnabled &&
			    DateTime.Now.Month > 2 &&
			    memberType != MembershipType.Guest &&
			    (membershipExpiryDate.HasValue == false || membershipExpiryDate.Value.Year <= DateTime.Now.Year))
			{
				EnableMemberRenewal = true;
			}
			else
			{
				EnableMemberRenewal = false;
			}

			IsGuest = memberType == MembershipType.Guest;

			ShowBuySwimSubsSpecial = memberType != MembershipType.Guest &&
			                         GetMemberBool(memberData, MemberProperty.swimSubsJanToMar) == false &&
			                         DateTime.Now.Month <= 3;
			ShowBuySwimSubs1 = memberType != MembershipType.Guest &&
			                   GetMemberBool(memberData, MemberProperty.swimSubsAprToSept) == false && DateTime.Now.Month <= 9;
			ShowBuySwimSubs2 = memberType != MembershipType.Guest &&
			                   GetMemberBool(memberData, MemberProperty.SwimSubsOctToMar) == false;

			membershipOptionalExtras.Text = string.Join("<br/>", OptionalExtras(memberData));

			EnableOpenWater = GetMemberBool(memberData, MemberProperty.OpenWaterIndemnityAcceptance);
			object swimAuthObj = memberData[MemberProperty.SwimAuthNumber];
			if (swimAuthObj != null && string.IsNullOrEmpty(swimAuthObj.ToString()) == false)
			{
				openWaterAuthNumber.Text = ((int) swimAuthObj).ToString("D3");
			}

			int creditsBought = 0;
			int.TryParse(memberData[MemberProperty.SwimCreditsBought].ToString(), out creditsBought);
			int creditsUsed = 0;
			int.TryParse(memberData[MemberProperty.SwimCreditsUsed].ToString(), out creditsUsed);
			litSwimCredits.Text = (creditsBought - creditsUsed).ToString();

			hiddenEmail.Value = memberData[MemberProperty.Email].ToString();


			//Bind events
			if (GetMemberBool(memberData, MemberProperty.DuathlonEntered))
			{
				EventList.Items.Add("Duathlon");
			}
			string triFestEntry = memberData[MemberProperty.TriFestEntry] as string;
			if (string.IsNullOrWhiteSpace(triFestEntry) == false)
			{
				EventList.Items.Add(triFestEntry);
			}
			string charitySwimEntry = memberData[MemberProperty.CharitySwimEntry] as string;
			if (string.IsNullOrWhiteSpace(charitySwimEntry) == false)
			{
				EventList.Items.Add(charitySwimEntry);
			}
		}

		string[] roles = Roles.GetRolesForUser();
		ShowMemberAdminLink = roles.Contains("MemberAdmin");
		ShowSwimAdminLink = roles.Contains("SwimAdmin");
	}

	private List<string> OptionalExtras(IDictionary<String, object> memberData)
	{
		var extras = new List<string>();
		if (GetMemberBool(memberData, MemberProperty.swimSubsJanToMar))
		{
			extras.Add("Pool swim Jan - Mar.");
		}
        if (GetMemberBool(memberData, MemberProperty.swimSubsAprToSept))
		{
			extras.Add("Pool swim Apr - Sept.");
		}
		if (GetMemberBool(memberData, MemberProperty.SwimSubsOctToMar))
		{
			extras.Add("Pool swim Oct - Mar.");
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
        object propertyValue;
	    memberData.TryGetValue(memberPropertyName, out propertyValue);
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

	public void btn_BuySwimSubsSpecialClick(object sender, EventArgs e)
	{
		MakeSwimSubsPayment(PaymentStates.SS05996);
	}

	public void btn_BuySwimSubs1Click(object sender, EventArgs e)
	{
		MakeSwimSubsPayment(PaymentStates.SS05991);
	}

	public void btn_BuySwimSubs2Click(object sender, EventArgs e)
	{
		MakeSwimSubsPayment(PaymentStates.SS05992);
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

	private void MakeSwimSubsPayment(PaymentStates paymentState)
	{
		var goCardlessProvider = new GoCardlessProvider();

		MembershipType memberType = (MembershipType)Enum.Parse(typeof(MembershipType), membershipType.Text);
		var redirectUrl = goCardlessProvider.CreateSimpleBill(hiddenEmail.Value, paymentState == PaymentStates.SS05996 ? 15m : 30m,
			"Swim subs payment",
			string.Format("{0}", paymentState.GetAttributeOfType<DescriptionAttribute>().Description), paymentState, Request.Url);

		var sessionProvider = new SessionProvider();
		sessionProvider.CanProcessPaymentCompletion = true;
		//RedirectToCompletePage(paymentState.ToString()); //Can be used for testing
		Response.Redirect(redirectUrl);
	}

	private void MakeSwimPayment(PaymentStates paymentState)
	{
		var goCardlessProvider = new GoCardlessProvider();

		MembershipType memberType = (MembershipType)Enum.Parse(typeof(MembershipType), membershipType.Text);
		var redirectUrl = goCardlessProvider.CreateSimpleBill(hiddenEmail.Value, _membershipCostCalcualtor.SwimCreditsCost(paymentState, memberType),
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