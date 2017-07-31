﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco.Membership;
using GoCardlessSdk;
using GoCardlessSdk.Connect;
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
			new ListItem("Sprint Individual - &pound;23", ((int) PaymentStates.E00TRISI205C).ToString()),
			new ListItem("Olympic Individual - &pound;23", ((int) PaymentStates.E00TRIOI201C).ToString()),
            new ListItem("Olympic Relay - &pound;13", ((int) PaymentStates.E00TRIOR202C).ToString()),
			new ListItem("Middle distance Individual - &pound;23", ((int) PaymentStates.E00TRIMI203C).ToString()),
			new ListItem("Middle distance Relay - &pound;13", ((int) PaymentStates.E00TRIMR204C).ToString()),
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

		decimal cost = 10m;
		string memberEmail = currentmemdata[MemberProperty.Email] as string;
		RedirectToGocardless(memberEmail, cost, "MSTC Duathlon Entry", "Mid Sussex Tri Club Duathlon Entry", PaymentStates.E00D101C);
		//RedirectToCompletePage(PaymentStates.E00D101C.ToString()); //Can use this for local testing
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

		decimal cost = (entryType == PaymentStates.E00TRIOI201C || entryType == PaymentStates.E00TRIMI203C || entryType == PaymentStates.E00TRISI205C) ? 23m : 13m;
		if (string.IsNullOrEmpty(tbTriFestBTFNumber.Text) == false)
		{
			cost = cost - 3;
			currentmemdata[MemberProperty.BTFNumber] = tbTriFestBTFNumber.Text;
			MemberHelper.Update(currentmemdata);
		}

		string memberEmail = currentmemdata[MemberProperty.Email] as string;
		string paymentDescription = string.Format("{0}", entryType.GetAttributeOfType<DescriptionAttribute>().Description);
		RedirectToGocardless(memberEmail, cost, "MSTC Tri Fest", paymentDescription, entryType);
		//RedirectToCompletePage(entryType.ToString()); //Can use this for local testing
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

		decimal cost = 20m;
		string memberEmail = currentmemdata[MemberProperty.Email] as string;
		string paymentDescription = string.Format("{0}", entryType.GetAttributeOfType<DescriptionAttribute>().Description);
		RedirectToGocardless(memberEmail, cost, "MSTC 5-3-1 Charity Swim", paymentDescription, entryType);
		//RedirectToCompletePage(entryType.ToString()); //Can use this for local testing
	}

	private void RedirectToCompletePage(string state)
	{
		string rootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
			Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port);
		string redirectUrl = string.Format("{0}/payment-complete?state={1}", rootUrl, state);
		Response.Redirect(redirectUrl);
	}

	private void RedirectToGocardless(string memberEmail, decimal cost, string name, string description, PaymentStates paymentState)
	{
		var goCardlessProvider = new GoCardlessProvider();
		var redirectUrl = goCardlessProvider.CreateSimpleBill(memberEmail, cost, name, description, paymentState, Request.Url);
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
