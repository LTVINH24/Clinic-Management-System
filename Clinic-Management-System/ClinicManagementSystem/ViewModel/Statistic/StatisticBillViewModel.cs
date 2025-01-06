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
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.ViewModel.Statistic
{
    /// <summary>
    /// ViewModel cho StatisticBill
    /// </summary>
    public class StatisticBillViewModel : INotifyPropertyChanged
    {
        IDao _dao;
        public PlotModel ChartModel { get; private set; }
        public DateTimeOffset? startDate { get; set; } = null;
        public DateTimeOffset? endDate { get; set; } = null;
        private ObservableCollection<BillStatistic> _billstatistic;
        public ObservableCollection<BillStatistic> BillStatistic
        {
            get => _billstatistic ??= new ObservableCollection<BillStatistic>();
            set => _billstatistic = value;
        }
        public StatisticBillViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            startDate = DateTimeOffset.Now.Date.AddDays(-1);
            endDate = DateTimeOffset.Now.Date;
            LoadData();
            UpdateChart();
        }
		/// <summary>
		/// Cập nhật giao diện biểu đồ
		/// </summary>
		private void UpdateChartTheme()
        {
			if (!startDate.HasValue || !endDate.HasValue)
			{
				return;
			}

			var currentTheme = ThemeService.Instance.GetCurrentTheme();
            var backgroundColor = currentTheme == "Dark" ? OxyColor.FromRgb(32, 32, 32) :OxyColor.FromRgb(255, 255, 255);
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
		/// Lấy dữ liệu từ database
		/// </summary>
		public void LoadData()
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                return;
            }

            var items = _dao.GetBillStatistic(startDate.Value, endDate.Value);
            BillStatistic.Clear();
            foreach (var item in items)
            {
                BillStatistic.Add(item);
            }
        }
		/// <summary>
		/// Cập nhật biểu đồ
		/// </summary>
		public void UpdateChart()
        {
			if (!startDate.HasValue || !endDate.HasValue)
			{
				return;
			}

			var model = new PlotModel { Title = "Statistic Bill" };
            var dateAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Date",
                Minimum = DateTimeAxis.ToDouble(startDate.Value.Date),
                Maximum = DateTimeAxis.ToDouble(endDate.Value.Date),
                StringFormat = "dd/MM/yyyy",
                IntervalType = DateTimeIntervalType.Days,
                MajorStep = 1
            };
            model.Axes.Add(dateAxis);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Money" });
            var lineSeries = new LineSeries { Title = "Examinations", StrokeThickness = 2, Color = OxyColors.Red };
            var currentDateTime = startDate.Value.DateTime;
            var endDateTime = endDate.Value.DateTime;
            while (currentDateTime <= endDateTime)
            {
                var statistic = BillStatistic.FirstOrDefault(s => s.CreateDate.Date == currentDateTime.Date);
                double xValue = DateTimeAxis.ToDouble(currentDateTime.Date);
                if (statistic != null)
                {
                    lineSeries.Points.Add(new DataPoint(xValue, statistic.TotalAmount));
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
