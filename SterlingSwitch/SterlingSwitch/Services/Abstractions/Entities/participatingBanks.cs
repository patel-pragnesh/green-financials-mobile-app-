using System.Collections.ObjectModel;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class participatingBanks
    {
        public ObservableCollection<Banklist> bankList { get; set; }
        public ObservableCollection<string> bankNameList { get; set; }
        public int refid { get; set; }
        public string bankcode { get; set; }
        public string old_bankcode { get; set; }
        public string bankname { get; set; }
        public int category { get; set; }
        public int statusflag { get; set; }
        public string bankshort { get; set; }
        public int? TransRate { get; set; }
    }
}
