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
            this.Title = "Clinic Management System";

		}
		/// <summary>
		/// Xử lí sự kiện khi cửa sổ được bật
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            viewModel.LoadPassword(usernameTextbox, passwordBox);
        }
        public MainViewModel viewModel { get; set; } = new MainViewModel();

		/// <summary>
		/// Xử lí sự kiện khi nhấn nút đăng nhập
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Login_Click(object sender, RoutedEventArgs e)
        {
            bool isRemember = rememberPassword.IsChecked ?? false;
            viewModel.Authentication(viewModel.UserLogin, isRemember);

            // if (rememberPassword.IsChecked == true)
            // {
            //     viewModel.Authentication(viewModel.UserLogin, true);
            // }
            // viewModel.Authentication(viewModel.UserLogin, false);
        }

		/// <summary>
		/// Xử lí sự kiện khi đăng nhập thành công
		/// </summary>
		/// <param name="isSuccess"></param>
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

		/// <summary>
		/// Xử lí sự kiện khi đăng nhập thất bại
		/// </summary>
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
