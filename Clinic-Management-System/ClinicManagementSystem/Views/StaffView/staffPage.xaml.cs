﻿using ClinicManagementSystem.Views.StaffView;
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
			};
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
					case "Home":
						contentFrame.Navigate(typeof(HomePage));
						break;
					case "MedicalExaminationForm":
						contentFrame.Navigate(typeof(listMedicalExaminationForm));
						break;
					case "AddMedicalExaminationForm":
						contentFrame.Navigate(typeof(AddMedicalExaminationForm));
						break;
					case "ListPatient":
						contentFrame.Navigate(typeof(ListPatient));
						break;
					default:
						break;
				}
			}
			else
			{
				contentFrame.Navigate(typeof(SettingsPage));
			}
			//else
			//{
			//	contentFrame.Navigate(typeof(SettingsPage));
			//}
		}
	}
}
