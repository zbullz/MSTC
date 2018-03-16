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
		var membershipTypes = new List<ListItem>()
		{
			new ListItem(
				string.Format("Individual membership - &pound;{0}", 
                (decimal)MembershipCostCalculator.GetTypeCostPence(MembershipType.Individual, DateTime.Now)/100),
				((int) MembershipType.Individual).ToString()),
			new ListItem(
				string.Format(
					@"Couple membership - &pound;{0}<br /> <i>Only select this option if your partner will also be renewing their membership - The membership secretary will be checking!</i>",
                    (decimal)MembershipCostCalculator.GetTypeCostPence(MembershipType.Couple, DateTime.Now)/100),
				((int) MembershipType.Couple).ToString()),
			new ListItem(
				string.Format("Youth (age 16-17), student (18+) or unemployed - &pound;{0}",
                    (decimal)MembershipCostCalculator.GetTypeCostPence(MembershipType.Concession, DateTime.Now)/100),
				((int) MembershipType.Concession).ToString())
		};
		membershipType.Items.AddRange(membershipTypes.ToArray());

		var extrasList = new List<ListItem>();

		if (2 < DateTime.Now.Month && DateTime.Now.Month < 10)
		{
			extrasList.Add(new ListItem("Swim subs April to Sept - &pound;30", MembershipExtras.SwimSubsAprToSept.ToString()));
		}
		extrasList.Add(new ListItem("Swim subs Oct to March - &pound;30", MembershipExtras.SwimSubsOctToMar.ToString()));
		extrasList.Add(new ListItem("England Athletics Membership* - &pound;15", MembershipExtras.EnglandAthletics.ToString()));

        extras.Items.AddRange(extrasList.ToArray());

		var indemnityOptionsList = new List<ListItem>()
	    {
		    new ListItem(
                @"I have read and understand the open water swimming indemnity document.<br />I agree to and accept the terms without qualification and agree to be included in the duty rota for OWS by attending sessions as a spotter or kayaker.",
			    AcceptIndemnity),
		    new ListItem(
                @"I do not wish to take part in open water swimming this season.",
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
            EnglandAthleticsMembership = extras.Items.FindByValue(MembershipExtras.EnglandAthletics.ToString()).Selected,
            OpenWaterIndemnityAcceptance = indemnityOptions.SelectedValue == AcceptIndemnity,
			Volunteering = true //Hardcode to true as can't renew unless this is selected :)
		};
	}
}