using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace triclub.Umbraco
{
	// ===============================================================================================================================================================
    public class UDT_adminAddEvents : UserControl, umbraco.editorControls.userControlGrapper.IUsercontrolDataEditor
	{
		#region IUsercontrolDataEditor Members
        public object value { get { return ""; } set { } }
        #endregion
		
		/// <summary>
        /// DocumentType alias for nodes that are folders of event items
        /// </summary>
        public String FolderAlias { get { return "triclubEntriesList"; } }
        /// <summary>
        /// DocumentType alias for nodes that are event items
        /// </summary>
        //  public String ItemAlias { get { return "GreenRegisterItem"; } }
        /// <summary>
        /// Pattern to match property aliases that are of PDCalendar (http://our.umbraco.org/projects/website-utilities/pdcalendar) type
        /// </summary>
        // 	public String PDCalendarTypeExpression { get { return "txtEventDate"; } }
		
		protected TextBox 		txtEventTitle, txtEventDate, txtEventDescription, txtEventLink, txtEventLocation, txtResultsLink, txtNewsLink, txtPhotoLink;
		protected DropDownList 	ddlEventType, ddlEventDistance;
		protected DataList		dlEvents;
			
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
VALUES (@eventTitle, @eventDate, @eventType, @eventDistance, @eventDescription, @eventLink, @eventLocation, @resultsLink, @newsLink, @photoLink)
";
					objCmd.Parameters.AddWithValue("@eventTitle", txtEventTitle.Text);
					objCmd.Parameters.AddWithValue("@eventDate", txtEventDate.Text);
					objCmd.Parameters.AddWithValue("@eventType", ddlEventType.SelectedItem.Value);
					objCmd.Parameters.AddWithValue("@eventDistance", ddlEventDistance.SelectedItem.Value);
					objCmd.Parameters.AddWithValue("@eventDescription", txtEventDescription.Text);
					objCmd.Parameters.AddWithValue("@eventLink", txtEventLink.Text);
					objCmd.Parameters.AddWithValue("@eventLocation", txtEventLocation.Text);
					objCmd.Parameters.AddWithValue("@resultsLink", txtResultsLink.Text);
					objCmd.Parameters.AddWithValue("@newsLink", txtNewsLink.Text);
					objCmd.Parameters.AddWithValue("@photoLink", txtPhotoLink.Text);

					objCmd.ExecuteNonQuery();
				}
				
				Page.Response.Redirect(Page.Request.Url.ToString(), true);
			}
		}
	}
}	