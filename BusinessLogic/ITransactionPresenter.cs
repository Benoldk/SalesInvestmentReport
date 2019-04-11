using DataModels;
using System;
using System.Collections.Generic;
using System.Data;

namespace Presenter
{
    /// <summary>
    /// Contains function declarations for presenter to define for transaction processing
    /// </summary>
    public interface ITransactionPresenter
    {
        /// <summary>
        /// Loads data from csv file into a DataTable and a list of transactions
        /// </summary>
        /// <param name="sFilePath">file name and path to read and get data from</param>
        /// <returns>A DataTable and a list of transactions</returns>
        KeyValuePair<DataTable, List<ITransaction>> LoadTransactions(string sFilePath);

        /// <summary>
        /// Generates data for year to date sales for each fund sold by the sales reps
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<KeyValuePair<string, double>> GenerateYearToDateSummaryByType(IEnumerable<ITransaction> transactions, string type);

        /// <summary>
        /// Generates data for Month to date shares sold for each fund by the sales reps
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Dictionary<string, List<KeyValuePair<DateTime, double>>> GenerateMonthToDateSummaryByType(IEnumerable<ITransaction> transactions, string type);

        /// <summary>
        /// Generates data for Quarter to date shares sold for each fund by the sales reps
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Dictionary<string, List<KeyValuePair<string, double>>> GenerateQuarterToDateSummaryByType(IEnumerable<ITransaction> transactions, string type);

        /// <summary>
        /// Generates data for shares sold by the sales reps based in relation to the date the shares were first bought (inception date)
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        Dictionary<string, List<KeyValuePair<string, double>>> GenerateInceptionToDateSummary(IEnumerable<ITransaction> transactions);

        /// <summary>
        /// Generates data for the net amount held by investors across all funds whether positive or negative
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        Dictionary<string, List<KeyValuePair<string, double>>> GenerateAssetsUnderManagementSummary(IEnumerable<ITransaction> transactions);

        /// <summary>
        /// Generates data for the view to display to negative cash and share balances for each investor and fund
        /// </summary>
        /// <param name="lstTransactions"></param>
        /// <returns></returns>
        Dictionary<string, List<KeyValuePair<string, double>>> GenerateBreakReport(IEnumerable<ITransaction> lstTransactions);

        /// <summary>
        /// Generates data for for the view to display the net profit or loss on all investments
        /// </summary>
        /// <param name="lstTransactions"></param>
        /// <returns></returns>
        Dictionary<string, List<KeyValuePair<string, double>>> GenerateInvestorProfitReport(IEnumerable<ITransaction> lstTransactions);
    }
}