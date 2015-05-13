using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cFront.Umbraco;
using umbraco.cms.businesslogic.property;

/// <summary>
/// Summary description for MemberProvider
/// </summary>
public class MemberProvider
{
	public MemberProvider()
	{
		//
		// TODO: Add constructor logic here
		//
	}

	public umbraco.cms.businesslogic.member.Member CreateMember(RegistrationDetails regDetails, string[] roles)
	{
		return MemberHelper.Create(regDetails.Email, regDetails.Password, regDetails.FullName, regDetails.Email, "Member",
			roles);
	}

	public void UpdateMemberDetails(umbraco.cms.businesslogic.member.Member member, RegistrationFullDetails regDetails, DateTime membershipExpiry)
	{
		IDictionary<String, object> currentmemdata = MemberHelper.Get(member);

		SetMemberDetails(currentmemdata, regDetails.RegistrationDetails);
		SetMembershipOptions(currentmemdata, regDetails.MembershipOptions, membershipExpiry);

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
	}

	private void SetMembershipOptions(IDictionary<String, object> currentmemdata, MembershipOptions membershipOptions, DateTime membershipExpiry)
	{
		currentmemdata[MemberProperty.membershipType] = ((int)membershipOptions.MembershipType).ToString();
		currentmemdata[MemberProperty.OpenWaterIndemnityAcceptance] = membershipOptions.OpenWaterIndemnityAcceptance;
		currentmemdata[MemberProperty.swimSubsJanToJune] = membershipOptions.SwimSubsJanToJune;
		currentmemdata[MemberProperty.SwimSubsJulyToDec] = membershipOptions.SwimSubsJulyToDec;
		currentmemdata[MemberProperty.Volunteering] = membershipOptions.Volunteering;
		currentmemdata[MemberProperty.MembershipExpiry] = membershipExpiry;
		currentmemdata[MemberProperty.SwimCreditsBought] = 0;

		if (membershipOptions.OpenWaterIndemnityAcceptance)
		{
			//Calculate the next available swim auth number
			IMemberDal memberDal = new MemberDal(new DataConnection());
			IEnumerable<MemberOptionsDto> memberOptions = memberDal.GetMemberOptions();
			var membersWithSwimAuthNumbers = memberOptions.Where(m => m.SwimAuthNumber.HasValue).OrderBy(m => m.SwimAuthNumber);
			int swimAuthNumber = membersWithSwimAuthNumbers.Any()
				? membersWithSwimAuthNumbers.Last().SwimAuthNumber.Value + 1
				: 1;
			currentmemdata[MemberProperty.SwimAuthNumber] = swimAuthNumber;
		}
	}
}