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
            this.DataContext = ViewModel;
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

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                ViewModel.Keyword = sender.Text;
            }
        }

        private void PagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is PageInfo selectedPage)
            {
                ViewModel.SelectedPageInfo = selectedPage;
            }
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && 
                comboBox.SelectedItem is ComboBoxItem selectedItem && 
                ViewModel != null)
            {
                string status = selectedItem.Tag?.ToString() ?? "";
                ViewModel.SelectedStatus = status;
            }
        }
    }
} 