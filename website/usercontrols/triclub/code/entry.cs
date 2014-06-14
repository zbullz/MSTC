using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net.Mail;
using System.Collections;

namespace cFront.Projects.CFSL.Web.UI.UserControls
{
	// ===============================================================================================================================================================
    public class EntryForm : UserControl
	{
		protected TextBox 		txtFirstName, txtLastName, txtAddr1, txtAddr2, txtAddr3, txtAddr4, txtPostcode, txtPhone, txtEmail, txtClub, txtBTF;
		protected DropDownList 	ddlDateDay, ddlDateMonth, ddlDateYear, ddlGender, ddlSwimMins, ddlSwimSecs;
		protected PlaceHolder 	containerEntry, containerEntryFull;
		protected Label			lblTerms, lblCaptcha;
		protected RadioButton	rbFull, rbAquabike;
		protected CheckBox		cbAgree;
		protected int			intSecretEntry;
		protected Boolean		boolSecretEntry;
		protected int			intViewState, intEntries, intEventType;
		
		void Page_Load()
		{
			intSecretEntry = Convert.ToInt32(Request.QueryString["secretsteve"]);
			
			getPageSettings();
			getEntries();
		}
		
		protected void getPageSettings()
		{
			if(!IsPostBack)
			{
				ArrayList monthArray = new ArrayList();
				ArrayList dayArray = new ArrayList();
				ArrayList yearArray = new ArrayList();

				int i;
				
				for(i=1; i<13; i++)
					monthArray.Add(i);
				for(i=1; i<32; i++)
					dayArray.Add(i);
				for(i=(Convert.ToInt32(DateTime.Now.Year - 90)); i<(Convert.ToInt32(DateTime.Now.Year - 15)); i++)
					yearArray.Add(i);

				
				ddlDateDay.DataSource = dayArray;
				ddlDateDay.DataBind();
				ddlDateMonth.DataSource = monthArray;
				ddlDateMonth.DataBind();
				ddlDateYear.DataSource = yearArray;
				ddlDateYear.DataBind();
			}
		}
		
		protected void addEntry(Object s, EventArgs e)
		{
			if(cbAgree.Checked && Page.IsValid)
			{
				string 	strDOB, strClub;
			
				strDOB = (ddlDateYear.SelectedItem.Value + "/" + ddlDateMonth.SelectedItem.Value + "/" + ddlDateDay.SelectedItem.Value);
				strClub = txtClub.Text;
				
				if (strClub == "")
				{
					strClub = "Unattached";
				}
				else
				{
					strClub = strClub;
				}
				
				if(rbFull.Checked)
				{
					intEventType = 1;
				}
				else if(rbAquabike.Checked)
				{
					intEventType = 2;
				}
			
				using(SqlConnection objConn = new SqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
				{
					objConn.Open();
					SqlCommand objCmd = objConn.CreateCommand();
					objCmd.CommandText = 
@"
INSERT INTO Entries (FirstName, LastName, Gender, DOB, Addr1, Addr2, Addr3, Addr4, Postcode, 
Phone, Email, Club, BTFNum, Mins, Secs, Declaration, Accept, EntryDate, EventType) 
VALUES (@FirstName, @LastName, @Gender, @DOB, @Addr1, @Addr2, @Addr3, @Addr4, @Postcode, 
@Phone, @Email, @Club, @BTFNum, @Mins, @Secs, 1, 0, @Now, @EventType)
";
					
					objCmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
					objCmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
					objCmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedItem.Value);
					objCmd.Parameters.AddWithValue("@DOB", strDOB);
					objCmd.Parameters.AddWithValue("@Addr1", txtAddr1.Text);
					objCmd.Parameters.AddWithValue("@Addr2", txtAddr2.Text);
					objCmd.Parameters.AddWithValue("@Addr3", txtAddr3.Text);
					objCmd.Parameters.AddWithValue("@Addr4", txtAddr4.Text);
					objCmd.Parameters.AddWithValue("@Postcode", txtPostcode.Text);
					objCmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
					objCmd.Parameters.AddWithValue("@Email", txtEmail.Text);
					objCmd.Parameters.AddWithValue("@Club", strClub);
					objCmd.Parameters.AddWithValue("@BTFNum", txtBTF.Text);
					objCmd.Parameters.AddWithValue("@Mins", ddlSwimMins.SelectedItem.Text);
					objCmd.Parameters.AddWithValue("@Secs", ddlSwimSecs.SelectedItem.Text);
					objCmd.Parameters.AddWithValue("@Now", DateTime.Now);
					objCmd.Parameters.AddWithValue("@EventType", intEventType);
				
					objCmd.ExecuteNonQuery();
				}
				
				SendMessage();
				Response.Redirect("race-entry/race-entry-payment.aspx");
			}
			else
			{
				if(!cbAgree.Checked)
				{
					lblTerms.Text = "<span class='mst_fielderror_terms'><span class='ERROR'>Error!</span>You must agree to the race terms &#038; conditions to proceed</span>";
					lblTerms.Visible = true;
				}
				if(!Page.IsValid)
				{
					lblCaptcha.Text = "<span class='mst_fielderror_terms'><span class='ERROR'>Error!</span>Incorrect please try again</span>";
					lblCaptcha.Visible = true;
				}
			}
		}

		protected void SendMessage()
		{
			MailMessage objMail = new MailMessage();		
			objMail.To.Add(ConfigurationManager.AppSettings["midSussexTriEntryEmailTo"] ?? "sales@midsussextriclub.com");
			objMail.From = new MailAddress("noreply@midsussextriclub.com", "noreply@midsussextriclub.com");
			objMail.Subject = "Mid Sussex Triathlon - Entry Received";
			
			objMail.IsBodyHtml = true;
			
			objMail.Body = "<p>" + txtFirstName.Text + " " + txtLastName.Text +" has entered the event.</p><p><a href='http://www.midsussextriclub.com' target='_blank'>Go to Mid Sussex Tri Club site</a></p>";
			
			GmailSmtpClient GmailSmtpClient = new GmailSmtpClient();
			GmailSmtpClient.Send(objMail);
				
			ViewState["ViewEntryForm"] = 1;
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
				}
				
				if(intEntries >= 350 && intSecretEntry != 1)
				{
					containerEntryFull.Visible = true;
					containerEntry.Visible = false;
				}
				else
				{
					containerEntryFull.Visible = false;
					containerEntry.Visible = true;
				}
			}
		}
	}
}	