using ClinicManagementSystem.Model;
using ClinicManagementSystem.Model.Statistic;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.Service.DataAccess;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.ViewModel.Statistic
{
    public class StatisticMedicalExaminationViewModel : INotifyPropertyChanged
    {
        IDao _dao;
        public PlotModel ChartModel { get; private set; }
        private ObservableCollection<MedicalExaminationStatistic> _medicalExaminationstatistic;
        public DateTimeOffset startDate { get; set; }
        public DateTimeOffset endDate { get; set; }
        public ObservableCollection<MedicalExaminationStatistic> MedicalExaminationstatistic
        {
            get => _medicalExaminationstatistic ??= new ObservableCollection<MedicalExaminationStatistic>();
            set => _medicalExaminationstatistic = value;
        }
        public StatisticMedicalExaminationViewModel()
        {
            _dao= ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            startDate = DateTimeOffset.Now;
            endDate = DateTimeOffset.Now;
            LoadData();
            UpdateChart();
        }
		/// <summary>
		/// Load dữ liệu thống kê khám bệnh
		/// </summary>
		public void LoadData()
        {
            var items = _dao.GetMedicalExaminationStatisticsByDate(startDate, endDate);
            MedicalExaminationstatistic.Clear();
            foreach (var item in items)
            {
                MedicalExaminationstatistic.Add(item);
            }
        }
		/// <summary>
		/// Cập nhật giao diện biểu đồ
		/// </summary>
		private void UpdateChartTheme()
        {
            var currentTheme = ThemeService.Instance.GetCurrentTheme();
            var backgroundColor = currentTheme == "Dark" ? OxyColor.FromRgb(32, 32, 32) : OxyColor.FromRgb(255, 255, 255);
            var foregroundColor = currentTheme == "Dark" ? OxyColor.FromRgb(255, 255, 255) : OxyColor.FromRgb(0, 0, 0);


            ChartModel.Background = backgroundColor;
            ChartModel.TextColor = foregroundColor;
            ChartModel.PlotAreaBorderColor = foregroundColor;

            foreach (var axis in ChartModel.Axes)
            {
                axis.TextColor = foregroundColor;
                axis.TicklineColor = foregroundColor;
                axis.TitleColor = foregroundColor;
            }
            ChartModel.InvalidatePlot(true);
        }
		/// <summary>
		/// Cập nhật biểu đồ
		/// </summary>
		public void UpdateChart()
        {
            var model = new PlotModel { Title = "Statistic Patient Visits" };

            var dateAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Date",
                Minimum = DateTimeAxis.ToDouble(startDate.DateTime),
                Maximum = DateTimeAxis.ToDouble(endDate.DateTime),
                StringFormat = "dd/MM/yyyy",
                IntervalType = DateTimeIntervalType.Days,
                MajorStep = 1,
            };
            //Truc x
            model.Axes.Add(dateAxis);
            //Truc y
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Number of Examinations" });



            var lineSeries = new LineSeries { Title = "Examinations", StrokeThickness = 2, Color = OxyColors.Red };
            var currentDateTime = startDate.DateTime;
            var endDateTime  = endDate.DateTime;
            while (currentDateTime <= endDateTime)
            {
                var statistic = MedicalExaminationstatistic.FirstOrDefault(s => s.date.Date == currentDateTime.Date);
                double xValue = DateTimeAxis.ToDouble(currentDateTime.Date);
                if (statistic!=null)
                {
                    lineSeries.Points.Add(new DataPoint(xValue, statistic.amount));
                }
                else
                {
                    lineSeries.Points.Add(new DataPoint(xValue, 0));
                }
                currentDateTime = currentDateTime.AddDays(1);
            }
            model.Series.Add(lineSeries);
            ChartModel = model;

            UpdateChartTheme();
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
