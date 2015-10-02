using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Mstc.Core.Domain;
using Mstc.Core.Providers;

public partial class usercontrols_cFront_MembershipOptions : System.Web.UI.UserControl
{
	private const string AcceptIndemnity = "Accept";

    protected void Page_Load(object sender, EventArgs e)
    {
	    if (Page.IsPostBack == false)
	    {
		    BindControls();
	    }
    }

	private void BindControls()
	{
		var membershipCostCalculator = new MembershipCostCalculator();

		var membershipTypes = new List<ListItem>()
		{
			new ListItem(
				string.Format("Individual membership - &pound;{0}",
					membershipCostCalculator.GetTypeCost(MembershipType.Individual, DateTime.Now)),
				((int) MembershipType.Individual).ToString()),
			new ListItem(
				string.Format(
					@"Couple membership - &pound;{0}<br /> <i>Only select this option if your partner will also be renewing their membership - The membership secretary will be checking!</i>",
					membershipCostCalculator.GetTypeCost(MembershipType.Couple, DateTime.Now)),
				((int) MembershipType.Couple).ToString()),
			new ListItem(
				string.Format("Unemployed/full-time student (18 years or above) - &pound;{0}",
					membershipCostCalculator.GetTypeCost(MembershipType.Concession, DateTime.Now)),
				((int) MembershipType.Concession).ToString())
		};
		membershipType.Items.AddRange(membershipTypes.ToArray());

		var extrasList = new List<ListItem>();

		if (DateTime.Now.Month < 7)
		{
			extrasList.Add(new ListItem("Swim subs January to June - &pound;30", MembershipExtras.SwimSubsJanToJune.ToString()));
		}
		extrasList.Add(new ListItem("Swim subs July to December - &pound;30", MembershipExtras.SwimSubsJulyToDec.ToString()));
	    
		extras.Items.AddRange(extrasList.ToArray());

		var indemnityOptionsList = new List<ListItem>()
	    {
		    new ListItem(
			    @"I have read and understand the open water swimming indemnity document.<br />I agree to and accept the terms without qualification.",
			    AcceptIndemnity),
		    new ListItem(
			    @"I do not accept the terms in the open water swimming indemnity document.<br />I understand I will not be elligible to take part in club open water swim sessions for this membership year.",
			    "NotAccepted")
	    };

		indemnityOptions.Items.AddRange(indemnityOptionsList.ToArray());
	}

	protected void CheckBoxRequired_ServerValidate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = volunteering.Checked;
	}

	public MembershipOptions GetMembershipOptions()
	{
		var swimSubJanToJuneItem = extras.Items.FindByValue(MembershipExtras.SwimSubsJanToJune.ToString());
		return new MembershipOptions()
		{
			MembershipType = (MembershipType) Enum.Parse(typeof(MembershipType), membershipType.SelectedValue),
			SwimSubsJanToJune = swimSubJanToJuneItem != null && swimSubJanToJuneItem.Selected,
			SwimSubsJulyToDec = extras.Items.FindByValue(MembershipExtras.SwimSubsJulyToDec.ToString()).Selected,
			OpenWaterIndemnityAcceptance = indemnityOptions.SelectedValue == AcceptIndemnity,
			Volunteering = true //Hardcode to true as can't renew unless this is selected :)
		};
	}
}