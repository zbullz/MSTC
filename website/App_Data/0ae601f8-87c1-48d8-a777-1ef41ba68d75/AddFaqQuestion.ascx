<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddFaqQuestion.ascx.cs" Inherits="umbracoFAQ.AddFaqQuestion" %>
<asp:Panel ID="pQuestion" runat="server">
<h3><asp:Literal runat="server" ID="litTitle"></asp:Literal></h3>
<asp:Label ID="lblAddQuestion" CssClass="umbracoFaqAddQuestionLabel" runat="server"></asp:Label>
<asp:TextBox ID="txtAddQuestion" runat="server" CssClass="umbracoFaqAddQuestionText"></asp:TextBox>
<asp:RequiredFieldValidator 
    ID="QuestionValidator" 
    runat="server" 
    ErrorMessage="You need to enter your question first." 
    ControlToValidate="txtAddQuestion"
    EnableClientScript="true">
    *</asp:RequiredFieldValidator>
<asp:Button runat="server" ID="btnAddQuestion" Text="" OnClick="SubmitQuestion_Click" CssClass="umbracoFaqAddQuestionButton"/>
</asp:Panel>
<asp:Panel ID="pFeedback" CssClass="fbPositive" runat="server">
    <asp:Label ID="lblFeedback" runat="server"></asp:Label>
</asp:Panel>