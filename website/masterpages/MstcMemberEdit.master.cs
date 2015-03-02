using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cFront.Umbraco.Membership;

public partial class masterpages_MstcMemberEdit : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
		var memberDetails = MemberHelper.Get();
		editMemberImage.LoadImage(memberDetails);
		editMemberOptions.LoadOptions(memberDetails);
    }
}
