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
    public class EntriesLeft : UserControl
	{
		protected int			intEntries, intEntriesLeft, intEntriesAvailable;
		protected string		strEntriesLeft;
		
		void Page_Load()
		{
			intEntriesAvailable = 350;
		
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
select count(*) AS TotalEntries from Entries WHERE Accept=1
";
				using(MySqlDataReader objRdr = objCmd.ExecuteReader())
				{
					objRdr.Read();
					
					intEntries = objRdr["TotalEntries"] is DBNull ? 0 : Convert.ToInt32(objRdr["TotalEntries"]);
					intEntriesLeft = intEntriesAvailable - intEntries;
				}
			}
			
			if(intEntriesLeft > 0)
				strEntriesLeft = Convert.ToString(intEntriesLeft);
			else
				strEntriesLeft = "FULL";
		}
	}
}	