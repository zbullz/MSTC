using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Mstc.Core.Domain;
using Mstc.Core.Dto;
using umbraco.cms.presentation.create.controls;

namespace Mstc.Core.DataAccess
{
	public interface IMemberDal
	{
		IEnumerable<MemberServiceDto> GetMembersWithServices();
		IEnumerable<MemberSummaryDto> GetMemberSummaries();
		IEnumerable<MemberOptionsDto> GetMemberOptions();
		void UpdateSwimCredits(List<string> nodeIds);
	}

	public class MemberDal : IMemberDal
	{
		private readonly IDataConnection _dataConnection;

		public MemberDal(IDataConnection dataConnection)
		{
			_dataConnection = dataConnection;
		}

		//Somewhat hideous query but I can't help the Umbraco database structures :S
		protected string BaseSelectQuery = @"SELECT	MemberTypes.Alias AS PropertyAlias,					 
					n.TEXT As Name,
                    CmsMember.Email,
					MemberList.NodeId,
					ISNULL(CASE 
						WHEN MemberTypes.datatypeID IN (SELECT NodeId FROM DBO.CMSDATATYPE WHERE DBTYPE = 'Nvarchar') THEN MemberDataTable.[dataNvarchar] 
						WHEN MemberTypes.datatypeID IN (SELECT NodeId FROM DBO.CMSDATATYPE WHERE DBTYPE = 'Ntext') THEN MemberDataTable.[dataNtext] 
						WHEN MemberTypes.datatypeID IN (SELECT NodeId FROM DBO.CMSDATATYPE WHERE DBTYPE = 'Date') THEN CONVERT(NVARCHAR, MemberDataTable.[dataDate]) 
						WHEN MemberTypes.datatypeID IN (SELECT NodeId FROM DBO.CMSDATATYPE WHERE DBTYPE = 'Integer') THEN  CONVERT(NVARCHAR, MemberDataTable.[dataInt])
						ELSE NULL END, NULL) AS PropertyValue 
			FROM	(SELECT id, text FROM dbo.umbracoNode WHERE (nodeObjectType = '9b5416fb-e72f-45a9-a07b-5a9a2709ce43')) AS MemberTypeId 
					LEFT OUTER JOIN (SELECT nodeId, contentType FROM dbo.cmsContent) AS MemberList ON MemberList.contentType = MemberTypeId.id 
					LEFT OUTER JOIN dbo.cmsPropertyType AS MemberTypes ON MemberTypes.contentTypeId = MemberList.contentType 
					LEFT OUTER JOIN dbo.cmsPropertyData AS MemberDataTable ON MemberDataTable.contentNodeId = MemberList.nodeId AND MemberDataTable.propertytypeid = MemberTypes.id 
					LEFT OUTER JOIN dbo.cmsMember AS CmsMember ON CmsMember.nodeId = MemberList.nodeId
					inner join dbo.umbracoNode n on n.id = MemberList.nodeId ";


		public IEnumerable<MemberServiceDto> GetMembersWithServices()
		{
			string query = BaseSelectQuery +
			               @" inner join (select [dataInt] as ShowService, contentNodeId, propertytypeid 
								From dbo.cmsPropertyData 
								Where propertytypeid = 246 And [dataInt] = 1) AS ShowServiceData 
								ON ShowServiceData.contentNodeId = MemberList.nodeId  
			        WHERE	(MemberList.nodeId IS NOT NULL)
					        and MemberTypes.Alias in ('serviceLinkAddress', 'serviceLinkText', 'serviceImage', 'serviceDescription', 'showService')";

			IEnumerable<MemberData> memberData;
			using (IDbConnection connection = _dataConnection.SqlConnection)
			{
				memberData = connection.Query<MemberData>(query, null);
			}

			IEnumerable<MemberServiceDto> memberServiceDtos = memberData.GroupBy(m => m.Email).Select(g => MapMemberDataToService(g));
			return memberServiceDtos;
		}

		public IEnumerable<MemberSummaryDto> GetMemberSummaries()
		{
			string query = BaseSelectQuery +
			               @" WHERE	(MemberList.nodeId IS NOT NULL)
					        and MemberTypes.Alias in ('phoneMobile', 'profileImage')";

			IEnumerable<MemberData> memberData;
			using (IDbConnection connection = _dataConnection.SqlConnection)
			{
				memberData = connection.Query<MemberData>(query, null);
			}

			IEnumerable<MemberSummaryDto> memberSummaries = memberData.GroupBy(m => m.Email).Select(g => MapMemberDataToSummary(g));
			return memberSummaries;
		}

		public IEnumerable<MemberOptionsDto> GetMemberOptions()
		{
			string query = BaseSelectQuery +
			               string.Format(@" WHERE	(MemberList.nodeId IS NOT NULL)
					        and MemberTypes.Alias in ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}')",
				               MemberProperty.Phone, MemberProperty.membershipType, MemberProperty.swimSubsAprToSept,
				               MemberProperty.SwimSubsOctToMar, MemberProperty.OpenWaterIndemnityAcceptance,
				               MemberProperty.Volunteering, MemberProperty.MembershipExpiry, MemberProperty.SwimAuthNumber,
				               MemberProperty.DuathlonEntered, MemberProperty.SwimCreditsBought, MemberProperty.SwimCreditsUsed,
				               MemberProperty.TriFestEntry, MemberProperty.CharitySwimEntry);

			IEnumerable<MemberData> memberData;
			using (IDbConnection connection = _dataConnection.SqlConnection)
			{
				memberData = connection.Query<MemberData>(query, null);
			}
			IEnumerable<MemberOptionsDto> memberOptions = memberData.GroupBy(m => m.Email).Select(g => MapMemberDataToOptions(g));
			return memberOptions;
		}

		public void UpdateSwimCredits(List<string> nodeIds)
		{
			string query = string.Format(@"Update MemberDataTable
							Set MemberDataTable.[dataInt] = ISNULL(MemberDataTable.[dataInt], 0) + 1
							From (SELECT id FROM dbo.umbracoNode WHERE (nodeObjectType = '9b5416fb-e72f-45a9-a07b-5a9a2709ce43')) AS MemberTypeId 
						LEFT OUTER JOIN (SELECT nodeId, contentType FROM dbo.cmsContent) AS MemberList ON MemberList.contentType = MemberTypeId.id 
						LEFT OUTER JOIN dbo.cmsPropertyType AS MemberTypes ON MemberTypes.contentTypeId = MemberList.contentType 
						LEFT OUTER JOIN dbo.cmsPropertyData AS MemberDataTable ON MemberDataTable.contentNodeId = MemberList.nodeId 
							AND MemberDataTable.propertytypeid = MemberTypes.id 
						LEFT OUTER JOIN dbo.cmsMember AS CmsMember ON CmsMember.nodeId = MemberList.nodeId
						inner join dbo.umbracoNode n on n.id = MemberList.nodeId 
						Where	MemberTypes.Alias = '{0}' and MemberList.nodeId in @NodeIds", MemberProperty.SwimCreditsUsed);
			using (IDbConnection connection = _dataConnection.SqlConnection)
			{
				connection.Execute(query, new {NodeIds = nodeIds});
			}
		}

		private MemberServiceDto MapMemberDataToService(IGrouping<string, MemberData> groupedMemberData)
		{
			MemberServiceDto memberServiceDto = new MemberServiceDto()
			{
				Name = groupedMemberData.First().Name,
				ServiceLinkAddress = GetPropertyValueForAlias(groupedMemberData, "serviceLinkAddress"),
				ServiceLinkText = GetPropertyValueForAlias(groupedMemberData, "serviceLinkText"),
				ServiceDescription = GetPropertyValueForAlias(groupedMemberData, "serviceDescription"),
			};

			int serviceImageId;
			if (int.TryParse(GetPropertyValueForAlias(groupedMemberData, "serviceImage"), out serviceImageId))
			{
				memberServiceDto.ServiceImageId = serviceImageId;
			}
			return memberServiceDto;
		}

		private MemberSummaryDto MapMemberDataToSummary(IGrouping<string, MemberData> groupedMemberData)
		{
			MemberSummaryDto memberServiceDto = new MemberSummaryDto()
			{
				Name = groupedMemberData.First().Name,
				Email = groupedMemberData.Key,
				Phone = GetPropertyValueForAlias(groupedMemberData, "phoneMobile")
			};

			int imageId;
			if (int.TryParse(GetPropertyValueForAlias(groupedMemberData, "profileImage"), out imageId))
			{
				memberServiceDto.ProfileImageId = imageId;
			}
			return memberServiceDto;
		}

		private MemberOptionsDto MapMemberDataToOptions(IGrouping<string, MemberData> groupedMemberData)
		{
			MemberOptionsDto memberOptionsDto = new MemberOptionsDto()
			{
				NodeId = groupedMemberData.First().NodeId,
				Name = groupedMemberData.First().Name,
				Email = groupedMemberData.Key,

				Phone = GetPropertyValueForAlias(groupedMemberData, MemberProperty.Phone),
				SwimSubsJanToJune = GetBool(GetPropertyValueForAlias(groupedMemberData, MemberProperty.swimSubsAprToSept)),
				SwimSubsJulyToDec = GetBool(GetPropertyValueForAlias(groupedMemberData, MemberProperty.SwimSubsOctToMar)),
				DuathlonEntered = GetBool(GetPropertyValueForAlias(groupedMemberData, MemberProperty.DuathlonEntered)),
				OpenWaterIndemnityAcceptance =
					GetBool(GetPropertyValueForAlias(groupedMemberData, MemberProperty.OpenWaterIndemnityAcceptance)),
				Volunteering = GetBool(GetPropertyValueForAlias(groupedMemberData, MemberProperty.Volunteering)),
				TriFestEntry = GetPropertyValueForAlias(groupedMemberData, MemberProperty.TriFestEntry),
				CharitySwimEntry = GetPropertyValueForAlias(groupedMemberData, MemberProperty.CharitySwimEntry)
			};

			int swimCreditsBought = 0;
			int.TryParse(GetPropertyValueForAlias(groupedMemberData, MemberProperty.SwimCreditsBought), out swimCreditsBought);
			memberOptionsDto.SwimCreditsBought = swimCreditsBought;

			int swimCreditsUsed = 0;
			int.TryParse(GetPropertyValueForAlias(groupedMemberData, MemberProperty.SwimCreditsUsed), out swimCreditsUsed);
			memberOptionsDto.SwimCreditsUsed = swimCreditsUsed;
		
			string membershipType = GetPropertyValueForAlias(groupedMemberData, MemberProperty.membershipType);
			memberOptionsDto.MembershipType = string.IsNullOrEmpty(membershipType)
				? (MembershipType?) null
				: (MembershipType) Enum.Parse(typeof (MembershipType), membershipType);
		
			string membershipExpiry = GetPropertyValueForAlias(groupedMemberData, MemberProperty.MembershipExpiry);
			memberOptionsDto.MembershipExpiry = string.IsNullOrEmpty(membershipExpiry) ? (DateTime?) null : DateTime.Parse(membershipExpiry);
		
			string swimAuthNumber = GetPropertyValueForAlias(groupedMemberData, MemberProperty.SwimAuthNumber);
			memberOptionsDto.SwimAuthNumber = string.IsNullOrEmpty(swimAuthNumber) ? (int?)null : int.Parse(swimAuthNumber);
		 
	
			return memberOptionsDto;
		}

		private string GetPropertyValueForAlias(IGrouping<string, MemberData> groupedMemberData, string alias)
		{
			MemberData memberData = groupedMemberData.FirstOrDefault(d => d.PropertyAlias == alias);
			return memberData != null ? memberData.PropertyValue : string.Empty;
		}

		private bool GetBool(string valueString)
		{
			return valueString == "1";
		}
	}
}