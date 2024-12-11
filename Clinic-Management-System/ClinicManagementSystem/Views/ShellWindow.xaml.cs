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
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ShellWindow : Window
	{
		public ShellWindow(string name)
		{
			this.InitializeComponent();

			var fullNamespace = $"{GetType().Namespace}.{name}";
			var type = Type.GetType(fullNamespace);
			content.Navigate(type);

			this.Title = "Clinic Management System";
		}

		/// <summary>
		/// Xử lí sự kiện khi cửa sổ được đóng
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void Window_Closed(object sender, WindowEventArgs args)
		{
			var screen = new MainWindow();
			screen.Activate();

			this.Close();
		}

	}
}
