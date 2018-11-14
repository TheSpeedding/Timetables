<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" exclude-result-prefixes="msxsl xsi">
	<xsl:output method="html" indent="no"/>
	<xsl:template match="/">
		<html>
			<head>
				<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
			</head>
			<body id="journey-type">
				<span id="basic-info"></span>				
				<div class="journey">
					<xsl:for-each select="./Journey/JourneySegments/JourneySegment">
						<div class="box detail">								
							<xsl:choose>
								<xsl:when test="@xsi:type = 'FootpathSegment'">
									<h1>
										<span class="transfer-constant"></span>
										 ·
										<span class="total-duration">
											<span class="departure-from-source">
												<xsl:value-of select="./DepartureDateTime/text()"/>
											</span>
											<span class="arrival-to-target">
												<xsl:value-of select="./ArrivalDateTime/text()"/>
											</span>
										</span>
									</h1>
								</xsl:when>
								<xsl:otherwise>													
									<h1>
										<xsl:attribute name="class">
											<xsl:value-of select="./MeanOfTransport/text()"/>
										</xsl:attribute>
										<xsl:attribute name="style">
											background-color: <xsl:value-of select="./LineColor/@Hex"/>;
										</xsl:attribute>
										<xsl:value-of select="./LineLabel/text()"/> · <xsl:value-of select="./Headsign/text()"/>
									</h1>										
								</xsl:otherwise>
							</xsl:choose>								

							<xsl:if test="@xsi:type = 'TripSegment'">
								<ol>
									<li>
										<span style="font-weight: bold;">
											<span class="iso8601">
												<xsl:value-of select="./DepartureDateTime/text()"/>
											</span>
											 ·
											<span class="station-id">
												<xsl:value-of select="./SourceStopID/text()"/>
											</span>
										</span>
									</li>
									<xsl:for-each select="./IntermediateStops/IntermediateStop">
										<li>
											<span class="iso8601">
												<xsl:value-of select="./Arrival/text()"/>
											</span>
											 ·
											<span class="station-id">
												<xsl:value-of select="./StopID/text()"/>
											</span>
										</li>
									</xsl:for-each>
									<li>
										<span style="font-weight: bold;">
											<span class="iso8601">
												<xsl:value-of select="./ArrivalDateTime/text()"/>
											</span>
											 ·
											<span class="station-id">
												<xsl:value-of select="./TargetStopID/text()"/>
											</span>
										</span>
									</li>
								</ol>
							</xsl:if>
						</div>
					</xsl:for-each>
				</div>					
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
