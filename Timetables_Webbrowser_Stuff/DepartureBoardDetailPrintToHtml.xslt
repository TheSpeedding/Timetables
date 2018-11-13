<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" exclude-result-prefixes="msxsl xsi">
	
	<xsl:output method="html" indent="no"/>
	
	<xsl:template match="/">
		<html>			
			<head>
				<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
			</head>

			<body id="departure-board-type">
				<div class="departure">

					<span id="basic-info"></span>
					
					<div class="box detail">

						<h1>

							<xsl:attribute name="class">
								<xsl:value-of select="./Departure/MeanOfTransport/text()"/>
							</xsl:attribute>

							<xsl:attribute name="style">
								background-color: <xsl:value-of select="./Departure/LineColor/@Hex"/>;
							</xsl:attribute>

							<xsl:value-of select="./Departure/LineLabel/text()"/> · <xsl:value-of select="./Departure/Headsign/text()"/>

						</h1>

						<ol>
							<li>
								<span style="font-weight: bold;">
									<span class="iso8601">
										<xsl:value-of select="./Departure/DepartureDateTime/text()"/>
									</span>
									 ·
									<span class="station-id">
										<xsl:value-of select="./Departure/StopID/text()"/>
									</span>
								</span>
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
