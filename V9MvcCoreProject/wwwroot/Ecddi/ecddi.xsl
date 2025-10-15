<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:include href="quick_purchase.xsl"/>
  <xsl:output method="html" doctype-public="XSLT-compat" omit-xml-declaration="yes" encoding="UTF-8" indent="yes" />
  <xsl:template match="report_date">
  </xsl:template>
  <xsl:template match="input_request">
  </xsl:template>
  <xsl:template match="/">
    <html>
      <head>
        <title>
			<xsl:value-of select="xml/request_criteria/matchName" />
		</title>
		  <style type="text/css">
			  <!-- Output the raw CSS content -->
			  <xsl:text disable-output-escaping="yes"><![CDATA[</xsl:text>
            <xsl:value-of select="$cssContent" disable-output-escaping="yes"/>
            <xsl:text disable-output-escaping="yes">]]></xsl:text>
		  </style>
      </head>
      <body>
		<div class="wrapper">
			<table width="100%">
				<tr>
					<td align="left">
						<img src="data:image/png;base64,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" alt="" width="176px" height="59px" /></td>
					<td align="right" valign="bottom" class="italic-bold"><p>CrediTrack by Experian</p><p><xsl:value-of select="xml/report_date"/></p></td>
				</tr>
			</table>
            <p class="h1">ENHANCED CUSTOMER DUE-D INDIVIDUAL (ECDDI)</p>
			
            <xsl:apply-templates />
			<br /><br /><br />
			<!--bottom-->
			<xsl:variable name="dateValue" select="xml/request_criteria/dateRequest"/>
			<xsl:variable name="date" select="substring-after($dateValue, ' ')"/>
			<xsl:variable name="year" select="substring-after($date, ' ')"/>
			<p class="no-margin liteblue bold" align="center"><font>COMMERCIAL CONFIDENTIAL</font></p>
			<p class="no-margin liteblue bold">Experian Information Services (Malaysia) Sdn. Bhd. (532271-T) is certified to ISO/IEC 27001:2013, Cert. No: ISM 00290</p>
			<p class="small liteblue term">NOTICE: The information provided by Experian Information Services (Malaysia) Sdn. Bhd. (EXPERIAN) in this report is based on information which has been compiled from public sources and third parties. We do not guarantee the accuracy of the information provided by EXPERIAN. While we have used our best endeavours to ensure that the data is complete and accurate, we do not accept any liability for errors, omissions, incomplete information or non-current data and a purchaser or user of the information in this Report shall verify the accuracy of the information on its own. The information furnished is <u>STRICTLY CONFIDENTIAL</u> and should not be disclosed to any party including the subject concerned. The information in this report is not for evaluation or a comment on the credit-worthiness of the subject nor is it any advice, analysis, observation, representation or comment on the credit risk of the subject person or company/business or any other entity on whom/which the information is provided. EXPERIAN shall not be liable for any conclusions drawn by you/the user of any of the information found in this report. Please notify &amp; contact EXPERIAN promptly of any questions regarding the accuracy of the information contained in this report to the Customer Service Division at: Suite 16.02, Level 16, Centrepoint South Mid Valley City, Lingkaran Syed Putra, 59200 Kuala Lumpur, Malaysia or call: +60326151111.<br/><span style="text-align: left;">Dow Jones</span><span style="float:right;text-align: right;">Copyright@<xsl:value-of select="$year"/>. Dow Jones &amp; Company Inc. All rights reserved.</span></p>
        </div>
      </body>
    </html>
  </xsl:template>
  
	<xsl:template match="xml/request_criteria">
	
		<table border="0" width="100%" class="full_border">
			<tr>
				<td class="kyc_header" colspan="2" align="left">
					<font>An enhanced customer due diligence search to deep dive into a particular matched profile obtained from Customer Due Diligence – Individual (CDDI) search.</font>
				</td>
			</tr>
			<tr>
				<td colspan="2" align="left"><span class="kyc_request_table"><b>REQUEST CRITERIA</b></span><br /><span class="small">(You have requested to search on the following)</span></td>
			</tr>
			
			<xsl:if test="dateRequest">
				<tr>
					<td width="20%" class="bold">Date of Request</td>
					<td width="80%"><xsl:value-of select="dateRequest"/></td>
				</tr>
			</xsl:if>
			
			<!-- <xsl:value-of select="substring(dateRequest, 12, 8)"/> -->
			
			<xsl:if test="requestor_name">
				<tr>
					<td width="20%" class="bold">Name of Requestor</td>
					<td width="80%"><xsl:value-of select="requestor_name"/></td>
				</tr>
			</xsl:if>
			
			<xsl:if test="PARTIALNAME">
				<tr>
					<td width="20%" class="bold">Match Name</td>
					<td width="80%"><xsl:value-of select="matchName"/></td>
				</tr>
			</xsl:if>
			
			<xsl:if test="ProID">
				<tr>
					<td width="20%" class="bold">Profile ID</td>
					<td width="80%"><xsl:value-of select="ProID"/></td>
				</tr>
			</xsl:if>
			
			<xsl:if test="matchName">
				<tr>
					<td width="20%" class="bold">Match Name</td>
					<td width="80%">
						<xsl:choose>
							<xsl:when test="matchName != ''">
								<xsl:value-of select="matchName"/>
							</xsl:when>
							<xsl:otherwise>
								-
							</xsl:otherwise>
						</xsl:choose>
					</td>
				</tr>
			</xsl:if>
			
			<!-- <tr>
				<td width="20%" class="bold">Match %</td>
				<td width="80%">-->
					<!-- <xsl:value-of select="matchPerc"/> -->
					<!-- <xsl:choose>
							<xsl:when test="matchPerc != ''">
								<xsl:value-of select="matchPerc"/>
							</xsl:when>
							<xsl:otherwise>
								-
							</xsl:otherwise>
						</xsl:choose>
				</td>
			</tr> -->
			
			<!-- <xsl:if test="reqCountry"> -->
				<!-- <tr> -->
					<!-- <td width="20%" class="bold">Country</td> -->
					<!-- <td width="80%"><xsl:value-of select="reqCountry"/></td> -->
				<!-- </tr> -->
			<!-- </xsl:if> -->
			
			<tr>
				<td width="20%" class="bold">Country</td>
				<td width="80%">
					<!-- <xsl:value-of select="matchPerc"/> -->
					<xsl:choose>
						<xsl:when test="reqCountry != ''">
							<xsl:value-of select="reqCountry"/>
						</xsl:when>
						<xsl:otherwise>
							-
						</xsl:otherwise>
					</xsl:choose>
				</td>
			</tr>
			
			<xsl:if test="subscriber_refno">
				<tr>
					<td width="20%" class="bold">Your Ref. No</td>
					<td width="80%"><xsl:value-of select="subscriber_refno"/></td>
				</tr>
			</xsl:if>
			
		</table>
	</xsl:template>
	
	<xsl:template match="xml/search_result">
		<br />
		<table border="0" width="100%" class="full_border">
			
			<!-- <xsl:call-template name="result_details" /> -->
			<xsl:if test="profile_lists/PrfInfs">
				<xsl:call-template name="profile_template" />
			</xsl:if>
			
			<xsl:if test="profile_lists/DatInfs">
				<xsl:call-template name="date_template" />
			</xsl:if>
			
			<xsl:if test="profile_lists/NamInfs">
				<xsl:call-template name="name_template" />
			</xsl:if>
			
			<xsl:if test="profile_lists/CtyInfs">
				<xsl:call-template name="country_template" />
			</xsl:if>
			
			<xsl:if test="profile_lists/AddInfs">
				<xsl:call-template name="address_template" />
			</xsl:if>
			
			<xsl:if test="profile_lists/PobInfs">
				<xsl:call-template name="birthplace_template" />
			</xsl:if>
			
			<xsl:if test="profile_lists/IDInfs">
				<xsl:call-template name="id_template" />
			</xsl:if>
			
			<xsl:if test="profile_lists/WatLstInf/DetInfs">
				<xsl:call-template name="reference_template" />
			</xsl:if>
			
			<xsl:if test="profile_lists/WatLstInf/DesCatInfs">
				<xsl:call-template name="match_template" />
			</xsl:if>
			
			<xsl:if test="profile_lists/WatLstInf/Rmk">
				<xsl:call-template name="profile_note_template" />
			</xsl:if>
			

		</table>
		
		<br />
		
		
		<xsl:if test="profile_lists/RelInfs">
			<xsl:call-template name="relatives_template" />
		</xsl:if>
		
		
		
		<br />
		<br />
		
		<table border="0" width="100%" class="full_border">
			<xsl:if test="profile_lists/WatLstInf/RolInfs">
				<xsl:call-template name="roleinfo_template" />
			</xsl:if>
		</table>
		
		<br />
		
		<table border="0" width="100%" class="full_border">
			<xsl:if test="profile_lists/LasUpdDat">
				<xsl:call-template name="lastupdate_template" />
			</xsl:if>
		</table>
		
		
	</xsl:template>
	
	
	<xsl:template name="lastupdate_template">
		<tr>
			<th  align="left">LAST REVIEWED DATE: <xsl:value-of select="profile_lists/LasUpdDat"/></th>
		</tr>
		
		<!-- <tr> -->
			<!-- <td ><xsl:value-of select="profile_lists/LasUpdDat"/><br /></td> -->
		<!-- </tr> -->
		
	</xsl:template>
	
	<xsl:template name="profile_template">
		<tr>
			<th width="20%" align="left" style="border-right: 0px;">PROFILE<br /></th>
			<th width="30%" style="border-left: 0px; border-right: 0px;"><br /></th>
			<th width="20%" style="border-left: 0px; border-right: 0px;"><br /></th>
			<th width="30%" style="border-left: 0px;"><br /></th>
		</tr>
		<xsl:choose>
			<xsl:when test="(count(profile_lists/PrfInfs/PrfInf)) > 0">
				<xsl:for-each select="profile_lists/PrfInfs/PrfInf">
					<tr>
						<td colspan="1" class="bold">Gender<br /></td>
						<td colspan="3"><xsl:value-of select="Gdr"/></td>
					</tr>
					
					<tr>
						<td colspan="1" class="bold">Deceased<br /></td>
						<td colspan="3">
							<xsl:choose>
								<xsl:when test="IsDea = 'Y'">Yes</xsl:when>
								<xsl:when test="IsDea = 'N'">No</xsl:when>
								<xsl:otherwise><xsl:value-of select="IsDea"/></xsl:otherwise>
							</xsl:choose>
							
						</td>
					</tr>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<tr>
					<td colspan="4" class="show_border">No Information Available</td>
				</tr>
			</xsl:otherwise>
		</xsl:choose>
		
	</xsl:template>
	
	<xsl:template name="date_template">
		<tr>
			<th colspan="4" align="left">DATE<br /></th>
		</tr>
		
		<xsl:choose>
			<xsl:when test="(count(profile_lists/DatInfs/DatInf)) > 0">
				<xsl:for-each select="profile_lists/DatInfs/DatInf">
					<tr>
						<!-- <td width="20%" class="bold">Date of Birth<br /></td> -->
						<td colspan="1" class="bold"><xsl:value-of select="DatTyp"/></td>
						
						 <!-- <td colspan="3"><xsl:value-of select="Date"/></td> -->
						<!-- <td colspan="3"><xsl:value-of select="concat(Day, '/', Mth, '/', Yea)"/></td> -->
						<xsl:choose>
							<xsl:when test="Day">
								<td colspan="3"><xsl:value-of select="concat(Day, '/', Mth, '/', Yea)"/></td>
							</xsl:when>
							<xsl:when test="Mth">
								<td colspan="3"><xsl:value-of select="concat( Mth, '/', Yea)"/></td>
							</xsl:when>
							<xsl:otherwise>
								<td colspan="3"><xsl:value-of select="Yea"/></td>
							</xsl:otherwise>
						</xsl:choose>
					</tr>
					<xsl:if test="(count(Rmk)) > 0">
						<tr>
							<td colspan="1" ></td>
							<td colspan="3" >Remarks : <xsl:value-of select="Rmk"/></td>
						</tr>
					</xsl:if>
					
					
				</xsl:for-each>
				
				
				<xsl:if test="profile_lists/DatInfs/PepFlag">
					<tr>
						<td colspan="4"><i>Inactive as of (PEP) Indicates the earliest known date, as confirmed by research, as of which this person ceased to be an active PEP.</i></td>
					</tr>
				</xsl:if>
				
			</xsl:when>
			<xsl:otherwise>
				<tr>
					<td colspan="4" class="show_border">No Information Available</td>
				</tr>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	
	<xsl:template name="birthplace_template">
		
		<tr>
			<th colspan="4" align="left">PLACE OF BIRTH<br /></th>
		</tr>
		
		
		<xsl:choose>
			<xsl:when test="(count(profile_lists/PobInfs/PobInf)) > 0">
				<xsl:for-each select="profile_lists/PobInfs/PobInf">
					<tr>
						<td colspan ="4">
							<xsl:for-each select="*">
								<xsl:if test="position() > 1">, </xsl:if>
								<xsl:value-of select='.'/>
							</xsl:for-each>
						</td>
					</tr>					
					
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<tr>
					<td colspan="4" class="show_border">No Information Available</td>
				</tr>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	
	
	<xsl:template name="name_template">
		<tr>
			<th colspan="4" align="left">NAMES<br /></th>
		</tr>
		<xsl:choose>
			<xsl:when test="(count(profile_lists/NamInfs/NamInf)) > 0">
				<xsl:for-each select="profile_lists/NamInfs/NamInf">
					<tr><td colspan="4" align="left" bgcolor="#C5D9F1" class="bold"><xsl:value-of select="NamTyp"/></td></tr>
					<xsl:for-each select="Infs/Inf">
						
						<xsl:if test="ForNam">
							<tr>
								<td colspan="1" class="bold">First Name</td>
								<td colspan="3"><xsl:value-of select="ForNam"/></td>
							</tr>
						</xsl:if>
						
						<xsl:if test="MidNam">
							<tr>
								<td colspan="1" class="bold">Middle Name</td>
								<td colspan="3"><xsl:value-of select="MidNam"/></td>
							</tr>
						</xsl:if>
						
						<xsl:if test="Nam">
							<tr>
								<td colspan="1" class="bold">Name</td>
								<td colspan="3"><xsl:value-of select="Nam"/></td>
							</tr>
						</xsl:if>
						
						<xsl:if test="SurNam">
							<tr>
								<td colspan="1" class="bold">Surname</td>
								<td colspan="3"><xsl:value-of select="SurNam"/></td>
							</tr>
						</xsl:if>
						
						<xsl:if test="OthNamInfs/OthNamInf">
							<xsl:for-each select="OthNamInfs/OthNamInf">
								
								
								
								<xsl:choose>
									<xsl:when test="NamTyp = 'Original Script Name'">
										<tr>
										<td class="bold"><xsl:value-of select="NamTyp"/></td>
											
											<xsl:if test="Infs/Inf">
												<!-- <xsl:call-template name="name_other_template" /> -->
											
											
												<xsl:for-each select="Infs/Inf">
													<!-- <td width="30%" ><xsl:value-of select="position()"/></td> -->
													<xsl:choose>
													<xsl:when test="position() > 1">
														<tr><td width="20%" class="bold"></td>
															<xsl:call-template name="name_other_oriscript_template" />
															
														</tr>
													</xsl:when>
													<xsl:otherwise>
														<!-- <xsl:if test="OsnInfs/OsnInf"> -->
															<xsl:call-template name="name_other_oriscript_template" />
															
															
														<!-- </xsl:if> -->
													</xsl:otherwise>
													</xsl:choose>
												</xsl:for-each>
											</xsl:if>
										</tr>
									</xsl:when>
									<xsl:otherwise>
										<tr>
											<td class="bold"><xsl:value-of select="NamTyp"/></td>
											
											<xsl:for-each select="Infs/Inf">
											
												<xsl:choose>
													<xsl:when test="position() > 1">
														<tr><td width="20%" class="bold"></td>
															<xsl:call-template name="name_other_template" />
														</tr>
													</xsl:when>
													<xsl:otherwise>
														<xsl:call-template name="name_other_template" />
													</xsl:otherwise>
												</xsl:choose>
											
											</xsl:for-each>
										</tr>
									</xsl:otherwise>
								</xsl:choose>
						
								
							</xsl:for-each>
						</xsl:if>
						
						
						<tr>
							<td colspan="4" class="bold"></td>
						</tr>
					</xsl:for-each>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<tr>
					<td colspan="4" class="show_border">No Information Available</td>
				</tr>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	
	<xsl:template name="name_other_oriscript_template">
		<!-- <xsl:for-each select="OsnInfs/OsnInf"> -->
		
		<!-- <td width="30%" ><xsl:value-of select="position()"/></td> -->
				<td width="30%"><xsl:value-of select="NamLanCodDes"/></td>
				<td width="20%">Original Script</td>
				<td width="30%"><xsl:value-of select="Nam"/></td> 
			<!-- </xsl:for-each> -->
	</xsl:template>
	
	<xsl:template name="name_other_template">
		<xsl:if test="SglNam">
			<td colspan="3"><xsl:value-of select="SglNam"/></td>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="ForNam and SurNam">
				<td colspan="3">First Name : <xsl:value-of select="ForNam"/> <br />Surname : <xsl:value-of select="SurNam"/></td>
			</xsl:when>
			<xsl:when test="SurNam">
				<td colspan="3"><xsl:value-of select="SurNam"/></td>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="country_template">
		<tr>
			<th colspan="4" align="left">COUNTRIES DETAILS<br /></th>
		</tr>
		
		<xsl:choose>
			<xsl:when test="(count(profile_lists/CtyInfs/CtyInf)) > 0">
				<xsl:for-each select="profile_lists/CtyInfs/CtyInf">
					<tr>
						<td colspan="1" class="bold"><xsl:value-of select="CtyTyp"/></td>
						<td colspan="3"><xsl:value-of select="Cty"/></td>
					</tr>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<tr>
					<td colspan="4" class="show_border">No Information Available</td>
				</tr>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="address_template">
		<tr>
			<th colspan="4" align="left">ADDRESSES<br /></th>
		</tr>
		
		<xsl:choose>
			<xsl:when test="(count(profile_lists/AddInfs/AddInf)) > 0">
				<xsl:for-each select="profile_lists/AddInfs/AddInf">
					<tr>
						<!-- <td colspan="1" class="bold">First line of the address</td> -->
						<td colspan="4"><xsl:value-of select="Add"/></td>
					</tr>
					
					<!-- <xsl:if test="IsTty">
						<tr>
							<td colspan="1" class="bold">Is Territory</td>
							<td colspan="3"><xsl:value-of select="IsTty"/></td>
						</tr>
					</xsl:if>
					<xsl:if test="URL">
						<tr>
							<td colspan="1" class="bold">Website</td>
							<td colspan="3"><xsl:value-of select="URL"/></td>
						</tr>
					</xsl:if> -->
				</xsl:for-each>
			</xsl:when>
				<xsl:otherwise>
				<tr>
					<td colspan="4" class="show_border">No Information Available</td>
				</tr>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="id_template">
		<tr>
			<th colspan="4" align="left">ID NUMBER TYPES<br /></th>
		</tr>
		
		<xsl:choose>
			<xsl:when test="(count(profile_lists/IDInfs/IDInf)) > 0">
				<tr>
					<td colspan="1" class="bold" bgcolor="#C5D9F1">Number<br /></td>
					<td colspan="3" class="bold" bgcolor="#C5D9F1">Type<br /></td>
				</tr>
				<xsl:for-each select="profile_lists/IDInfs/IDInf">
					
					<tr>
						<td colspan="1"><xsl:value-of select="ID"/></td>
						<td colspan="3">
							<xsl:value-of select="IDTyp"/>
							<xsl:if test="(count(Rmk)) > 0"> ( <xsl:value-of select="Rmk"/> )
							</xsl:if>
						
						</td>
					</tr>
				</xsl:for-each>
			</xsl:when>
				<xsl:otherwise>
				<tr>
					<td colspan="4" class="show_border">No Information Available</td>
				</tr>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="reference_template">
		<tr>
			<th colspan="4" align="left">LIST REFERENCE<br /></th>
		</tr>
		
		<xsl:choose>
			<xsl:when test="(count(profile_lists/WatLstInf/DetInfs/DetInf)) > 0">
				<xsl:for-each select="profile_lists/WatLstInf/DetInfs/DetInf">
					<tr>
						<td colspan="4" class="bold" bgcolor="#C5D9F1"><xsl:value-of select="Des"/></td>
					</tr>
					
					<xsl:if test="Nam">
					   <tr>
					   <td class="bold" colspan="1">Name</td>
					   <td colspan="3"><xsl:value-of select="Nam"/></td>
					   </tr>
					</xsl:if>
					
					<tr>
					   <td class="bold" colspan="1">Status</td>
					   <td colspan="3"><xsl:value-of select="Stt"/></td>
					</tr>
					
					<tr>
					   <td class="bold" colspan="1">Source</td>
					   <td colspan="3"><xsl:value-of select="Src"/></td>
					</tr>
					   
					<!-- <tr>
					   <td class="bold" colspan="2">Source Provider Code</td>
					   <td colspan="2"><xsl:value-of select="SrcCod"/></td>
					</tr>-->
					
										   
					<xsl:if test="StrDatInf">
					   <tr>
					   <td class="bold" colspan="1">Since</td>
					   <td colspan="3">
							<!-- <xsl:value-of select="StrDatInf"/></td> -->
							<xsl:choose>
								<xsl:when test="StrDatInf/Day">
									<xsl:value-of select="concat(StrDatInf/Day, '/', StrDatInf/Mth, '/', StrDatInf/Yea)"/>
								</xsl:when>
								<xsl:when test="StrDatInf/Mth">
									<xsl:value-of select="concat( StrDatInf/Mth, '/', StrDatInf/Yea)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="StrDatInf/Yea"/>
								</xsl:otherwise>
							</xsl:choose>
						</td>
					   </tr>
					</xsl:if>
					<xsl:if test="EndDatInf">
					   <tr>
					   <td class="bold" colspan="1">To</td>
					   <!-- <td colspan="2"><xsl:value-of select="EndDatInf"/></td> -->
					   <td colspan="3">
							<!-- <xsl:value-of select="StrDatInf"/></td> -->
							<xsl:choose>
								<xsl:when test="EndDatInf/Day">
									<xsl:value-of select="concat(EndDatInf/Day, '/', EndDatInf/Mth, '/', EndDatInf/Yea)"/>
								</xsl:when>
								<xsl:when test="EndDatInf/Mth">
									<xsl:value-of select="concat( EndDatInf/Mth, '/', EndDatInf/Yea)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="EndDatInf/Yea"/>
								</xsl:otherwise>
							</xsl:choose>
						</td>
					   </tr>
					</xsl:if>
					
				</xsl:for-each>
			</xsl:when>
				<xsl:otherwise>
				<tr>
					<td colspan="4" class="show_border">No Information Available</td>
				</tr>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="match_template">
		<tr>
			<th colspan="4" align="left">MATCH CATEGORY<br /></th>
		</tr>
		
		<xsl:choose>
			<xsl:when test="(count(profile_lists/WatLstInf/DesCatInfs/DesCatInf)) > 0 or (count(profile_lists/WatLstInf/Stt)) > 0">
				<tr>
					<td colspan="1" class="bold" bgcolor="#C5D9F1">Status<br /></td>
					<td colspan="3" class="bold" bgcolor="#C5D9F1">Category<br /></td>
				</tr>
				<tr>
					<td colspan="1"><xsl:value-of select="profile_lists/WatLstInf/Stt"/><br /></td>
					<td colspan="3">
						<xsl:for-each select="profile_lists/WatLstInf/DesCatInfs/DesCatInf">
							Category <xsl:value-of select="Cat"/>: <xsl:value-of select="SglDes"/><br />
						</xsl:for-each>
					</td>
				</tr>
			</xsl:when>
			<xsl:otherwise>
			<tr>
				<td colspan="4" class="show_border">No Information Available</td>
			</tr>
			</xsl:otherwise>
		</xsl:choose>
		
	</xsl:template>
	
	<xsl:template name="profile_note_template">
		
		<tr>
			<th colspan="4" align="left">PROFILE NOTES<br /></th>
		</tr>
		
		<tr>

		<xsl:choose>
			<xsl:when test="(count(profile_lists/WatLstInf/Rmk/item)) > 0">
				<td colspan="4">
				<xsl:for-each select="profile_lists/WatLstInf/Rmk/item">
					<xsl:value-of select='.'/><br />
				</xsl:for-each>
				</td>	
			</xsl:when>
		<xsl:otherwise>
			<tr>
				<td colspan="4" class="show_border">No Information Available</td>
			</tr>
		</xsl:otherwise>
		</xsl:choose>
		
		</tr>
		
	</xsl:template>
	
	
	<xsl:template name="relatives_template">
		<table border="0" width="100%" class="full_border">
			<tr>
				<th colspan="4" align="left">RELATIVES/ CLOSE ASSOCIATES<br /></th>
			</tr>
			
			<xsl:choose>
				<xsl:when test="(count(profile_lists/RelInfs/RelInf)) > 0">
					<tr>
						<!-- <td colspan="1" class="bold" bgcolor="#C5D9F1">Profile ID</td> -->
						<td colspan="1" class="bold" bgcolor="#C5D9F1">Risk Category</td>
						<td colspan="1" class="bold" bgcolor="#C5D9F1">Name</td>
						<td colspan="1" class="bold" bgcolor="#C5D9F1">Relation</td>
					</tr>
					
								
						<xsl:for-each select="profile_lists/RelInfs/RelInf">
							<tr>
								<!-- <td colspan="2"><xsl:value-of select="DesCatInfs"/></td> -->
								<!-- <td colspan="1"><xsl:value-of select="ProID"/></td> -->
								<td colspan="1">
									<xsl:for-each select="DesCatInfs/DesCatInf">
										<xsl:value-of select='.'/><br />
									</xsl:for-each>
								</td>
								
								<td colspan="1"><xsl:value-of select="Nam"/></td>
								<td colspan="1"><xsl:value-of select="Rls"/></td>
							</tr>
						</xsl:for-each>
					
			</xsl:when>
				<xsl:otherwise>
				<tr>
					<td colspan="4" class="show_border">No Information Available</td>
				</tr>
				</xsl:otherwise>
			</xsl:choose>
		</table>
	</xsl:template>
	
	<xsl:template name="roleinfo_template">
		<tr>
			<th colspan="4" align="left">ROLES<br /></th>
		</tr>
		
		
		
		<xsl:choose>
			<xsl:when test="(count(profile_lists/WatLstInf/RolInfs/RolInf)) > 0">
				<xsl:for-each select="profile_lists/WatLstInf/RolInfs/RolInf">
				
					<tr>
						<td class="bold" colspan="4" bgcolor="#C5D9F1"><xsl:value-of select="RolTyp"/></td>
					</tr>
					<tr>
						<td class="bold" >Occupation Category</td>
						<td class="bold">Occupation Title</td>
						<td class="bold">Since</td>
						<td class="bold">To</td>
					</tr>
					
					<xsl:for-each select="Infs/Inf">
						<tr>
						<td colspan="1"><xsl:value-of select="TitCat"/></td>
						<td colspan="1"><xsl:value-of select="Tit"/></td>
						
						<td colspan="1">
							<!-- <xsl:value-of select="StrDatInf"/></td> -->
							<xsl:choose>
								<xsl:when test="StrDatInf/Day">
									<xsl:value-of select="concat(StrDatInf/Day, '/', StrDatInf/Mth, '/', StrDatInf/Yea)"/>
								</xsl:when>
								<xsl:when test="StrDatInf/Mth">
									<xsl:value-of select="concat( StrDatInf/Mth, '/', StrDatInf/Yea)"/>
								</xsl:when>
								<xsl:when test="StrDatInf/Yea">
									<xsl:value-of select="StrDatInf/Yea"/>
								</xsl:when>
								<xsl:otherwise>
									-
								</xsl:otherwise>
							</xsl:choose>
						</td>
						
						
					   <td colspan="1">
							<!-- <xsl:value-of select="StrDatInf"/></td> -->
							<xsl:choose>
								<xsl:when test="EndDatInf/Day">
									<xsl:value-of select="concat(EndDatInf/Day, '/', EndDatInf/Mth, '/', EndDatInf/Yea)"/>
								</xsl:when>
								<xsl:when test="EndDatInf/Mth">
									<xsl:value-of select="concat( EndDatInf/Mth, '/', EndDatInf/Yea)"/>
								</xsl:when>
								<xsl:when test="EndDatInf/Yea">
									<xsl:value-of select="EndDatInf/Yea"/>
								</xsl:when>
								<xsl:otherwise>
									-
								</xsl:otherwise>
							</xsl:choose>
						</td>
					</tr>	
					</xsl:for-each>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<tr>
					<td colspan="4" class="show_border">No Information Available</td>
				</tr>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="roleinfo_template_old">
		<tr>
			<th colspan="5" align="left">ROLE INFORMATION LIST<br /></th>
		</tr>
		
		<tr>
			<td class="bold" colspan="1" bgcolor="#C5D9F1">Role Type</td>
			<td class="bold" colspan="1" bgcolor="#C5D9F1">Occupation Title</td>
			<td class="bold" colspan="1" bgcolor="#C5D9F1">Category of the Occupation</td>
			<td class="bold" colspan="1" bgcolor="#C5D9F1">Since</td>
			<td class="bold" colspan="1" bgcolor="#C5D9F1">To</td>
		</tr>
		
		<xsl:choose>
			<xsl:when test="(count(profile_lists/WatLstInf/RolInfs/RolInf)) > 0">
				<xsl:for-each select="profile_lists/WatLstInf/RolInfs/RolInf">
				
				<tr>
					<td colspan="1" ><xsl:value-of select="RolTyp"/></td>
					<td colspan="1"><xsl:value-of select="Tit"/></td>
					<td colspan="1"><xsl:value-of select="TitCat"/></td>
					
					<td colspan="1">
						<!-- <xsl:value-of select="StrDatInf"/></td> -->
						<xsl:choose>
							<xsl:when test="StrDatInf/Day">
								<xsl:value-of select="concat(StrDatInf/Day, '/', StrDatInf/Mth, '/', StrDatInf/Yea)"/>
							</xsl:when>
							<xsl:when test="StrDatInf/Mth">
								<xsl:value-of select="concat( StrDatInf/Mth, '/', StrDatInf/Yea)"/>
							</xsl:when>
							<xsl:when test="StrDatInf/Yea">
								<xsl:value-of select="StrDatInf/Yea"/>
							</xsl:when>
							<xsl:otherwise>
								-
							</xsl:otherwise>
						</xsl:choose>
					</td>
					
					
				   <td colspan="1">
						<!-- <xsl:value-of select="StrDatInf"/></td> -->
						<xsl:choose>
							<xsl:when test="EndDatInf/Day">
								<xsl:value-of select="concat(EndDatInf/Day, '/', EndDatInf/Mth, '/', EndDatInf/Yea)"/>
							</xsl:when>
							<xsl:when test="EndDatInf/Mth">
								<xsl:value-of select="concat( EndDatInf/Mth, '/', EndDatInf/Yea)"/>
							</xsl:when>
							<xsl:when test="EndDatInf/Yea">
								<xsl:value-of select="EndDatInf/Yea"/>
							</xsl:when>
							<xsl:otherwise>
								-
							</xsl:otherwise>
						</xsl:choose>
					</td>
				</tr>	
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<tr>
					<td colspan="4" class="show_border">No Information Available</td>
				</tr>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
				
		
	
	
	
	<!--End of Report-->
	<xsl:template match="xml/end">
		
		<!--Remark Legend-->
		<!--Risk Category-->
		<br /><br />
		<table border="0" width="100%" class="full_border">
			<tr>
				<th colspan="2" align="left">Risk Category<br /></th>
				<th colspan="2" align="left">Description<br /></th>
			</tr>
			<tr>
				<td width="10%" class="bold">SAN</td>
				<td width="20%" class="bold">Sanction List - Person</td>
				<td width="70%">PERSONS WHO ARE SUBJECT TO COMPREHENSIVE OR TARGETED RESTRICTIVE MEASURE OR CITED IN SANCTIONS LIST FOR VARIOUS REASON</td>
			</tr>
			
			<tr>
				<td width="10%" class="bold">OOL</td>
				<td width="20%" class="bold">Other Official List - Person</td>
				<td width="70%">PERSONS WHO ARE ASSOCIATED WITH POTENTIALLY HIGH RISK ACTIVITIES SUCH AS SERIOUS CRIME AND FINANCIAL OR PROFESSIONAL ENFORCEMENT ACTIONS OR WARNINGS.</td>
			</tr>
			
			<tr>
				<td width="10%" class="bold">PEP</td>
				<td width="20%" class="bold">Politically Exposed Person</td>
				<td width="70%">PERSONS WHO ARE WIELDING POLITICAL POWER OR INFLUENCE, EITHER IN A GOVERNMENT POSITION OR IN AN INTERNATIONAL ORGANIZATION.</td>
			</tr>
			
			<tr>
				<td width="10%" class="bold">SI</td>
				<td width="20%" class="bold">Special Interest - Person</td>
				<td width="70%">PERSONS WHO ARE PROMINENT IN THE NEWS ASSOCIATION WITH ACCUSED OF, ARRESTED FOR OR CONVICTED OF SERIOUS CRIMES.</td>
			</tr>
			
			<tr>
				<td width="10%" class="bold">SI-LT</td>
				<td width="20%" class="bold">Special Interest - Person (Lower Threshold)</td>
				<td width="70%">PERSONS WHO ARE REPORTED TO HAVE COMMITTED CRIMES, OR HAS BEEN FORMALLY DECLARED WANTED AN OFFENCE, OR HAS BEEN CHARGED WITH AND/ OR ARRESTED ON SUSPICION ACTIVITIES.</td>
			</tr>
			
			<tr>
				<td width="10%" class="bold">OEL</td>
				<td width="20%" class="bold">Other Exclusion List - Person</td>
				<td width="70%">EXCLUSIONS FROM PUBLIC SECTOR ACTIVITIES AGAINST PEOPLE AND/OR ENTITIES OPERATING IN A PARTICULAR JURISDICTION, SECTOR OR INDUSTRY, USUALLY FOR A SET PERIOD.</td>
			</tr>
			
			<tr>
				<td width="10%" class="bold">RCA</td>
				<td width="20%" class="bold">Relative or close associates</td>
				<td width="70%">THE IMMEDIATE FAMILY MEMBERS OF A PERSON AND THEIR CLOSEST NON-FAMILIAL ASSOCIATES OF PEP.</td>
			</tr>
			
			<tr>
				<td width="10%" class="bold">BM</td>
				<td width="20%" class="bold">Board member</td>
				<td width="70%">BOARD MEMBER OF THE STATE-OWNED COMPANIES BY THE COMPREHENSIVELY SANCTIONED COUNTRIES.</td>
			</tr>
			
		</table>
		<br /><br />
		
		
		<!--Glossary-->
		<table border="0" width="100%" class="full_border">
			<tr>
				<th colspan="1" align="left">Glossary<br /></th>
				<th colspan="1" align="left">Description<br /></th>
			</tr>
			
			<tr>
				<td width="30%" class="bold">Record Type:</td>
				<td width="70%">IDENTIFIES WHETHER THE PROFILE IS OF A PERSON.</td>
			</tr>
			
			<tr>
				<td width="30%" class="bold">Gender:</td>
				<td width="70%">IDENTIFIES A PERSON’S GENDER.</td>
			</tr>
			
			<tr>
				<td width="30%" class="bold">Deceased:</td>
				<td width="70%">IDENTIFIES IF A PERSON’S DECEASED.</td>
			</tr>
			
			<tr>
				<td width="30%" class="bold">Names:</td>
				<td width="70%">IDENTIFIES THE PERSONS’ NAME DETAILS.<br />
THIS INCLUDES ALL THE NAME TYPES LIKE PRIMARY NAME, AKA, SPELLING VARIATION, LOW QUALITY AKA, ETC. IF KNOWN. EACH NAME TYPE INCLUDES THE SUB NAME TYPES, LIKE ORIGINAL SCRIPT NAME, SPELLING VARIATION, IF KNOWN.
</td>
			</tr>
			
			<tr>
				<td width="30%" class="bold">Addresses:</td>
				<td width="70%">DETAILS THE ADDRESS FOR A PERSON, IF KNOWN.</td>
			</tr>
			
			<tr>
				<td width="30%" class="bold">Dates:</td>
				<td width="70%">IDENTIFIES DATE OF BIRTH OR DECEASED DATE FOR A PERSON, IF KNOWN. ALSO, INCLUDES NOTES REGARDING A PERSON’S DATE OF BIRTH, IF KNOWN.</td>
			</tr>
			
			<tr>
				<td width="30%" class="bold">ID Number Types:</td>
				<td width="70%">IDENTIFIES THE PERSON’S IDENTIFICATION DETAILS. FOR EXAMPLE: PASSPORT NUMBER, NATIONAL IDENTITY CARD NUMBER, ETC., IF KNOWN.</td>
			</tr>
			
			<tr>
				<td width="30%" class="bold">Sanctions List:</td>
				<td width="70%">IDENTIFIES THE SANCTION REFERENCES LIST AND OTHER OFFICIAL LISTS ASSIGNED TO THE PERSON.</td>
			</tr>
			
			<tr>
				<td width="30%" class="bold">Status:</td>
				<td width="70%">IDENTIFIES THE STATUS OF THE PERSON. FOR EXAMPLE: ACTIVE OR INACTIVE.</td>
			</tr>
			
			<tr>
				<td width="30%" class="bold">Category 1:</td>
				<td width="70%">IDENTIFIES A PERSON’S CATEGORY 1. FOR EXAMPLE: POLITICALLY EXPOSED PERSON, ADVERSE MEDIA OR RELATIVE AND CLOSE ASSOCIATES.</td>
			</tr>
			
			<tr>
				<td width="30%" class="bold">Category 2:</td>
				<td width="70%">IDENTIFIES THE PERSON’S CATEGORY 2, IF ASSIGNED. FOR EXAMPLE: ENFORCEMENT, SANCTIONS.</td>
			</tr>
			
			<tr>
				<td width="30%" class="bold">Category 3:</td>
				<td width="70%">IDENTIFIES THE PERSON’S CATEGORY 3, IF ASSIGNED.FOR EXAMPLE: LOWER THRESHOLD - INDIVIDUALS WHO ARE REPORTED IN THE PRESS TO HAVE COMMITTED OFFENCES BELOW A SPECIFIC FINANCIAL THRESHOLD CRIME CATEGORIES.</td>
			</tr>
			
			<tr>
				<td width="30%" class="bold">Relatives/Close Associates:</td>
				<td width="70%">IDENTIFIES A LIST OF RELATIVES AND/OR ASSOCIATES KNOWN TO BE AFFILIATED WITH THE ENTITY.</td>
			</tr>
			
			<tr>
				<td width="30%" class="bold">Roles:</td>
				<td width="70%">IDENTIFIES WITH THE PERSON’S PROFESSIONAL DETAILS.</td>
			</tr>
			
			<tr>
				<td width="30%" class="bold">Previous Role:</td>
				<td width="70%">IDENTIFIES THE PERSON’S PREVIOUS KNOWN OCCUPATION DETAILS.</td>
			</tr>
			
			<tr>
				<td width="30%" class="bold">Other Current Roles:</td>
				<td width="70%">REFERS TO DETAILS ANY ADDITIONAL ROLES A PERSON IS FULFILLING ASIDE FROM THE CURRENT PROFESSION LISTED IN OCCUPATION TITLE.</td>
			</tr>
			
			<tr>
				<td width="30%" class="bold">Profile Notes:</td>
				<td width="70%">REFERS TO DETAILS ANY EXTRA NOTES OR INFORMATION ABOUT THE PERSON, IF KNOWN.</td>
			</tr>
			
			<tr>
				<td width="30%" class="bold">Last Reviewed date:</td>
				<td width="70%">IDENTIFIES THE DATE WHEN THE PROFILE WAS LAST REVIEWED.</td>
			</tr>
			
			
		</table>
		
		<p class="bold">-END OF REPORT-</p>
		<table border="0" width="100%">
			<tr>
				<td align="left">SUBSCRIBER NAME: <xsl:value-of select="/xml/end/subscriber_name" /></td>
				<td align="right">ORDER DATE: <xsl:value-of select="/xml/end/order_date" /></td>
			</tr>
			<tr>
				<td align="left">USER NAME: <xsl:value-of select="/xml/end/username" /></td>
				<td align="right">ORDER TIME: <xsl:value-of select="/xml/end/order_time" /></td>
			</tr>
		</table>
	</xsl:template>
	
</xsl:stylesheet>