using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.BusinessLogic;
using umbraco.presentation.umbracobase;


[RestExtension("SwimAdmin")]
public class SwimAdmin
{
	[RestExtensionMethod()]
	public static bool UpdateSwimCredits()
	{
		string nodeIdsString = HttpContext.Current.Request["nodeIds"];
		List<string> nodeIds = nodeIdsString.Split(',').ToList();

		IMemberDal memberDal = new MemberDal(new DataConnection());
		memberDal.UpdateSwimCredits(nodeIds);

		Log.Add(LogTypes.Custom, -1, string.Format("Decremented swim credits for nodeIds: {0}", nodeIdsString));

		return true;
	}

}