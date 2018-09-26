namespace switch_mobile.Services.Abstractions.Entities
{
    public class SendSMS
    {
        

        public int ReferenceID { get; set; }
        public int RequestType { get; set; }
        public string Msg { get; set; }
        public string gsm { get; set; }
    }
}
