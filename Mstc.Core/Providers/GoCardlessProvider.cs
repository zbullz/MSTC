using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using GoCardless;
using GoCardless.Exceptions;
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
			System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
			var environment = ConfigurationManager.AppSettings["gocardlessEnvironment"] == "Production"
		        ? GoCardlessClient.Environment.LIVE
		        : GoCardlessClient.Environment.SANDBOX;

		    _client = GoCardlessClient.Create(ConfigurationManager.AppSettings["gocardlessAccessToken"],
		        environment);
		}

	    public RedirectResponseDto CreateRedirectRequest(CustomerDto customer, string description, string sessionToken, string successUrl)
	    {
	        var redirectFlowResponse =  _client.RedirectFlows.CreateAsync(new RedirectFlowCreateRequest()
	        {
	            Description = description,
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

	    public PaymentResponseDto CreatePayment(string memberMandateId, string memberEmail, int costInPence, string description)
	    {
            int retries = 5;
            int tried = 0;
	        string idempotencyKey = Guid.NewGuid().ToString();
            return TryCreatePayment(idempotencyKey, memberMandateId, memberEmail, costInPence, description, retries, tried);
        }

	    public PaymentResponseDto TryCreatePayment(string idempotencyKey, string memberMandateId, string memberEmail,
	        int costInPence, string description, int retries, int tried)
	    {
	        if (tried >= retries)
	        {
	            return PaymentResponseDto.UnknownError;
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
	                Description = description,
	                IdempotencyKey = idempotencyKey
	            }).Result;

	            string message =
	                $"New CreatePayment request. memberEmail: {memberEmail}, memberMandateId: {memberMandateId}, " +
	                $"cost: {costInPence}, description: {description}";
	            Log.Add(LogTypes.Custom, -1, message);

	            return PaymentResponseDto.Success;
	        }
	        catch (Exception ex)
	        {
	            Log.Add(LogTypes.Error, -1,
	                string.Format(
	                    $"Unable to CreatePayment for memberEmail: {memberEmail}, memberMandateId: {memberMandateId},, exception: {0}",
	                    ex));

	            var exception = ex.InnerException as ApiException;
	            if (exception != null)
	            {
	                var mandateErrors = new List<string>() {"Mandate is failed, cancelled or expired", "Mandate not found"};
	                if (mandateErrors.Contains(exception.ApiErrorResponse.Error.Message))
	                {
	                    return PaymentResponseDto.MandateError;
	                }
	            }

	            return TryCreatePayment(idempotencyKey, memberMandateId, memberEmail, costInPence, description, retries,
	                tried);
	        }
	    }
	}
}