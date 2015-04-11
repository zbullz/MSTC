using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using GoCardlessSdk;
using GoCardlessSdk.Connect;

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
		return redirectUrl;
	}

	public string CreateBill(BillRequest billRequest, string redirectUri, string cancelUri)
	{
		string requestUrl = new GoCardlessSdk.Connect.ConnectClient().NewBillUrl(billRequest, redirectUri, cancelUri);
		return requestUrl;
	}

	public void ConfirmBill(NameValueCollection confirmQueryStringCollection)
	{
		GoCardless.Connect.ConfirmResource(confirmQueryStringCollection);
	}
}