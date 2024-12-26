using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ClinicManagementSystem.Views;

namespace ClinicManagementSystem.Service
{
	public class ThemeService
	{
		private static ThemeService _instance;
		public static ThemeService Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ThemeService();
				}
				return _instance;
			}
		}

		public void SetTheme(Window window, string theme)
		{
			var localSettings = ApplicationData.Current.LocalSettings;
			localSettings.Values["AppTheme"] = theme;

			if (window.Content is FrameworkElement rootElement)
			{
				ElementTheme elementTheme;
				switch (theme)
				{
					case "Light":
						elementTheme = ElementTheme.Light;
						break;
					case "Dark":
						elementTheme = ElementTheme.Dark;
						break;
					default:
						elementTheme = ElementTheme.Default;
						break;
				}

				rootElement.RequestedTheme = elementTheme;
				if (window is ShellWindow shell && shell.Content is Frame frame)
				{
					frame.RequestedTheme = elementTheme;
					if (frame.Content is Page page)
					{
						page.RequestedTheme = elementTheme;
					}
				}
			}
		}

		public string GetCurrentTheme()
		{
			var localSettings = ApplicationData.Current.LocalSettings;
			return localSettings.Values["AppTheme"] as string ?? "System";
		}
	}
}
