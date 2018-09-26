using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class DailyTipsResponseModel
    {
        public Tip[] Data { get; set; }
        

        public class Tip
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string ImagePath { get; set; }
        }

    }
}
