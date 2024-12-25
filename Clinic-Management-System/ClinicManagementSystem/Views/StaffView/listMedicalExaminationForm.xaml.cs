using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service.DataAccess;
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
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ClinicManagementSystem.Views.StaffView
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class listMedicalExaminationForm : Page
	{
		public MedicalExaminationFormViewModel ViewModel { get; set; }
		public listMedicalExaminationForm()
		{
			ViewModel = new MedicalExaminationFormViewModel();
			this.DataContext = ViewModel;
			this.InitializeComponent();
		}
		IDao _dao;


		public ClinicManagementSystem.Model.MedicalExaminationForm editForm { get; set; }

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

		bool init = false;

		/// <summary>
		/// Xử lí sự kiện khi chọn trang
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (init == false)
			{
				init = true;
				return;
			}
			if (pagesComboBox.SelectedIndex >= 0)
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
		/// Xử lí sự kiện khi nhấn nút Update
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void updateMedicalExaminationForm(object sender, RoutedEventArgs e)
		{
			var success = ViewModel.Update();
			ViewModel.LoadData();
			string notify = "";
			if (success)
			{
				notify = "Updated successfully.";
			}
			else
			{
				notify = "Update failed.";
			}
			EditPopup.IsOpen = false;
			await Notify(notify);
		}

		/// <summary>
		/// Xử lí sự kiện khi nhấn nút Delete
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void deleteMedicalExaminationForm(object sender, RoutedEventArgs e)
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

			if(result == ContentDialogResult.Primary)
			{
				var success = ViewModel.Delete();
				ViewModel.LoadData();
				string notify = "";
				if (success)
				{
					notify = "Deleted successfully.";
				}
				else
				{
					notify = "Delete failed.";
				}
				EditPopup.IsOpen = false;
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
		/// Xử lí sự kiện khi chọn loại khám
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void setVisitType(object sender, RoutedEventArgs e)
		{
			if (sender is MenuFlyoutItem menuItem)
			{
				ViewModel.FormEdit.VisitType = menuItem.Text;
			}
		}

		private void ClearFilter_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.StartDate = null;
			ViewModel.EndDate = null;
			ViewModel.LoadData();
		}


		private void ClosePopup_Click(object sender, RoutedEventArgs e)
		{
			EditPopup.IsOpen = false;
		}

		private void Edit_Click(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			editForm = button?.DataContext as MedicalExaminationForm;
			ViewModel.Edit(editForm);
			EditPopup.IsOpen = true;	
		}

		private async void SendResult_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				LoadingRing.IsActive = true;
				LoadingPanel.Visibility = Visibility.Visible;

				var button = sender as Button;
				var form = button?.DataContext as MedicalExaminationForm;

				if (form == null) return;

				var formDetail = ViewModel._dao.GetMedicalExaminationFormDetail(form.Id);

				if (string.IsNullOrEmpty(formDetail.PatientEmail))
				{
					await Notify("Patient does not have an email address!");
					return;
				}

				var emailService = new EmailService();
				string subject = "Kết quả khám bệnh";
				string body = $@"
					<html>
						<body>
							<h3>Kính gửi {formDetail.PatientName},</h3>
							<p>Chúng tôi gửi kết quả khám bệnh của bạn:</p>
							<ul>
								<li>Ngày khám: {formDetail.Time:dd/MM/yyyy}</li>
								<li>Triệu chứng: {formDetail.Symptoms}</li>
								<li>Chẩn đoán: {formDetail.Diagnosis}</li>
								<li>Bác sĩ khám: {formDetail.DoctorName}</li>
								<li>Đơn thuốc:</li>
								<ul>
									{string.Join("\n", formDetail.Medicines.Select(m =>
												$"<li>{m.MedicineName}: {m.Dosage} - Số lượng: {m.Quantity}</li>"))}
								</ul>
								{(formDetail.NextExaminationDate != null ? $"<li>Ngày tái khám: {formDetail.NextExaminationDate:dd/MM/yyyy}</li>" : "")}
							</ul>
							<p>Vui lòng liên hệ nếu cần thêm thông tin.</p>
							<p>Trân trọng,</p>
							<p>Phòng khám VTV</p>
						</body>
					</html>";

				await emailService.SendEmailAsync(formDetail.PatientEmail, subject, body);
				await Notify("Email sent successfully!");
			}
			catch (Exception ex)
			{
				await Notify(ex.Message);
			}
			finally
			{
				LoadingRing.IsActive = false;
				LoadingPanel.Visibility = Visibility.Collapsed;
			}
		}
    }	
}
