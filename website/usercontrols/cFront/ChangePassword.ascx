<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.ascx.cs" Inherits="usercontrols_cFront_ChangePassword" %>

<form id="loginForm" runat="server" class="form-horizontal">
    <div class="form-group">
        <label class="col-sm-3 control-label" for="tbEmail">Enter your current password*</label>
        <div class="col-sm-7">
            <asp:TextBox ID="tbCurrentPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="tbCurrentPassword" Text="Please enter your current password"
                CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
        </div>
    </div>
        <div class="form-group">
        <label class="col-sm-3 control-label" for="tbEmail">Enter your new password*</label>
        <div class="col-sm-7">
            <asp:TextBox ID="tbNewPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbNewPassword" Text="Please enter your new password"
                CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
            <asp:Literal ID="litError" runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-10">
            <asp:Button ID="btnSubmit" CommandName="Login" runat="server" Text="Change Password" CssClass="btn btn-yellow pull-right" OnClick="BtnSubmitClick"></asp:Button>
        </div>
    </div>
</form>