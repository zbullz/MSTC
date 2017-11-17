using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading.Tasks;
using GoCardless;
using GoCardless.Services;
using Mstc.Core.Domain;
using Mstc.Core.Dto;
using umbraco.BusinessLogic;

namespace Mstc.Core.Providers
{
	public class GoCardlessProvider
	{
	    private GoCardlessClient _client;

        public GoCardlessProvider()
		{
		    var environment = ConfigurationManager.AppSettings["gocardlessEnvironment"] == "Production"
		        ? GoCardlessClient.Environment.LIVE
		        : GoCardlessClient.Environment.SANDBOX;

		    _client = GoCardlessClient.Create(ConfigurationManager.AppSettings["gocardlessAccessToken"],
		        environment);
		}

		public string MerchantId
		{
			get { return ConfigurationManager.AppSettings["gocardlessMerchantId"]; }
		}

	    public async Task<RedirectResponseDto> CreateRedirectRequest(CustomerDto customer, string sessionToken, string successUrl)
	    {
	        var redirectFlowResponse = await _client.RedirectFlows.CreateAsync(new RedirectFlowCreateRequest()
	        {
	            Description = "MSTC Member",
	            SessionToken = sessionToken,
	            SuccessRedirectUrl = successUrl,
	            // Optionally, prefill customer details on the payment page
	            PrefilledCustomer = new RedirectFlowCreateRequest.RedirectFlowPrefilledCustomer()
	            {
	                GivenName = customer.GivenName,
	                FamilyName = customer.FamilyName,
	                Email = customer.Email,
	                AddressLine1 = customer.AddressLine1,
	                City = customer.City,
	                PostalCode = customer.PostalCode
	            }
	        });

            var redirectFlow = redirectFlowResponse.RedirectFlow;
            return new RedirectResponseDto()
            {
                Id = redirectFlow.Id,
                RedirectUrl = redirectFlow.RedirectUrl
            };
        }

        /*
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
        */
	}
}