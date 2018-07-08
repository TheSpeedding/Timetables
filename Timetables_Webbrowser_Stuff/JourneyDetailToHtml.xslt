﻿<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" exclude-result-prefixes="msxsl xsi">

	<xsl:output method="html" indent="yes"/>

	<xsl:template match="/">
		<html>

			<head>
				<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
				<script>
					javascript: document.write(window.external.LoadCssStylesheet('css/JourneyDetailToHtml.css'));
				</script>
				<title>Journey - <xsl:value-of select="./Journey/JourneySegments/JourneySegment[position() = 1]/SourceStopName/text()"/> - <xsl:value-of select="./Journey/JourneySegments/JourneySegment[position() = last()]/TargetStopName/text()"/></title>
			</head>

			<body>
				
					<div class="journey">

						<div class="leaves-in">
							<xsl:if test="./Journey/JourneySegments/JourneySegment/Outdated[text() = 'true']">
								<span class="outdated">
									Outdated!
								</span>
							</xsl:if>
							<script>
								javascript: document.write(window.external.LeavingTimeToString('<xsl:value-of select="./Journey/JourneySegments/JourneySegment[position() = 1]/DepartureDateTime/text()"/>'));
							</script>
						</div>

						<ul class="tools">
							<li>
								<a href="#">Map</a>
							</li>
							<li>
								<a href="#">Print</a>
							</li>
						</ul>

						<div class="box">

							<div class="info">

								<div class="duration">
									<script>
										javascript: document.write(window.external.TotalDurationToString('<xsl:value-of select="./Journey/JourneySegments/JourneySegment[position() = 1]/DepartureDateTime/text()"/>', '<xsl:value-of select="./Journey/JourneySegments/JourneySegment[position() = last()]/ArrivalDateTime/text()"/>'));
									</script>
								</div>

								<div class="transfers">
									<script>
										javascript: document.write(window.external.TotalTransfersToString(<xsl:value-of select="count(./Journey/JourneySegments/JourneySegment[@xsi:type = 'TripSegment'])"/>));
									</script>
								</div>

							</div>

							<div class="main">

								<div class="departure">

									<div class="time">
										<script>
											javascript: document.write(window.external.Iso8601ToSimpleString('<xsl:value-of select="./Journey/JourneySegments/JourneySegment[position() = 1]/DepartureDateTime/text()"/>'));
										</script>
									</div>

									<div class="station">
										<xsl:value-of select="./Journey/JourneySegments/JourneySegment[position() = 1]/SourceStopName/text()"/>
									</div>

								</div>

								<ol class="segments">
									<xsl:for-each select="./Journey/JourneySegments/JourneySegment">
										<xsl:choose>

											<xsl:when test="@xsi:type = 'FootpathSegment'">
												<li class="Footpath">
													<img width="20" height="20" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABmJLR0QA/wD/AP+gvaeTAAAACXBIWXMAAA7DAAAOwwHHb6hkAAAAB3RJTUUH4gcIDykjDbFNcgAAAiFJREFUWMPFlzFoVEEQhv/RM4l3yIkkNkEtRJRTo4WEkEoQRJRoZWG0tAjaiIWdjYKkEOxSi40RVAghEFDEwtIrUmgjYiEIJgp6xhzh4n02e7Cuz+Pu3tvn3+wyOzv/v7uzO+9JKQBUgBdAA6gC48oLQBlY5k/8Aka7ibMphYZjkoYS4p3LS8BAhJhdH8FP/sZonnkwBtSAOvADOK3/AWAXsLWXuYUeyAYkTUk6K6lf0pKkG2ZWz2OlO4D3Cec+lwd5BfjEv3E4JvkJl2jt8CgW+eWAaBFYTRBQB0pZk/cDHzyS20CpzS7MxEi61ns/5WyXEojXvf7eLAXsBtZc4O3O9jRBwD5XiFq4BwxmlfkAX4Gis60H5MvANuBUYK8BT4ByGgHnXbA3QJ87khBLQMH5bwFmEq7rSK/VcMy1nyU1JH1L8HltZhuSZGYNM7si6YCkSUlrzmdP2nJcNTPMrClpOhgbAsw3mFnNzB6aWUlS0czmez2CAnAwsG0G3gVbfCivqnfLtROBgJVwF2IJWAUWXH82EHE3NvlRj+yiu3pfgseoHFPAfY/ssbNdDXahGot8p/vkAmgCE97YfCDiQgwBDzyC761X0Y0NB6X6Y+xyPJLgcy3wmc2KfNBlftMFvtPG96UnoAkcyULAcy/o2w4K14bn/yot+aS3mpVOrhhwPSzTaQTc9H44z3Qx75knYDiNgD7gJLC/y3lF4DhQ6cT/N9dexPM3mnpEAAAAAElFTkSuQmCC"/>
												</li>
											</xsl:when>

											<xsl:otherwise>
												<li>

													<xsl:attribute name="class">
														<xsl:value-of select="./MeanOfTransport/text()"/>
													</xsl:attribute>

													<xsl:attribute name="style">background-color: <xsl:value-of select="./LineColor/@Hex"/>;</xsl:attribute>

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
											javascript: document.write(window.external.Iso8601ToSimpleString('<xsl:value-of select="./Journey/JourneySegments/JourneySegment[position() = last()]/ArrivalDateTime/text()"/>'));
										</script>
									</div>

									<div class="station">
										<xsl:value-of select="./Journey/JourneySegments/JourneySegment[position() = last()]/TargetStopName/text()"/>
									</div>

								</div>

							</div>
						</div>
					</div>
					
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
