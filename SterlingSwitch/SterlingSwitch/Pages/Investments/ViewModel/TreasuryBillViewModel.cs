using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Pages.Investments.ViewModel
{
    public class TreasuryBillViewModel
    {
        public string Referenceid { get; set; }
        public int RequestType { get; set; }
        public string Translocation { get; set; }
        public string TransactionReference { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class TreasuryBill
    {
        public int Id { get; set; }
        public int InstrumentTypeId { get; set; }
        public string Name { get; set; }
        public string Maturity { get; set; }
        public float Coupon { get; set; }
        public float InitialVolume { get; set; }
        public float AvailableVolume { get; set; }
        public float SubscribedVolume { get; set; }
        public float Price { get; set; }
        public float Tenor { get; set; }
        public float InterestRate { get; set; }
        public bool AvailableForAuction { get; set; }
        public int Status { get; set; }
        public string LastUpdated { get; set; }
        public string Description { get; set; }
        public bool IsPendingAction { get; set; }
        public Instrumenttype InstrumentType { get; set; }
    }

    public class Instrumenttype
    {
        public int InstrumentTypeId { get; set; }
        public string Name { get; set; }
        public object Instruments { get; set; }
    }

    public class SpayResponse
    {
        public string message { get; set; }
        public string response { get; set; }
        public string responsedata { get; set; }
        public object data { get; set; }
    }


    public class ComputeMaturityModel
    {
        public string Referenceid { get; set; }
        public int RequestType { get; set; }
        public string Translocation { get; set; }
        public string InstrumentsID { get; set; }
        public string PhoneNumber { get; set; }
        public string Amount { get; set; }
    }


    public class MaturityModel
    {
        public float ValueAtMaturity { get; set; }
        public float TotalProfit { get; set; }
        public float DailyProfit { get; set; }
    }


}
