CREATE TABLE #SwimData
	(
		nodeId INT,		
		HasSubsAprToSept bit,
		HasSubsOctToMar bit
	) 

Insert into #SwimData(nodeId, HasSubsAprToSept) 
Select CmsMember.nodeId, MemberDataTable.[DATAINT]
							From (SELECT id FROM dbo.umbracoNode WHERE (nodeObjectType = '9b5416fb-e72f-45a9-a07b-5a9a2709ce43')) AS MemberTypeId 
						LEFT OUTER JOIN (SELECT nodeId, contentType FROM dbo.cmsContent) AS MemberList ON MemberList.contentType = MemberTypeId.id 
						LEFT OUTER JOIN dbo.cmsPropertyType AS MemberTypes ON MemberTypes.contentTypeId = MemberList.contentType 
						LEFT OUTER JOIN dbo.cmsPropertyData AS MemberDataTable ON MemberDataTable.contentNodeId = MemberList.nodeId 
							AND MemberDataTable.propertytypeid = MemberTypes.id 
						LEFT OUTER JOIN dbo.cmsMember AS CmsMember ON CmsMember.nodeId = MemberList.nodeId
						inner join dbo.umbracoNode n on n.id = MemberList.nodeId 			
						Where	MemberTypes.Alias in ('swimSubsAprToSept') 

Update sd 
Set sd.HasSubsOctToMar = MemberData.[DATAINT]
From #SwimData sd
Inner join 
(Select CmsMember.nodeId, MemberDataTable.[DATAINT]
							From (SELECT id FROM dbo.umbracoNode WHERE (nodeObjectType = '9b5416fb-e72f-45a9-a07b-5a9a2709ce43')) AS MemberTypeId 
						LEFT OUTER JOIN (SELECT nodeId, contentType FROM dbo.cmsContent) AS MemberList ON MemberList.contentType = MemberTypeId.id 
						LEFT OUTER JOIN dbo.cmsPropertyType AS MemberTypes ON MemberTypes.contentTypeId = MemberList.contentType 
						LEFT OUTER JOIN dbo.cmsPropertyData AS MemberDataTable ON MemberDataTable.contentNodeId = MemberList.nodeId 
							AND MemberDataTable.propertytypeid = MemberTypes.id 
						LEFT OUTER JOIN dbo.cmsMember AS CmsMember ON CmsMember.nodeId = MemberList.nodeId
						inner join dbo.umbracoNode n on n.id = MemberList.nodeId 			
						Where	MemberTypes.Alias in ('swimSubsOctToMar')) As MemberData on MemberData.NODEID = sd.nodeId


Update MemberDataTable
Set MemberDataTable.[dataNVarChar] = 'Swim Subs Apr to Sept 2018'
From (SELECT id FROM dbo.umbracoNode WHERE (nodeObjectType = '9b5416fb-e72f-45a9-a07b-5a9a2709ce43')) AS MemberTypeId 
					LEFT OUTER JOIN (SELECT nodeId, contentType FROM dbo.cmsContent) AS MemberList ON MemberList.contentType = MemberTypeId.id 
					LEFT OUTER JOIN dbo.cmsPropertyType AS MemberTypes ON MemberTypes.contentTypeId = MemberList.contentType 
					LEFT OUTER JOIN dbo.cmsPropertyData AS MemberDataTable ON MemberDataTable.contentNodeId = MemberList.nodeId 
						AND MemberDataTable.propertytypeid = MemberTypes.id 
					LEFT OUTER JOIN dbo.cmsMember AS CmsMember ON CmsMember.nodeId = MemberList.nodeId
					inner join dbo.umbracoNode n on n.id = MemberList.nodeId 
					inner join #SwimData sd on sd.nodeId = MemberList.nodeId and sd.HasSubsAprToSept = 1
			Where	MemberTypes.Alias in ('swimSubs1')

Update MemberDataTable
Set MemberDataTable.[dataNVarChar] = 'Swim Subs Oct 2018 to Mar 2019'
From (SELECT id FROM dbo.umbracoNode WHERE (nodeObjectType = '9b5416fb-e72f-45a9-a07b-5a9a2709ce43')) AS MemberTypeId 
					LEFT OUTER JOIN (SELECT nodeId, contentType FROM dbo.cmsContent) AS MemberList ON MemberList.contentType = MemberTypeId.id 
					LEFT OUTER JOIN dbo.cmsPropertyType AS MemberTypes ON MemberTypes.contentTypeId = MemberList.contentType 
					LEFT OUTER JOIN dbo.cmsPropertyData AS MemberDataTable ON MemberDataTable.contentNodeId = MemberList.nodeId 
						AND MemberDataTable.propertytypeid = MemberTypes.id 
					LEFT OUTER JOIN dbo.cmsMember AS CmsMember ON CmsMember.nodeId = MemberList.nodeId
					inner join dbo.umbracoNode n on n.id = MemberList.nodeId 
					inner join #SwimData sd on sd.nodeId = MemberList.nodeId and sd.HasSubsOctToMar = 1
			Where	MemberTypes.Alias in ('swimSubs2')

Drop Table #SwimData

