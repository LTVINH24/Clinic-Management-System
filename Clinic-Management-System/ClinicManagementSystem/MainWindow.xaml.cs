using ClinicManagementSystem.ViewModel;
using ClinicManagementSystem.Views;
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
using System.Security.Cryptography;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Vpn;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ClinicManagementSystem
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            viewModel.LoginCompleted += OnLoginCompleted;

        }
        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            viewModel.LoadPassword(usernameTextbox, passwordBox);
        }
        public MainViewModel viewModel { get; set; } = new MainViewModel();
        public void Login_Click(object sender, RoutedEventArgs e)
        {
            if (rememberPassword.IsChecked == true)
            {
                viewModel.SavePassWord(viewModel.UserLogin);
            }
            else
            {
                viewModel.Authentication(viewModel.UserLogin);
            }
        }
        private void OnLoginCompleted(string isSuccess)
        {
            if (isSuccess != "")
            {
                string namePage = $"{isSuccess}Page";
                namePage = namePage.Replace(" ", "");


                var screen = new ShellWindow(namePage);
                screen.Activate();



                viewModel.LoginCompleted -= OnLoginCompleted;
                this.Close();
            }
            else
            {
                LoginFailed();

            }
        }
        private async void LoginFailed()
        {
            await new ContentDialog()
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Login failed",
                Content = "Incorrect username or password",
                CloseButtonText = "OK"
            }.ShowAsync();
        }
    }
}
