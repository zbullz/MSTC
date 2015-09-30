using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class usercontrols_triclub_entriesLeft : System.Web.UI.UserControl
{
	protected string strEntriesLeft;

	void Page_Load()
	{
		getEntries();
	}

	protected void getEntries()
	{
		int intEntriesAvailable = 350;
		int intEntriesLeft;
		using (SqlConnection objConn = new SqlConnection(ConfigurationManager.AppSettings["triclubDSN"]))
		{
			objConn.Open();

			SqlCommand objCmd = objConn.CreateCommand();
			objCmd.CommandText =
@"
select count(*) AS TotalEntries from Entries WHERE Accept=1
";
			using (SqlDataReader objRdr = objCmd.ExecuteReader())
			{
				objRdr.Read();

				int intEntries = objRdr["TotalEntries"] is DBNull ? 0 : Convert.ToInt32(objRdr["TotalEntries"]);
				intEntriesLeft = intEntriesAvailable - intEntries;
			}
		}

		if (intEntriesLeft > 0)
			strEntriesLeft = Convert.ToString(intEntriesLeft);
		else
			strEntriesLeft = "FULL";
	}
}