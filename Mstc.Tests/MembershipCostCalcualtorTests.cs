using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using Mstc.Core.Domain;
using Mstc.Core.Providers;

namespace Mstc.Tests
{
	public class MembershipCostCalcualtorTests
	{
		public class when_getting_Individual_type_cost_in_september : WithSubject<MembershipCostCalculator>
		{
			private Because of = () =>  cost = Subject.GetTypeCostPence(MembershipType.Individual, new DateTime(DateTime.Now.Year, 9, 1));

			private It is_4000_Pence = () => cost.ShouldEqual(4000);

			private static decimal cost;
		}

		public class when_getting_Individual_type_cost_in_october : WithSubject<MembershipCostCalculator>
		{
			private Because of = () => cost = Subject.GetTypeCostPence(MembershipType.Individual, new DateTime(DateTime.Now.Year, 10, 1));

			private It is_2000_Pence = () => cost.ShouldEqual(2000);

			private static decimal cost;
		}

		public class when_getting_discounted_type_cost_months : WithSubject<MembershipCostCalculator>
		{
			private Because of = () => discountedMonths = MembershipCostCalculator.DiscountedMonths;

			private It includes_October_November_December_Januaray_Feburary = () =>
			{
				discountedMonths.ShouldContain(10);
				discountedMonths.ShouldContain(11);
				discountedMonths.ShouldContain(12);
				discountedMonths.ShouldContain(1);
				discountedMonths.ShouldContain(2);
			};

			static List<int> discountedMonths;
		}

		public class when_calculating_full_member_cost_with_both_swim_subs_in_september : WithSubject<MembershipCostCalculator>
		{
			private Because of = () => cost = Subject.Calculate(new MembershipOptions()
			{
				MembershipType = MembershipType.Individual,
				SwimSubsAprToSept = true,
				SwimSubsOctToMar = true
			},
				new DateTime(DateTime.Now.Year, 9, 1));

			private It is_10000_Pence = () => cost.ShouldEqual(10000);

			private static decimal cost;
		}


	}
}
