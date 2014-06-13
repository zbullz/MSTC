using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Configuration;

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
			using(SqlConnection objConn = new SqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
			{
				objConn.Open();
				
				SqlCommand objCmd = objConn.CreateCommand();
				objCmd.CommandText =
@"
select count(*) AS TotalEntries from Entries WHERE Accept=1
";
				using(SqlDataReader objRdr = objCmd.ExecuteReader())
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