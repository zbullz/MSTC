using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web.Security;
using cFront.Umbraco.Membership;
using Mstc.Core.Domain;
using Mstc.Core.Providers;
using umbraco.cms.businesslogic.member;

public partial class usercontrols_cFront_EditMemberOptions : System.Web.UI.UserControl
{
    private readonly SessionProvider _sessionProvider;

    public bool IsGuest { get; set; }
	public bool EnableMemberRenewal { get; set; }
	public bool EnableOpenWater { get; set; }
	public bool ShowMemberAdminLink { get; set; }
	public bool ShowSwimAdminLink { get; set; }
	public bool ShowBuySwimSubs1 { get; set; }
	public bool ShowBuySwimSubs2 { get; set; }
	public bool EnableGuestUpgrade { get; set; }
	public bool EnableGuestRenewal { get; set; }
	public bool ShowIceLink { get; set; }

    public usercontrols_cFront_EditMemberOptions()
    {
        _sessionProvider = new SessionProvider();
    }

	protected void Page_Load(object sender, EventArgs e)
	{

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

			bool isGuest = memberType == MembershipType.Guest;
		    IsGuest = isGuest;
            EnableGuestUpgrade = isGuest && hasExpired == false;
			EnableGuestRenewal = isGuest && hasExpired;

			ShowBuySwimSubs1 = EnableMemberRenewal == false && memberType != MembershipType.Guest &&
			                   string.IsNullOrEmpty(memberData[MemberProperty.swimSubs1] as string) && DateTime.Now.Month < 10 && DateTime.Now.Month > 3;
            ShowBuySwimSubs2 = EnableMemberRenewal == false && memberType != MembershipType.Guest &&
                               string.IsNullOrEmpty(memberData[MemberProperty.swimSubs2] as string);

			membershipOptionalExtras.Text = string.Join("<br/>", OptionalExtras(memberData));

			bool openWaterEnabled = bool.Parse(ConfigurationManager.AppSettings["openWaterEnabled"]);

			EnableOpenWater = GetMemberBool(memberData, MemberProperty.OpenWaterIndemnityAcceptance) && openWaterEnabled;
			object swimAuthObj = memberData[MemberProperty.SwimAuthNumber];
			if (swimAuthObj != null && string.IsNullOrEmpty(swimAuthObj.ToString()) == false)
			{
				openWaterAuthNumber.Text = ((int) swimAuthObj).ToString("D3");
			}

			int swimBalanceLastYear = 0;
			int.TryParse(memberData[MemberProperty.SwimBalanceLastYear].ToString(), out swimBalanceLastYear);
			int creditsBought = 0;
			int.TryParse(memberData[MemberProperty.SwimCreditsBought].ToString(), out creditsBought);
			int creditsUsed = 0;
			int.TryParse(memberData[MemberProperty.SwimCreditsUsed].ToString(), out creditsUsed);
			litSwimCredits.Text = (swimBalanceLastYear + creditsBought - creditsUsed).ToString();

			hiddenEmail.Value = memberData[MemberProperty.Email].ToString();


            //Bind events
            string duathlonEntry = memberData[MemberProperty.DuathlonEntry] as string;

            if (string.IsNullOrWhiteSpace(duathlonEntry) == false)
			{
				EventList.Items.Add(duathlonEntry);
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
		ShowIceLink = roles.Contains("MemberAdmin") || roles.Contains("Coach");
	}

	private List<string> OptionalExtras(IDictionary<String, object> memberData)
	{
		var extras = new List<string>();
        string swimSub1 = memberData[MemberProperty.swimSubs1] as string;
        if (!string.IsNullOrWhiteSpace(swimSub1))
		{
			extras.Add(swimSub1);
		}
        string swimSub2 = memberData[MemberProperty.swimSubs2] as string;
        if (!string.IsNullOrWhiteSpace(swimSub2))
        {
            extras.Add(swimSub2);
        }
        if (GetMemberBool(memberData, MemberProperty.EnglandAthleticsMembership))
        {
            extras.Add("England Athletics Member.");
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

    public void btn_openWaterWaiverClick(object sender, EventArgs e)
    {
        var member = Member.GetCurrentMember();
        var memberProvider = new MemberProvider();
        memberProvider.AcceptOpenWaterWaiver(member);
        Response.Redirect(Request.RawUrl);
    }

    private void MakeSwimSubsPayment(PaymentStates paymentState)
	{
        RedirectToPaymentPages(paymentState);
    }

	private void MakeSwimPayment(PaymentStates paymentState)
	{
        RedirectToPaymentPages(paymentState);
	}

    private void RedirectToPaymentPages(PaymentStates state)
    {
        _sessionProvider.MandateSuccessPage = "payment-confirmation";
        _sessionProvider.CanProcessPaymentCompletion = true;

        var currentmemdata = MemberHelper.Get();
        bool hasMandate = string.IsNullOrWhiteSpace(currentmemdata[MemberProperty.directDebitMandateId] as string) == false;
        string page = hasMandate ? "payment-confirmation" : "mandate-request";

        string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
            Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
        string redirectUrl = string.Format("{0}/{1}?state={2}", rootUrl, page, state);
        Response.Redirect(redirectUrl);
    }
}