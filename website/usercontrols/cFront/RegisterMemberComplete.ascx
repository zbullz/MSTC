<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RegisterMemberComplete.ascx.cs" Inherits="usercontrols_cFront_RegisterMemberComplete" %>
<% if (PromptForConfirmation) { %>

    <p>Your selected membership options:</p>
    <p><strong><%=PaymentDescription %></strong></p>
    <p>Would like to register for Mid Sussex Tri Club for <strong>£<%=Cost %>?</strong></p>
    <asp:Button ID="ConfirmButton" runat="server" Text="Confirm Payment" CssClass="btn pull-left btn-yellow" OnClick="Confirm_OnClick"/>

<% } else if (IsRegistered) { %>

    <p>Thank you for registering with the Mid Sussex Tri Club! We have setup a payment for £<%=Cost %></p> 

    <p>Please go to <a href="/members-area/my-details">your details page</a> to make sure your information is up to date and retrieve your open water swim authorisation number.</p>


<% } else { %>

    <p>Sorry, there has been a problem completing your registration. Please <a href="the-club/contact-us.aspx">send us a message</a> for further assistance </p>

<% } %>