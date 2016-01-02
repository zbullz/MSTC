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
		public bool SwimSubsJanToMar { get; set; }
		public bool SwimSubsAprToSept { get; set; }
		public bool SwimSubsOctToMar { get; set; }
		public bool OpenWaterIndemnityAcceptance { get; set; }
		public bool Volunteering { get; set; }
		public DateTime? MembershipExpiry { get; set; }
		public int? SwimAuthNumber { get; set; }
		public int SwimCreditsBought { get; set; }
		public int SwimCreditsUsed { get; set; }

		public bool DuathlonEntered { get; set; }
		public string TriFestEntry { get; set; }
		public string CharitySwimEntry { get; set; }
	}
}