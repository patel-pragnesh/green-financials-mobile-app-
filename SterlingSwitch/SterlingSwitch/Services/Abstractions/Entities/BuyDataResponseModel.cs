using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class BuyDataResponseModel
    {
        public string message { get; set; }
        public string response { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string billerResponse { get; set; }
        public string status { get; set; }
    }
}
