<%@ Control AutoEventWireup="true" CodeFile="code/ForgotPassword.ascx.cs" Inherits="usercontrols.cFront.code.usercontrols_cFront_ForgotPassword" %>

<form id="loginForm" runat="server" class="form-horizontal">
    <div class="form-group">
        <label class="col-sm-3 control-label" for="tbEmail">Enter your email address*</label>
        <div class="col-sm-7">
            <asp:TextBox ID="tbEmail" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="tbEmail" Text="Please enter your user name"
                CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
            <asp:Literal ID="litError" runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-10">
            <asp:Button ID="btnSubmit" CommandName="Login" runat="server" Text="Send Password" CssClass="btn btn-yellow pull-right" OnClick="BtnSubmitClick"></asp:Button>
        </div>
    </div>
</form>
