using System.Collections.Generic;
using Mstc.Core.Domain;

namespace Mstc.Core.Providers
{
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

		public decimal SwimCreditsCost(PaymentStates credits, MembershipType membershipType)
		{
			return membershipType == MembershipType.Guest ? new decimal(5*(int) credits) : new decimal(3*(int) credits);
		}
	}
}