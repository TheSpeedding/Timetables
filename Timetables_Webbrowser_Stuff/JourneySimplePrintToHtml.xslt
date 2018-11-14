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
				<xsl:for-each select="//Journey">
					<div class="journey">						
						<div class="box">
							<div class="main">
								<!-- Writes info about source station, i.e. leaving time and its name. -->
								<div class="departure">
									<div class="time">
										<span class="iso8601">
											<xsl:value-of select="./JourneySegments/JourneySegment[position() = 1]/DepartureDateTime/text()"/>
										</span>
									</div>
									<div class="station">
										<span class="station-id">
											<xsl:value-of select="./JourneySegments/JourneySegment[position() = 1]/SourceStopID/text()"/>
										</span>
									</div>
								</div>
								<!-- Writes information about the journey, i.e. its segments. -->
								<ol class="segments">
									<xsl:for-each select="./JourneySegments/JourneySegment">
										<xsl:choose>
											<xsl:when test="@xsi:type = 'FootpathSegment'">
											</xsl:when>
											<xsl:otherwise>
												<li>
													<xsl:attribute name="class">
														<xsl:value-of select="./MeanOfTransport/text()"/>
													</xsl:attribute>
													<xsl:attribute name="style">
														background-color: <xsl:value-of select="./LineColor/@Hex"/>;
													</xsl:attribute>
													<xsl:value-of select="./LineLabel/text()"/>
												</li>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:for-each>
								</ol>
								<!-- Writes info about target station, i.e. departure time and its name. -->
								<div class="arrival">
									<div class="time">
										<span class="iso8601">
											<xsl:value-of select="./JourneySegments/JourneySegment[position() = last()]/ArrivalDateTime/text()"/>
										</span>
									</div>
									<div class="station">
										<span class="station-id">
											<xsl:value-of select="./JourneySegments/JourneySegment[position() = last()]/TargetStopID/text()"/>
										</span>
									</div>
								</div>
							</div>												
						</div>
					</div>
				</xsl:for-each>
			</body>
		</html>
	</xsl:template>	
</xsl:stylesheet>