<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditMemberOptions.ascx.cs" Inherits="usercontrols_cFront_EditMemberOptions" %>

<h2>Membership</h2>

<div class="contact-form-wrapper">
    <div class="form-group <%=EnableRenewal ? "" : "hide"%>">
        <div class="col-sm-12">
            <umbraco:Macro ID="membershipRenewalPageMacro" runat="server" Language="cshtml" linkID="[#membershipRenewalPage]" linkText="Renew now" 
                Class="btn btn-yellow pull-left">@RenderPage("~/macroscripts/PageLink.cshtml")
            </umbraco:Macro>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-6 control-label"><b>Membership expires:</b></label>
        <div class="col-sm-6 member-date">
            <p>
                <asp:Literal ID="membershipExpiry" runat="server"></asp:Literal></p>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-6 control-label"><b>Membership Type:</b></label>
        <div class="col-sm-6 member-date">
            <p>
                <asp:Literal ID="membershipType" runat="server"></asp:Literal></p>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-6 control-label"><b>Optional extras:</b></label>
        <div class="col-sm-6 member-date">
            <p>
                <asp:Literal ID="membershipOptionalExtras" runat="server"></asp:Literal></p>
        </div>
    </div>
    <div class="form-group <%=EnableOpenWater ? "" : "hide"%>">
        <label class="col-sm-6 control-label"><b>Open water swim auth number:</b></label>
        <div class="col-sm-6 member-date">
            <p>
                <asp:HyperLink ID="openWaterAuthNumber" runat="server" data-toggle="tooltip" data-placement="bottom"
                    title="Take this number with you to open water swim sessions. You can't get in the water unless you have your number!"></asp:HyperLink>
            </p>
        </div>
    </div>
</div>
