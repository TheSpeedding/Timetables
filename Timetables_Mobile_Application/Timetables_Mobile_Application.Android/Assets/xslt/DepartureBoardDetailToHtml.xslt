<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" exclude-result-prefixes="msxsl xsi">
	<xsl:output method="html" indent="no"/>	
	<xsl:template match="/">		
		<html>			
			<head>
				<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
			</head>
			<body>
				<div class="departure">
					<div class="leaves-in">
						<xsl:if test="./Outdated[text() = 'true']">
							<!-- Indicates whether the trip uses outdated timetables. -->
							<span class="outdated"></span>
						</xsl:if>
						<!-- Writes the relative time that the trip leaves in. -->
						<span class="leaving-time">
							<xsl:value-of select="./Departure/DepartureDateTime/text()"/>
						</span>
					</div>
					<!-- Links to other windows. -->
					<ul class="tools">
						<li>
							<a class="map-link" href="#"></a>
						</li>
						<li>
							<a class="print-link" href="#"></a>
						</li>
					</ul>
					<div class="basic">
						<div class="box">
							<div class="label">
								<span>
									<xsl:attribute name="class">
										<xsl:value-of select="./Departure/MeanOfTransport/text()"/>
									</xsl:attribute>
									<xsl:attribute name="style">
										background-color: <xsl:value-of select="./Departure/LineColor/@Hex"/>;
										color: <xsl:value-of select="./Departure/LineTextColor/@Hex"/>;
									</xsl:attribute>
									<xsl:value-of select="./Departure/LineLabel/text()"/>
								</span>
							</div>
							<div class="headsign">
								<xsl:value-of select="./Departure/Headsign/text()"/>
							</div>
							<div class="time">
								<span class="iso8601">
									<xsl:value-of select="./Departure/DepartureDateTime/text()"/>
								</span>
							</div>
						</div>
						<hr/>
					</div>
					<div class="box detail">
						<h1>
							<xsl:attribute name="class">
								<xsl:value-of select="./Departure/MeanOfTransport/text()"/>
							</xsl:attribute>
							<xsl:attribute name="style">
								background-color: <xsl:value-of select="./Departure/LineColor/@Hex"/>;
								color: <xsl:value-of select="./Departure/LineTextColor/@Hex"/>;
							</xsl:attribute>
							<xsl:value-of select="./Departure/LineLabel/text()"/> · <xsl:value-of select="./Departure/Headsign/text()"/>
						</h1>
						<ol>
							<li>
								<div style="font-weight: bold;">
									<span class="iso8601">
										<xsl:value-of select="./Departure/DepartureDateTime/text()"/>
									</span>
										· 
									<span class="station-id">
										<xsl:value-of select="./Departure/StopID/text()"/>
									</span>
								</div>
							</li>
							<xsl:for-each select="./Departure/IntermediateStops/IntermediateStop">
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
						</ol>
					</div>
				</div>
			</body>
		</html>
	</xsl:template>	
</xsl:stylesheet>
