<%@ Control Language="C#" AutoEventWireup="true" Inherits="triclub.Umbraco.UDT_adminEntries" %>
<div style="clear:both; width:100%; padding-top:30px;">
<asp:DataList id="dlEntries"
			   GridLines="Both"
			   CellPadding="1"
			   CellSpacing="0"
			   width="700px"
			   Font-Size="11px"
			   OnEditCommand="Edit_Command"
			   OnUpdateCommand="Update_Command"
			   OnDeleteCommand="Delete_Command"
			   OnCancelCommand="Cancel_Command"
			   DataKeyField="IID"
			   runat="server">
			   
			 <AlternatingItemStyle BackColor="#EBEBEB">
			 </AlternatingItemStyle>
			
			 <EditItemStyle BackColor="#EDEDED">
			 </EditItemStyle>
				<HeaderTemplate>
					<tr>
						<td>
							Name
						</td>
						<td>
							Club
						</td>
						<td>
							Event Type
						</td>
						<td>
							Entry Date
						</td>
						<td>
							Entry Confirmed
						</td>
						<td>
							Contact Details
						</td>
						<td>
							&nbsp;
						</td>
					</tr>
				</HeaderTemplate>
				<ItemTemplate>
					<tr>
						<td>
							<b><%# DataBinder.Eval(Container.DataItem, "ULastName") %></b>,&nbsp;
							<%# DataBinder.Eval(Container.DataItem, "FirstName") %>
						</td>
						<td>
							<%# DataBinder.Eval(Container.DataItem, "Club") %>
						</td>
						<td>
							<%# DataBinder.Eval(Container.DataItem, "EventTypeName") %>
						</td>
						<td>
							<%# DataBinder.Eval(Container.DataItem, "EntryDate") %>
						</td>
						<td>
							<%# DataBinder.Eval(Container.DataItem, "Confirmed") %>
						</td>
						<td>
							<a href='mailto:<%# DataBinder.Eval(Container.DataItem, "Email") %>'><%# DataBinder.Eval(Container.DataItem, "Email") %></a><br />
							<%# DataBinder.Eval(Container.DataItem, "Phone") %><br />
						</td>
						<td>
							<asp:LinkButton 
							id="EditButton" 
							Text="Edit" 
							CommandName="Edit"
							CssClass="editlink"
							runat="server"/>
						</td>
					</tr>
			 </ItemTemplate>

			 <EditItemTemplate>
				<tr>
					<td colspan="6">
						<span class="ed_title">First Name:</span>
						<asp:TextBox id="etxtFirstName" Text='<%# DataBinder.Eval(Container.DataItem, "FirstName") %>' runat="server"/>
						<span class="ed_title">Surname:</span>
						<asp:TextBox id="etxtLastName" Text='<%# DataBinder.Eval(Container.DataItem, "ULastName") %>' runat="server"/>
						<br />
						<span class="ed_title">Email:</span>
						<asp:TextBox id="etxtEmail" Text='<%# DataBinder.Eval(Container.DataItem, "Email") %>' runat="server"/>	
						<br />
						<span class="ed_title">Club:</span>
						<asp:TextBox id="etxtClub" Text='<%# DataBinder.Eval(Container.DataItem, "Club") %>' runat="server"/>
						<br>
						<span class="ed_title">Event Type:</span>
						<br />
						<asp:DropDownList id="eddlEventType" selectedvalue='<%# Bind("EventType") %>' runat="server">
							<asp:ListItem Value="1">Full Triathlon</asp:ListItem>
							<asp:ListItem Value="2">Aquabike</asp:ListItem>
						</asp:DropDownList>
						<br>
						<span class="ed_title">Payment:</span>
						<br />
						<asp:TextBox id="etxtPayment" Text='<%# DataBinder.Eval(Container.DataItem, "Payment") %>' runat="server"/>
						<br />
						<span class="ed_title">Swim time:</span>
						<br />
						<asp:TextBox id="etxtMins" Text='<%# DataBinder.Eval(Container.DataItem, "Mins") %>' runat="server"/> : <asp:TextBox id="etxtSecs" Text='<%# DataBinder.Eval(Container.DataItem, "Secs") %>' runat="server"/>					
						<br />
						<span class="ed_title">Accepted:</span>
						<asp:CheckBox id="ecbAccept" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Accept")) %>' runat="server" />
						<br />
					</td>
				</tr>
				<tr class="ed_btns">
				<td colspan="6">
					<asp:LinkButton id="UpdateButton" 
						 Text=" + Update" 
						 CommandName="Update"
						 CssClass="updatelink" 
						 runat="server"/>
					<asp:LinkButton id="DeleteButton" 
						 Text=" X Delete" 
						 CommandName="Delete" 
						 CssClass="deletelink"
						 runat="server"/>
					<asp:LinkButton id="CancelButton" 
						 Text=" - Cancel" 
						 CommandName="Cancel"
						 CssClass="cancellink"
						 runat="server"/>
					</td>
				</tr>
			 </EditItemTemplate>
		  </asp:DataList>
</div>
