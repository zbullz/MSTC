using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace cFront.Projects.CFSL.Web.UI.UserControls
{
	// ===============================================================================================================================================================
    public class eventFilters : UserControl
	{
		protected Repeater 		rpEventFilterType, rpEventFilterDistance;
		
		void Page_Load()
		{
			getEventFilters();
		}
		
		// Get all future events
		protected void getEventFilters()
		{
			{
				using(SqlConnection objConn = new SqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					SqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
SELECT * FROM eventTypeDefinitions
";
					using(SqlDataReader objRdr = objCmd.ExecuteReader())
					{
						rpEventFilterType.DataSource = objRdr;
						rpEventFilterType.DataBind();
					}
					
					objCmd.Parameters.Clear();
					
					objCmd.CommandText = 
@"
SELECT * FROM eventDistanceDefinitions
";
					using(SqlDataReader objRdr = objCmd.ExecuteReader())
					{
						rpEventFilterDistance.DataSource = objRdr;
						rpEventFilterDistance.DataBind();
					}
					
					
				}
			}
		}
	}
}	