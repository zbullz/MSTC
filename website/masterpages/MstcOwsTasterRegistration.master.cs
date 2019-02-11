using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Mstc.Core.Domain;
using Mstc.Core.Providers;
using Newtonsoft.Json;
using umbraco.BusinessLogic;
using Mstc.Core.Dto;

public partial class masterpages_MstcOwsTasterRegistration : System.Web.UI.MasterPage
{
    private SessionProvider _sessionProvider;
    private GoCardlessProvider _goCardlessProvider;

    public masterpages_MstcOwsTasterRegistration()
    {
        _sessionProvider = new SessionProvider();
        _goCardlessProvider = new GoCardlessProvider();

    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

	protected void Enter_OnClick(object sender, EventArgs e)
	{
		if (Page.IsValid == false)
		{
			return;
		}

		var regDetails = registrationDetailsControl.GetRegistrationDetails();
		var registrationFullDetails = new RegistrationFullDetails()
		{
			MembershipOptions = new MembershipOptions()
			{
				MembershipType = MembershipType.Guest,
				OpenWaterIndemnityAcceptance = true,
				SwimSubs1 = "",
				SwimSubs2 = "",
				Volunteering = false,
                GuestCode = "OWS Taster"
            },
			RegistrationDetails = regDetails
		};

        _sessionProvider.RegistrationFullDetails = registrationFullDetails;

        Log.Add(LogTypes.Custom, -1, string.Format("New OWS Taster registration request: {0}",
            JsonConvert.SerializeObject(registrationFullDetails)));

        var customerDto = new CustomerDto()
        {
            GivenName = registrationFullDetails.RegistrationDetails.FirstName,
            FamilyName = registrationFullDetails.RegistrationDetails.LastName,
            AddressLine1 = registrationFullDetails.RegistrationDetails.Address1,
            City = registrationFullDetails.RegistrationDetails.City,
            PostalCode = registrationFullDetails.RegistrationDetails.Postcode,
            Email = registrationFullDetails.RegistrationDetails.Email
        };
        string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
            Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
        string successUrl = string.Format("{0}/training-zone/ows-taster-registration-complete", rootUrl);

        string paymentDescription = "Open Water Swim Taster Session";
        var redirectResponse = _goCardlessProvider.CreateRedirectRequest(customerDto, paymentDescription, _sessionProvider.SessionId,
            successUrl);

        _sessionProvider.GoCardlessRedirectFlowId = redirectResponse.Id;
        Response.Redirect(redirectResponse.RedirectUrl);


    }

    protected void CheckBoxRequired_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = indemnityAcceptance.Checked;
    }
}
