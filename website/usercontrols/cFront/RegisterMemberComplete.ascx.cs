using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco;
using Mstc.Core.Domain;
using Mstc.Core.Providers;
using Newtonsoft.Json;
using umbraco.cms.businesslogic.property;
using umbraco.providers.members;

public partial class usercontrols_cFront_RegisterMemberComplete : System.Web.UI.UserControl
{
	protected SessionProvider _sessionProvider;
	protected SessionProvider SessionProvider
	{
		get
		{
			if (_sessionProvider == null)
			{
				_sessionProvider = new SessionProvider();
			}
			return _sessionProvider;
		}
	}

    protected void Page_Load(object sender, EventArgs e)
    {
		if (IsPostBack == false)
		{
			RegistrationFullDetails registrationFullDetails = SessionProvider.RegistrationFullDetails;
			if (registrationFullDetails == null)
			{
				return; //Prevent duplicate registration
			}

		    if (Request.QueryString["resource_uri"] != null)
		    {
			    ConfirmPaymentRequest();
		    }

			//lblMemberOptions.Text = JsonConvert.SerializeObject(registrationFullDetails);
			
			var memberProvider = new MemberProvider();
			var member = memberProvider.CreateMember(registrationFullDetails.RegistrationDetails, new string[] {"Member"});
			memberProvider.UpdateMemberDetails(member, registrationFullDetails);

			//Login the member
			FormsAuthentication.SetAuthCookie(member.LoginName, true);

			var emailProvider = new EmailProvider();
			string content = string.Format("<p>A new member has registered with the club</p><p>Member details: {0}</p>",
				JsonConvert.SerializeObject(registrationFullDetails, Formatting.Indented));

			emailProvider.SendEmail(ConfigurationManager.AppSettings["newRegistrationEmailTo"], EmailProvider.SupportEmail, "New MSTC member registration", content);

			SessionProvider.RegistrationFullDetails = null;
		}
    }

	private void ConfirmPaymentRequest()
	{
		var goCardlessProvider = new GoCardlessProvider();
		goCardlessProvider.ConfirmBill(Request.QueryString);
	}


}