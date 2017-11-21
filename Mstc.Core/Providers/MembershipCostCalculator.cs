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
		private Dictionary<MembershipType, int> TypeCosts = new Dictionary<MembershipType, int>()
		{
			{MembershipType.Individual, 4000},
			{MembershipType.Couple,3500},
			{MembershipType.Concession, 3000},
		};

		public static List<int> DiscountedMonths
		{
			get { return new List<int>() {10, 11, 12, 1, 2}; }
		}

		private int SwimsSubsCost = 3000;

		public int GetTypeCostPence(MembershipType type, DateTime currentDate)
		{
			return DiscountedMonths.Contains(currentDate.Month) ? TypeCosts[type]/2 : TypeCosts[type];
		}

		public int Calculate(MembershipOptions membershipOptions, DateTime currentDate)
		{
			var cost = GetTypeCostPence(membershipOptions.MembershipType, currentDate);		
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

		public int SwimCreditsCost(PaymentStates credits, MembershipType membershipType)
		{
			return (int) credits * 100;
		}
	}
}