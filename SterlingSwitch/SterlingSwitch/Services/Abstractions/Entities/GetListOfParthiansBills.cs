namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class GetListOfParthiansBills
    {
       
            public string Referenceid { get; set; }
            public int RequestType { get; set; }
            public string Translocation { get; set; }
            public string TransactionReference { get; set; }
            public string PhoneNumber { get; set; }
        

    }
}