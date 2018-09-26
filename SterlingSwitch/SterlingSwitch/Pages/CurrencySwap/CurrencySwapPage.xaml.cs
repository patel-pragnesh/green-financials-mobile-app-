using SterlingSwitch.Helper;
using SterlingSwitch.Pages.CurrencySwap.ViewModel;
using SterlingSwitch.PopUps;
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

namespace SterlingSwitch.Pages.CurrencySwap
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CurrencySwapPage : SwitchMasterPage
	{
        CurrencySwapViewModel vm;
		public CurrencySwapPage ()
		{
            try
            {
                InitializeComponent();

                vm = new CurrencySwapViewModel(Navigation);

                BindingContext = vm;
                //AmtRev.Text = "eg. NGN 0.0";
                //AmountSentInForeignCurrecy.InputTransparent = true;

                //get name of the page
                var x = (this.GetType().Name);
                BusinessLogic.LogFrequentPage(x, PageAliasConstant.currencyswap, ImageConstants.AirtimeDataIcon);



            }
            catch (Exception rx)
            {
                rx.Message.ToString();
                //throw;
            }
		}

        protected async override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                if (vm != null)
                {
                   // var pb = await ProgressDialog.Show("Please wait...");
                    vm.InitailStartObject();
                    FromCurrency.ItemsSource = vm.ListFromCurrency;
                   // ToCurrecny.ItemsSource = vm.ListToCuurency.ToList();
                   // await pb.Dismiss(); 
                }


            }
            catch (Exception ex)
            {
                ex.Message.ToString();
               // throw;
            }
        }

        private void FromCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedValue = FromCurrency.SelectedItem;

               
                if (string.IsNullOrEmpty(selectedValue))
                {
                    //AmtRev.Text = "eg. NGN 0.0";
                    return;
                }

                if (string.IsNullOrEmpty(AmountSentInForeignCurrecy.Text))
                {
                    //AmtRev.Text = "eg. NGN 0.0";
                    return;
                }
                //AmountSentInForeignCurrecy.InputTransparent = false;
                AmountSentInForeignCurrecy.IsCurrencyVisible = true;
                string AmtyRecieve = string.Empty;
                vm.AmountSent = AmountSentInForeignCurrecy.Text;
                if (selectedValue == "USD")
                {
                    AmountSentInForeignCurrecy.CurrencySymbol = "USD";
                    AmtyRecieve = $" {string.Format("{0:N}",double.Parse(vm.USDCurrencyValue) * double.Parse(AmountSentInForeignCurrecy.Text))}";
                    AmtRev.Text = AmtyRecieve;
                }
                if (selectedValue == "GBP")
                {
                    AmountSentInForeignCurrecy.CurrencySymbol = "GBP";
                    AmtyRecieve = $" {string.Format("{0:N}", double.Parse(vm.GBPCurrencyValue) * double.Parse(AmountSentInForeignCurrecy.Text))}";
                    AmtRev.Text = AmtyRecieve;
                }
                if (selectedValue == "EUR")
                {
                    AmountSentInForeignCurrecy.CurrencySymbol = "EUR";
                    AmtyRecieve = $" {string.Format("{0:N}", double.Parse(vm.EURCurrecnyValue) * double.Parse(AmountSentInForeignCurrecy.Text))}";
                    AmtRev.Text = AmtyRecieve;
                }


                // }


            }
            catch (Exception ex)
            {
                ex.Message.ToString();
               // throw;
            }
            
        }

        private void btnContinue_Clicked(object sender, EventArgs e)
        {
            vm.RefernceMsg = ReferenceMessage.Text;
            double value;
            var foreignAmt = double.TryParse(AmountSentInForeignCurrecy.Text,out value) ? value  : (double?)null;

            vm.SelectedCurrencyValue = FromCurrency.SelectedItem;
            vm.AmountSent = AmountSentInForeignCurrecy.Text;
            if (foreignAmt != null)
            {
                if (vm.SelectedCurrencyValue == "USD")
                    vm.AmountRecieved = (double.Parse(vm.USDCurrencyValue) * double.Parse(AmountSentInForeignCurrecy.Text)).ToString();
                else if (vm.SelectedCurrencyValue == "GBP")
                    vm.AmountRecieved = (double.Parse(vm.GBPCurrencyValue) * double.Parse(AmountSentInForeignCurrecy.Text)).ToString();
                else if(vm.SelectedCurrencyValue == "EUR")
                    vm.AmountRecieved = (double.Parse(vm.EURCurrecnyValue) * double.Parse(AmountSentInForeignCurrecy.Text)).ToString();

            }

            if (!vm.ValidateForm("CurrencySwapPage"))
            {
                MessageDialog.Show("Info", "You have to fill all fields before you can proceed", DialogType.Info, "OK", null, "", null);
                return;
            }

            Navigation.PushAsync(new CurrencySwapConclusion(vm));
        }

        private void AmountSentInForeignCurrecy_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var selectedValue = FromCurrency.SelectedItem;
               // vm.AmountRecieved = 
                if (string.IsNullOrEmpty(selectedValue))
                {
                    //AmtRev.Text = "eg. NGN 0.0";
                    return;
                }

                if (string.IsNullOrEmpty(AmountSentInForeignCurrecy.Text))
                {
                    //AmtRev.Text = "eg. NGN 36100";
                    return;
                }

               
                string AmtyRecieve = string.Empty;
                AmountSentInForeignCurrecy.IsCurrencyVisible = true;
                vm.AmountSent = AmountSentInForeignCurrecy.Text;
                if (selectedValue == "USD")
                {
                    AmountSentInForeignCurrecy.CurrencySymbol = "USD";
                    AmtyRecieve = $" {string.Format("{0:N}", double.Parse(vm.USDCurrencyValue) * double.Parse(AmountSentInForeignCurrecy.Text))}";
                    AmtRev.Text = AmtyRecieve;
                }
               else if (selectedValue == "GBP")
                {
                    AmountSentInForeignCurrecy.CurrencySymbol = "GBP";
                    AmtyRecieve = $" {string.Format("{0:N}", double.Parse(vm.GBPCurrencyValue) * double.Parse(AmountSentInForeignCurrecy.Text))}";
                    AmtRev.Text = AmtyRecieve;
                }
               else if (selectedValue == "EUR")
                {
                    AmountSentInForeignCurrecy.CurrencySymbol = "EUR";
                    AmtyRecieve = $" {string.Format("{0:N}", double.Parse(vm.EURCurrecnyValue) * double.Parse(AmountSentInForeignCurrecy.Text))}";
                    AmtRev.Text = AmtyRecieve;
                }

            }
            catch ( Exception ex)
            {
                ex.Message.ToString();

               // throw;
            }
        }
    }
}