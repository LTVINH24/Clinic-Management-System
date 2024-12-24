using ClinicManagementSystem.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ClinicManagementSystem.Views.AdminView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class addMedicine : Page
    {
        public MedicineViewModel ViewModel { get; set; }
        public addMedicine()
        {
            ViewModel = new MedicineViewModel();
            this.InitializeComponent();
            this.DataContext = ViewModel;

        }


        private void cancelMedicine(object sender, RoutedEventArgs e)
        {
            ViewModel.CancelMedicine();
        }

        private void addMedicines(object sender, RoutedEventArgs e)
        {
            bool success = ViewModel.AddMedicine();
            if (success)
            {
                Notify("Add medicine successfully");
            }
            else
            {
                Notify("Add medicine failed");
            }
        }
        private async void Notify(string notify)
        {
            await new ContentDialog()
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Notify",
                Content = $"{notify}",
                CloseButtonText = "OK"
            }.ShowAsync();
        }
    }
}
