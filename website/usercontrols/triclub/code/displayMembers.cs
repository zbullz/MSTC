using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data; 
using MySql.Data.MySqlClient; 
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;

using cFront.Umbraco.Membership; // Requires ~/bin/cfMemberExtensions.dll
using cFront.Umbraco.WebControls; // Requires ~/bin/cfUmbracoWebControls.dll

using umbraco.cms.businesslogic.media;

namespace cFront.Projects.AbsoluteVacuum.Web.UI.UserControls
{
	// ===============================================================================================================================================================
    public class displayMembers : UserControl
	{
		protected Repeater 		rpMembers;
		protected PlaceHolder	phMemberList, phMemberDetail;
		protected int			intMemberID;
		protected string		strName, strEmail, strDOB, strPhone;
		protected MediaImage 	CurrentProfileImage;
		
		void Page_Load()
		{
			intMemberID = Convert.ToInt32(Request.QueryString["memberID"]);
			
			if(intMemberID == 0)
				getMembers();
			else
				getMemberDetail();
		}
		
		// Get all future events
		protected void getMembers()
		{
			rpMembers.DataSource = MemberHelper.GetAllMembers();
			rpMembers.DataBind();
			
			phMemberList.Visible = true;
			phMemberDetail.Visible = false;
		}
		

		// Get details for chosen event
		protected void getMemberDetail()
		{
			IDictionary<String, object> currentmemdata = MemberHelper.Get(intMemberID);
			 
			strName = Convert.ToString(currentmemdata["Name"]);
			strEmail = Convert.ToString(currentmemdata["Email"]);
			strDOB = String.Format("{0:dd/MM/yyyy}", currentmemdata["dateOfBirth"]);
			strPhone = Convert.ToString(currentmemdata["phoneMobile"]);
			
			CurrentProfileImage.SetMediaIDFromObject(currentmemdata["profileImage"]);
			
			phMemberList.Visible = false;
			phMemberDetail.Visible = true;
		}
		
		protected int SafeID(object data)
		{
			int res;
			if(data != null && Int32.TryParse(Convert.ToString(data), out res))
				return res;
			else 
				return 0;
		}
	}
}	