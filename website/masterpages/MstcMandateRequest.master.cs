using System;
using System.Collections.Generic;
using System.Linq;
using cFront.Umbraco.Membership;
using Mstc.Core.Domain;
using Mstc.Core.Dto;
using Mstc.Core.Providers;

public partial class masterpages_MstcMandateRequest : System.Web.UI.MasterPage
{
    private SessionProvider _sessionProvider;
    private GoCardlessProvider _goCardlessProvider;


    public masterpages_MstcMandateRequest()
    {
        _sessionProvider = new SessionProvider();
        _goCardlessProvider = new GoCardlessProvider();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
		if (IsPostBack == false)
		{
			IDictionary<String, object> currentmemdata = MemberHelper.Get();
			if (currentmemdata == null || _sessionProvider.CanProcessPaymentCompletion == false || Request.QueryString["state"] == null)
			{
				return; //Ensure the form is behind a login form and not a duplicate request
			}

            MandateRequest(currentmemdata);
		}
    }

    protected void MandateRequest(IDictionary<String, object> currentmemdata)
    {
        var fullName = currentmemdata[MemberProperty.Name] as string ?? string.Empty;
        string familyName = fullName;
        string givenName = string.Empty;
        if (fullName.Contains(" "))
        {
            var names = fullName.Split(' ');
            familyName = names.Last();
            givenName = string.Join(" ", names.Take(names.Length - 1));
        }

        var customerDto = new CustomerDto()
        {
            GivenName = givenName,
            FamilyName = familyName,
            AddressLine1 = currentmemdata[MemberProperty.Address1] as string,
            City = currentmemdata[MemberProperty.Address2] as string,
            PostalCode = currentmemdata[MemberProperty.Postcode] as string,
            Email = currentmemdata[MemberProperty.Email] as string
        };

        string page = _sessionProvider.MandateSuccessPage;
        string state = Request.QueryString["state"];

        string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
            Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
        string redirectUrl = string.Format("{0}/{1}?state={2}", rootUrl, page, state);

        var redirectResponse = _goCardlessProvider.CreateRedirectRequest(customerDto, "Mid Sussex Tri Club DD Mandate Setup", _sessionProvider.SessionId,
            redirectUrl);

        _sessionProvider.GoCardlessRedirectFlowId = redirectResponse.Id;
        Response.Redirect(redirectResponse.RedirectUrl);
    }
 


}
