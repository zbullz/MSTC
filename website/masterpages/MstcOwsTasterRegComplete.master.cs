using System;
using System.Configuration;
using System.Web.Security;
using Mstc.Core.Domain;
using Mstc.Core.Providers;
using Newtonsoft.Json;
using Mstc.Core.Dto;
using cFront.Umbraco;

public partial class masterpages_MstcOwsTasterRegComplete : System.Web.UI.MasterPage
{
    protected bool IsRegistered;

    private SessionProvider _sessionProvider;
    private GoCardlessProvider _goCardlessProvider;
    private MemberProvider _memberProvider;
    EmailProvider _emailProvider;

    public masterpages_MstcOwsTasterRegComplete()
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

            int costInPence = MembershipCostCalculator.OwsTasterCost;
            string paymentDescription = "Open Water Swim Taster Session";

            var regDetails = registrationFullDetails.RegistrationDetails;
            var paymentResponse = _goCardlessProvider.CreatePayment(regDetails.DirectDebitMandateId, regDetails.Email, costInPence,
                paymentDescription);

            IsRegistered = paymentResponse == PaymentResponseDto.Success;

            if (IsRegistered)
            {
                var member = _memberProvider.CreateMember(regDetails, new string[] { MSTCRoles.Guest });
                _memberProvider.UpdateMemberDetails(member, registrationFullDetails);

                var currentmemdata = MemberHelper.Get(member);
                currentmemdata[MemberProperty.SwimCreditsBought] = costInPence / 100m;
                MemberHelper.Update(member, currentmemdata);

                litCost.Text = (costInPence / 100m).ToString("N2");

                //Login the member
                FormsAuthentication.SetAuthCookie(member.LoginName, true);

                //Email details to OWS admin
                var emailProvider = new EmailProvider();
                string content = string.Format("<p>A new OWS Taster guest has registered with the club.</p>" +
                                               "<p>Please contact them to arrange a taster session</p>" +
                                               "<p>Guest details: {0}</p>",
                    JsonConvert.SerializeObject(registrationFullDetails, Formatting.Indented));
                var passwordObfuscator = new PasswordObfuscator();
                content = passwordObfuscator.ObfuscateString(content);

                _emailProvider.SendEmail(ConfigurationManager.AppSettings["owsEmailTo"], EmailProvider.SupportEmail, "New OWS Taster registration", content);

                _sessionProvider.RegistrationFullDetails = null;
                _sessionProvider.GoCardlessRedirectFlowId = null;
            }
            else
            {
                string content = string.Format("<p>A new OWS Taster session has NOT been registered with the club</p><p>Guest details: {0}</p>",
                    JsonConvert.SerializeObject(registrationFullDetails, Formatting.Indented));
                var passwordObfuscator = new PasswordObfuscator();
                content = passwordObfuscator.ObfuscateString(content);

                _emailProvider.SendEmail(EmailProvider.SupportEmail, EmailProvider.SupportEmail,
                    "MSTC OWS Taster registration problem", content);
            }

        }
    }




}
