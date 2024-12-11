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
		public PatientViewModel ViewModel { get; set; }
		public ListPatient()
		{
			ViewModel = new PatientViewModel();
			this.DataContext = ViewModel;
			this.InitializeComponent();
		}

		bool init = false;

		/// <summary>
		/// Xử lí sự kiện khi nhấn nút Next
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void nextButton_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.GoToNextPage();
		}

		/// <summary>
		/// Xử lí sự kiện khi nhấn nút Previous
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void previousButton_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.GoToPreviousPage();
		}

		/// <summary>
		/// Xử lí sự kiện khi chọn trang
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// Xử lí sự kiện khi nhập vào ô tìm kiếm
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void searchTextbox_Click(object sender, TextChangedEventArgs e)
		{
			ViewModel.Search();
		}

		/// <summary>
		/// Xử lí sự kiện khi nhấn nút Search
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void searchButton_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.Search();
		}

		/// <summary>
		/// Xử lí sự kiện update thông tin bệnh nhân
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void updatePatient(object sender, RoutedEventArgs e)
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

		/// <summary>
		/// Xử lí sự kiện xóa thông tin bệnh nhân
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void deletePatient(object sender, RoutedEventArgs e)
		{
			var confirmContentDialog = new ContentDialog
			{
				XamlRoot = this.Content.XamlRoot,
				Title = "Confirmation",
				Content = "Are you sure you want to delete this Medical Examination Form?",
				PrimaryButtonText = "Yes",
				SecondaryButtonText = "Cancel"
			};

			var result = await confirmContentDialog.ShowAsync();

			if (result == ContentDialogResult.Primary)
			{
				var success = ViewModel.Delete();
				ViewModel.LoadData();
				string notify = "";
				if (success)
				{
					notify = "Deleted successfully";
				}
				else
				{
					notify = "Delete failed";
				}
				Notify(notify);
			}
		}


		/// <summary>
		/// Hiển thị thông báo
		/// </summary>
		/// <param name="notify"></param>
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

		/// <summary>
		/// Xử lí sự kiện khi nhấn nút Cancel
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cancelEdit(object sender, RoutedEventArgs e)
		{
			ViewModel.LoadData();
		}

		/// <summary>
		/// Xử lí sự kiện khi chọn bệnh nhân
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Patient_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var patientEdit = itemsComboBox.SelectedItem as Patient;
			ViewModel.Edit(patientEdit);

			if (EditPanel.Visibility == Visibility.Visible && ViewModel.PatientEdit == patientEdit)
			{
				EditPanel.Visibility = Visibility.Collapsed;
				itemsComboBox.SelectedItem = null;
			}
			else
			{
				if (patientEdit != null)
				{
					ViewModel.Edit(patientEdit);
					EditPanel.Visibility = Visibility.Visible;
				}
			}
		}

		/// <summary>
		/// Xử lí sự kiện khi chọn giới tính
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void setGender(object sender, RoutedEventArgs e)
		{
			if (sender is MenuFlyoutItem menuItem)
			{
				ViewModel.PatientEdit.Gender = menuItem.Text;
			}
		}
	}
}
