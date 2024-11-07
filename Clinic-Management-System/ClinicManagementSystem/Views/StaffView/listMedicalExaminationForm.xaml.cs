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

namespace ClinicManagementSystem.Views.StaffView
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class listMedicalExaminationForm : Page
	{
		public MedicalExaminationFormViewModel ViewModel { get; set; }
		public listMedicalExaminationForm()
		{
			ViewModel = new MedicalExaminationFormViewModel();
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

		private void medicalExaminationFormList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var formEdit = itemsComboBox.SelectedItem as MedicalExaminationForm;
			ViewModel.Update(formEdit);
		}

	}	
}
