using System;
using System.Collections.Generic;
using Mstc.Core.Domain;

namespace Mstc.Core.Providers
{
	/// <summary>
	/// Summary description for MembershipCostCalcualtor
	/// </summary>
	public class MembershipCostCalculator
	{
		private Dictionary<MembershipType, decimal> TypeCosts = new Dictionary<MembershipType, decimal>()
		{
			{MembershipType.Individual, new decimal(40.00)},
			{MembershipType.Couple, new decimal(35.00)},
			{MembershipType.Concession, new decimal(30.00)},
		};

		public static List<int> DiscountedMonths
		{
			get { return new List<int>() {10, 11, 12, 1, 2}; }
		}

		private decimal SwimsSubsCost = new decimal(30);

		public decimal GetTypeCost(MembershipType type, DateTime currentDate)
		{
			return DiscountedMonths.Contains(currentDate.Month) ? TypeCosts[type]/2 : TypeCosts[type];
		}

		public decimal Calculate(MembershipOptions membershipOptions, DateTime currentDate)
		{
			var cost = GetTypeCost(membershipOptions.MembershipType, currentDate);		
			if (membershipOptions.SwimSubsAprToSept)
			{
				cost += SwimsSubsCost;
			}
			if (membershipOptions.SwimSubsOctToMar)
			{
				cost += SwimsSubsCost;
			}
	
			return cost;
		}

		public decimal SwimCreditsCost(PaymentStates credits, MembershipType membershipType)
		{
			return membershipType == MembershipType.Guest ? new decimal(5*(int) credits) : new decimal(3*(int) credits);
		}
	}
}