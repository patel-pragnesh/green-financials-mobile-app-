using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SterlingSwitch.Helper;
using SterlingSwitch.Models;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.ViewModelBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using switch_mobile.Services.Abstractions.Entities;
using SterlingSwitch.Extensions;
using SterlingSwitch.Services;
using System.Windows.Input;
using SterlingSwitch.Pages.AllTransactions;
using SterlingSwitch.Pages.AllTransactions.ViewModel;
using SterlingSwitch.Services.Abstractions.Entities;
using System.Linq;
using SterlingSwitch.Pages.WalletAllTransaction.ViewModel;
using SterlingSwitch.Pages.WalletAllTransaction;
using System.Net;

namespace SterlingSwitch.Pages.BankAccounts
{
    public class BankAccountsViewModel : BaseViewModel
    {
        public BankAccountsViewModel(INavigation navigation) : base(navigation)
        {
            CustomerAccounts = new ObservableRangeCollection<CustomerAccount>();
            CustomerAccounts.CollectionChanged += CustomerAccounts_CollectionChanged;


            CustomerCards = new ObservableRangeCollection<MyCards>();
            CustomerTransactions = new ObservableRangeCollection<RefinedTransactions>();
            WalletDetails = new WalletDetails();
            WalletTransacttions = new ObservableRangeCollection<WalletTransacttion>();
            AllTransactionsCommand = new Command(() =>
            {
                var vm = new AllTransactionsViewModel(Navigation);
                vm.SelectedAccount = SelectedAccount;
                var page = new AllTransactionsView();
                page.BindingContext = vm;

                Navigation.PushAsync(page);
            });
            AllWalletTransactionsCommand = new Command(() =>
            {
                var vm = new WalletAllTransactionPageViewModel(Navigation);
                vm.WalletNuban = WalletNuban;
                var page = new WalletAllTransactionPage();
                page.BindingContext = vm;

                Navigation.PushAsync(page);
            });


            SelectedAccount = new CustomerAccount();
            ReloadWallet = new Command(async () => await GetCustomerWalletbyPhoneNumber(GlobalStaticFields.Customer.PhoneNumber));
            ReloadAccounts = new Command(async () => await GetCustomerAccountsbyPhoneNumber(GlobalStaticFields.Customer.PhoneNumber));
        }

        private void CustomerAccounts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }

        #region Properties

        public ObservableRangeCollection<RefinedTransactions> CustomerTransactions { get; set; }
        public ObservableRangeCollection<CustomerAccount> CustomerAccounts { get; set; }
        public ObservableRangeCollection<MyCards> CustomerCards { get; set; }
        public ObservableRangeCollection<WalletTransacttion> WalletTransacttions { get; set; }


        private CustomerAccount _selectedAccount;
        public CustomerAccount SelectedAccount
        {
            get { return _selectedAccount; }
            set { SetProperty(ref _selectedAccount, value); }
        }

        private bool _isAccountLoaded;
        public bool IsAccountLoaded
        {
            get { return _isAccountLoaded; }
            set { SetProperty(ref _isAccountLoaded, value); }
        }

        private bool _cardLoading;
        public bool CardLoading
        {
            get { return _cardLoading; }
            set { SetProperty(ref _cardLoading, value); }
        }


        private bool _showReloadCustomerAccounts = false;

        public bool ShowReloadCustomerAccounts
        {
            get { return _showReloadCustomerAccounts; }
            set { SetProperty(ref _showReloadCustomerAccounts, value); }
        }


        /// <summary>
        /// This is displayed when an error occurred while loading Customer Accounts
        /// </summary>
        private string _customerAccountsErrorMsg = default(string);

        public string CustomerAccountsErrorMsg
        {
            get { return _customerAccountsErrorMsg; }
            set { SetProperty(ref _customerAccountsErrorMsg, value); }
        }





        private bool _istransactionLoading;
        public bool IsTransactionLoading
        {
            get { return _istransactionLoading; }
            set { SetProperty(ref _istransactionLoading, value); }
        }

        private string _transactionMessage;
        public string TransactionMessage
        {
            get { return _transactionMessage; }
            set { SetProperty(ref _transactionMessage, value); }
        }
        private bool _showTransactionErrorMessage;
        public bool ShowTransactionErrorMessage
        {
            get { return _showTransactionErrorMessage; }
            set { SetProperty(ref _showTransactionErrorMessage, value); }
        }

        private bool _canSwipeLeft;
        public bool CanSwipeLeft
        {
            get { return _canSwipeLeft; }
            set { SetProperty(ref _canSwipeLeft, value); }
        }
        private bool _canSwipeRight;
        public bool CanSwipeRight
        {
            get { return _canSwipeRight; }
            set { SetProperty(ref _canSwipeRight, value); }
        }

        private WalletDetails _walletDetails;

        public WalletDetails WalletDetails
        {
            get { return _walletDetails; }
            set { SetProperty(ref _walletDetails, value); }
        }

        #region Wallet
        private bool _walletLoading;
        public bool WalletLoading
        {
            get { return _walletLoading; }
            set { SetProperty(ref _walletLoading, value); }
        }

        private bool _hasWallet;
        public bool HasWallet
        {
            get { return _hasWallet; }
            set { SetProperty(ref _hasWallet, value); }
        }
        private bool _showWalletErrorMessage;
        public bool ShowWalletErrorMessage
        {
            get { return _showWalletErrorMessage; }
            set { SetProperty(ref _showWalletErrorMessage, value); }
        }


        private string _walletAccountBalance;
        public string WalletAccountBalance
        {
            get { return _walletAccountBalance; }
            set { SetProperty(ref _walletAccountBalance, value); }
        }
        private string _walletNuban;
        public string WalletNuban
        {
            get { return _walletNuban; }
            set { SetProperty(ref _walletNuban, value); }
        }
        private string _walletCurrencyCode;
        public string WalletCurrencyCode
        {
            get { return _walletCurrencyCode; }
            set { SetProperty(ref _walletCurrencyCode, value); }
        }

        private string _walletAvailableBalance;
        public string WalletAvailableBalance
        {
            get { return _walletAvailableBalance; }
            set { SetProperty(ref _walletAvailableBalance, value); }
        }

        private string _walletBalance;
        public string WalletBalance
        {
            get { return _walletBalance; }
            set { SetProperty(ref _walletBalance, value); }
        }
        private string _walletBookBalance;
        public string WalletBookBalance
        {
            get { return _walletBookBalance; }
            set { SetProperty(ref _walletBookBalance, value); }
        }

        private string _walletErrorMessage;

        public string WalletErrorMessage
        {
            get { return _walletErrorMessage; }
            set { SetProperty(ref _walletErrorMessage, value); }
        }


        private string _walletTransactionErrorMessage = default(string);

        public string WalletTransactionErrorMessage
        {
            get { return _walletTransactionErrorMessage; }
            set { SetProperty(ref _walletTransactionErrorMessage, value); }
        }

        private bool _showWalletTransactionError = default(bool);

        public bool ShowWalletTransactionError
        {
            get { return _showWalletTransactionError; }
            set { SetProperty(ref _showWalletTransactionError, value); }
        }


        private bool _isWalletTransactionLoading = default(bool);

        public bool IsWalletTransactionLoading
        {
            get { return _isWalletTransactionLoading; }
            set { SetProperty(ref _isWalletTransactionLoading, value); }
        }






        #endregion
        #endregion

        public ICommand AllTransactionsCommand { get; set; }
        public ICommand AllWalletTransactionsCommand { get; set; }
        public ICommand ReloadAccounts { get; set; }
        public ICommand ReloadWallet { get; set; }
        public List<RefinedTransactions> transactions = new List<RefinedTransactions>();
        #region Events      
        public async Task GetCustomerAccountsbyPhoneNumber(string telephone)
        {

            try
            {
                CheckConnectivity();
                ShowReloadCustomerAccounts = false;
                CustomerAccountsErrorMsg = "";
                CardLoading = true;

                List<CustomerAccount> customerAccounts = await GlobalStaticFields.Customer.GetAccountsbyPhoneNumber(telephone);

                if (customerAccounts?.Count > 0)
                {

                    CustomerAccounts.ReplaceRange(customerAccounts);
                    //await GlobalStaticFields.Customer.GetCards(false);
                }
            }
            catch (WebException e)
            {
                ShowReloadCustomerAccounts = true;
                CustomerAccountsErrorMsg = "connection timed out";
            }
            catch (Exception ex)
            {

                ShowReloadCustomerAccounts = true;
                CustomerAccountsErrorMsg = "An error occurred";
            }
            finally
            {
                IsAccountLoaded = true;
                CardLoading = false;
            }
        }

        
        public async Task GetTransactionsByNuban(string nuban, string currencyCode)
        {
            try
            {
                var selectedMonthIndex = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;
                var daysInMonth = DateTime.DaysInMonth(currentYear, selectedMonthIndex);
                var startDate = new DateTime(currentYear, selectedMonthIndex, 1);
                var endDate = new DateTime(currentYear, selectedMonthIndex, daysInMonth);

                CheckConnectivity();
                if (string.IsNullOrEmpty(nuban))
                {
                    return;
                }
                IsTransactionLoading = true;
                TransactionMessage = string.Empty;
                ShowTransactionErrorMessage = false;
                CustomerTransactions.Clear();
                var apirequest = new ApiRequest();

         
                string url = $"Transaction/GetUserTransactionsByRange?userId={GlobalStaticFields.Customer.Email}&StartDate={startDate.ToString("MM-dd-yyyy")}&EndDate={endDate.ToString("MM-dd-yyyy")}";
                var request = await apirequest.GetWithSwitchId(GlobalStaticFields.Customer.Email, "", URLConstants.SwitchApiLiveBaseUrl, url, "BankAccounts");
                if (request.IsSuccessStatusCode)
                {
                    var jsonString = await request.Content.ReadAsStringAsync();
                    jsonString = jsonString.JsonCleanUp();
                    transactions.Clear();
                    var jobj = JsonConvert.DeserializeObject<APIResponse<List<GetStatements>>>(jsonString);
                    if (jobj.Status)
                    {
                        var filt = jobj.Data.Where(c => c.FromAccount == nuban || c.ToAccount == nuban).OrderByDescending(c=>c.ID).ToList().Take(3);
                      
                        foreach (var rec in filt)
                        {
                            var amt = Convert.ToDouble(rec.Amount).ToString("##, ###.##");
                            var refinedTrans = new RefinedTransactions
                            {
                                CategoryName = rec.CategoryName,
                                BeneficiaryName = rec.BeneficiaryName,
                                Amount = rec.IsBeneficiary == true ? $"{Utilities.GetCurrency(currencyCode)} {amt}" : $"{Utilities.GetCurrency(currencyCode)} {amt}",
                                AmountColor = rec.IsBeneficiary == true ? Color.Green : Color.Red,
                                ReferenceID = rec.ReferenceID,
                                TransactionDate = rec.TransactionDate.ToString("d MMM"),
                                PaymentReference = rec.PaymentReference
                            };
                            transactions.Add(refinedTrans);
                        }

                        CustomerTransactions.ReplaceRange(transactions);
                        if (CustomerTransactions.Count > 0)
                            TransactionMessage = "";

                    }
                    else
                    {
                        ShowTransactionErrorMessage = true;
                        TransactionMessage = "No Transaction Found";
                    }
                }
                else
                {
                    var content = await request.Content.ReadAsStringAsync();
                }
            }
            catch (WebException e)
            {

            }
            catch (Exception ex)
            {


            }
            finally
            {
                IsTransactionLoading = false;
            }
        }

        public async Task GetCustomerWalletbyPhoneNumber(string telephone)
        {

            try
            {
                CheckConnectivity();
                WalletLoading = true;
                HasWallet = false;
                ShowWalletErrorMessage = false;
                WalletErrorMessage = string.Empty;
                var apirequest = new ApiRequest();

                var walletrequest = new WalletDetailsRequest
                {
                    Referenceid = Utilities.GenerateReferenceId(),
                    nuban = telephone,
                    RequestType = 116,
                    Translocation = GlobalStaticFields.GetUserLocation
                };

                var request = await apirequest.Post<WalletDetailsRequest>(walletrequest, "", URLConstants.SwitchApiLiveBaseUrl, "Spay/SBPMWalletDetReq", "BankAccountsViewModel");

                if (request.IsSuccessStatusCode)
                {
                    var response = await request.Content.ReadAsStringAsync();
                    var content = JsonConvert.DeserializeObject<WalletDetails>(response);
                    if (string.Equals(content.message, "No content", StringComparison.OrdinalIgnoreCase))
                    {
                        ShowWalletErrorMessage = true;
                        WalletErrorMessage = content.message;
                        HasWallet = false;
                    }
                    else
                    {
                        HasWallet = true;
                        WalletAccountBalance = content.data.AccountBalance.ToString();
                        WalletCurrencyCode = content.data.currencycode;
                        WalletAvailableBalance = content.data.availablebalance.ToString();
                        WalletNuban = content.data.nuban;

                        WalletBalance = $"{Utilities.GetCurrency(content.data.currencycode)} {content.data.AccountBalance.ToString()}";
                        WalletBookBalance = $"Book balance {Utilities.GetCurrency(content.data.currencycode)} {content.data.availablebalance.ToString()}";
                    }

                }
                else
                {
                    var response = await request.Content.ReadAsStringAsync();
                }

            }
            catch (Exception ex)
            {
                HasWallet = false;
                ShowWalletErrorMessage = true;
                WalletErrorMessage = "An Error Occurred";

            }
            finally
            {
                WalletLoading = false;
            }
        }

        public async Task GetWalletTransactions()
        {
            try
            {
                string phone = string.Empty;
                var startDate = DateTime.UtcNow.AddDays(-10).ToString("dd-MMM-yy");
                var endDate = DateTime.UtcNow.ToString("dd-MMM-yy");
                WalletTransactionErrorMessage = string.Empty;
                ShowWalletErrorMessage = false;
                IsWalletTransactionLoading = true;

                if (WalletNuban.Contains("+234"))
                {
                    phone = $"0{ WalletNuban.Substring(4)}";
                }
                else
                {
                    phone = WalletNuban;
                }

                WalletStatementRequest walletrequest = new WalletStatementRequest
                {
                    startdate = startDate,
                    enddate = endDate,
                    nuban = WalletNuban.ToMSDN(),
                    RequestType = 118,
                    Referenceid = Utilities.GenerateReferenceId(),
                    Translocation = GlobalStaticFields.GetUserLocation
                };
                var apirequest = new ApiRequest();
                var request = await apirequest.Post<WalletStatementRequest>(walletrequest, "", URLConstants.SwitchApiLiveBaseUrl, "Spay/SBPMWalletStatmentReq", "BankAccountsViewModel");

                if (request.IsSuccessStatusCode)
                {
                    var walletTransacttionList = new List<WalletTransacttion>();
                    var response = await request.Content.ReadAsStringAsync();
                    var jobj = JObject.Parse(response);
                    var responseCode = jobj.Value<string>("response");
                    if (responseCode == "00")
                    {
                        var responsedata = JArray.Parse(jobj["responsedata"].ToString());
                        for (int i = 0; i < responsedata.Count; i++)
                        {
                            var dataToken = responsedata[i];
                            var walletTransaction = new WalletTransacttion
                            {
                                AmountFormatted = $"{Utilities.GetCurrency(dataToken.Value<string>("currencycode"))} {dataToken.Value<string>("amt")}",
                                amt = dataToken.Value<string>("amt"),
                                Balance = dataToken.Value<string>("Balance"),
                                currencycode = dataToken.Value<string>("currencycode"),
                                deb_cre_indaa1 = dataToken.Value<string>("deb_cre_indaa1"),
                                remarks = dataToken.Value<string>("remarks"),
                                TRA_DATE = dataToken.Value<string>("TRA_DATE"),
                                val_date = dataToken.Value<string>("val_date"),
                                TransactionType = dataToken.Value<string>("deb_cre_indaa1") == "1" ? "Debit" : "Credit",
                            };

                            walletTransacttionList.Add(walletTransaction);
                        }
                        WalletTransacttions.ReplaceRange(walletTransacttionList.Take(10).ToList());
                    }
                    else
                    {
                        WalletTransactionErrorMessage = "Unable to load Wallet Transactions";
                        ShowWalletErrorMessage = true;
                    }
                }
                else
                {
                    var response = await request.Content.ReadAsStringAsync();
                }
            }
            catch (WebException e)
            {
                WalletTransactionErrorMessage = "Connection time out";
                ShowWalletErrorMessage = true;
            }
            catch (Exception ex)
            {
                WalletTransactionErrorMessage = "An Error Occurred";
                ShowWalletErrorMessage = true;
            }
            finally
            {
                IsWalletTransactionLoading = false;
            }
        }

        #endregion
    }
}
