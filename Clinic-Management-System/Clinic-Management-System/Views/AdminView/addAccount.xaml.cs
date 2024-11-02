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
using Clinic_Management_System.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clinic_Management_System.Views.AdminView
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
            if(sender is MenuFlyoutItem menuItem)
            {
                RoleDropDown.Content = menuItem.Text;
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
