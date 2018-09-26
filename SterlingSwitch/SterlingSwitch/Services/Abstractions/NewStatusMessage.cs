using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Services.Abstractions
{
    public class NewStatusMessage
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
