namespace switch_mobile.Services.Abstractions.Entities
{
    public class UploadFile
    {
        public string ReferenceID { get; set; }
        public int RequestType { get; set; }
        public string NUBAN { get; set; }
        public string ImageByte { get; set; }
    }
}
