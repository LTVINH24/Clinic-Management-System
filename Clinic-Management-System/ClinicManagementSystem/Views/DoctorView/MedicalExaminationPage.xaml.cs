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
		/// Xử lý sự kiện khi trang được điều hướng đến
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
		/// Xử lý sự kiện khi chọn một phiếu khám bệnh
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

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                System.Diagnostics.Debug.WriteLine($"Text changed to: {sender.Text}"); // Debug
                ViewModel.Keyword = sender.Text;
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

        private void PagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is PageInfo pageInfo)
            {
                ViewModel.GoToPage(pageInfo.Page);
            }
        }
    }
}
