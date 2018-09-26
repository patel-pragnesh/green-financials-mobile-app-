namespace switch_mobile.Services.Abstractions.Entities
{

    public class IBSResponse
    { 
        public string ReferenceID { get; set; }
 
        public int RequestType { get; set; }
 
        public string ResponseCode { get; set; }
 
        public string ResponseText { get; set; }
    }

}
