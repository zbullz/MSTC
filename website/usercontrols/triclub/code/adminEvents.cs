using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace cFront.Projects.CFSL.Web.UI.UserControls
{
	// ===============================================================================================================================================================
    public class AdminEvents : UserControl
	{
		protected TextBox 		txtEventTitle, txtEventDate, txtEventDescription, txtEventLink, txtEventLocation, txtResultsLink, txtNewsLink, txtPhotoLink;
		protected DropDownList 	ddlEventType, ddlEventDistance;
		
		void Page_Load()
		{
			
		}
		
		protected void getPageSettings()
		{
			if(!IsPostBack)
			{
				
			}
		}
		
		// Add a new event to the database 
		protected void addEvent(Object s, EventArgs e)
		{
			{
				using(SqlConnection objConn = new SqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					SqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
INSERT INTO Events (eventTitle, eventDate, eventType, eventDistance, eventDescription, eventLink, eventLocation, resultsLink, newsLink, photoLink) 
VALUES (?eventTitle, ?eventDate, ?eventType, ?eventDistance, ?eventDescription, ?eventLink, ?eventLocation, ?resultsLink, ?newsLink, ?photoLink)
";
					objCmd.Parameters.AddWithValue("?eventTitle", txtEventTitle.Text);
					objCmd.Parameters.AddWithValue("?eventDate", txtEventDate.Text);
					objCmd.Parameters.AddWithValue("?eventType", ddlEventType.SelectedItem.Value);
					objCmd.Parameters.AddWithValue("?eventDistance", ddlEventDistance.SelectedItem.Value);
					objCmd.Parameters.AddWithValue("?eventDescription", txtEventDescription.Text);
					objCmd.Parameters.AddWithValue("?eventLink", txtEventLink.Text);
					objCmd.Parameters.AddWithValue("?eventLocation", txtEventLocation.Text);
					objCmd.Parameters.AddWithValue("?resultsLink", txtResultsLink.Text);
					objCmd.Parameters.AddWithValue("?newsLink", txtNewsLink.Text);
					objCmd.Parameters.AddWithValue("?photoLink", txtPhotoLink.Text);

					objCmd.ExecuteNonQuery();
				}
				Page_Load();
			}
		}
	}
}	