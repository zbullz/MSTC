using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
		    new ListItem("Individual membership - &pound;40", MembershipType.Individual.ToString()),
		    new ListItem(
			    @"Couple membership - &pound;35<br /> <i>Only select this option if your partner will also be renewing their membership - The membership secretary will be checking!</i>",
			    MembershipType.Couple.ToString()),
		    new ListItem("Unemployed/full-time student (18 years or above) - &pound;30",
			    MembershipType.Concession.ToString())
	    };
		membershipType.Items.AddRange(membershipTypes.ToArray());

		var extrasList = new List<ListItem>()
	    {
		    new ListItem("Swim subs January to June - &pound;30", MembershipExtras.SwimSubsJanToJune.ToString()),
		    new ListItem("Swim subs July to December - &pound;30", MembershipExtras.SwimSubsJulyToDec.ToString())
	    };
		extras.Items.AddRange(extrasList.ToArray());

		var indemnityOptionsList = new List<ListItem>()
	    {
		    new ListItem(
			    @"I have read and understand the open water swimming indemnity document.<br />I agree to and accept them without qualification.",
			    AcceptIndemnity),
		    new ListItem(
			    @"I do not accept the open water swimming indemnity document.<br />I understand I will not be elligible to take part in club open water swim sessions for this membership year.",
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
		return new MembershipOptions()
		{
			MembershipType = (MembershipType) Enum.Parse(typeof(MembershipType), membershipType.SelectedValue),
			SwimSubsJanToJune = extras.Items.FindByValue(MembershipExtras.SwimSubsJanToJune.ToString()).Selected,
			SwimSubsJulyToDec = extras.Items.FindByValue(MembershipExtras.SwimSubsJulyToDec.ToString()).Selected,
			OpenWaterIndemnityAcceptance = indemnityOptions.SelectedValue == AcceptIndemnity,
			Volunteering = true //Hardcode to true as can't renew unless this is selected :)
		};
	}
}