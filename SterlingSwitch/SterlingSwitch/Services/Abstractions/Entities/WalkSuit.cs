namespace switch_mobile.Services.Abstractions.Entities
{

    public class WalkSuit
    {
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public int Statusflag { get; set; }
        public string Memo { get; set; }
        public string DocNum { get; set; }
        public string CusNum { get; set; }
        public string Nuban { get; set; }
        public string SourceFlag { get; set; }
        public string ProductLedgerCode { get; set; }
    }

}
