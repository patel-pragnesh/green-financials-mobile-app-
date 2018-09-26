using System;
using System.Collections.Generic;
using SterlingSwitch.Templates;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.CardlessWithdrawals
{
    public partial class CardlessWithdrawalSuccessful : SwitchMasterPage
    {
        public CardlessWithdrawalSuccessful()
        {
            InitializeComponent();
        }

        private void btnFindAtm_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FindATMPage());
        }
        private async void lblDismiss_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
