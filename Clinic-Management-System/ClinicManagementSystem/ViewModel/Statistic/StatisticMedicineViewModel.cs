using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Views.AdminView;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;




namespace ClinicManagementSystem.ViewModel.Statistic
{
    public class StatisticMedicineViewModel : INotifyPropertyChanged
    {
        IDao _dao;
        public PlotModel ChartModel { get; private set; }
        public PlotModel ChartModelMoney { get; private set; }
        public DateTimeOffset startDate { get; set; }
        public DateTimeOffset endDate { get; set; }
        private ObservableCollection<MedicineStatistic> _medicinestatistic;
        public ObservableCollection<MedicineStatistic> MedicinesStatistic
        {
            get => _medicinestatistic ??= new ObservableCollection<MedicineStatistic>();
            set => _medicinestatistic = value;
        }
        private ObservableCollection<MedicineStatistic> _medicinestatisticmoney;
        public ObservableCollection<MedicineStatistic> MedicinesStatisticMoney
        {
            get => _medicinestatisticmoney ??= new ObservableCollection<MedicineStatistic>();
            set => _medicinestatisticmoney = value;
        }
        public StatisticMedicineViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            startDate = DateTimeOffset.Now;
            endDate = DateTimeOffset.Now;
            LoadData();
            UpdateChart();
        }
        public void LoadData()
        { 
            var items = _dao.GetMedicineStatistic(startDate, endDate, 10, "QuantitySold");
            MedicinesStatistic.Clear();
            foreach (var item in items)
            {
                MedicinesStatistic.Add(item);
            }

            var itemsMoney = _dao.GetMedicineStatistic(startDate, endDate, 10, "MoneySold");
            MedicinesStatisticMoney.Clear();
            foreach (var item in itemsMoney)
            {
                MedicinesStatisticMoney.Add(item);
            }

        }
        public void UpdateChart()
        {
            var modelMoney = new PlotModel { Title = "Highest drug revenue" };
            var categoryMoneyAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                Title = "Medicine name"
            };
            var linearMoneyAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Money",
                Minimum = 0,
            };
            var model = new PlotModel { Title = "Most sold medicine" };
            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                Title = "Medicine name"
            };
            var linearAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Amount",
                Minimum = 0,
            };
            var series = new BarSeries { Title = "Amount" ,FillColor=OxyColor.FromRgb(219,24,219),FontSize=24, };
            var seriesMoney = new BarSeries { Title="USD",FillColor=OxyColor.FromRgb(34,218,219),FontSize=24};
            foreach (var medicine in MedicinesStatistic)
            {
                categoryAxis.Labels.Add(medicine.MedicineName);
                series.Items.Add(new BarItem { Value = medicine.QuantitySold });
            }
            foreach(var item in MedicinesStatisticMoney)
            {
                categoryMoneyAxis.Labels.Add(item.MedicineName);
                seriesMoney.Items.Add(new BarItem { Value = item.Money });


            }
            model.Axes.Add(categoryAxis);
            model.Axes.Add(linearAxis);
            model.Series.Add(series);
            modelMoney.Axes.Add(categoryMoneyAxis);
            modelMoney.Axes.Add(linearMoneyAxis);
            modelMoney.Series.Add(seriesMoney);
            ChartModel = model;
            ChartModelMoney=modelMoney;
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
