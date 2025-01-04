using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ClinicManagementSystem.ViewModel;
using ClinicManagementSystem.Model;

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
            if (sender is ListView listView && listView.SelectedItem is Bill selectedBill)
            {
                Frame.Navigate(typeof(BillDetailPage), selectedBill.Id);
                listView.SelectedItem = null;
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GoToPreviousPage();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GoToNextPage();
        }

        private void PageButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Content is int page)
            {
                ViewModel.GoToPage(page);
            }
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ClearFilter();
        }
    }
} 