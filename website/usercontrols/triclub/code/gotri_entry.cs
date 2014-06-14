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
		protected TextBox 		txtFirstName, txtLastName, 
								txtAge, txtSchool, txtSchoolYear, txtMedical, txtYPEmail, txtDiet, txtExperience, 
								txtParentFirstName, txtParentLastName, 
								txtAddr1, txtAddr2, txtAddr3, txtAddr4, txtPostcode, txtPhone, txtEmail, txtName;
		protected DropDownList 	ddlDateDay, ddlDateMonth, ddlDateYear, ddlGender;
		protected PlaceHolder 	containerEntry, containerEntryClosed;
		protected Label			lblTerms, lblCaptcha;
		protected CheckBox		cbAgree;
		protected int			intSecretEntry;
		protected Boolean		boolSecretEntry;
		protected int			intViewState, intEntries, intEventType;
		
		void Page_Load()
		{
			intSecretEntry = Convert.ToInt32(Request.QueryString["secretsteve"]);
			
			getPageSettings();
			//getEntries();
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
				for(i=(Convert.ToInt32(DateTime.Now.Year - 17)); i<(Convert.ToInt32(DateTime.Now.Year - 8)); i++)
					yearArray.Add(i);

				
				ddlDateDay.DataSource = dayArray;
				ddlDateDay.DataBind();
				ddlDateMonth.DataSource = monthArray;
				ddlDateMonth.DataBind();
				ddlDateYear.DataSource = yearArray;
				ddlDateYear.DataBind();
			}
		}
		
		protected void addEntry()
		{
			string 	strDOB;
		
			strDOB = (ddlDateYear.SelectedItem.Value + "/" + ddlDateMonth.SelectedItem.Value + "/" + ddlDateDay.SelectedItem.Value);

			using(var objConn = new SqlConnection(ConfigurationSettings.AppSettings["triclubDSN"]))
			{
				objConn.Open();
				SqlCommand objCmd = objConn.CreateCommand();
				objCmd.CommandText = 
@"
INSERT INTO gotrientries (FirstName, LastName, DOB, Age, Gender, School, SchoolYear, Email, 
MedicalNotes, DietNotes, TriExperienceNotes, 
ParentFirstName, ParentLastName, Address1, Address2, Address3, Address4, Postcode, 
Phone, ParentEmail, TandCName, TandCAgree, EntryDate, Accept) 
VALUES (@FirstName, @LastName, @DOB, @Age, @Gender, @School, @SchoolYear, @Email, 
@MedicalNotes, @DietNotes, @TriExperienceNotes, 
@ParentFirstName, @ParentLastName, @Address1, @Address2, @Address3, @Address4, @Postcode, 
@Phone, @ParentEmail, @TandCName, 1, @Now, 0)
";
				
				objCmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
				objCmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
				objCmd.Parameters.AddWithValue("@DOB", strDOB);
				objCmd.Parameters.AddWithValue("@Age", strDOB);
				objCmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedItem.Value);
				objCmd.Parameters.AddWithValue("@School", txtSchool.Text);
				objCmd.Parameters.AddWithValue("@SchoolYear", txtSchoolYear.Text);
				objCmd.Parameters.AddWithValue("@Email", txtYPEmail.Text);
				objCmd.Parameters.AddWithValue("@MedicalNotes", txtMedical.Text);
				objCmd.Parameters.AddWithValue("@DietNotes", txtDiet.Text);
				objCmd.Parameters.AddWithValue("@TriExperienceNotes", txtExperience.Text);
				objCmd.Parameters.AddWithValue("@ParentFirstName", txtParentFirstName.Text);
				objCmd.Parameters.AddWithValue("@ParentLastName", txtParentLastName.Text);
				objCmd.Parameters.AddWithValue("@Address1", txtAddr1.Text);
				objCmd.Parameters.AddWithValue("@Address2", txtAddr2.Text);
				objCmd.Parameters.AddWithValue("@Address3", txtAddr3.Text);
				objCmd.Parameters.AddWithValue("@Address4", txtAddr4.Text);
				objCmd.Parameters.AddWithValue("@Postcode", txtPostcode.Text);
				objCmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
				objCmd.Parameters.AddWithValue("@ParentEmail", txtEmail.Text);
				objCmd.Parameters.AddWithValue("@TandCName", txtName.Text);
				objCmd.Parameters.AddWithValue("@Now", DateTime.Now);
				
				objCmd.ExecuteNonQuery();
			}
		}
		
		protected void submitEntry(object s, EventArgs e)
		{
			if(cbAgree.Checked && Page.IsValid)
			{
				addEntry();
				SendMessage();
				SendMessagetoParent();
				Response.Redirect("/mid-sussex-triathlon-juniors/gotri!/gotri!-payment.aspx");
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
			
			string 	strDOB, strTCAgree;
			
			strDOB = (ddlDateYear.SelectedItem.Value + "/" + ddlDateMonth.SelectedItem.Value + "/" + ddlDateDay.SelectedItem.Value);
			
			if(cbAgree.Checked)
			{
				strTCAgree = "Terms and conditions accepted by parent";
			}
			else
			{
				strTCAgree = "Terms and conditions not accepted";
			}
			
			MailMessage objMail = new MailMessage();			
			objMail.To.Add(ConfigurationManager.AppSettings["gotriEntryEmailTo"] ?? "juniors@midsussextriclub.com");
			objMail.From = new MailAddress("noreply@midsussextriclub.com", "noreply@midsussextriclub.com");
			objMail.Subject = "GoTri! Junior Training Camp - Entry Received";
			
			objMail.IsBodyHtml = true;
			
			objMail.Body = "<h2>Young Person / Triathlete details</h2><p>" + txtFirstName.Text + " " + txtLastName.Text + "</p>" +
			"<p>Date of birth: " +  strDOB + ", Age: " + txtAge.Text + ", Gender: " + ddlGender.SelectedItem.Value + "</p>" +
			"<p>School attended: " +  txtSchool.Text + ", School year: " + txtSchoolYear.Text + "</p>" +
			"<p>Medical conditions: " +  txtMedical.Text + "</p>" +
			"<p>Email: " +  txtEmail.Text + "</p>" +
			"<h2>Notes</h2><p>Medical condition: " + txtMedical.Text + "</p>" +
			"<p>Dietary information: " + txtDiet.Text + "</p>" +
			"<p>Triathlon Experience: " + txtExperience.Text + "</p>" +
			"<h2>Parent details and emergency contact</h2><p>" + txtParentFirstName.Text + " " + txtParentLastName.Text + "</p>" +
			"<p>Address: " +  txtAddr1.Text + ", " + txtAddr2.Text + ", " + txtAddr3.Text + ", " + txtAddr4.Text + ", " + txtPostcode.Text + "</p>" +
			"<p>Phone: " +  txtPhone.Text + "</p>" +
			"<p>Name: " +  txtName.Text + "</p>" +
			"<p>" + strTCAgree + "</p>";
			
			GmailSmtpClient GmailSmtpClient = new GmailSmtpClient();
			GmailSmtpClient.Send(objMail);
				
			ViewState["ViewEntryForm"] = 1;
		}
		
		protected void SendMessagetoParent()
		{
		
			string 	strDOB, strTCAgree;
			
			strDOB = (ddlDateYear.SelectedItem.Value + "/" + ddlDateMonth.SelectedItem.Value + "/" + ddlDateDay.SelectedItem.Value);
			
			if(cbAgree.Checked)
			{
				strTCAgree = "Terms and conditions accepted by parent";
			}
			else
			{
				strTCAgree = "Terms and conditions not accepted";
			}
		
			MailMessage objMail = new MailMessage();
			objMail.To.Add(txtEmail.Text);
			objMail.From = new MailAddress("juniors@midsussextriclub.com");
			objMail.Subject = "GoTri! Junior Training Camp - Entry Confirmation";
			
			objMail.IsBodyHtml = true;
			
			objMail.Body = "<p>Thanks you for entering your child in the GoTri! Junior Training Camp</p>" +
			"<h2>Payment</h2>" +
			"<p></p>" +
			"<p>To confirm your child's place please send your payment using one of the following methods:</p>" +
			"<p><a href='http://www.midsussextriclub.com/mid-sussex-triathlon-juniors/gotri!/gotri!-payment.aspx'>Paypal - click here to visit the GoTri! payment page and pay using Paypal</a></p>" +
			"<p>Please pay online using BACS by putting GoTri! and then your child's name as a reference</p><p>Account Name: MSTC<br />Sort code: 20-49-76<br />Account Number: 43272192<br /><br />or make cheques payable to Mid Sussex Triathlon Club and send to:</p><p><br>MSTC GoTri!,<br /> 12 Brading Road, Brighton BN2 3PD</p><p>Please enter as soon as possible as places are limited.</p>" +
			"<h2>Young Person / Triathlete details</h2><p>" + txtFirstName.Text + " " + txtLastName.Text + "</p>" +
			"<p>Date of birth" +  strDOB + ", Age:" + txtAge.Text + ", Gender: " + ddlGender.SelectedItem.Value + "</p>" +
			"<p>School attended" +  txtSchool.Text + ", School year:" + txtSchoolYear.Text + "</p>" +
			"<p>Medical conditions" +  txtMedical.Text + "</p>" +
			"<p>Email" +  txtEmail.Text + "</p>" +
			"<h2>Notes</h2><p>Medical condition: " + txtMedical.Text + "</p>" +
			"<p>Dietary information: " + txtDiet.Text + "</p>" +
			"<p>Triathlon Experience: " + txtExperience.Text + "</p>" +
			"<h2>Parent details and emergency contact</h2><p>" + txtParentFirstName.Text + " " + txtParentLastName.Text + "</p>" +
			"<p>Address" +  txtAddr1.Text + ", " + txtAddr2.Text + ", " + txtAddr3.Text + ", " + txtAddr4.Text + ", " + txtPostcode.Text + "</p>" +
			"<p>Phone" +  txtPhone.Text + "</p>" +
			"<p>Name" +  txtName.Text + "</p>" +
			"<p>" + strTCAgree + "</p>";
			
			GmailSmtpClient GmailSmtpClient = new GmailSmtpClient();
			GmailSmtpClient.Send(objMail);
			
		}
		
		/*protected void getEntries()
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
				
				if(intEntries >= 340 && intSecretEntry != 1)
				{
					containerEntryClosed.Visible = true;
					containerEntry.Visible = false;
				}
				else
				{
					containerEntryClosed.Visible = false;
					containerEntry.Visible = true;
				}
			}
		}*/
	}
}	