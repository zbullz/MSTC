using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mstc.Core.DataAccess;
using Mstc.Core.Dto;
using Newtonsoft.Json;
using umbraco.BusinessLogic;
using umbraco.presentation.umbracobase;

namespace Mstc.Core.WebMethods
{
	[RestExtension("MemberAdmin")]
	public class MemberAdmin
	{
		[RestExtensionMethod(returnXml = false, allowAll = false, allowGroup = "MemberAdmin")]
		public static string Get()
		{
			try
			{
				//string memberNodeIdsString = HttpContext.Current.Request["memberNodeIds"] ?? string.Empty;
				//string guestNodeIdsString = HttpContext.Current.Request["guestNodeIds"] ?? string.Empty;
				//int creditCost = 6;

				Log.Add(LogTypes.Custom, -1, string.Format("Called MemberAdmin: {0} {1}", "a", "b"));


				IMemberDal memberDal = new MemberDal(new DataConnection());

				IEnumerable<MemberOptionsDto> memberOptions = memberDal.GetMemberOptions().OrderBy(m => m.Name);

				return JsonConvert.SerializeObject(memberOptions.ToList());
			}
			catch (Exception ex)
			{
				Log.Add(LogTypes.Error, -1, string.Format("Exception in MemberAdmin: {0}", ex.ToString()));
				return "";

			}
		}
	}
}