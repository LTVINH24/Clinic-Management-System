using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Views.StaffView
{
    public sealed partial class UnbilledPrescriptionsDetailPage : Page
    {
        public UnbilledPrescriptionsDetailViewModel ViewModel { get; }

        public UnbilledPrescriptionsDetailPage()
        {
            this.InitializeComponent();
            ViewModel = new UnbilledPrescriptionsDetailViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is int prescriptionId)
            {
                ViewModel.LoadData(prescriptionId);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SaveBill())
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
} 