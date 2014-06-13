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


    <xsl:output method="xml" omit-xml-declaration="yes"/>

    <xsl:param name="currentPage"/>
    <xsl:variable name="animation" select="/macro/animation"/>
    <xsl:variable name="title" select="/macro/title"/>

    <xsl:template match="/">
        <xsl:if test="count($currentPage/faqQuestion)&gt;0">
            <xsl:if test="$title!=''">
                <h3>
                    <xsl:value-of select="$title"/>
                </h3>
            </xsl:if>
            <xsl:value-of select="umbraco.library:AddJquery()"/>
            <xsl:if test="$animation=1">
                <script type="text/javascript">
                    $(document).ready(function(){
                    $(".faqQuestion div").hide();
                    $(".faqQuestion").click(function () {
                    $(this).find("div.answer").toggle("fast");


                    });

                    });
                </script>
            </xsl:if>
            <xsl:for-each select="$currentPage/faqQuestion">
                <div class="faqQuestion">
                    <span>
                        <strong>
                            <xsl:value-of select="umbraco.library:RemoveFirstParagraphTag(questionText)" disable-output-escaping="yes"/>
                        </strong>
                    </span>
                    <div class="answer">
                        <xsl:value-of select="bodyText" disable-output-escaping="yes"/>
                    </div>
                </div>
            </xsl:for-each>
        </xsl:if>
    </xsl:template>

</xsl:stylesheet>