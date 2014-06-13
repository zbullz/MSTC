using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.UI;
using System.Web.UI.WebControls;

using cFront.Umbraco.Membership; // Requires ~/bin/cfMemberExtensions.dll
using cFront.Umbraco.WebControls; // Requires ~/bin/cfUmbracoWebControls.dll

using umbraco.cms.businesslogic.media;

namespace cFront.Projects.AbsoluteVacuum.Web.UI.UserControls
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
    public class EditMemberExtra : UserControl
	{
        public bool IsDebug { get { return Request.QueryString["cfDebug"] == "member"; } }             

        protected PlaceHolder 	DebugContainerExtra, pHRenewMembership, pHRenewSwimSubs;
		protected Label			lblMembershipExpiry, lblSwimSubsExpiry;
		
        public String DebugText { get; set; }

		protected void Page_Load()
		{
            if (!IsPostBack)
			{
                pHRenewMembership.Visible = false;
				pHRenewSwimSubs.Visible = false;
				
				LoadMember();
			}

            DebugContainerExtra.Visible = IsDebug;

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
				// Get membership expiry date
				object memexp = currentmemdata["membershipExpiry"];
				
				// Check to see if there is membership expiry date
				if((memexp != null) && (memexp is DateTime))
				{
					lblMembershipExpiry.Text = String.Format("{0:dd MMM yyyy}", memexp);
					
					pHRenewMembership.Visible = (DateTime)memexp < DateTime.Today;
				}
				else
				{
					pHRenewMembership.Visible = true;
				}
				
				// Get swim subs expiry date
				object swimexp = currentmemdata["swimSubsExpiry"];
				
				// Check to see if there is a swim subs expiry date
				if((swimexp != null) && (swimexp is DateTime))
				{
					lblSwimSubsExpiry.Text = String.Format("{0:dd MMM yyyy}", swimexp);
					
					pHRenewSwimSubs.Visible = (DateTime)swimexp < DateTime.Today;
				}
				else
				{
					pHRenewSwimSubs.Visible = true;
				}
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