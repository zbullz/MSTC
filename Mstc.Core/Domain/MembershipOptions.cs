using System;

namespace Mstc.Core.Domain
{
	[Serializable]
	public class MembershipOptions
	{
		public MembershipType MembershipType { get; set; }
		public bool SwimSubsAprToSept { get; set; }
		public bool SwimSubsOctToMar { get; set; }
		public bool OpenWaterIndemnityAcceptance { get; set; }
		public bool Volunteering { get; set; }
		public string GuestCode { get; set; }
		public string ReferredByMember { get; set; }
	}
}