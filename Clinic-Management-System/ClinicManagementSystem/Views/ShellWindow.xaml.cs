using ClinicManagementSystem.Service;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ClinicManagementSystem.Views
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ShellWindow : Window
	{
        private static ShellWindow _current;
        public static ShellWindow Current
        {
            get => _current;
            private set => _current = value;
        }
        public ShellWindow(string name)
		{
			Current = this;
			this.InitializeComponent();
			ThemeService.Instance.SetTheme(this, ThemeService.Instance.GetCurrentTheme());
			
			var fullNamespace = $"{GetType().Namespace}.{name}";
			var type = Type.GetType(fullNamespace);
			
			content.Navigate(type);

			this.Title = "Clinic Management System";
		}

		/// <summary>
		/// Xử lí sự kiện khi cửa sổ được đóng
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void Window_Closed(object sender, WindowEventArgs args)
		{
			Current = null;
			
			if (UserSessionService.Instance.GetLoggedInUserId() != 0)
			{
				var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
				bool isRemember = localSettings.Values.ContainsKey("username") &&
						   localSettings.Values.ContainsKey("password");

				UserSessionService.Instance.ClearSession(true);
			}
			
			var loginWindow = new MainWindow();
			loginWindow.Activate();
		}
        public IntPtr GetWindowHandle()
        {
            return WinRT.Interop.WindowNative.GetWindowHandle(this);
        }

        public XamlRoot GetXamlRoot()
        {
            return Content.XamlRoot;
        }

    }
}
