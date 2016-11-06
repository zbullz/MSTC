using System;
using Mstc.Core.Domain;

namespace Mstc.Core.Dto
{
	public class MemberIceDto
	{
		public int NodeId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string MedicalConditions { get; set; }
		public string EmergencyContactName { get; set; }
		public string EmergencyContactNumber { get; set; }
	}
}