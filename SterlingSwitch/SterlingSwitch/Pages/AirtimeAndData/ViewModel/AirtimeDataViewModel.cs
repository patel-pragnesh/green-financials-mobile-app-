using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SterlingSwitch.Helper;
using SterlingSwitch.Pages.AirtimeAndData.Service;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.ViewModelBase;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.AirtimeAndData.ViewModel
{
    public class AirtimeDataViewModel:BaseViewModel
    {
        private bool _islistofbundlevisible = false;
        private int _selectedCategory = 0;
        private string _selectedCategoryItem;
        private string _selectedProvider = string.Empty;
        private int _selectedBundle = -1;
        private string _enteredAmount = string.Empty;
        private string _enteredPhoneNumber = string.Empty;
        private string _selectedAccount;
        private ObservableRangeCollection<string> _netWorkProviders;
        private ObservableRangeCollection<string> _listedAccounts;
        private ObservableRangeCollection<string> _categories = new ObservableRangeCollection<string>(new List<string>{ "Mobile Top-up", "Data bundle" });
        private AirtimeDataService _service;
        private List<BillerItemResponse.Item> _dataBunbleItems;
        private string _paymentCode;
        public string MobileNumber { set; get; }
        public string PaymentCode
        {
            get => _paymentCode;
            set => SetProperty(ref _paymentCode, value);
        }

        public List<BillerItemResponse.Item> DataBunbleItems
        {
            get => _dataBunbleItems;
            set => SetProperty(ref _dataBunbleItems, value);
        }

        public List<BillerPostResponseModel.Biller> DataProviders { get; set; }
        public ICommand ContinueCommand { get; set; }

        public ICommand BackCommand { get; set; }

        public string SelectedCategoryItem
        {
            get => _selectedCategoryItem;
            set => SetProperty(ref _selectedCategoryItem, value);
        }


        public ObservableRangeCollection<string> Categories 
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public ObservableRangeCollection<string> ListedAccounts
        {
            get => _listedAccounts ;
            set => SetProperty(ref _listedAccounts, value);
        }

        public ObservableRangeCollection<string> NetWorkProviders
        {
            get => _netWorkProviders;
            set => SetProperty(ref _netWorkProviders, value);
        }

        public string SelectedAccount
        {
            get => _selectedAccount;
            set => SetProperty(ref _selectedAccount, value);
        }
        private string _balance = String.Empty;
        public string Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value);
        }
        public string EnteredPhoneNumber
        {
            get => _enteredPhoneNumber;
            set => SetProperty(ref _enteredPhoneNumber, value);
        }
        public string EnteredAmount
        {
            get => _enteredAmount;
            set => SetProperty(ref _enteredAmount, value);
        }

        public int SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        public int SelectedBundle
        {
            get => _selectedBundle;
            set => SetProperty(ref _selectedBundle, value);
        }
        public string SelectedProvider
        {
          get => _selectedProvider;
          set => SetProperty(ref _selectedProvider, value);
        }
        public bool IsListofBundleVisible
        {
            get => _islistofbundlevisible;
            set => SetProperty(ref _islistofbundlevisible, value);
        }
        public AirtimeDataViewModel(INavigation navigation) : base(navigation)
        {
            _service = new AirtimeDataService();
            ContinueCommand = new Command(ContinueClicked);
            BackCommand = new Command(BackCommandClicked);
            Task.Run(async () => await GetDataProviders());

        }

       async  void ContinueClicked()
        {
           
            if (string.IsNullOrEmpty(EnteredPhoneNumber))
            {
                EnteredPhoneNumber = EnteredPhoneNumber.Replace(" ", "");
                MessageDialog.Show("Missing Field","Please enter the Beneficiary Mobile Number ", DialogType.Info, "OK",
                    null, string.Empty, null);
                return;
            }

            try
            {
                var formattedAmount = double.Parse(EnteredAmount.Replace($"{Utilities.GetCurrency("NGN")} ", ""));
                MessageDialog.Show("Transaction Confirmation", $"You are about to buy {SelectedCategoryItem} worth \n {Utilities.GetCurrency("NGN")}{formattedAmount:##,###}", DialogType.Question, "Proceed",
                 async () => Device.BeginInvokeOnMainThread(async () => await BuyService()), "Cancel", null);
            }
            catch (Exception e)
            {
                await BusinessLogic.Log(e.Message, "On Page", string.Empty, "Continue Clicked.", string.Empty, "AirtimeData Page");
            }
            
        }

        async Task BuyService()
        {
            //var mobileNumber = string.Empty;
            if (EnteredPhoneNumber[0] == '0' && EnteredPhoneNumber.Length == 11)
            {
                MobileNumber = "234" + EnteredPhoneNumber.Substring(1);
            }else if (EnteredPhoneNumber.Contains("+234") && EnteredPhoneNumber.Length > 11)
            {
                MobileNumber =  EnteredPhoneNumber.Substring(1);
            }else{
                //invalid MSDN
                MessageDialog.Show("Invalid Phonenumber", $"Please ensure the number starts with +234.", DialogType.Error, "Ok", null);
                return;
            }
            try
            {
                if(GlobalStaticFields.Customer.IsTPin == true)
                {

                    var ValidateTransactionwithTPIN = new PopUps.VerifyPinPage("Confirmation", EnteredAmount, null);
                    ValidateTransactionwithTPIN.Validated += ValidateTransactionwithTPIN_Validated;
                    await Navigation.PushAsync(ValidateTransactionwithTPIN);
                }
                else
                {
                    DoAirttimeAndData(MobileNumber);
                }
               
            }
            catch (Exception e)
            {
                await BusinessLogic.Log(e.Message, "On Page", string.Empty, "Transaction", string.Empty, "AirtimeData Page");
            }
            
            
        }

        public async void DoAirttimeAndData(string mobileNumber)
        {
            if (SelectedCategory == 0)
            {
                //Buy Airtime
                var airtimeRequest = new AirtimeRequest()
                {
                    Amount = EnteredAmount.Replace(",", ""),
                    Beneficiary = mobileNumber,
                    Mobile = mobileNumber,
                    NUBAN = SelectedAccount,
                    NetworkID = SelectedProvider,
                    RequestType = "932",
                    ReferenceID = Utilities.GenerateReferenceId(),
                    Type = string.Empty
                };
                var pd = await ProgressDialog.Show("Buying Airtime, please wait.");
                var response = await _service.BuyAirtime(airtimeRequest);
                if (response.Contains("<ResponseCode>00</ResponseCode>"))
                {
                    await pd.Dismiss("All Done");
                    MessageDialog.Show("Transaction Status", $"Airtime aquisition was successfull", DialogType.Success, "Ok", null);
                    await BusinessLogic.Log(response.ToString(), "Airtime Transaction", string.Empty, "Transaction", string.Empty, "AirtimeData Page");
                }
                else
                {
                    await pd.Dismiss("All Done");
                    MessageDialog.Show("Transaction Status", $"Airtime aquisition was not successfull", DialogType.Error, "Ok", null);
                    await BusinessLogic.Log(response.ToString(), "Airtime Transaction", string.Empty, "Transaction", string.Empty, "AirtimeData Page");
                }
            }
            else if (SelectedCategory == 1)
            {
                //Buy Data Bundle
                var databundleRequest = new BuyDataRequestModel
                {
                    RequestType = "108",
                    ActionType = string.Empty,
                    Referenceid = Utilities.GenerateReferenceId(),
                    SubscriberInfo1 = mobileNumber,
                    Translocation = string.Empty,
                    amt = EnteredAmount,
                    email = GlobalStaticFields.Customer?.Email,
                    mobile = mobileNumber,
                    nuban = SelectedAccount,
                    paymentcode = PaymentCode //ItemPayment Code
                };
                var pd = await ProgressDialog.Show("Suscribing to Data Bundle, please wait.");
                var response = await _service.BuyDatabundle(databundleRequest);
                if (response.response == "00")
                {
                    await pd.Dismiss("All Done");
                    MessageDialog.Show("Transaction Status", $"Data bundle Subscription was successfull", DialogType.Success, "Ok", null);

                }
                else
                {
                    await pd.Dismiss("All Done");
                    MessageDialog.Show("Transaction Status", $"{response.message}", DialogType.Error, "Ok", null);
                }

            }
        }

        private void ValidateTransactionwithTPIN_Validated(object sender, bool e)
        {
            DoAirttimeAndData(MobileNumber);
        }

        void BackCommandClicked()
        {
            Navigation.PopAsync(true);
        }

        public async Task<List<NetworkProviderModel.Provider>> SetServiceProviders()
        {
            try
            {
                var list = await _service.GetAvailableProviders();
                return list;
            }
            catch (Exception e)
            {
               
            }
           return new List<NetworkProviderModel.Provider>();
        }

        public async Task GetDataProviders()
        {
            try
            {
                DataProviders = await _service.GetDateBundleProviders();
            }
            catch (Exception e)
            {
            }
            
        }

        public async Task<List<BillerItemResponse.Item>> GetDataItems(string ItemId)
        {
            return DataBunbleItems = await  _service.GetBillerItem(ItemId);
        }
    }
}
