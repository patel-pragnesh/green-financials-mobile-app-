using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Models
{
    //public class IBSResponse
    //{
    //    public string ReferenceID { get; set; }
    //    public string RequestType { get; set; }
    //    public string ResponseCode { get; set; }
    //    public string ResponseText { get; set; }
    //    // public T Transactions { get; set; }
    //}


    public class IbsresponseRoot
    {
        public IBSResponse IBSResponse { get; set; }
    }

    public class IBSResponse
    {
        public string ReferenceID { get; set; }
        public string RequestType { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseText { get; set; }
        public string AccountName { get; set; }
        public string SessionID { get; set; }
        public string TrnxTagID { get; set; }
        public Transactions Transactions { get; set; }
    }

    public class Transactions
    {
        public List<Transaction> Transaction { get; set; }
    }

   

}
