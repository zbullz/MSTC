using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco;
using Newtonsoft.Json;

public partial class usercontrols_cFront_RenewMemberComplete : System.Web.UI.UserControl
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
			IDictionary<String, object> currentmemdata = MemberHelper.Get();
			var membershipOptions = SessionProvider.RenewalOptions;
			if (currentmemdata == null || membershipOptions == null)
			{
				return; //Ensure user is logged in and request hasn't been duplicated
			}

			//lblQueryString.Text = Request.QueryString["resource_uri"];
		    if (Request.QueryString["resource_uri"] != null)
		    {
			    ConfirmPaymentRequest();
		    }
			UpdateMemberDetails(currentmemdata, membershipOptions);
	    }
    }

	private void ConfirmPaymentRequest()
	{
		var goCardlessProvider = new GoCardlessProvider();
		goCardlessProvider.ConfirmBill(Request.QueryString);
	}

	private void UpdateMemberDetails(IDictionary<String, object> currentmemdata, MembershipOptions membershipOptions)
	{
		currentmemdata[MemberProperty.membershipType] = membershipOptions.MembershipType.ToString();
		currentmemdata[MemberProperty.OpenWaterIndemnityAcceptance] = membershipOptions.OpenWaterIndemnityAcceptance;
		currentmemdata[MemberProperty.swimSubsJanToJune] = membershipOptions.SwimSubsJanToJune;
		currentmemdata[MemberProperty.SwimSubsJulyToDec] = membershipOptions.SwimSubsJulyToDec;
		currentmemdata[MemberProperty.Volunteering] = membershipOptions.Volunteering;
		currentmemdata[MemberProperty.MembershipExpiry] = new DateTime(DateTime.Now.Year + 1, 4, 1).ToString("yyyy-MM-dd");

		if (membershipOptions.OpenWaterIndemnityAcceptance)
		{
			//Calculate the next available swim auth number
			IMemberDal memberDal = new MemberDal(new DataConnection());
			IEnumerable<MemberOptionsDto> memberOptions = memberDal.GetMemberOptions();
			var membersWithSwimAuthNumbers = memberOptions.Where(m => m.SwimAuthNumber.HasValue).OrderBy(m => m.SwimAuthNumber);
			int swimAuthNumber = membersWithSwimAuthNumbers.Any()
				? membersWithSwimAuthNumbers.Last().SwimAuthNumber.Value + 1
				: 1;
			currentmemdata[MemberProperty.SwimAuthNumber] = swimAuthNumber;
		}

		MemberHelper.Update(currentmemdata);

		SessionProvider.RenewalOptions = null;
	}
}