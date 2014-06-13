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
				using(MySqlConnection objConn = new MySqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					MySqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
SELECT * FROM eventTypeDefinitions
";
					using(MySqlDataReader objRdr = objCmd.ExecuteReader())
					{
						rpEventFilterType.DataSource = objRdr;
						rpEventFilterType.DataBind();
					}
					
					objCmd.Parameters.Clear();
					
					objCmd.CommandText = 
@"
SELECT * FROM eventDistanceDefinitions
";
					using(MySqlDataReader objRdr = objCmd.ExecuteReader())
					{
						rpEventFilterDistance.DataSource = objRdr;
						rpEventFilterDistance.DataBind();
					}
					
					
				}
			}
		}
	}
}	