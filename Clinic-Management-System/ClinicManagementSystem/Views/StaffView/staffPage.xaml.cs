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
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class staffPage : Page
	{
		public staffPage()
		{
			this.InitializeComponent();

			nvSample.Loaded += (s, e) =>
			{
				nvSample.IsPaneOpen = false;
				contentFrame.Navigate(typeof(StaffHomePage));
				nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
										.FirstOrDefault(x => x.Tag.ToString() == "StaffPage");
			};

			contentFrame.Navigated += ContentFrame_Navigated;
		}
		/// <summary>
		/// Xử lí sự kiện khi NavigationView đã được load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
		{
			if (e.SourcePageType == typeof(StaffHomePage))
			{
				nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
										.FirstOrDefault(x => x.Tag.ToString() == "StaffPage");
			}
			else if (e.SourcePageType == typeof(ListMedicalExaminationForm))
			{
				nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
										.FirstOrDefault(x => x.Tag.ToString() == "MedicalExaminationForm");
			}
			else if (e.SourcePageType == typeof(AddMedicalExaminationForm))
			{
				nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
										.FirstOrDefault(x => x.Tag.ToString() == "AddMedicalExaminationForm");
			}
			else if (e.SourcePageType == typeof(ListPatient))
			{
				nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
										.FirstOrDefault(x => x.Tag.ToString() == "ListPatient");
			}
		}

		/// <summary>
		/// Xử lí sự kiện khi chọn mục trong NavigationView
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
		{
			if (args.IsSettingsSelected == false && args.SelectedItemContainer is NavigationViewItem selectedItem)
			{

				string selectedTag = selectedItem.Tag.ToString();
				
				switch (selectedTag)
				{
					case "StaffPage":
						contentFrame.Navigate(typeof(StaffHomePage));
						break;
					case "MedicalExaminationForm":
						contentFrame.Navigate(typeof(ListMedicalExaminationForm));
						break;
					case "AddMedicalExaminationForm":
						contentFrame.Navigate(typeof(AddMedicalExaminationForm));
						break;
					case "ListPatient":
						contentFrame.Navigate(typeof(ListPatient));
						break;
					case "UnbilledPrescriptionsPage":
						contentFrame.Navigate(typeof(UnbilledPrescriptionsPage));
						break;
					default:
						break;
				}
			}
			else
			{
				contentFrame.Navigate(typeof(SettingsPage));
			}
		}
	}
}
