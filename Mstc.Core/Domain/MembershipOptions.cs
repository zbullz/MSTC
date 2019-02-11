using System;

namespace Mstc.Core.Domain
{
	[Serializable]
	public class MembershipOptions
	{
		public MembershipType MembershipType { get; set; }
		public string SwimSubs1 { get; set; }
		public string SwimSubs2 { get; set; }
        public bool EnglandAthleticsMembership { get; set; }
		public bool OpenWaterIndemnityAcceptance { get; set; }
		public bool Volunteering { get; set; }
		public string GuestCode { get; set; }
		public string ReferredByMember { get; set; }
	}
}