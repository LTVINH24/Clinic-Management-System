using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ClinicManagementSystem.ViewModel;
using ClinicManagementSystem.Model;

namespace ClinicManagementSystem.Views.DoctorView
{
    /// <summary>
    /// ExaminedFormsPage là trang phiếu khám bệnh
    /// </summary>
    public sealed partial class ExaminedFormsPage : Page
    {
        private ExaminedFormsViewModel ViewModel { get; }

        public ExaminedFormsPage()
        {
            ViewModel = new ExaminedFormsViewModel();
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }

        /// <summary>
        /// Xử lí sự kiện khi nhập vào AutoSuggestBox
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
        /// Xử lí sự kiện khi chọn một phiếu khám bệnh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is MedicalExaminationForm selectedForm)
            {
                // Điều hướng đến trang chi tiết
                Frame.Navigate(typeof(ExaminedFormDetailPage), selectedForm);
                listView.SelectedItem = null; // Reset selection
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
        /// Xử lí sự kiện khi chọn một trang
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is PageInfo pageInfo)
            {
                ViewModel.GoToPage(pageInfo.Page);
            }
        }

        /// <summary>
        /// Xử lí sự kiện khi nhấn nút ClearFilter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ClearFilter();
        }
    }
} 