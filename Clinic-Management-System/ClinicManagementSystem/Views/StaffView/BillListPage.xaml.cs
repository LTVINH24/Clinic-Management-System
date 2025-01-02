using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Views.StaffView
{
    public sealed partial class BillListPage : Page
    {
        public BillListViewModel ViewModel { get; }

        public BillListPage()
        {
            this.InitializeComponent();
            ViewModel = new BillListViewModel();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.SelectedBill != null)
            {
                Frame.Navigate(typeof(BillDetailPage), ViewModel.SelectedBill.Id);
            }
        }
    }
} 