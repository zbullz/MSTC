<%@ Control Inherits="usercontrols.cFront.code.EditMemberImage" Src="code/editMemberImage.cs" %>
<%@ Register TagPrefix="cfu" Assembly="cfUmbracoWebControls" Namespace="cFront.Umbraco.WebControls" %>

<asp:PlaceHolder ID="EditContainer" runat="server">
    <h2>Profile image</h2>

    <div class="profile-image">
        <cfu:MediaImage ID="CurrentProfileImage" runat="server" /><br/>
        *recommended size 200 x 200px
    </div>
    <div class="contact-form-wrapper">
            <div class="form-group">
                <label class="col-sm-5 control-label" for="profileImage"><b>Upload new:*</b></label>
                <div class="col-sm-7">
                    <cfu:MediaUpload ID="profileImage" MediaTypeAlias="Image" ParentFolderNameInRoot="Member Profile Images" runat="server" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-5 control-label" for="RemoveProfileImage"><b>Remove image</b></label>
                <div class="col-sm-7">
                    <asp:CheckBox ID="RemoveProfileImage" runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-12">
                    <asp:Button ID="SubmitButton" OnClick="SaveMemberClicked" Text="Save changes" runat="server" CssClass="btn btn-yellow pull-left" />
                </div>
            </div>
    </div>
</asp:PlaceHolder>
<asp:PlaceHolder ID="DebugContainer" runat="server">
<div class="member_debug">
    <%= DebugText %>
</div>
</asp:PlaceHolder>