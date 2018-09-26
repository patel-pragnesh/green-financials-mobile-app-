using Newtonsoft.Json;
using SterlingSwitch.Helper;
using SterlingSwitch.Models;
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

namespace SterlingSwitch.Pages.CurrencySwap
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CurrencySwapConclusion : SwitchMasterPage
	{
        CurrencySwapViewModel vm;
        public string Phone = GlobalStaticFields.Customer.PhoneNumber;
        public CurrencySwapConclusion (CurrencySwapViewModel currencySwapViewModel)
		{
            try
            {
                InitializeComponent();
                vm = currencySwapViewModel;
                BindingContext = vm;

                SelectedAccount.ItemsSource = vm.CustomerAccounts;
            }
            catch (Exception ex)
            {

               // throw;
            }
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (vm == null)
                return;
            var Recieve = decimal.Parse(vm.AmountRecieved);
            var Sent = decimal.Parse(vm.AmountSent);
            
            RecipentGet.RightText = $"{Utilities.GetCurrency("NGN")}{string.Format("{0:N}",Recieve)}";
            
            if(vm.SelectedCurrencyValue == "USD")
            {
                ExchangeRate.RightText = $"{Utilities.GetCurrency("USD")}{ vm.USDCurrencyValue}";
                AmountSent.RightText = $"{Utilities.GetCurrency("USD")}{string.Format("{0:N}",Sent)}";
            }

            if (vm.SelectedCurrencyValue == "GBP")
            {
                ExchangeRate.RightText = $"{Utilities.GetCurrency("GBP")}{vm.GBPCurrencyValue}";
                AmountSent.RightText = $"{Utilities.GetCurrency("GBP")}{string.Format("{0:N}", Sent)}";
            }
            if (vm.SelectedCurrencyValue == "EUR")
            {
                ExchangeRate.RightText = $"{Utilities.GetCurrency("EUR")}{ vm.EURCurrecnyValue}";
                AmountSent.RightText = $"{Utilities.GetCurrency("EUR")}{string.Format("{0:N}", Sent)}";
            }

            if (!vm.IsAccountLoaded)
            {
                
                if (GlobalStaticFields.Customer.ListOfAllAccounts != null && GlobalStaticFields.Customer.ListOfAllAccounts.Count > 0)
                {
                    if (GlobalStaticFields.Customer.Email.ToLower().Contains("olayinkayusufm"))
                    {
                        GlobalStaticFields.Customer.ListOfAllAccounts.Add(new CustomerAccount
                        {
                            nuban = "0064828616",
                            CustomerId = "124358",
                            currency = "405",
                            currencyCode = "USD",
                            balance = "1500",
                            accountType = "domiciliary",
                        });
                    }
                    List<CustomerAccount> acc = new List<CustomerAccount>();
                    foreach (var item in GlobalStaticFields.Customer.ListOfAllAccounts)
                    {
                        if(item.currency != "566")
                        {

                            vm.CustomerAccounts.Add(item.AccountBalanceAccountType);
                        }
                    }

                    vm.IsAccountLoaded = true;
                }
                else
                {
                    await vm.GetCustomerAccountsbyPhoneNumber(Phone);
                }
            }

        }

        private void btnDismiss_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void btnConfirm_Clicked(object sender, EventArgs e)
        {
            try
            {
                vm.SelectAccount = SelectedAccount.SelectedItem;
                if (!vm.ValidateForm("CurrencySwapConclusion"))
                {
                    MessageDialog.Show("Info", "You have to fill all fields before you can proceed", DialogType.Info, "OK", null, "", null);
                    return;
                }

                vm.CurrencyCode = GlobalStaticFields.Customer.ListOfAllAccounts.FirstOrDefault(a => a.AccountBalanceAccountType == vm.SelectAccount).currencyCode;
                
                if(vm.SelectedCurrencyValue != vm.CurrencyCode)
                {
                    MessageDialog.Show("Error", "The Source Account currency and the Currency you selected do not match", DialogType.Error, "OK", null, "", null);
                    return;
                }

                string newRate = string.Empty;
                if(vm.SelectedCurrencyValue == "USD")
                {
                    newRate = vm.USDCurrencyValue;
                }
                if(vm.SelectedCurrencyValue == "GBP")
                {
                    newRate = vm.GBPCurrencyValue;
                }
                if(vm.SelectedCurrencyValue == "EUR")
                {
                    newRate = vm.EURCurrecnyValue;
                }

              vm.Nuban = GlobalStaticFields.Customer.ListOfAllAccounts.FirstOrDefault(a => a.AccountBalanceAccountType == vm.SelectAccount).nuban;
                string responseMsg = $"Source Account: {vm.Nuban} \n Destination Account:{vm.BeneficiaryAcc} \n Rate:{newRate} \n Amount: {string.Format("{0:N1}", vm.AmountRecieved)}";
                MessageDialog.Show("Confirmation", responseMsg, DialogType.Info, "Yes",
                    () =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (GlobalStaticFields.Customer.IsTPin)
                            {
                                var tpin = new PopUps.VerifyPinPage("Confirmation", vm.AmountRecieved, "NGN", responseMsg);
                                tpin.Validated += Tpin_Validated;
                                Navigation.PushAsync(tpin);
                            }
                            else
                            {
                                vm.ExecuteCurrencySwap(vm.Nuban, vm.CurrencyCode);
                            }
                            
                        });
                    },"No",null
                    );

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
               // throw;
            }
        }

        private void Tpin_Validated(object sender, bool e)
        {
            vm.ExecuteCurrencySwap(vm.Nuban, vm.CurrencyCode);
        }

        private async void DoCurrencySwap()
        {

        }

        private async void ExtendedEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var Nuban = BeneficiaryAccount.Text;
                if (Nuban.Length == 10)
                {
                    string Accno = Nuban.Trim();
                    var pd = await ProgressDialog.Show("Beneficiaries, Please wait.");
                    string response = await vm.GetNameEnquire(Accno);
                    await pd.Dismiss();
                    if (!string.IsNullOrEmpty(response))
                    {
                        if(response.Contains("nothing"))
                        {
                            MessageDialog.Show("Error", "Account number does not exist", DialogType.Error, "OK", null, "", null);
                            return;
                        }
                        var result = JsonConvert.DeserializeObject<CustomerInfoFromT24>(response);

                        if (!string.IsNullOrEmpty(result.customerid))
                        {
                            if(result.currencycode == "NGN")
                            {
                                xBeneficaryNameEnquire.IsVisible = true;
                                //await pd.Dismiss();
                                xBeneficaryNameEnquire.ContentText = $"{result.AccountName.ToUpper()}-{result.nuban}";
                                vm.BeneficiaryAcc = result.nuban;
                            }
                            else
                            {
                                //await pd.Dismiss();
                                MessageDialog.Show("Error", "Can not transfer to foreign Account", DialogType.Error, "OK", null, "", null);
                                return;
                            }
                        }
                        else
                        {
                            //await pd.Dismiss();
                            MessageDialog.Show("Error", "Account number does not exist", DialogType.Error, "OK", null, "", null);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

               // throw;
            }
        }
    }
}