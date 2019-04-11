using System;

namespace DataModels
{
    /// <summary>
    /// Concrete Transaction class that extends interface's properties that mirror the data coming from the file.
    /// </summary>
    public class Transaction : ITransaction
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public double Shares { get; set; }
        public double Price { get; set; }
        public string Fund { get; set; }
        public string Investor { get; set; }
        public string SalesRep { get; set; }
    }
}