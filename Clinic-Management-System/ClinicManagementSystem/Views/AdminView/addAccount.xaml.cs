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
using ClinicManagementSystem.Helper;
using ClinicManagementSystem.ViewModel.EndUser;
using ClinicManagementSystem.Service;

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

        /// <summary>
        /// Xử lí sự kiện khi chọn click role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

		/// <summary>
		/// Xử lí sự kiện khi chọn click giới tính
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GenderMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem)
            {
                GenderDropDown.Content = menuItem.Text;
            }
        }

		/// <summary>
		/// Xử lí sự kiện khi click vào nút tạo tài khoản
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Create_Click(object sender, RoutedEventArgs e) 
        {
            if (ValidData())
            {
                string notify= viewModel.CreateUser();
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

		/// <summary>
		/// Kiểm tra dữ liệu nhập vào
		/// </summary>
		/// <returns>True nếu dữ liệu hợp lệ, False nếu dữ liệu không hợp lệ</returns>
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

		/// <summary>
		/// Hiển thị thông báo
		/// </summary>
		/// <param name="notify"></param>
		private async void Notify(string notify)
        {
			var currentTheme = ThemeService.Instance.GetCurrentTheme();
			ElementTheme dialogTheme;

			switch (currentTheme)
			{
				case "Light":
					dialogTheme = ElementTheme.Light;
					break;
				case "Dark":
					dialogTheme = ElementTheme.Dark;
					break;
				default:
					dialogTheme = ElementTheme.Default;
					break;
			}

			await new ContentDialog()
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Notify",
                Content = $"{notify}",
                CloseButtonText = "OK",
				RequestedTheme = dialogTheme
			}.ShowAsync();
        }

       

        private void NewSpecialtyClick(object sender, RoutedEventArgs e)
        {
            if(NewSpecialty.Text==null||NewSpecialty.Text=="")
            {
                Notify("Please enter a valid address");
                return;
            }
            bool success=viewModel.CreateNewSpecialty();
            if (success)
            {
                Notify("Specialty created successfully");
                viewModel.LoadSpecialties();
            }
            else
            {
                Notify("Specialty created failed");

            }
        }
    }
}
