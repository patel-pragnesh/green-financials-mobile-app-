using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using SterlingSwitch.Pages.Investments.FixedDeposit;
using System;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.PopUps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FixedDepositTryDialog : PopupPage
    {
        
		public FixedDepositTryDialog (string depositAndDays, string expectedReturn, string maturity)
		{  
            InitializeComponent ();
		    Returnamount.Text = expectedReturn;
		    summary.Text = depositAndDays;
		    Duration.Text = maturity;
		}

        private void TryAnother_Tapped(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PopPopupAsync();
        }

        private void Proceed_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PopPopupAsync();
           // Navigation.PushAsync(new FixedDepositAccountReinvestPage(summary.Text, Returnamount.Text, Duration.Text));
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}