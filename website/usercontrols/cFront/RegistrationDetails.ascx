<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RegistrationDetails.ascx.cs" Inherits="usercontrols_cFront_RegistrationDetails" %>

    <div class="personal-details">
        <div class="form-group">
            <h3 class="col-sm-12">Personal Details</h3>
        </div>
        <div class="form-group">
            <label for="tbFirstName" class="col-sm-2 control-label"><b>Your First Name*</b></label>
            <div class="col-sm-7">
                <asp:TextBox ID="tbFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="requiredFirstName" runat="server" ErrorMessage="Please enter your first name" 
                    ControlToValidate="tbFirstName" CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
            </div>
        </div>
                <div class="form-group">
            <label for="tbLastName" class="col-sm-2 control-label"><b>Your Last Name*</b></label>
            <div class="col-sm-7">
                <asp:TextBox ID="tbLastName" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter your last name" 
                    ControlToValidate="tbLastName" CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <label for="tbEmail" class="col-sm-2 control-label"><b>Your Email*</b></label>
            <div class="col-sm-7">
                
                 <asp:TextBox ID="tbEmail" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="requiredEmail" runat="server" ErrorMessage="Please enter your email" 
                    ControlToValidate="tbEmail" CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="availableEmailValidator" runat="server" ControlToValidate="tbEmail" CssClass="help-block alert-danger"
                    ErrorMessage="A user with your email address has already registered" OnServerValidate="availableEmailValidator_OnValidate"></asp:CustomValidator>

            </div>
        </div>
                <div class="form-group">
            <label for="tbPassword" class="col-sm-2 control-label"><b>Your Password*</b></label>
            <div class="col-sm-7">
                 <asp:TextBox ID="tbPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="requiredPassword" runat="server" ErrorMessage="Please enter your password" 
                    ControlToValidate="tbPassword" CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ControlToValidate="tbPassword" ID="minLengthValidator" ValidationExpression = "^[\s\S]{8,}$" runat="server" 
                    CssClass="help-block alert-danger" ErrorMessage="Enter at least 8 characters"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="form-group">
            <label for="rblGender" class="col-sm-2 control-label"><b>Gender*</b></label>
            <div class="col-sm-7">
                <asp:RadioButtonList ID="rblGender" runat="server" RepeatLayout="Flow" CssClass="radio"></asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="requiredGender" runat="server" ErrorMessage="Please select your gender" 
                    ControlToValidate="rblGender" CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
            </div>            
        </div>
        <div class="form-group">
            <label for="tbdateOfBirth" class="col-sm-2 control-label"><b>Date of birth*</b></label>
            <div class="col-sm-7">
                 <asp:TextBox ID="tbdateOfBirth" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredDateOfBirth" runat="server" ErrorMessage="Please select your date of birth" 
                    ControlToValidate="tbdateOfBirth" CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="RangeDateOfBirth" runat="server" ControlToValidate="tbdateOfBirth" Type="Date" MinimumValue="01/01/1900"
                    ErrorMessage="You must be at least 16 years old at the time of registration" CssClass="help-block alert-danger"></asp:RangeValidator>
                <asp:CustomValidator ID="CustomDateOfBirth" runat="server" ControlToValidate="tbdateOfBirth" CssClass="help-block alert-danger"
                    ErrorMessage="Your date of birth must be in the format dd/mm/yyyy" OnServerValidate="dateOfBirthValidator_OnValidate"></asp:CustomValidator>
            </div>            
        </div>
        <div class="form-group">
            <label for="tbStreet" class="col-sm-2 control-label"><b>House number and Street*</b></label>
            <div class="col-sm-7">
                 <asp:TextBox ID="tbStreet" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredStreet" runat="server" ErrorMessage="Please enter your house number and street" 
                    ControlToValidate="tbStreet" CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
            </div>            
        </div>
        <div class="form-group">
            <label for="tbCity" class="col-sm-2 control-label"><b>Town / City*</b></label>
            <div class="col-sm-7">
                 <asp:TextBox ID="tbCity" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredCity" runat="server" ErrorMessage="Please enter your town / city" 
                    ControlToValidate="tbCity" CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
            </div>            
        </div>
        <div class="form-group">
            <label for="tbPostcode" class="col-sm-2 control-label"><b>Postcode*</b></label>
            <div class="col-sm-7">
                 <asp:TextBox ID="tbPostcode" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredPostcode" runat="server" ErrorMessage="Please enter your postcode" 
                    ControlToValidate="tbPostcode" CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
            </div>            
        </div>
        <div class="form-group">
            <label for="tbPhoneNumber" class="col-sm-2 control-label"><b>Phone number*</b></label>
            <div class="col-sm-7">
                 <asp:TextBox ID="tbPhoneNumber" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredPhoneNumber" runat="server" ErrorMessage="Please enter your phone number" 
                    ControlToValidate="tbPhoneNumber" CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
            </div>            
        </div>
        <div class="form-group">
            <label for="tbBTFNumber" class="col-sm-2 control-label"><b>BTF number</b></label>
            <div class="col-sm-7">
                 <asp:TextBox ID="tbBTFNumber" runat="server" CssClass="form-control"></asp:TextBox>
            </div>            
        </div>
    </div>

    <div class="emergency-details">
        <div class="form-group">
            <h3 class="col-sm-12">Emergency Details</h3>
        </div>
        <div class="form-group">
            <label for="tbMedicalConditions" class="col-sm-2 control-label"><b>Medical conditions*</b></label>
            <div class="col-sm-7">
                <asp:TextBox ID="tbMedicalConditions" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredMedicalConditions" runat="server" ErrorMessage="Please enter your medical conditions or 'none' if you don't have any" 
                    ControlToValidate="tbMedicalConditions" CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <label for="tbEmergencyContactName" class="col-sm-2 control-label"><b>Emergency contact name*</b></label>
            <div class="col-sm-7">
                <asp:TextBox ID="tbEmergencyContactName" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredEmergencyContactName" runat="server" ErrorMessage="Please enter the name of your emergency contact" 
                    ControlToValidate="tbEmergencyContactName" CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <label for="tbEmergencyContactNumber" class="col-sm-2 control-label"><b>Emergency contact phone number*</b></label>
            <div class="col-sm-7">
                <asp:TextBox ID="tbEmergencyContactNumber" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredEmergencyContactNumber" runat="server" ErrorMessage="Please enter the phone number of your emergency contact" 
                    ControlToValidate="tbEmergencyContactNumber" CssClass="help-block alert-danger"></asp:RequiredFieldValidator>
            </div>
        </div>
    </div>

