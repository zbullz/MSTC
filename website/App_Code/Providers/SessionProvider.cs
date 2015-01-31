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
}