using ClinicManagementSystem.Model;
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
using Windows.Media.Effects;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ClinicManagementSystem.Views.AdminView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Medicine : Page
    {
        public MedicineViewModel ViewModel { get; set; }
        public Medicine()
        {
            ViewModel = new MedicineViewModel();
            this.DataContext = ViewModel;
            this.InitializeComponent();
        }
        bool init = false;

        private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (init == false)
            {
                init = true;
                return;
            }
            if (pagesComboBox.SelectedIndex >= 0)
            {
                var item = pagesComboBox.SelectedItem as PageInfo;
                ViewModel.GoToPage(item.Page);
            }
        }
        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GoToPreviousPage();
        }

        private void medicineList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GoToNextPage();
        }

        public ClinicManagementSystem.Model.Medicine editMedicine { get; set; }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            SaveBtn.Visibility = Visibility.Visible;
            AddBtn.Visibility = Visibility.Collapsed;
            var button =sender as Button;
            editMedicine = button?.DataContext as ClinicManagementSystem.Model.Medicine;
            ViewModel.EditMedicine(editMedicine);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var deleteMedicine = button?.DataContext as ClinicManagementSystem.Model.Medicine;
            bool success = ViewModel.DeleteMedicine(deleteMedicine);
            if (success)
            {
                Notify("Delete medicine successfully");
            }
            else
            {
                Notify("Delete medicine failed");
            }

        }

        private void addMedicine(object sender, RoutedEventArgs e)
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

        private void saveMedicine(object sender, RoutedEventArgs e)
        {
            bool succes = ViewModel.UpdateMedicine();
            if (succes)
            {
                Notify("Update medicine successfully");
            }
            else
            {
                Notify("Update medicine failed");
            }
        }

        private void cancelMedicine(object sender, RoutedEventArgs e)
        {
            SaveBtn.Visibility = Visibility.Collapsed;
            AddBtn.Visibility = Visibility.Visible;
            ViewModel.CancelMedicine();
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

        private void searchTextbox_Click(object sender, TextChangedEventArgs e)
        {
            ViewModel.Search();
        }
    }
}
