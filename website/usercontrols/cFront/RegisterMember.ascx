<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RegisterMember.ascx.cs" Inherits="usercontrols_cFront_RegisterMember" %>

<%@ Register TagPrefix="cf" TagName="RegistrationDetails" Src="~/usercontrols/cFront/RegistrationDetails.ascx" %>
<%@ Register TagPrefix="cf" TagName="MembershipOptions" Src="~/usercontrols/cFront/MembershipOptions.ascx" %>

<div class="form-wrapper">
    <p>Please complete the form below to register for membership to the club. <br/>
        Current membership runs from <strong>1st April <%=FromYear %></strong> to <strong>31st March <%=ToYear %></strong>.
    </p>

    <!--Registration details-->
    <cf:RegistrationDetails ID="registrationDetailsControl" runat="server" />

    <!--Membership options-->
    <cf:MembershipOptions ID="membershipOptionsControl" runat="server" />

    <div class="form-group">
        <div class="col-sm-2"></div>
        <div class="col-sm-10">
            <asp:Button ID="RegisterMember" runat="server" Text="Continue to payment" CssClass="btn pull-left btn-yellow" OnClick="RegisterMember_OnClick" />
        </div>
    </div>
    
</div>
