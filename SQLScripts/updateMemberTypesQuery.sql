Create table #OldMemberTypes (LoginName nvarchar(1000)  COLLATE SQL_Latin1_General_CP1_CI_AS, 
								OldMemberType nvarchar(500)  COLLATE SQL_Latin1_General_CP1_CI_AS)

Insert into #OldMemberTypes (LoginName, OldMemberType)
    Select CmsMember.LoginName,	MemberDataTable.[dataNvarchar]
FROM	(SELECT id, text FROM dbo.umbracoNode WHERE (nodeObjectType = '9b5416fb-e72f-45a9-a07b-5a9a2709ce43')) AS MemberTypeId 
		LEFT OUTER JOIN (SELECT nodeId, contentType FROM dbo.cmsContent) AS MemberList ON MemberList.contentType = MemberTypeId.id 
		LEFT OUTER JOIN dbo.cmsPropertyType AS MemberTypes ON MemberTypes.contentTypeId = MemberList.contentType 
		LEFT OUTER JOIN dbo.cmsPropertyData AS MemberDataTable ON MemberDataTable.contentNodeId = MemberList.nodeId AND MemberDataTable.propertytypeid = MemberTypes.id 
		LEFT OUTER JOIN dbo.cmsMember AS CmsMember ON CmsMember.nodeId = MemberList.nodeId
		inner join dbo.umbracoNode n on n.id = MemberList.nodeId
WHERE	(MemberList.nodeId IS NOT NULL)
		and MemberTypes.Alias in ('membershipTypeOld')
		
--Select * from #OldMemberTypes

Update MemberDataTable
Set MemberDataTable.[dataNVarChar] = (Case omt.OldMemberType When 'Individual' Then 43
										WHEN 'Couple' Then 44
										WHEN 'Concession' Then 45 
								End)
								--select omt.LoginName, CmsMember.LoginName								
From (SELECT id FROM dbo.umbracoNode WHERE (nodeObjectType = '9b5416fb-e72f-45a9-a07b-5a9a2709ce43')) AS MemberTypeId 
					LEFT OUTER JOIN (SELECT nodeId, contentType FROM dbo.cmsContent) AS MemberList ON MemberList.contentType = MemberTypeId.id 
					LEFT OUTER JOIN dbo.cmsPropertyType AS MemberTypes ON MemberTypes.contentTypeId = MemberList.contentType 
					LEFT OUTER JOIN dbo.cmsPropertyData AS MemberDataTable ON MemberDataTable.contentNodeId = MemberList.nodeId 
						AND MemberDataTable.propertytypeid = MemberTypes.id 
					LEFT OUTER JOIN dbo.cmsMember AS CmsMember ON CmsMember.nodeId = MemberList.nodeId
					inner join dbo.umbracoNode n on n.id = MemberList.nodeId 
					LEFT OUTER JOIN  #OldMemberTypes omt on omt.LoginName = CmsMember.LoginName
			Where	MemberTypes.Alias in ('membershipType')

Drop table #OldMemberTypes

