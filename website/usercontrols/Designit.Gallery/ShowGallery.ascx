<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShowGallery.ascx.cs" Inherits="Designit.Umb.Gallery.WebUi.usercontrols.Designit.Gallery.ShowGallery" %>
<asp:Repeater ID="repImages" runat="server" EnableViewState="false">
    <HeaderTemplate>
        <div id="diGallery">
    </HeaderTemplate>
    <FooterTemplate>
        </div>
    </FooterTemplate>
    <ItemTemplate>
        <asp:HyperLink ID="hlImage" runat="server"><asp:Image ID="img" runat="server" /></asp:HyperLink>
    </ItemTemplate>
</asp:Repeater>