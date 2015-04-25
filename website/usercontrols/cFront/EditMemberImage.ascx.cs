using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco.Membership;
using cFront.Umbraco.WebControls;
using umbraco.cms.businesslogic.media;

public partial class usercontrols_cFront_EditMemberImage : System.Web.UI.UserControl
{
	public void LoadImage(IDictionary<String, object> currentmemdata)
	{
		if (currentmemdata != null) // Null if no current member
		{
			// Show current profile image, if any.
			// This helper function is used to set the MediaID, which is an int, from the object returned by the properties
			CurrentProfileImage.SetMediaIDFromObject(currentmemdata[MemberProperty.ProfileImage]);
		}
	}

	public IDictionary<String, object> SaveImage()
	{
		// Get current member data to get media ID of profile image
		IDictionary<String, object> currentmemdata = MemberHelper.Get();

		if (currentmemdata != null)
		{
			// Create dictionary for update data
			var newMemberData = new Dictionary<String, object>();

			if (RemoveProfileImage.Checked) // Delete profile image if requested
			{
				// Attempt to delete the image, if present. Pass true as the second parameter to bypass the REcycle bin and delete permanently
				MediaUpload.DeleteMediaFromObjectProperty(currentmemdata["profileImage"], false);

				// Update profile to reflect deletion
				newMemberData["profileImage"] = null;

				RemoveProfileImage.Checked = false; // Don't maintain between postbacks
			}
			else if (profileImage.HasFile) // Save a profile image if we have one
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
			else
			{
				return currentmemdata;
			}

			MemberHelper.Update(newMemberData); // If you don't provide a member, it uses the current. If no current, this will silently fail.
			return newMemberData;
		}
		return null;
	}

	protected void SaveMemberClicked(object sender, EventArgs e)
	{
		if (Page.IsValid)
		{
			var newMemberData = SaveImage();
			if (newMemberData != null)
			{
				LoadImage(newMemberData); // Refresh to get current info
			}
		}
	}  


	protected void Page_Load(object sender, EventArgs e)
    {

    }
}