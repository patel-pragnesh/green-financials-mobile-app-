using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
    public class sendEmai
    {
        public  string Body { get; set; }

        public  string DestinationEmail { get; set; }

        public  string SourceEmail { get; set; }

        public  string Subject { get; set; }

    }
}


