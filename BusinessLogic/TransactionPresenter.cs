using DataModels;
using NotVisualBasic.FileIO;
using Presenter.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;

namespace Presenter
{
    public class TransactionPresenter : ITransactionPresenter
    {
        protected ITransactionBusinessLogic businessLogic;

        public TransactionPresenter()
        {
            businessLogic = new TransactionBusinessLogic();
        }

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

        public List<KeyValuePair<string, double>> GenerateYearToDateSummaryByType(IEnumerable<ITransaction> transactions, string type)
        {
            return businessLogic.GenerateYearToDateSummaryByType(transactions, type);
        }

        public Dictionary<string, List<KeyValuePair<DateTime, double>>> GenerateMonthToDateSummaryByType(IEnumerable<ITransaction> transactions, string type)
        {
            return businessLogic.GenerateMonthToDateSummaryByType(transactions, type);
        }

        public Dictionary<string, List<KeyValuePair<string, double>>> GenerateQuarterToDateSummaryByType(IEnumerable<ITransaction> transactions, string type)
        {
            return businessLogic.GenerateQuarterToDateSummaryByType(transactions, type);
        }

        public Dictionary<string, List<KeyValuePair<string, double>>> GenerateInceptionToDateSummary(IEnumerable<ITransaction> transactions)
        {
            return businessLogic.GenerateInceptionToDateSummary(transactions);
        }

        public Dictionary<string, List<KeyValuePair<string, double>>> GenerateAssetsUnderManagementSummary(IEnumerable<ITransaction> transactions)
        {
            return businessLogic.GenerateAssetsUnderManagementSummary(transactions);
        }

        public Dictionary<string, List<KeyValuePair<string, double>>> GenerateBreakReport(IEnumerable<ITransaction> transactions)
        {
            return businessLogic.GenerateBreakReport(transactions);
        }

        public Dictionary<string, List<KeyValuePair<string, double>>> GenerateInvestorProfitReport(IEnumerable<ITransaction> transactions)
        {
            return businessLogic.GenerateInvestorProfitReport(transactions);
        }
    }
}