using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Rainmeter;

namespace ChromeCheckPlugin
{
    public class Measure
    {
        // Check if Google Chrome is installed by looking at the registry and file system.
        internal bool IsChromeInstalled()
        {
            // Check registry for Chrome entry
            if (IsChromeInRegistry())
            {
                return true;
            }

            // Check the common installation paths for Chrome
            if (IsChromeInFileSystem())
            {
                return true;
            }

            return false;
        }

        private bool IsChromeInRegistry()
        {
            string[] registryPaths = new string[]
            {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall" // For 32-bit applications on 64-bit OS
            };

            foreach (var registryPath in registryPaths)
            {
                using (var key = Registry.LocalMachine.OpenSubKey(registryPath))
                {
                    if (key == null) continue;

                    // Iterate through subkeys to find "Google Chrome".
                    foreach (var subKeyName in key.GetSubKeyNames())
                    {
                        using (var subKey = key.OpenSubKey(subKeyName))
                        {
                            var displayName = subKey.GetValue("DisplayName") as string;
                            if (displayName != null && displayName.Contains("Google Chrome"))
                            {
                                return true; // Chrome is installed in the registry.
                            }
                        }
                    }
                }
            }

            return false; // No Chrome entry in the registry.
        }

        private bool IsChromeInFileSystem()
        {
            // Common Chrome installation paths to check.
            string[] chromePaths = new string[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Google", "Chrome", "Application", "chrome.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Google", "Chrome", "Application", "chrome.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google", "Chrome", "Application", "chrome.exe"),
            };

            // Check each path for the existence of chrome.exe
            return chromePaths.Any(File.Exists);
        }

        internal double Update()
        {
            // Return 1 if Chrome is installed, 0 if not.
            return IsChromeInstalled() ? 1.0 : 0.0;
        }

        internal string GetString()
        {
            // Return a string indicating whether Chrome is installed.
            return IsChromeInstalled() ? "1" : "0";
        }
    }

    public static class Plugin
    {
        static IntPtr StringBuffer = IntPtr.Zero;

        [DllExport]
        public static void Initialize(ref IntPtr data, IntPtr rm)
        {
            data = GCHandle.ToIntPtr(GCHandle.Alloc(new Measure()));
        }

        [DllExport]
        public static void Finalize(IntPtr data)
        {
            GCHandle.FromIntPtr(data).Free();

            if (StringBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(StringBuffer);
                StringBuffer = IntPtr.Zero;
            }
        }

        [DllExport]
        public static void Reload(IntPtr data, IntPtr rm, ref double maxValue)
        {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            // Reload any necessary data here.
        }

        [DllExport]
        public static double Update(IntPtr data)
        {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            return measure.Update();
        }

        [DllExport]
        public static IntPtr GetString(IntPtr data)
        {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            if (StringBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(StringBuffer);
                StringBuffer = IntPtr.Zero;
            }

            string stringValue = measure.GetString();
            if (stringValue != null)
            {
                StringBuffer = Marshal.StringToHGlobalUni(stringValue);
            }

            return StringBuffer;
        }
    }
}
