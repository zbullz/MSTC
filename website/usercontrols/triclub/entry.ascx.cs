using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net.Mail;
using System.Collections;
using Mstc.Core.configuration;

public partial class usercontrols_triclub_entry : UserControl
{
	/*
	protected TextBox txtFirstName, txtLastName, txtAddr1, txtAddr2, txtAddr3, txtAddr4, txtPostcode, txtPhone, txtEmail, txtClub, txtBTF;
	protected DropDownList ddlDateDay, ddlDateMonth, ddlDateYear, ddlGender, ddlSwimMins, ddlSwimSecs;
	protected PlaceHolder containerEntry, containerEntryFull;
	protected Label lblTerms, lblCaptcha;
	protected RadioButton rbFull, rbAquabike;
	protected CheckBox cbAgree;	
	protected Boolean boolSecretEntry;
	protected int intViewState, intEntries, intEventType;
	*/

	protected int intSecretEntry;

	void Page_Load()
	{
		intSecretEntry = Convert.ToInt32(Request.QueryString["secretsteve"]);

		getPageSettings();
		getEntries();
	}

	protected void getPageSettings()
	{
		if (!IsPostBack)
		{
			ArrayList monthArray = new ArrayList();
			ArrayList dayArray = new ArrayList();
			ArrayList yearArray = new ArrayList();

			int i;

			for (i = 1; i < 13; i++)
				monthArray.Add(i);
			for (i = 1; i < 32; i++)
				dayArray.Add(i);
			for (i = (Convert.ToInt32(DateTime.Now.Year - 90)); i < (Convert.ToInt32(DateTime.Now.Year - 15)); i++)
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
        if (cbAgree.Checked && Page.IsValid && 
            String.IsNullOrEmpty(txtEmailConfirm.Text)) //This is a simple spam bot prevention field, people won't see it see won't fill it out
		{
			string strDOB, strClub;

			strDOB = (ddlDateYear.SelectedItem.Value + "/" + ddlDateMonth.SelectedItem.Value + "/" + ddlDateDay.SelectedItem.Value);
			strClub = txtClub.Text;

			if (strClub == "")
			{
				strClub = "Unattached";
			}

			int intEventType = 1;
			if (rbFull.Checked)
			{
				intEventType = 1;
			}
			else if (rbAquabike.Checked)
			{
				intEventType = 2;
			}

			using (SqlConnection objConn = new SqlConnection(ConfigurationManager.AppSettings["triclubDSN"]))
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

            SendEntrantMessage();
            SendMessage();
		    
            Response.Redirect("race-entry/race-entry-payment.aspx");
		}
		else
		{
			if (!cbAgree.Checked)
			{
				lblTerms.Text = "<span class='mst_fielderror_terms'><span class='ERROR'>Error!</span>You must agree to the race terms &#038; conditions to proceed</span>";
				lblTerms.Visible = true;
			}
		}
	}

	protected void SendMessage()
	{
		MailMessage objMail = new MailMessage();
		objMail.To.Add(ConfigurationManager.AppSettings["midSussexTriEntryEmailTo"] ?? "sales@midsussextriclub.com");
		objMail.From = new MailAddress("noreply@midsussextriclub.com");
		objMail.Subject = "Mid Sussex Triathlon - New Entry";

		objMail.IsBodyHtml = true;

		objMail.Body = "<p>" + txtFirstName.Text + " " + txtLastName.Text + " has entered the event.</p><p><a href='http://www.midsussextriclub.com' target='_blank'>Go to Mid Sussex Tri Club site</a></p>";

		GmailSmtpClient GmailSmtpClient = new GmailSmtpClient();
		GmailSmtpClient.Send(objMail);

		ViewState["ViewEntryForm"] = 1;
	}

    protected void SendEntrantMessage()
    {
        string content = "<p>Congratulations! You are now registered for the Mid Sussex Triathlon/Aquabike. " +
                         "Your entry will only be confirmed on the website when we have received your payment " +
                         "and your name has been entered into the '<a href='http://www.midsussextriclub.com/the-mid-sussex-triathlon/enter-race/entries-so-far.aspx'>Entries so far</a>' list. " +
                         "Please allow up to 48 hours for this. Please familiarise yourself with the <a href='http://www.midsussextriclub.com/the-mid-sussex-triathlon/race-info/race-instructions.aspx'>race instructions</a>.</p>" +
                         "<p>We do not post anything out so you will pick your number up at registration on the Saturday from 17:00 until 18:30 " +
                         "or on race day before 06:45 when registration closes.</p>" +
                         "<p>We are running a <a href='http://www.midsussextriclub.com/the-mid-sussex-triathlon/course-details/course-familiarisation-day.aspx'>course familiarisation day</a> of the cycle and run courses. " +
                         "Please <a href='http://www.midsussextriclub.com/the-mid-sussex-triathlon/course-details/course-familiarisation-day.aspx'>check the website for details</a> and let us know if you want to attend.</p>" +
                         "<p>A member of our club has developed an iPhone app to aid in triathlon training. Its free so why not give it a go?<br/>" +
                         "The app can be downloaded from the iTunes store here: <a href='https://itunes.apple.com/us/app/mytriathlonbuddy/id1182728584?ls=1&mt=8'>Download MyTriathlonBuddy</a></p>" +
                         "<p>As a race entrant you can also request a promo code to unlock the apps long distance training plans. Please send us an email if you are interested.</p>" +
                         "<p>Thanks</p>" +
                         "<p>Steve Mac<br/>Event Director</p>";

        var mailMessage = new MailMessage();
        mailMessage.To.Add(txtEmail.Text);
        var fromAdd = new MailAddress("info@midsussextriclub.com");
        mailMessage.From = fromAdd;
        mailMessage.ReplyToList.Add(fromAdd);
        mailMessage.Subject = "Mid Sussex Triathlon - Entry Received";
        mailMessage.IsBodyHtml = true;
        mailMessage.Body = content;

        var GmailSmtpClient = new GmailSmtpClient();
        GmailSmtpClient.Send(mailMessage);
    }

	protected void getEntries()
	{
		using (SqlConnection objConn = new SqlConnection(ConfigurationManager.AppSettings["triclubDSN"]))
		{
			objConn.Open();

			SqlCommand objCmd = objConn.CreateCommand();
			objCmd.CommandText =
@"
select count(*) AS TotalEntries from Entries WHERE Accept=1
";

			int intEntries;
			using (SqlDataReader objRdr = objCmd.ExecuteReader())
			{
				objRdr.Read();

				intEntries = objRdr["TotalEntries"] is DBNull ? 0 : Convert.ToInt32(objRdr["TotalEntries"]);
			}

			if (intEntries >= 350 && intSecretEntry != 1)
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