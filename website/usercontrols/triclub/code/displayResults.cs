using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace cFront.Projects.CFSL.Web.UI.UserControls
{
	// ===============================================================================================================================================================
    public class displayResults : UserControl
	{
		protected Repeater 		rpEvents;
		protected PlaceHolder	phEventsList, phEventDetail;
		protected int			intEventID, intEventTypeID, intEventDistanceID;
		protected string		strEventTitle, strEventDate, strEventType, strEventDistance, strEventLocation, strEventWebsite, strResults, strSearchEvents, strUser;
		protected Label			lblNoEvent;
		protected LoginView		MemberResultsAddView;
		protected TextBox 		txtEventTitle, txtEventDate, txtEventDescription, txtEventLink, txtEventLocation, txtResultsLink;
		protected DropDownList 	ddlEventType, ddlEventDistance;
		
		void Page_Load()
		{
			intEventID = Convert.ToInt32(Request.QueryString["eventID"]);
			intEventTypeID = Convert.ToInt32(Request.QueryString["eventType"]);
			intEventDistanceID = Convert.ToInt32(Request.QueryString["eventDistance"]);
			strSearchEvents = Request.Form["txtEventSearch"];
			
			if(intEventID == 0)
				getEvents();
			else
				getEventDetail();
		}
		
		// Get all future events
		protected void getEvents()
		{
			if(intEventTypeID == 0 && intEventDistanceID == 0 && strSearchEvents == null)
			{
				phEventsList.Visible = true; // show event list
				phEventDetail.Visible = false; // hide event detail
				
				{
					using(SqlConnection objConn = new SqlConnection(ConfigurationManager.AppSettings["triclubDSN"]))
					{
						objConn.Open();
						SqlCommand objCmd = objConn.CreateCommand();
						objCmd.CommandText = 
	@"
	SELECT E.IID, E.eventTitle, E.eventDate, T.eventTypeDefinition AS eventType, D.eventDistanceDefinition AS eventDistance, E.eventLocation FROM Events E
	INNER JOIN eventTypeDefinitions T ON E.eventType = T.eventTypeID
	INNER JOIN eventDistanceDefinitions D ON E.eventDistance = D.eventDistanceID
	WHERE E.resultsLink != ''
	ORDER BY eventDate
	";
						using(SqlDataReader objRdr = objCmd.ExecuteReader())
						{
							rpEvents.DataSource = objRdr;
							rpEvents.DataBind();
						}
					}
				}
			}
			if(strSearchEvents != null)
				getEventsSearched();
			else if(intEventTypeID != 0 || intEventDistanceID != 0)
				getEventsFiltered();
		}
		
		// Get all future events filtered by appropriate filter
		protected void getEventsFiltered()
		{
			phEventsList.Visible = true; // show event list
			phEventDetail.Visible = false; // hide event detail
			
			{
				using(SqlConnection objConn = new SqlConnection(ConfigurationManager.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					SqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
SELECT E.IID, E.eventTitle, E.eventDate, T.eventTypeDefinition AS eventType, D.eventDistanceDefinition AS eventDistance, E.eventLocation FROM Events E
INNER JOIN eventTypeDefinitions T ON E.eventType = T.eventTypeID
INNER JOIN eventDistanceDefinitions D ON E.eventDistance = D.eventDistanceID
WHERE E.resultsLink != ''
AND
(E.eventType = @eventTypeID OR E.eventDistance = @eventDistanceID)
ORDER BY eventDate
";
					objCmd.Parameters.AddWithValue("@eventTypeID", intEventTypeID);
					objCmd.Parameters.AddWithValue("@eventDistanceID", intEventDistanceID);
					
					using(SqlDataReader objRdr = objCmd.ExecuteReader())
					{
						rpEvents.DataSource = objRdr;
						rpEvents.DataBind();
					}
				}
			}
		}
		
		// Get all future events filtered by appropriate search
		protected void getEventsSearched()
		{
			phEventsList.Visible = true; // show event list
			phEventDetail.Visible = false; // hide event detail
			
			{
				using(SqlConnection objConn = new SqlConnection(ConfigurationManager.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					SqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
SELECT E.IID, E.eventTitle, E.eventDate, T.eventTypeDefinition AS eventType, D.eventDistanceDefinition AS eventDistance, E.eventLocation FROM Events E
INNER JOIN eventTypeDefinitions T ON E.eventType = T.eventTypeID
INNER JOIN eventDistanceDefinitions D ON E.eventDistance = D.eventDistanceID
WHERE E.resultsLink != ''
AND
(E.eventTitle LIKE CONCAT('%',@eventSearch,'%'))
ORDER BY eventDate
";
					objCmd.Parameters.AddWithValue("@eventSearch", strSearchEvents);
					
					using(SqlDataReader objRdr = objCmd.ExecuteReader())
					{
						rpEvents.DataSource = objRdr;
						rpEvents.DataBind();
					}
				}
			}
		}
		
		// Get details for chosen event
		protected void getEventDetail()
		{
			phEventsList.Visible = false; // hide events list placeholder
			phEventDetail.Visible = true; // show event detail placeholder
			
			{
				using(SqlConnection objConn = new SqlConnection(ConfigurationManager.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					SqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
SELECT E.IID, E.eventTitle, E.eventDate, E.resultsLink, T.eventTypeDefinition AS eventType, D.eventDistanceDefinition AS eventDistance, E.eventLocation, E.eventLink FROM Events E
INNER JOIN eventTypeDefinitions T ON E.eventType = T.eventTypeID
INNER JOIN eventDistanceDefinitions D ON E.eventDistance = D.eventDistanceID
WHERE E.IID = @eventID
";
					objCmd.Parameters.AddWithValue("@eventID", intEventID);
					
					using(SqlDataReader objRdr = objCmd.ExecuteReader())
					{
						if(objRdr.Read())
						{
							strEventTitle = objRdr["eventTitle"] is DBNull ? "Not entered" : Convert.ToString(objRdr["eventTitle"]);
							strEventDate = objRdr["eventDate"] is DBNull ? "Not entered" : Convert.ToString(String.Format("{0 : dd MMM yyyy}", objRdr["eventDate"]));
							strEventType = objRdr["eventType"] is DBNull ? "Not entered" : Convert.ToString(objRdr["eventType"]);
							strEventDistance = objRdr["eventDistance"] is DBNull ? "Not entered" : Convert.ToString(objRdr["eventDistance"]);
							strEventLocation = objRdr["eventLocation"] is DBNull ? "Not entered" : Convert.ToString(objRdr["eventLocation"]);
							strEventWebsite = objRdr["eventLink"] is DBNull ? "Not entered" : "<a href='" + Convert.ToString(objRdr["eventLink"]) + "' target='_blank'>Visit event website</a>";
							strResults = objRdr["resultsLink"] is DBNull ? "Not available" : "<a href='" + Convert.ToString(objRdr["resultsLink"]) + "' target='_blank'>View results</a>";
						}
						else
						{
							lblNoEvent.Visible = true;
							lblNoEvent.Text = "<h3 class='error'>No event found</h3>";
						}
					}
				}
			}
		}
	}
}	