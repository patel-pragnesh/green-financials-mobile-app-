using System;
namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class AirtimeSavedBeneficiary
    {
		public int Id { get; set; }
        public string Amount { get; set; }
        public string Beneficiary { get; set; }
        public string Mobile { get; set; }
        public string NUBAN { get; set; }
        public string RequestType { get; set; }
        public int NetworkID { get; set; }
        public string ReferenceID { get; set; }
        public string Type { get; set; }
        public DateTime DateAdded { get; set; }
        public string UserId { get; set; }
        public bool IsVisible { get; set; }

    }
}
