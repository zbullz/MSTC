create table #swimdata
(NodeID int,
CreditsBought int,
CreditsUsed int)

Insert into #swimdata
Select CmsMember.NodeID, ISNULL(DataInt,0), 0 From (SELECT id FROM dbo.umbracoNode WHERE (nodeObjectType = '9b5416fb-e72f-45a9-a07b-5a9a2709ce43')) AS MemberTypeId 
					LEFT OUTER JOIN (SELECT nodeId, contentType FROM dbo.cmsContent) AS MemberList ON MemberList.contentType = MemberTypeId.id 
					LEFT OUTER JOIN dbo.cmsPropertyType AS MemberTypes ON MemberTypes.contentTypeId = MemberList.contentType 
					LEFT OUTER JOIN dbo.cmsPropertyData AS MemberDataTable ON MemberDataTable.contentNodeId = MemberList.nodeId 
						AND MemberDataTable.propertytypeid = MemberTypes.id 
					LEFT OUTER JOIN dbo.cmsMember AS CmsMember ON CmsMember.nodeId = MemberList.nodeId
					inner join dbo.umbracoNode n on n.id = MemberList.nodeId 
			Where	MemberTypes.Alias in ('swimCreditsBought') 
			--#swimdata
			
Update #swimdata
Set CreditsUsed = 
	ISNULL(DataInt,0) From (SELECT id FROM dbo.umbracoNode WHERE (nodeObjectType = '9b5416fb-e72f-45a9-a07b-5a9a2709ce43')) AS MemberTypeId 
					LEFT OUTER JOIN (SELECT nodeId, contentType FROM dbo.cmsContent) AS MemberList ON MemberList.contentType = MemberTypeId.id 
					LEFT OUTER JOIN dbo.cmsPropertyType AS MemberTypes ON MemberTypes.contentTypeId = MemberList.contentType 
					LEFT OUTER JOIN dbo.cmsPropertyData AS MemberDataTable ON MemberDataTable.contentNodeId = MemberList.nodeId 
						AND MemberDataTable.propertytypeid = MemberTypes.id 
					LEFT OUTER JOIN dbo.cmsMember AS CmsMember ON CmsMember.nodeId = MemberList.nodeId
					inner join dbo.umbracoNode n on n.id = MemberList.nodeId 
			Where	MemberTypes.Alias in ('swimCreditsUsed') 
			and CmsMember.NodeID = #swimdata.NodeID
			
Select * from #swimData

-- Set Credits remaining last year = Credits remaining
Update MemberDataTable
Set MemberDataTable.[dataInt] = (MemberDataTable.[dataInt] + #swimdata.CreditsBought - #swimdata.CreditsUsed)*3
							From (SELECT id FROM dbo.umbracoNode WHERE (nodeObjectType = '9b5416fb-e72f-45a9-a07b-5a9a2709ce43')) AS MemberTypeId 
						LEFT OUTER JOIN (SELECT nodeId, contentType FROM dbo.cmsContent) AS MemberList ON MemberList.contentType = MemberTypeId.id 
						LEFT OUTER JOIN dbo.cmsPropertyType AS MemberTypes ON MemberTypes.contentTypeId = MemberList.contentType 
						LEFT OUTER JOIN dbo.cmsPropertyData AS MemberDataTable ON MemberDataTable.contentNodeId = MemberList.nodeId 
							AND MemberDataTable.propertytypeid = MemberTypes.id 
						LEFT OUTER JOIN dbo.cmsMember AS CmsMember ON CmsMember.nodeId = MemberList.nodeId
						inner join dbo.umbracoNode n on n.id = MemberList.nodeId 
						inner join #swimdata on #swimdata.NodeID = CmsMember.NodeID
						Where	MemberTypes.Alias = 'swimCreditsRemaingLastYear'
						
-- Set Credits bought = 0				
Update MemberDataTable
Set MemberDataTable.[dataInt] = 0
							From (SELECT id FROM dbo.umbracoNode WHERE (nodeObjectType = '9b5416fb-e72f-45a9-a07b-5a9a2709ce43')) AS MemberTypeId 
						LEFT OUTER JOIN (SELECT nodeId, contentType FROM dbo.cmsContent) AS MemberList ON MemberList.contentType = MemberTypeId.id 
						LEFT OUTER JOIN dbo.cmsPropertyType AS MemberTypes ON MemberTypes.contentTypeId = MemberList.contentType 
						LEFT OUTER JOIN dbo.cmsPropertyData AS MemberDataTable ON MemberDataTable.contentNodeId = MemberList.nodeId 
							AND MemberDataTable.propertytypeid = MemberTypes.id 
						LEFT OUTER JOIN dbo.cmsMember AS CmsMember ON CmsMember.nodeId = MemberList.nodeId
						inner join dbo.umbracoNode n on n.id = MemberList.nodeId 
						inner join #swimdata on #swimdata.NodeID = CmsMember.NodeID
						Where	MemberTypes.Alias = 'swimCreditsBought'
						
-- Set Credits used = 0				
Update MemberDataTable
Set MemberDataTable.[dataInt] = 0
							From (SELECT id FROM dbo.umbracoNode WHERE (nodeObjectType = '9b5416fb-e72f-45a9-a07b-5a9a2709ce43')) AS MemberTypeId 
						LEFT OUTER JOIN (SELECT nodeId, contentType FROM dbo.cmsContent) AS MemberList ON MemberList.contentType = MemberTypeId.id 
						LEFT OUTER JOIN dbo.cmsPropertyType AS MemberTypes ON MemberTypes.contentTypeId = MemberList.contentType 
						LEFT OUTER JOIN dbo.cmsPropertyData AS MemberDataTable ON MemberDataTable.contentNodeId = MemberList.nodeId 
							AND MemberDataTable.propertytypeid = MemberTypes.id 
						LEFT OUTER JOIN dbo.cmsMember AS CmsMember ON CmsMember.nodeId = MemberList.nodeId
						inner join dbo.umbracoNode n on n.id = MemberList.nodeId 
						inner join #swimdata on #swimdata.NodeID = CmsMember.NodeID
						Where	MemberTypes.Alias = 'swimCreditsUsed'

drop table #swimData