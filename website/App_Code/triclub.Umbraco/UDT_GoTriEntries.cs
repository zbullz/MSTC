using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace triclub.Umbraco
{
	// ===============================================================================================================================================================
    public class UDT_GoTriEntries : UserControl, umbraco.editorControls.userControlGrapper.IUsercontrolDataEditor
	{
		#region IUsercontrolDataEditor Members
        public object value { get { return ""; } set { } }
        #endregion
		
		/// <summary>
        /// DocumentType alias for nodes that are folders of event items
        /// </summary>
        public String FolderAlias { get { return "goTriEntriesList"; } }
        /// <summary>
        /// DocumentType alias for nodes that are event items
        /// </summary>
        //  public String ItemAlias { get { return "GreenRegisterItem"; } }
        /// <summary>
        /// Pattern to match property aliases that are of PDCalendar (http://our.umbraco.org/projects/website-utilities/pdcalendar) type
        /// </summary>
        /// public String PDCalendarTypeExpression { get { return "eventDate"; } }
	
		protected DataList 		dlEntries;
		protected TextBox 		txtFirstName, txtLastName, txtParentFirstName, txtParentLastName, etxtFirstName, etxtLastName;
		protected TextBox 		etxtPayment;
		protected DropDownList 	ddlGender, ddlDOBY, ddlDOBM, ddlDOBD;
		protected TextBox 		txtAddr1, txtAddr2, txtAddr3, txtAddr4, txtPostcode, txtPhone, txtEmail, txtPayment, txtComments;
		protected Boolean		boolAccepted;
		protected string		strAccept;
	
		void Page_Load()
		{	
			if (!IsPostBack) 
			{
				getEntries();
			}
		}
		
		protected void getEntries()
		{
			using(SqlConnection objConn = new SqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
			{
				objConn.Open();
				SqlCommand objCmd = objConn.CreateCommand();
				objCmd.CommandText=
@"
SELECT IID, FirstName, UPPER(LastName) AS ULastName, ParentFirstName, ParentLastName, EntryDate, PaymentRef, Accept, Email, Phone,  
CASE Accept
        WHEN 1 THEN 'Yes'
        ELSE 'No'
        END
		AS Confirmed
FROM gotrientries
ORDER BY EntryDate DESC
";
				using(SqlDataReader objRdr = objCmd.ExecuteReader())
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
			
			String strFirstName, strLastName, strEmail, strPayment;
			
			intID = Convert.ToInt32(dlEntries.DataKeys[e.Item.ItemIndex]);
			
			strFirstName = ((TextBox)e.Item.FindControl("etxtFirstName")).Text;
			strLastName = ((TextBox)e.Item.FindControl("etxtLastName")).Text;
			strEmail = ((TextBox)e.Item.FindControl("etxtEmail")).Text;
			strPayment = ((TextBox)e.Item.FindControl("etxtPayment")).Text;
			boolAccepted = Convert.ToBoolean(((CheckBox)e.Item.FindControl("ecbAccept")).Checked);
				
			using(SqlConnection objConn = new SqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
			{	
				objConn.Open();
				
				SqlCommand objCmd = objConn.CreateCommand();
				objCmd.CommandText =
@"
UPDATE gotrientries 
SET FirstName=@FirstName, LastName=@LastName, Email=@Email, PaymentRef=@Payment, Accept=@Accept
WHERE IID=@ID
";
				
				objCmd.Parameters.AddWithValue("@FirstName", strFirstName);
				objCmd.Parameters.AddWithValue("@LastName", strLastName);
				objCmd.Parameters.AddWithValue("@Email", strEmail);
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
			
			using(SqlConnection objConn = new SqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
			{
				objConn.Open();
				SqlCommand objCmd = objConn.CreateCommand();
				objCmd.CommandText=
@"DELETE FROM gotrientries WHERE IID=@ID";
				objCmd.Parameters.AddWithValue("@ID", intID);
				objCmd.ExecuteNonQuery();
			}
			
			dlEntries.EditItemIndex = -1;
			getEntries();
			
		}

	}
}	