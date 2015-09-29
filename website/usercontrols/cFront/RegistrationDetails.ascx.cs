using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mstc.Core.Domain;
using umbraco.providers.members;

public partial class usercontrols_cFront_RegistrationDetails : System.Web.UI.UserControl
{
	protected void Page_Load(object sender, EventArgs e)
    {
	    if (Page.IsPostBack == false)
	    {
		    BindControls();
	    }
    }

	private void BindControls()
	{
		var gender = new List<ListItem>()
		{
			new ListItem("Male", "Male"),
			new ListItem("Female", "Female")
		};
		rblGender.Items.AddRange(gender.ToArray());

	}

	public void availableEmailValidator_OnValidate(object source, ServerValidateEventArgs args)
	{
		UmbracoMembershipProvider provider = new UmbracoMembershipProvider();
		var userName = provider.GetUserNameByEmail(args.Value);
		args.IsValid = string.IsNullOrWhiteSpace(userName);
	}

	public RegistrationDetails GetRegistrationDetails()
	{
		return new RegistrationDetails()
		{
			FullName = tbFullName.Text,
			Email = tbEmail.Text,
			Password = tbPassword.Text,
			Gender = rblGender.SelectedValue,
			DateOfBirth = DateTime.ParseExact(tbdateOfBirth.Text, "dd/MM/yyyy", null),
			Address1 = tbStreet.Text,
			City = tbCity.Text,
			Postcode = tbPostcode.Text,
			PhoneNumber = tbPhoneNumber.Text,
			BTFNumber = tbBTFNumber.Text,
			MedicalConditions = tbMedicalConditions.Text,
			EmergencyContactName = tbEmergencyContactName.Text,
			EmergencyContactPhone = tbEmergencyContactNumber.Text
		};
	}
}