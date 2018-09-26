using Newtonsoft.Json;
using SterlingSwitch.Extensions;
using SterlingSwitch.Helper;
using SterlingSwitch.Models;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.ViewModelBase;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.AllTransactions.ViewModel
{
    public class AllTransactionsViewModel : BaseViewModel
    {
        public AllTransactionsViewModel(INavigation navigation) : base(navigation)
        {
            var currentMonthIndex = DateTime.Now.Month - 1;

            Months = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().Where((x, i) => !string.IsNullOrWhiteSpace(x) && i <= currentMonthIndex).Select(x => x.Substring(0, 3).ToUpper()).ToList();
            // Months.Add("UPCOMING");
            SelectedMonthCommand = new Command<object>(async (x) => await GetTransactionByMonth(x));
            CustomerTransactions = new ObservableRangeCollection<RefinedTransactions>();
            GroupedTransactions = new ObservableRangeCollection<Grouping<string, RefinedTransactions>>();
            RefreshCommand = new Command(async () => await RefreshEvent());
        }

        #region Properties
        public ObservableRangeCollection<RefinedTransactions> CustomerTransactions { get; set; }
        public ObservableRangeCollection<Grouping<string, RefinedTransactions>> GroupedTransactions { get; set; }
        private CustomerAccount _selectedAccount;
        public CustomerAccount SelectedAccount
        {
            get => _selectedAccount;
            set => SetProperty(ref _selectedAccount, value);
        }

        private List<string> _months;
        public List<string> Months
        {
            get => _months;
            set => SetProperty(ref _months, value);
        }

        private bool _isTransactionLoading;
        public bool IsTransactionLoading
        {
            get => _isTransactionLoading;
            set => SetProperty(ref _isTransactionLoading, value);
        }

        private string _transactionMessage;
        public string TransactionMessage
        {
            get => _transactionMessage;
            set => SetProperty(ref _transactionMessage, value);
        }
        private bool _showTransactionErrorMessage;
        public bool ShowTransactionErrorMessage
        {
            get => _showTransactionErrorMessage;
            set => SetProperty(ref _showTransactionErrorMessage, value);
        }

        private string _selectedMonth;
        public string SelectedMonth
        {
            get => _selectedMonth;
            set => SetProperty(ref _selectedMonth, value);
        }

        private string _total;
        public string Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }

        private string _groupBinder;
        public string GroupBinder
        {
            get => _groupBinder;
            set => SetProperty(ref _groupBinder, value);
        }

        private string _groupnameBinder;
        public string GroupNameBinder
        {
            get => _groupnameBinder;
            set => SetProperty(ref _groupnameBinder, value);
        }

        private int monthIndex = 0;

        public int MonthIndex
        {
            get => monthIndex;
            set => SetProperty(ref monthIndex, value);
        }



        #endregion
        #region Commands
        public ICommand SelectedMonthCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        #endregion
        #region Events
        public async Task GetAllTransactions(string nuban, string currencyCode, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                CheckConnectivity();
                if (string.IsNullOrEmpty(nuban))
                {
                    return;
                }
                IsTransactionLoading = true;
                TransactionMessage = string.Empty;
                ShowTransactionErrorMessage = false;
                var transactions = new List<RefinedTransactions>();
                CustomerTransactions.Clear();
                var apirequest = new ApiRequest();

                string url = $"Transaction/GetUserTransactionsByRange?userId={GlobalStaticFields.Customer.Email}&StartDate={StartDate.ToString("MM-dd-yyyy")}&EndDate={EndDate.ToString("MM-dd-yyyy")}";

                var request = await apirequest.GetWithSwitchId(GlobalStaticFields.Customer.Email, "", URLConstants.SwitchApiLiveBaseUrl, url, "BankAccounts");
                if (request.IsSuccessStatusCode)
                {
                    var jsonString = await request.Content.ReadAsStringAsync();
                    jsonString = jsonString.JsonCleanUp();
                    transactions.Clear();
                    var jobj = JsonConvert.DeserializeObject<APIResponse<List<GetStatements>>>(jsonString);
                    if (jobj.Status)
                    {
                        var filt = jobj.Data.Where(c => c.FromAccount == nuban || c.ToAccount == nuban).OrderByDescending(c => c.ID).ToList();
                        var today = DateTime.Now.ToString("d MMMM");
                        foreach (var rec in filt)
                        {
                            GroupNameBinder = rec.TransactionDate.ToString("d MMM yyy");
                            GroupBinder = rec.TransactionDate.ToString();
                            var amt = Convert.ToDouble(rec.Amount).ToString("##, ###.##");

                            var refinedTrans = new RefinedTransactions
                            {
                                CategoryName = rec.CategoryName,
                                BeneficiaryName = rec.BeneficiaryName,
                                Amount = rec.IsBeneficiary == true ? $"{Utilities.GetCurrency(currencyCode)} {amt}" : $"{Utilities.GetCurrency(currencyCode)} {amt}",
                                AmountColor = rec.IsBeneficiary == true ? Color.Green : Color.Red,
                                TransactionDate = rec.TransactionDate.ToString("d MMMM") == today ? "Today" : rec.TransactionDate.ToString("d MMMM"),
                                CategoryID = rec.CategoryID,
                                ReferenceID = rec.ReferenceID,
                                ID = rec.ID,
                                ToAccount = rec.ToAccount,
                                FromAccount = rec.FromAccount,
                                IsCredit = rec.IsCredit
                                
                            };
                            transactions.Add(refinedTrans);
                        }

                        Total = Utilities.GetCurrency(currencyCode) + " " + filt.Sum(x => x.Amount).ToString("##, ###.##");
                        var grouped = (from tr in transactions
                                       group tr by tr.TransactionDate into trGroup
                                       select new Grouping<string, RefinedTransactions>(trGroup.Key, trGroup)).ToList();

                        GroupedTransactions.Clear();
                        foreach (var grp in grouped)
                        {
                            GroupedTransactions.Add(new Grouping<string, RefinedTransactions>(grp.Key, grp.ToList()));
                        }
                        //  GroupedTransactions = new ObservableRangeCollection<Grouping<string, RefinedTransactions>>(grouped);
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
            catch (WebException)
            {

            }
            catch (Exception)
            {


            }
            finally
            {
                IsTransactionLoading = false;
            }


        }

        #endregion

        #region Methods
        private async Task GetTransactionByMonth(object item)
        {
            if ((string)item == "UPCOMING")
                return;

            SelectedMonth = item as string;
            Total = "";
            var selectedMonthIndex = Months.IndexOf(item.ToString());
            MonthIndex = selectedMonthIndex;
            var currentYear = DateTime.Now.Year;
            var daysInMonth = DateTime.DaysInMonth(currentYear, selectedMonthIndex + 1);
            var startDate = new DateTime(currentYear, selectedMonthIndex + 1, 1);
            var endDate = new DateTime(currentYear, selectedMonthIndex + 1, daysInMonth);

            await GetAllTransactions(SelectedAccount.nuban, SelectedAccount.currencyCode, startDate, endDate);

        }

        private async Task RefreshEvent()
        {
           
            Total = "";
            var selectedMonthIndex = MonthIndex;
            var currentYear = DateTime.Now.Year;
            var daysInMonth = DateTime.DaysInMonth(currentYear, selectedMonthIndex + 1);
            var startDate = new DateTime(currentYear, selectedMonthIndex + 1, 1);
            var endDate = new DateTime(currentYear, selectedMonthIndex + 1, daysInMonth);

            await GetAllTransactions(SelectedAccount.nuban, SelectedAccount.currencyCode, startDate, endDate);

        }
        #endregion
    }
}
