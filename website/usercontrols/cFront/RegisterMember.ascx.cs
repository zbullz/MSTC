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

    public usercontrols_cFront_RegisterMember()
    {
        _sessionProvider = new SessionProvider();
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

	protected async void RegisterMember_OnClick(object sender, EventArgs e)
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

		decimal cost = (new MembershipCostCalculator()).Calculate(registrationFullDetails.MembershipOptions, DateTime.Now);
		var memberProvider = new MemberProvider();

        var goCardlessProvider = new GoCardlessProvider();

        var customerDto = new CustomerDto()
        {
            //TODO - Need to amend form to split first name and surname
            GivenName = registrationFullDetails.RegistrationDetails.FullName,
            //FamilyName = registrationFullDetails.
            AddressLine1 = registrationFullDetails.RegistrationDetails.Address1,
            City = registrationFullDetails.RegistrationDetails.City,
            PostalCode = registrationFullDetails.RegistrationDetails.Postcode,
            Email = registrationFullDetails.RegistrationDetails.Email
        };
	    string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
	        Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
        string successUrl = string.Format("{0}/the-club/membership-registration-complete", rootUrl);

	    var redirectResponse = await goCardlessProvider.CreateRedirectRequest(customerDto, _sessionProvider.SessionId,
	        successUrl);

        //TODO -Save the redirectResponse.Id to the user session

        Response.Redirect(redirectResponse.RedirectUrl);

        //RedirectToGocardless(registrationFullDetails.RegistrationDetails.Email, cost, memberProvider.GetPaymentDescription(registrationFullDetails.MembershipOptions));
		//RedirectToCompletePage(); //Can use this for local testing
	}

	private void RedirectToCompletePage()
	{
		string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
			Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
		string redirectUrl = string.Format("{0}/the-club/membership-registration-complete", rootUrl);
		Response.Redirect(redirectUrl);
	}

	public int FromYear { get { return DateTime.Now.Month < 3 ? DateTime.Now.Year - 1 : DateTime.Now.Year; } }
	public int ToYear { get { return DateTime.Now.Month < 3 ? DateTime.Now.Year : DateTime.Now.Year + 1; } }
}