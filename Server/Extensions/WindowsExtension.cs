using Microsoft.Win32;

namespace UtilityBox.App.Server.Extensions
{
    public static class WindowsExtension
    {
        public static bool IsWindows10(int aboveBuildNumber)
        {
            try
            {
                var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                var productName = (string)reg.GetValue("ProductName");

                var result = productName.StartsWith("Windows 10");
                var build = 0;

                if (result)
                    build = int.Parse((string)reg.GetValue("CurrentBuild"));

                return result && build >= aboveBuildNumber;
            }
            catch
            {
                return false;
            }
        }
    }
}