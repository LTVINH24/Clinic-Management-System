using Clinic_Management_System.ViewModel;
using Clinic_Management_System.Views;
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
using Windows.Networking.Vpn;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clinic_Management_System
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
		public MainViewModel viewModel { get;set; }=new MainViewModel();
        public void Login_Click(object sender, RoutedEventArgs e)
		{
			viewModel.Authentication(viewModel.UserLogin);
        }
		private void OnLoginCompleted(string isSuccess)
		{
			Console.Write(isSuccess);
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
				XamlRoot =this.Content.XamlRoot,
				Title= "Login failed",
				Content = "Incorrect username or password",
				CloseButtonText="OK"
            }.ShowAsync();
		}
    }
}
