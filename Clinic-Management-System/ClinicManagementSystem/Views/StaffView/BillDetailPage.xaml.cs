using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Views.StaffView
{
    public sealed partial class BillDetailPage : Page
    {
        public BillDetailViewModel ViewModel { get; }

        public BillDetailPage()
        {
            this.InitializeComponent();
            ViewModel = new BillDetailViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is int billId)
            {
                ViewModel.LoadData(billId);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
} 