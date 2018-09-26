namespace switch_mobile.Services.Abstractions.Entities
{
    public class VendorResponse<T>
    {
        public string message { get; set; }
        public string response { get; set; }

        public T data { get; set; }
    }
}
