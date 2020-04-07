<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MembershipOptions.ascx.cs" Inherits="usercontrols_cFront_MembershipOptions" %>
<%@ Import Namespace="Mstc.Core.Providers" %>

<div class="member-options">
        <div class="form-group">
            <h3 class="col-sm-12">Membership options</h3>
        </div>
        <div class="form-group">
            <label for="Name" class="col-sm-2 control-label"><b>Membership*</b></label>
            <div class="col-sm-10">
                <asp:RadioButtonList ID="membershipType" runat="server" RepeatLayout="Flow" CssClass="radio"></asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="requiredMembershipType" runat="server" ErrorMessage="Please select your membership type" 
                    ControlToValidate="membershipType" CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
            <% if (MembershipCostCalculator.DiscountedMonths.Contains(DateTime.Now.Month)) { %>
                <span class="help-block alert-success">Good news! The 50% second half season discount has been applied!</span>
            <% } %>
                <span class="help-block alert-warning">Youth members parents must complete the <a href="http://midsussextriclub.com/media/63667/Consent%20form%20Feb%202017.pdf">consent form</a>.</span>
            </div>
        </div>
        <div class="form-group">
            <label for="contact-email" class="col-sm-2 control-label"><b>Optional extras</b></label>
            <div class="col-sm-10">
                <asp:CheckBoxList ID="extras" runat="server" CssClass="checkbox"></asp:CheckBoxList>
                <span class="help-block alert-info">*If you choose England Athletics Membership we will provide England Athletics with your personal data which they will use to enable access to an online portal for you (called myAthletics). England Athletics will contact you to invite you to sign in and update your MyAthletics portal which allows you, amongst other things, to set and amend your privacy settings. If you have any questions about the continuing privacy of your personal data when it is shared with England Athletics, please contact dataprotection@englandathletics.org</span>
            </div>

        </div>
        <div class="form-group">
            <label for="contact-message" class="col-sm-2 control-label"><b>Open water swimming indemnity waiver*</b></label>
            <div class="col-sm-10">
                <p><a href="/training-zone/open-water-swim-sessions.aspx" target="_blank">Click here to review the OWS Safety Video and Guidelines</a></p>
                <p><a href="http://midsussextriclub.com/media/47452/MSTCIndemnityWaiver.pdf" target="_blank">Click here to view the Open Water Swimming Indemnity Document</a></p>
                <asp:RadioButtonList ID="indemnityOptions" runat="server" RepeatLayout="Flow" CssClass="radio"></asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="requiredIndemnityOptions" runat="server" ErrorMessage="Please select your open water indemnity waiver response" 
                    ControlToValidate="indemnityOptions" CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label"><b>Volunteering agreement*</b></label>
            <div class="col-sm-10">
                <asp:CheckBox ID="volunteering" runat="server" CssClass="checkbox" 
                    Text="I agree to give my time to assist in marshalling duties this year at either the Mid Sussex Triathlon or club events." />
                <asp:CustomValidator runat="server" ID="requiredVolunteering" OnServerValidate="CheckBoxRequired_ServerValidate" CssClass="help-block alert-danger" ErrorMessage="Please accept the volunteering agreement to proceed" />   
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label"><b>Personal data</b></label>
            <div class="col-sm-10">
                <p class="form-control-static">In becoming a member of Mid Sussex Triathlon Club ("the Club"), the Club will collect certain 
                                                personal information about you. This data will be used for the purposes of administration of Club
                                                membership, Club management, sending communications to you about the Club, and administration
                                                of training sessions and club competitions. Further details can be found in the Club's <a href="https://midsussextriclub.com/media/69408/mstcprivacypolicymay2018.pdf" target="_blank">privacy
                                                statement and policy</a>.</p>
            </div>
        </div>
    </div>
