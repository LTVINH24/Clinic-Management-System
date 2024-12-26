using ClinicManagementSystem.Service;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.Storage;
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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            LoadThemeSettings();
        }

        private void LoadThemeSettings()
        {
            
        }

        private void ThemeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            
        }

		private async void LogoutButton_Click(object sender, RoutedEventArgs e)
		{
            var confirmDialog = new ContentDialog()
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Confirm Logout",
                Content = "Are you sure you want to logout?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No"
            };

            var result = await confirmDialog.ShowAsync();
            
            if (result == ContentDialogResult.Primary)
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                bool isRemember = localSettings.Values.ContainsKey("username") 
                                && localSettings.Values.ContainsKey("password");

                UserSessionService.Instance.ClearSession(isRemember);

                var currentWindow = ShellWindow.Current;
                if (currentWindow != null)
                {
                    currentWindow.Close();
                }
            }
        }
    }
}
