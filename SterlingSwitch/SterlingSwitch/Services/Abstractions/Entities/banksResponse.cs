namespace SterlingSwitch.Services.Abstractions.Entities
{


    public class banksResponse
    {
        public Datum data { get; set; }
    }

    public class Datum
    {
        public int refid { get; set; }
        public string bankcode { get; set; }
        public string old_bankcode { get; set; }
        public string bankname { get; set; }
        public int category { get; set; }
        public int statusflag { get; set; }
        public string bankshort { get; set; }
        public int? transRate { get; set; }
    }


}
