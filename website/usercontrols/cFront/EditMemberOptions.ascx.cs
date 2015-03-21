using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Security;

public partial class usercontrols_cFront_EditMemberOptions : System.Web.UI.UserControl
{
	public bool EnableRenewal { get; set; }
	public bool EnableOpenWater { get; set; }
	public bool ShowMemberAdminLink { get; set; }

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
}