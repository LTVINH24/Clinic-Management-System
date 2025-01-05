using System;
using Microsoft.UI;
using Microsoft.UI.Xaml;

namespace ClinicManagementSystem.Helper
{
    /// <summary>
    /// Helper cho các window
    /// </summary>
    public static class WindowHelper
    {
        private static Window mainWindow;
        
        /// <summary>
        /// Khởi tạo window
        /// </summary>
        /// <param name="window">Window</param>
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