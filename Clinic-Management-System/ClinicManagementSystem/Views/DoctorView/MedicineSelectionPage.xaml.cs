using ClinicManagementSystem.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace ClinicManagementSystem.Views.DoctorView
{
    public sealed partial class MedicineSelectionPage : Page
    {
        public MedicineSelectionPage()
        {
            this.InitializeComponent();
            this.DataContext = new MedicineSelectionViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is int medicalExaminationFormId)
            {
                var viewModel = DataContext as MedicineSelectionViewModel;
                //viewModel?.LoadMedicines(medicalExaminationFormId);
            }
        }
    }
}
