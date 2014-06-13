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

using cFront.Umbraco.Membership;

namespace cFront.Projects.CFSL.Web.UI.UserControls
{
	// ===============================================================================================================================================================
    public class displayEvents : UserControl
	{
		protected Repeater 		rpEvents, WhoElseRepeater;
		protected PlaceHolder	phEventsList, phEventDetail;
		protected int			intEventID, intEventTypeID, intEventDistanceID, intEventIsOld, intMemberID, intIsMemberID;
		protected string		strEventTitle, strEventDate, strEventType, strEventDistance, strEventLocation, strEventDescription, strEventWebsite, strSearchEvents, 
								strUser, strResults;
        protected Label 		lblNoEvent, NooneElseLabel;
		protected LoginView		MemberEventAddView, MemberAddResultsView, MemberDoingEvent;
		protected TextBox 		txtEventTitle, txtEventDate, txtEventDescription, txtEventLink, txtEventLocation, txtResultsLink;
		protected DropDownList 	ddlEventType, ddlEventDistance;
		protected Button		btnAddMemberToEvent, btnRemoveMemberFromEvent;
        protected Literal DebugText;
		
		void Page_Load()
		{
			intEventID = Convert.ToInt32(Request.QueryString["eventID"]);
			intEventTypeID = Convert.ToInt32(Request.QueryString["eventType"]);
			intEventDistanceID = Convert.ToInt32(Request.QueryString["eventDistance"]);
			intEventIsOld = Convert.ToInt32(Request.QueryString["eventIsOld"]);
			strSearchEvents = Request.Form["txtEventSearch"];
			
			if(intEventID == 0)
				getEvents();
			else
				getEventDetail();
		}
		
		// Get all future events
		protected void getEvents()
		{
			if(intEventTypeID == 0 && intEventDistanceID == 0 && intEventIsOld == 0 && strSearchEvents == null)
			{
				phEventsList.Visible = true; // show event list
				phEventDetail.Visible = false; // hide event detail
				
				{
					using(MySqlConnection objConn = new MySqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
					{
						objConn.Open();
						MySqlCommand objCmd = objConn.CreateCommand();
						objCmd.CommandText = 
	@"
	SELECT E.IID, E.eventTitle, E.eventDate, T.eventTypeDefinition AS eventType, D.eventDistanceDefinition AS eventDistance, E.eventLocation FROM Events E
	INNER JOIN eventTypeDefinitions T ON E.eventType = T.eventTypeID
	INNER JOIN eventDistanceDefinitions D ON E.eventDistance = D.eventDistanceID
	WHERE eventDate >= NOW()
	ORDER BY eventDate
	";
						using(MySqlDataReader objRdr = objCmd.ExecuteReader())
						{
							rpEvents.DataSource = objRdr;
							rpEvents.DataBind();
						}
					}
				}
			}
			if(strSearchEvents != null)
				getEventsSearched();
			else if(intEventIsOld != 0)
				getPastEvents();
			else if(intEventTypeID != 0 || intEventDistanceID != 0)
				getEventsFiltered();
		}
		
		// Get all past events
		protected void getPastEvents()
		{
			phEventsList.Visible = true; // show event list
			phEventDetail.Visible = false; // hide event detail
				
			{
				using(MySqlConnection objConn = new MySqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					MySqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
SELECT E.IID, E.eventTitle, E.eventDate, T.eventTypeDefinition AS eventType, D.eventDistanceDefinition AS eventDistance, E.eventLocation FROM Events E
INNER JOIN eventTypeDefinitions T ON E.eventType = T.eventTypeID
INNER JOIN eventDistanceDefinitions D ON E.eventDistance = D.eventDistanceID
WHERE eventDate < NOW()
ORDER BY eventDate desc
";
					using(MySqlDataReader objRdr = objCmd.ExecuteReader())
					{
						rpEvents.DataSource = objRdr;
						rpEvents.DataBind();
					}
				}
			}
		}
		
		// Get all future events filtered by appropriate filter
		protected void getEventsFiltered()
		{
			phEventsList.Visible = true; // show event list
			phEventDetail.Visible = false; // hide event detail
			
			{
				using(MySqlConnection objConn = new MySqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					MySqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
SELECT E.IID, E.eventTitle, E.eventDate, T.eventTypeDefinition AS eventType, D.eventDistanceDefinition AS eventDistance, E.eventLocation FROM Events E
INNER JOIN eventTypeDefinitions T ON E.eventType = T.eventTypeID
INNER JOIN eventDistanceDefinitions D ON E.eventDistance = D.eventDistanceID
WHERE eventDate >= NOW()
AND
(E.eventType = ?eventTypeID OR E.eventDistance = ?eventDistanceID)
ORDER BY eventDate
";
					objCmd.Parameters.AddWithValue("?eventTypeID", intEventTypeID);
					objCmd.Parameters.AddWithValue("?eventDistanceID", intEventDistanceID);
					
					using(MySqlDataReader objRdr = objCmd.ExecuteReader())
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
				using(MySqlConnection objConn = new MySqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					MySqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
SELECT E.IID, E.eventTitle, E.eventDate, T.eventTypeDefinition AS eventType, D.eventDistanceDefinition AS eventDistance, E.eventLocation FROM Events E
INNER JOIN eventTypeDefinitions T ON E.eventType = T.eventTypeID
INNER JOIN eventDistanceDefinitions D ON E.eventDistance = D.eventDistanceID
WHERE (E.eventTitle LIKE CONCAT('%',?eventSearch,'%')) OR ((YEAR(E.eventDate) LIKE CONCAT('%',?eventSearch,'%')) OR (MONTHNAME(E.eventDate) LIKE CONCAT('%',?eventSearch,'%')))
ORDER BY eventDate
";
					objCmd.Parameters.AddWithValue("?eventSearch", strSearchEvents);
					
					using(MySqlDataReader objRdr = objCmd.ExecuteReader())
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
				using(MySqlConnection objConn = new MySqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					MySqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
SELECT E.IID, E.eventTitle, E.eventDate, T.eventTypeDefinition AS eventType, D.eventDistanceDefinition AS eventDistance, E.eventLocation, E.eventDescription, E.eventLink, E.resultsLink, 
M.eventID, M.memberID 
FROM Events E INNER JOIN eventTypeDefinitions T ON E.eventType = T.eventTypeID
INNER JOIN eventDistanceDefinitions D ON E.eventDistance = D.eventDistanceID
LEFT OUTER JOIN MemberEvents M ON E.IID = M.eventID
WHERE E.IID = ?eventID
";
					objCmd.Parameters.AddWithValue("?eventID", intEventID);
					
					using(MySqlDataReader objRdr = objCmd.ExecuteReader())
					{
						if(objRdr.Read())
						{
							strEventTitle = objRdr["eventTitle"] is DBNull ? "Not entered" : Convert.ToString(objRdr["eventTitle"]);
							strEventDate = objRdr["eventDate"] is DBNull ? "Not entered" : Convert.ToString(String.Format("{0 : dd MMM yyyy}", objRdr["eventDate"]));
							strEventType = objRdr["eventType"] is DBNull ? "Not entered" : Convert.ToString(objRdr["eventType"]);
							strEventDistance = objRdr["eventDistance"] is DBNull ? "Not entered" : Convert.ToString(objRdr["eventDistance"]);
							strEventLocation = objRdr["eventLocation"] is DBNull ? "Not entered" : Convert.ToString(objRdr["eventLocation"]);
							strEventDescription = objRdr["eventDescription"] is DBNull ? "Not entered" : Convert.ToString(objRdr["eventDescription"]);
							strEventWebsite = objRdr["eventLink"] is DBNull ? "Not entered" : "<a href='" + Convert.ToString(objRdr["eventLink"]) + "' target='_blank'>Visit event website</a>";
							strResults = objRdr["resultsLink"] is DBNull || objRdr["resultsLink"] == "" ? "Not available" : "<a href='" + Convert.ToString(objRdr["resultsLink"]) + "' target='_blank'>View results</a>";
							intIsMemberID = objRdr["memberID"] is DBNull ? 0 : Convert.ToInt32(objRdr["memberID"]);
						}
						else
						{
							lblNoEvent.Visible = true;
							lblNoEvent.Text = "<h3 class='error'>No event found</h3>";
						}
					}
				}
				
				if(strResults != "Not available")
					MemberAddResultsView.Visible = false;
			}
			
				if(MemberHelper.IsLoggedIn)
				{
					// List all participants
					getEventMembers();
					getMemberIsDoingEvent();
				}
		}
		
		// Get details for chosen event
		protected void getMemberIsDoingEvent()
		{
			{
				using(MySqlConnection objConn = new MySqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					MySqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
SELECT * FROM MemberEvents 
where memberid = ?memberID and eventid = ?eventID
";
					objCmd.Parameters.AddWithValue("?eventID", intEventID);
					objCmd.Parameters.AddWithValue("?memberID",  Convert.ToInt32(System.Web.Security.Membership.GetUser().ProviderUserKey));
					
					using(MySqlDataReader objRdr = objCmd.ExecuteReader())
					{
						if(objRdr.Read())
						{
							intIsMemberID = objRdr["memberID"] is DBNull ? 0 : Convert.ToInt32(objRdr["memberID"]);
						}
					}
				}
			}
			
			if(MemberHelper.IsLoggedIn)
			{
				if(intIsMemberID == Convert.ToInt32(System.Web.Security.Membership.GetUser().ProviderUserKey))
				{
					((Button)MemberDoingEvent.FindControl("btnAddMemberToEvent")).Visible = false;
					((Button)MemberDoingEvent.FindControl("btnRemoveMemberFromEvent")).Visible = true;
				}
			}
		}
	
		// Add a new event to the database 
		protected void addEvent(Object s, EventArgs e)
		{
			{
				using(MySqlConnection objConn = new MySqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					MySqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
INSERT INTO Events (eventTitle, eventDate, eventType, eventDistance, eventDescription, eventLink, eventLocation) 
VALUES (?eventTitle, ?eventDate, ?eventType, ?eventDistance, ?eventDescription, ?eventLink, ?eventLocation)
";
					objCmd.Parameters.AddWithValue("?eventTitle", ((TextBox)MemberEventAddView.FindControl("txtEventTitle")).Text);
					objCmd.Parameters.AddWithValue("?eventDate", ((TextBox)MemberEventAddView.FindControl("txtEventDate")).Text);
					objCmd.Parameters.AddWithValue("?eventType", ((DropDownList)MemberEventAddView.FindControl("ddlEventType")).SelectedItem.Value);
					objCmd.Parameters.AddWithValue("?eventDistance", ((DropDownList)MemberEventAddView.FindControl("ddlEventDistance")).SelectedItem.Value);
					objCmd.Parameters.AddWithValue("?eventDescription", ((TextBox)MemberEventAddView.FindControl("txtEventDescription")).Text);
					objCmd.Parameters.AddWithValue("?eventLink", ((TextBox)MemberEventAddView.FindControl("txtEventLink")).Text);
					objCmd.Parameters.AddWithValue("?eventLocation", ((TextBox)MemberEventAddView.FindControl("txtEventLocation")).Text);

					objCmd.ExecuteNonQuery();
				}
			}
			
			Response.Redirect(Request.Url.ToString());
		}
		
		// Add a result to selected event
		protected void addResults(Object s, EventArgs e)
		{
			{
				using(MySqlConnection objConn = new MySqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					MySqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
UPDATE Events SET resultsLink = ?resultsLink WHERE IID = ?eventID
";
					objCmd.Parameters.AddWithValue("?resultsLink", ((TextBox)MemberAddResultsView.FindControl("txtResultsLink")).Text);
					objCmd.Parameters.AddWithValue("?eventID", intEventID);

					objCmd.ExecuteNonQuery();
				}
			}
			Response.Redirect(Request.Url.ToString());
		}
		
		protected void addMemberToEvent(Object s, EventArgs e)
		{
			strUser = Convert.ToString(System.Web.Security.Membership.GetUser().ProviderUserKey);
			intMemberID = Convert.ToInt32(System.Web.Security.Membership.GetUser().ProviderUserKey);
			
			// Create record in db to link event ID to Member ID in MemberEvents table
			{
				using(MySqlConnection objConn = new MySqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					MySqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
INSERT INTO MemberEvents (eventID, memberID) 
VALUES (?eventID, ?memberID)
";
					objCmd.Parameters.AddWithValue("?eventID", intEventID);
					objCmd.Parameters.AddWithValue("?memberID", intMemberID);

					objCmd.ExecuteNonQuery();
				}
			}
			
			Response.Redirect(Request.Url.ToString());
		}
		
		// Remove the member's id from the event
		protected void removeMemberFromEvent(Object s, EventArgs e)
		{
			strUser = Convert.ToString(System.Web.Security.Membership.GetUser().ProviderUserKey);
			intMemberID = Convert.ToInt32(System.Web.Security.Membership.GetUser().ProviderUserKey);
			
			// Remove record in db to link event ID to Member ID in MemberEvents table
			{
				using(MySqlConnection objConn = new MySqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					MySqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
DELETE FROM MemberEvents
WHERE eventID = ?eventID
AND memberID = ?memberID
";
					objCmd.Parameters.AddWithValue("?eventID", intEventID);
					objCmd.Parameters.AddWithValue("?memberID", intMemberID);

					objCmd.ExecuteNonQuery();
				}
			}
			
			Response.Redirect(Request.Url.ToString());
		}
		
		// Get list memberIDs for all members doing this event
		protected void getEventMembers()
		{
			{
				using(MySqlConnection objConn = new MySqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					MySqlCommand objCmd = objConn.CreateCommand();

                    // Get everyone who isn't the curretn member who is doing this event
					objCmd.CommandText = 
@"
SELECT MemberID
FROM MemberEvents
WHERE EventID = ?eventID 

";
					objCmd.Parameters.AddWithValue("?eventID", intEventID);
                    objCmd.Parameters.AddWithValue("?memberID", MemberHelper.Get()["ID"]);

                    List<IDictionary<String, object>> memdetails = new List<IDictionary<String, object>>();

					using(MySqlDataReader objRdr = objCmd.ExecuteReader())
					{
                        while (objRdr.Read())
                        {
                            // Get member data for each ID in map table
                            IDictionary<String, object> m = MemberHelper.Get(Convert.ToInt32(objRdr["MemberID"]));

                            // If m is null, it means the member has been deleted. Not likely, but check to be sure
                            if (m != null)
                                memdetails.Add(m);
                        }
					}

                    // If nothing in list, just logged in member hsa signed up so far.
                    NooneElseLabel.Visible = memdetails.Count == 0;

                    WhoElseRepeater.DataSource = memdetails;
                    WhoElseRepeater.DataBind();
				}
			}
		}			
	}
}	