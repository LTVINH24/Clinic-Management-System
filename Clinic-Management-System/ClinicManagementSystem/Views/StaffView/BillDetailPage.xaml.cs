using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Views.StaffView
{
    /// <summary>
    /// BillDetailPage là trang chi tiết hóa đơn
    /// </summary>
    public sealed partial class BillDetailPage : Page
    {
        public BillDetailViewModel ViewModel { get; }

        public BillDetailPage()
        {
            this.InitializeComponent();
            ViewModel = new BillDetailViewModel();
        }

        /// <summary>
        /// Xử lí sự kiện khi được chuyển đến trang
        /// </summary>
        /// <param name="e"></param>    
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is int billId)
            {
                ViewModel.LoadData(billId);
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