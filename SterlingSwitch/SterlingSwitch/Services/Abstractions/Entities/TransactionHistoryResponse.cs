using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
    public class TransactionHistoryResponse
    {

        public Ibsresponse IBSResponse { get; set; }

    }


        public class Ibsresponse
        {
            public Records Records { get; set; }
            public string ReferenceID { get; set; }
            public string RequestType { get; set; }
            public string ResponseCode { get; set; }
            public object ResponseText { get; set; }
        }

        public class Records
        {
            public Rec[] Rec { get; set; }
        }

        public class Rec
        {
            public string Amount { get; set; }
            public DateTime Date { get; set; }
            public string DC { get; set; }
            public string Remark { get; set; }
            public string Traid { get; set; }
        }


    




}
