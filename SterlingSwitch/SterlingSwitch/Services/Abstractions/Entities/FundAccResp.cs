using System;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class FundAccResp
    {

        public class Rootobject
        {
            public string status { get; set; }
            public Data data { get; set; }
        }

        public class Data
        {
            public Transfer transfer { get; set; }
            public object authurl { get; set; }
            public object responsehtml { get; set; }
            public bool pendingValidation { get; set; }
            public string chargeMethod { get; set; }
        }

        public class Transfer
        {
            public int id { get; set; }
            public string type { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string phoneNumber { get; set; }
            public object recipientPhone { get; set; }
            public string status { get; set; }
            public object system_status { get; set; }
            public string medium { get; set; }
            public string ip { get; set; }
            public object exchangeRate { get; set; }
            public int amountToSend { get; set; }
            public int amountToCharge { get; set; }
            public string disburseCurrency { get; set; }
            public string chargeCurrency { get; set; }
            public string flutterChargeResponseCode { get; set; }
            public string flutterChargeResponseMessage { get; set; }
            public object flutterDisburseResponseMessage { get; set; }
            public string flutterChargeReference { get; set; }
            public object flutterDisburseReference { get; set; }
            public object flutterDisburseResponseCode { get; set; }
            public int merchantCommission { get; set; }
            public int moneywaveCommission { get; set; }
            public int netDebitAmount { get; set; }
            public int chargedFee { get; set; }
            public object receiptNumber { get; set; }
            public string redirectUrl { get; set; }
            public object linkingReference { get; set; }
            public string source { get; set; }
            public int source_id { get; set; }
            public Meta meta { get; set; }
            public object additionalFields { get; set; }
            public string _ref { get; set; }
            public int r1 { get; set; }
            public int r2 { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime updatedAt { get; set; }
            public object deletedAt { get; set; }
            public int userId { get; set; }
            public int merchantId { get; set; }
            public int beneficiaryId { get; set; }
            public object accountId { get; set; }
            public int cardId { get; set; }
            public object account { get; set; }
            public Beneficiary beneficiary { get; set; }
        }

        public class Meta
        {
        }

        public class Beneficiary
        {
            public int id { get; set; }
            public string accountNumber { get; set; }
            public string accountName { get; set; }
            public string bankCode { get; set; }
            public string bankName { get; set; }
            public int userId { get; set; }
            public string currency { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime updatedAt { get; set; }
            public object deletedAt { get; set; }
        }

    }
}
