using ClinicManagementSystem.Helper;
using ClinicManagementSystem.ViewModel.EndUser;
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

namespace ClinicManagementSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
       
        public InformationViewModel viewModel { get; private set; }
        public HomePage()
        {
            this.InitializeComponent();
            viewModel = new InformationViewModel();
            viewModel.LoadInformationUser();
        }



        private void GenderMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem)
            {
                GenderDropDown.Content = menuItem.Text;
            }
        }

        private bool ValidData()
        {
            var valid = new IsValidData();
            if (!valid.IsValidName(NameUser.Text))
            {
                Notify("Please enter a valid name");
                return false;
            }
            if (!valid.IsValidPhone(PhoneUser.Text))
            {
                Notify("Please enter a valid phone number");
                return false;
            }
            if (!valid.IsValidAddress(AddressUser.Text))
            {
                Notify("Please enter a valid address");
                return false;
            }
            if (!valid.IsValidDatePicker(BirthDayUser.Date))
            {
                Notify("Please choose a valid birthday");
                return false;
            }
            return true;
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (ValidData())
            {
                bool success = viewModel.UpdateInformationUser();
                if (success)
                {
                    Notify("Updated successfully");
                }
                else
                {
                    Notify("Updated failed");
                }
            }
        }
    }
}

