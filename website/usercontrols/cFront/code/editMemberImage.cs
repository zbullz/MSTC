using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco.Membership;
using cFront.Umbraco.WebControls;
using umbraco.cms.businesslogic.media;

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
    public class EditMemberImage : UserControl
	{
        public bool IsDebug { get { return Request.QueryString["cfDebug"] == "member"; } }             

        protected PlaceHolder 	DebugContainer;
        protected CheckBox RemoveProfileImage;
        protected MediaUpload profileImage;
        protected MediaImage CurrentProfileImage;

        public String DebugText { get; set; }

		protected void Page_Load()
		{
            //if (!IsPostBack)
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
                // Show current profile image, if any.
                // This helper function is used to set the MediaID, which is an int, from the object returned by the properties
                CurrentProfileImage.SetMediaIDFromObject(currentmemdata["profileImage"]);
            }
        }

        public void SaveMember()
        {
            // Get current member data to get media ID of profile image
            IDictionary<String, object> currentmemdata = MemberHelper.Get();

            if (currentmemdata != null)
            {
                // Create dictionary for update data
                var newMemberData = new Dictionary<String, object>();
				
                // Delete profile image if requested
                if (RemoveProfileImage.Checked)
                {
                    // Attempt to delete the image, if present. Pass true as the second parameter to bypass the REcycle bin and delete permanently
                    MediaUpload.DeleteMediaFromObjectProperty(currentmemdata["profileImage"], false);

                    // Update profile to reflect deletion
                    newMemberData["profileImage"] = null;

                    RemoveProfileImage.Checked = false; // Don't maintain between postbacks
                }
                // Save a profile image if we have one
                else if (profileImage.HasFile)
                {
                    // If a profile image is set then the  media uploader will overwrite it 
                    // This helper function is used to set the MediaID, which is an int, from the object returned by the properties
                    profileImage.SetMediaIDFromObject(currentmemdata["profileImage"]);
                    //

                    // Save with a sensible name (this will also update an existing profile image if the member's name changes)
                    profileImage.SaveAs(currentmemdata["Name"] + " (" + Convert.ToString(currentmemdata["ID"]) + ")");
                    //

                    // Update member with media ID in case it was newly created
                    newMemberData["profileImage"] = profileImage.MediaID;
                }
                //

                MemberHelper.Update(newMemberData); // If you don't provide a member, it uses the current. If no current, this will silently fail.
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