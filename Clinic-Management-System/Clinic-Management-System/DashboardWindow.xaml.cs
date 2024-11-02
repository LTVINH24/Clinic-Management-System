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

namespace Clinic_Management_System
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class DashboardWindow : Window
	{
		public DashboardWindow()
		{
			this.InitializeComponent();

			nvSample8.Loaded += (s, e) =>
			{
				nvSample8.IsPaneOpen = false;
			};

		}

		//private void pageButton_Click(object sender, RoutedEventArgs e)
		//{
		//	var button = sender as Button;
		//	var pageName = button.Tag as string;
		//	var screen = new ShellWindow(pageName);
		//	screen.Activate();

		//	this.Close();
		//}

		private void NavigationView_SelectionChanged8(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
		{
			
			if (args.IsSettingsSelected == false && args.SelectedItemContainer is NavigationViewItem selectedItem)
			{
				
				string selectedTag = selectedItem.Tag.ToString();

				
				switch (selectedTag)
				{
					case "SamplePage1":
						contentFrame8.Navigate(typeof(SamplePage1));
						break;
					case "SamplePage2":
						contentFrame8.Navigate(typeof(SamplePage2));
						break;
					case "SamplePage3":
						contentFrame8.Navigate(typeof(SamplePage3));
						break;
					case "SamplePage4":
						contentFrame8.Navigate(typeof(SamplePage4));
						break;
					case "SamplePage5":
						contentFrame8.Navigate(typeof(SamplePage5));
						break;
					case "SamplePage6":
						contentFrame8.Navigate(typeof(SamplePage6));
						break;
					default:
						break;
				}
			}
			else
			{
				contentFrame8.Navigate(typeof(SettingsPage));
			}
		}

	}
}
