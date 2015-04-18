using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco;
using Newtonsoft.Json;
using umbraco.cms.businesslogic.property;
using umbraco.providers.members;

public partial class usercontrols_cFront_RegisterMemberComplete : System.Web.UI.UserControl
{
	protected SessionProvider _sessionProvider;
	protected SessionProvider SessionProvider
	{
		get
		{
			if (_sessionProvider == null)
			{
				_sessionProvider = new SessionProvider();
			}
			return _sessionProvider;
		}
	}

    protected void Page_Load(object sender, EventArgs e)
    {
		if (IsPostBack == false)
		{
			RegistrationFullDetails registrationFullDetails = SessionProvider.RegistrationFullDetails;
			if (registrationFullDetails == null)
			{
				return; //Prevent duplicate registration
			}

			//lblQueryString.Text = Request.QueryString["resource_uri"];
		    if (Request.QueryString["resource_uri"] != null)
		    {
			    ConfirmPaymentRequest();
		    }

			//lblMemberOptions.Text = JsonConvert.SerializeObject(registrationFullDetails);

			var member = CreateMember(registrationFullDetails.RegistrationDetails);
			UpdateMemberDetails(member, registrationFullDetails);

			//Login the member
			FormsAuthentication.SetAuthCookie(member.LoginName, true);

			var emailProvider = new EmailProvider();
			string content = string.Format("<p>A new member has registered with the club</p><p>Member details: {0}</p>",
				JsonConvert.SerializeObject(registrationFullDetails, Formatting.Indented));

			emailProvider.SendEmail(ConfigurationManager.AppSettings["newRegistrationEmailTo"], EmailProvider.SupportEmail, "New member registration", content);

			SessionProvider.RegistrationFullDetails = null;
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

		foreach (Property property in (List<Property>)member.GenericProperties)
		{
			if (currentmemdata.ContainsKey(property.PropertyType.Alias))
				property.Value = currentmemdata[property.PropertyType.Alias];
		}
		member.Save();

		//MemberHelper.Update(member, currentmemdata);
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

	private void SetMembershipOptions(IDictionary<String, object> currentmemdata, MembershipOptions membershipOptions)
	{
		currentmemdata[MemberProperty.membershipType] = ((int) membershipOptions.MembershipType).ToString();
		currentmemdata[MemberProperty.OpenWaterIndemnityAcceptance] = membershipOptions.OpenWaterIndemnityAcceptance;
		currentmemdata[MemberProperty.swimSubsJanToJune] = membershipOptions.SwimSubsJanToJune;
		currentmemdata[MemberProperty.SwimSubsJulyToDec] = membershipOptions.SwimSubsJulyToDec;
		currentmemdata[MemberProperty.Volunteering] = membershipOptions.Volunteering;
		currentmemdata[MemberProperty.MembershipExpiry] = new DateTime(DateTime.Now.Year + 1, 4, 1);
		currentmemdata[MemberProperty.SwimCredits] = 0;

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