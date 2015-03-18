using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class MemberOptionsDto
{
	public string Name { get; set; }
	public string Email { get; set; }
	public string Phone { get; set; }
	public MembershipType? MembershipType { get; set; }
	public bool SwimSubsJanToJune { get; set; }
	public bool SwimSubsJulyToDec { get; set; }
	public bool OpenWaterIndemnityAcceptance { get; set; }
	public bool Volunteering { get; set; }
	public DateTime? MembershipExpiry { get; set; }
	public int? SwimAuthNumber { get; set; }
}