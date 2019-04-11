using DataModels;
using NotVisualBasic.FileIO;
using Presenter.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;

namespace Presenter
{
    /// <summary>
    /// Concreate Presenter class to process data and return models to the view for display
    /// </summary>
    public class TransactionPresenter : ITransactionPresenter
    {
        /// <summary>
        /// Business logic object to run business specific calculations on data
        /// before being presented to the view
        /// </summary>
        protected ITransactionBusinessLogic businessLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        public TransactionPresenter()
        {
            businessLogic = new TransactionBusinessLogic();
        }

        /// <summary>
        /// Loads data from csv file into a DataTable and a list of transactions
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <returns></returns>
        public KeyValuePair<DataTable, List<ITransaction>> LoadTransactions(string sFilePath)
        {
            KeyValuePair<DataTable, List<ITransaction>> viewDataAndTransactions = 
                new KeyValuePair<DataTable, List<ITransaction>>(new DataTable(), new List<ITransaction>());

            try
            {
                if (!string.IsNullOrEmpty(sFilePath))
                {
                    using (CsvTextFieldParser csvParser = new CsvTextFieldParser(sFilePath))
                    {
                        csvParser.Delimiters = new[] { "," };
                        string[] headers = csvParser.ReadFields();

                        foreach(var header in headers)
                        {
                            viewDataAndTransactions.Key.Columns.Add(header.Trim());
                        }

                        string[] fields;
                        while((fields = csvParser.ReadFields()) != null)
                        {
                            // add data to view data
                            viewDataAndTransactions.Key.Rows.Add(fields);

                            // create transaction data object
                            ITransaction trx = new Transaction
                            {
                                Date = DateTime.Parse(fields[0].Trim()),
                                Type = fields[1].Trim(),
                                Shares = double.Parse(fields[2].Trim()),
                                Price = double.Parse(fields[3].TrimStart('$').Trim()),
                                Fund = fields[4].Trim(),
                                Investor = fields[5].Trim(),
                                SalesRep = fields[6]
                            };
                            viewDataAndTransactions.Value.Add(trx);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return viewDataAndTransactions;
        }

        /// <summary>
        /// Generates data for year to date sales for each fund sold by the sales reps
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, double>> GenerateYearToDateSummaryByType(IEnumerable<ITransaction> transactions, string type)
        {
            return businessLogic.GenerateYearToDateSummaryByType(transactions, type);
        }

        /// <summary>
        /// Generates data for Month to date shares sold for each fund by the sales reps
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Dictionary<string, List<KeyValuePair<DateTime, double>>> GenerateMonthToDateSummaryByType(IEnumerable<ITransaction> transactions, string type)
        {
            return businessLogic.GenerateMonthToDateSummaryByType(transactions, type);
        }

        /// <summary>
        /// Generates data for Quarter to date shares sold for each fund by the sales reps
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Dictionary<string, List<KeyValuePair<string, double>>> GenerateQuarterToDateSummaryByType(IEnumerable<ITransaction> transactions, string type)
        {
            return businessLogic.GenerateQuarterToDateSummaryByType(transactions, type);
        }

        /// <summary>
        /// Generates data for shares sold by the sales reps based in relation to the date the shares were first bought (inception date)
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        public Dictionary<string, List<KeyValuePair<string, double>>> GenerateInceptionToDateSummary(IEnumerable<ITransaction> transactions)
        {
            return businessLogic.GenerateInceptionToDateSummary(transactions);
        }

        /// <summary>
        /// Generates data for the net amount held by investors across all funds whether positive or negative
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        public Dictionary<string, List<KeyValuePair<string, double>>> GenerateAssetsUnderManagementSummary(IEnumerable<ITransaction> transactions)
        {
            return businessLogic.GenerateAssetsUnderManagementSummary(transactions);
        }

        /// <summary>
        /// Generates data for the view to display to negative cash and share balances for each investor and fund
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        public Dictionary<string, List<KeyValuePair<string, double>>> GenerateBreakReport(IEnumerable<ITransaction> transactions)
        {
            return businessLogic.GenerateBreakReport(transactions);
        }

        /// <summary>
        /// Generates data for for the view to display the net profit or loss on all investments
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        public Dictionary<string, List<KeyValuePair<string, double>>> GenerateInvestorProfitReport(IEnumerable<ITransaction> transactions)
        {
            return businessLogic.GenerateInvestorProfitReport(transactions);
        }
    }
}