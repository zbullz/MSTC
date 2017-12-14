using System;
using System.Configuration;
using System.Web.Security;
using Mstc.Core.Domain;
using Mstc.Core.Dto;
using Mstc.Core.Providers;
using Newtonsoft.Json;

public partial class usercontrols_cFront_RegisterMemberComplete : System.Web.UI.UserControl
{
    protected bool IsRegistered;

    private SessionProvider _sessionProvider;
    private GoCardlessProvider _goCardlessProvider;
    private MemberProvider _memberProvider;
    EmailProvider _emailProvider;

    public usercontrols_cFront_RegisterMemberComplete()
    {
        _sessionProvider = new SessionProvider();
        _goCardlessProvider = new GoCardlessProvider();
        _memberProvider = new MemberProvider();
        _emailProvider = new EmailProvider();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
		if (IsPostBack == false)
		{
			RegistrationFullDetails registrationFullDetails = _sessionProvider.RegistrationFullDetails;
			if (registrationFullDetails == null || string.IsNullOrWhiteSpace(_sessionProvider.GoCardlessRedirectFlowId))
			{
				return; //Prevent duplicate registration
			}

		    string mandateId = _goCardlessProvider.CompleteRedirectRequest(_sessionProvider.GoCardlessRedirectFlowId,
		        _sessionProvider.SessionId);
            registrationFullDetails.RegistrationDetails.DirectDebitMandateId = mandateId;

            int cost = MembershipCostCalculator.Calculate(registrationFullDetails.MembershipOptions, DateTime.Now);
            string paymentDescription = _memberProvider.GetPaymentDescription(registrationFullDetails.MembershipOptions);

            var regDetails = registrationFullDetails.RegistrationDetails;
		    var paymentResponse = _goCardlessProvider.CreatePayment(regDetails.DirectDebitMandateId, regDetails.Email, cost,
		        paymentDescription);

		    IsRegistered = paymentResponse == PaymentResponseDto.Success;

            if (IsRegistered)
		    {
		        var member = _memberProvider.CreateMember(regDetails, new string[] {"Member"});
		        _memberProvider.UpdateMemberDetails(member, registrationFullDetails);

		        //Login the member
		        FormsAuthentication.SetAuthCookie(member.LoginName, true);
		        
		        string content = string.Format("<p>A new member has registered with the club</p><p>Member details: {0}</p>",
		            JsonConvert.SerializeObject(registrationFullDetails, Formatting.Indented));
		        var passwordObfuscator = new PasswordObfuscator();
		        content = passwordObfuscator.ObfuscateString(content);

		        _emailProvider.SendEmail(ConfigurationManager.AppSettings["newRegistrationEmailTo"], EmailProvider.SupportEmail,
		            "New MSTC member registration", content);

		        _sessionProvider.RegistrationFullDetails = null;
		        _sessionProvider.GoCardlessRedirectFlowId = null;
		    }
		    else
		    {
                string content = string.Format("<p>A new member has NOT been registered with the club</p><p>Member details: {0}</p>",
                    JsonConvert.SerializeObject(registrationFullDetails, Formatting.Indented));
                var passwordObfuscator = new PasswordObfuscator();
                content = passwordObfuscator.ObfuscateString(content);

                _emailProvider.SendEmail(EmailProvider.SupportEmail, EmailProvider.SupportEmail,
                    "MSTC member registration problem", content);
            }

		}
    }
}