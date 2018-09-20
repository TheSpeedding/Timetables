<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" exclude-result-prefixes="msxsl xsi">

	<xsl:output method="html" indent="no"/>

	<xsl:template match="/">
		<html>
			<head>
				<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
			</head>

			<body>

				<script>
					javascript: document.write(window.external.ShowJourneyText());
				</script>
				
				<div class="journey">
					<xsl:for-each select="./Journey/JourneySegments/JourneySegment">

						<div class="box detail">
								
							<xsl:choose>
								<xsl:when test="@xsi:type = 'FootpathSegment'">
									<h1>
										<script>javascript: document.write(window.external.TransferStringConstant());</script> ·
										<script>
											javascript: document.write(window.external.TotalDurationToString('<xsl:value-of select="./DepartureDateTime/text()"/>', '<xsl:value-of select="./ArrivalDateTime/text()"/>'));
										</script>
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
											<script>
												javascript: document.write(window.external.Iso8601ToSimpleString('<xsl:value-of select="./DepartureDateTime/text()"/>'));
											</script> ·
											<script>
												javascript: document.write(window.external.ReplaceIdWithName(<xsl:value-of select="./SourceStopID/text()"/>));
											</script>
										</span>
									</li>

									<xsl:for-each select="./IntermediateStops/IntermediateStop">

										<li>
											<script>
												javascript: document.write(window.external.Iso8601ToSimpleString('<xsl:value-of select="./Arrival/text()"/>'));
											</script> ·
											<script>
												javascript: document.write(window.external.ReplaceIdWithName(<xsl:value-of select="./StopID/text()"/>));												
											</script>
										</li>

									</xsl:for-each>

									<li>
										<span style="font-weight: bold;">
											<script>
												javascript: document.write(window.external.Iso8601ToSimpleString('<xsl:value-of select="./ArrivalDateTime/text()"/>'));
											</script> ·
											<script>
												javascript: document.write(window.external.ReplaceIdWithName(<xsl:value-of select="./TargetStopID/text()"/>));
											</script>
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
