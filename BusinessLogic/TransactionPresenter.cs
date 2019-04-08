using DataModels;
using System;
using System.Collections.Generic;
using System.IO;

namespace BusinessLogic
{
    public class TransactionPresenter
    {
        public List<ITransaction> LoadTransactions(string sFilePath)
        {
            List<ITransaction> transactions = new List<ITransaction>();
            try
            {
                if (!string.IsNullOrEmpty(sFilePath))
                {
                    using (StreamReader sr = new StreamReader(new FileStream(sFilePath, FileMode.Open, FileAccess.Read)))
                    {
                        while (!sr.EndOfStream)
                        {
                            string[] commaSeparatedData = sr.ReadLine().Split(',');
                            transactions.Add(new Transaction
                            {
                                Date = DateTime.Parse(commaSeparatedData[0]),
                                Type = commaSeparatedData[1],
                                Shares = double.Parse(commaSeparatedData[2]),
                                Price = double.Parse(commaSeparatedData[3]),
                                Fund = commaSeparatedData[4],
                                Investor = commaSeparatedData[5],
                                SalesRep = commaSeparatedData[6]
                            });
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
    }
}