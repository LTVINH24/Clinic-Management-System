using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ClinicManagementSystem.ViewModel;
using ClinicManagementSystem.Model;

namespace ClinicManagementSystem.Views.StaffView
{
    /// <summary>
    /// BillListPage là trang hóa đơn
    /// </summary>
    public sealed partial class BillListPage : Page
    {
        public BillListViewModel ViewModel { get; }
        
        public BillListPage()
        {
            this.InitializeComponent();
            ViewModel = new BillListViewModel();
            this.DataContext = ViewModel;
        }

        /// <summary>
        /// Xử lí sự kiện khi chọn hóa đơn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is Bill selectedBill)
            {
                Frame.Navigate(typeof(BillDetailPage), selectedBill.Id);
                listView.SelectedItem = null;
            }
        }

        /// <summary>
        /// Xử lí sự kiện khi nhấn nút Previous
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GoToPreviousPage();
        }

        /// <summary>
        /// Xử lí sự kiện khi nhấn nút Next
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GoToNextPage();
        }

        /// <summary>
        /// Xử lí sự kiện khi nhấn nút Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Content is int page)
            {
                ViewModel.GoToPage(page);
            }
        }

        /// <summary>
        /// Xử lí sự kiện khi nhấn nút ClearFilter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            // Clear filter và tự động load lại data
            ViewModel.ClearFilter();

            // Reset Medicine Status ComboBox
            var medicineStatusComboBox = this.FindName("MedicineStatusComboBox") as ComboBox;
            if (medicineStatusComboBox != null)
            {
                medicineStatusComboBox.SelectedIndex = 0;  // Set về "All"
            }
        }

        /// <summary>
        /// Xử lí sự kiện khi nhấn nút AutoSuggestBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                ViewModel.Keyword = sender.Text;
            }
        }

        /// <summary>
        /// Xử lí sự kiện khi nhấn nút PagesComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>    
        private void PagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is PageInfo selectedPage)
            {
                ViewModel.SelectedPageInfo = selectedPage;
            }
        }

        /// <summary>
        /// Xử lí sự kiện khi nhấn nút StatusComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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