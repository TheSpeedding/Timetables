<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" exclude-result-prefixes="msxsl xsi">
	<xsl:output method="html" indent="no"/>	
	<xsl:template match="/">
		<html>			
			<head>
				<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
			</head>			
			<body id="departure-board-type">
				<ul class="tools">
					<li>
						<a id="edit-parameters-link" href="#"></a>
					</li>
					<li>
						<a id="print-list-link" href="#"></a>
					</li>
				</ul>				
				<xsl:for-each select="//Departure">
					<div class="departure">						
						<div class="leaves-in">														
								<xsl:if test="./Outdated[text() = 'true']">			
									<!-- Indicates whether the trip uses outdated timetables. -->
									<span class="outdated"></span>
								</xsl:if>
							<!-- Writes the relative time that the trip leaves in. -->
							<span class="leaving-time">
								<xsl:value-of select="./DepartureDateTime/text()"/>
							</span>
						</div>
						<!-- Links to other windows. -->
						<ul class="tools">
							<li>
								<a class="detail-link" href="#">
									<xsl:attribute name="id">
										<xsl:value-of select="position() - 1"/>
									</xsl:attribute>
								</a>
							</li>
						</ul>						
						<div class="box">
							<div class="label">
								<span>
									<xsl:attribute name="class">
										<xsl:value-of select="./MeanOfTransport/text()"/> detailbox
									</xsl:attribute>
									<xsl:attribute name="style">
										background-color: <xsl:value-of select="./LineColor/@Hex"/>;
										color: <xsl:value-of select="./LineTextColor/@Hex"/>;
									</xsl:attribute>
									<xsl:value-of select="./LineLabel/text()"/>
									<span class="detail">
									 · 
										<xsl:value-of select="./Headsign/text()"/>
										 (<span class="iso8601">
											<xsl:value-of select="./DepartureDateTime/text()"/>
										</span>)
									</span>
								</span>								
							</div>
							<div class="headsign">
								<xsl:value-of select="./Headsign/text()"/>
							</div>
							<div class="time">
								<span class="iso8601">
									<xsl:value-of select="./DepartureDateTime/text()"/>
								</span>
							</div>							
						</div>
					</div>
				</xsl:for-each>
			</body>
		</html>
	</xsl:template>	
</xsl:stylesheet>