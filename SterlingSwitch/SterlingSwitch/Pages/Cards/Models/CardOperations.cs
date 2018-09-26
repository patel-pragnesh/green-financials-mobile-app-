using SterlingSwitch.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SterlingSwitch.Pages.Cards.Models
{
    public class FundWithWalletModel
    {
        public string toPAN { get; set; }
        public string WalletfromAcct { get; set; }
        public string Amt { get; set; }
        public string Narrations { get; set; }
        public string Referenceid { get; set; }
    }


    public class FundWithAccountModel
    {
        public string PAN { get; set; }
        public string Account { get; set; }
        public string Narration { get; set; }
        public string Amount { get; set; }
        public int RequestType { get; set; }
        public string Referenceid { get; set; }
        public string Translocation { get; set; }
    }


    public class FundWithCardModel
    {
        public string customerId { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string pin { get; set; }
        public string cvv { get; set; }
        public string expiry_date { get; set; }
        public string pan { get; set; }
        public string CreditAccount { get; set; }
        public string Referenceid { get; set; }
        public int RequestType { get; set; }
        public string Translocation { get; set; }
        public string remark { get; set; }
    }

    public class VirtualCard
    {
        public string UserEmail { get; set; }
        public bool IsVirtualCard { get; set; }
        public int IsVirtualCardActive { get; set; }
        public int VirtualCardRequestChannelID { get; set; }
        public int VirtualCardCurrency { get; set; }
        public int ProductID { get; set; }
        public string Account { get; set; }
    }

    public class RequestCardModel
    {
        public string Customernumber { get; set; }
        public string Currencycode { get; set; }
        public string Ledgercode { get; set; }
        public string CusName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Sex { get; set; }
        public string Cuscity { get; set; }
        public string Cardmidname { get; set; }
        public string Cusregion { get; set; }
        public string Account { get; set; }
        public string Cardfirstname { get; set; }
        public string Cardsurname { get; set; }
        public string Carddelivery { get; set; }
        public string Pindelivery { get; set; }
        public string PASNOM { get; set; }
        public string Dateissued { get; set; }
        public string PasExpDat { get; set; }
        public string PasPlace { get; set; }
        public string SecretQuer { get; set; }
        public string SecretAnsw { get; set; }
        public string Resident { get; set; }
        public string CountryRes { get; set; }
        public string CntryReg { get; set; }
        public string RegionReg { get; set; }
        public string CityReg { get; set; }
        public string ResAddress { get; set; }
        public string CntryLive { get; set; }
        public string Birthday { get; set; }
        public string GroupCmd { get; set; }
        public string FinProf { get; set; }
        public string productID { get; set; }
        public string RequestChannelID { get; set; }

        //public string CardType { get; set; }
    }

    [XmlRoot(ElementName = "IBSRequest")]
    public class ActivateCardModel : IBSRequest
    {
        public string PAN {get; set;}
        public string seq_nr {get; set;}
        public string PIN {get; set;}
        public string expiryDate {get; set;}
    }

    public class CardLimitReq
    {
        public string pan { get; set; }
        public string sequenceNumber { get; set; }
        public string CardProgram { get; set; }
        public string userEmail { get; set; }
    }

    public class CheckItem
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
