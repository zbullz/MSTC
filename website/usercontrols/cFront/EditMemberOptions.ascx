<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditMemberOptions.ascx.cs" Inherits="usercontrols_cFront_EditMemberOptions" %>

<h2>Membership</h2>

<div class="contact-form-wrapper col-sm-12">
    <div class="form-group <%=EnableMemberRenewal ? "" : "hide"%>">
        <div>
            <umbraco:Macro ID="membershipRenewalPageMacro" runat="server" Language="cshtml" linkID="[#membershipRenewalPage]" linkText="Renew now"
                Class="btn btn-yellow pull-left">@RenderPage("~/macroscripts/PageLink.cshtml")
            </umbraco:Macro>
        </div>
    </div>
    <div class="form-group <%=EnableGuestUpgrade ? "" : "hide"%>">
        <div>
            <umbraco:Macro ID="Macro3" runat="server" Language="cshtml" linkID="[#membershipRenewalPage]" linkText="Upgrade now"
                Class="btn btn-yellow pull-left">@RenderPage("~/macroscripts/PageLink.cshtml")
            </umbraco:Macro>
        </div>
    </div>
    <div class="form-group <%=EnableGuestRenewal ? "" : "hide"%>">
        <div>
            <umbraco:Macro ID="Macro1" runat="server" Language="cshtml" linkID="[#guestRenewalPage]" linkText="Renew now"
                Class="btn btn-yellow pull-left">@RenderPage("~/macroscripts/PageLink.cshtml")
            </umbraco:Macro>
        </div>
    </div>
    <div class="form-group">
        <label class="control-label"><b>Membership expires</b></label>
        <p class="form-control-static">
            <asp:Literal ID="membershipExpiry" runat="server"></asp:Literal></p>
    </div>
    <div class="form-group">
        <label class="control-label"><b>Membership Type</b></label>
        <p class="form-control-static">
            <asp:Literal ID="membershipType" runat="server"></asp:Literal></p>
    </div>
    <div class="form-group">
        <label class="control-label"><b>Optional extras</b></label>
        <p class="form-control-static">
            <asp:Literal ID="membershipOptionalExtras" runat="server"></asp:Literal></p>
    </div>
    <div class="form-group <%=ShowBuySwimSubs1 || ShowBuySwimSubs2 ? "" : "hide"%>">
        <label class="control-label"><b>Pool Swim Subs</b></label>
    </div>
    <div class="form-group <%=ShowBuySwimSubs1 ? "" : "hide"%>">
        <div>
            <asp:Button ID="BuySwimSubs1" runat="server" CssClass="btn btn-yellow pull-left wrap" OnClick="btn_BuySwimSubs1Click" />
        </div>
    </div>
    <div class="form-group <%=ShowBuySwimSubs2 ? "" : "hide"%>">
        <div>
            <asp:Button ID="BuySwimSubs2" runat="server" CssClass="btn btn-yellow pull-left wrap" OnClick="btn_BuySwimSubs2Click" />
        </div>
    </div>
    <div class="form-group">
        <label class="control-label"><b>Payment details</b></label>
    </div>
        <div class="form-group">
        <div>
            <p>If you need to update your bank account details press the button below.<br /> 
               You will then be prompted to enter new details when you next make a payment</p>
            <asp:Button ID="UnlinkBankAccount" runat="server" CssClass="btn btn-yellow pull-left wrap" OnClick="btn_UnlinkBankAccount" Text="Unlink bank account" />
        </div>
    </div>


    <div class="form-group <%=ShowMemberAdminLink ? "" : "hide"%>">
        <label class="control-label"><b>Super secret admin page</b></label>
        <p class="form-control-static"><a href="/members-area/members-admin-details.aspx">Members Admin Details</a></p>
    </div>

    <div class="form-group <%=ShowSwimAdminLink ? "" : "hide"%>">
        <label class="control-label"><b>Swim admin page</b></label>
        <p class="form-control-static"><a href="/members-area/swim-admin.aspx">Swim Admin</a></p>
    </div>

    <div class="form-group <%=ShowIceLink ? "" : "hide"%>">
        <label class="control-label"><b>In case of Emergency page</b></label>
        <p class="form-control-static"><a href="/members-area/members-ice-details.aspx">Members ICE Details</a></p>
    </div>
</div>

<div id="open-water-section" class="<%=EnableOpenWater ? "" : "hide"%>">
    <h2>Open Water Swim</h2>
    <div class="contact-form-wrapper col-sm-12">
        <div class="<%=OwsIndemnityAccepted ? "" : "hide"%>">
            <div class="form-group">
                <label class="control-label"><b>O/W swim auth number</b></label>
                    <p class="form-control-static">
                        <asp:HyperLink ID="openWaterAuthNumber" runat="server" data-toggle="tooltip" data-placement="bottom"
                            title="Take this number with you to open water swim sessions. You can't get in the water unless you have your number!"></asp:HyperLink>
                    </p>
            </div>
            <div class="form-group <%=ShowBuy1SwimCredit ? "" : "hide"%>">
                <div>
                    <asp:Button ID="btn_1SwimCredits" runat="server" Text="Buy £6 credits" CssClass="btn btn-yellow pull-left" OnClick="btn_1SwimCreditsClick" />
                </div>
            </div>
            <div class="form-group <%=IsGuest ? "hide" : ""%>">
                <div>
                    <asp:Button ID="btn_5SwimCredits" runat="server" Text="Buy £18 credits" CssClass="btn btn-yellow pull-left" OnClick="btn_5SwimCreditsClick" />
                </div>
            </div>
            <div class="form-group <%=IsGuest ? "hide" : ""%>">
                <div>
                    <asp:Button ID="btn_10SwimCredits" runat="server" Text="Buy £30 credits" CssClass="btn btn-yellow pull-left" OnClick="btn_10SwimCreditsClick" />
                </div>
            </div>
            <div class="form-group <%=IsGuest ? "hide" : ""%>">
                <div >
                    <asp:Button ID="btn_15SwimCredits" runat="server" Text="Buy £42 credits" CssClass="btn btn-yellow pull-left" OnClick="btn_15SwimCreditsClick" />
                </div>
            </div>
        </div>
        <div class="form-group <%=!OwsIndemnityAccepted ? "" : "hide"%>">
            <p>If you would like to take part in open water swimming please read the waiver and indicate your acceptance.</p>
            <p><a href="http://midsussextriclub.com/media/47452/MSTCIndemnityWaiver.pdf" target="_blank">Open water swim indemnity waiver</a></p>
            <asp:Button ID="Button1" runat="server" Text="Accept Open Water Indemnity Waiver" CssClass="btn btn-yellow pull-left" OnClick="btn_openWaterWaiverClick" />
        </div>
        <div class="form-group">
            <label class="control-label"><b>O/W Swim Balance</b></label>
            <p class="form-control-static">
                    £<asp:Literal ID="litSwimCredits" runat="server"></asp:Literal>
                    <asp:HiddenField ID="hiddenEmail" runat="server" />
            </p>            
        </div>
    </div>
</div>

<div id="events-section">
    <h2><a href="/club-events/seasons-events.aspx">Club Events Entered</a></h2>
    <asp:BulletedList ID="EventList" runat="server"></asp:BulletedList>
</div>
