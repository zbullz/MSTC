using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco.Membership;
using GoCardlessSdk;
using GoCardlessSdk.Connect;

public partial class masterpages_MstcDuathlonEntryComplete : System.Web.UI.MasterPage
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
			EnterMemberInDuathlon();
		}
    }

	private void ConfirmPaymentRequest()
	{
		var goCardlessProvider = new GoCardlessProvider();
		goCardlessProvider.ConfirmBill(Request.QueryString);
	}

	private void EnterMemberInDuathlon()
	{
		IDictionary<String, object> currentmemdata = MemberHelper.Get();
		if (currentmemdata == null)
		{
			return; //Ensure the form is behind a login form
		}

		currentmemdata[MemberProperty.DuathlonEntered] = true;
		MemberHelper.Update(currentmemdata);
	}
}
