using System;
using System.Collections.Generic;
using System.Configuration;
using GoCardless;
using GoCardless.Services;
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

	    public RedirectResponseDto CreateRedirectRequest(CustomerDto customer, string sessionToken, string successUrl)
	    {
	        var redirectFlowResponse =  _client.RedirectFlows.CreateAsync(new RedirectFlowCreateRequest()
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
	        }).Result;

            var redirectFlow = redirectFlowResponse.RedirectFlow;
            return new RedirectResponseDto()
            {
                Id = redirectFlow.Id,
                RedirectUrl = redirectFlow.RedirectUrl
            };
        }

	    public string CompleteRedirectRequest(string requestId, string sessionToken)
	    {
	        var redirectFlowResponse = _client.RedirectFlows
	            .CompleteAsync(requestId,
	                new RedirectFlowCompleteRequest()
	                {
	                    SessionToken = sessionToken
	                }
	            ).Result;

	        return redirectFlowResponse.RedirectFlow.Links.Mandate;
	    }

	    public bool CreatePayment(string memberMandateId, string memberEmail, int costInPence, string description)
	    {
            int retries = 5;
            int tried = 0;
	        string idempotencyKey = Guid.NewGuid().ToString();
            return TryCreatePayment(idempotencyKey, memberMandateId, memberEmail, costInPence, description, retries, tried);
        }

        public bool TryCreatePayment(string idempotencyKey, string memberMandateId, string memberEmail, int costInPence, string description,int retries, int tried)
        {
            if (tried >= retries)
            {
                return false;
            }

            try
            {
                tried++;

                var createResponse = _client.Payments.CreateAsync(new PaymentCreateRequest()
                {
                    Amount = costInPence,
                    Currency = PaymentCreateRequest.PaymentCurrency.GBP,
                    Links = new PaymentCreateRequest.PaymentLinks()
                    {
                        Mandate = memberMandateId,
                    },
                    Metadata = new Dictionary<string, string>()
                    {
                        {"description", description}
                    },
                    IdempotencyKey = idempotencyKey
                }).Result;

                string message = $"New CreatePayment request. memberEmail: {memberEmail}, memberMandateId: {memberMandateId}, " +
                            $"cost: {costInPence}, description: {description}";
                Log.Add(LogTypes.Custom, -1, message);

                return true;
            }
            catch (Exception ex)
            {
                Log.Add(LogTypes.Error, -1, string.Format($"Unable to CreatePayment for memberEmail: {memberEmail}, memberMandateId: {memberMandateId},, exception: {0}", ex));
                return TryCreatePayment(idempotencyKey, memberMandateId, memberEmail, costInPence, description, retries, tried);
            }
        }
    }
}