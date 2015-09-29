using System;

namespace Mstc.Core.Domain
{
	[Serializable]
	public class RegistrationFullDetails
	{
		public RegistrationDetails RegistrationDetails { get; set; }
		public MembershipOptions MembershipOptions { get; set; }
	}
}