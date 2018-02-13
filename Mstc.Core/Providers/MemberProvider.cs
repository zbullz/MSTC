using System;
using System.Collections.Generic;
using System.Linq;
using cFront.Umbraco;
using Mstc.Core.DataAccess;
using Mstc.Core.Domain;
using Mstc.Core.Dto;
using umbraco.cms.businesslogic.property;

namespace Mstc.Core.Providers
{
	/// <summary>
	/// Summary description for MemberProvider
	/// </summary>
	public class MemberProvider
	{
		public string GetPaymentDescription(MembershipOptions membershipOptions)
		{
			List<string> descriptionList = new List<string>() { membershipOptions.MembershipType.ToString() };
			if (membershipOptions.SwimSubsAprToSept)
			{
				descriptionList.Add("Swim subs Apr to Sept");
			}
			if (membershipOptions.SwimSubsOctToMar)
			{
				descriptionList.Add("Swim subs Oct to Mar");
			}
		    if (membershipOptions.EnglandAthleticsMembership)
		    {
				descriptionList.Add("England Athletics Membership");
            }

            return string.Join(", ", descriptionList);
		}

		public DateTime GetNewMemberExpiry(DateTime currentDate)
		{
			return currentDate.Month == 1 || currentDate.Month == 2
				? new DateTime(DateTime.Now.Year, 4, 1)
				: new DateTime(DateTime.Now.Year + 1, 4, 1);
		}

		public umbraco.cms.businesslogic.member.Member CreateMember(RegistrationDetails regDetails, string[] roles)
		{
			return MemberHelper.Create(regDetails.Email, regDetails.Password, $"{regDetails.FirstName} {regDetails.LastName}", regDetails.Email, "Member",
				roles);
		}

		public void UpdateMemberDetails(umbraco.cms.businesslogic.member.Member member, RegistrationFullDetails regDetails)
		{
			IDictionary<String, object> currentmemdata = MemberHelper.Get(member);

			SetMemberDetails(currentmemdata, regDetails.RegistrationDetails);
			var membershipExpiry = GetNewMemberExpiry(DateTime.Now);
			SetMembershipOptions(currentmemdata, regDetails.MembershipOptions, membershipExpiry, zeroSwimCredits: true);

			foreach (Property property in (List<Property>)member.GenericProperties)
			{
				if (currentmemdata.ContainsKey(property.PropertyType.Alias))
					property.Value = currentmemdata[property.PropertyType.Alias];
			}
			member.Save();
		}

		public void UpdateMemberOptions(umbraco.cms.businesslogic.member.Member member, MembershipOptions membershipOptions)
		{
			IDictionary<String, object> currentmemdata = MemberHelper.Get(member);

			var membershipExpiry = GetNewMemberExpiry(DateTime.Now);
			SetMembershipOptions(currentmemdata, membershipOptions, membershipExpiry, zeroSwimCredits: false);

			foreach (Property property in (List<Property>)member.GenericProperties)
			{
				if (currentmemdata.ContainsKey(property.PropertyType.Alias))
					property.Value = currentmemdata[property.PropertyType.Alias];
			}
			member.Save();
		}

		private void SetMemberDetails(IDictionary<String, object> currentmemdata, RegistrationDetails registrationDetails)
		{
			currentmemdata[MemberProperty.Gender] = registrationDetails.Gender;
			currentmemdata[MemberProperty.DateOfBirth] = registrationDetails.DateOfBirth;
			currentmemdata[MemberProperty.Address1] = registrationDetails.Address1;
			currentmemdata[MemberProperty.Address2] = registrationDetails.City;
			currentmemdata[MemberProperty.Postcode] = registrationDetails.Postcode;
			currentmemdata[MemberProperty.Phone] = registrationDetails.PhoneNumber;
			currentmemdata[MemberProperty.BTFNumber] = registrationDetails.BTFNumber;

			currentmemdata[MemberProperty.medicalConditions] = registrationDetails.MedicalConditions;
			currentmemdata[MemberProperty.emergencyContactName] = registrationDetails.EmergencyContactName;
			currentmemdata[MemberProperty.emergencyContactNumber] = registrationDetails.EmergencyContactPhone;
			currentmemdata[MemberProperty.directDebitMandateId] = registrationDetails.DirectDebitMandateId;

        }

		private void SetMembershipOptions(IDictionary<String, object> currentmemdata, MembershipOptions membershipOptions, DateTime membershipExpiry, bool zeroSwimCredits)
		{
			currentmemdata[MemberProperty.membershipType] = ((int)membershipOptions.MembershipType).ToString();
			currentmemdata[MemberProperty.OpenWaterIndemnityAcceptance] = membershipOptions.OpenWaterIndemnityAcceptance;
			currentmemdata[MemberProperty.swimSubsAprToSept] = membershipOptions.SwimSubsAprToSept;
			currentmemdata[MemberProperty.SwimSubsOctToMar] = membershipOptions.SwimSubsOctToMar;
		    currentmemdata[MemberProperty.EnglandAthleticsMembership] = membershipOptions.EnglandAthleticsMembership;
			currentmemdata[MemberProperty.Volunteering] = membershipOptions.Volunteering;
			currentmemdata[MemberProperty.MembershipExpiry] = membershipExpiry;
			if (zeroSwimCredits)
			{
				currentmemdata[MemberProperty.SwimCreditsBought] = 0;
			}
			currentmemdata[MemberProperty.GuestCode] = membershipOptions.GuestCode;
			currentmemdata[MemberProperty.ReferredByMember] = membershipOptions.ReferredByMember;

			if (membershipOptions.OpenWaterIndemnityAcceptance)
			{
				int swimAuthNumber = GetSwimAuthNumber(membershipOptions.MembershipType);
				currentmemdata[MemberProperty.SwimAuthNumber] = swimAuthNumber;
			}
		}

		private int GetSwimAuthNumber(MembershipType membershipType)
		{
			//Calculate the next available swim auth number
			IMemberDal memberDal = new MemberDal(new DataConnection());
			IEnumerable<MemberOptionsDto> memberOptions = memberDal.GetMemberOptions();
			IEnumerable<MemberOptionsDto> openWaterSwimmers = memberOptions.Where(m => m.SwimAuthNumber.HasValue);

			int swimAuthNumber = 0;
			if (membershipType != MembershipType.Guest)
			{
				var swimMembers = openWaterSwimmers.Where(m => m.SwimAuthNumber < 1000).OrderBy(m => m.SwimAuthNumber);
				swimAuthNumber = swimMembers.Any() ? swimMembers.Last().SwimAuthNumber.Value + 1 : 1;
			}
			else
			{
				var guestSwimmers = openWaterSwimmers.Where(m => m.SwimAuthNumber > 1000 && m.SwimAuthNumber < 2000).OrderBy(m => m.SwimAuthNumber);
				swimAuthNumber = guestSwimmers.Any() ? guestSwimmers.Last().SwimAuthNumber.Value + 1 : 1001;
			}

			return swimAuthNumber;
		}
	}
}