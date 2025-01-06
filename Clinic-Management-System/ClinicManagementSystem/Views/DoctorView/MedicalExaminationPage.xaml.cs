using ClinicManagementSystem.Model;
using ClinicManagementSystem.ViewModel;
using ClinicManagementSystem.Views.DoctorView;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.ObjectModel;

namespace ClinicManagementSystem.Views.DoctorView
{
    /// <summary>
    /// MedicalExaminationPage là trang khám bệnh
    /// </summary>
	public sealed partial class MedicalExaminationPage : Page
    {
        public ObservableCollection<MedicalExaminationForm> ExaminationForms => ViewModel.ExaminationForms;
        public ObservableCollection<PageInfo> PageInfos => ViewModel.PageInfos;
        public PageInfo SelectedPageInfo
        {
            get => ViewModel.SelectedPageInfo;
            set => ViewModel.SelectedPageInfo = value;
        }

        private MedicalExaminationViewModel ViewModel { get; }

        public MedicalExaminationPage()
        {
            ViewModel = new MedicalExaminationViewModel();
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }
        
        /// <summary>
        /// Xử lí sự kiện khi trang được điều hướng đến
        /// </summary>
        /// <param name="e"></param>
        /// <exception cref="InvalidOperationException"></exception>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Frame navigationFrame)
            {
                ViewModel.NavigationFrame = navigationFrame;
            }
            else
            {
                throw new InvalidOperationException("Không thể lấy Frame từ tham số điều hướng.");
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
                ViewModel.NavigateToDiagnosisPage(selectedForm);
                listView.SelectedItem = null; // Reset selection
            }
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
                System.Diagnostics.Debug.WriteLine($"Text changed to: {sender.Text}"); // Debug
                ViewModel.Keyword = sender.Text;
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
