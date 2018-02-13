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
                <span class="help-block alert-info">*If you choose England Athletics Membership we will provide England Athletics with your personal data which they will use to enable access to an online portal for you. England Athletics will contact you to invite you to sign in and update your MyAthletics portal which allows you, amongst other things, to set and amend your privacy settings.</span>
            </div>

        </div>
        <div class="form-group">
            <label for="contact-message" class="col-sm-2 control-label"><b>Open water swimming indemnity waiver*</b></label>
            <div class="col-sm-10">
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
                <asp:CustomValidator runat="server" ID="requiredVolunteering" OnServerValidate="CheckBoxRequired_ServerValidate" CssClass="help-block alert-danger">
                    Please accept the volunteering agreement to proceed</asp:CustomValidator>
            </div>
            
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label"><b>Personal data agreement*</b></label>
            <div class="col-sm-10">
                <asp:CheckBox ID="personalData" runat="server" CssClass="checkbox" 
                    Text="I agree to allow the club to store my personal data." />
                <asp:CustomValidator runat="server" ID="requiredPersonalData" OnServerValidate="PersonalDataRequired_ServerValidate" CssClass="help-block alert-danger">
                    Please accept the personal data agreement to proceed</asp:CustomValidator>
            </div>
        </div>
    </div>
