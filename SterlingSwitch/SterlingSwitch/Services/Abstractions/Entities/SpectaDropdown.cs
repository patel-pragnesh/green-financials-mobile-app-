using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{

    public class SpectaDropdown
    {
        public Dropdownlist[] dropdownList { get; set; }
    }

    public class Dropdownlist
    {
        public string dropdownListName { get; set; }
        public Value[] value { get; set; }
    }

    public class Value
    {
        public string descrip { get; set; }
        public string refid { get; set; }
    }

}
