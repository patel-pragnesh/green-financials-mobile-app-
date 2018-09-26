namespace switch_mobile.Services.Abstractions.Entities
{

    public class StatusMessage
    {
        public string message { get; set; }
        public string response { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string status { get; set; }
    }
}
