using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using Mstc.Core.configuration;

public partial class usercontrols_triclub_UDT_adminEntries : UserControl, umbraco.editorControls.userControlGrapper.IUsercontrolDataEditor
{

	public object value { get { return ""; } set { } }

	/// <summary>
	/// DocumentType alias for nodes that are folders of event items
	/// </summary>
	public String FolderAlias { get { return "triclubEntriesList"; } }
	/// <summary>
	/// DocumentType alias for nodes that are event items
	/// </summary>
	//  public String ItemAlias { get { return "GreenRegisterItem"; } }
	/// <summary>
	/// Pattern to match property aliases that are of PDCalendar (http://our.umbraco.org/projects/website-utilities/pdcalendar) type
	/// </summary>
	/// public String PDCalendarTypeExpression { get { return "eventDate"; } }

	protected Boolean boolAccepted;
	protected string strAccept;

	void Page_Load()
	{
		if (!IsPostBack)
		{
			getEntries();
		}
	}

	protected void getEntries()
	{
		using (SqlConnection objConn = new SqlConnection(ConfigurationManager.AppSettings["triclubDSN"]))
		{
			objConn.Open();
			SqlCommand objCmd = objConn.CreateCommand();
			objCmd.CommandText =
@"
SELECT IID, FirstName, UPPER(LastName) AS ULastName, Club, EntryDate, Payment, Accept, Mins, Secs, Email, Phone, E.EventType, T.eventtypename AS EventTypeName,  
CASE Accept
        WHEN 1 THEN 'Yes'
        ELSE 'No'
        END
		AS Confirmed
FROM Entries E INNER JOIN entrieseventtype T ON E.EventType = T.eventtypeid
ORDER BY EntryDate DESC
";
			using (SqlDataReader objRdr = objCmd.ExecuteReader())
			{
				dlEntries.DataSource = objRdr;
				dlEntries.DataBind();
			}
		}
	}

	protected void Edit_Command(Object s, DataListCommandEventArgs e)
	{
		dlEntries.EditItemIndex = e.Item.ItemIndex;
		getEntries();
	}

	protected void Cancel_Command(Object s, DataListCommandEventArgs e)
	{
		dlEntries.EditItemIndex = -1;
		getEntries();
	}

	protected void Update_Command(Object s, DataListCommandEventArgs e)
	{
		int intID, intEventType;

		String strFirstName, strLastName, strEmail, strClub, strMins, strSecs, strPayment;

		intID = Convert.ToInt32(dlEntries.DataKeys[e.Item.ItemIndex]);

		strFirstName = ((TextBox)e.Item.FindControl("etxtFirstName")).Text;
		strLastName = ((TextBox)e.Item.FindControl("etxtLastName")).Text;
		strEmail = ((TextBox)e.Item.FindControl("etxtEmail")).Text;
		strClub = ((TextBox)e.Item.FindControl("etxtClub")).Text;
		intEventType = Convert.ToInt32(((DropDownList)e.Item.FindControl("eddlEventType")).SelectedItem.Value);
		strMins = ((TextBox)e.Item.FindControl("etxtMins")).Text;
		strSecs = ((TextBox)e.Item.FindControl("etxtSecs")).Text;
		strPayment = ((TextBox)e.Item.FindControl("etxtPayment")).Text;
		boolAccepted = Convert.ToBoolean(((CheckBox)e.Item.FindControl("ecbAccept")).Checked);

		using (SqlConnection objConn = new SqlConnection(ConfigurationManager.AppSettings["triclubDSN"]))
		{
			objConn.Open();

			SqlCommand objCmd = objConn.CreateCommand();
			objCmd.CommandText =
@"
UPDATE Entries 
SET FirstName=@FirstName, LastName=@LastName, Email=@Email, Club=@Club, EventType=@EventType, Mins = @Mins, Secs = @Secs, Payment=@Payment, Accept=@Accept
WHERE IID=@ID
";

			objCmd.Parameters.AddWithValue("@FirstName", strFirstName);
			objCmd.Parameters.AddWithValue("@LastName", strLastName);
			objCmd.Parameters.AddWithValue("@Email", strEmail);
			objCmd.Parameters.AddWithValue("@Club", strClub);
			objCmd.Parameters.AddWithValue("@EventType", intEventType);
			objCmd.Parameters.AddWithValue("@Mins", strMins);
			objCmd.Parameters.AddWithValue("@Secs", strSecs);
			objCmd.Parameters.AddWithValue("@Payment", strPayment);
			objCmd.Parameters.AddWithValue("@Accept", boolAccepted);
			objCmd.Parameters.AddWithValue("@ID", intID);

			objCmd.ExecuteNonQuery();
		}

		dlEntries.EditItemIndex = -1;
		getEntries();
	}

	protected void Delete_Command(Object s, DataListCommandEventArgs e)
	{
		int intID;

		intID = Convert.ToInt32(dlEntries.DataKeys[e.Item.ItemIndex]);

		using (SqlConnection objConn = new SqlConnection(ConfigurationManager.AppSettings["triclubDSN"]))
		{
			objConn.Open();
			SqlCommand objCmd = objConn.CreateCommand();
			objCmd.CommandText = @"DELETE FROM Entries WHERE IID=@ID";
			objCmd.Parameters.AddWithValue("@ID", intID);
			objCmd.ExecuteNonQuery();
		}

		dlEntries.EditItemIndex = -1;
		getEntries();
	}
}