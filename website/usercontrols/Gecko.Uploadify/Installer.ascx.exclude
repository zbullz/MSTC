<%@ Control Language="C#" AutoEventWireup="true" Inherits="Gecko.Uploadify.Installer" Codebehind="Installer.ascx.cs" %>

<asp:Panel ID="pnlInstaller" runat="server">
    <h1>Gecko Uploadify - auto install</h1>
    <p>This installer will:</p>
    <ul>
			<li>Create a Gecko Uploadify macro.</li>
       <li>Create the Gecko Uploadify data type in the developer section.</li>		
       <li>Add an "Upload" tab to the Folder media type and add a property with the Gecko Uploadify data type.</li>
    </ul>
    <p>To start the install process press the <strong>Install</strong> button.</p>
    <p>If you wish to perform the steps manually, press the <strong>Skip</strong> button.</p>
    <p><em>Warning:</em> If you wish to uninstall the package be sure to follow the instructions in the documentation and displayed on the last page of the installer, otherwise you could end up invalidating all folders in the media section.
    </p>
    <p>
        <asp:Button ID="btnSkip" Text="Skip" runat="server" OnClick="SkipClick" />
        <asp:Button ID="btnInstall" Text="Install" runat="server" OnClick="Install" />
    </p>
</asp:Panel>

<asp:Panel ID="pnlSkip" runat="server" Visible="false">
    <h1>Gecko Uploadify - manual install</h1>

		<ol>
			<li>
				Create the Gecko Uploadify data type in the Developer section.
				<ol>
					<li>Go to the <strong>Developer</strong> section, right-click <strong>Data Types</strong>, click <strong>Create</strong></li>
					<li>Give the data type the name "Gecko Uploadify" and press the <strong>Create</strong> button</li>
					<li>In the "Render control" drop down, select the <strong>Gecko Uploadify</strong> entry and click the Save button</li>
				</ol>
			</li>
			<li>
				Add the Gecko Uploadify data type to the Folder media type.
				<ol>
					<li>Go to the <strong>Settings</strong> section and expand the <strong>Media Types</strong> folder</li>
					<li>Click on <strong>Folder</strong> and click the <strong>Tabs</strong> tab</li>
					<li>In the <strong>New tab</strong> field write "Upload" and press <strong>New tab</strong> button</li>
					<li>Select the <strong>Generic properties</strong> tab and create a new property. Name it "Upload Files", select the Gecko Uploadify data type as the <strong>Type</strong> field and the "Upload" tab as the <strong>Tab</strong></li>
					<li>Press the Save button</li>
				</ol>
			</li>
    </ol>
    <br />
</asp:Panel>

<asp:Panel ID="pnlSucces" runat="server" Visible="false">
    <h1>Gecko Uploadify successfully installed</h1>
</asp:Panel>

<asp:Panel ID="pnlError" runat="server" Visible="false">
    <h1>An error occurred while installing the Gecko Uploadify package</h1>
</asp:Panel>

<asp:Panel ID="pnlUninstall" runat="server" Visible="false">
    <h3>List of files that have been added to the umbraco instance:</h3>
    <ul>
        <li>~/bin/Gecko.Uploadify.dll</li>
        <li>~/usercontrols/Gecko.Uploadify - the whole folder</li>
    </ul>
    <h3>Uninstalling the Gecko Uploadifyd package</h3>
		<br />
    <em>Warning:</em> If you wish to uninstall the package be sure to follow the instructions below in the in the correct order, otherwise you could end up invalidating the folders in the media section.
    <ol>
        <li>Go to the <strong>Settings</strong> section and expand the <strong>Media Type</strong> folder</li>
        <li>Click on <strong>Folder</strong>, go to the <strong>Generic Properties</strong> tab and delete the property <strong>"Upload Files"</strong></li>
        <li>Go to the <strong>Tabs</strong> tab and delete the <strong>Upload</strong> tab</li>
        <li>Go to the <strong>Developer</strong> section and delete the Gecko Uploadify data type as well as the macro</li>
        <li>Finally, make sure the files listed above are removed from the file system</li>
    </ol>
</asp:Panel>