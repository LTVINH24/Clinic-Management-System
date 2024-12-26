using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service;
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
using System.Threading.Tasks;
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

		public ClinicManagementSystem.Model.Patient editPatient;

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
		private async void updatePatient(object sender, RoutedEventArgs e)
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
			EditPopup.IsOpen = false;
			await Notify(notify);
		}

		/// <summary>
		/// Xử lí sự kiện xóa thông tin bệnh nhân
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void Delete_Click(object sender, RoutedEventArgs e)
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
				await Notify(notify);
			}
		}


		/// <summary>
		/// Hiển thị thông báo
		/// </summary>
		/// <param name="notify"></param>
		private async Task Notify(string notify)
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

		private void ClosePopup_Click(object sender, RoutedEventArgs e)
		{
			EditPopup.IsOpen = false;
		}

		private void Edit_Click(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			editPatient = button?.DataContext as Patient;
			ViewModel.Edit(editPatient);
			EditPopup.IsOpen = true;
		}

		private async void SendMail_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				LoadingRing.IsActive = true;
				LoadingPanel.Visibility = Visibility.Visible;

				var button = sender as Button;
				var patient = button.DataContext as Patient;

				if (string.IsNullOrEmpty(patient.Email))
				{
					await Notify("Patient does not have an email address!");
					return;
				}

				var emailService = new EmailService();
				string subject = "Nhắc lịch tái khám";
				string body = $@"
				<html>
					<body>
						<h3>Kính gửi {patient.Name},</h3>
						<p>Chúng tôi gửi thông tin về lịch tái khám của bạn:</p>
						<ul>
							<li>Họ tên: {patient.Name}</li>
							<li>Ngày sinh: {patient.DoB:dd/MM/yyyy}</li>
							<li>Địa chỉ: {patient.Address}</li>
							<li><b>Ngày tái khám: {patient.NextExaminationDate:dd/MM/yyyy}</b></li>
						</ul>
						<p>Vui lòng kiểm tra và phản hồi nếu có bất kỳ thắc mắc nào.</p>
						<br/>
						<p>Trân trọng,</p>
						<p>Phòng khám VTV</p>
					</body>
				</html>";

				await emailService.SendEmailAsync(patient.Email, subject, body);
				await Notify("Email has been sent successfully!");
			}
			catch (Exception ex)
			{
				await Notify($"Error sending email: {ex.Message}");
			}
			finally
			{
				LoadingRing.IsActive = false;
				LoadingPanel.Visibility = Visibility.Collapsed;
			}
		}

		private void ClearFilter_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.StartDateFollowUp = null;
			ViewModel.EndDateFollowUp = null;
			ViewModel.LoadData();
		}
    }
}
