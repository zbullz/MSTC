using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

/// <summary>
/// Summary description for SessionProvider
/// </summary>
public class SessionProvider
{
	private HttpSessionState CurrentSession
	{
		get { return HttpContext.Current.Session; }
	}

	private const string _renewalOptionsKey = "RenewalOptions";
	private const string _regFullDetails = "RegistrationFullDetails";

	public SessionProvider()
	{
		//
		// TODO: Add constructor logic here
		//
	}

	public MembershipOptions RenewalOptions
	{
		get { return (MembershipOptions) CurrentSession[_renewalOptionsKey]; }
		set { CurrentSession[_renewalOptionsKey] = value; }
	}

	public RegistrationFullDetails RegistrationFullDetails
	{
		get { return (RegistrationFullDetails)CurrentSession[_regFullDetails]; }
		set { CurrentSession[_regFullDetails] = value; }
	}
}