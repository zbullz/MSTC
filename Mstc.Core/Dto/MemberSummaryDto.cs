using Mstc.Core.Domain;

namespace Mstc.Core.Dto
{
	public class MemberSummaryDto
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int? ProfileImageId { get; set; }
        public MembershipType MembershipType { get; set; }
    }
}