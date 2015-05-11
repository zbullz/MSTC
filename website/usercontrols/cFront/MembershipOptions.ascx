<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MembershipOptions.ascx.cs" Inherits="usercontrols_cFront_MembershipOptions" %>

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
            </div>

        </div>
        <div class="form-group">
            <label for="contact-email" class="col-sm-2 control-label"><b>Optional extras</b></label>
            <div class="col-sm-10">
                <asp:CheckBoxList ID="extras" runat="server" CssClass="checkbox"></asp:CheckBoxList>
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
                    Text="I agree to give at least 5 hours of my time to assist in duties for the club during the current year." />
                <asp:CustomValidator runat="server" ID="requiredVolunteering" OnServerValidate="CheckBoxRequired_ServerValidate" CssClass="help-block alert-danger">
                    Please accept the volunteering agreement to proceed</asp:CustomValidator>
            </div>
            
        </div>
    </div>
