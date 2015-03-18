﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco;
using Newtonsoft.Json;

public partial class usercontrols_cFront_RenewMemberComplete : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if (IsPostBack == false)
		{
			//lblQueryString.Text = Request.QueryString["resource_uri"];
		    if (Request.QueryString["resource_uri"] != null)
		    {
			    ConfirmPaymentRequest();
		    }
		    UpdateMemberDetails();
	    }
    }

	private void ConfirmPaymentRequest()
	{
		var goCardlessProvider = new GoCardlessProvider();
		goCardlessProvider.ConfirmBill(Request.QueryString);
	}

	private void UpdateMemberDetails()
	{
		IDictionary<String, object> currentmemdata = MemberHelper.Get();
		if (currentmemdata == null)
		{
			return; //Ensure the form is behind a login form
		}

		var sessionProvider = new SessionProvider();
		var membershipOptions = sessionProvider.RenewalOptions;

		//lblMemberOptions.Text = JsonConvert.SerializeObject(membershipOptions);

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
	}
}