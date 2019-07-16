using System;
using Mstc.Core.Domain;

namespace Mstc.Core.Dto
{
	public class MemberOptionsDto
	{
		public int NodeId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public MembershipType? MembershipType { get; set; }
		public string SwimSubs1 { get; set; }
		public string SwimSubs2 { get; set; }
        public bool EnglandAthleticsMembership { get; set; }
        public bool OpenWaterIndemnityAcceptance { get; set; }
		public bool Volunteering { get; set; }
		public DateTime? MembershipExpiry { get; set; }
		public int? SwimAuthNumber { get; set; }
		public int SwimBalanceLastYear { get; set; }
		public int SwimCreditsBought { get; set; }
		public int SwimCreditsUsed { get; set; }

		public string DuathlonEntry { get; set; }
		public string TriFestEntry { get; set; }
		public string CharitySwimEntry { get; set; }
        public string RelayTeamName { get; set; }
        public string BtfNumber { get; set; }

        public bool IsGuest => MembershipType.HasValue && MembershipType.Value == Domain.MembershipType.Guest;

	}
}