using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class usercontrols_cFront_EditMemberOptions : System.Web.UI.UserControl
{
	public bool EnableRenewal { get; set; }
	public bool EnableOpenWater { get; set; }

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

			if (membershipExpiryDate.HasValue == false || membershipExpiryDate.Value.Year <= DateTime.Now.Year)
			{
				EnableRenewal = true;
			}
			else
			{
				EnableRenewal = false;
			}

			membershipType.Text = memberData[MemberProperty.membershipType] as string;

			membershipOptionalExtras.Text = string.Join("<br/>", OptionalExtras(memberData));

			int? openWaterIndemnityAcceptance = (int?)memberData[MemberProperty.OpenWaterIndemnityAcceptance];
			EnableOpenWater = openWaterIndemnityAcceptance.HasValue && openWaterIndemnityAcceptance == 1;
		}
	}

	private List<string> OptionalExtras(IDictionary<String, object> memberData)
	{
		var extras = new List<string>();

		int? swimSubsJan = (int?) memberData[MemberProperty.swimSubsJanToJune];
		if (swimSubsJan.HasValue && swimSubsJan == 1)
		{
			extras.Add("Pool swim Jan - June.");
		}
		int? swimSubsJuly = (int?)memberData[MemberProperty.SwimSubsJulyToDec];
		if (swimSubsJuly.HasValue && swimSubsJuly == 1)
		{
			extras.Add("Pool swim July - Dec.");
		}
		int? core1 = (int?)memberData[MemberProperty.CoreSubsAprilToSept];
		if (core1.HasValue && core1 == 1)
		{
			extras.Add("Core/Spin April - Sept.");
		}
		int? core2 = (int?)memberData[MemberProperty.CoreSubsOctToMarch];
		if (core2.HasValue && core2 == 1)
		{
			extras.Add("Core/Spin Oct - March.");
		}

		return extras;
	}
}