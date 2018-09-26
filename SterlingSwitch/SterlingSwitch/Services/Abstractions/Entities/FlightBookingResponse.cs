using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SterlingSwitch.Services.RestServices
{

    public class FlightBookingResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public Datas Data { get; set; }
    }

    public class Datas
    {
        public string BookingNo { get; set; }
        public object TicketMessage { get; set; }
        public string Message { get; set; }
        public bool PriceChanged { get; set; }
    }

}