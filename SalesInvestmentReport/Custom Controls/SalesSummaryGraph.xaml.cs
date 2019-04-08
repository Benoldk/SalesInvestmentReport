using LiveCharts;
using System;
using System.Windows.Controls;

namespace SalesInvestmentReport.Custom_Controls
{
    /// <summary>
    /// Interaction logic for SalesSummaryGraph.xaml
    /// </summary>
    public partial class SalesSummaryGraph : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public SalesSummaryGraph()
        {
            InitializeComponent();

            Formatter = value => value.ToString("N");

            DataContext = this;
        }
    }
}
