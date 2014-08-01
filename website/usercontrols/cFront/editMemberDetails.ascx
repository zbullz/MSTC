<%@ Control Inherits="usercontrols.cFront.code.EditMemberDetails" Src="code/editMemberDetails.cs" %>

<asp:PlaceHolder ID="EditContainer" runat="server">
    <!-- Contact Form -->
    <h2>Edit your details</h2>
    <p>Please use the form below to update your personal details.</p>

    <div class="contact-form-wrapper">
            <div class="form-group">
                <label class="col-sm-4 control-label"><b>Personal Details</b></label>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label" for="Name"><b>Your name</b></label>
                <div class="col-sm-9">
                    <asp:TextBox ID="Name" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label" for="dateOfBirth"><b>Date of birth</b></label>
                <div class="col-sm-9">
                    <asp:TextBox ID="dateOfBirth" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label" for="address1"><b>House number and Street</b></label>
                <div class="col-sm-9">
                    <asp:TextBox ID="address1" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label" for="address2"><b>Town / City</b></label>
                <div class="col-sm-9">
                    <asp:TextBox ID="address2" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label" for="address3"><b>County</b></label>
                <div class="col-sm-9">
                    <asp:TextBox ID="address3" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label" for="postcode"><b>Postcode</b></label>
                <div class="col-sm-9">
                    <asp:TextBox ID="postcode" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label" for="phoneMobile"><b>Phone number</b></label>
                <div class="col-sm-9">
                    <asp:TextBox ID="phoneMobile" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label" for="Email"><b>Your Email</b></label>
                <div class="col-sm-9">
                    <asp:TextBox ID="Email" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label"><b>Emergency Details</b></label>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label" for="txtMedConditions"><b>Medical Conditions</b></label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtMedConditions" TextMode="MultiLine" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label" for="txtEmergencyName"><b>Emergency contact name</b></label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtEmergencyName" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label" for="txtEmergencyNumber"><b>Emergency contact number</b></label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtEmergencyNumber" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label"><b>Volunteering</b></label>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label" for="txtVolMST"><b>Mid Sussex Triathlon</b></label>
                <div class="col-sm-9">
                    <asp:CheckBox ID="txtVolMST" runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-12">
                    <asp:Button ID="SubmitButton" OnClick="SaveMemberClicked" Text="Save changes" runat="server" CssClass="btn btn-yellow pull-right" />
                </div>
            </div>
    </div>
</asp:PlaceHolder>
<asp:PlaceHolder ID="DebugContainer" runat="server">
    <div class="member_debug">
        <%= DebugText %>
    </div>
</asp:PlaceHolder>
