using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Views.AdminView;
using OfficeOpenXml;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Windows.Forms;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage.Pickers;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Windows.Storage;
using ClinicManagementSystem.Views;

namespace ClinicManagementSystem.ViewModel.Statistic
{
    /// <summary>
    /// ViewModel cho StatisticMedicine
    /// </summary>
    public class StatisticMedicineViewModel : INotifyPropertyChanged
    {
        IDao _dao;
        public PlotModel ChartModel { get; private set; }
        public PlotModel ChartModelMoney { get; private set; }
        public DateTimeOffset? startDate { get; set; } = null;
        public DateTimeOffset? endDate { get; set; } = null;
        private ObservableCollection<MedicineStatistic> _medicinestatisticcommon;
        public ObservableCollection<MedicineStatistic> MedicinesStatisticCommom
        {
            get => _medicinestatisticcommon ??= new ObservableCollection<MedicineStatistic>();
            set => _medicinestatisticcommon = value;
        }
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
            startDate = DateTimeOffset.Now.Date;
            endDate = DateTimeOffset.Now.Date;
            LoadData();
            UpdateChart();
        }
		/// <summary>
		/// Load data từ database
		/// </summary>
		public void LoadData()
        {
			if (!startDate.HasValue || !endDate.HasValue)
			{
				return;
			}

			var items = _dao.GetTopMedicineStatistic(startDate.Value, endDate.Value, 10, "QuantitySold");
            MedicinesStatistic.Clear();
            foreach (var item in items)
            {
                MedicinesStatistic.Add(item);
            }

            var itemsMoney = _dao.GetTopMedicineStatistic(startDate.Value, endDate.Value, 10, "MoneySold");
            MedicinesStatisticMoney.Clear();
            foreach (var item in itemsMoney)
            {
                MedicinesStatisticMoney.Add(item);
            }

        }
		/// <summary>
		/// Cập nhật giao diện của biểu đồ
		/// </summary>
		/// <param name="ChartModel"></param>
		private void UpdateChartTheme(PlotModel ChartModel)
        {
			if (!startDate.HasValue || !endDate.HasValue)
			{
				return;
			}

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
			if (!startDate.HasValue || !endDate.HasValue)
			{
				return;
			}

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
            UpdateChartTheme(ChartModel);
            UpdateChartTheme(ChartModelMoney);

        }
		/// <summary>
		/// Load dữ liệu để xuất file excel
		/// </summary>
		private void LoadDataToExportExcel()
        {
			if (!startDate.HasValue || !endDate.HasValue)
			{
				return;
			}

			var items = _dao.GetMedicineStatistic(startDate.Value, endDate.Value);
            foreach (var item in items)
            {
                MedicinesStatisticCommom.Add(item);
            }
        }
		/// <summary>
		/// Xuất file excel
		/// </summary>
		public async void MedicineExportToExcel()
        {
			if (!startDate.HasValue || !endDate.HasValue)
			{
				return;
			}

			LoadDataToExportExcel();

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Medicine Statistic");
                worksheet.Cells[1, 1].Value = "From " + startDate.Value.ToString("dd/MM/yyyy") + " to " + endDate.Value.ToString("dd/MM/yyyy");
                using (var range = worksheet.Cells[1, 1, 1, 4])
                {
                    range.Merge = true;
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                worksheet.Cells[3, 1].Value = "Date";
                worksheet.Cells[3, 2].Value = "Medicine name";
                worksheet.Cells[3, 3].Value = "Quantity sold";
                worksheet.Cells[3, 4].Value = "Revenue";
                using (var range = worksheet.Cells[3, 1, 3, 4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }
                int row = 4;
                for (DateTime date = startDate.Value.Date; date <= endDate.Value.Date; date = date.AddDays(1))
                {
                    decimal dailyTotal = 0;
                    var dateData = MedicinesStatisticCommom.Where(x => x.Date.Date == date);

                    worksheet.Cells[row, 1].Value = date.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 1].Style.Font.Bold = true;

                    if (dateData.Any())
                    {
                        foreach (var item in dateData)
                        {
                            worksheet.Cells[row, 2].Value = item.MedicineName;
                            worksheet.Cells[row, 3].Value = item.QuantitySold;
                            worksheet.Cells[row, 4].Value = item.Money;
                            dailyTotal += item.Money;
                            row++;
                        }
                    }
                    row++;
                    worksheet.Cells[row, 1].Value = "Daily Total:";
                    worksheet.Cells[row, 4].Value = dailyTotal;
                    worksheet.Cells[row, 1, row, 4].Style.Font.Bold = true;
                    worksheet.Cells[row, 1, row, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[row, 1, row, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

                    row++;
                }
                worksheet.Cells[1, 1, row - 1, 4].AutoFitColumns();

                var savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add("Excel Files", new List<string>() { ".xlsx" });
                savePicker.SuggestedFileName = $"Medicine_Statistics_{startDate:yyyyMMdd}-{endDate:yyyyMMdd}";

                var hwnd = ShellWindow.Current.GetWindowHandle();
                WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);

                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    using (var stream = await file.OpenStreamForWriteAsync())
                    {
                        package.SaveAs(stream);
                    }
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
