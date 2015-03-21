using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco;
using Newtonsoft.Json;
using umbraco.providers.members;

public partial class usercontrols_cFront_RegisterMemberComplete : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if (IsPostBack == false)
		{
			//lblQueryString.Text = Request.QueryString["resource_uri"];
		    if (Request.QueryString["resource_uri"] != null)
		    {
			    ConfirmPaymentRequest();
		    }

			var sessionProvider = new SessionProvider();
			RegistrationFullDetails registrationFullDetails = sessionProvider.RegistrationFullDetails;

			lblMemberOptions.Text = JsonConvert.SerializeObject(registrationFullDetails);

			var member = CreateMember(registrationFullDetails.RegistrationDetails);
			UpdateMemberDetails(member, registrationFullDetails);

			//Login the member
			FormsAuthentication.SetAuthCookie(member.LoginName, true);
	    }
    }

	private void ConfirmPaymentRequest()
	{
		var goCardlessProvider = new GoCardlessProvider();
		goCardlessProvider.ConfirmBill(Request.QueryString);
	}

	private umbraco.cms.businesslogic.member.Member CreateMember(RegistrationDetails regDetails)
	{
		return MemberHelper.Create(regDetails.Email, regDetails.Password, regDetails.FullName, regDetails.Email, "Member",
			new string[] {"Member"});
	}


	private void UpdateMemberDetails(umbraco.cms.businesslogic.member.Member member, RegistrationFullDetails regDetails)
	{
		IDictionary<String, object> currentmemdata = MemberHelper.Get(member);
		SetMemberDetails(currentmemdata, regDetails.RegistrationDetails);
		SetMembershipOptions(currentmemdata, regDetails.MembershipOptions);
		MemberHelper.Update(currentmemdata);
	}

	private void SetMemberDetails(IDictionary<String, object> currentmemdata, RegistrationDetails registrationDetails)
	{
		currentmemdata[MemberProperty.Gender] = registrationDetails.Gender;
		currentmemdata[MemberProperty.DateOfBirth] = registrationDetails.DateOfBirth.ToString("yyyy-MM-dd");
		currentmemdata[MemberProperty.Address1] = registrationDetails.Address1;
		currentmemdata[MemberProperty.Address2] = registrationDetails.City;
		currentmemdata[MemberProperty.Postcode] = registrationDetails.Postcode;
		currentmemdata[MemberProperty.Phone] = registrationDetails.PhoneNumber;
		currentmemdata[MemberProperty.BTFNumber] = registrationDetails.BTFNumber;

		currentmemdata[MemberProperty.medicalConditions] = registrationDetails.MedicalConditions;
		currentmemdata[MemberProperty.emergencyContactName] = registrationDetails.EmergencyContactName;
		currentmemdata[MemberProperty.emergencyContactNumber] = registrationDetails.EmergencyContactPhone;
	}

	private void SetMembershipOptions(IDictionary<String, object> currentmemdata, MembershipOptions membershipOptions)
	{
		currentmemdata[MemberProperty.membershipType] = membershipOptions.MembershipType.ToString();
		currentmemdata[MemberProperty.OpenWaterIndemnityAcceptance] = membershipOptions.OpenWaterIndemnityAcceptance;
		currentmemdata[MemberProperty.swimSubsJanToJune] = membershipOptions.SwimSubsJanToJune;
		currentmemdata[MemberProperty.SwimSubsJulyToDec] = membershipOptions.SwimSubsJulyToDec;
		currentmemdata[MemberProperty.Volunteering] = membershipOptions.Volunteering;
		currentmemdata[MemberProperty.MembershipExpiry] = new DateTime(DateTime.Now.Year + 1, 4, 1).ToString("yyyy-MM-dd");

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