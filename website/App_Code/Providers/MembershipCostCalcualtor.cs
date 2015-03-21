using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MembershipCostCalcualtor
/// </summary>
public class MembershipCostCalcualtor
{
	private Dictionary<MembershipType, decimal> TypeCosts = new Dictionary<MembershipType, decimal>()
	{
		{MembershipType.Individual, new decimal(40.00)},
		{MembershipType.Couple, new decimal(35.00)},
		{MembershipType.Concession, new decimal(30.00)},
	};

	private decimal SwimsSubsCost = new decimal(30);

	public decimal Calculate(MembershipOptions membershipOptions)
	{
		var cost = TypeCosts[membershipOptions.MembershipType];		
		if (membershipOptions.SwimSubsJanToJune)
		{
			cost += SwimsSubsCost;
		}
		if (membershipOptions.SwimSubsJulyToDec)
		{
			cost += SwimsSubsCost;
		}
	
		return cost;
	}

}