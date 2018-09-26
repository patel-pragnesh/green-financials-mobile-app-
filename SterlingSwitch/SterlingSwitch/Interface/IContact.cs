using System;
using System.Collections.Generic;
using System.Text;
using SterlingSwitch.Models;

namespace SterlingSwitch.Interface
{
    public interface IContact
    {   
       IEnumerable<PhoneContact> GetAllContacts();
    }
}
