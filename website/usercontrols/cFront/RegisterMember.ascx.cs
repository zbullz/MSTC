using System;
using System.Collections.Generic;
using Mstc.Core.Domain;
using Mstc.Core.Dto;
using Mstc.Core.Providers;
using Newtonsoft.Json;
using umbraco.BusinessLogic;

public partial class usercontrols_cFront_RegisterMember : System.Web.UI.UserControl
{
    private SessionProvider _sessionProvider;
    private GoCardlessProvider _goCardlessProvider;

    public usercontrols_cFront_RegisterMember()
    {
        _sessionProvider = new SessionProvider();
        _goCardlessProvider = new GoCardlessProvider();

    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

	protected void RegisterMember_OnClick(object sender, EventArgs e)
	{
		if (Page.IsValid == false)
		{
			return;
		}

		var registrationFullDetails = new RegistrationFullDetails()
		{
			MembershipOptions = membershipOptionsControl.GetMembershipOptions(),
			RegistrationDetails = registrationDetailsControl.GetRegistrationDetails()
		};

        _sessionProvider.RegistrationFullDetails = registrationFullDetails;

		Log.Add(LogTypes.Custom, -1, string.Format("New member registration request: {0}",
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
        string successUrl = string.Format("{0}/the-club/membership-registration-complete", rootUrl);

	    var redirectResponse = _goCardlessProvider.CreateRedirectRequest(customerDto, "MSTC Member Registration", _sessionProvider.SessionId,
	        successUrl);
        
        _sessionProvider.GoCardlessRedirectFlowId = redirectResponse.Id;
        Response.Redirect(redirectResponse.RedirectUrl);
	}

	public int FromYear { get { return DateTime.Now.Month < 3 ? DateTime.Now.Year - 1 : DateTime.Now.Year; } }
	public int ToYear { get { return DateTime.Now.Month < 3 ? DateTime.Now.Year : DateTime.Now.Year + 1; } }
}