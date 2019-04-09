using DataModels;
using NotVisualBasic.FileIO;
using Presenter.BusinessLogic;
using System;
using System.Collections.Generic;

namespace Presenter
{
    public class TransactionPresenter : ITransactionPresenter
    {
        protected ITransactionBusinessLogic businessLogic;

        public TransactionPresenter()
        {
            businessLogic = new TransactionBusinessLogic();
        }

        public List<ITransaction> LoadTransactions(string sFilePath)
        {
            List<ITransaction> transactions = new List<ITransaction>();
            try
            {
                if (!string.IsNullOrEmpty(sFilePath))
                {
                    using (CsvTextFieldParser csvParser = new CsvTextFieldParser(sFilePath))
                    {
                        csvParser.Delimiters = new[] { "," };
                        csvParser.ReadFields();
                        string[] fields;
                        while((fields = csvParser.ReadFields()) != null)
                        {
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
                            transactions.Add(trx);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return transactions;
        }

        public List<KeyValuePair<string, double>> GenerateSalesInceptionToDateSummaryByType(IEnumerable<ITransaction> transactions, string type)
        {
            return businessLogic.GenerateSalesInceptionToDateSummaryByType(transactions, type);
        }

        public Dictionary<string, List<KeyValuePair<DateTime, double>>> GenerateSalesMonthToDateSummaryByType(IEnumerable<ITransaction> transactions, string type)
        {
            return businessLogic.GenerateSalesMonthToDateSummaryByType(transactions, type);
        }

        public Dictionary<string, List<KeyValuePair<string, double>>> GenerateSalesQuarterToDateSummaryByType(IEnumerable<ITransaction> transactions, string type)
        {
            return businessLogic.GenerateSalesQuarterToDateSummaryByType(transactions, type);
        }

        public List<KeyValuePair<string, double>> GenerateSalesYearToDateSummaryByType(IEnumerable<ITransaction> transactions, string type)
        {
            return businessLogic.GenerateSalesYearToDateSummaryByType(transactions, type);
        }
    }
}