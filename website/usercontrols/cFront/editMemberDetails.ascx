<%@ Control Language="C#" AutoEventWireup="true" CodeFile="editMemberDetails.ascx.cs" Inherits="usercontrols_cFront_editMemberDetails" %>

<%@ Register TagPrefix="cfu" Assembly="cfUmbracoWebControls" Namespace="cFront.Umbraco.WebControls" %>

<asp:PlaceHolder ID="EditContainer" runat="server">
    <!-- Contact Form -->
    <h2>Edit your details</h2>
    <p>Please use the form below to update your personal details.</p>

    <div class="contact-form-wrapper">
            <div class="form-group">
                <h3 class="col-sm-12">Personal Details</h3>
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
                    <asp:TextBox ID="Email" runat="server" CssClass="form-control"   />
                </div>
            </div>

            <div class="form-group">
                <h3 class="col-sm-12">Emergency Details</h3>
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
                <h3 class="col-sm-12">Promote a Service</h3>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label" for="cbShowService"><b>Show my service on the website</b></label>
                <div class="col-sm-9">
                    <asp:CheckBox ID="cbShowService" runat="server" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label" for="tbServiceLinkAddress"><b>Web Link to service</b></label>
                <div class="col-sm-9">
                    <asp:TextBox ID="tbServiceLinkAddress" runat="server" CssClass="form-control" ToolTip="eg. http://www.myservice.com" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label" for="tbServiceLinkText"><b>Text on service link</b></label>
                <div class="col-sm-9">
                    <asp:TextBox ID="tbServiceLinkText" runat="server" CssClass="form-control" ToolTip="eg. My Service" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label" for="serviceImage"><b>Service image</b></label>
                <div class="col-sm-9">
                    <cfu:MediaImage ID="currentServiceImage" runat="server" Width="300px" />
                    <cfu:MediaUpload ID="serviceImage" MediaTypeAlias="Image" ParentFolderNameInRoot="Member Service Images" runat="server" 
                        ToolTip="Recommended size 300px width, 200px height" />    
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-3 control-label" for="tbServiceDescription"><b>Short description of service</b></label>
                <div class="col-sm-9">
                    <asp:TextBox ID="tbServiceDescription" TextMode="MultiLine" runat="server" CssClass="form-control" Rows="3" ToolTip="Up to 200 characters" />
                    <asp:RegularExpressionValidator runat="server" ID="valServiceDescription" ControlToValidate="tbServiceDescription"
                        ValidationExpression="^[\s\S]{0,200}$" ErrorMessage="Please enter a maximum of 200 characters" Display="Dynamic"></asp:RegularExpressionValidator>
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