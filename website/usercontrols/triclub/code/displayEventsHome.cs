using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

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
				using(SqlConnection objConn = new SqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					SqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
SELECT E.IID as eventID, E.eventTitle, E.eventDate, T.eventTypeDefinition AS eventType, D.eventDistanceDefinition AS eventDistance FROM Events E
INNER JOIN eventTypeDefinitions T ON E.eventType = T.eventTypeID
INNER JOIN eventDistanceDefinitions D ON E.eventDistance = D.eventDistanceID
WHERE eventDate >= GetDate()
ORDER BY eventDate
LIMIT 7
";
					using(SqlDataReader objRdr = objCmd.ExecuteReader())
					{
						rpEventsHome.DataSource = objRdr;
						rpEventsHome.DataBind();
					}
				}
			}
		}
	}
}	