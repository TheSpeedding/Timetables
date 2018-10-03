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

					<script>
						javascript: document.write(window.external.ShowDepartureText());
					</script>
					
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