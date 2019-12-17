<%@ Control Language="C#" AutoEventWireup="true" CodeFile="contacts.ascx.cs" Inherits="usercontrols_cFront_contacts" %>
<asp:PlaceHolder ID="phMessageForm" runat="server">
    <div class="form-wrapper">
        <h3>Send Us a Message</h3>
        <p> Get in touch with us to find out about joining, training, sponsorship or whatever you like.</p>
        <p>If you have a query about the Mid Sussex Triathlon please visit our event website.</p>
        <form id="Form1" class="form-horizontal" role="form" runat="server">
            <div class="form-group">
                <label for="txtName" class="col-sm-2 control-label"><b>Your name *</b></label>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtName" runat="Server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" CssClass="help-block alert-danger"
                        ErrorMessage="Please enter your name" Display="Dynamic" />                    
                </div>
            </div>
            <div class="form-group">
                <label for="txtEmail" class="col-sm-2 control-label"><b>Your Email *</b></label>                
                <div class="col-sm-10">
                    <asp:TextBox ID="txtEmail" runat="Server" CssClass="form-control" />  
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" CssClass="help-block alert-danger"
                        ErrorMessage="Please enter your email" Display="Dynamic" />
                    <asp:RegularExpressionValidator CssClass="help-block alert-danger" ID="revEmail" ControlToValidate="txtEmail" runat="server" 
                        ValidationExpression="^\S+@\S+\.\S+$" ErrorMessage="Invalid email address" Display="Dynamic" />                        
                </div>
            </div>
            <div class="form-group">
                <label for="contact-message" class="col-sm-2 control-label"><b>Select Topic *</b></label>
                <div class="col-sm-10">
                    <asp:DropDownList ID="topicSelect" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Please select topic..."></asp:ListItem>
                        <asp:ListItem Text="General" Value="General"></asp:ListItem>
                        <asp:ListItem Text="Membership" Value="Membership"></asp:ListItem>
                        <asp:ListItem Text="Open water swimming" Value="OWS"></asp:ListItem>
                        <asp:ListItem Text="Sponsorship" Value="Sponsorship"></asp:ListItem>
                        <asp:ListItem Text="Juniors" Value="Juniors"></asp:ListItem>
                        <asp:ListItem Text="Website" Value="Website"></asp:ListItem>
                        <asp:ListItem Text="Coaching" Value="Coaching"></asp:ListItem>
                    </asp:DropDownList>
                   
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="topicSelect" CssClass="help-block alert-danger"
                        ErrorMessage="Please select a topic" Display="Dynamic" InitialValue="Please select topic..." />   
                </div>
            </div>
            <div class="form-group">
                <label for="txtMsg" class="col-sm-2 control-label"><b>Message *</b></label>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtMsg" TextMode="MultiLine" runat="Server" CssClass="form-control" />         
                    <asp:RequiredFieldValidator ID="rfvTxtMsg" runat="server" ControlToValidate="txtMsg" CssClass="help-block alert-danger"
                        ErrorMessage="Please enter your message" Display="Dynamic" />           
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-2"></div>
                <div class="col-sm-10">
                    <asp:Label ID="lblResult" Visible="false" runat="server" CssClass="contact-error" />
                    <asp:Button ID="btnSend" Text="Send message" runat="server" OnClick="SendMessage" CssClass="btn pull-left btn-yellow" />  
                </div>
            </div>
        </form>
    </div>

</asp:PlaceHolder>
<asp:PlaceHolder ID="phMessageSent" runat="server">
    <p class="sent_title">
        Message sent!
    </p>
    <p class="sent">
        We will get back to you as soon as possible.
    </p>
</asp:PlaceHolder>