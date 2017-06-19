using System;
using umbraco.NodeFactory;

public partial class masterpages_MstcVideoContent : System.Web.UI.MasterPage
{
    public string VideoSource { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false && Node.GetCurrent() != null && Node.GetCurrent().GetProperty("videoSource") != null)
        {
            VideoSource = Node.GetCurrent().GetProperty("videoSource").Value;
        }
    }
}
