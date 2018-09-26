using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class AllParthianAvailable
    {
        public class Rootobject
        {
            public PathInv[] Property1 { get; set; }
        }

        public class PathInv
        {
            public int Id { get; set; }
            public int InstrumentTypeId { get; set; }
            public string Name { get; set; }
            public string Maturity { get; set; }
            public float Coupon { get; set; }
            public float InitialVolume { get; set; }
            public float AvailableVolume { get; set; }
            public float SubscribedVolume { get; set; }
            public float Price { get; set; }
            public float Tenor { get; set; }
            public float InterestRate { get; set; }
            public bool AvailableForAuction { get; set; }
            public int Status { get; set; }
            public string LastUpdated { get; set; }
            public string Description { get; set; }
            public bool IsPendingAction { get; set; }
            public Instrumenttype InstrumentType { get; set; }
        }

        public class Instrumenttype
        {
            public int InstrumentTypeId { get; set; }
            public string Name { get; set; }
            public object Instruments { get; set; }
        }

    }
}
