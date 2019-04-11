using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Presenter.BusinessLogic
{
    /// <summary>
    /// Transaction Business Logic concrete class that defines function from
    /// interface to operate on data as needed presenting to the view.
    /// </summary>
    public class TransactionBusinessLogic : ITransactionBusinessLogic
    {
        /// <summary>
        /// Generates data for year to date sales for each fund sold by the sales reps
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, double>> GenerateYearToDateSummaryByType(IEnumerable<ITransaction> transactions, string type)
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

        /// <summary>
        /// Generates data for Month to date shares sold for each fund by the sales reps
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Dictionary<string, List<KeyValuePair<DateTime, double>>> GenerateMonthToDateSummaryByType(IEnumerable<ITransaction> transactions, string type)
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

        /// <summary>
        /// Generates data for Quarter to date shares sold for each fund by the sales reps
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Dictionary<string, List<KeyValuePair<string, double>>> GenerateQuarterToDateSummaryByType(IEnumerable<ITransaction> transactions, string type)
        {
            // result to return
            var quarterToDateResults = new Dictionary<string, List<KeyValuePair<string, double>>>();

            // month by the quarters they fall into
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

        /// <summary>
        /// Generates data for shares sold by the sales reps based in relation to the date the shares were first bought (inception date)
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        public Dictionary<string, List<KeyValuePair<string, double>>> GenerateInceptionToDateSummary(IEnumerable<ITransaction> transactions)
        {
            var summaryData = new Dictionary<string, List<KeyValuePair<string, double>>>();

            // funds bought grouped by investor, fund name, sales rep
            var boughtInvestorFundGroup = transactions.Where(w => w.Type == "BUY").GroupBy(g => new { g.Investor, g.Fund, g.SalesRep  });

            // funds sold grouped by investor, fund name, sales rep
            var soldInvestorFundGroup = transactions.Where(w => w.Type == "SELL").GroupBy(g => new { g.Investor, g.Fund, g.SalesRep  });

            DateTime maxSellDate = transactions.Where(w => w.Type == "SELL").OrderByDescending(o => o.Date).Select(s => s.Date).First();
            
            foreach (var fundsBoughtGrp in boughtInvestorFundGroup)
            {
                List<KeyValuePair<string, double>> lstFundInceptionToDateData = new List<KeyValuePair<string, double>>();
                var selectedBuyGroup = fundsBoughtGrp.OrderBy(o => o.Date).Select(s => new { s.Investor, s.Fund, s.SalesRep, s.Date  }).First();
                string inceptionDate = selectedBuyGroup.Date.ToShortDateString();
                string sellDate = maxSellDate.ToShortDateString();
                foreach(var sellGrp in soldInvestorFundGroup)
                {
                    var soldFunds = sellGrp.Where(w => w.Investor == selectedBuyGroup.Investor
                                                        && w.Fund == selectedBuyGroup.Fund
                                                        && w.SalesRep == selectedBuyGroup.SalesRep);

                    double totalSold = 0;
                    if (soldFunds != null)
                    {
                        foreach(var fund in soldFunds.OrderByDescending(o => o.Date))
                        {
                            totalSold += fund.Shares * fund.Price;
                            sellDate = fund.Date.ToShortDateString();
                        }
                    }

                    string inceptionDateToSoldDate = string.Format("{0} - {1}", inceptionDate, sellDate);
                    KeyValuePair<string, double> fundSoldInceptionToCurDate = new KeyValuePair<string, double>(inceptionDateToSoldDate, totalSold);
                    lstFundInceptionToDateData.Add(fundSoldInceptionToCurDate);
                    continue;
                }
                string investorToFund = string.Format("{0} : {1}", selectedBuyGroup.Investor, selectedBuyGroup.Fund);
                summaryData.Add(investorToFund, lstFundInceptionToDateData);
                continue;
            }

            return summaryData;
        }

        /// <summary>
        /// Generates data for the net amount held by investors across all funds whether positive or negative
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        public Dictionary<string, List<KeyValuePair<string, double>>> GenerateAssetsUnderManagementSummary(IEnumerable<ITransaction> transactions)
        {
            var results = new Dictionary<string, List<KeyValuePair<string, double>>>();

            // funds bought grouped by investor, fund name, sales rep
            var boughtInvestorFundGroups = transactions.Where(w => w.Type == "BUY").GroupBy(g => new { g.Investor, g.Fund, g.SalesRep });

            // funds sold grouped by investor, fund name, sales rep
            var soldInvestorFundGroups = transactions.Where(w => w.Type == "SELL").GroupBy(g => new { g.Investor, g.Fund, g.SalesRep });

            foreach (var fundsBoughtGrp in boughtInvestorFundGroups)
            {
                List<KeyValuePair<string, double>> lstInvestorNetSharesHeld = new List<KeyValuePair<string, double>>();
                double sharesBoughtSum = fundsBoughtGrp.Sum(s => s.Shares);

                var fundsBought = fundsBoughtGrp.First();
                double sharesSoldSum = 0;
                foreach (var fundsSoldGrp in soldInvestorFundGroups)
                {
                    var soldFunds = fundsSoldGrp.Where(w => w.Investor == fundsBought.Investor
                                                        && w.Fund == fundsBought.Fund
                                                        && w.SalesRep == fundsBought.SalesRep);

                    sharesSoldSum += soldFunds.Sum(s => s.Shares);
                }

                double netShares = sharesSoldSum - sharesBoughtSum;
                lstInvestorNetSharesHeld.Add(new KeyValuePair<string, double>(fundsBought.Fund, netShares));
                results.Add(string.Format("{0}: {1}", fundsBought.Investor, fundsBought.Fund), lstInvestorNetSharesHeld);
            }

            return results;
        }

        /// <summary>
        /// Generates data for the view to display to negative cash and share balances for each investor and fund
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        public Dictionary<string, List<KeyValuePair<string, double>>> GenerateBreakReport(IEnumerable<ITransaction> transactions)
        {
            var results = new Dictionary<string, List<KeyValuePair<string, double>>>();

            // funds bought grouped by investor, fund name
            var boughtInvestorFundGroups = transactions.Where(w => w.Type == "BUY").GroupBy(g => new { g.Investor, g.Fund });

            // funds sold grouped by investor, fund name
            var soldInvestorFundGroups = transactions.Where(w => w.Type == "SELL").GroupBy(g => new { g.Investor, g.Fund });

            foreach (var fundsBoughtGrp in boughtInvestorFundGroups)
            {
                List<KeyValuePair<string, double>> lstInvestorTradesBreak = new List<KeyValuePair<string, double>>();
                double sharesBoughtSum = fundsBoughtGrp.Sum(s => s.Shares);
                double cashSpent = fundsBoughtGrp.Sum(s => s.Price * s.Shares);

                var fundsBought = fundsBoughtGrp.First();
                double sharesSoldSum = 0;
                double cashMadeSum = 0;
                foreach (var fundsSoldGrp in soldInvestorFundGroups)
                {
                    var soldFunds = fundsSoldGrp.Where(w => w.Investor == fundsBought.Investor
                                                        && w.Fund == fundsBought.Fund
                                                        && w.SalesRep == fundsBought.SalesRep);

                    sharesSoldSum += soldFunds.Sum(s => s.Shares);
                    cashMadeSum += soldFunds.Sum(s => s.Price * s.Shares);
                }

                double shareDiff = sharesSoldSum - sharesBoughtSum;
                double cashDiff = cashMadeSum - cashSpent;

                if (shareDiff < 0)
                    lstInvestorTradesBreak.Add(new KeyValuePair<string, double>("Shares", shareDiff));

                if(cashDiff < 0)
                    lstInvestorTradesBreak.Add(new KeyValuePair<string, double>("Cash", cashDiff));

                results.Add(string.Format("{0}: {1}", fundsBought.Investor, fundsBought.Fund), lstInvestorTradesBreak);
            }

            return results;
        }

        /// <summary>
        /// Generates data for for the view to display the net profit or loss on all investments
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        public Dictionary<string, List<KeyValuePair<string, double>>> GenerateInvestorProfitReport(IEnumerable<ITransaction> transactions)
        {
            var results = new Dictionary<string, List<KeyValuePair<string, double>>>();

            // funds bought grouped by investor, fund name
            var boughtInvestorFundGroups = transactions.Where(w => w.Type == "BUY").GroupBy(g => new { g.Investor, g.Fund });

            // funds sold grouped by investor, fund name
            var soldInvestorFundGroups = transactions.Where(w => w.Type == "SELL").GroupBy(g => new { g.Investor, g.Fund });

            foreach (var fundsBoughtGrp in boughtInvestorFundGroups)
            {
                List<KeyValuePair<string, double>> lstInvestorProfitsLosses = new List<KeyValuePair<string, double>>();
                double cashSpent = fundsBoughtGrp.Sum(s => s.Price * s.Shares);

                var fundsBought = fundsBoughtGrp.First();
                double cashMadeSum = 0;
                foreach (var fundsSoldGrp in soldInvestorFundGroups)
                {
                    var soldFunds = fundsSoldGrp.Where(w => w.Investor == fundsBought.Investor
                                                        && w.Fund == fundsBought.Fund
                                                        && w.SalesRep == fundsBought.SalesRep);
                    
                    cashMadeSum += soldFunds.Sum(s => s.Price * s.Shares);
                }

                double profitLoss = cashMadeSum - cashSpent;
                
                lstInvestorProfitsLosses.Add(new KeyValuePair<string, double>(fundsBought.Fund, profitLoss));

                results.Add(string.Format("{0}: {1}", fundsBought.Investor, fundsBought.Fund), lstInvestorProfitsLosses);
            }

            return results;
        }
    }
}