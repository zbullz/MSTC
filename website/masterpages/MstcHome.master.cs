using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco.Membership;
using GoCardlessSdk;
using GoCardlessSdk.Connect;
using Lucene.Net.Search.Function;
using Mstc.Core.Domain;
using Mstc.Core.Providers;
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
