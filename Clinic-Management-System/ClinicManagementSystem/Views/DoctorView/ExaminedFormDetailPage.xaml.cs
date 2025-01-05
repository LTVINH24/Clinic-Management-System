using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Views.DoctorView
{
    /// <summary>
    /// ExaminedFormDetailPage là trang chi tiết phiếu khám bệnh
    /// </summary>
    public sealed partial class ExaminedFormDetailPage : Page
    {
        public ExaminedFormDetailViewModel ViewModel { get; }

        public ExaminedFormDetailPage()
        {
            this.InitializeComponent();
            ViewModel = new ExaminedFormDetailViewModel();
        }

        /// <summary>
        /// Xử lí sự kiện khi được chuyển đến trang
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is MedicalExaminationForm form)
            {
                ViewModel.LoadData(form);
            }
        }

        /// <summary>
        /// Xử lí sự kiện khi nhấn nút Back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
} 