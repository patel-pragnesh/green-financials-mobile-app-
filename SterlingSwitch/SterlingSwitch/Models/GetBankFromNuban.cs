using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Models
{
    public class GetBankFromNuban
    {
        //public bool Status { get; set; }
        //public string Message { get; set; }
        //public List<string> Data { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        //public Datum[] Data { get; set; }
        public List<BAnkNameCode> Data { get; set; }
    }

    public class BAnkNameCode
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }


}
