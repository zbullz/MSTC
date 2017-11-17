﻿using System.Web;
using System.Web.SessionState;
using Mstc.Core.Domain;

namespace Mstc.Core.Providers
{
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
		private const string _canProcessPaymentCompletion = "CanProcessPaymentCompletion";

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

		public bool CanProcessPaymentCompletion
		{
			get { return CurrentSession[_canProcessPaymentCompletion] != null ? (bool) CurrentSession[_canProcessPaymentCompletion] : false; }
			set { CurrentSession[_canProcessPaymentCompletion] = value; }
		}

	    public string SessionId => CurrentSession.SessionID;
    }
}