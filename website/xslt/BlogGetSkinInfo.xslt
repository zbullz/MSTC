<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [
  <!ENTITY nbsp "&#x00A0;">
]>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxml="urn:schemas-microsoft-com:xslt"
	xmlns:umbraco.library="urn:umbraco.library" xmlns:Exslt.ExsltCommon="urn:Exslt.ExsltCommon" xmlns:Exslt.ExsltDatesAndTimes="urn:Exslt.ExsltDatesAndTimes" xmlns:Exslt.ExsltMath="urn:Exslt.ExsltMath" xmlns:Exslt.ExsltRegularExpressions="urn:Exslt.ExsltRegularExpressions" xmlns:Exslt.ExsltStrings="urn:Exslt.ExsltStrings" xmlns:Exslt.ExsltSets="urn:Exslt.ExsltSets" xmlns:umbraco.contour="urn:umbraco.contour" xmlns:tagsLib="urn:tagsLib" xmlns:BlogLibrary="urn:BlogLibrary"
	exclude-result-prefixes="msxml umbraco.library Exslt.ExsltCommon Exslt.ExsltDatesAndTimes Exslt.ExsltMath Exslt.ExsltRegularExpressions Exslt.ExsltStrings Exslt.ExsltSets umbraco.contour tagsLib BlogLibrary ">


  <xsl:output method="xml" omit-xml-declaration="yes"/>

  <xsl:param name="currentPage"/>

  <xsl:template match="/">

    <xsl:variable name="skin" select="BlogLibrary:GetSkin($currentPage/ancestor-or-self::Blog/css)"/>


    <xsl:choose>
      <xsl:when test="count($skin//description) &gt; 0">
        <a href="{$skin//url}" title="{$skin//title} by {$skin//name}" rel="{$skin//name}">
          <xsl:value-of select="$skin//title"/> by <xsl:value-of select="$skin//name"/>
        </a>
      </xsl:when>
      <xsl:otherwise>
        <a href="http://www.plaintxt.org/themes/sandbox/" title="Sandbox theme for umbraco" rel="designer">Sandbox</a>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>