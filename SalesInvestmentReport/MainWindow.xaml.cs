using Presenter;
using DataModels;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using System;
using System.Linq;

namespace SalesInvestmentReport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string SELLTYPE = "SELL";
        private const string BUYTYPE = "SELL";

        private ITransactionPresenter presenter;
        private List<ITransaction> lstTransactions;

        public MainWindow()
        {
            InitializeComponent();

            presenter = new TransactionPresenter();
        }

        private void CommandBinding_LoadReport(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                    lstTransactions = presenter.LoadTransactions(filePath);
                }
            }
            catch
            {
                MessageBox.Show("The file was invalid. Please load a proper file.");
            }
        }

        private void CommandBinding_GenerateReport(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            FormatYearToDateSummaryChart(presenter.GenerateSalesYearToDateSummaryByType(lstTransactions, SELLTYPE));
            FormatMonthToDateSummaryChart(presenter.GenerateSalesMonthToDateSummaryByType(lstTransactions, SELLTYPE));
            FormatQuarterToDateSummaryChart(presenter.GenerateSalesQuarterToDateSummaryByType(lstTransactions, SELLTYPE));
        }

        private void CommandBinding_SaveReport(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {

        }

        private void CommandBinding_ExitReport(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {

        }

        private void FormatYearToDateSummaryChart(IEnumerable<KeyValuePair<string, double>> summaryData)
        {
            ColumnSeries chartSeries = new ColumnSeries
            {
                Title = "2018",
                DependentValuePath = "Value",
                IndependentValuePath = "Key",
                ItemsSource = summaryData
            };

            saleSummaryYTDChart.Series.Add(chartSeries);
        }

        private void FormatMonthToDateSummaryChart(Dictionary<string, List<KeyValuePair<DateTime, double>>> summaryData)
        {
            foreach (var grpData in summaryData)
            {
                ColumnSeries chartSeries = new ColumnSeries
                {
                    Title = grpData.Key,
                    DependentValuePath = "Value",
                    IndependentValuePath = "Key",
                    ItemsSource = grpData.Value.OrderBy(o => o.Key)
                };

                saleSummaryMTDChart.Series.Add(chartSeries);
            }
        }

        private void FormatQuarterToDateSummaryChart(Dictionary<string, List<KeyValuePair<string, double>>> summaryData)
        {
            foreach (var grpData in summaryData)
            {
                ColumnSeries chartSeries = new ColumnSeries
                {
                    Title = grpData.Key,
                    DependentValuePath = "Value",
                    IndependentValuePath = "Key",
                    ItemsSource = grpData.Value
                };

                saleSummaryQTDChart.Series.Add(chartSeries);
            }
        }
    }
}