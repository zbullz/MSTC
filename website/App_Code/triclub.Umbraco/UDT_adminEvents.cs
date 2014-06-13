using System;
using System.Data;
using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient; 
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.web;
using umbraco.cms.businesslogic.property;

namespace triclub.Umbraco
{
	// ===============================================================================================================================================================
    public class UDT_adminEvents : UserControl, umbraco.editorControls.userControlGrapper.IUsercontrolDataEditor
	{
		#region IUsercontrolDataEditor Members
        public object value { get { return ""; } set { } }
        #endregion
		
		protected DataList		dlEvents;
		
		void Page_Load()
		{
			if(!IsPostBack)
			{
				getEvents();
			}
		}
		
		// Select item to edit
		protected void dlEvents_EditCommand(Object s, DataListCommandEventArgs e)
		{
			//throw new Exception("whoops!");
			
			dlEvents.EditItemIndex = e.Item.ItemIndex;
			getEvents();
		}
		
		// Cancel edit
		protected void Cancel_Command(Object s, DataListCommandEventArgs e)
		{
			dlEvents.EditItemIndex = -1;
			getEvents();
		}
		
		// Edit selected event
		protected void dlEvents_UpdateCommand(Object s, DataListCommandEventArgs e)
		{
			int intEventID = Convert.ToInt32(dlEvents.DataKeys[e.Item.ItemIndex]);
			
			{
				using(MySqlConnection objConn = new MySqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					MySqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
UPDATE Events 
SET eventTitle=?eventTitle, eventDate=?eventDate, eventDescription = ?eventDescription, 
eventLink = ?eventLink, eventLocation = ?eventLocation, resultsLink = ?resultsLink, newsLink = ?newsLink, photoLink = ?photoLink  
WHERE IID = ?eventID
";
					objCmd.Parameters.AddWithValue("?eventTitle", ((TextBox)e.Item.FindControl("etxtEventTitle")).Text);
					objCmd.Parameters.AddWithValue("?eventDate", ((TextBox)e.Item.FindControl("etxtEventDate")).Text);
					objCmd.Parameters.AddWithValue("?eventDescription", ((TextBox)e.Item.FindControl("etxtEventDescription")).Text);
					objCmd.Parameters.AddWithValue("?eventLink", ((TextBox)e.Item.FindControl("etxtEventLink")).Text);
					objCmd.Parameters.AddWithValue("?eventLocation", ((TextBox)e.Item.FindControl("etxtEventLocation")).Text);
					objCmd.Parameters.AddWithValue("?resultsLink", ((TextBox)e.Item.FindControl("etxtResultsLink")).Text);
					objCmd.Parameters.AddWithValue("?newsLink", ((TextBox)e.Item.FindControl("etxtNewsLink")).Text);
					objCmd.Parameters.AddWithValue("?photoLink", ((TextBox)e.Item.FindControl("etxtPhotoLink")).Text);
					objCmd.Parameters.AddWithValue("?eventID", intEventID);

					objCmd.ExecuteNonQuery();
				}
				
				dlEvents.EditItemIndex = -1;
				getEvents();
			}
		}
		
		// Add a new event to the database 
		protected void getEvents()
		{
			{
				using(MySqlConnection objConn = new MySqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					MySqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
SELECT * FROM Events ORDER BY eventTitle ASC
";
					using(MySqlDataReader objRdr = objCmd.ExecuteReader())
					{
						dlEvents.DataSource = objRdr;
						dlEvents.DataBind();
					}
				}
			}
		}
		
		// Edit selected event
		protected void Delete_Command(Object s, DataListCommandEventArgs e)
		{
			int intEventID = Convert.ToInt32(dlEvents.DataKeys[e.Item.ItemIndex]);
			
			{
				using(MySqlConnection objConn = new MySqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					MySqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
DELETE FROM Events WHERE IID = ?eventID
";
					objCmd.Parameters.AddWithValue("?eventID", intEventID);

					objCmd.ExecuteNonQuery();
				}
				
				dlEvents.EditItemIndex = -1;
				getEvents();
			}
		}
	}
}	