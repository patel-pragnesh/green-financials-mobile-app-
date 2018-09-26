using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class CreateParthianUser
    {
       public string Referenceid { get; set; }
       public int    RequestType { get; set; }
        public string Translocation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AccountNo { get; set; }
    }
}
