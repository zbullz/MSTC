using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net.Mail;

namespace triclub.Umbraco
{
	// ===============================================================================================================================================================
    public class UDT_adminEntries : UserControl, umbraco.editorControls.userControlGrapper.IUsercontrolDataEditor
	{
		#region IUsercontrolDataEditor Members
        public object value { get { return ""; } set { } }
        #endregion
		
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
	
		protected DataList 		dlEntries;
		protected TextBox 		txtFirstName, txtLastName, etxtFirstName, etxtLastName;
		protected TextBox 		etxtPayment;
		protected DropDownList 	ddlGender, ddlDOBY, ddlDOBM, ddlDOBD;
		protected TextBox 		txtAddr1, txtAddr2, txtAddr3, txtAddr4, txtPostcode, txtPhone, txtEmail, txtClub, txtBTANum, txtMins, txtSecs, txtPayment, txtComments;
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
SELECT IID, FirstName, UPPER(LastName) AS ULastName, Club, EntryDate, Payment, Accept, Mins, Secs, Email, Phone, E.EventType, T.eventtypename AS EventTypeName,  
CASE Accept
        WHEN 1 THEN 'Yes'
        ELSE 'No'
        END
		AS Confirmed
FROM Entries E INNER JOIN entrieseventtype T ON E.EventType = T.eventtypeid
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
				
			using(SqlConnection objConn = new SqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
			{	
				objConn.Open();
				
				SqlCommand objCmd = objConn.CreateCommand();
				objCmd.CommandText =
@"
UPDATE Entries 
SET FirstName=?FirstName, LastName=?LastName, Email=?Email, Club=?Club, EventType=?EventType, Mins = ?Mins, Secs = ?Secs, Payment=?Payment, Accept=?Accept
WHERE IID=?ID
";
				
				objCmd.Parameters.AddWithValue("?FirstName", strFirstName);
				objCmd.Parameters.AddWithValue("?LastName", strLastName);
				objCmd.Parameters.AddWithValue("?Email", strEmail);
				objCmd.Parameters.AddWithValue("?Club", strClub);
				objCmd.Parameters.AddWithValue("?EventType", intEventType);
				objCmd.Parameters.AddWithValue("?Mins", strMins);
				objCmd.Parameters.AddWithValue("?Secs", strSecs);
				objCmd.Parameters.AddWithValue("?Payment", strPayment);
				objCmd.Parameters.AddWithValue("?Accept", boolAccepted);
				objCmd.Parameters.AddWithValue("?ID", intID);
				
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
@"DELETE FROM Entries WHERE IID=?ID";
				objCmd.Parameters.AddWithValue("?ID", intID);
				objCmd.ExecuteNonQuery();
			}
			
			dlEntries.EditItemIndex = -1;
			getEntries();
			
		}
		
		protected void addEntry(Object s, EventArgs e)
		{
			string strDob, strUser;
		
			strDob = (ddlDOBY.SelectedItem.Value + "/" + ddlDOBM.SelectedItem.Value + "/" + ddlDOBD.SelectedItem.Value);
			
			if(txtClub.Text == "")
				txtClub.Text = "Unattached";
			else	
				txtClub.Text = txtClub.Text;
			
			using(SqlConnection objConn = new SqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
			{
				objConn.Open();
				SqlCommand objCmd = objConn.CreateCommand();
				objCmd.CommandText=
@"
INSERT INTO Entries 
(FirstName, LastName, Gender, Dob, Addr1, Addr2, Addr3, Addr4, Postcode, Phone, Email, Club, BTFNum, Mins, Secs, Declaration, Payment, Accept, Comments) 
VALUES (?FirstName, ?LastName, ?Gender, ?Dob, ?Addr1, ?Addr2, ?Addr3, ?Addr4, ?Postcode, ?Phone, ?Email, ?Club, ?BTFNum, ?Mins, ?Secs, 'Y', 
?Payment, 'Y', ?Comments)
";
				objCmd.Parameters.AddWithValue("?FirstName", txtFirstName.Text);
				objCmd.Parameters.AddWithValue("?LastName", txtLastName.Text);
				objCmd.Parameters.AddWithValue("?Gender", ddlGender.SelectedItem.Value);
				objCmd.Parameters.AddWithValue("?Dob", strDob);
				objCmd.Parameters.AddWithValue("?Addr1", txtAddr1.Text);
				objCmd.Parameters.AddWithValue("?Addr2", txtAddr2.Text);
				objCmd.Parameters.AddWithValue("?Addr3", txtAddr3.Text);
				objCmd.Parameters.AddWithValue("?Addr4", txtAddr4.Text);
				objCmd.Parameters.AddWithValue("?Postcode", txtPostcode.Text);
				objCmd.Parameters.AddWithValue("?Phone", txtPhone.Text);
				objCmd.Parameters.AddWithValue("?Email", txtEmail.Text);
				objCmd.Parameters.AddWithValue("?Club", txtClub.Text);
				objCmd.Parameters.AddWithValue("?BTFNum", txtBTANum.Text);
				objCmd.Parameters.AddWithValue("?Mins", txtMins.Text);
				objCmd.Parameters.AddWithValue("?Secs", txtSecs.Text);
				objCmd.Parameters.AddWithValue("?Payment", txtPayment.Text);
				objCmd.Parameters.AddWithValue("?Comments", txtComments.Text);
				objCmd.ExecuteNonQuery();
			}
			
			SendMessage();
		}
		
		protected void SendMessage()
		{
			MailMessage objMail = new MailMessage();
			
			objMail.To.Add(txtEmail.Text);
			objMail.From = new MailAddress("sales@midsussextriclub.com");
			objMail.Subject = "Mid Sussex Triathlon - Entry Confirmation";
			
			objMail.IsBodyHtml = true;
			
			objMail.Body = "<p>Dear " + txtFirstName.Text + " " + txtLastName.Text + "</p><p>Thank you for entering the Mid Sussex Triathlon. We can confirm that your entry has been accepted.</p>" +
							"<p>Please visit <a href='http://www.midsussextriclub.com/the-mid-sussex-triathlon/'>www.midsussextriclub.com/the-mid-sussex-triathlon</a> for more information.<p>Mid Sussex Triathlon Club</p>";
			SmtpClient smtpClient = new SmtpClient();
			smtpClient.Send(objMail);
			
			ViewState["ViewEntryForm"] = 1;
		}
	}
}	