using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service;
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
using Windows.Media.Effects;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ClinicManagementSystem.Views.AdminView
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Medicine : Page
    {
        public MedicineViewModel ViewModel { get; set; }
        public Medicine()
        {
            ViewModel = new MedicineViewModel();
            this.DataContext = ViewModel;
            this.InitializeComponent();
        }
        bool init = false;

		/// <summary>
		/// Xử lí sự kiện khi chọn trang
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
		/// Xử lí sự kiện khi nhấn nút trang trước
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GoToPreviousPage();
        }


		/// <summary>
		/// Xử lí sự kiện khi nhấn nút trang sau
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GoToNextPage();
        }

        public ClinicManagementSystem.Model.Medicine editMedicine { get; set; }

        /// <summary>
        /// Xử lí sự kiện khi nhấn nút edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            editMedicine = button?.DataContext as ClinicManagementSystem.Model.Medicine;
            ViewModel.EditMedicine(editMedicine);
            EditPopup.IsOpen = true;
        }

        /// <summary>
        /// Xử lí sự kiện khi nhấn nút delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var deleteMedicine = button?.DataContext as ClinicManagementSystem.Model.Medicine;
            bool success = ViewModel.DeleteMedicine(deleteMedicine);
            if (success)
            {
                Notify("Delete medicine successfully");
            }
            else
            {
                Notify("Delete medicine failed");
            }

        }

        /// <summary>
        /// Xử lí sự kiện khi nhấn nút add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addMedicine(object sender, RoutedEventArgs e)
        {
            bool success = ViewModel.AddMedicine();
            if (success)
            {
                Notify("Add medicine successfully");
            }
            else
            {
                Notify("Add medicine failed");
            }

        }

        /// <summary>
        /// Xử lí sự kiện khi nhấn nút cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelMedicine(object sender, RoutedEventArgs e)
        {
            ViewModel.CancelMedicine();
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
		/// Xử lí sự kiện khi nhập vào ô search
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void searchTextbox_Click(object sender, TextChangedEventArgs e)
        {
            ViewModel.Search();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if(RadioButton10Days.IsChecked == true)
            {
                ViewModel.LoadMedicines(10);
            }
            else if(RadioButton20Days.IsChecked == true)
            {
                ViewModel.LoadMedicines(20);


            }
            else if (RadioButton30Days.IsChecked == true)
            {
                ViewModel.LoadMedicines(30);

            }
            else
            {
                ViewModel.LoadMedicines(0);

            }
        }

        private void clearFilter(object sender, RoutedEventArgs e)
        {
            RadioButton10Days.IsChecked = false;
            RadioButton20Days.IsChecked = false;
            RadioButton30Days.IsChecked = false;
            ViewModel.LoadMedicines(0);


        }

        private void UpdateMedicine_Click(object sender, RoutedEventArgs e)
        {
            bool succes = ViewModel.UpdateMedicine();
            if (succes)
            {
                Notify("Update medicine successfully");
            }
            else
            {
                Notify("Update medicine failed");
            }
            EditPopup.IsOpen = false;

        }

        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            EditPopup.IsOpen = false;
        }
    }
}
