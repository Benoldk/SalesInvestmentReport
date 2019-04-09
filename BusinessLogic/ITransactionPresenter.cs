using DataModels;
using System;
using System.Collections.Generic;

namespace Presenter
{
    public interface ITransactionPresenter
    {
        List<ITransaction> LoadTransactions(string sFilePath);
        List<KeyValuePair<string, double>> GenerateSalesInceptionToDateSummaryByType(IEnumerable<ITransaction> transactions, string type);
        Dictionary<string, List<KeyValuePair<DateTime, double>>> GenerateSalesMonthToDateSummaryByType(IEnumerable<ITransaction> transactions, string type);
        Dictionary<string, List<KeyValuePair<string, double>>> GenerateSalesQuarterToDateSummaryByType(IEnumerable<ITransaction> transactions, string type);
        List<KeyValuePair<string, double>> GenerateSalesYearToDateSummaryByType(IEnumerable<ITransaction> transactions, string type);
    }
}