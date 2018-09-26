using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Services.RestServices
{
    public class ExceptionModel
    {
        public string AppName { get; set; }
        public string Exception { get; set; }
        public string ExceptionRemark { get; set; }

        public string UserName { get; set; }
        public string Endpoint { get; set; }
        public string parameters { get; set; }

        public string ResponseFromAPI { get; set; }
        public string PageVisited { get; set; }
    }
}
