# Research - BYOVD Avast! aswArPot  

Windows hates aswArPot, so the toughest part of this process is getting it installed as a service...  
> [SC] StartService FAILED 2148204812: A certificate was explicitly revoked by its issuer.  
or  
> Failed to start the service. Error: -2146762484  

https://kb.eventtracker.com/evtpass/evtPages/EventId_51_CertSvc_47572.asp  

*not a huge fan of disabling defender to get a poc to work...  
*certificate for aswArPot driver has definately been revoked...  
*can we disable checks or find one with a valid certificate???  
https://www.bleepingcomputer.com/news/security/how-malware-gains-trust-by-abusing-the-windows-cryptoapi-flaw/  
https://decoded.avast.io/martinchlumecky/dirtymoe-3/  

Articles  
https://www.bleepingcomputer.com/news/security/hackers-abuse-avast-anti-rootkit-driver-to-disable-defenses/  
https://www.trendmicro.com/en_us/research/22/e/avoslocker-ransomware-variant-abuses-driver-file-to-disable-anti-Virus-scans-log4shell.html  
https://cybersecuritynews.com/10-year-old-flaws-with-avast-and-avg/  

Related CVEs  
CVE-2022-26522  
CVE-2022-26523  
CVE-2021-40539  

Avast download 20.8.2432  
https://archive.org/details/avast_free_antivirus_setup_offline_202010  

Original download  
http://files.avast.com/iavs9x/avast_free_antivirus_setup_offline.exe  

Path of drivers after install of Avast 20.8.2432  
```  
C:\Program Files\Avast Software\Avast\setup\Inf\x64\
```  

Create aswSP_ArPot2 service  
```  
sc.exe create aswSP_ArPot2 binPath= "C:\<Path to driver>\aswArPot.sys" type= kernel
```  

Create aswSP_Avar service  
```  
sc.exe create aswSP_Avar binPath= "C:\<Path to driver>\aswSP.sys" type= kernel
```  

Original KillProcessPOC - https://github.com/timwhitez/killProcessPOC/blob/main/aswarpot.go  

poc.go in this repo is modded version of original POC (aswarpot.go), but it directly calls win32 API, so no reliance on package "golang.org/x/sys/windows" as in original (no 'go mod init' necessary...)  

## Security Process Names  
```  
ERAAgent.exe
efwd.exe
SophosSafestore64.exe
PEFService.exe
mfefw.exe
mfemms.exe
mfewc.exe
McDiReg.exe
ProtectedModuleHost.exe
msmpeng.exe
ssdvagent.exe
avp.exe
swc_service.exe
bccavsvc.exe
psuaservice.exe
TmCCSF.exe
coreserviceshell.exe
repux.exe
vapm.exe
cylancesvc.exe
avpsus.exe
sbamtray.exe
sophoslivequeryservice.exe
epwd.exe
sophosmtrextension.exe
sophoscleanup.exe
iptray.exe
sentinelservicehost.exe
endpointbasecamp.exe
macompatsvc.exe
sepWscSvc64.exe
epab_svc.exe
mcsagent.exe
sophosfilescanner.exe
aswtoolssvc.exe
mfemactl.exe
avpsus.exe
avpui.exe
mcupdatemgr.exe
McAWFwk.exe
mfewch.exe
mfevtps.exe
McCSPServiceHost.exe
McPvTray.exe
MMSSHOST.exe
sophosui.exe
avastui.exe
paui.exe
bcc.exe
psuamain.exe
tesvc.exe
coreframeworkhost.exe
RepUtils.exe
updaterui.exe
cptrayui.exe
savservice.exe
SBAMSvc.exe
sophossafestore.exe
epam_svc.exe
sdcservice.exe
hostedagent.exe
idafserverhostservice.exe
sentinelhelperservice.exe
cetasvc.exe
macmnsvc.exe
sepagent.exe
responseservice.exe
mbcloudea.exe
SophosCleanM64.exe
easervicemonitor.exe
mfeann.exe
SophosNtpService.exe
ekrn.exe
MsMpEng.exe
QcShm.exe
mfeatp.exe
mfehcs.exe
mcshield.exe
Launch.exe
MclnstruTrack.exe
MfeAVSvc.exe
avastsvc.exe
ntrtscan.exe
svcgenerichost.exe
psanhost.exe
swi_service.exe
clientmanager.exe
repmgr.exe
tmlisten.exe
cptraylogic.exe
savapi.exe
vstskmgr.exe
wrsa.exe
efrservice.exe
scanhost.exe
sophosfimservice.exe
hostedagent.exe
SentinelAgentWorker.exe
cloudendpointservice.exe
logwriter.exe
SentinelUl.exe
dsa-connect.exe
mbamservice.exe
smcgui.exe
endpoint agent tray.exe
mctray.exe
SophosHealth.exe
eguiProxy.exe
agentsvc.exe
ModuleCoreService.exe
mfeesp.exe
mfeensppl
mfetp.exe
delegate.exe
McUICnt.exe
alsvc.exe
notifier.exe
sspservice.exe
pccntmon.exe
swi_fc.exe
ccsvchst.exe
remediationservice.exe
tmcpmadapter.exe
cpda.exe
savadminservice.exe
VipreNis.exe
ds_monitor.exe
dsa.exe
sbpimsvc.exe
sophososquery.exe
hmpalert.exe
sentinelagent.exe
sophos ui.exe
klnagent.exe
sentinelstaticenginescanner.exe
wscommunicator.exe
masvc.exe
sfc.exe
fsagentservice.exe
mcsclient.exe
sophosfs.exe
avwrapper.exe
```  

