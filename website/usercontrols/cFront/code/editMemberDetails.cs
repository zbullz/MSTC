using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco.Membership;
using cFront.Umbraco.WebControls;
using umbraco.cms.businesslogic.media;
// Requires ~/bin/cfMemberExtensions.dll

namespace usercontrols.cFront.code
{
	// ===============================================================================================================================================================
    /*
     * 
     * MemberHelper class is in cFront.Umbraco.Membership
     * 
     * The member helpers make loadng and saving data avery simple one step process for all member data, both system and custom.
     * 
     * In order for that to work you CANNOT have custom generic properties on the MemberType that have the following reserved aliases: 
     * 
     * ID, LoginName, Name, Email
     * 
     */
    public class EditMemberDetails : UserControl
	{
		

        public bool IsDebug { get { return Request.QueryString["cfDebug"] == "member"; } }

	    protected PlaceHolder DebugContainer;
	    protected TextBox phoneMobile,
		    Email,
		    Name,
		    address1,
		    address2,
		    address3,
		    postcode,
		    dateOfBirth,
		    txtMedConditions,
		    txtEmergencyName,
		    txtEmergencyNumber,
			tbServiceLinkText,
			tbServiceLinkAddress,
			tbServiceDescription;
		protected MediaUpload serviceImage;
		protected MediaImage currentServiceImage;
	    protected CheckBox cbShowService;

        public String DebugText { get; set; }

		protected void Page_Load()
		{
            if (!IsPostBack)
                LoadMember();

            DebugContainer.Visible = IsDebug;

            if (IsDebug) 
                ShowDebugText();
        }

        public void LoadMember()
        {
            // Calling MemberHelper.Get() with no arguments gets the current member. Call it with an ID if you want to get details for a specific member,
            // although if you want to do that, for a directory or something, it would be better to use XSLT
            IDictionary<String, object> currentmemdata = MemberHelper.Get();

            if (currentmemdata != null) // Null if no current member
            {
                // Same process for getting system data as custom data - just use the preset aliases mentioned above for system data
                Email.Text = Convert.ToString(currentmemdata["Email"]); 
                phoneMobile.Text = Convert.ToString(currentmemdata["phoneMobile"]);
                Name.Text = Convert.ToString(currentmemdata["Name"]);
                address1.Text = Convert.ToString(currentmemdata["address1"]);
                address2.Text = Convert.ToString(currentmemdata["address2"]);
                address3.Text = Convert.ToString(currentmemdata["address3"]);
                postcode.Text = Convert.ToString(currentmemdata["postcode"]);
                if(currentmemdata["dateOfBirth"] != null)
				{
					dateOfBirth.Text = String.Format("{0:dd/MM/yyyy}", currentmemdata["dateOfBirth"]);
				}

				txtMedConditions.Text = Convert.ToString(currentmemdata["medicalConditions"]);
				txtEmergencyName.Text = Convert.ToString(currentmemdata["emergencyContactName"]);
				txtEmergencyNumber.Text = Convert.ToString(currentmemdata["emergencyContactNumber"]);
					
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
                newmemdata["Email"] = Email.Text;
				newmemdata["LoginName"] = Email.Text;
                newmemdata["phoneMobile"] = phoneMobile.Text;
                newmemdata["Name"] = Name.Text;
                newmemdata["address1"] = address1.Text;
                newmemdata["address2"] = address2.Text;
                newmemdata["address3"] = address3.Text;
                newmemdata["postcode"] = postcode.Text;

	            if (string.IsNullOrWhiteSpace(dateOfBirth.Text) == false)
		            newmemdata["dateOfBirth"] = DateTime.ParseExact(dateOfBirth.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
	            else
	            {
		            newmemdata["dateOfBirth"] = null;
	            }
				
				newmemdata["medicalConditions"] = txtMedConditions.Text;
				newmemdata["emergencyContactName"] = txtEmergencyName.Text;
				newmemdata["emergencyContactNumber"] = txtEmergencyNumber.Text;
				
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
					serviceImage.SaveAs(currentmemdata["Name"] + "-" + Convert.ToString(currentmemdata["ID"]));

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

                // Reload debug info after details saved
                if (IsDebug)
                    ShowDebugText();
            }
        }        

        // -------------------------------------------------------------------------------------------------------
        // -- EVERYTHING AFTER HERE IS DEBUG ONLY AND UNRELATED TO MEMBER EDITING --------------------------------
        // -------------------------------------------------------------------------------------------------------
        private void ShowDebugText()
        {
            DebugText = "Page.Request.ApplicationPath: " + Page.Request.ApplicationPath + "<hr/>";
            DebugText += MemberHelper.Debug(true, true).Replace("\r\n", "<br/>");
            DebugText += "<hr/>";
            DebugText += Debugger.DebugMedia("Has file", 1193);
            DebugText += Debugger.DebugMedia("New", 1357);
            DebugText += Debugger.DebugMedia("Pat's profileImage", 1367);
        }
	}

    public class Debugger
    {
        public static String DebugMedia(String title, int id)
        {
            Media m = null;

            try
            {
                m = new Media(id);
            }
            catch (ArgumentException) { }

            List<String> l = new List<String>();
            l.Add(title.ToUpper());
            l.Add("Id: " + id.ToString());

            if (m != null)
            {
                l.Add("Hashcode: " + m.GetHashCode().ToString());
                l.Add("Text: " + m.Text);

                foreach (umbraco.cms.businesslogic.property.Property prop in m.GenericProperties)
                    l.Add(prop.PropertyType.Alias + ": " + prop.Value + " (PropId: " + prop.Id.ToString() + ")");
            }
            else
                l.Add("Member is null");

            return String.Join("<br/>", l.ToArray()) + "<hr/>";
        }

        public static int GetRootFolderIDByName(String name)
        {
            Media[] rootmeds = Media.GetRootMedias();

            foreach (Media m in rootmeds)
                if (m.Text == name)
                    return m.Id;

            return -1;
        }
    }
}	