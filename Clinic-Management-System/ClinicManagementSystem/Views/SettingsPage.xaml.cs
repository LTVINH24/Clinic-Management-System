using ClinicManagementSystem.Helper;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.ViewModel.EndUser;
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
        public InformationViewModel viewModel { get; private set; }

        public SettingsPage()
        {
            this.InitializeComponent();
            viewModel = new InformationViewModel();
            viewModel.LoadInformationUser();
            LoadThemeSettings();
        }
        private void GenderMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem)
            {
                GenderDropDown.Content = menuItem.Text;
            }
        }

        private bool ValidData()
        {
            var valid = new IsValidData();
            if (!valid.IsValidName(NameUser.Text))
            {
                Notify("Please enter a valid name");
                return false;
            }
            if (!valid.IsValidPhone(PhoneUser.Text))
            {
                Notify("Please enter a valid phone number");
                return false;
            }
            if (!valid.IsValidAddress(AddressUser.Text))
            {
                Notify("Please enter a valid address");
                return false;
            }
            if (!valid.IsValidDatePicker(BirthDayUser.Date))
            {
                Notify("Please choose a valid birthday");
                return false;
            }
            return true;
        }
        private async void Notify(string notify)
        {
            var currentTheme = ThemeService.Instance.GetCurrentTheme();
            ElementTheme dialogTheme;

            switch (currentTheme)
            {
                case "Light":
                    dialogTheme = ElementTheme.Light;
                    break;
                case "Dark":
                    dialogTheme = ElementTheme.Dark;
                    break;
                default:
                    dialogTheme = ElementTheme.Default;
                    break;
            }

            await new ContentDialog()
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Notify",
                Content = $"{notify}",
                CloseButtonText = "OK",
                RequestedTheme = dialogTheme
            }.ShowAsync();
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (ValidData())
            {
                bool success = viewModel.UpdateInformationUser();
                if (success)
                {
                    Notify("Updated successfully");
                }
                else
                {
                    Notify("Updated failed");
                }
            }
        }
        private void LoadThemeSettings()
        {
			var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
			
			if (!localSettings.Values.ContainsKey("AppTheme"))
			{
				localSettings.Values["AppTheme"] = "System";
			}
					
			string currentTheme = localSettings.Values["AppTheme"] as string ?? "System";

			switch (currentTheme)
			{
				case "Light":
					LightTheme.IsChecked = true;
					break;
				case "Dark":
					DarkTheme.IsChecked = true;
					break;
				default:
					DefaultTheme.IsChecked = true;
					break;
			}
		}

        private void ThemeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
			if (sender is RadioButton radioButton && radioButton.IsChecked == true)
			{
				string newTheme = radioButton.Content.ToString();
				var currentWindow = ShellWindow.Current;
				if (currentWindow != null)
				{
					ThemeService.Instance.SetTheme(currentWindow, newTheme);
				}
			}
		}

		private async void LogoutButton_Click(object sender, RoutedEventArgs e)
		{
            var currentTheme = ThemeService.Instance.GetCurrentTheme();
            ElementTheme dialogTheme;

			switch (currentTheme)
			{
				case "Light":
					dialogTheme = ElementTheme.Light;
					break;
				case "Dark":
					dialogTheme = ElementTheme.Dark;
					break;
				default:
					dialogTheme = ElementTheme.Default;
					break;
			}


			var confirmDialog = new ContentDialog()
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Confirm Logout",
                Content = "Are you sure you want to logout?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No",
                RequestedTheme = dialogTheme
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
