using ClinicManagementSystem.Views.DoctorView;
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
	/// Trang bác sĩ
	/// </summary>
	public sealed partial class doctorPage : Page
	{
		public doctorPage()
		{
			this.InitializeComponent();

			// Thiết lập trang mặc định khi khởi động
			contentFrame.Navigate(typeof(DoctorHomePage), contentFrame);

			nvSample.Loaded += (s, e) =>
			{
				nvSample.IsPaneOpen = false;
				// Chọn item mặc định trong navigation
				nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
										.FirstOrDefault(x => x.Tag.ToString() == "DoctorPage");
			};

			contentFrame.Navigated += ContentFrame_Navigated;
		}

		private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
		{
			if (e.SourcePageType == typeof(DoctorHomePage))
			{
				nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
										.FirstOrDefault(x => x.Tag.ToString() == "DoctorPage");
			}
			else if (e.SourcePageType == typeof(MedicalExaminationPage))
			{
				nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
										.FirstOrDefault(x => x.Tag.ToString() == "MedicalExaminationPage");
			}
			else if (e.SourcePageType == typeof(ExaminedFormsPage))
			{
				nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
										.FirstOrDefault(x => x.Tag.ToString() == "ExaminedFormsPage");
			}
			else if (e.SourcePageType == typeof(DocumentPage))
			{
				nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>()
										.FirstOrDefault(x => x.Tag.ToString() == "DocumentPage");
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
					case "DoctorPage":
						contentFrame.Navigate(typeof(DoctorHomePage), contentFrame);
						break;
					case "MedicalExaminationPage":
						contentFrame.Navigate(typeof(MedicalExaminationPage), contentFrame);
						break;
					case "ExaminedFormsPage":
						contentFrame.Navigate(typeof(ExaminedFormsPage), contentFrame);
						break;
					case "DocumentPage":
						contentFrame.Navigate(typeof(DocumentPage), contentFrame);
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
