using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class MembershipOptions
{
	public MembershipType MembershipType { get; set; }
	public bool SwimSubsJanToJune { get; set; }
	public bool SwimSubsJulyToDec { get; set; }
	public bool CoreSubsAprilToSept { get; set; }
	public bool CoreSubsOctToMarch { get; set; }
	public bool OpenWaterIndemnityAcceptance { get; set; }
	public bool Volunteering { get; set; }
}