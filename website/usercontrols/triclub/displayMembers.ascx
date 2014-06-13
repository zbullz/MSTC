<%@ Control Inherits="cFront.Projects.AbsoluteVacuum.Web.UI.UserControls.displayMembers" AutoEventWireup="true" Src="code/displayMembers.cs" %>
<%@ Register TagPrefix="cfu" Assembly="cfUmbracoWebControls" Namespace="cFront.Umbraco.WebControls" %>
<asp:PlaceHolder id="phMemberList" runat="server" >
<div class="member-item-list">
<asp:Repeater id="rpMembers" runat="server" >
<ItemTemplate>
	<div class="member-item">
		<div class="member-image">
			<cfu:MediaImage MediaID='<%# SafeID(DataBinder.Eval(Container.DataItem, "[profileImage]")) %>' runat="server"/>
		</div>
		<div class="member-name">
			<%# DataBinder.Eval(Container.DataItem, "[Name]") %>
		</div>
<asp:LoginView runat="server">
	<LoggedInTemplate>
		<div class="member-links">
			<a href='?memberID=<%# Eval("[ID]") %>'><img src="/images/info.gif" title="Athlete info" alt="Info" /></a>
			<a href='mailto:<%# Eval("[Email]") %>'><img src="/images/mail.gif" title="Email this athlete" alt="Email" /></a>
		</div>
	</LoggedInTemplate>
</asp:LoginView>
	</div>
</ItemTemplate>
</asp:Repeater>
</div>
</asp:PlaceHolder>
<asp:PlaceHolder id="phMemberDetail" runat="server">
	<table class="member-detail">
		<tr>
			<td class="member-detail-image">
				<cfu:MediaImage id="CurrentProfileImage" runat="server"/>
			</td>
			<td class="member-detail-info">
				<span class="member-name"><%= strName %></span> <a href='mailto:<%= strEmail %>'><img src="/images/mail.gif" title="Email this athlete" alt="Email" /></a><br />
				<label>DOB:</label> <%= strDOB %><br />
				<label>Tel:</label> <%= strPhone %>
			</td>
		</tr>
	</table>
</asp:PlaceHolder>
