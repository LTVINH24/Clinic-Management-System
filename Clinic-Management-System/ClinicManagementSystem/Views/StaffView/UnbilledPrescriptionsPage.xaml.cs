using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ClinicManagementSystem.ViewModel;
using ClinicManagementSystem.Model;

namespace ClinicManagementSystem.Views.StaffView
{
    public sealed partial class UnbilledPrescriptionsPage : Page
    {
        private UnbilledPrescriptionsViewModel ViewModel { get; }

        public UnbilledPrescriptionsPage()
        {
            ViewModel = new UnbilledPrescriptionsViewModel();
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
            if (sender is ListView listView && listView.SelectedItem is Prescription selectedPrescription)
            {
                Frame.Navigate(typeof(UnbilledPrescriptionsDetailPage), selectedPrescription.Id);
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
    }
} 