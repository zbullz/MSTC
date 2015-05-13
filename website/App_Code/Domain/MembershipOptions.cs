using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[Serializable]
public class MembershipOptions
{
	public MembershipType MembershipType { get; set; }
	public bool SwimSubsJanToJune { get; set; }
	public bool SwimSubsJulyToDec { get; set; }
	public bool OpenWaterIndemnityAcceptance { get; set; }
	public bool Volunteering { get; set; }
	public string GuestCode { get; set; }
}