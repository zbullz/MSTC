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
    public class AdminEntries : UserControl
	{
		protected int			intEntries, intEntriesLeft;
		protected string		strFirstName, strLastName, strClub;
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
SELECT count(*) AS TotalEntries, FirstName, UPPER(LastName) AS ULastName, Club FROM Entries 
WHERE Accept='Y' 
ORDER BY LastName ASC
";
				using(MySqlDataReader objRdr = objCmd.ExecuteReader())
				{
					objRdr.Read();
					
					intEntries = objRdr["TotalEntries"] is DBNull ? 0 : Convert.ToInt32(objRdr["TotalEntries"]);
					intEntriesLeft = (350 - intEntries);
					
					rpEntries.DataSource = objRdr;
					rpEntries.DataBind();
				}
			}
		}
	}
}	