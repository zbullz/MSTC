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

    }
}
