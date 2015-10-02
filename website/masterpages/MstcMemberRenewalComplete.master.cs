using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco;
using Mstc.Core.Domain;
using Mstc.Core.Providers;
using umbraco.cms.businesslogic.member;

public partial class masterpages_MstcMemberRenewalComplete : System.Web.UI.MasterPage
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
		if (IsPostBack == false)
		{
			var member = Member.GetCurrentMember();
			IDictionary<String, object> currentmemdata = MemberHelper.Get();
			litAction.Text = IsRenewing(currentmemdata) ? "renewing" : "upgrading";

			var membershipOptions = SessionProvider.RenewalOptions;
			if (member == null || membershipOptions == null)
			{
				return; //Ensure user is logged in and request hasn't been duplicated
			}

			//lblQueryString.Text = Request.QueryString["resource_uri"];
			if (Request.QueryString["resource_uri"] != null)
			{
				ConfirmPaymentRequest();
			}
			var memberProvider = new MemberProvider();
			memberProvider.UpdateMemberOptions(member, membershipOptions);

			SessionProvider.RenewalOptions = null;
		}
	}

	private void ConfirmPaymentRequest()
	{
		var goCardlessProvider = new GoCardlessProvider();
		goCardlessProvider.ConfirmBill(Request.QueryString);
	}

	private bool IsRenewing(IDictionary<String, object> currentmemdata)
	{
		string membershipTypeValue = currentmemdata[MemberProperty.membershipType] as string;
		if (string.IsNullOrWhiteSpace(membershipTypeValue) == false)
		{
			//Sadly there is no nicer way to do this as umbraco gives us an int as a string object! 
			int membershipTypeInt;
			if (int.TryParse(membershipTypeValue, out membershipTypeInt))
			{
				return (MembershipType) membershipTypeInt != MembershipType.Guest;
			}
		}

		return true;

	}
}
