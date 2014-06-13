using System;
using MySql.Data;
using MySql.Data.MySqlClient; 
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net.Mail;
using System.Collections;

namespace cFront.Projects.CFSL.Web.UI.UserControls
{
	// ===============================================================================================================================================================
    public class EntriesSoFar : UserControl
	{
		protected int			intEntries, intEntriesLeft;
		protected Repeater		rpEntries;
		
		void Page_Load()
		{
			getEntries();
		}
		
		protected void getEntries()
		{
			using(MySqlConnection objConn = new MySqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
			{
				objConn.Open();
				
				MySqlCommand objCmd = objConn.CreateCommand();
				objCmd.CommandText =
@"
SELECT FirstName, UPPER(LastName) AS ULastName, Club, Mins, Secs, T.eventtypename AS EventType 
FROM Entries E INNER JOIN entrieseventtype T ON E.EventType = T.eventtypeid
WHERE Accept = 1
ORDER BY LastName ASC
";
				using(MySqlDataReader objRdr = objCmd.ExecuteReader())
				{

					rpEntries.DataSource = objRdr;
					rpEntries.DataBind();
					
					//intEntries = objRdr["TotalEntries"] is DBNull ? 0 : Convert.ToInt32(objRdr["TotalEntries"]);
					
				}
				
				objCmd.CommandText =
@"
SELECT count(*) AS TotalEntries from Entries WHERE Accept=1
";
				using(MySqlDataReader objRdr = objCmd.ExecuteReader())
				{

					objRdr.Read();
					
					intEntries = objRdr["TotalEntries"] is DBNull ? 0 : Convert.ToInt32(objRdr["TotalEntries"]);
					
				}
			}
		}
	}
}	