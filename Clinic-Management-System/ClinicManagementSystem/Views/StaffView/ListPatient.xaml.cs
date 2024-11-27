using ClinicManagementSystem.Model;
using ClinicManagementSystem.ViewModel;
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

namespace ClinicManagementSystem.Views.StaffView
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ListPatient : Page
	{
		public ListPatient()
		{
			ViewModel = new PatientViewModel();
			this.DataContext = ViewModel;
			this.InitializeComponent();
		}

		public PatientViewModel ViewModel { get; set; }
		bool init = false;

		private void searchTextbox_Click(object sender, TextChangedEventArgs e)
		{
			ViewModel.GoToNextPage();
		}

		private void nextButton_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.GoToNextPage();
		}

		private void previousButton_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.GoToPreviousPage();
		}
		private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(init == false)
			{
				init = true;
				return;
			}
			if(pagesComboBox.SelectedIndex >= 0)
			{
				var item = pagesComboBox.SelectedItem as PageInfo;
				ViewModel.GoToPage(item.Page);
			}
		}

		private void listPatientSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var patientEdit = itemsComboBox.SelectedItems as Patient;
			ViewModel.Edit(patientEdit);
		}

		private void searchButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void update_editUser(object sender, RoutedEventArgs e)
		{
			var success = ViewModel.Update();
			ViewModel.LoadData();
			string notify = "";
			if (success)
			{
				notify = "Updated successfully";
			}
			else
			{
				notify = "Update failed";
			}
			Notify(notify);
		}

		private void delete_editUser(object sender, RoutedEventArgs e)
		{

		}

		private void cancel_editUser(object sender, RoutedEventArgs e)
		{

		}



		private async void Notify(string notify)
		{
			await new ContentDialog()
			{
				XamlRoot = this.Content.XamlRoot,
				Title = "Notify",
				Content = $"{notify}",
				CloseButtonText = "OK"
			}.ShowAsync();
		}
	}
}
