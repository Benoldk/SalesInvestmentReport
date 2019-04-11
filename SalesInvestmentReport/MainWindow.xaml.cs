using Presenter;
using DataModels;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using System;
using System.Linq;
using System.Data;

namespace SalesInvestmentReport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        private const string SELLTYPE = "SELL";
        private const string BUYTYPE = "BUY";

        /// <summary>
        /// (MVP) Presenter object to retrieve data for the view and process
        /// background operations
        /// </summary>
        /// <remarks>
        /// MVP: Model View Presenter
        /// </remarks>
        private ITransactionPresenter presenter;

        /// <summary>
        /// List of Model objects representing the data loaded from the file
        /// </summary>
        private List<ITransaction> lstTransactions;
        #endregion

        /// <summary>
        /// View constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            presenter = new TransactionPresenter();
        }

        /// <summary>
        /// Event handler bound to custom command to open the file dialog to allow user to select the data file to load.
        /// Changes to the Sales Report tab once the data is loaded and enables the "Generate Report" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBinding_LoadReport(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                    var data = presenter.LoadTransactions(filePath);
                    lstTransactions = data.Value;
                    dgReportData.ItemsSource = new DataView(data.Key);
                    btnGenereReport.IsEnabled = true;
                    tcInvestorReport.SelectedItem = tabSalesReportData;
                }
            }
            catch
            {
                MessageBox.Show("The file was invalid. Please load a proper file.");
            }
        }

        /// <summary>
        /// Event Handler bound to custom command to generate the reports after the file has been loaded.
        /// Uses the presenter to get the data parse the transaction data to put into the Year to Date,
        /// Month to Date, Quarter to Date, Inception to Date, Assetsts Under Management, Break and Investor
        /// Profit reports.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBinding_GenerateReport(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            FormatSingleColumnSeriesChart(presenter.GenerateYearToDateSummaryByType(lstTransactions, SELLTYPE), saleSummaryYTDChart, "Year: 2018");
            FormatMultiColumnSeriesChart(presenter.GenerateMonthToDateSummaryByType(lstTransactions, SELLTYPE), saleSummaryMTDChart);
            FormatMultiColumnSeriesChart(presenter.GenerateQuarterToDateSummaryByType(lstTransactions, SELLTYPE), saleSummaryQTDChart);
            FormatMultiColumnSeriesChart(presenter.GenerateInceptionToDateSummary(lstTransactions), saleSummaryITDChart);
            FormatMultiColumnSeriesChart(presenter.GenerateAssetsUnderManagementSummary(lstTransactions), assetsUndertManagementSummaryChart);
            FormatMultiColumnSeriesChart(presenter.GenerateBreakReport(lstTransactions), breakReportChart);
            FormatMultiColumnSeriesChart(presenter.GenerateInvestorProfitReport(lstTransactions), investorProfitChart);
            tcInvestorReport.SelectedItem = tabSalesSummary;
        }

        /// <summary>
        /// Event Handler bound to custom commmand to close and exit the applicatio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBinding_ExitReport(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            Environment.Exit(0); // get out!!!
        }

        /// <summary>
        /// Helper function to create a simple column chart
        /// </summary>
        /// <param name="summaryData"></param>
        /// <param name="title"></param>
        private static void FormatSingleColumnSeriesChart(IEnumerable<KeyValuePair<string, double>> summaryData, Chart chart, string title)
        {
            ColumnSeries columnSeries = CreateColumnSeries(title, summaryData);
            chart.Series.Add(columnSeries);
        }

        /// <summary>
        /// Helper function to create a complex column chart
        /// </summary>
        /// <param name="summaryData"></param>
        /// <param name="chart"></param>
        private static void FormatMultiColumnSeriesChart(Dictionary<string, List<KeyValuePair<DateTime, double>>> summaryData, Chart chart)
        {
            foreach (var grpData in summaryData)
            {
                ColumnSeries columnSeries = CreateColumnSeries(grpData.Key, grpData.Value);
                chart.Series.Add(columnSeries);
            }
        }

        /// <summary>
        /// Helper function, overloaded, to create a complex column chart
        /// </summary>
        /// <param name="summaryData"></param>
        /// <param name="chart"></param>
        private static void FormatMultiColumnSeriesChart(Dictionary<string, List<KeyValuePair<string, double>>> summaryData, Chart chart)
        {
            foreach (var grpData in summaryData)
            {
                ColumnSeries columnSeries = CreateColumnSeries(grpData.Key, grpData.Value);
                chart.Series.Add(columnSeries);
            }
        }

        /// <summary>
        /// Helper function create columns
        /// </summary>
        /// <param name="title"></param>
        /// <param name="chartData"></param>
        /// <param name="dependentValuePath"></param>
        /// <param name="IndependentValuePath"></param>
        /// <returns></returns>
        private static ColumnSeries CreateColumnSeries(string title, System.Collections.IEnumerable chartData, string dependentValuePath = "Value", string IndependentValuePath = "Key" )
        {
            return new ColumnSeries
            {
                Title = title,
                DependentValuePath = dependentValuePath,
                IndependentValuePath = IndependentValuePath,
                ItemsSource = chartData
            };
        }
    }
}