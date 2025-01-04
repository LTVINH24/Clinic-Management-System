using System;
using Microsoft.UI;
using Microsoft.UI.Xaml;

namespace ClinicManagementSystem.Helper
{
    public static class WindowHelper
    {
        private static Window mainWindow;
        
        public static void Initialize(Window window)
        {
            mainWindow = window;
        }

        public static IntPtr GetActiveWindow()
        {
            return WinRT.Interop.WindowNative.GetWindowHandle(mainWindow);
        }
    }
}