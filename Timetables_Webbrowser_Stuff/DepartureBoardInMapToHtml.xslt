<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" exclude-result-prefixes="msxsl xsi">
	
	<xsl:output method="html" indent="no"/>
	
	<xsl:template match="/">
		<html>			
			<head>
				<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
			</head>
			
			<body>
				<div class="content">
					<div class="title">
						<script>
							javascript: document.write(window.external.ReplaceIdWithName(<xsl:value-of select="//Departure/StopID/text()"/>));
						</script>
						<xsl:if test="not(//Departure)">
							<script>
								javascript: document.write(window.external.NoDepartures());
							</script>
						</xsl:if>
					</div>
					<div class="arrival">
						<script>
							javascript:
							document.write(window.external.ShowArrivalConstant());
							document.write(window.external.ShowArrivalTime(<xsl:value-of select="//Departure/StopID/text()"/>));
						</script>
					</div>

					<xsl:for-each select="//Departure">
						<div class="departure">

							<div class="box">

								<div class="label">
									<span>
										<xsl:attribute name="class">
											<xsl:value-of select="./MeanOfTransport/text()"/>
										</xsl:attribute>

										<xsl:attribute name="style">
											background-color: <xsl:value-of select="./LineColor/@Hex"/>;
										</xsl:attribute>

										<xsl:value-of select="./LineLabel/text()"/>
									</span>
								</div>

								<div class="headsign">
									<xsl:value-of select="./Headsign/text()"/>
								</div>


								<div class="time">
									<script>
										javascript: document.write(window.external.Iso8601ToSimpleString('<xsl:value-of select="./DepartureDateTime/text()"/>'));
									</script>
								</div>
							</div>
						</div>
					</xsl:for-each>
				</div>
			</body>
		</html>
	</xsl:template>	
</xsl:stylesheet>
