<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RenewMember.ascx.cs" Inherits="usercontrols_cFront_RenewMember" %>

<%@ Register TagPrefix="cf" TagName="MembershipOptions" Src="/usercontrols/cFront/MembershipOptions.ascx" %>

<div class="form-wrapper">
    <p>Please select from the options below to renew your club membership for 2015</p>
    <!--Membership options-->
    <cf:MembershipOptions ID="membershipOptionsControl" runat="server" />

    <div class="form-group">
        <div class="col-sm-2"></div>
        <div class="col-sm-10">
            <asp:Button ID="RenewMember" runat="server" Text="Continue to payment" CssClass="btn pull-left btn-yellow" OnClick="RenewMember_OnClick" />
        </div>
    </div>
    <asp:Label ID="lblMemberOptions" runat="server"></asp:Label>
</div>
