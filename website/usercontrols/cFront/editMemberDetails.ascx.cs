using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco;
using Mstc.Core.Domain;

public partial class usercontrols_cFront_editMemberDetails : UserControl
{
	public bool IsDebug { get { return Request.QueryString["cfDebug"] == "member"; } }
	public String DebugText { get; set; }

	protected void Page_Load()
	{
		if (!IsPostBack)
			LoadMember();

		DebugContainer.Visible = IsDebug;
	}

	public void LoadMember()
	{
		// Calling MemberHelper.Get() with no arguments gets the current member. Call it with an ID if you want to get details for a specific member,
		// although if you want to do that, for a directory or something, it would be better to use XSLT
		IDictionary<String, object> currentmemdata = MemberHelper.Get();

		if (currentmemdata != null) // Null if no current member
		{
			// Same process for getting system data as custom data - just use the preset aliases mentioned above for system data
			Email.Text = Convert.ToString(currentmemdata[MemberProperty.Email]);
			phoneMobile.Text = Convert.ToString(currentmemdata[MemberProperty.Phone]);
			Name.Text = Convert.ToString(currentmemdata[MemberProperty.Name]);
			address1.Text = Convert.ToString(currentmemdata[MemberProperty.Address1]);
			address2.Text = Convert.ToString(currentmemdata[MemberProperty.Address2]);
			address3.Text = Convert.ToString(currentmemdata[MemberProperty.Address3]);
			postcode.Text = Convert.ToString(currentmemdata[MemberProperty.Postcode]);
			if (currentmemdata[MemberProperty.DateOfBirth] != null)
			{
				dateOfBirth.Text = String.Format("{0:dd/MM/yyyy}", currentmemdata[MemberProperty.DateOfBirth]);
			}

			txtMedConditions.Text = Convert.ToString(currentmemdata[MemberProperty.medicalConditions]);
			txtEmergencyName.Text = Convert.ToString(currentmemdata[MemberProperty.emergencyContactName]);
			txtEmergencyNumber.Text = Convert.ToString(currentmemdata[MemberProperty.emergencyContactNumber]);

			// Member service
			if (Convert.ToString(currentmemdata[MemberProperty.showService]) == "1")
				cbShowService.Checked = true;
			else
				cbShowService.Checked = false;

			tbServiceLinkAddress.Text = Convert.ToString(currentmemdata[MemberProperty.serviceLinkAddress]);
			tbServiceLinkText.Text = Convert.ToString(currentmemdata[MemberProperty.serviceLinkText]);
			currentServiceImage.SetMediaIDFromObject(currentmemdata[MemberProperty.serviceImage]);
			tbServiceDescription.Text = Convert.ToString(currentmemdata[MemberProperty.serviceDescription]);
		}
	}

	public void SaveMember()
	{
		// Get current member data to get media ID of profile image
		IDictionary<String, object> currentmemdata = MemberHelper.Get();

		if (currentmemdata != null)
		{
			// Create dictionary for update data
			Dictionary<String, object> newmemdata = new Dictionary<String, object>();

			// Same process for setting system data as custom data - just use the preset aliases mentioned above for system data
			newmemdata[MemberProperty.Email] = Email.Text;
			newmemdata[MemberProperty.LoginName] = Email.Text;
			newmemdata[MemberProperty.Phone] = phoneMobile.Text;
			newmemdata[MemberProperty.Name] = Name.Text;
			newmemdata[MemberProperty.Address1] = address1.Text;
			newmemdata[MemberProperty.Address2] = address2.Text;
			newmemdata[MemberProperty.Address3] = address3.Text;
			newmemdata[MemberProperty.Postcode] = postcode.Text;

			if (string.IsNullOrWhiteSpace(dateOfBirth.Text) == false)
				newmemdata[MemberProperty.DateOfBirth] = DateTime.ParseExact(dateOfBirth.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
			else
			{
				newmemdata[MemberProperty.DateOfBirth] = null;
			}

			newmemdata[MemberProperty.medicalConditions] = txtMedConditions.Text;
			newmemdata[MemberProperty.emergencyContactName] = txtEmergencyName.Text;
			newmemdata[MemberProperty.emergencyContactNumber] = txtEmergencyNumber.Text;

			// Service
			newmemdata[MemberProperty.showService] = cbShowService.Checked;
			newmemdata[MemberProperty.serviceLinkAddress] = tbServiceLinkAddress.Text;
			newmemdata[MemberProperty.serviceLinkText] = tbServiceLinkText.Text;
			if (serviceImage.HasFile)
			{
				// If a profile image is set then the  media uploader will overwrite it 
				// This helper function is used to set the MediaID, which is an int, from the object returned by the properties
				serviceImage.SetMediaIDFromObject(currentmemdata[MemberProperty.serviceImage]);
				//

				// Save with a sensible name (this will also update an existing profile image if the member's name changes)
				serviceImage.SaveAs(currentmemdata[MemberProperty.Name] + "-" + Convert.ToString(currentmemdata["ID"]));

				// Update member with media ID in case it was newly created
				newmemdata[MemberProperty.serviceImage] = serviceImage.MediaID;
			}

			newmemdata[MemberProperty.serviceDescription] = tbServiceDescription.Text;

			MemberHelper.Update(newmemdata); // If you don't provide a member, it uses the current. If no current, this will silently fail.
		}
	}

	protected void SaveMemberClicked(object sender, EventArgs e)
	{
		if (Page.IsValid)
		{
			SaveMember();
			LoadMember(); // Refresh to get current info

		}
	}
}