<%@ Control Language="C#" AutoEventWireup="true" Inherits="Gecko.Uploadify.UploadifyControl" %>

<link href="/usercontrols/Gecko.Uploadify/uploadify.css" rel="stylesheet" type="text/css" />
<script src="/usercontrols/Gecko.Uploadify/jquery.uploadify.v2.1.0.min.js" type="text/javascript"></script>
<script src="/usercontrols/Gecko.Uploadify/swfobject.js" type="text/javascript"></script>

<%var clientId = InputFieldId ?? this.ClientID; %>

<input id="<%=clientId%>" name="<%=clientId%>" type="file" />

<script type="text/javascript">
	$(function () {
		var errorsOccurred = false;
		var auth = "<%=AuthId%>";
		var input = $('#<%=clientId%>');
		input.uploadify({
			'uploader': '/usercontrols/Gecko.Uploadify/uploadify.swf',
			'script': '/usercontrols/Gecko.Uploadify/Uploadify.ashx',
			'cancelImg': '/usercontrols/Gecko.Uploadify/cancel.png',
			'auto': true,
			'multi': true,
			'wmode': 'transparent',
			//'buttonImg' : '/usercontrols/Gecko.Uploadify/upload.png',
			//'width': 102,
			//'height': 37,
			'sizeLimit': <%=SizeLimitBytes%>,
			'buttonText': '<%=ButtonText%>',
			'scriptData': {
				'AUTHID': auth,
				'mediaFolder' : <%=MediaFolder%>
			}
			<% if (!string.IsNullOrEmpty(AllowedTypes)) { %>
				,'fileExt' : '<%=AllowedTypes%>'
				,'fileDesc' : '<%=AllowedTypes%>'
			<% } %>
			,'onComplete': function(event, queueID, fileObj, response, data) {
				<%=OnComplete%>
						
				<% // default behaviour when OnComplete is not set.
					if (string.IsNullOrEmpty(OnComplete)) { %>
					if (response != "1") { errorsOccurred = true; alert(response); }
				<% } %>
				return false;
			}
			,'onAllComplete': function(event, data)  {
				<%=OnAllComplete%>
						
				<% // default behaviour when OnAllComplete is not set.
				if (string.IsNullOrEmpty(OnAllComplete)) {  %>
				if (data.errors == 0 && !errorsOccurred) {
					window.location.reload();
				}
				errorsOccurred = false;
				<% } %>
			}
		});
	});
</script>
<%--
<script type="text/javascript">
	
	$(function () {
				var errorsOccurred = false;
				var auth = "<%=AuthId%>";
				var input = $('#<%=InputFieldId%>');
				input.uploadify({
					'uploader': '/usercontrols/Gecko.Uploadify/uploadify.swf',
					'script': '/usercontrols/Gecko.Uploadify/Uploadify.ashx',
					'cancelImg': '/usercontrols/Gecko.Uploadify/cancel.png',
					'auto': true,
					'multi': true,
					'wmode': 'transparent',
					//'buttonImg' : '/usercontrols/Gecko.Uploadify/upload.png',
					//'width': 102,
					//'height': 37,
					'sizeLimit': <%=SizeLimitBytes%>,
					'buttonText': '<%=ButtonText%>',
					'scriptData': {
						'AUTHID': auth,
						'mediaFolder' : <%=MediaFolder%>
					}
					<% if (!string.IsNullOrEmpty(AllowedTypes)) { %>
						,'fileExt' : '<%=AllowedTypes%>'
						,'fileDesc' : '<%=AllowedTypes%>'
					<% } %>
		//			, 'onError': function (event, queueID ,fileObj, errorObj) { alert(errorObj.info	); }
					,'onComplete': function(event, queueID, fileObj, response, data) {
						<%=OnComplete%>
						
						<% // default behaviour when OnComplete is not set.
							if (string.IsNullOrEmpty(OnComplete)) { %>
							if (response != "1") { errorsOccurred = true; alert(response); }
						<% } %>
						return false;
					}
					,'onAllComplete': function(event, data)  {
						<%=OnAllComplete%>
						
						<% // default behaviour when OnAllComplete is not set.
						if (string.IsNullOrEmpty(OnAllComplete)) {  %>
						if (data.errors == 0 && !errorsOccurred) {
							window.location.reload();
						}
						errorsOccurred = false;
						<% } %>
					}
				});
	});
</script>
--%>