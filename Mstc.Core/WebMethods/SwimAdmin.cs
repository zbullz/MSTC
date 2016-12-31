using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mstc.Core.DataAccess;
using umbraco.BusinessLogic;
using umbraco.presentation.umbracobase;

namespace Mstc.Core.WebMethods
{
	[RestExtension("SwimAdmin")]
	public class SwimAdmin
	{
		[RestExtensionMethod()]
		public static bool UpdateSwimCredits()
		{
			string nodeIdsString = HttpContext.Current.Request["nodeIds"];
			int creditCost = 5;
			int.TryParse(HttpContext.Current.Request["cost"], out creditCost);
			List<string> nodeIds = nodeIdsString.Split(',').ToList();

			IMemberDal memberDal = new MemberDal(new DataConnection());
			memberDal.UpdateSwimCredits(nodeIds, creditCost);

			Log.Add(LogTypes.Custom, -1, string.Format("Decremented swim credits for nodeIds: {0}", nodeIdsString));

			return true;
		}

	}
}