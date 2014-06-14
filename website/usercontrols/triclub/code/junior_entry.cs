using System;
using System.Data;
using MySql.Data; 
using MySql.Data.MySqlClient; 
using System.Web;
using System.IO;
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
								txtAge, txtSchool, txtSchoolYear, txtMedical, txtYPEmail, txtMedicalNote, txtDiet, txtExperience, 
								txtParentFirstName, txtParentLastName, 
								txtAddr1, txtAddr2, txtAddr3, txtAddr4, txtPostcode, txtPhone, txtEmail, txtName;
		protected DropDownList 	ddlDateDay, ddlDateMonth, ddlDateYear, ddlGender, ddlSwimMins, ddlSwimSecs;
		protected PlaceHolder 	containerEntry, containerEntryClosed;
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
				
		protected void submitEntry(object s, EventArgs e)
		{
			if(cbAgree.Checked && Page.IsValid)
			{
				SendMessage();
				SendMessagetoParent();
				Response.Redirect("/mid-sussex-triathlon-juniors/tri-hub/tri-hub-payment.aspx");
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
			objMail.To.Add(ConfigurationManager.AppSettings["juniorEntryEmailTo"] ?? "juniors@midsussextriclub.com");
			objMail.From = new MailAddress("noreply@midsussextriclub.com");
			objMail.Subject = "TriHub Junior Training - Entry Received";
			
			objMail.IsBodyHtml = true;
			
			objMail.Body = "<h2>Young Person / Triathlete details</h2><p>" + txtFirstName.Text + " " + txtLastName.Text + "</p>" +
			"<p>Date of birth: " +  strDOB + ", Age: " + txtAge.Text + ", Gender: " + ddlGender.SelectedItem.Value + "</p>" +
			"<p>School attended: " +  txtSchool.Text + ", School year: " + txtSchoolYear.Text + "</p>" +
			"<p>Medical conditions: " +  txtMedical.Text + "</p>" +
			"<p>Email: " +  txtEmail.Text + "</p>" +
			"<h2>Notes</h2><p>Medical condition: " + txtMedicalNote.Text + "</p>" +
			"<p>Dietary information: " + txtDiet.Text + "</p>" +
			"<p>Triathlon Experience: " + txtExperience.Text + "</p>" +
			"<h2>Parent details and emergency contact</h2><p>" + txtParentFirstName.Text + " " + txtParentLastName.Text + "</p>" +
			"<p>Address: " +  txtAddr1.Text + ", " + txtAddr2.Text + ", " + txtAddr3.Text + ", " + txtAddr4.Text + ", " + txtPostcode.Text + "</p>" +
			"<p>Phone: " +  txtPhone.Text + "</p>" +
			"<p>Name: " +  txtName.Text + "</p>" +
			"<p>" + strTCAgree + "</p>";
			
			GmailSmtpClient smtpClient = new GmailSmtpClient();
			smtpClient.Send(objMail);
				
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
			objMail.Subject = "Tri Hub - Entry Confirmation";
			
			objMail.IsBodyHtml = true;
			
			objMail.Body = "<p>Thanks you for entering your child in the Tri Hub</p>" +
			"<h2>Payment</h2>" +
			"<p>Tri Hub costs&nbsp;£50 per young person, £80 for two or £100 for three.</p>" +
			"<p>To confirm your child's place please send your payment using one of the following methods:</p>" +
			"<p><a href='http://www.midsussextriclub.com/mid-sussex-triathlon-juniors/tri-hub/tri-hub-payment.aspx'>Paypal - click here to visit the TriHub payment page and pay using Paypal</a></p>" +
			"<p>Please pay online using BACS by putting TriHub and then your child's name as a reference</p><p>Account Name: MSTC<br />Sort code: 20-49-76<br />Account Number: 43272192<br /><br />or make cheques payable to Mid Sussex Triathlon Club and send to:</p><p><br>MSTC TriHub 12 ,Brading Road, Brighton BN2 3PD</p><p>Please enter as soon as possible as places are limited.<br /><br />It is parents/carers responsibility to enter the participants in to the East Grinstead Kidstri on the 18th May and to ensure that they get there on time. Follow the link below and enter the appropriate TS race;</p><p><a href='http://entryapp.co.uk/2014/hedgehogtri/child/'>East Grinstead Kidstri website</a></p>" +
			"<h2>Young Person / Triathlete details</h2><p>" + txtFirstName.Text + " " + txtLastName.Text + "</p>" +
			"<p>Date of birth" +  strDOB + ", Age:" + txtAge.Text + ", Gender: " + ddlGender.SelectedItem.Value + "</p>" +
			"<p>School attended" +  txtSchool.Text + ", School year:" + txtSchoolYear.Text + "</p>" +
			"<p>Medical conditions" +  txtMedical.Text + "</p>" +
			"<p>Email" +  txtEmail.Text + "</p>" +
			"<h2>Notes</h2><p>Medical condition: " + txtMedicalNote.Text + "</p>" +
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
	}
}	