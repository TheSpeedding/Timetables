<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" exclude-result-prefixes="msxsl xsi">

	<xsl:output method="html" indent="no"/>

	<xsl:template match="/">
		<html>
			<head>
				<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
			</head>

			<body id="journey-type">

				<div class="journey">

					<div class="leaves-in">
						<xsl:if test="./Journey/JourneySegments/JourneySegment/Outdated[text() = 'true']">
							<!-- Indicates whether the journey uses outdated timetables. -->
							<span class="outdated"></span>
						</xsl:if>
						<!-- Writes the relative time that the journey leaves in. -->
						<script>
							javascript: document.write(window.external.LeavingTimeToString('<xsl:value-of select="./Journey/JourneySegments/JourneySegment[position() = 1]/DepartureDateTime/text()"/>'));
						</script>
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

					<div class="box">

						<div class="info">

							<!-- Writes total duration of the journey. -->
							<div class="duration">
								<script>
									javascript: document.write(window.external.TotalDurationToString('<xsl:value-of select="./Journey/JourneySegments/JourneySegment[position() = 1]/DepartureDateTime/text()"/>', '<xsl:value-of select="./Journey/JourneySegments/JourneySegment[position() = last()]/ArrivalDateTime/text()"/>'));
								</script>
							</div>

							<!-- Writes number of transfers. -->
							<div class="transfers">
								<script>
									javascript: document.write(window.external.TotalTransfersToString(<xsl:value-of select="count(./Journey/JourneySegments/JourneySegment[@xsi:type = 'TripSegment'])"/>));
								</script>
							</div>

						</div>

						<div class="main">

							<!-- Writes info about source station, i.e. leaving time and its name. -->
							<div class="departure">

								<div class="time">
									<span class="iso8601">
										<xsl:value-of select="./Journey/JourneySegments/JourneySegment[position() = 1]/DepartureDateTime/text()"/>
									</span>
								</div>

								<div class="station">
									<span class="station-id">
										<xsl:value-of select="./Journey/JourneySegments/JourneySegment[position() = 1]/SourceStopID/text()"/>
									</span>
								</div>

							</div>

							<!-- Writes information about the journey, i.e. its segments. -->
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
										<xsl:value-of select="./Journey/JourneySegments/JourneySegment[position() = last()]/ArrivalDateTime/text()"/>
									</span>
								</div>

								<div class="station">
									<span class="station-id">
										<xsl:value-of select="./Journey/JourneySegments/JourneySegment[position() = last()]/TargetStopID/text()"/>
									</span>
								</div>

							</div>

						</div>
					</div>		
					
					<!-- Detailed info. -->

					<hr/>
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
