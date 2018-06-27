﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" exclude-result-prefixes="msxsl xsi">
	<xsl:output method="html" indent="yes"/>
	<xsl:template match="/">
		<html>
			<head>
				<script src="TransformationHelperFunctions.js"></script>
				<title>Journeys (<xsl:value-of select="count(//Journey)"/>) - <xsl:value-of select="//Journey[position() = 1]/JourneySegments/JourneySegment[position() = 1]/SourceStopName/text()"/> - <xsl:value-of select="//Journey[position() = 1]/JourneySegments/JourneySegment[position() = last()]/TargetStopName/text()"/></title>
			</head>
			<body>
				<xsl:for-each select="//Journey">
					<div class="journey">
						<div class="leaves-in">
							<script>javascript: document.write(leavingTimeText('<xsl:value-of select="./JourneySegments/JourneySegment[position() = 1]/DepartureDateTime/text()"/>'));</script>
						</div>
						<ul class="tools">
							<li>
								<a href="#">Print</a>
							</li>
							<li>
								<a href="#">Show details</a>
							</li>
						</ul>
						<div class="box">
							<div class="duration">
								<script>javascript: document.write(getDuration('<xsl:value-of select="./JourneySegments/JourneySegment[position() = 1]/DepartureDateTime/text()"/>', '<xsl:value-of select="./JourneySegments/JourneySegment[position() = last()]/ArrivalDateTime/text()"/>'));</script>
							</div>
							<div class="transfers">
								<script>javascript: document.write(totalTransfers(<xsl:value-of select="count(./JourneySegments/JourneySegment[@xsi:type = 'TripSegment'])"/>));</script>
							</div>
							<div class="outdated">outdated</div>
							<div class="departure">
								<div class="time">
									<script>javascript: document.write(parseDateTime('<xsl:value-of select="./JourneySegments/JourneySegment[position() = 1]/DepartureDateTime/text()"/>').toLocaleTimeString());</script>
								</div>
								<div class="station"><xsl:value-of select="./JourneySegments/JourneySegment[position() = 1]/SourceStopName/text()"/></div>
							</div>
							<ol class="segments">
								<xsl:for-each select="./JourneySegments/JourneySegment">
									<xsl:choose>
										<xsl:when test="not(./MeanOfTransport)">
											<li class="Footpath"/>
										</xsl:when>
										<xsl:otherwise>
											<li>
												<xsl:attribute name="class">
													<xsl:value-of select="./MeanOfTransport/text()"/>
												</xsl:attribute>
												<xsl:attribute name="title">
													<xsl:value-of select="./SourceStopName/text()"/>
													<xsl:text> - </xsl:text>
													<xsl:value-of select="./TargetStopName/text()"/>
												</xsl:attribute>
												<xsl:value-of select="./LineLabel/text()"/>
											</li>
										</xsl:otherwise>									
									</xsl:choose>
								</xsl:for-each>
							</ol>
							<div class="arrival">
								<div class="time">
									<script>javascript: document.write(parseDateTime(<xsl:value-of select="./JourneySegments/JourneySegment[position() = last()]/ArrivalDateTime/text()"/>').toLocaleTimeString());</script>
								</div>
								<div class="station"><xsl:value-of select="./JourneySegments/JourneySegment[position() = last()]/TargetStopName/text()"/></div>
							</div>
						</div>
					</div>
				</xsl:for-each>
			</body>
		</html>
	</xsl:template>	
</xsl:stylesheet>
