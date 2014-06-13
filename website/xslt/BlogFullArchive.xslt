<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:msxml="urn:schemas-microsoft-com:xslt"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
xmlns:math="http://exslt.org/math"
xmlns:umbraco.library="urn:umbraco.library"
xmlns:yourprefix="urn:my-scripts"
exclude-result-prefixes="msxml yourprefix umbraco.library">


    <xsl:output method="html" omit-xml-declaration="yes"/>

    <xsl:param name="currentPage"/>

    <msxsl:script implements-prefix='yourprefix' language='CSharp'>
        <![CDATA[
public string monthName(int monthNum)
{
DateTime date = new DateTime(1,monthNum,1);
return date.ToString("MMMM");
}
]]>

    </msxsl:script>

    <xsl:template match="/">
        <div id="archieve">

            <xsl:for-each select="$currentPage/ancestor-or-self::Blog/DateFolder">
                <xsl:sort select="number(@nodeName)" data-type="number" order="descending"/>
                <h3>
                    <xsl:value-of select="@nodeName"/>
                </h3>
                <div class="tab">
                    <xsl:for-each select="./DateFolder">
                        <xsl:sort select="number(@nodeName)" data-type="number" order="descending"/>
                        <h4>
                            <xsl:value-of select="yourprefix:monthName(number(@nodeName))"/>
                        </h4>
                        <ul>
                            <xsl:for-each select=".//BlogPost">
                                <li>
                                    <a href="{umbraco.library:NiceUrl(@id)}">
                                        <xsl:value-of select="@nodeName"/>
                                    </a>
                                    <br/>
                                    <small>
                                        Posted: <xsl:value-of select="umbraco.library:LongDate(PostDate)"/>
                                        By: <xsl:value-of select="@writerName"/>
                                    </small>
                                </li>

                            </xsl:for-each>
                        </ul>
                    </xsl:for-each>
                </div>

            </xsl:for-each>
        </div>

    </xsl:template>

</xsl:stylesheet>