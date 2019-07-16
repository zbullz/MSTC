using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using cFront.Umbraco.Membership;
using Mstc.Core.Domain;
using Mstc.Core.Providers;

public partial class masterpages_MstcClubEventEntry : System.Web.UI.MasterPage
{
	protected SessionProvider _sessionProvider;
	protected SessionProvider SessionProvider
	{
		get
		{
			if (_sessionProvider == null)
			{
				_sessionProvider = new SessionProvider();
			}
			return _sessionProvider;
		}
	}

    protected void Page_Load(object sender, EventArgs e)
    {
	    if (IsPostBack)
		    return;

	    string clubEvent = Request.QueryString["e"];

	    switch (clubEvent)
	    {
		    case "d":
		    {
			    duathlonEntryForm.Visible = true;
			    break;
		    }
			case "t":
		    {
			    trifestEntryForm.Visible = true;
			    break;
		    }
			case "s":
		    {
			    charitySwimEntryForm.Visible = true;
			    break;
		    }
	    }

	    BindControls();
    }

	private void BindControls()
	{
		var eventTypes = new List<ListItem>()
		{
			new ListItem("Triathlon - Sprint Individual", ((int) PaymentStates.E00TRISI205C).ToString()),
			new ListItem("Triathlon - Sprint Relay", ((int) PaymentStates.E00TRISR206C).ToString()),
            new ListItem("Triathlon - Olympic Individual", ((int) PaymentStates.E00TRIOI201C).ToString()),
            new ListItem("Triathlon - Olympic Relay", ((int) PaymentStates.E00TRIOR202C).ToString()),
			new ListItem("Triathlon - Middle distance Individual", ((int) PaymentStates.E00TRIMI203C).ToString()),
			new ListItem("Triathlon - Middle distance Relay", ((int) PaymentStates.E00TRIMR204C).ToString()),
            new ListItem("Aquathlon - Sprint Individual", ((int) PaymentStates.E00ASI211C).ToString()),
            new ListItem("Aquathlon - Sprint Relay", ((int) PaymentStates.E00ASR212C).ToString()),
            new ListItem("Aquathlon - Olympic Individual", ((int) PaymentStates.E00AOI207C).ToString()),
            new ListItem("Aquathlon - Olympic Relay", ((int) PaymentStates.E00AOR208C).ToString()),
            new ListItem("Aquathlon - Middle distance Individual", ((int) PaymentStates.E00AMI209C).ToString()),
            new ListItem("Aquathlon - Middle distance Relay", ((int) PaymentStates.E00AMR210C).ToString()),
        };
		triFestEventType.Items.AddRange(eventTypes.ToArray());

        var swimEventTypes = new List<ListItem>()
		{
			new ListItem("Charity Swim - 1km", ((int) PaymentStates.E00S1KM301C).ToString()),
			new ListItem("Charity Swim - 3km", ((int) PaymentStates.E00S3KM302C).ToString()),
			new ListItem("Charity Swim - 5km", ((int) PaymentStates.E00S5KM303C).ToString()),
			new ListItem("Charity Swim - 1km and 3km", ((int) PaymentStates.E00S1KM3KM304C).ToString()),
			new ListItem("Charity Swim - 1km and 5km", ((int) PaymentStates.E00S1KM5KM305C).ToString()),
			new ListItem("Charity Swim - 3km and 5km", ((int) PaymentStates.E00S3KM5KM306C).ToString()),
			new ListItem("Charity Swim - 1km, 3km and 5km", ((int) PaymentStates.E00S1KM3KM5KM307C).ToString())
		};
		charitySwimEventType.Items.AddRange(swimEventTypes.ToArray());
	}

	protected void DualthonEnter_OnClick(object sender, EventArgs e)
	{
		if (Page.IsValid == false)
		{
			return;
		}

		IDictionary<String, object> currentmemdata = MemberHelper.Get();
		if (currentmemdata == null)
		{
			return; //Ensure the form is behind a login form
		}

		SessionProvider.CanProcessPaymentCompletion = true;
		RedirectToPaymentPages(currentmemdata, PaymentStates.E00D101C.ToString()); 
	}

	protected void TriFestEnter_OnClick(object sender, EventArgs e)
	{
		if (Page.IsValid == false)
		{
			return;
		}

		IDictionary<String, object> currentmemdata = MemberHelper.Get();
		if (currentmemdata == null)
		{
			return; //Ensure the form is behind a login form
		}

		SessionProvider.CanProcessPaymentCompletion = true;

		PaymentStates entryType = (PaymentStates)Enum.Parse(typeof(PaymentStates), triFestEventType.SelectedValue);
		
		if (string.IsNullOrEmpty(tbTriFestBTFNumber.Text) == false)
		{
			currentmemdata[MemberProperty.BTFNumber] = tbTriFestBTFNumber.Text;	
		}

        currentmemdata[MemberProperty.RelayTeamName] = tbRelayTeamName.Text;
        MemberHelper.Update(currentmemdata);

        RedirectToPaymentPages(currentmemdata, entryType.ToString());
	}

	protected void CharitySwimEnter_OnClick(object sender, EventArgs e)
	{
		if (Page.IsValid == false)
		{
			return;
		}

		IDictionary<String, object> currentmemdata = MemberHelper.Get();
		if (currentmemdata == null)
		{
			return; //Ensure the form is behind a login form
		}

		SessionProvider.CanProcessPaymentCompletion = true;

		PaymentStates entryType = (PaymentStates)Enum.Parse(typeof(PaymentStates), charitySwimEventType.SelectedValue);
		
		RedirectToPaymentPages(currentmemdata, entryType.ToString()); 
	}

	private void RedirectToPaymentPages(IDictionary<String, object> currentmemdata, string state)
	{
	    _sessionProvider.MandateSuccessPage = "payment-confirmation";

        bool hasMandate = string.IsNullOrWhiteSpace(currentmemdata[MemberProperty.directDebitMandateId] as string) == false;
	    string page = hasMandate ? "payment-confirmation" : "mandate-request";

        string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
			Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
		string redirectUrl = string.Format("{0}/{1}?state={2}", rootUrl, page, state);
		Response.Redirect(redirectUrl);
	}

	protected void DuathlonTerms_Validate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = checkBoxDuathlonTerms.Checked;
	}

	protected void TriFestTerms_Validate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = checkBoxTriFestTerms.Checked;
	}

	protected void CharitySwimTerms_Validate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = charitySwimTerms.Checked;
	}
}
