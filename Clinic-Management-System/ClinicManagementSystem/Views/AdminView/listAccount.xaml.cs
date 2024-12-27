using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.ViewModel.EndUser;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
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
	public sealed partial class listAccount : Page
    {
        public AccountViewModel ViewModel { get; set; }
        public listAccount()
        {
            ViewModel =new AccountViewModel();
            this.DataContext = ViewModel;
            this.InitializeComponent();
        }

		/// <summary>
		/// Xử lí sự kiện khi chọn button next
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GoToNextPage();
        }
		/// <summary>
		/// Xử lí sự kiện khi chọn button previous
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void previousButton_Click(object sender, RoutedEventArgs e)
         {
            ViewModel.GoToPreviousPage();
         }
        bool init = false;

		/// <summary>
		/// Xử lí sự kiện khi chọn page
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// Xử lí sự kiện khi chọn button search
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Search();
        }

		/// <summary>
		/// Xử lí sự kiện khi nhập text vào search box
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void searchTextbox_Click(object sender, TextChangedEventArgs e)
        {
            ViewModel.Search();
        }

		/// <summary>
		/// Xử lí sự kiện khi chọn item trong list
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void userList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var useredit = itemsComboBox.SelectedItem as User;
            ViewModel.Edit(useredit);
        }

		/// <summary>
		/// Xử lí sự kiện khi chọn button update
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		

		/// <summary>
		/// Xử lí sự kiện khi chọn button delete
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void delete_editUser(object sender, RoutedEventArgs e)
        {
            var success = ViewModel.Delete();
            ViewModel.LoadData();
            string notify = "";
            if (success)
            {
                notify = "Deleted successfully";
            }
            else
            {
                notify = "Deleted failed";
            }
            Notify(notify);
        }

		/// <summary>
		/// Xử lí sự kiện khi chọn button cancel
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cancel_editUser(object sender, RoutedEventArgs e)
        {
            ViewModel.Cancel();
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

		/// <summary>
		/// Xử lí sự kiện khi chọn giới tính
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void setGender(object sender, RoutedEventArgs e)
		{
            if (sender is MenuFlyoutItem menuItem)
            {
                ViewModel.UserEdit.gender = menuItem.Text;
            }
        }

		/// <summary>
		/// Xử lí sự kiện khi chọn role
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void setRole(object sender, RoutedEventArgs e)
        {
            if(sender is MenuFlyoutItem menuItem)
            {
                ViewModel.UserEdit.role = menuItem.Text;
            }
        }

        private void Lock(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var user = button?.DataContext as User;
            var fontIcon = button?.DataContext as FontIcon;
            if (user.status == "locked")
            {
               
                ViewModel.LockUser(user.id, "active");
                ViewModel.LoadData();
            }
            else if(user.status =="active")
            {
                ViewModel.LockUser(user.id, "locked");
                ViewModel.LoadData();
            }
        }
    }
}
