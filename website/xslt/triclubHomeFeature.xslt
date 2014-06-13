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
        <xsl:if test="position()&lt;= 5">
      <div class="ui-tabs-panel" style="">
        <xsl:attribute name="id">
           <xsl:value-of select="concat('fragment-', position())" />
         </xsl:attribute>
                
        <xsl:if test="string(newsPhoto) != ''">
          <xsl:variable name="image" select="umbraco.library:GetMedia(newsPhoto, 0)"/>
			<img>
				<xsl:attribute name="src">
					<xsl:text>igen.axd?vpath=</xsl:text>
					<xsl:value-of select="$image/umbracoFile" />
					<xsl:text>&amp;size=homeFeature</xsl:text>
				</xsl:attribute>
				<xsl:attribute name="alt">
					<xsl:value-of select="$image/@nodeName" />
				</xsl:attribute>
			</img>
        </xsl:if>
		<xsl:if test="string(newsPhoto) = ''">

			<img src="/images/muppetsride.jpg" alt="Muppets Ride" />

		</xsl:if>
        <div class="info" >
          <h2><a href="{umbraco.library:NiceUrl(@id)}"><xsl:value-of select="@nodeName" /></a></h2>
          <p>
            <xsl:value-of select="newsPrecis" />
          </p>
        </div>
      </div>
        </xsl:if>
      </xsl:for-each>
    <div class="ui-tabs-nav">
            <ul class="ui-tabs-nav">
        <li class="img-cover">
          &nbsp;
        </li>
    <xsl:for-each select="$newsSource//BlogPost">
        <xsl:sort select="./../../../@sortOrder" order="descending" />
        <xsl:sort select="./../../@sortOrder" order="descending" />
        <xsl:sort select="./../@nodeName" order="descending" />
        <xsl:sort select="@sortOrder" order="descending" />
        <xsl:if test="position()&lt;= 5">
            <li class="ui-tabs-nav-item ui-tabs-selected" id="nav-fragment-1">
            <xsl:attribute name="id">
               <xsl:value-of select="concat('fragment-', position())" />
           </xsl:attribute>
                <a>
                  <xsl:attribute name="href">
                   <xsl:value-of select="concat('#fragment-', position())" />
                   </xsl:attribute>
                  <xsl:value-of select="position()" />
               </a>
              </li>
        </xsl:if>
      </xsl:for-each>
          </ul>
        </div>
  </xsl:template>
</xsl:stylesheet>