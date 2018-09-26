using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Models
{
    public class PhoneContact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get => $"{FirstName} {LastName}"; }

    }
}
