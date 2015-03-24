<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="viewLog.ascx.cs" Inherits="tswe.log.viewLog" %>
<%@ Register TagPrefix="cc1" Namespace="umbraco.uicontrols" Assembly="controls" %>
<h2 class="propertypaneTitel">
    Search log</h2>
<cc1:Pane ID="Pane1" runat="server">
    <cc1:PropertyPanel runat="server" ID="pp_node" Text="Content item">
        <asp:PlaceHolder ID="contentPlace" runat="server" />
        <asp:CheckBox ID="cbxAuditTrail" runat="server" Text="Audit trail (no System or Open log items)" /><br /> 
        <span class="guiDialogTiny">Search for a single content item always gets items of all users and log types.<br />
            Selected since days are ignored, you will get all available log entries.</span>
    </cc1:PropertyPanel>
</cc1:Pane>    
<cc1:Pane ID="Pane3" runat="server">
    <cc1:PropertyPanel runat="server" ID="pp_LogType" Text="Log Type">
        <asp:DropDownList ID="ddlLogType" Width="200px" runat="server" class="guiInputText" />
    </cc1:PropertyPanel>
    <cc1:PropertyPanel runat="server" ID="pp_User" Text="User">
        <asp:DropDownList ID="ddlUser" Width="200px" runat="server" class="guiInputText" /><br /> 
        <span class="guiDialogTiny">You can not search log entries for all log types <em>and</em> all users.<br />
            Select a single log type <em>or</em> user instead.</span>
    </cc1:PropertyPanel>
    <cc1:PropertyPanel runat="server" ID="pp_Since" Text="Since">
        <asp:RadioButtonList ID="rblSince" runat="server" RepeatDirection="Horizontal" 
            RepeatLayout="Flow">
            <asp:ListItem Selected="True" Value="1">1 Day</asp:ListItem>
            <asp:ListItem Value="10">10 Days</asp:ListItem>
            <asp:ListItem Value="0">select Date</asp:ListItem>
        </asp:RadioButtonList>
        &nbsp;<asp:PlaceHolder ID="sincePlace" runat="server" />
    </cc1:PropertyPanel>
</cc1:Pane>
<h2 class="propertypaneTitel" id="titlePane2" runat="server">
    Log entries</h2>
<cc1:Pane ID="Pane2" runat="server">
    <cc1:PropertyPanel runat="server" ID="pp_LogTable">
        <asp:DataGrid class="pane" Font-Size="9px" runat="server" ID="logGrid" HeaderStyle-Font-Bold="True"
            AutoGenerateColumns="False" BackColor="#FEFEFE" HeaderStyle-Height="18px" 
            HeaderStyle-VerticalAlign="Middle" HeaderStyle-BackColor="#F4F4F9"
            ItemStyle-VerticalAlign="Top" ItemStyle-Height="16px" 
            BorderColor="#CFCFCF" BorderStyle="Solid" BorderWidth="1px" Width="100%">
            <AlternatingItemStyle BackColor="#F7F6FA" />
            <ItemStyle VerticalAlign="Top" Height="16px"/>
            <Columns>
                <asp:BoundColumn DataField="logHeader" HeaderText="Type" ItemStyle-Width="10%">
                    <ItemStyle Width="10%" />
                </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="User" ItemStyle-Width="15%">
                    <ItemTemplate>
                        <%# GetUserName(DataBinder.Eval(Container.DataItem, "userId").ToString()) %>
                    </ItemTemplate>
                    <ItemStyle Width="15%" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Node" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <%# GetNodeName(DataBinder.Eval(Container.DataItem, "nodeId").ToString()) %>
                    </ItemTemplate>
                    <ItemStyle Width="10%" />
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="DateStamp" HeaderText="Date Stamp" 
                    ItemStyle-Font-Size="9px" ItemStyle-Width="15%">
                    <ItemStyle Font-Size="9px" Width="15%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="logComment" HeaderText="Comment" 
                    ItemStyle-Font-Size="9px">
                    <ItemStyle Font-Size="9px" />
                </asp:BoundColumn>
            </Columns>
            <HeaderStyle Font-Bold="True" BackColor="#F4F4F9" />
        </asp:DataGrid>
    </cc1:PropertyPanel>
</cc1:Pane>
