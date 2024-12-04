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

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GoToNextPage();
        }
        private void previousButton_Click(object sender, RoutedEventArgs e)
         {
            ViewModel.GoToPreviousPage();
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

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Search();
        }

        private void searchTextbox_Click(object sender, TextChangedEventArgs e)
        {
            ViewModel.Search();
        }

        private void userList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var useredit = itemsComboBox.SelectedItem as User;
            ViewModel.Edit(useredit);
        }

        private void update_editUser(object sender, RoutedEventArgs e)
        {
            var success=ViewModel.Update();
            ViewModel.LoadData();
            string notify="";
            if (success)
            {
                notify = "Updated successfully";
            }
            else
            {
                notify = "Update failed";
            }
            Notify(notify);
        }

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

        private void cancel_editUser(object sender, RoutedEventArgs e)
        {
            ViewModel.Cancel();
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
