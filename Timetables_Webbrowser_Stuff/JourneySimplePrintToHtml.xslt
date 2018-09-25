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
								
				<xsl:for-each select="//Journey">
					<div class="journey">						
						<div class="box">
							<div class="main">

								<!-- Writes info about source station, i.e. leaving time and its name. -->
								<div class="departure">

									<div class="time">
										<script>
											javascript: document.write(window.external.Iso8601ToSimpleString('<xsl:value-of select="./JourneySegments/JourneySegment[position() = 1]/DepartureDateTime/text()"/>'));
										</script>
									</div>

									<div class="station">
										<script>
											javascript: document.write(window.external.ReplaceIdWithName(<xsl:value-of select="./JourneySegments/JourneySegment[position() = 1]/SourceStopID/text()"/>));
										</script>										
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
										<script>
											javascript: document.write(window.external.Iso8601ToSimpleString('<xsl:value-of select="./JourneySegments/JourneySegment[position() = last()]/ArrivalDateTime/text()"/>'));
										</script>
									</div>

									<div class="station">
										<script>
											javascript: document.write(window.external.ReplaceIdWithName(<xsl:value-of select="./JourneySegments/JourneySegment[position() = last()]/TargetStopID/text()"/>));
										</script>										
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
