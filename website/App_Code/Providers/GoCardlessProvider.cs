using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using GoCardlessSdk;
using GoCardlessSdk.Connect;
using umbraco.BusinessLogic;

public class GoCardlessProvider
{
	public GoCardlessProvider()
	{
		GoCardless.Environment = (GoCardless.Environments)
			Enum.Parse(typeof (GoCardless.Environments), ConfigurationManager.AppSettings["gocardlessEnvironment"]);
		GoCardless.AccountDetails = new AccountDetails
		{
			AppId = ConfigurationManager.AppSettings["gocardlessAppId"],
			AppSecret = ConfigurationManager.AppSettings["gocardlessAppSecret"], 
			Token = ConfigurationManager.AppSettings["gocardlessToken"]
		};
	}

	public string MerchantId
	{
		get { return ConfigurationManager.AppSettings["gocardlessMerchantId"]; }
	}

	public string CreateSimpleBill(string memberEmail, decimal cost, string name, string description, PaymentStates paymentState, Uri requestUri)
	{
		Log.Add(LogTypes.Custom, -1,
			string.Format(
				"New CreateSimpleBill request. memberEmail: {0}, cost: {1}, name: {2}, description: {3}, paymentState: {4}",
				memberEmail, cost, name, description, paymentState.ToString()));

		var goCardlessProvider = new GoCardlessProvider();
		var billRequest = new BillRequest(goCardlessProvider.MerchantId, cost)
		{
			Name = name,
			Description = description,
			User = new UserRequest()
			{
				Email = memberEmail,
			}
		};

		//Could wrap this in a provider
		string rootUrl = string.Format("{0}://{1}{2}", requestUri.Scheme, requestUri.Host,
			requestUri.Port == 80 ? string.Empty : ":" + requestUri.Port);
		string redirectUrl = string.Format("{0}/payment-complete", rootUrl);
		string cancelUrl = string.Format("{0}", rootUrl);

		string requestUrl = new GoCardlessSdk.Connect.ConnectClient().NewBillUrl(billRequest, redirectUrl, cancelUrl, paymentState.ToString());
		return requestUrl;
	}

	public string CreateBill(BillRequest billRequest, string redirectUri, string cancelUri)
	{
		string requestUrl = new GoCardlessSdk.Connect.ConnectClient().NewBillUrl(billRequest, redirectUri, cancelUri);
		return requestUrl;
	}

	public void ConfirmBill(NameValueCollection confirmQueryStringCollection)
	{
        //There have been some connectivity issues with the confirm bill request to go cardless so have put 5 retries in to guard against it
	    int retries = 5;
	    int tried = 0;
	    TryConfirmBill(confirmQueryStringCollection, retries, tried);
	}

    public void TryConfirmBill(NameValueCollection confirmQueryStringCollection, int retries, int tried)
    {
        if (tried >= retries)
        {
            return;
        }

        try
        {
            tried++;
	        GoCardless.Connect.ConfirmResource(confirmQueryStringCollection);
	    }
	    catch (Exception ex)
	    {
	        Log.Add(LogTypes.Error, -1, string.Format("Unable to ConfirmBill, exception: {0}", ex.ToString()));
	        TryConfirmBill(confirmQueryStringCollection, retries, tried);
	    }
    }
}