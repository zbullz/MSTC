using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using cFront.Umbraco.Membership;

public partial class usercontrols_cFront_RenewMember : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

	protected void RenewMember_OnClick(object sender, EventArgs e)
	{
		if (Page.IsValid == false)
		{
			return;
		}

		IDictionary<String, object> currentmemdata = MemberHelper.Get();
		if (currentmemdata == null)
		{
			return; //Ensure the form is behind a login form
		}
		
		MembershipOptions membershipOptions = membershipOptionsControl.GetMembershipOptions();
		lblMemberOptions.Text = JsonConvert.SerializeObject(membershipOptions);

		currentmemdata[MemberProperty.membershipType] = membershipOptions.MembershipType.ToString();
		currentmemdata[MemberProperty.OpenWaterIndemnityAcceptance] = membershipOptions.OpenWaterIndemnityAcceptance;
		currentmemdata[MemberProperty.swimSubsJanToJune] = membershipOptions.SwimSubsJanToJune;
		currentmemdata[MemberProperty.SwimSubsJulyToDec] = membershipOptions.SwimSubsJulyToDec;
		currentmemdata[MemberProperty.CoreSubsAprilToSept] = membershipOptions.CoreSubsAprilToSept;
		currentmemdata[MemberProperty.CoreSubsOctToMarch] = membershipOptions.CoreSubsOctToMarch;
		currentmemdata[MemberProperty.Volunteering] = membershipOptions.Volunteering;

		MemberHelper.Update(currentmemdata);
	}
}