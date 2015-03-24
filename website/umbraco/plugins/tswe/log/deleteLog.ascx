<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="deleteLog.ascx.cs" Inherits="tswe.log.deleteLog" %>
<%@ Register TagPrefix="cc1" Namespace="umbraco.uicontrols" Assembly="controls" %>
<cc1:Pane ID="Pane1" runat="server">
    <cc1:PropertyPanel runat="server" ID="pp_select" Text="Select old Log entries">
        before&nbsp;<asp:PlaceHolder ID="content" runat="server" />
    </cc1:PropertyPanel>
</cc1:Pane>
<cc1:Pane ID="Pane7" Style="padding-right: 10px; padding-left: 10px; padding-bottom: 10px;
    padding-top: 10px; text-align: left" runat="server" Height="100%" Width="100%">

    <script language="javascript" type="text/javascript">
<!--

function select_deselectAll (chkVal, idVal) 
{ 
    var frm = document.forms[0];
    // Loop through all elements
    for (i=0; i<frm.length; i++) 
    {
        // Look for our Header Template's Checkbox
        if (idVal.indexOf ('CheckAll') != -1) 
        {
            // Check if main checkbox is checked, then select or deselect datagrid checkboxes 
            if(chkVal == true) 
            {
                frm.elements[i].checked = true;
            } 
            else 
            {
                frm.elements[i].checked = false;
            }
            // Work here with the Item Template's multiple checkboxes
        } 
        else if (idVal.indexOf ('DeleteThis') != -1) 
        {
            // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
            if(frm.elements[i].checked == false) 
            {
                frm.elements[1].checked = false; //Uncheck main select all checkbox
            }
        }
    }
}

function confirmDelete (frm) 
{ 
    // loop through all elements
    for (i=0; i<frm.length; i++) 
    {
        // Look for our checkboxes only
        if (frm.elements[i].name.indexOf("DeleteThis") !=-1) 
        {
            // If any are checked then confirm alert, otherwise nothing happens
            if(frm.elements[i].checked) 
            {
                return confirm ('Are you sure you want to remove the selected logs?')
                }
        }
    }
}
// -->
    </script>

    <cc1:PropertyPanel runat="server" ID="pp_selection">
        For delete select log categories with entries older than
        <asp:Label runat="server" ID="lblDays" />
        days:
    </cc1:PropertyPanel>
    <cc1:PropertyPanel runat="server" ID="pp_LogTable">
        <asp:DataGrid class="pane" Font-Size="9px" runat="server" ID="logGrid" HeaderStyle-Font-Bold="True"
            AutoGenerateColumns="False" BackColor="#FEFEFE" HeaderStyle-Height="18px" HeaderStyle-VerticalAlign="Middle"
            HeaderStyle-BackColor="#F4F4F9" BorderColor="#CFCFCF" BorderWidth="1px"
            style="border-color:#CFCFCF">
            <AlternatingItemStyle BackColor="#F7F6FA" />
            <Columns>
                <asp:TemplateColumn ItemStyle-Width="80px">
                    <HeaderTemplate>
                        <asp:CheckBox ID="CheckAll" OnClick="javascript: return select_deselectAll (this.checked, this.id);"
                            Text="" runat="server" />Select
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="DeleteThis" OnClick="javascript: return select_deselectAll (this.checked, this.id);"
                            runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="logHeader" HeaderText="Type" ItemStyle-Width="180px">
                    <ItemStyle Width="180px" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="logCount" HeaderText="Count" ItemStyle-Width="800px">
                    <ItemStyle Width="800px" />
                </asp:BoundColumn>
            </Columns>
            <HeaderStyle Font-Bold="True" BackColor="#F4F4F9" />
        </asp:DataGrid>
    </cc1:PropertyPanel>
</cc1:Pane>
