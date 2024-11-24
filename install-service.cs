using System;
using System.Runtime.InteropServices;

class DriverServiceInstaller
{
    const int SERVICE_KERNEL_DRIVER = 0x00000001;
    const int SERVICE_DEMAND_START = 0x00000003;
    const int SERVICE_ERROR_NORMAL = 0x00000001;

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr OpenSCManager(string lpMachineName, string lpDatabaseName, uint dwDesiredAccess);

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr CreateService(
        IntPtr hSCManager,
        string lpServiceName,
        string lpDisplayName,
        uint dwDesiredAccess,
        uint dwServiceType,
        uint dwStartType,
        uint dwErrorControl,
        string lpBinaryPathName,
        string lpLoadOrderGroup,
        IntPtr lpdwTagId,
        string lpDependencies,
        string lpServiceStartName,
        string lpPassword);

    [DllImport("advapi32.dll", SetLastError = true)]
    public static extern bool CloseServiceHandle(IntPtr hSCObject);

    [DllImport("advapi32.dll", SetLastError = true)]
    public static extern bool StartService(IntPtr hService, int dwNumServiceArgs, IntPtr lpServiceArgVectors);

    static void Main(string[] args)
    {
        string serviceName = "aswSP_Avar"; // Name of the service
        string driverPath = @"C:\<Path to driver>\aswSP.sys";

        IntPtr scmHandle = OpenSCManager(null, null, 0x000F003F); // SC_MANAGER_ALL_ACCESS
        if (scmHandle == IntPtr.Zero)
        {
            Console.WriteLine("Failed to open the Service Control Manager. Error: " + Marshal.GetLastWin32Error());
            return;
        }

        IntPtr serviceHandle = CreateService(
            scmHandle,
            serviceName,
            serviceName,
            0xF01FF, // SERVICE_ALL_ACCESS
            SERVICE_KERNEL_DRIVER,
            SERVICE_DEMAND_START,
            SERVICE_ERROR_NORMAL,
            driverPath,
            null,
            IntPtr.Zero,
            null,
            null,
            null);

        if (serviceHandle == IntPtr.Zero)
        {
            Console.WriteLine("Failed to create the service. Error: " + Marshal.GetLastWin32Error());
        }
        else
        {
            Console.WriteLine("Service created successfully!");

            // Start the service
            if (StartService(serviceHandle, 0, IntPtr.Zero))
            {
                Console.WriteLine("Service started successfully!");
            }
            else
            {
                Console.WriteLine("Failed to start the service. Error: " + Marshal.GetLastWin32Error());
            }

            CloseServiceHandle(serviceHandle);
        }

        CloseServiceHandle(scmHandle);
    }
}
