<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [
  <!ENTITY nbsp "&#x00A0;">
]>
<!--
  Based on: MTT Ultimate Nav.
  Fixed for 4.5 schema. Tested in 4.5.2
  Params: 
    startNodeId - start menu construction from this node - can be CURRENT, PARENT or an id
    maxLevels - display this many levels of nodes in the menu (default = 1)
    skipFirstLevel - if 1, then the first level of nodes is ignored, and menu construction 
      will start at the children of the first node. This makes it work out of the box with the 
      Runway setup. (default = 1 - set to 0 if pages are all direct children of Content node)
    orientation - Superfish orientation value - blank creates a horizontal menu, other options
      are 'verical' and 'navbar' - see http://users.tpg.com.au/j_birch/plugins/superfish/#sample4
    customCssClass - custom CSS class. Will overwrite any of the defaults, so add 'sf-menu' etc. to your
      custom class string if you simply want to override the standard menu.
-->
<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxml="urn:schemas-microsoft-com:xslt"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  xmlns:umbraco.library="urn:umbraco.library"
  exclude-result-prefixes="msxml umbraco.library">

  <xsl:output method="html" omit-xml-declaration="yes"/>

  <xsl:param name="currentPage"/>

  <!-- Holds the start node for the navigation. Optional -->
  <xsl:param name="startNodeId" select="/macro/startNodeId"/>
  <!-- Holds number of sublevels to generate. Macro parameter is optional. Defaults to one if not supplied -->
  <xsl:param name="maxDrilldownLevel">
    <xsl:choose>
      <xsl:when test="string(/macro/maxLevels) != ''">
        <xsl:value-of select="/macro/maxLevels"/>
      </xsl:when>
      <xsl:otherwise>1</xsl:otherwise>
    </xsl:choose>
  </xsl:param>
  <!-- 
    If 1, then the first level will be skipped. This is the default so it can be plugged straight into 
    a Runway type site and display the main menu. The drill down level is applied from this new
    start level. Any site where all content is a child of the home page should use this.
  -->
  <xsl:param name="skipFirstLevel">
    <xsl:choose>
      <xsl:when test="string(/macro/skipFirstLevel) != ''">
        <xsl:value-of select="/macro/skipFirstLevel"/>
      </xsl:when>
      <xsl:otherwise>1</xsl:otherwise>
    </xsl:choose>
  </xsl:param>
  <!-- 
    Orientation class. If an orientation parameter is provided, sf-(orientation) will be added to
    non custom class lists. Orientations supported by Superfish are 'vertical' and 'navbar'. 
    Horizontal is the default and will be used if no orientation provided.
  -->
  <xsl:param name="orientationClass">
    <xsl:choose>
      <xsl:when test="string(/macro/orientation) != ''">
        <xsl:value-of select="concat(' sf-', /macro/orientation)"/>
      </xsl:when>
    </xsl:choose>
  </xsl:param>
  <!-- 
    Set up the CSS class for the main ul. If a cssClass parameter is provided, then only that will
    be used. 
  -->
  <xsl:param name="cssClass">
    <xsl:choose>
      <xsl:when test="string(/macro/customCssClass) != ''">
        <xsl:value-of select="/macro/customCssClass"/>
      </xsl:when>
      <xsl:otherwise>sf-menu<xsl:value-of select="$orientationClass"/></xsl:otherwise>
    </xsl:choose>
  </xsl:param>
  <!-- 
    If this is set to 1 or omitted then the top level menu item that the current page is descended
    from will be tagged with the class 'currentMenuItem'
  -->
  <xsl:param name="flagCurrent">
    <xsl:choose>
      <xsl:when test="string(/macro/flagCurrent) != ''">
        <xsl:value-of select="/macro/flagCurrent"/>
      </xsl:when>
      <xsl:otherwise>1</xsl:otherwise>
    </xsl:choose>
  </xsl:param>

  <!-- ================================================================================================
    TEMPLATES START HERE
    ================================================================================================ -->
  <xsl:template match="/">
    <!-- Check whether a start node has been supplied -->
    <xsl:choose>
      <!-- Special start node - use current page -->
      <xsl:when test="$startNodeId = 'CURRENT'">
        <xsl:call-template name="buildTopNavigation">
          <xsl:with-param name="navigationNodes" select="$currentPage"/>
        </xsl:call-template>        
      </xsl:when>
      <!-- Special start node - use parent -->
      <xsl:when test="$startNodeId = 'PARENT'">
        <xsl:call-template name="buildTopNavigation">
          <xsl:with-param name="navigationNodes" select="umbraco.library:GetXmlNodeById($currentPage/@parentID)"/>
        </xsl:call-template>        
      </xsl:when>
      <!-- Specified start node -->
      <xsl:when test="$startNodeId != ''">
        <xsl:call-template name="buildTopNavigation">
          <xsl:with-param name="navigationNodes" select="umbraco.library:GetXmlNodeById($startNodeId)"/>
        </xsl:call-template>        
      </xsl:when>
      <!-- Assume first level is single site root docuemnt, as in Runway; start from next level -->
      <xsl:when test="$skipFirstLevel = '1'">
        <xsl:call-template name="buildTopNavigation">
          <xsl:with-param name="navigationNodes" select="umbraco.library:GetXmlAll()/*[@isDoc]"/>
        </xsl:call-template>
      </xsl:when>
      <!-- Start building navigation from top level node -->
      <xsl:otherwise>
        <xsl:call-template name="buildTopNavigation">
          <xsl:with-param name="navigationNodes" select="umbraco.library:GetXmlAll()"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <!-- Start building the top navigation (first level navigation) -->
  <xsl:template name="buildTopNavigation">
    <xsl:param name="navigationNodes"/>

    <ul>
      <xsl:attribute name="class"><xsl:value-of select="$cssClass"/></xsl:attribute>

      <!-- Iterate child nodes -->
      <xsl:for-each select="$navigationNodes/child::*[@isDoc]">
	  
        <!-- Create var for easier reading/processing -->
        <xsl:variable name="currentProcessedNode" select="."/>
        <xsl:variable name="currentLevel" select="0"/>
		
		<xsl:variable name="numChildren">            
			<xsl:value-of select="count($currentProcessedNode/child::*[@isDoc])"/>
		</xsl:variable>
		
		
        <!-- Check whether node should be visible in first level navigation -->
        <xsl:if test="string($currentProcessedNode/umbracoNaviHide) != '1'">
			
			<xsl:variable name="matchedDescendants">            
				<xsl:value-of select="count($currentProcessedNode/descendant-or-self::*[@id = $currentPage/@id])"/>
			</xsl:variable>

          <li>
            <xsl:choose>
              <xsl:when test="($matchedDescendants &gt; 0) and ($flagCurrent = '1')">
                <xsl:attribute name="class">
                  currentMenuItem <xsl:value-of select="concat('mi-',$currentProcessedNode/@urlName)"/>
                </xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="class"><xsl:value-of select="concat('mi-', $currentProcessedNode/@urlName)"/></xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>
            
            <!-- Build the navigation link using the node currently being processed in the for-each loop -->
            <xsl:call-template name="buildLink">
              <xsl:with-param name="node" select="$currentProcessedNode"/>
              <xsl:with-param name="level" select="$currentLevel"/>
              <xsl:with-param name="hasChildren" select="$numChildren"/>
            </xsl:call-template>

            <!-- Build next level navigation only if applicable -->
            <!-- Still need to check whether all child nodes have been set to umbracoHideChildren = 1 whereas umbracoNaviHide = 0
                this case would yield an empty ul element -->
            <xsl:if test="(count($currentProcessedNode/*[@isDoc]) &gt; 0)
                      and (string($currentProcessedNode/umbracoNaviHide) != '1')
                      and ($currentLevel &lt; $maxDrilldownLevel)">
              <xsl:call-template name="buildNavigation">
                <xsl:with-param name="parentNode" select="$currentProcessedNode"/>
                <xsl:with-param name="level" select="$currentLevel + 1"/>
              </xsl:call-template>
            </xsl:if>

          </li>

        </xsl:if>

      </xsl:for-each>

    </ul>

  </xsl:template>

  <!-- A template used for building the non top navigation tree -->
  <xsl:template name="buildNavigation">
    <xsl:param name="parentNode"/>
    <xsl:param name="level"/>
    <xsl:param name="hasChildren"/>

    <ul>
      <!-- Iterate over the child nodes-->
      <xsl:for-each select="$parentNode/*[@isDoc]">

        <!-- Create var for easier reading/processing -->
        <xsl:variable name="currentProcessedNode" select="."/>

        <!-- Check whether node should be processed -->
        <xsl:if test="string($currentProcessedNode/umbracoNaviHide) != '1'">

          <li>
            <xsl:attribute name="class"><xsl:value-of select="concat('child mi-', $currentProcessedNode/@urlName)"/></xsl:attribute>

            <!-- Build the navigation link -->
            <xsl:call-template name="buildLink">
              <xsl:with-param name="node" select="$currentProcessedNode"/>
              <xsl:with-param name="level" select="$level"/>
            </xsl:call-template>

            <!-- Build next level navigation only if applicable; recursive call -->
            <!-- Still need to check whether all child nodes have been set to umbracoHideChildren = 1 whereas umbracoNaviHide = 0
                this case would yield an empty ul element -->
            <xsl:if test="
                    (count($currentProcessedNode/*[@isDoc]) &gt; 0) 
                      and (string($currentProcessedNode/umbracoHideChildren) != '1')
                      and ($level &lt; $maxDrilldownLevel)">
              <xsl:call-template name="buildNavigation">
                <xsl:with-param name="parentNode" select="$currentProcessedNode"/>
                <xsl:with-param name="level" select="$level + 1"/>
              </xsl:call-template>
            </xsl:if>

          </li>

        </xsl:if>

      </xsl:for-each>

    </ul>

  </xsl:template>

  <!-- A template that builds our navigation link based on node properties -->
  <xsl:template name="buildLink">
    <xsl:param name="node"/>
	<xsl:param name="level"/>
	<xsl:param name="hasChildren"/>

    <xsl:choose>

      <!-- Build link to external page -->
      <xsl:when test="string($node/externalURL) != ''">

        <xsl:call-template name="buildExternalLink">
          <xsl:with-param name="node" select="$node"/>
        </xsl:call-template>

      </xsl:when>

      <!-- Build link for redirecting to a custom supplied url -->
      <xsl:when test="string($node/umbracoRedirect) != ''">

        <xsl:call-template name="buildRedirectLink">
          <xsl:with-param name="node" select="$node"/>
        </xsl:call-template>

      </xsl:when>

      <!-- Default link builder -->
      <xsl:otherwise>

        <xsl:call-template name="buildNormalLink">
          <xsl:with-param name="node" select="$node"/>
          <xsl:with-param name="level" select="$level"/>
          <xsl:with-param name="hasChildren" select="$hasChildren"/>
        </xsl:call-template>

      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <!-- A template that builds a link to an external page -->
  <xsl:template name="buildExternalLink">
    <xsl:param name="node"/>

    <!--
    <xsl:call-template name ="outputNode">
      <xsl:with-param name="currentNode" select="$node"/>
    </xsl:call-template>
    -->

    <a>
      <!-- Set the href attribute -->
	  <xsl:attribute name="href">
        <xsl:value-of select="$node/externalURL"/>
      </xsl:attribute>
      <!-- Set the target attribute if available from the properties -->
      <xsl:if test="string($node/dataexternalTarget) != ''">
        <xsl:attribute name="target">
          <xsl:value-of select="$node/externalTarget"/>
        </xsl:attribute>
      </xsl:if>
      <!-- Set the title attribute if available from the properties -->
      <xsl:if test="string($node/navTooltip) != ''">
        <xsl:attribute name="title">
          <xsl:value-of select="string($node/navTooltip)"/>
        </xsl:attribute>
      </xsl:if>
      <!-- Set actual text for the link, either available from the properties or just plain umbraco link-->
      <xsl:choose>
        <xsl:when test="string($node/navText) != ''">
          <xsl:value-of select="string($node/navText)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$node/@nodeName"/>
        </xsl:otherwise>
      </xsl:choose>
    </a>

  </xsl:template>

  <xsl:template name="buildRedirectLink">
    <xsl:param name="node"/>

    <!--
    <xsl:call-template name ="outputNode">
      <xsl:with-param name="currentNode" select="$node"/>
    </xsl:call-template>
    -->

    <a>
      <!-- Set the href attribute based on the redirect supplied -->
      <xsl:attribute name="href">
        <xsl:value-of select="umbraco.library:NiceUrl($node/umbracoRedirect)"/>
      </xsl:attribute>
      <!-- Set the title attribute if available from the properties -->
      <xsl:if test="string($node/navTooltip) != ''">
        <xsl:attribute name="title">
          <xsl:value-of select="string($node/navTooltip)"/>
        </xsl:attribute>
      </xsl:if>
      <!-- Set actual text for the link, either available from the properties or just plain umbraco link-->
      <xsl:choose>
        <xsl:when test="string($node/navText) != ''">
          <xsl:value-of select="string($node/navText)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$node/@nodeName"/>
        </xsl:otherwise>
      </xsl:choose>
    </a>

  </xsl:template>

  <xsl:template name="buildNormalLink">
    <xsl:param name="node"/>
	<xsl:param name="level"/>
	<xsl:param name="hasChildren"/>

    <!--
    <xsl:call-template name ="outputNode">
      <xsl:with-param name="currentNode" select="$node"/>
    </xsl:call-template>
    -->

    <a>
      <!-- Set the href attribute, either the alias if available, else use NiceUrl() -->
      <xsl:attribute name="href">
        <xsl:choose>
          <xsl:when test="string($node/umbracoUrlAlias) != ''">
            <xsl:value-of select="string($node/umbracoUrlAlias)"/>
          </xsl:when>
          <xsl:otherwise>
			<xsl:choose>
				<xsl:when test="$level != '0' or $hasChildren = 0">
					<xsl:value-of select="umbraco.library:NiceUrl($node/@id)"/>
				</xsl:when>
				<xsl:otherwise>
					#
				</xsl:otherwise>
			</xsl:choose>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <!-- Set the title attribute if available from the properties -->
      <xsl:if test="string($node/navTooltip) != ''">
        <xsl:attribute name="title">
          <xsl:value-of select="string($node/navTooltip)"/>
        </xsl:attribute>
      </xsl:if>
      <!-- Set actual text for the link, either available from the properties or just plain umbraco link-->
      <xsl:choose>
        <xsl:when test="string($node/navText) != ''">
          <xsl:value-of select="string($node/navText)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$node/@nodeName"/>
        </xsl:otherwise>
      </xsl:choose>
    </a>

  </xsl:template>

  <!-- For debugging purposes, writes out all relevant node properties -->
  <xsl:template name="outputNode">
    <xsl:param name="currentNode"/>
    <ul>
      <li>
        @id=<xsl:value-of select="$currentNode/@id"/>
      </li>
      <li>
        @nodeName=<xsl:value-of select="$currentNode/@nodeName"/>
      </li>
      <li>
        @umbracoNaviHide=<xsl:value-of select="$currentNode/umbracoNaviHide"/>
      </li>
      <li>
        @umbracoHideChildren=<xsl:value-of select="$currentNode/umbracoHideChildren"/>
      </li>
      <li>
        @navText=<xsl:value-of select="$currentNode/navText"/>
      </li>
      <li>
        @navTooltip=<xsl:value-of select="$currentNode/navTooltip"/>
      </li>
      <li>
        @externalURL=<xsl:value-of select="$currentNode/externalURL"/>
      </li>
      <li>
        @externalTarget=<xsl:value-of select="$currentNode/externalTarget"/>
      </li>
      <li>
        @umbracoRedirect=<xsl:value-of select="$currentNode/umbracoRedirect"/>
      </li>
      <li>
        @umbracoUrlAlias=<xsl:value-of select="$currentNode/umbracoUrlAlias"/>
      </li>
    </ul>
  </xsl:template>

</xsl:stylesheet>
