using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SterlingSwitch.Extensions;
using SterlingSwitch.Helper;
using SterlingSwitch.Models;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.ViewModelBase;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.WalletAllTransaction.ViewModel
{
  public  class WalletAllTransactionPageViewModel : BaseViewModel
    {
        public WalletAllTransactionPageViewModel(INavigation navigation) : base(navigation)
        {
            var currentMonthIndex = DateTime.Now.Month - 1;

            Months = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().Where((x, i) => !string.IsNullOrWhiteSpace(x) && i <= currentMonthIndex).Select(x => x.Substring(0, 3).ToUpper()).ToList();
            // Months.Add("UPCOMING");
            SelectedMonthCommand = new Command<object>(async (x) => await GetTransactionByMonth(x));
            WalletTransacttions = new ObservableRangeCollection<WalletTransacttion>();
        }

        #region Properties
        public ObservableRangeCollection<WalletTransacttion> WalletTransacttions { get; set; }
        private List<string> _months;
        public List<string> Months
        {
            get { return _months; }
            set { SetProperty(ref _months, value); }
        }

        private bool _isTransactionLoading;
        public bool IsTransactionLoading
        {
            get { return _isTransactionLoading; }
            set { SetProperty(ref _isTransactionLoading, value); }
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

        private string _selectedMonth;
        public string SelectedMonth
        {
            get { return _selectedMonth; }
            set { SetProperty(ref _selectedMonth, value); }
        }

        private string _total;
        public string Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
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

        private string _walletNuban;
        public string WalletNuban
        {
            get { return _walletNuban; }
            set { SetProperty(ref _walletNuban, value); }
        }


        private bool _showWalletErrorMessage;
        public bool ShowWalletErrorMessage
        {
            get { return _showWalletErrorMessage; }
            set { SetProperty(ref _showWalletErrorMessage, value); }
        }
        #endregion
        #region Commands
        public ICommand SelectedMonthCommand { get; set; }
        #endregion
        #region Events

        public async Task GetWalletTransactions(DateTime sDate, DateTime eDate)
        {
            try
            {
                string phone = string.Empty;
                var startDate = sDate.ToString("dd-MMM-yy");
                var endDate = eDate.ToString("dd-MMM-yy");
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
                    enddate =  endDate,
                    nuban =  WalletNuban.ToMSDN(),
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
                        WalletTransacttions.ReplaceRange(walletTransacttionList.ToList());
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
                WalletTransactionErrorMessage = "Connection timed out";
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

        #region Methods
        async Task GetTransactionByMonth(object item)
        {
            if ((string)item == "UPCOMING")
                return;

            SelectedMonth = item as string;
            Total = "";
            var selectedMonthIndex = Months.IndexOf(item.ToString());
            var currentYear = DateTime.Now.Year;
            var daysInMonth = DateTime.DaysInMonth(currentYear, selectedMonthIndex + 1);
            var startDate = new DateTime(currentYear, selectedMonthIndex + 1, 1);
            var endDate = new DateTime(currentYear, selectedMonthIndex + 1, daysInMonth);

            await GetWalletTransactions(startDate, endDate);

        }
        #endregion
    }
}
