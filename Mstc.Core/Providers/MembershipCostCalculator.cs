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
		private static Dictionary<MembershipType, int> TypeCosts = new Dictionary<MembershipType, int>()
		{
			{MembershipType.Individual, 4000},
			{MembershipType.Couple,3500},
			{MembershipType.Concession, 3000},
		};

		public static List<int> DiscountedMonths
		{
			get { return new List<int>() {10, 11, 12, 1, 2}; }
		}

		public static int SwimsSubsCostInPence = 3000;
	    public static int EnglandAthleticsCostInPence = 1500;

		public static int GetTypeCostPence(MembershipType type, DateTime currentDate)
		{
			return DiscountedMonths.Contains(currentDate.Month) ? TypeCosts[type]/2 : TypeCosts[type];
		}

		public static int Calculate(MembershipOptions membershipOptions, DateTime currentDate)
		{
			var cost = GetTypeCostPence(membershipOptions.MembershipType, currentDate);		
			if (membershipOptions.SwimSubsAprToSept)
			{
				cost += SwimsSubsCostInPence;
			}
			if (membershipOptions.SwimSubsOctToMar)
			{
				cost += SwimsSubsCostInPence;
			}
		    if (membershipOptions.EnglandAthleticsMembership)
		    {
		        cost += EnglandAthleticsCostInPence;
		    }
	
			return cost;
		}

		public static int SwimCreditsCost(PaymentStates credits)
		{
			return (int) credits * 100;
		}

        public static int PaymentStateCost(PaymentStates state, bool hasBTFNumber)
        {
            switch (state)
            {
                case PaymentStates.S00599C:
                case PaymentStates.S001099C:
                case PaymentStates.S001599C:
                case PaymentStates.S002499C:
                {
                    return SwimCreditsCost(state);
                    }
                case PaymentStates.E00D101C:
                {
                    return 1000;
                }
                case PaymentStates.E00TRIOI201C:
                case PaymentStates.E00TRIMI203C:
                
                case PaymentStates.E00TRISI205C:
                {
                    return hasBTFNumber ? 2000 : 2300;
                }
                case PaymentStates.E00TRIOR202C:
                case PaymentStates.E00TRIMR204C:
                {
                    return hasBTFNumber ? 1000 : 1300;
                }
                case PaymentStates.E00S1KM301C:
                case PaymentStates.E00S3KM302C:
                case PaymentStates.E00S5KM303C:
                case PaymentStates.E00S1KM3KM304C:
                case PaymentStates.E00S1KM5KM305C:
                case PaymentStates.E00S3KM5KM306C:
                case PaymentStates.E00S1KM3KM5KM307C:
                    {
                        return 2000;
                    }
                case PaymentStates.SS05991:
                case PaymentStates.SS05992:
                case PaymentStates.SS05996:
                    {
                        return SwimsSubsCostInPence;
                    }
            }

            throw new Exception("Unknown cost");
        }

    }
}