using ClinicManagementSystem.Helper;
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
    /// Medicine là trang quản lý thuốc
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
		/// Kiểm tra dữ liệu nhập vào
		/// </summary>
		/// <returns>True nếu dữ liệu hợp lệ, False nếu dữ liệu không hợp lệ</returns>
		private bool ValidData()
        {
            var valid = new IsValidData();
            if (!valid.IsValidEmpty(NewMedicineName.Text))
            {
                Notify("Please enter a valid name");
                return false;
            }
            if (NewMedicinePrice.Value<0)
            {
                Notify("Price >= 0");
                return false;
            }
            if (!valid.IsValidEmpty(NewMedicineManufacturer.Text))
            {
                Notify("Please enter a valid manufacturer");
                return false;
            }
            if (NewMedicineQuantityImport.Value<=0)
            {
                Notify("Quantity > 0");
                return false;
            }
            if (!valid.IsValidTwoDatePicker(NewMedicineMfgDate.Date, NewMedicineExpDate.Date))
            {
                Notify("MfgDate must be less than or equal ExpDate");
                return false;
            }
            return true;
        }
		/// <summary>
		/// Kiểm tra dữ liệu nhập vào khi edit
		/// </summary>
		/// <returns>True nếu dữ liệu hợp lệ, False nếu dữ liệu không hợp lệ</returns>
		private bool ValidDataMedicineEdit()
        {
            var valid = new IsValidData();
            if (!valid.IsValidEmpty(NameEditMedicine.Text))
            {
                Notify("Please enter a valid name");
                return false;
            }
            if (PriceEditMedicine.Value < 0)
            {
                Notify("Price >= 0");
                return false;
            }
            if (QuantityImportEditMedicine.Value < 0)
            {
                Notify("Quantity >= 0");
                return false;
            }
            if (!valid.IsValidEmpty(ManufacturerEditMedicine.Text))
            {
                Notify("Please enter a valid manufacturer");
                return false;
            }
            if(!valid.IsValidTwoDatePicker(MfgDateEditMedicine.Date, ExpDateEditMedicine.Date))
            {
                Notify("MfgDate must be less than or equal ExpDate");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Xử lí sự kiện khi nhấn nút add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addMedicine(object sender, RoutedEventArgs e)
        {
            if(ValidData())
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

		/// <summary>
		/// Xử lí sự kiện khi chọn radio button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if(RadioButton10Days.IsChecked == true)
            {
                ViewModel.DayRemainFilter = 10;
                ViewModel.LoadMedicines();
            }
            else if(RadioButton20Days.IsChecked == true)
            {
                ViewModel.DayRemainFilter = 20;
                ViewModel.LoadMedicines();


            }
            else if (RadioButton30Days.IsChecked == true)
            {
                ViewModel.DayRemainFilter = 30;

                ViewModel.LoadMedicines();

            }
            else
            {
                ViewModel.DayRemainFilter = 0;

                ViewModel.LoadMedicines();

            }
        }
		/// <summary>
		/// Xử lí sự kiện khi nhấn nút clear filter
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void clearFilter(object sender, RoutedEventArgs e)
        {
            RadioButton10Days.IsChecked = false;
            RadioButton20Days.IsChecked = false;
            RadioButton30Days.IsChecked = false;
            ViewModel.DayRemainFilter = 0;
            ViewModel.LoadMedicines();
        }
		/// <summary>
		/// Xử lí sự kiện khi nhấn nút update
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UpdateMedicine_Click(object sender, RoutedEventArgs e)
        {
            if(ValidDataMedicineEdit())
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

        }
		/// <summary>
		/// Xử lí sự kiện khi nhấn nút close popup
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            EditPopup.IsOpen = false;
        }
        private bool isDragging = false;
        private Windows.Foundation.Point initialPosition;
        private Windows.Foundation.Point popupPosition;

		/// <summary>
		/// Xử lí sự kiện khi kéo popup
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DragArea_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            isDragging = true;
            var properties = e.GetCurrentPoint(null).Properties;
            if (properties.IsLeftButtonPressed)
            {
                ((UIElement)sender).CapturePointer(e.Pointer);
                initialPosition = e.GetCurrentPoint(null).Position;
                popupPosition = new Windows.Foundation.Point(EditPopup.HorizontalOffset, EditPopup.VerticalOffset);
            }
        }
		/// <summary>
		/// Xử lí sự kiện khi kéo chuột
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DragArea_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (isDragging)
            {
                var currentPosition = e.GetCurrentPoint(null).Position;
                var deltaX = currentPosition.X - initialPosition.X;
                var deltaY = currentPosition.Y - initialPosition.Y;

                EditPopup.HorizontalOffset = popupPosition.X + deltaX;
                EditPopup.VerticalOffset = popupPosition.Y + deltaY;
            }
        }
		/// <summary>
		/// Xử lí sự kiện khi nhả chuột
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DragArea_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            isDragging = false;
            ((UIElement)sender).ReleasePointerCapture(e.Pointer);
        }

    }
}
