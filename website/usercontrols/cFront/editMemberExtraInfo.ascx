<%@ Control Inherits="cFront.Projects.AbsoluteVacuum.Web.UI.UserControls.EditMemberExtra" Src="code/editMemberExtraInfo.cs" %>
<%@ Register TagPrefix="cfu" Assembly="cfUmbracoWebControls" Namespace="cFront.Umbraco.WebControls" %>
<asp:PlaceHolder id="EditContainerExtra" runat="server">
<div class="member_edit">
	<div id="memberSubs">
		<h2>
			Membership
		</h2>
		<div>
			<p>Membership expires:<span class="date"><asp:Label id="lblMembershipExpiry" runat="server"/></span></p>
			<asp:PlaceHolder id="pHRenewMembership" runat="server">
				<a href="/about-mstc/member-area/membership-payment.aspx">Renew membership ></a>
			</asp:PlaceHolder>
			<p>Swim subs expire:<span class="date"><asp:Label id="lblSwimSubsExpiry" runat="server"/></span></p>
			<asp:PlaceHolder id="pHRenewSwimSubs" runat="server">
				<a href="/about-mstc/member-area/membership-payment.aspx">Renew swim subs ></a>
			</asp:PlaceHolder>
		</div>
	</div>
</form>
<script>
(function($)
{
    // UI DatePicker defaults
 
    if($.datepicker)
    {
        $.datepicker.setDefaults(
            {
                changeYear: true,
                constrainInput: true
            }
        );
    }
})(jQuery);
</script>
</asp:PlaceHolder><asp:PlaceHolder id="DebugContainerExtra" runat="server">
<div class="member_debug">
	<%= DebugText %>
</div>
</asp:PlaceHolder>