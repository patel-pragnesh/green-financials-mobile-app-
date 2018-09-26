using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class DealsResponseModel
    {

       
            public Deal[] Deals { get; set; }

        public class Deal
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Path { get; set; }
            public string IsVisible { get; set; }
            public DateTime DateAdded { get; set; }
            public DateTime DateStatusChanged { get; set; }
            public string ImageContent { get; set; }
        }

    }
}
