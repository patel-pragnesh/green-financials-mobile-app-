using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SterlingSwitch.Pages.Investments.FixedDeposit;
using SterlingSwitch.Pages.Investments.TreasuryBills;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SterlingSwitch.Pages.Investments.AllInvestments;

namespace SterlingSwitch.Pages.Pagelanding
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InvestmentLanding : SwitchMasterPage
    {
		public InvestmentLanding ()
		{
			InitializeComponent ();
		}

        private void Tbills_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TreasuryBillsPage(), true);
        }

        private void FD_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FixedDepositPage(), true);
        }

        private void AllInvestments_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AllInvestments(), true);
        }
    }
}