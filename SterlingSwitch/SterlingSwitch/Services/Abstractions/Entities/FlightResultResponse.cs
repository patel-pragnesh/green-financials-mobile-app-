namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class FlightResultResponse
    {
            public bool Status { get; set; }
            public string Message { get; set; }
            public Datum[] Data { get; set; }
       

        public class Datum
        {
            public int ProviderType { get; set; }
            public Flightclass FlightClass { get; set; }
            public string CurrencyCode { get; set; }
            public int Price { get; set; }
            public Leg[] Legs { get; set; }
            public Pricedetail[] PriceDetails { get; set; }
            public string OfferId { get; set; }
            public int TotalAdults { get; set; }
            public string DepartureTime { get; set; }
            public string TripDuration { get; set; }
            public bool IsRefundable { get; set; }
        }

        public class Flightclass
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }

        public class Leg
        {
            public string DepartureCode { get; set; }
            public string DepartureName { get; set; }
            public string DepartureTime { get; set; }
            public string DepartureDate { get; set; }
            public string ArrivalCode { get; set; }
            public string ArrivalName { get; set; }
            public string ArrivalTime { get; set; }
            public string ArrivalDate { get; set; }
            public string CarrierName { get; set; }
            public string CarrierCode { get; set; }
            public string CarrierLogoUrl { get; set; }
            public int TotalStopOver { get; set; }
            public Extra[] Extras { get; set; }
        }

        public class Extra
        {
            public string DepartureName { get; set; }
            public string DepartureTime { get; set; }
            public string ArrivalName { get; set; }
            public string ArrivalTime { get; set; }
            public string Duration { get; set; }
            public string FlightNo { get; set; }
            public string AircraftNo { get; set; }
            public string Carrier { get; set; }
            public int TotalSeatsAvailable { get; set; }
        }

        public class Pricedetail
        {
            public string Passenger { get; set; }
            public string Tax { get; set; }
            public string PricePerPerson { get; set; }
            public string TotalPrice { get; set; }
        }


    }
}
