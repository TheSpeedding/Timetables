<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" exclude-result-prefixes="msxsl xsi">
	
	<xsl:output method="html" indent="yes"/>
	
	<xsl:template match="/">
		<html>
			
			<head>
				<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
					<script>
						javascript: document.write(window.external.LoadCssStylesheet('JourneysSimpleToHtml.css'));
					</script>
				<title>Journeys (<xsl:value-of select="count(//Journey)"/>) - <xsl:value-of select="//Journey[position() = 1]/JourneySegments/JourneySegment[position() = 1]/SourceStopName/text()"/> - <xsl:value-of select="//Journey[position() = 1]/JourneySegments/JourneySegment[position() = last()]/TargetStopName/text()"/></title>
			</head>
			
			<body>
				<xsl:for-each select="//Journey">
					<div class="journey">
						
						<div class="leaves-in">							
								<xsl:if test="./JourneySegments/JourneySegment/Outdated[text() = 'true']">
									<span class="outdated">
										Outdated!
									</span>
								</xsl:if>
							<script>javascript: document.write(window.external.LeavingTimeToString('<xsl:value-of select="./JourneySegments/JourneySegment[position() = 1]/DepartureDateTime/text()"/>'));</script>
						</div>
						
						<ul class="tools">
							<li>
								<a href="#">Map</a>
							</li>
							<li>
								<a href="#">Detail</a>
							</li>
							<li>
								<a href="#">Print</a>
							</li>
						</ul>
						
						<div class="box">

							<div class="info">
								
								<div class="duration">
									<script>
										javascript: document.write(window.external.TotalDurationToString('<xsl:value-of select="./JourneySegments/JourneySegment[position() = 1]/DepartureDateTime/text()"/>', '<xsl:value-of select="./JourneySegments/JourneySegment[position() = last()]/ArrivalDateTime/text()"/>'));
									</script>
								</div>

								<div class="transfers">
									<script>
										javascript: document.write(window.external.TotalTransfersToString(<xsl:value-of select="count(./JourneySegments/JourneySegment[@xsi:type = 'TripSegment'])"/>));
									</script>
								</div>
							
							</div>

							<div class="main">

								<div class="departure">

									<div class="time">
										<script>
											javascript: document.write(window.external.Iso8601ToSimpleString('<xsl:value-of select="./JourneySegments/JourneySegment[position() = 1]/DepartureDateTime/text()"/>'));
										</script>
									</div>

									<div class="station">
										<xsl:value-of select="./JourneySegments/JourneySegment[position() = 1]/SourceStopName/text()"/>
									</div>

								</div>

								<ol class="segments">
									<xsl:for-each select="./JourneySegments/JourneySegment">
										<xsl:choose>

											<xsl:when test="not(./MeanOfTransport)">
												<li class="Footpath">
													Transfer
												</li>
											</xsl:when>

											<xsl:otherwise>
												<li>

													<xsl:attribute name="class">
														<xsl:value-of select="./MeanOfTransport/text()"/>
													</xsl:attribute>

													<xsl:attribute name="style">
														background-color: <xsl:value-of select="./LineColor/@Web"/>;
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
										<script>
											javascript: document.write(window.external.Iso8601ToSimpleString('<xsl:value-of select="./JourneySegments/JourneySegment[position() = last()]/ArrivalDateTime/text()"/>'));
										</script>
									</div>

									<div class="station">
										<xsl:value-of select="./JourneySegments/JourneySegment[position() = last()]/TargetStopName/text()"/>
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
