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
        var membershipProvider = new MemberProvider();

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
				string.Format("Concession: Youth (16-17) / Student / Unemployed - &pound;{0}",
                    (decimal)MembershipCostCalculator.GetTypeCostPence(MembershipType.Concession, DateTime.Now)/100),
				((int) MembershipType.Concession).ToString())
		};
		membershipType.Items.AddRange(membershipTypes.ToArray());

		var extrasList = new List<ListItem>();

		if (2 < DateTime.Now.Month && DateTime.Now.Month < 10)
		{
            string swim1Desc = string.Format("Swim subs {0} - Standard &pound;30 / Concessions &pound;15", membershipProvider.GetSwimSub1Description(DateTime.Now));
            //extrasList.Add(new ListItem(swim1Desc, MembershipExtras.SwimSubsAprToSept.ToString())); //COVID-19 change
		}
        string swim2Desc = string.Format("Swim subs {0} - Standard &pound;30 / Concessions &pound;15", membershipProvider.GetSwimSub2Description(DateTime.Now));

		//extrasList.Add(new ListItem(swim2Desc, MembershipExtras.SwimSubsOctToMar.ToString())); //COVID-19 change
		extrasList.Add(new ListItem("England Athletics Membership* - &pound;16", MembershipExtras.EnglandAthletics.ToString()));

        extras.Items.AddRange(extrasList.ToArray());

		var indemnityOptionsList = new List<ListItem>()
	    {
		    new ListItem(
                @"I wish to take part in open water swimming.<br />I have read and understand the open water swimming indemnity document.<br />I agree to and accept the terms without qualification and agree to be included in the duty rota for the safety team by attending sessions as a spotter or kayaker.",
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
        var membershipProvider = new MemberProvider();

        var swimSubs1Item = extras.Items.FindByValue(MembershipExtras.SwimSubsAprToSept.ToString());
        var swimSubs2Item = extras.Items.FindByValue(MembershipExtras.SwimSubsOctToMar.ToString());

        return new MembershipOptions()
		{
			MembershipType = (MembershipType) Enum.Parse(typeof(MembershipType), membershipType.SelectedValue),
			SwimSubs1 = swimSubs1Item != null && swimSubs1Item.Selected ? membershipProvider.GetSwimSub1Description(DateTime.Now) : "",
			SwimSubs2 = swimSubs2Item != null && swimSubs2Item.Selected ? membershipProvider.GetSwimSub2Description(DateTime.Now) : "",
            EnglandAthleticsMembership = extras.Items.FindByValue(MembershipExtras.EnglandAthletics.ToString()).Selected,
            OpenWaterIndemnityAcceptance = indemnityOptions.SelectedValue == AcceptIndemnity,
			Volunteering = true //Hardcode to true as can't renew unless this is selected :)
		};
	}
}