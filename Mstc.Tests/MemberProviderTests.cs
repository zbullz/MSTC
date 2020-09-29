using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;
using Mstc.Core.Providers;

namespace Mstc.Tests
{
    /*
    //TODO - Rewrite with NUnit, can't run these tests any more
    public class MemberProviderTests
    {
		[Subject(typeof(MemberProvider))]
		public class When_Getting_Member_Expiry_in_April : WithSubject<MemberProvider>
		{

			Because of = () => expiryDate = Subject.GetNewMemberExpiry(new DateTime(DateTime.Now.Year, 4, 1));

			private It has_expiry_date_of_next_april = () =>
				expiryDate.ShouldEqual(new DateTime(DateTime.Now.Year + 1, 4, 1));
			private static DateTime expiryDate;
		}

		[Subject(typeof(MemberProvider))]
	    public class When_Getting_Member_Expiry_in_March : WithSubject<MemberProvider>
	    {

		    Because of = () => expiryDate = Subject.GetNewMemberExpiry(new DateTime(DateTime.Now.Year, 3, 1));

		    private It has_expiry_date_of_next_april = () => 
				expiryDate.ShouldEqual(new DateTime(DateTime.Now.Year + 1, 4, 1));
			private static DateTime expiryDate;
	    }

		[Subject(typeof(MemberProvider))]
		public class When_Getting_Member_Expiry_in_Jan : WithSubject<MemberProvider>
		{

			Because of = () => expiryDate = Subject.GetNewMemberExpiry(new DateTime(DateTime.Now.Year, 1, 1));

			private It has_expiry_date_of_this_april = () =>
				expiryDate.ShouldEqual(new DateTime(DateTime.Now.Year, 4, 1));
			private static DateTime expiryDate;
		}

		[Subject(typeof(MemberProvider))]
		public class When_Getting_Member_Expiry_in_Feb : WithSubject<MemberProvider>
		{

			Because of = () => expiryDate = Subject.GetNewMemberExpiry(new DateTime(DateTime.Now.Year, 2, 1));

			private It has_expiry_date_of_this_april = () =>
				expiryDate.ShouldEqual(new DateTime(DateTime.Now.Year, 4, 1));
			private static DateTime expiryDate;
		}

        [Subject(typeof(MemberProvider))]
        public class When_Getting_SwimSub1_in_Jan : WithSubject<MemberProvider>
        {

            Because of = () => swimSub1Desc = Subject.GetSwimSub1Description(new DateTime(2018, 1, 1));

            private It has_last_years_dates = () =>  swimSub1Desc.ShouldEqual("Apr to Sep 2017");
            private static string swimSub1Desc;
        }

        [Subject(typeof(MemberProvider))]
        public class When_Getting_SwimSub1_in_Feb : WithSubject<MemberProvider>
        {

            Because of = () => swimSub1Desc = Subject.GetSwimSub1Description(new DateTime(2018, 2, 1));

            private It has_last_years_dates = () => swimSub1Desc.ShouldEqual("Apr to Sep 2017");
            private static string swimSub1Desc;
        }

        [Subject(typeof(MemberProvider))]        
        public class When_Getting_SwimSub1_in_Dec : WithSubject<MemberProvider>
        {

            Because of = () => swimSub1Desc = Subject.GetSwimSub1Description(new DateTime(2018, 12, 1));

            private It has_this_years_dates = () =>  swimSub1Desc.ShouldEqual("Apr to Sep 2018");
            private static string swimSub1Desc;
        }

        [Subject(typeof(MemberProvider))]
        public class When_Getting_SwimSub2_in_Jan : WithSubject<MemberProvider>
        {
            Because of = () => swimSubsDesc = Subject.GetSwimSub2Description(new DateTime(2018, 1, 1));
            private It has_last_years_dates = () => swimSubsDesc.ShouldEqual("Oct 2017 to Mar 2018");
            private static string swimSubsDesc;
        }

        [Subject(typeof(MemberProvider))]
        public class When_Getting_SwimSub2_in_Feb : WithSubject<MemberProvider>
        {
            Because of = () => swimSubsDesc = Subject.GetSwimSub2Description(new DateTime(2018, 2, 1));
            private It has_last_years_dates = () => swimSubsDesc.ShouldEqual("Oct 2017 to Mar 2018");
            private static string swimSubsDesc;
        }

        [Subject(typeof(MemberProvider))]
        public class When_Getting_SwimSub2_in_Mar : WithSubject<MemberProvider>
        {
            Because of = () => swimSubsDesc = Subject.GetSwimSub2Description(new DateTime(2018, 3, 1));
            private It has_this_years_dates = () => swimSubsDesc.ShouldEqual("Oct 2018 to Mar 2019");
            private static string swimSubsDesc;
        }

        [Subject(typeof(MemberProvider))]
        public class When_Getting_SwimSub2_in_Dec : WithSubject<MemberProvider>
        {
            Because of = () => swimSubsDesc = Subject.GetSwimSub2Description(new DateTime(2018, 12, 1));
            private It has_this_years_dates = () => swimSubsDesc.ShouldEqual("Oct 2018 to Mar 2019");
            private static string swimSubsDesc;
        }

    }
    */
}
