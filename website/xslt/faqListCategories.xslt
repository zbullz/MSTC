<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [
    <!ENTITY nbsp "&#x00A0;">
]>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxml="urn:schemas-microsoft-com:xslt"
  xmlns:umbraco.library="urn:umbraco.library"
  exclude-result-prefixes="msxml umbraco.library">

    <xsl:variable name="title" select="/macro/title"/>
    <xsl:output method="xml" omit-xml-declaration="yes"/>

    <xsl:param name="currentPage"/>

    <xsl:template match="/">

        <xsl:choose>
            <xsl:when test="count($currentPage/faqCategory)&lt;=0">
                <div id="NoComments">
                    There are no comments yet... be the first to make one ...
                </div>
            </xsl:when>
            <xsl:otherwise>

                <xsl:if test="$title!=''">
                    <h3>
                        <xsl:value-of select="$title"/>
                    </h3>
                </xsl:if>

                <xsl:for-each select="$currentPage/faqCategory">
                    <a href="{umbraco.library:NiceUrl(@id)}">
                        <xsl:value-of select="@nodeName"/>
                    </a> (<xsl:value-of select="count(faqQuestion)"/>)<br/>
                    <xsl:value-of select="bodyText" disable-output-escaping="yes"/>
                </xsl:for-each>


            </xsl:otherwise>
        </xsl:choose>


    </xsl:template>

</xsl:stylesheet>