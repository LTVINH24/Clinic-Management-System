using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ClinicManagementSystem.ViewModel;
using ClinicManagementSystem.Model;

namespace ClinicManagementSystem.Views.StaffView
{
    /// <summary>
    /// UnbilledPrescriptionsDetailPage là trang chi tiết phiếu khám bệnh không được hóa đơn
    /// </summary>
    public sealed partial class UnbilledPrescriptionsDetailPage : Page
    {
        public UnbilledPrescriptionsDetailViewModel ViewModel { get; }

        public UnbilledPrescriptionsDetailPage()
        {
            this.InitializeComponent();
            ViewModel = new UnbilledPrescriptionsDetailViewModel();
        }

        /// <summary>
        /// Xử lí sự kiện khi được chuyển đến trang
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is int prescriptionId)
            {
                ViewModel.LoadData(prescriptionId);
            }
        }

        /// <summary>
        /// Xử lí sự kiện khi nhấn nút Save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("SaveButton clicked");

            if (ViewModel.SaveBill())
            {
                System.Diagnostics.Debug.WriteLine("Bill saved successfully");
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
            else
            {
                var dialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Error",
                    Content = "Failed to save bill. Please try again.",
                    CloseButtonText = "OK"
                };
                dialog.ShowAsync();
                System.Diagnostics.Debug.WriteLine("Failed to save bill");
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