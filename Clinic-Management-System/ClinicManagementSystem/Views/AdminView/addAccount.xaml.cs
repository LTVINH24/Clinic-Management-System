using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ClinicManagementSystem.ViewModel;
using ClinicManagementSystem.Helper;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ClinicManagementSystem.Views.AdminView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class addAccount : Page
    {
        public addAccount()
        {
            this.InitializeComponent();
        }
        public  UserViewModel viewModel { get; private set; }   =new UserViewModel();

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RoleMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var valid = new IsValidData();
            if(sender is MenuFlyoutItem menuItem)
            {
                if(menuItem.Text=="doctor")
                {
                    Specialty.Visibility = Visibility.Visible;
                    Room.Visibility = Visibility.Visible;
                    Grid.SetRowSpan(RoleAndSpecialty, 2);
                }
                else
                {
                    Specialty.Visibility = Visibility.Collapsed;
                    Room.Visibility = Visibility.Collapsed;
                    Grid.SetRowSpan(RoleAndSpecialty, 1);
                }
                RoleDropDown.Content = menuItem.Text;
                RoleDropDown.Tag =menuItem.Text;
            }
           
        }
        private void GenderMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem)
            {
                GenderDropDown.Content = menuItem.Text;
            }
        }
        private void Create_Click(object sender, RoutedEventArgs e) 
        {
            if (ValidData())
            {
                string notify= viewModel.CreateUser(viewModel.user);
                if(notify == "")
                {
                    Notify("Account created successfully");

                }
                else
                {
                    Notify(notify);
                }    
            }
        }
        private bool  ValidData()
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
            if (!valid.IsValidUsername(UserNameUser.Text))
            {
                Notify("A username must be at least 3 characters long and contain only letters, numbers, or underscores");
                return false;
            }

            //if (!valid.IsValidGender(GenderDropDown.Tag.ToString()))
            //{
            //    Notify("Please choose a gender");
            //    return false;
            //}
            //if (!valid.IsValidDescription(RoleDropDown.Tag.ToString()))
            //{
            //    Notify("Please choose a gender");
            //    return false;
            //}
            if (!valid.IsValidDatePicker(BirthDayUser.Date))
            {
                Notify("Please choose a valid birthday");
                return false;
            }
            if (!valid.IsValidPassword(PasswordUser.Password))
            {
                Notify("Password: 8+ chars, uppercase, lowercase, number, special char");
                return false ;
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
    }
}
