<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditMemberOptions.ascx.cs" Inherits="usercontrols_cFront_EditMemberOptions" %>

<h2>Membership</h2>

<div class="contact-form-wrapper">
    <div class="form-group <%=EnableMemberRenewal ? "" : "hide"%>">
        <div class="col-sm-12">
            <umbraco:Macro ID="membershipRenewalPageMacro" runat="server" Language="cshtml" linkID="[#membershipRenewalPage]" linkText="Renew now"
                Class="btn btn-yellow pull-left">@RenderPage("~/macroscripts/PageLink.cshtml")
            </umbraco:Macro>
        </div>
    </div>
    <div class="form-group <%=EnableGuestUpgrade ? "" : "hide"%>">
        <div class="col-sm-12">
            <umbraco:Macro ID="Macro3" runat="server" Language="cshtml" linkID="[#membershipRenewalPage]" linkText="Upgrade now"
                Class="btn btn-yellow pull-left">@RenderPage("~/macroscripts/PageLink.cshtml")
            </umbraco:Macro>
        </div>
    </div>
    <div class="form-group <%=EnableGuestRenewal ? "" : "hide"%>">
        <div class="col-sm-12">
            <umbraco:Macro ID="Macro1" runat="server" Language="cshtml" linkID="[#guestRenewalPage]" linkText="Renew now"
                Class="btn btn-yellow pull-left">@RenderPage("~/macroscripts/PageLink.cshtml")
            </umbraco:Macro>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-6 control-label"><b>Membership expires:</b></label>
        <div class="col-sm-6 member-date">
            <p>
                <asp:Literal ID="membershipExpiry" runat="server"></asp:Literal>
            </p>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-6 control-label"><b>Membership Type:</b></label>
        <div class="col-sm-6 member-date">
            <p>
                <asp:Literal ID="membershipType" runat="server"></asp:Literal>
            </p>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-6 control-label"><b>Optional extras:</b></label>
        <div class="col-sm-6 member-date">
            <p>
                <asp:Literal ID="membershipOptionalExtras" runat="server"></asp:Literal>
            </p>
        </div>
    </div>
    <div class="form-group <%=ShowBuySwimSubs1 ? "" : "hide"%>">
        <div class="col-sm-12">
            <asp:Button ID="BuySwimSubs1" runat="server" Text="Buy pool swim subs Apr-Sept" CssClass="btn btn-yellow pull-left" OnClick="btn_BuySwimSubs1Click" />
        </div>
    </div>
    <div class="form-group <%=ShowBuySwimSubs2 ? "" : "hide"%>">
        <div class="col-sm-12">
            <asp:Button ID="BuySwimSubs2" runat="server" Text="Buy pool swim subs Oct-Mar" CssClass="btn btn-yellow pull-left" OnClick="btn_BuySwimSubs2Click" />
        </div>
    </div>


    <div class="form-group <%=ShowMemberAdminLink ? "" : "hide"%>">
        <label class="col-sm-6 control-label"><b>Super secret admin page:</b></label>
        <div class="col-sm-6 member-date">
            <p>
                <a href="/members-area/members-admin-details.aspx">Members Admin Details</a>
            </p>
        </div>
    </div>
    
        <div class="form-group <%=ShowIceLink ? "" : "hide"%>">
        <label class="col-sm-6 control-label"><b>In case of Emergency page:</b></label>
        <div class="col-sm-6 member-date">
            <p>
                <a href="/members-area/members-ice-details.aspx">Members ICE Details</a>
            </p>
        </div>
    </div>
</div>

<div id="open-water-section">
    <h2>Open Water Swim</h2>
    <div class="contact-form-wrapper">
        <div class="<%=EnableOpenWater && !IsGuest ? "" : "hide"%>">
            <div class="form-group">
                <label class="col-sm-6 control-label"><b>O/W swim auth number:</b></label>
                <div class="col-sm-6 member-date">
                    <p>
                        <asp:HyperLink ID="openWaterAuthNumber" runat="server" data-toggle="tooltip" data-placement="bottom"
                            title="Take this number with you to open water swim sessions. You can't get in the water unless you have your number!"></asp:HyperLink>
                    </p>
                </div>
            </div>
            <div class="form-group <%=IsGuest ? "hide" : ""%>">
                <div class="col-sm-12">
                    <asp:Button ID="btn_5SwimCredits" runat="server" Text="Buy £16 credits" CssClass="btn btn-yellow pull-left" OnClick="btn_5SwimCreditsClick" />
                </div>
            </div>
            <div class="form-group <%=IsGuest ? "hide" : ""%>">
                <div class="col-sm-12">
                    <asp:Button ID="btn_10SwimCredits" runat="server" Text="Buy £32 credits" CssClass="btn btn-yellow pull-left" OnClick="btn_10SwimCreditsClick" />
                </div>
            </div>
            <div class="form-group <%=IsGuest ? "hide" : ""%>">
                <div class="col-sm-12">
                    <asp:Button ID="btn_15SwimCredits" runat="server" Text="Buy £44 credits" CssClass="btn btn-yellow pull-left" OnClick="btn_15SwimCreditsClick" />
                </div>
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-6 control-label"><b>O/W Swim Balance:</b></label>
            <div class="col-sm-6 member-date">
                <p>
                    £<asp:Literal ID="litSwimCredits" runat="server"></asp:Literal>
                    <asp:HiddenField ID="hiddenEmail" runat="server" />
                </p>
            </div>
        </div>
        <div class="form-group <%=ShowSwimAdminLink ? "" : "hide"%>">
            <label class="col-sm-6 control-label"><b>Swim admin page:</b></label>
            <div class="col-sm-6 member-date">
                <p>
                    <a href="/members-area/swim-admin.aspx">Swim Admin</a>
                </p>
            </div>
        </div>
    </div>
</div>

<div id="events-section">
    <h2><a href="/club-events/seasons-events.aspx"><%=DateTime.Now.Year %> Club Events Entered</a></h2>
    <asp:BulletedList ID="EventList" runat="server"></asp:BulletedList>
</div>
