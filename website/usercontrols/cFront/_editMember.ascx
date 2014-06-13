<%@ Control Inherits="cFront.Projects.AbsoluteVacuum.Web.UI.UserControls.EditMember" Src="code/editMember.cs" %>
<%@ Register TagPrefix="cfu" Assembly="cfUmbracoWebControls" Namespace="cFront.Umbraco.WebControls" %>
<script>
	$(document).ready(function() {
		$("input[name*='dateOfBirth']").datepicker({dateFormat: 'dd/mm/yy', yearRange: '-65:-12'});	
		});
</script>
<asp:PlaceHolder id="EditContainer" runat="server"/>
<div class="member_edit">
	<div class="edit_item">
		<label>Name:</label><asp:Textbox id="Name" runat="server"/>
	</div>
	<hr />
	<div class="edit_item">
		<label>Membership Expires:</label><asp:Label id="lblMembershipExpiry" runat="server"/>
	</div>
	<div class="edit_item">
		<label>Swim Subs Expire:</label><asp:Label id="lblSwimSubsExpiry" runat="server"/>
	</div>
	<hr />
	<div class="edit_item">
		<label>Profile image:</label><cfu:MediaImage id="CurrentProfileImage" runat="server"/>
	</div>
	<div class="edit_item">
		<label>Upload new:</label><cfu:MediaUpload id="profileImage" MediaTypeAlias="Image" ParentFolderNameInRoot="Member Profile Images" runat="server"/>
	</div>
	<div class="edit_item">
		<label>Remove image:</label><asp:CheckBox ID="RemoveProfileImage" runat="server"/>
	</div>
	<hr />
	<div class="edit_item">
		<label>Email:</label><asp:Textbox id="Email" runat="server"/>
	</div>
	<div class="edit_item">
		<label>Mobile:</label><asp:Textbox id="phoneMobile" runat="server"/>
	</div>
	<div class="edit_item sectionstart">
		<label>House no. &#038; street:</label><asp:Textbox id="address1" runat="server"/>
	</div>
	<div class="edit_item">
		<label>Town/City:</label><asp:Textbox id="address2" runat="server"/>
	</div>
	<div class="edit_item">
		<label>County:</label><asp:Textbox id="address3" runat="server"/>
	</div>
	<div class="edit_item">
		<label>Post code:</label><asp:Textbox id="postcode" runat="server"/>
	</div>
	<div class="edit_item sectionstart">
		<label>Date of birth:</label><asp:Textbox id="dateOfBirth" runat="server"/>
	</div>
	<div class="edit_item">
		<label>BTF no:</label><asp:Textbox id="bTFNumber" runat="server"/>
	</div>
	<div class="edit_item">
		<label>Bio:</label><asp:Textbox id="biography" TextMode="MultiLine" runat="server"/>
	</div>
	<div class="edit_controls">
		<asp:Button id="SubmitButton" OnClick="SaveMemberClicked" Text="Save" runat="server"/>
	</div>
</div>
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
</asp:PlaceHolder/><asp:PlaceHolder id="DebugContainer" runat="server"/>
<div class="member_debug">
	<%= DebugText %>
</div>
</asp:PlaceHolder/>