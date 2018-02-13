<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RegisterMemberComplete.ascx.cs" Inherits="usercontrols_cFront_RegisterMemberComplete" %>

<% if (IsRegistered){ %>

    <p>Thank you for registering with the Mid Sussex Tri Club! We have setup a payment for £<asp:Literal ID="litCost" runat="server"></asp:Literal></p> 

    <p>Please go to <a href="/members-area/my-details">your details page</a> to make sure your information is up to date and retrieve your open water swim authorisation number.</p>


<% }else { %>

    <p>Sorry, there has been a problem completing your registration. Please <a href="the-club/contact-us.aspx">send us a message</a> for further assistance </p>

<% } %>