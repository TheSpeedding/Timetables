<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" exclude-result-prefixes="msxsl xsi">
	
	<xsl:output method="html" indent="no"/>
	
	<xsl:template match="/">
		<html>			
			<head>
				<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
			</head>
			
			<body>
				<xsl:for-each select="//item">
					<div class="extraevent">
						
						<div class="name">														
								<xsl:value-of select="./title/text()"/>	
            </div>
												
						<div class="box">
								<xsl:value-of select="./description/text()" disable-output-escaping="yes"/>											
						</div>
					</div>
				</xsl:for-each>
			</body>
		</html>
	</xsl:template>	
</xsl:stylesheet>
