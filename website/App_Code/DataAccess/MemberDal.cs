using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

public interface IMemberDal
{
	IEnumerable<MemberServiceDto> GetMembersWithServices();
    IEnumerable<MemberSummaryDto> GetMemberSummaries();
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

		var memberServiceDtos = memberData.GroupBy(m => m.Email).Select(g => MapMemberDataToService(g));
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

        var memberSummaries = memberData.GroupBy(m => m.Email).Select(g => MapMemberDataToSummary(g));
        return memberSummaries;
    }

    private MemberServiceDto MapMemberDataToService(IGrouping<string, MemberData> groupedMemberData)
	{
		var memberServiceDto = new MemberServiceDto()
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

	private string GetPropertyValueForAlias(IGrouping<string, MemberData> groupedMemberData, string alias)
	{
		var memberData = groupedMemberData.FirstOrDefault(d => d.PropertyAlias == alias);
		return memberData != null ? memberData.PropertyValue : string.Empty;
	}

    private MemberSummaryDto MapMemberDataToSummary(IGrouping<string, MemberData> groupedMemberData)
    {
        var memberServiceDto = new MemberSummaryDto()
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
}