using ClinicManagementSystem.Views.AdminView;
using ClinicManagementSystem.Views.StaffView;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ClinicManagementSystem.Views
{
	/// <summary>
	/// Trang admin
	/// </summary>
	public sealed partial class adminPage : Page
	{
		public adminPage()
		{
			this.InitializeComponent();

            nvSample.Loaded += (s, e) =>
            {
                nvSample.IsPaneOpen = false;
                contentFrame.Navigate(typeof(AdminHomePage));
                nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
                                        .FirstOrDefault(x => x.Tag.ToString() == "AdminHomePage");
            };

            contentFrame.Navigated += ContentFrame_Navigated;
        }
		/// <summary>
		/// Xử lí sự kiện khi chuyển trang
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.SourcePageType == typeof(AdminHomePage))
            {
                nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
                                        .FirstOrDefault(x => x.Tag.ToString() == "AdminHomePage");
            }
            else if (e.SourcePageType == typeof(addAccount))
            {
                nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
                                        .FirstOrDefault(x => x.Tag.ToString() == "addAccount");
            }
            else if (e.SourcePageType == typeof(adminPage))
            {
                nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
                                        .FirstOrDefault(x => x.Tag.ToString() == "adminPage");
            }
            else if (e.SourcePageType == typeof(listAccount))
            {
                nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
                                        .FirstOrDefault(x => x.Tag.ToString() == "listAccount");
            }
            else if (e.SourcePageType == typeof(Medicine))
            {
                nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
                                        .FirstOrDefault(x => x.Tag.ToString() == "Medicine");
            }
            else if (e.SourcePageType == typeof(reportBillAdmin))
            {
                nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
                                        .FirstOrDefault(x => x.Tag.ToString() == "reportBillAdmin");
            }
            else if (e.SourcePageType == typeof(reportMedicineAdmin))
            {
                nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
                                        .FirstOrDefault(x => x.Tag.ToString() == "reportMedicineAdmin");
            }
            else if (e.SourcePageType == typeof(reportPatientVisitsAdmin))
            {
                nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
                                        .FirstOrDefault(x => x.Tag.ToString() == "reportPatientVisitsAdmin");
            }
        }
        /// <summary>
        /// Xử lí sự kiện khi chọn mục trong NavigationView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
		{
			if (args.IsSettingsSelected)
			{
				contentFrame.Navigate(typeof(SettingsPage));
				return;
			}

			if (args.SelectedItemContainer is NavigationViewItem selectedItem)
			{
				string selectedTag = selectedItem.Tag?.ToString();
				if (string.IsNullOrEmpty(selectedTag))
					return;

				switch (selectedTag)
				{
					case "AdminHomePage":
						contentFrame.Navigate(typeof(AdminHomePage));
						break;
					case "addAccount":
						contentFrame.Navigate(typeof(addAccount));
						break;
					case "listAccount":
						contentFrame.Navigate(typeof(listAccount));
						break;
					case "Medicine":
						contentFrame.Navigate(typeof(Medicine));
						break;
					case "report":
						break;
					case "reportBillAdmin":
						contentFrame.Navigate(typeof(reportBillAdmin));
						break;
					case "reportMedicineAdmin":
						contentFrame.Navigate(typeof(reportMedicineAdmin));
						break;
					case "reportPatientVisitsAdmin":
						contentFrame.Navigate(typeof(reportPatientVisitsAdmin));
						break;
					default:
						break;
				}
			}
		}
	}
}
