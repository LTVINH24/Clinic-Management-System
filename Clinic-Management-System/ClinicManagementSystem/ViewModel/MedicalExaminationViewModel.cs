using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ClinicManagementSystem.Command;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Views.DoctorView;
using Microsoft.UI.Xaml.Controls;

namespace ClinicManagementSystem.ViewModel
{
    public class MedicalExaminationViewModel : BaseViewModel
    {
        private readonly SqlServerDao _dataAccess;
        private Frame _navigationFrame;
        private ObservableCollection<MedicalExaminationForm> _examinationForms;

        // Số phần tử mỗi trang
        private const int PageSize = 2;
        private int _currentPage = 0; // Trang hiện tại

        public ObservableCollection<MedicalExaminationForm> ExaminationForms
        {
            get => _examinationForms;
            set => SetProperty(ref _examinationForms, value);
        }

        public Frame NavigationFrame
        {
            get => _navigationFrame;
            set => _navigationFrame = value;
        }

        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }

        public MedicalExaminationViewModel()
        {
            _dataAccess = new SqlServerDao();
            ExaminationForms = new ObservableCollection<MedicalExaminationForm>();
            PreviousPageCommand = new RelayCommand(PreviousPage);
            NextPageCommand = new RelayCommand(NextPage);
            LoadExaminationForms(); // Gọi phương thức để tải dữ liệu
        }

        private void LoadExaminationForms()
        {
            var forms = _dataAccess.GetMedicalExaminationForms();
            ExaminationForms.Clear();

            // Chỉ lấy các phần tử cho trang hiện tại
            var pagedForms = forms.Skip(_currentPage * PageSize).Take(PageSize);
            foreach (var form in pagedForms)
            {
                ExaminationForms.Add(form);
            }
        }

        private void PreviousPage()
        {
            if (_currentPage > 0)
            {
                _currentPage--;
                LoadExaminationForms(); // Tải lại các phiếu khám cho trang trước
            }
        }

        private void NextPage()
        {
            if (ExaminationForms.Count == PageSize)
            {
                _currentPage++;
                LoadExaminationForms(); // Tải lại các phiếu khám cho trang tiếp theo
            }
        }

        public void NavigateToDiagnosisPage(MedicalExaminationForm selectedForm)
        {
            // Điều hướng từ MedicalExaminationPage sang DiagnosisPage
            NavigationFrame?.Navigate(typeof(DiagnosisPage), selectedForm);
        }
    }
}
