using System;
using System.Linq;
using System.Collections.ObjectModel;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service.DataAccess;

namespace ClinicManagementSystem.ViewModel
{
    public class ExaminedFormsViewModel : BaseViewModel
    {
        private readonly SqlServerDao _dataAccess;
        private ObservableCollection<MedicalExaminationForm> _examinedForms;
        private MedicalExaminationForm _selectedForm;

        public ExaminedFormsViewModel()
        {
            _dataAccess = new SqlServerDao();
            LoadExaminedForms();
        }

        public ObservableCollection<MedicalExaminationForm> ExaminedForms
        {
            get => _examinedForms;
            set => SetProperty(ref _examinedForms, value);
        }

        public MedicalExaminationForm SelectedForm
        {
            get => _selectedForm;
            set => SetProperty(ref _selectedForm, value);
        }

        private void LoadExaminedForms()
        {
            // Lấy danh sách phiếu khám đã khám
            var forms = _dataAccess.GetMedicalExaminationForms()
                .Where(f => f.IsExaminated == "true")
                .ToList();

            ExaminedForms = new ObservableCollection<MedicalExaminationForm>(forms);
        }
    }
}