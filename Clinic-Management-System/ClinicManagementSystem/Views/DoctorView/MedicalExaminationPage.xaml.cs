using ClinicManagementSystem.Model;
using ClinicManagementSystem.ViewModel;
using ClinicManagementSystem.Views.DoctorView;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace ClinicManagementSystem.Views
{
	public sealed partial class MedicalExaminationPage : Page
    {
        public MedicalExaminationPage()
        {
            this.InitializeComponent();
            this.DataContext = new MedicalExaminationViewModel(); // Gán DataContext tại đây
        }
		/// <summary>
		/// Xử lý sự kiện khi trang được điều hướng đến
		/// </summary>
		/// <param name="e"></param>
		/// <exception cref="InvalidOperationException"></exception>
		protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Frame navigationFrame)
            {
                ((MedicalExaminationViewModel)this.DataContext).NavigationFrame = navigationFrame;
            }
            else
            {
                throw new InvalidOperationException("Không thể lấy Frame từ tham số điều hướng.");
            }
        }
		/// <summary>
		/// Xử lý sự kiện khi chọn một phiếu khám bệnh
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is MedicalExaminationForm selectedForm)
            {
                // Sử dụng NavigationFrame của ViewModel để điều hướng
                ((MedicalExaminationViewModel)this.DataContext).NavigateToDiagnosisPage(selectedForm);
            }
        }
    }
}
