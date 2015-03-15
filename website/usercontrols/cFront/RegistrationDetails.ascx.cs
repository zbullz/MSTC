using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class usercontrols_cFront_RegistrationDetails : System.Web.UI.UserControl
{
	protected void Page_Load(object sender, EventArgs e)
    {
	    if (Page.IsPostBack == false)
	    {
		    BindControls();
	    }
    }

	private void BindControls()
	{
		var gender = new List<ListItem>()
		{
			new ListItem("Male", "Male"),
			new ListItem("Female", "Female")
		};
		rblGender.Items.AddRange(gender.ToArray());

		var extrasList = new List<ListItem>()
	    {
		    new ListItem("Swim subs January to June - &pound;30", MembershipExtras.SwimSubsJanToJune.ToString()),
		    new ListItem("Swim subs July to December - &pound;30", MembershipExtras.SwimSubsJulyToDec.ToString()),
		    new ListItem("Spin/Core subs April to Sept - &pound;20", MembershipExtras.CoreSubsAprilToSept.ToString()),
		    new ListItem("Turbo/Core subs Oct to March - &pound;20", MembershipExtras.CoreSubsOctToMarch.ToString())
	    };
	}

	public MembershipOptions GetRegistrationDetails()
	{
		return null;
	}
}