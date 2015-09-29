using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

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
			using(SqlConnection objConn = new SqlConnection(ConfigurationManager.AppSettings["triclubDSN"]))
			{
				objConn.Open();
				
				SqlCommand objCmd = objConn.CreateCommand();
				objCmd.CommandText =
@"
SELECT count(*) AS TotalEntries, FirstName, UPPER(LastName) AS ULastName, Club FROM Entries 
WHERE Accept='Y' 
ORDER BY LastName ASC
";
				using(SqlDataReader objRdr = objCmd.ExecuteReader())
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