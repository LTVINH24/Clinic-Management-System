using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ClinicManagementSystem.ViewModel;
using ClinicManagementSystem.Model;

namespace ClinicManagementSystem.Views.DoctorView
{
    public sealed partial class ExaminedFormsPage : Page
    {
        private ExaminedFormsViewModel ViewModel { get; }

        public ExaminedFormsPage()
        {
            ViewModel = new ExaminedFormsViewModel();
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                ViewModel.Keyword = sender.Text;
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is MedicalExaminationForm selectedForm)
            {
                // Điều hướng đến trang chi tiết
                Frame.Navigate(typeof(ExaminedFormDetailPage), selectedForm);
                listView.SelectedItem = null; // Reset selection
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