<%@ Control Language="C#" AutoEventWireup="true" Inherits="triclub.Umbraco.UDT_adminEvents" %>
<script>
	$(document).ready(function() {
	$("input[name*='txtEventDate']").datepicker({dateFormat: 'yy-mm-dd'});
	});
</script>
<asp:PlaceHolder id="containerEntry" runat="server" Visible="True" >
	<div id="events-list">
		<asp:DataList id="dlEvents"
			   CellPadding="1"
			   CellSpacing="0"
			   width="700px"
			   Font-Size="11px"
			   OnEditCommand="dlEvents_EditCommand"
			   OnUpdateCommand="dlEvents_UpdateCommand"
			   OnCancelCommand="Cancel_Command"
			   OnDeleteCommand="Delete_Command"
			   DataKeyField="IID"
			   runat="server">
			   
			 <AlternatingItemStyle BackColor="#EBEBEB">
			 </AlternatingItemStyle>
			
			 <EditItemStyle BackColor="#EDEDED">
			 </EditItemStyle>
				<HeaderTemplate>
					<tr>
						<td>
							Title
						</td>
						<td>
							Date
						</td>
						<td>
							&nbsp;
						</td>
					</tr>
				</HeaderTemplate>
				<ItemTemplate>
					<tr>
						<td class="dl-title">
							<%# DataBinder.Eval(Container.DataItem, "eventTitle") %>
						</td>
						<td>
							<%# DataBinder.Eval(Container.DataItem, "eventDate", "{0: dd MMM yyyy}") %>
						</td>
						<td class="dl-controls">
							<asp:Button 
							id="EditButton" 
							Text="Edit" 
							CommandName="Edit"
							CssClass="editlink"
							OnClientClick="return true;"
							runat="server"/>
							
							<asp:Button 
							id="DeleteButton" 
							Text="Delete" 
							CommandName="Delete"
							CssClass="deletelink"
							OnClientClick="return confirm ('Are you sure that you want to delete this event?');"
							runat="server"/>
							
						</td>
					</tr>
			</ItemTemplate>
			
			<EditItemTemplate>
				<tr>
					<td colspan="3">
						<p class="dl-editregion">
							<label>Event Name:</label>
							<asp:TextBox id="etxtEventTitle" Text='<%# DataBinder.Eval(Container.DataItem, "EventTitle") %>' runat="server"/>
						</p>
						<p class="dl-editregion">
							<label>Date:</label>
							<asp:TextBox id="etxtEventDate" Text='<%# DataBinder.Eval(Container.DataItem, "EventDate", "{0: yyyy/MM/dd}") %>' runat="server"/>
						</p>
						<p class="dl-editregion">
							<label>Description:</label>
							<asp:TextBox id="etxtEventDescription" TextMode="multiline" Text='<%# DataBinder.Eval(Container.DataItem, "EventDescription") %>' runat="server" />
						</p>
						<p class="dl-editregion">
							<label>Link:</label>
							<asp:TextBox id="etxtEventLink" Text='<%# DataBinder.Eval(Container.DataItem, "EventLink") %>' runat="server" />
						</p>
						<p class="dl-editregion">
							<label>Location:</label>
							<asp:TextBox id="etxtEventLocation" Text='<%# DataBinder.Eval(Container.DataItem, "EventLocation") %>' runat="server"/>
						</p>
						<p class="dl-editregion">
							<label>Results Link:</label>
							<asp:TextBox id="etxtResultsLink" Text='<%# DataBinder.Eval(Container.DataItem, "resultsLink") %>' runat="server" />
						</p>
						<p class="dl-editregion">
							<label>News Link:</label>
							<asp:TextBox id="etxtNewsLink" Text='<%# DataBinder.Eval(Container.DataItem, "newsLink") %>' runat="server" />
						</p>
						<p class="dl-editregion">
							<label>Photo Link:</label>
							<asp:TextBox id="etxtPhotoLink" Text='<%# DataBinder.Eval(Container.DataItem, "photoLink") %>' runat="server" />
						</p>
						<p class="ed_btns">
						<asp:Button id="UpdateButton" 
							 Text=" + Update" 
							 CommandName="Update"
							 CssClass="updatelink"
							 OnClientClick="return true;"
							 runat="server"/>
						<asp:Button id="CancelButton" 
							 Text=" - Cancel" 
							 CommandName="Cancel"
							 OnClientClick="return true;"
							 CssClass="cancellink"
							 runat="server"/>
						</p>
					</td>
				</tr>
			 </EditItemTemplate>
			 
		</asp:DataList>
	</div>
</asp:PlaceHolder>