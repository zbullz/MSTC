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

namespace cFront.Projects.CFSL.Web.UI.UserControls
{
	// ===============================================================================================================================================================
    public class displayEventsHome : UserControl
	{
		protected Repeater 	rpEventsHome;
		
		void Page_Load()
		{
			getEventsHome();
		}
		
		// Add a new event to the database 
		protected void getEventsHome()
		{
			{
				using(MySqlConnection objConn = new MySqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					MySqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
SELECT E.IID as eventID, E.eventTitle, E.eventDate, T.eventTypeDefinition AS eventType, D.eventDistanceDefinition AS eventDistance FROM Events E
INNER JOIN eventTypeDefinitions T ON E.eventType = T.eventTypeID
INNER JOIN eventDistanceDefinitions D ON E.eventDistance = D.eventDistanceID
WHERE eventDate >= NOW()
ORDER BY eventDate
LIMIT 7
";
					using(MySqlDataReader objRdr = objCmd.ExecuteReader())
					{
						rpEventsHome.DataSource = objRdr;
						rpEventsHome.DataBind();
					}
				}
			}
		}
	}
}	