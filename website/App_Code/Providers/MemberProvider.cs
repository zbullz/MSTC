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

	public void UpdateMemberDetails(umbraco.cms.businesslogic.member.Member member, RegistrationFullDetails regDetails, DateTime? membershipExpiry)
	{
		IDictionary<String, object> currentmemdata = MemberHelper.Get(member);

		SetMemberDetails(currentmemdata, regDetails.RegistrationDetails);
        SetMembershipOptions(currentmemdata, regDetails.MembershipOptions, membershipExpiry ?? new DateTime(DateTime.Now.Year + 1, 4, 1));

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
		currentmemdata[MemberProperty.GuestCode] = membershipOptions.GuestCode;

		if (membershipOptions.OpenWaterIndemnityAcceptance)
		{
		    GuestCodes? guestCode = string.IsNullOrWhiteSpace(membershipOptions.GuestCode)
		        ? null
		        : (GuestCodes?) Enum.Parse(typeof (GuestCodes), membershipOptions.GuestCode);
            int swimAuthNumber = GetSwimAuthNumber(guestCode);
			currentmemdata[MemberProperty.SwimAuthNumber] = swimAuthNumber;
		}
	}

    private int GetSwimAuthNumber(GuestCodes? guestCode)
    {
        //Calculate the next available swim auth number
        IMemberDal memberDal = new MemberDal(new DataConnection());
        IEnumerable<MemberOptionsDto> memberOptions = memberDal.GetMemberOptions();
        IEnumerable<MemberOptionsDto> openWaterSwimmers = memberOptions.Where(m => m.SwimAuthNumber.HasValue);

        int swimAuthNumber = 0;
        if (guestCode == null)
        {
            var swimMembers = openWaterSwimmers.Where(m => m.SwimAuthNumber < 1000).OrderBy(m => m.SwimAuthNumber);
            swimAuthNumber = swimMembers.Any() ? swimMembers.Last().SwimAuthNumber.Value + 1 : 1;
            return swimAuthNumber;
        }

        switch (guestCode)
        {
            case GuestCodes.EGAffiliate:
            {
                var EGAffiliateSwimmers = openWaterSwimmers.Where(m => m.SwimAuthNumber > 1000 && m.SwimAuthNumber < 2000).OrderBy(m => m.SwimAuthNumber);
                swimAuthNumber = EGAffiliateSwimmers.Any() ? EGAffiliateSwimmers.Last().SwimAuthNumber.Value + 1 : 1001;
                break;
            }
            case GuestCodes.BTRSLegend:
            {
                var BTRSSwimmers = openWaterSwimmers.Where(m => m.SwimAuthNumber > 2000 && m.SwimAuthNumber < 3000).OrderBy(m => m.SwimAuthNumber);
                swimAuthNumber = BTRSSwimmers.Any() ? BTRSSwimmers.Last().SwimAuthNumber.Value + 1 : 2001;
                break;
            }
        }
        return swimAuthNumber;
    }
}