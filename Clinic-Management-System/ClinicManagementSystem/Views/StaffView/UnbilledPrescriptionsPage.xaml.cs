using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Views.StaffView
{
    public sealed partial class UnbilledPrescriptionsPage : Page
    {
        public UnbilledPrescriptionsViewModel ViewModel { get; }

        public UnbilledPrescriptionsPage()
        {
            this.InitializeComponent();
            ViewModel = new UnbilledPrescriptionsViewModel();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.SelectedPrescription != null)
            {
                Frame.Navigate(typeof(UnbilledPrescriptionsDetailPage), ViewModel.SelectedPrescription.Id);
            }
        }
    }
} 