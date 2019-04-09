using System;

namespace DataModels
{
    public interface ITransaction
    {
        DateTime Date { get; set; }
        string Fund { get; set; }
        string Investor { get; set; }
        double Price { get; set; }
        string SalesRep { get; set; }
        double Shares { get; set; }
        string Type { get; set; }
    }
}