<%@ Control Inherits="cFront.Projects.AbsoluteVacuum.Web.UI.UserControls.EditMember" Src="code/editMember.cs" %>
<%@ Register TagPrefix="cfu" Assembly="cfUmbracoWebControls" Namespace="cFront.Umbraco.WebControls" %>
<script>
	$(document).ready(function() {
		$("input[name*='dateOfBirth']").datepicker({dateFormat: 'dd/mm/yy', yearRange: '-65:-12'});	
		});
</script>
<asp:PlaceHolder id="EditContainer" runat="server"/>
<div class="member_edit">
	<!--  Profile image -->
	<h3>Profile Image:</h3>
	<div class="edit_item">
		<label>Profile image:</label><cfu:MediaImage id="CurrentProfileImage" runat="server"/>
	</div>
	<div class="edit_item">
		<label>Upload new:</label><cfu:MediaUpload id="profileImage" MediaTypeAlias="Image" ParentFolderNameInRoot="Member Profile Images" runat="server"/>
	</div>
	<div class="edit_item">
		<label>Remove image:</label><asp:CheckBox ID="RemoveProfileImage" runat="server"/>
	</div>

	<!--  Personal details -->
	<h3>Personal Details:</h3>
	<div class="edit_item">
		<label>Name:</label><asp:Textbox id="Name" runat="server"/>
	</div>
	<div class="edit_item sectionstart">
		<label>Date of birth:</label><asp:Textbox id="dateOfBirth" runat="server"/>
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
	<div class="edit_item">
		<label>Mobile:</label><asp:Textbox id="phoneMobile" runat="server"/>
	</div>
	<div class="edit_item">
		<label>Email:</label><asp:Textbox id="Email" runat="server" CssClass="txt_large" />
	</div>
	<div class="edit_item">
		<label>BTF no:</label><asp:Textbox id="bTFNumber" runat="server"/>
	</div>
	<div class="edit_item">
		<label>Bio:</label><asp:Textbox id="biography" TextMode="MultiLine" runat="server"/>
	</div>
	
	<h2>Medical &amp; Emergency Contact Details</h2>
	<div class="member_emergency">
		<div class="edit_item">
			<label>Medical Conditions:</label><asp:Textbox id="txtMedConditions" TextMode="MultiLine" runat="server"/>
		</div>
		<div class="edit_item">
			<label>Emergency contact name:</label><asp:Textbox id="txtEmergencyName" runat="server"/>
		</div>
		<div class="edit_item">
			<label>Emergency contact number:</label><asp:Textbox id="txtEmergencyNumber" runat="server"/>
		</div>
	</div>
	
	<h2>Activities you are most interested in:</h2>
	<div class="member_activities">
		<h3>Swimming</h3>
		<div class="edit_item">
			<label>Pool swimming:</label><asp:Checkbox id="activityPoolSwim" runat="server"/>
		</div>
		<div class="edit_item">
			<label>Open water swimming:</label><asp:Checkbox id="activityOpenWaterSwim" runat="server"/>
		</div>
		
		<h3>Cycling</h3>
		<div class="edit_item">
			<label>Time trials:</label><asp:Checkbox id="activityTT" runat="server"/>
		</div>
		<div class="edit_item">
			<label>Road racing:</label><asp:Checkbox id="activityRR" runat="server"/>
		</div>
		<div class="edit_item">
			<label>Off road riding:</label><asp:Checkbox id="activityOffRoad" runat="server"/>
		</div>
		<div class="edit_item">
			<label>Sportives:</label><asp:Checkbox id="activitySportives" runat="server"/>
		</div>
		<div class="edit_item">
			<label>Social riding:</label><asp:Checkbox id="activitySocial" runat="server"/>
		</div>
		
		<h3>Running</h3>
		<div class="edit_item">
			<label>Road running:</label><asp:Checkbox id="activityRoadRunning" runat="server"/>
		</div>
		<div class="edit_item">
			<label>Cross country:</label><asp:Checkbox id="activityCrossCountry" runat="server"/>
		</div>
		
		<h3>Triathlon Distances</h3>
		<div class="edit_item">
			<label>Sprint:</label><asp:Checkbox id="activityDistanceSprint" runat="server"/>
		</div>
		<div class="edit_item">
			<label>Olympic:</label><asp:Checkbox id="activityDistanceOlympic" runat="server"/>
		</div>
		<div class="edit_item">
			<label>Long distance:</label><asp:Checkbox id="activityDistanceLong" runat="server"/>
		</div>
		
		<h2>Volunteering</h2>
		<div class="member_volunteering">
			<div class="edit_item">
				<label>Mid Sussex Triathlon, Burgess Hill:</label><asp:Checkbox id="txtVolMST" runat="server"/>
			</div>
		</div>
		
	</div>
	
	<div class="edit_controls">
		<asp:Button id="SubmitButton" OnClick="SaveMemberClicked" Text="Save" runat="server" CssClass="btn_submit" />
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