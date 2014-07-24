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
  <xsl:variable name="numberOfPosts" select="10"/>

  <xsl:variable name="pageNumber">
    <xsl:choose>
      <xsl:when test="umbraco.library:RequestQueryString('page') &lt;= 0 or string(umbraco.library:RequestQueryString('page')) = '' or string(umbraco.library:RequestQueryString('page')) = 'NaN'">1</xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="umbraco.library:RequestQueryString('page')"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <xsl:variable name="filter">
    <xsl:choose>
      <xsl:when test="string-length(umbraco.library:Request('filterby')) &gt; 0">
        <xsl:value-of select="umbraco.library:Request('filterby')"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <xsl:variable name="numberOfRecords">
    <xsl:choose>
      <xsl:when test="$filter = ''">
        <xsl:value-of select="count($currentPage/ancestor-or-self::Blog//BlogPost)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="count($currentPage/ancestor-or-self::Blog//BlogPost [contains(Exslt.ExsltStrings:lowercase(./tags), Exslt.ExsltStrings:lowercase($filter))])"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <xsl:template match="/">


    <xsl:if test="$filter != ''">
      <h2 class="page-title">
        Archive for tag:
        <span>
          <xsl:value-of select="$filter"/>
        </span>
      </h2>
    </xsl:if>
    
    <xsl:if test="$currentPage/@nodeTypeAlias = 'DateFolder'">
      <h2 class="page-title">
        Monthly Archives: <xsl:value-of select="umbraco.library:FormatDateTime(concat($currentPage/../@nodeName,'-',$currentPage/@nodeName,'-11T10:24:46'),'MMMM yyyy')"/>
      </h2>
    </xsl:if>

    <xsl:if test="$numberOfRecords &gt; $numberOfPosts">
		<h2 class="page-title">
			News
		</h2>
		
	<div id="nav-above" class="navigation">

      <div class="nav-previous">
        <xsl:if test="(($pageNumber ) * $numberOfPosts) &lt; ($numberOfRecords)">
          <span class="meta-nav">&#171; </span>
          <a href="?page={$pageNumber +1}">Older posts</a>
        </xsl:if>
      </div>

      <div class="nav-next">
        <xsl:if test="$pageNumber &gt; 1">
          <a href="?page={$pageNumber -1}">Newer posts</a>
          <span class="meta-nav"> &#187;</span>
        </xsl:if>
      </div>

    </div>
    </xsl:if>
    
    <xsl:if test="$filter = ''">
      
      <xsl:for-each select="$currentPage/ancestor-or-self::Blog//BlogPost">
        <xsl:sort select="./PostDate" order="descending" />

        <xsl:if test="position() &gt; $numberOfPosts * (number($pageNumber)-1) and
        position() &lt;= number($numberOfPosts * (number($pageNumber)-1) +
        $numberOfPosts )">
          
          <xsl:call-template name="showpost">
            <xsl:with-param name="post" select="."/>
          </xsl:call-template>
      
        </xsl:if>
        
       </xsl:for-each>
    </xsl:if>

    <xsl:if test="$filter != ''">

      <xsl:for-each select="$currentPage/ancestor-or-self::Blog//BlogPost [contains(Exslt.ExsltStrings:lowercase(./tags), Exslt.ExsltStrings:lowercase($filter))]">
        <xsl:sort select="./PostDate" order="descending" />
        <xsl:if test="position() &gt; $numberOfPosts * (number($pageNumber)-1) and
        position() &lt;= number($numberOfPosts * (number($pageNumber)-1) +
        $numberOfPosts )">
          <xsl:call-template name="showpost">
            <xsl:with-param name="post" select="."/>
          </xsl:call-template>
        </xsl:if>
      </xsl:for-each>
    </xsl:if>

    <div id="nav-below" class="navigation">

      <div class="nav-previous">
        <xsl:if test="(($pageNumber ) * $numberOfPosts) &lt; ($numberOfRecords)">
          <span class="meta-nav">&#171; </span>
          <a href="?page={$pageNumber +1}">Older posts</a>
        </xsl:if>
      </div>

      <div class="nav-next">
        <xsl:if test="$pageNumber &gt; 1">
          <a href="?page={$pageNumber -1}">Newer posts</a>
          <span class="meta-nav"> &#187;</span>
        </xsl:if>
      </div>

    </div>


  </xsl:template>

  <xsl:template name="showpost">
    <xsl:param name="post"/>

    <div class="blog-post blog-single-post">
      <div class="single-post-title">
        <h3>
          <xsl:value-of select="$post/@nodeName"/>
        </h3>
      </div>
      <div class="single-post-info">
        <i class="glyphicon glyphicon-time"></i><xsl:value-of select="umbraco.library:LongDate($post/PostDate)"/>, <xsl:value-of select="$post/PostAuthor"/>
      </div>
      <div class="single-post-image">
        <xsl:if test="string($post/newsPhoto) != ''">
          <xsl:variable name="image" select="umbraco.library:GetMedia($post/newsPhoto, 0)"/>
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
      </div>
      <div class="single-post-content">
        <div class="entry-content">
          <xsl:value-of select="$post/bodyText" disable-output-escaping="yes"/>
        </div>
      </div>
      <div class="entry-meta">
        <span class="author vcard">
          By: <span class="author">
            <xsl:value-of select="$post/PostAuthor"/>
          </span>
        </span>
        <span class="meta-sep"> |</span>
        <span class="tag-links">
          <xsl:variable name="tags" select="tagsLib:getTagsFromNode(@id)" />
          <xsl:choose>
            <xsl:when test="count($tags/tags/tag) = 0">
              Not tagged
            </xsl:when>
            <xsl:otherwise>
              Tagged:
              <xsl:for-each select="$tags/tags/tag">
                <a href="{umbraco.library:NiceUrl($currentPage/ancestor-or-self::Blog/@id)}?filterby={.}" rel="tag">
                  <xsl:value-of select="."/>
                </a>
                <xsl:if test="position() != last()">, </xsl:if>
              </xsl:for-each>
            </xsl:otherwise>
          </xsl:choose>
        </span>
      </div>
    </div>
    
  </xsl:template>
</xsl:stylesheet>