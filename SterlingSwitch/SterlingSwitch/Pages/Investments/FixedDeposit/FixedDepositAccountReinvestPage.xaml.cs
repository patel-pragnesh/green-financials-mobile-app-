using SterlingSwitch.Pages.Investments.ViewModel;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Templates;
using System.Collections.Generic;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Investments.FixedDeposit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FixedDepositAccountReinvestPage : SwitchMasterPage
    {
        private FixedDepositViewModel _vm;
        public string _depositDays { get; set; }
        public string _expectedReturn { get; set; }
        public string _maturity { get; set; }
        public FixedDepositAccountReinvestPage (string text, string text1)
		{
			InitializeComponent ();
		    this.BindingContext = _vm = new FixedDepositViewModel(Navigation);
          
            GetAccounts();
            ReInvestOption();
		}

        private void GetAccounts()
        {
            if (GlobalStaticFields.Customer?.ListOfAllAccounts != null && GlobalStaticFields.Customer?.ListOfAllAccounts.Count > 0)
            {
                List<string> acc = new List<string>();
                foreach (var item in GlobalStaticFields.Customer.ListOfAllAccounts)
                {
                    acc.Add(item.AccountNumberWithBalance);
                }
                dpdDebitAccount.ItemsSource = acc;               
            }
        }

        private void ReInvestOption()
        {
            List<string> vs = new List<string>()
            {
                "Principal only","Principal + Interest"
            };
            dpdReInvestOption.ItemsSource = vs;
        }

        private async void btnProceed_Clicked(object sender, System.EventArgs e)
        {
          if((dpdDebitAccount.SelectedIndex > -1) && (dpdReInvestOption.SelectedIndex > -1))            
           
            {
                MessageDialog.Show("OOPS", "Please choose an account to debit and enable re-invest option to proceed", DialogType.Error, "DISMISS", null);
            }
        }

        private void dpdReInvestOption_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            
        }

        private void dpdDebitAccount_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FixedDepositDataModel.DebitAccount = dpdDebitAccount.SelectedItem;
        }
    }
}