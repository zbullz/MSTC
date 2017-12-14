using System;
using umbraco.NodeFactory;

public partial class masterpages_MstcHome : System.Web.UI.MasterPage
{
	protected bool EventEntryEnabled = false;

    protected void Page_Load(object sender, EventArgs e)
    {
		if (IsPostBack == false && Node.GetCurrent() != null && Node.GetCurrent().GetProperty("eventEntryEnabled") != null)
		{
		    EventEntryEnabled = Node.GetCurrent().GetProperty("eventEntryEnabled").Value == "1";
		}
    }
}
