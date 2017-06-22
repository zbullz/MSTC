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
				string.Format("Youth (age 16-17), student (18+) or unemployed - &pound;{0}",
					membershipCostCalculator.GetTypeCost(MembershipType.Concession, DateTime.Now)),
				((int) MembershipType.Concession).ToString())
		};
		membershipType.Items.AddRange(membershipTypes.ToArray());

		var extrasList = new List<ListItem>();

		if (2 < DateTime.Now.Month && DateTime.Now.Month < 10)
		{
			extrasList.Add(new ListItem("Swim subs April to Sept - &pound;30", MembershipExtras.SwimSubsAprToSept.ToString()));
		}
		extrasList.Add(new ListItem("Swim subs Oct to March - &pound;30", MembershipExtras.SwimSubsOctToMar.ToString()));
	    
		extras.Items.AddRange(extrasList.ToArray());

		var indemnityOptionsList = new List<ListItem>()
	    {
		    new ListItem(
			    @"I have read and understand the open water swimming indemnity document.<br />I agree to and accept the terms without qualification and agree to be included in the duty rota.",
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
		var swimSubAprToSeptItem = extras.Items.FindByValue(MembershipExtras.SwimSubsAprToSept.ToString());
		return new MembershipOptions()
		{
			MembershipType = (MembershipType) Enum.Parse(typeof(MembershipType), membershipType.SelectedValue),
			SwimSubsAprToSept = swimSubAprToSeptItem != null && swimSubAprToSeptItem.Selected,
			SwimSubsOctToMar = extras.Items.FindByValue(MembershipExtras.SwimSubsOctToMar.ToString()).Selected,
			OpenWaterIndemnityAcceptance = indemnityOptions.SelectedValue == AcceptIndemnity,
			Volunteering = true //Hardcode to true as can't renew unless this is selected :)
		};
	}
}