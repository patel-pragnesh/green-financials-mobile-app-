using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Services.RestServices
{
    public class APIResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

   

}
