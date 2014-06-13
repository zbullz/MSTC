<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [ <!ENTITY nbsp "&#x00A0;"> ]>
<xsl:stylesheet 
  version="1.0" 
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
  xmlns:msxml="urn:schemas-microsoft-com:xslt"
  xmlns:umbraco.library="urn:umbraco.library" xmlns:Exslt.ExsltCommon="urn:Exslt.ExsltCommon" xmlns:Exslt.ExsltDatesAndTimes="urn:Exslt.ExsltDatesAndTimes" xmlns:Exslt.ExsltMath="urn:Exslt.ExsltMath" xmlns:Exslt.ExsltRegularExpressions="urn:Exslt.ExsltRegularExpressions" xmlns:Exslt.ExsltStrings="urn:Exslt.ExsltStrings" xmlns:Exslt.ExsltSets="urn:Exslt.ExsltSets" 
  exclude-result-prefixes="msxml umbraco.library Exslt.ExsltCommon Exslt.ExsltDatesAndTimes Exslt.ExsltMath Exslt.ExsltRegularExpressions Exslt.ExsltStrings Exslt.ExsltSets ">


<xsl:output method="xml" omit-xml-declaration="yes"/>

<xsl:param name="currentPage"/>

<xsl:template match="/">

<xsl:variable name="debugOptions" select="umbraco.library:RequestQueryString('cfDebug')"/>
<xsl:variable name="nodeId" select="umbraco.library:RequestQueryString('cfDebug_nodeId')"/>
  
  <xsl:if test="$debugOptions != ''">
    <iframe id="cfDebug" style="width:100%;height:600px;background:white">&nbsp;</iframe>
     <xsl:choose>
       <xsl:when test="$debugOptions = 'xml'">
         <div id="cfDebugContent" style="display:none"><xmp><xsl:copy-of select="umbraco.library:GetXmlAll()" /></xmp></div>
         <xsl:call-template name="dumpScript"/>
       </xsl:when>
       <xsl:when test="$debugOptions = 'xmlCurrent'">
         <div id="cfDebugContent" style="display:none"><xmp><xsl:copy-of select="$currentPage" /></xmp></div>
         <xsl:call-template name="dumpScript"/>
       </xsl:when>
       <xsl:when test="$debugOptions = 'xmlNode'">
         <div id="cfDebugContent" style="display:none"><xmp><xsl:copy-of select="umbraco.library:GetXmlNodeById($nodeId)" /></xmp></div>
         <xsl:call-template name="dumpScript"/>
       </xsl:when>
    </xsl:choose>
  </xsl:if>

</xsl:template>

<xsl:template name="dumpScript">
   <script language="Javascript" type="text/javascript">
     var xml = document.getElementById('cfDebugContent');
     var frame = document.getElementById('cfDebug');         
     var doc = frame.contentDocument ? frame.contentDocument : frame.contentWindow.document;
     //if(doc)
     {
       doc.open();
       doc.write(xml.innerHTML);
       doc.close();
     }
   </script>
</xsl:template>

</xsl:stylesheet>