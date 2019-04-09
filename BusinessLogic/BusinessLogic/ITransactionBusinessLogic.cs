using DataModels;
using System;
using System.Collections.Generic;

namespace Presenter.BusinessLogic
{
    public interface ITransactionBusinessLogic
    {
        List<KeyValuePair<string, double>> GenerateSalesInceptionToDateSummaryByType(IEnumerable<ITransaction> transactions, string type);
        Dictionary<string, List<KeyValuePair<DateTime, double>>> GenerateSalesMonthToDateSummaryByType(IEnumerable<ITransaction> transactions, string type);
        Dictionary<string, List<KeyValuePair<string, double>>> GenerateSalesQuarterToDateSummaryByType(IEnumerable<ITransaction> transactions, string type);
        List<KeyValuePair<string, double>> GenerateSalesYearToDateSummaryByType(IEnumerable<ITransaction> transactions, string type);
    }
}