using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SterlingSwitch.Models.SomeConstant;

namespace SterlingSwitch.Pages.Cards
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CardDetailsPage : SwitchMasterPage
    {
        public CardDetailsPage(MyCards card)
        {
            InitializeComponent();
            SetCardDetails(card);

            //get name of the page
            var x = (this.GetType().Name);
            BusinessLogic.LogFrequentPage(x, PageAliasConstant.CardPage, ImageConstants.CardPageIcon);


        }


        private void SetCardDetails(MyCards card)
        {
            DigitSegOneTxt.Text = card.CardPan.Substring(0, 4);
            DigitSegTwoTxt.Text = card.CardPan.Substring(4, 4);
            DigitSegThreeTxt.Text = card.CardPan.Substring(8, 4);
            DigitSegFourTxt.Text = card.CardPan.Substring(12, 4);

            CardNameTxt.Text = card.CardName;
            CardExpiryTxt.Text = $"{card.ExpiryMonth}/{card.ExpiryYear}";
            CardCVVTxt.Text = card.CVV;

            CardLogo.Source = card.CardLogo;
        }
    }
}