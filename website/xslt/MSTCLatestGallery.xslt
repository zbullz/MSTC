<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [ <!ENTITY nbsp "&#x00A0;"> ]>
<xsl:stylesheet 
  version="1.0" 
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
  xmlns:msxml="urn:schemas-microsoft-com:xslt"
  xmlns:umbraco.library="urn:umbraco.library" xmlns:Exslt.ExsltCommon="urn:Exslt.ExsltCommon" xmlns:Exslt.ExsltDatesAndTimes="urn:Exslt.ExsltDatesAndTimes" xmlns:Exslt.ExsltMath="urn:Exslt.ExsltMath" xmlns:Exslt.ExsltRegularExpressions="urn:Exslt.ExsltRegularExpressions" xmlns:Exslt.ExsltStrings="urn:Exslt.ExsltStrings" xmlns:Exslt.ExsltSets="urn:Exslt.ExsltSets" xmlns:tagsLib="urn:tagsLib" xmlns:BlogLibrary="urn:BlogLibrary" 
  exclude-result-prefixes="msxml umbraco.library Exslt.ExsltCommon Exslt.ExsltDatesAndTimes Exslt.ExsltMath Exslt.ExsltRegularExpressions Exslt.ExsltStrings Exslt.ExsltSets tagsLib BlogLibrary ">


<xsl:output method="xml" omit-xml-declaration="yes"/>

<xsl:param name="currentPage"/>
<xsl:param name="galleryRootId">
  <xsl:choose>
    <xsl:when test="/macro/galleryRootId != ''">
      <xsl:value-of select="/macro/galleryRootId" />
    </xsl:when>
    <xsl:otherwise>1257</xsl:otherwise>
  </xsl:choose>
</xsl:param>
<xsl:param name="galleryDisplayPage">
  <xsl:choose>
    <xsl:when test="/macro/galleryDisplayPage != ''">
      <xsl:value-of select="/macro/galleryDisplayPage" />
    </xsl:when>
    <xsl:otherwise>/photos/gallery.aspx</xsl:otherwise>
  </xsl:choose>
</xsl:param>

<xsl:template match="/">

  <xsl:for-each select="umbraco.library:GetMedia($galleryRootId, 1)/Folder[position() &lt;= 3]">
    <xsl:call-template name="displayGallery">
      <xsl:with-param name="gallery" select="."/>
    </xsl:call-template>
  </xsl:for-each>  

</xsl:template>

<xsl:template name="displayGallery">
  <xsl:param name="gallery"/>
  
  <xsl:variable name="count" select="count($gallery/Image)"/>
    <a href="{$galleryDisplayPage}?galleryId={$gallery/@id}">
      <xsl:for-each select="$gallery/Image[position() &lt;= 2]">
        <img class="pos{position()}" src="/umbraco/ImageGen.ashx?width=60&amp;height=60&amp;crop=1&amp;image={umbracoFile}"/>
      </xsl:for-each>
    </a>
</xsl:template>
      
</xsl:stylesheet>
