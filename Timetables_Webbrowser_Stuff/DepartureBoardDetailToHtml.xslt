﻿<?xml version="1.0" encoding="utf-8"?>

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
							<span class="outdated">
								<script>javascript: document.write(window.external.OutdatedStringConstant());</script>
							</span>
						</xsl:if>
						<!-- Writes the relative time that the trip leaves in. -->
						<script>
							javascript: document.write(window.external.LeavingTimeToString('<xsl:value-of select="./Departure/DepartureDateTime/text()"/>'));
						</script>
					</div>

					<!-- Links to other windows. -->
					<ul class="tools">
						<li>
							<a href="#">
								<script>javascript: document.write(window.external.MapStringConstant());</script>
							</a>
						</li>
						<li>
							<a href="#">
								<script>javascript: document.write(window.external.PrintStringConstant());</script>
							</a>
						</li>
					</ul>

					<div class="box">

						<div class="label">
							<span>
								<xsl:attribute name="class">
									<xsl:value-of select="./Departure/MeanOfTransport/text()"/>
								</xsl:attribute>

								<xsl:attribute name="style">
									background-color: <xsl:value-of select="./Departure/LineColor/@Hex"/>;
								</xsl:attribute>

								<xsl:value-of select="./Departure/LineLabel/text()"/>
							</span>
						</div>

						<div class="headsign">
							<xsl:value-of select="./Departure/Headsign/text()"/>
						</div>


						<div class="time">
							<script>
								javascript: document.write(window.external.Iso8601ToSimpleString('<xsl:value-of select="./Departure/DepartureDateTime/text()"/>'));
							</script>
						</div>
					</div>

					<hr/>

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
									<script>
										javascript: document.write(window.external.Iso8601ToSimpleString('<xsl:value-of select="./Departure/DepartureDateTime/text()"/>'));
									</script> ·
									<script>
										javascript: document.write(window.external.ReplaceIdWithName(<xsl:value-of select="./Departure/StopID/text()"/>));
									</script>
								</span>
							</li>

							<xsl:for-each select="./Departure/IntermediateStops/IntermediateStop">

								<li>
									<script>
										javascript: document.write(window.external.Iso8601ToSimpleString('<xsl:value-of select="./Arrival/text()"/>'));
									</script> ·
									<script>
										javascript: document.write(window.external.ReplaceIdWithName(<xsl:value-of select="./StopID/text()"/>));
									</script>
								</li>

							</xsl:for-each>
						</ol>

					</div>
				</div>
			</body>
		</html>
	</xsl:template>	
</xsl:stylesheet>
