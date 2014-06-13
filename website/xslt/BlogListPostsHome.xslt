<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [
  <!ENTITY nbsp "&#x00A0;">
]>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxml="urn:schemas-microsoft-com:xslt"
	xmlns:umbraco.library="urn:umbraco.library"
	xmlns:tagsLib="urn:tagsLib"
	xmlns:Exslt.ExsltStrings="urn:Exslt.ExsltStrings"
	xmlns:BlogLibrary="urn:BlogLibrary"
	exclude-result-prefixes="msxml umbraco.library tagsLib Exslt.ExsltStrings BlogLibrary">


  <xsl:output method="html" omit-xml-declaration="yes"/>

 <xsl:param name="currentPage"/>
 
	<xsl:variable name="newsSource" select="umbraco.library:GetXmlNodeById(1127)" />

  <xsl:template match="/">
      <xsl:for-each select="$newsSource//BlogPost">
        <xsl:sort select="./PostDate" order="descending" />
        <xsl:if test="position()&lt;= 7">
          <div class="newsitem">
				<h3 class="home-news-title"><a href="{umbraco.library:NiceUrl(@id)}"><xsl:value-of select="@nodeName" /></a></h3>
				<span class="home-news-date"><xsl:value-of select="umbraco.library:FormatDateTime(PostDate, 'dd MMM yyyy')"/></span>
          </div>
        </xsl:if>
      </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>