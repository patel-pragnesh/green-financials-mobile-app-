namespace switch_mobile.Services.Abstractions.Entities
{
	public class NameValRequest
	{
		public string Referenceid { get; set; }
		public string RequestType { get; set; }
		public string Translocation { get; set; }
		public string nuban { get; set; }
		public string destinationbankcode { get; set; }
	}


	public class NameValRequestResponse
	{
		public string message { get; set; }
		public string response { get; set; }
		public object responsedata { get; set; }
		public NameValRequestResponseData data { get; set; }
	}

	public class NameValRequestResponseData
	{
		public string AccountName { get; set; }
		public string AccountNumber { get; set; }
		public object status { get; set; }
	}

}
