using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class MemberServiceDto
{
	public string Name { get; set; }
	public string ServiceLinkAddress { get; set; }
	public string ServiceLinkText { get; set; }
	public int? ServiceImageId { get; set; }
	public string ServiceDescription { get; set; }
}