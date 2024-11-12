using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace SetLuma
{
    public static class BrightnessControl
    {
        private const int MONITOR_DEFAULTTONEAREST = 2;
        private const int PHYSICAL_MONITOR_DESCRIPTION_SIZE = 128;
        private const int MC_CAPS_BRIGHTNESS = 0x2;

        [StructLayout(LayoutKind.Sequential)]
        public struct PHYSICAL_MONITOR
        {
            public IntPtr hPhysicalMonitor;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U2, SizeConst = PHYSICAL_MONITOR_DESCRIPTION_SIZE)]
            public char[] szPhysicalMonitorDescription;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private extern static bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll", SetLastError = false)]
        private extern static IntPtr MonitorFromPoint(POINT pt, uint dwFlags);

        [DllImport("dxva2.dll", SetLastError = true)]
        private extern static bool GetPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, uint dwPhysicalMonitorArraySize, [Out] PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        [DllImport("dxva2.dll", SetLastError = true)]
        private extern static bool GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, out uint pdwNumberOfPhysicalMonitors);

        [DllImport("dxva2.dll", SetLastError = true)]
        private extern static bool GetMonitorCapabilities(IntPtr hMonitor, out uint pdwMonitorCapabilities, out uint pdwSupportedColorTemperatures);

        [DllImport("dxva2.dll", SetLastError = true)]
        private extern static bool GetMonitorBrightness(IntPtr hMonitor, out uint pdwMinimumBrightness, out uint pdwCurrentBrightness, out uint pdwMaximumBrightness);

        [DllImport("dxva2.dll", SetLastError = true)]
        private extern static bool SetMonitorBrightness(IntPtr hMonitor, uint dwNewBrightness);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        public static IntPtr GetCurrentMonitor()
        {
            if (!GetCursorPos(out POINT point))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return MonitorFromPoint(point, MONITOR_DEFAULTTONEAREST);
        }

        public static PHYSICAL_MONITOR[] GetPhysicalMonitors(IntPtr hMonitor)
        {
            uint dwNumberOfPhysicalMonitors;
            if (!GetNumberOfPhysicalMonitorsFromHMONITOR(hMonitor, out dwNumberOfPhysicalMonitors))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            PHYSICAL_MONITOR[] physicalMonitorArray = new PHYSICAL_MONITOR[dwNumberOfPhysicalMonitors];
            if (!GetPhysicalMonitorsFromHMONITOR(hMonitor, dwNumberOfPhysicalMonitors, physicalMonitorArray))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return physicalMonitorArray;
        }

        public static int GetBrightness()
        {
            try
            {
                IntPtr hMonitor = GetCurrentMonitor();
                PHYSICAL_MONITOR[] physicalMonitors = GetPhysicalMonitors(hMonitor);
                foreach (var physicalMonitor in physicalMonitors)
                {
                    uint minBrightness, currentBrightness, maxBrightness;
                    if (GetMonitorBrightness(physicalMonitor.hPhysicalMonitor, out minBrightness, out currentBrightness, out maxBrightness))
                    {
                        return (int)((double)(currentBrightness - minBrightness) / (maxBrightness - minBrightness) * 100);
                    }
                }
            }
            catch (Exception)
            {
            }
            return 50;
        }

        public static void SetBrightness(int brightness)
        {
            try
            {
                IntPtr hMonitor = GetCurrentMonitor();
                PHYSICAL_MONITOR[] physicalMonitors = GetPhysicalMonitors(hMonitor);
                foreach (var physicalMonitor in physicalMonitors)
                {
                    uint minBrightness, currentBrightness, maxBrightness;
                    if (GetMonitorBrightness(physicalMonitor.hPhysicalMonitor, out minBrightness, out currentBrightness, out maxBrightness))
                    {
                        uint newBrightness = (uint)(minBrightness + (maxBrightness - minBrightness) * (brightness / 100.0));
                        if (!SetMonitorBrightness(physicalMonitor.hPhysicalMonitor, newBrightness))
                        {
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
