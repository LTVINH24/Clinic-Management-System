﻿using ClinicManagementSystem.Model;
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
	/// Trang hiển thị danh sách bệnh nhân
	/// </summary>
	public sealed partial class ListPatient : Page
	{
		public PatientViewModel ViewModel { get; set; }
		public ListPatient()
		{
			ViewModel = new PatientViewModel();
			this.DataContext = ViewModel;
			this.InitializeComponent();

			DragArea.PointerEntered += (s, e) => {
				HoverOverlay.Opacity = 0.1;
			};

			DragArea.PointerExited += (s, e) => {
				HoverOverlay.Opacity = 0;
			};
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
			if(pagesComboBox.SelectedIndex >= 0)
			{
				var item = pagesComboBox.SelectedItem as PageInfo;
				if (item != null)
				{
					ViewModel.GoToPage(item.Page);
				}
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
			var (isSuccess, message) = ViewModel.Update();
			ViewModel.LoadData();
			string notify = "";
			if (isSuccess)
			{
				notify = "Updated successfully.";
			}
			else
			{
				notify = $"Update failed. {message}";
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
		/// <summary>
		/// Xử lí sự kiện khi chọn button ClosePopup
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ClosePopup_Click(object sender, RoutedEventArgs e)
		{
			EditPopup.IsOpen = false;
			EditPopup.HorizontalOffset = 0;
    		EditPopup.VerticalOffset = 0;
		}
		/// <summary>
		/// Xử lí sự kiện khi chọn button Edit
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Edit_Click(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			editPatient = button?.DataContext as Patient;
			ViewModel.Edit(editPatient);
			EditPopup.IsOpen = true;
		}
		/// <summary>
		/// Xử lí sự kiện khi chọn button SendMail
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
		/// <summary>
		/// Xử lí sự kiện khi chọn button clear filter
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ClearFilter_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.StartDateFollowUp = null;
			ViewModel.EndDateFollowUp = null;
			ViewModel.LoadData();
		}

		private bool isDragging = false;
		private Windows.Foundation.Point initialPosition;
		private Windows.Foundation.Point popupPosition;
		/// <summary>
		/// Xử lí sự kiện kéo thả popup
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DragArea_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			isDragging = true;
			var properties = e.GetCurrentPoint(null).Properties;
			if (properties.IsLeftButtonPressed)
			{
				((UIElement)sender).CapturePointer(e.Pointer);
				initialPosition = e.GetCurrentPoint(null).Position;
				popupPosition = new Windows.Foundation.Point(EditPopup.HorizontalOffset, EditPopup.VerticalOffset);
			}
		}
		/// <summary>
		/// Xử lí sự kiện di chuyển popup
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DragArea_PointerMoved(object sender, PointerRoutedEventArgs e)
		{
			if (isDragging)
			{
				var currentPosition = e.GetCurrentPoint(null).Position;
				var deltaX = currentPosition.X - initialPosition.X;
				var deltaY = currentPosition.Y - initialPosition.Y;

				EditPopup.HorizontalOffset = popupPosition.X + deltaX;
				EditPopup.VerticalOffset = popupPosition.Y + deltaY;
			}
		}
		/// <summary>
		/// Xử lí sự kiện di chuyển popup
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DragArea_PointerReleased(object sender, PointerRoutedEventArgs e)
		{
			isDragging = false;
			((UIElement)sender).ReleasePointerCapture(e.Pointer);
		}
	}
}
