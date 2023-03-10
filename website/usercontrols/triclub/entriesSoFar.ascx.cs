using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class usercontrols_triclub_entriesSoFar : System.Web.UI.UserControl
{
	protected int intEntries;

	protected void Page_Load(object sender, EventArgs e)
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
SELECT FirstName, UPPER(LastName) AS ULastName, Club, Mins, Secs, T.eventtypename AS EventType 
FROM Entries E INNER JOIN entrieseventtype T ON E.EventType = T.eventtypeid
WHERE Accept = 1
ORDER BY LastName ASC
";
			using (SqlDataReader objRdr = objCmd.ExecuteReader())
			{

				rpEntries.DataSource = objRdr;
				rpEntries.DataBind();

				//intEntries = objRdr["TotalEntries"] is DBNull ? 0 : Convert.ToInt32(objRdr["TotalEntries"]);

			}

			objCmd.CommandText =
				@"
SELECT count(*) AS TotalEntries from Entries WHERE Accept=1
";
			using (SqlDataReader objRdr = objCmd.ExecuteReader())
			{

				objRdr.Read();

				intEntries = objRdr["TotalEntries"] is DBNull ? 0 : Convert.ToInt32(objRdr["TotalEntries"]);

			}
		}
	}
}