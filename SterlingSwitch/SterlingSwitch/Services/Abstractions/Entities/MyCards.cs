using Xamarin.Forms;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class MyCards
    {
        public string CardPan { get; set; }
        public string MaskedPan { get; set; }
        public string ExpiryMonth { get; set; }
        public string CardName { get; set; }
        public string ExpiryYear { get; set; }
        public string CVV { get; set; }
        public string CardType { get; set; }
        public ImageSource CardLogo { get; set; }
        public Color HolderColor { get; internal set; }
        public string FormattedExp { get; set; }
        public string SequenceNumber { get; set; }
        public string CardProgram { get; set; }
        public string LastDigits { get; internal set; }
    }
}
