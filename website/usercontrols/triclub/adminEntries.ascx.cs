using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class usercontrols_triclub_adminEntries : UserControl
{
	protected int intEntries;

	void Page_Load()
	{
		getEntries();
	}

	protected void getEntries()
	{
		using (SqlConnection objConn = new SqlConnection(ConfigurationManager.AppSettings["triclubDSN"]))
		{
			objConn.Open();

			SqlCommand objCmd = objConn.CreateCommand();
			objCmd.CommandText =
@"
SELECT count(*) AS TotalEntries, FirstName, UPPER(LastName) AS ULastName, Club FROM Entries 
WHERE Accept='Y' 
ORDER BY LastName ASC
";
			using (SqlDataReader objRdr = objCmd.ExecuteReader())
			{
				objRdr.Read();

				intEntries = objRdr["TotalEntries"] is DBNull ? 0 : Convert.ToInt32(objRdr["TotalEntries"]);
				rpEntries.DataSource = objRdr;
				rpEntries.DataBind();
			}
		}
	}
}