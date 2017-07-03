<%@ Page language="c#" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System" %>


<script runat="server">
public void RecursiveDelete(string path, string name, bool bRemove)
  {
     foreach (string directory in Directory.GetDirectories(path))
     {
        if (directory.EndsWith("\\" + name))
        {
			if (bRemove)
			{
				Response.Write("removed " + directory+"<br/>");
				Directory.Delete(directory, true);
			}
			else
				Response.Write(directory+"<br/>");//xDirectory.Delete(directory, true);
        }
        else
        {
           RecursiveDelete(directory, name, bRemove);
        }
     }
  }

</script>

<%
	Response.Write("<html><body>");
	Response.Write("<h2>Script for removing ImageGen-\"cached\"-folders from the Media library</h2>");
	bool bRemove=false;
	if(Request.QueryString["remove"] == "true")
	{
		Response.Write("<h3>In remove-mode - removing the following folders:</h3>");
		bRemove=true;
	}
	else
	{
		Response.Write("<h3>In listing-mode - found the following matching folders:</h3>");
	}
	
	RecursiveDelete(MapPath(".\\Media"), "Cached", bRemove);
	Response.Write("</body></html>");
%>