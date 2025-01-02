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
            System.Diagnostics.Debug.WriteLine("SaveButton clicked");

            if (ViewModel.SaveBill())
            {
                System.Diagnostics.Debug.WriteLine("Bill saved successfully");
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
            else
            {
                var dialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Error",
                    Content = "Failed to save bill. Please try again.",
                    CloseButtonText = "OK"
                };
                dialog.ShowAsync();
                System.Diagnostics.Debug.WriteLine("Failed to save bill");
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