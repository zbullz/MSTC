using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

		return true;
	}

}