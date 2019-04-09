using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Presenter.BusinessLogic
{
    public class TransactionBusinessLogic : ITransactionBusinessLogic
    {
        public List<KeyValuePair<string, double>> GenerateSalesYearToDateSummaryByType(IEnumerable<ITransaction> transactions, string type)
        {
            List<KeyValuePair<string, double>> results = new List<KeyValuePair<string, double>>();

            foreach (var groupTrx in transactions.Where(t => t.Type == type).GroupBy(t => new { t.SalesRep }))
            {
                string key = groupTrx.Key.SalesRep;
                double aggregateYTD = 0;

                foreach (var grpFund in groupTrx.GroupBy(g => g.Fund))
                {
                    foreach (var data in grpFund)
                    {
                        aggregateYTD += (data.Shares * data.Price);
                    }
                }

                results.Add(new KeyValuePair<string, double>(key, aggregateYTD));
            }

            return results;
        }

        public Dictionary<string, List<KeyValuePair<DateTime, double>>> GenerateSalesMonthToDateSummaryByType(IEnumerable<ITransaction> transactions, string type)
        {
            var results = new Dictionary<string, List<KeyValuePair<DateTime, double>>>();

            foreach (var groupTrx in transactions.Where(t => t.Type == type).GroupBy(g => g.SalesRep))
            {
                var monthlyAmounts = new List<KeyValuePair<DateTime, double>>();

                foreach (var monthlySales in groupTrx.GroupBy(g => g.Date))
                {
                    double monthAggregate = 0;
                    foreach (var sale in monthlySales)
                    {
                        monthAggregate += (sale.Shares * sale.Price);
                    }

                    int daysInMonth = DateTime.DaysInMonth(monthlySales.Key.Year, monthlySales.Key.Month);
                    DateTime key = new DateTime(monthlySales.Key.Year, monthlySales.Key.Month, daysInMonth);
                    var monthResult = new KeyValuePair<DateTime, double>(key, monthAggregate);

                    monthlyAmounts.Add(monthResult);
                }

                results.Add(groupTrx.Key, monthlyAmounts);
            }

            return results;
        }

        public Dictionary<string, List<KeyValuePair<string, double>>> GenerateSalesQuarterToDateSummaryByType(IEnumerable<ITransaction> transactions, string type)
        {
            var quarterToDateResults = new Dictionary<string, List<KeyValuePair<string, double>>>();

            var monthToQuarters = new Dictionary<int, string>
            {
                { 1, "Q1" },
                { 2, "Q1" },
                { 3, "Q1" },
                { 4, "Q2" },
                { 5, "Q2" },
                { 6, "Q2" },
                { 7, "Q2" },
                { 8, "Q3" },
                { 9, "Q3" },
                { 10, "Q4" },
                { 11, "Q4" },
                { 12, "Q4" },
            };

            foreach (var trx in transactions.GroupBy(t => new { t.SalesRep }))
            {
                var quaterlyAmounts = new List<KeyValuePair<string, double>>();

                foreach (var quaterlySales in trx.GroupBy(g => monthToQuarters[g.Date.Month]))
                {
                    double quarterAggregate = 0;
                    foreach (var sale in quaterlySales)
                    {
                        quarterAggregate += (sale.Shares * sale.Price);
                    }

                    var quarterResult = new KeyValuePair<string, double>(quaterlySales.Key, quarterAggregate);

                    quaterlyAmounts.Add(quarterResult);
                }

                quarterToDateResults.Add(trx.Key.SalesRep, quaterlyAmounts);
            }

            return quarterToDateResults;
        }

        public List<KeyValuePair<string, double>> GenerateSalesInceptionToDateSummaryByType(IEnumerable<ITransaction> transactions, string type)
        {
            var summaryData = new List<KeyValuePair<string, double>>();

            

            foreach (var trx in transactions.GroupBy(t => new { t.SalesRep }))
            {

            }

            return summaryData;
        }
    }
}
