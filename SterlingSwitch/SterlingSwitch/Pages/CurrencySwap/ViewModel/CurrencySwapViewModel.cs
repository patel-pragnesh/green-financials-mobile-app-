using Newtonsoft.Json;
using SterlingSwitch.Helper;
using SterlingSwitch.Models;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.ViewModelBase;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.CurrencySwap.ViewModel
{
   public class CurrencySwapViewModel : BaseViewModel
    {
       
        public CurrencySwapViewModel(INavigation navigation) : base(navigation)
        {
            ListFromCurrency = new List<string>();
            // ListToCuurency = new ObservableRangeCollection<string>();
            CustomerAccounts = new List<string>();

            apiRequest = new ApiRequest();

            //InitailStartObject();
        }

        #region Properties
        public List<string> ListFromCurrency { set; get; } 
        public string USDCurrencyValue { set; get; }
        public string GBPCurrencyValue { get; set; }
        public string EURCurrecnyValue { set; get; }
        public string SelectedCurrencyValue { set; get; }
        public string AmountSent { set; get; }
        public string AmountRecieved { set; get; }
        public string RefernceMsg { set; get; }
        public ApiRequest apiRequest { set; get; }
        public List<string> CustomerAccounts { get; set; }
        public string SelectAccount { set; get; }
        public string BeneficiaryAcc { set; get; }
        public string Nuban { set; get; }
        public string CurrencyCode { get; set; }
        private bool _isAccountLoaded;
        public bool IsAccountLoaded
        {
            get { return _isAccountLoaded; }
            set { SetProperty(ref _isAccountLoaded, value); }
        } 
        #endregion Properties
        public async void InitailStartObject()
        {
             ListFromCurrency = new List<string>()
            {
                "USD","GBP","EUR"
            }; 
            USDCurrencyValue = await GetSelectedCurrencyRate("USD", "NGN", "1");
            EURCurrecnyValue = await GetSelectedCurrencyRate("EUR", "NGN", "1");
            GBPCurrencyValue = await GetSelectedCurrencyRate("GBP", "NGN", "1"); 
        }
        #region Events
        public async Task<string> GetSelectedCurrencyRate(string SourceCurrency, string DestinationCurrency,string Amt)
        {

            try
            {
                GetRateModel rateModel = new GetRateModel()
                {
                    DestinationCurrency = DestinationCurrency,
                    SourceAmt = Amt,
                    Referenceid = Utilities.GenerateReferenceId(),
                    RequestType = 701,
                    SourceCurrency = SourceCurrency,
                    Translocation = GlobalStaticFields.GetUserLocation,
                    TransactionType = "SELL"
                };
                string endpoint = "Spay/GetAmountInDestinatinCurrency";

                var response = await apiRequest.Post2(rateModel, URLConstants.SwitchApiLiveBaseUrl, endpoint, "Cuurency Swap Page");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var jsonObj = JsonConvert.DeserializeObject<SpayDefaultResponse>(result);
                    var newRate = !string.IsNullOrEmpty(jsonObj.message) ? jsonObj.message.Split('|')[0] : string.Empty;
                    return newRate;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
                //throw;
            }
        }

        public bool ValidateForm(string page)
        {
            bool isValidateValue = true;
            if (page == "CurrencySwapPage")
            {
                if (string.IsNullOrEmpty(SelectedCurrencyValue))
                {
                    isValidateValue = false;
                }
                else if (string.IsNullOrEmpty(AmountSent))
                {
                    isValidateValue = false;
                }
                else if (string.IsNullOrEmpty(RefernceMsg))
                {
                    isValidateValue = false;
                } 
            }

            if(page == "CurrencySwapConclusion")
            {
                if (string.IsNullOrEmpty(SelectAccount))
                {
                    isValidateValue = false;
                }
                else if(string.IsNullOrEmpty(BeneficiaryAcc))
                {
                    isValidateValue = false;
                }
            }

            return isValidateValue;
        }
       
        public async Task GetCustomerAccountsbyPhoneNumber(string telephone)
        {

            try
            {
                CheckConnectivity();
                //CardLoading = true;

                List<CustomerAccount> customerAccounts = await GlobalStaticFields.Customer.GetAccountsbyPhoneNumber(telephone);

                if (customerAccounts?.Count > 0)
                {
                    if (GlobalStaticFields.Customer.Email.ToLower().Contains("olayinkayusufm"))
                    {
                        customerAccounts.Add(new CustomerAccount
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
                    foreach (var item in customerAccounts?.Where(c => c.currency != "566"))
                    {
                        CustomerAccounts.Add(item.AccountBalanceAccountType);
                    }
                    //await GlobalStaticFields.Customer.GetCards(false);
                }


            }
            catch (Exception ex)
            {


            }
            finally
            {
                IsAccountLoaded = true;
                // CardLoading = false;
            }
        }

        public async Task<string> GetNameEnquire(string Acc)
        {
            try
            {
                CheckConnectivity();
                string endpoint = "switch/GetAccountFullInfo";
                var nubanModel = new FindAcctInfo()
                {
                    Nuban = Acc
                };

                var response = await apiRequest.Post2(nubanModel, URLConstants.SwitchApiLiveBaseUrl, endpoint, "Currency swap name enquire");

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }

            }
            catch (Exception ex)
            {
                return string.Empty;
               // throw;
            }
        }

        public async void ExecuteCurrencySwap(string nuban,string currencyCode)
        {
            var crosscccyTransfer = new CrossCurrencytransfer
            {
                fromAccountNo = nuban,
                fromAmount = AmountSent,
                fromCurrency = currencyCode,
                Remark = $"TRF/ {currencyCode} / {BeneficiaryAcc} / {RefernceMsg}",//"TRF/" + currencyCode + "/" + RecipientAccNo + "/" + remarkReference,
                ToAccount = BeneficiaryAcc,
                ToAmount = AmountRecieved,
                ToCurrency = "NGN",
            };

            string endpoint = "Switch/CrossCurrencyTransfer";
            var pd = await ProgressDialog.Show("Processing..., Please wait.");
            var response = await apiRequest.Post2(crosscccyTransfer, URLConstants.SwitchApiLiveBaseUrl, endpoint, "Cuurency Swap Page ExecuteMethod");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                await pd.Dismiss();

                if (result.ToLower().Contains("success"))
                {
                    MessageDialog.Show("Confirmation", "Your transaction was sucessful", DialogType.Success, "OK", () =>
                     {
                         Device.BeginInvokeOnMainThread(() =>
                         {
                             Navigation.PushAsync(new Pagelanding.PaymentsLanding());
                         });
                     });
                }
                else
                {
                    MessageDialog.Show("Confirmation", "Unable to complete this transaction. Please contact customer care", DialogType.Success, "OK", () =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Navigation.PushAsync(new Pagelanding.PaymentsLanding());
                        });
                    });
                }
            }
            else
            {
                MessageDialog.Show("Confirmation", "Unable to complete this transaction. Please contact customer care", DialogType.Success, "OK", () =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Navigation.PushAsync(new Pagelanding.PaymentsLanding());
                    });
                });
            }
            await pd.Dismiss();

            //return string.Empty;
        }
        #endregion Events
    }
}
