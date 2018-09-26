using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SterlingSwitch.Helper;
using SterlingSwitch.Models;
using SterlingSwitch.Pages.Investments;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.ViewModelBase;
using Xamarin.Forms;
using static SterlingSwitch.Services.Abstractions.Entities.AccountAdviceResponse;

namespace SterlingSwitch.Pages.Dashboard.ViewModel
{
    
    public class DashboardViewModel:BaseViewModel
    {
        
        private string _greeting = $"Welcome, {UppercaseFirst(GlobalStaticFields.Customer?.FirstName?.ToLower())}";
        private string _mastergreeting = string.Empty;
        private ObservableRangeCollection<DailyTipsResponseModel.Tip> _tipCollection;
        private List<DailyTipsResponseModel.Tip> _tipTempCollection;
        private ObservableRangeCollection<DealsResponseModel.Deal> _dealsCollection;
        private int _position; 
        private int _tipCount;
        private string _lastMonth;
        private ObservableRangeCollection<string> _overViews;
        private ObservableRangeCollection<Safetospend> _safetospends;
        private ObservableRangeCollection<StringedExpense> _overview;
        private List<DailyTipsResponseModel.Tip> tips;
        private readonly DashboardService.DashboardService _dashboardService;
        private bool _isReloadVisible = false;
        private bool _isSafeToSpendVisible = false;
        private bool _isFrequentlyUsedVisible = false;
        private bool _isOverviewVisible = false;
        public ICommand feedCommand { get; set; }
        public ICommand crossCommand { get; set; }
   
        public ICommand ReloadCommand { get; set; }

        public ICommand NavInvestmentCommand { get; set; }

        public bool IsSafeToSpendVisible
        {
            get => _isSafeToSpendVisible;
            set => SetProperty(ref _isSafeToSpendVisible, value);
            
        }

        public bool IsFrequentlyUsedVisible
        {
            get => _isFrequentlyUsedVisible;
            set => SetProperty(ref _isFrequentlyUsedVisible, value);
        }

        public bool IsOverviewVisible
        {
            get => _isOverviewVisible;
            set => SetProperty(ref _isOverviewVisible, value);
        }
        public ObservableRangeCollection<CustomerAccount> CustomerAccounts { get; set; }

        public ObservableRangeCollection<DailyTipsResponseModel.Tip> TipCollection
        {
            get => _tipCollection;
            set => SetProperty(ref _tipCollection, value);
        }

        public ObservableRangeCollection<StringedExpense> Overview
        {
            get => _overview;
            set => SetProperty(ref _overview, value);
        }
        public ObservableRangeCollection<Safetospend> SafeToSpend
        {
            get => _safetospends;
            set => SetProperty(ref _safetospends, value);
        }
        public ObservableRangeCollection<string> OverViews
        {
            get => _overViews;
            set => SetProperty(ref _overViews, value);
        }


        public ObservableRangeCollection<DealsResponseModel.Deal> DealsCollection
        {
            get => _dealsCollection;
            set => SetProperty(ref _dealsCollection, value);
        }

        public bool IsReloadVisible
        {
            get => _isReloadVisible;
            set => SetProperty(ref _isReloadVisible, value);
            
        }

        public string LastMonth
        {
            get => $"{_lastMonth = DateTime.Now.AddMonths(-1).ToString("MMMM")} overview";
            set => SetProperty(ref _lastMonth, value);
        }
        public string Greeting
        {
            get => _greeting; 
            set => SetProperty(ref _greeting, value);
        }

        public string GreetingonMaster
        {
            get => _mastergreeting;
            set => SetProperty(ref _mastergreeting, value);
        }
        public int TipCount
        {
            get => _tipCount;
            set => SetProperty(ref _tipCount, value);
        }

        public int Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        public DashboardViewModel(INavigation navigation):base(navigation)
        {
            _dashboardService = new DashboardService.DashboardService();
            CustomerAccounts = new ObservableRangeCollection<CustomerAccount>();

            feedCommand = new Command(SetDeals);
            crossCommand = new Command(RemoveCard);
            NavInvestmentCommand = new Command(NavigateToInvestment);
            ReloadCommand = new Command(ReloadTips);
            SetOverviews();
            SetTips();
            SetDeals();

        }

        public async Task GetAccountAdvice()
        {
            try
            {
                var userId = GlobalStaticFields.Customer.Email ?? string.Empty;
                var response = await _dashboardService.GetGustomerAccountAdvice(userId);
                if (response.Status && response.Data != null)
                {
                    Overview = new ObservableRangeCollection<StringedExpense>();
                    SafeToSpend = new ObservableRangeCollection<Safetospend>();
                    var TempOverview = response.Data.Overview.ToList();

                    if (TempOverview.Any())
                    {
                        var overviews = new List<Expense>();
                        var Temp = TempOverview.GroupBy(f => f.Currency).ToList();
                        foreach (var t in Temp)
                        {
                            decimal expense = decimal.Zero;
                            decimal income = decimal.Zero;
                            overviews.Add(new Expense
                            {
                                Currency = t.Key,
                                SumExpense = t.Sum(p => decimal.TryParse(p.TotalExpense, out expense) ? decimal.Parse(p.TotalExpense) : 0),
                                SumIncome = t.Sum(p => decimal.TryParse(p.TotalIncome, out income) ? decimal.Parse(p.TotalIncome) : 0),
                                SumSavings = t.Sum(p => p.TotalSavings < 0 ? 0 : p.TotalSavings)
                            });
                        }
                        var stringedOverview = new List<StringedExpense>();
                        foreach (var view in overviews)
                        {
                            var over = new StringedExpense();
                            over.Currency = view.Currency;
                            over.SumExpense = $"{Utilities.GetCurrency(view.Currency)} {view.SumExpense.ToString("N2")}";
                            over.SumIncome =  $"{Utilities.GetCurrency(view.Currency)} {view.SumExpense.ToString("N2")}";
                            over.SumSavings = $"{Utilities.GetCurrency(view.Currency)} {view.SumSavings.ToString("N2")}";
                            stringedOverview.Add(over);
                        }
                        
                        IsOverviewVisible = true;
                        Overview.AddRange(stringedOverview);

                    }
                    
                    
                    SafeToSpend.AddRange(response.Data.SafeToSpend.ToList());
                    if (SafeToSpend.Any())
                    {
                        foreach (var spent in SafeToSpend)
                        {
                            var bal = spent.CurrentBalance.Replace(",", "");
                            var vbal = double.Parse(bal);
                            var Spend = double.Parse(spent.Spent);
                            var PendingSchedule = double.Parse(spent.PendingSchedule);
                            var noNegative = vbal - (Spend + PendingSchedule) < 0 ? 0 : vbal - (Spend + PendingSchedule);
                            spent.SafeToSpendAmount = $" {Utilities.GetCurrency(spent.Currency)} {noNegative.ToString("N2")}";
                            spent.CurrentBalance = $"of your target of {Utilities.GetCurrency(spent.Currency)} {vbal.ToString("N2")}";
                            spent.PendingSchedule = $"{Utilities.GetCurrency(spent.Currency)} {PendingSchedule.ToString("N2")}";
                            spent.Spent = $"{Utilities.GetCurrency(spent.Currency)} {Spend.ToString("N2")}";
                            spent.percentageToSpend = ((vbal - (Spend + PendingSchedule)) / vbal) * 100;
                        }
                        IsSafeToSpendVisible = true;
                    }
                    
                }
            }
            catch(Exception ed)
            {
               await BusinessLogic.Log(ed.ToString(), "Exception Getting Customer Advice", string.Empty, string.Empty, string.Empty, string.Empty);
            }
          
        }

        private void ReloadTips()
        {
            IsReloadVisible = false;
            tips.AddRange(_tipTempCollection);
            TipCollection.ReplaceRange(_tipTempCollection);
            TipCount = TipCollection.Count;
        }

        void NavigateToInvestment()
        {
            this.Navigation.PushAsync(new LandingPage());
        }

        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        private async void SetTips()
        {
            try
            {
                tips = await _dashboardService.GetApplicationTips().ConfigureAwait(false);
                _tipTempCollection = new List<DailyTipsResponseModel.Tip>();
                _tipTempCollection.AddRange(tips);
                TipCollection = new ObservableRangeCollection<DailyTipsResponseModel.Tip>();
                TipCollection.AddRange(tips);
                TipCount = TipCollection.Count;
            }
            catch (Exception e)
            {
              
            }    
        }

        private  void SetOverviews()
        {
            OverViews = new ObservableRangeCollection<string>();
            OverViews.AddRange(new List<string> { "STRING", "STRING"});
        }

       private async void SetDeals()
       {
           try
           {
               var deals = await _dashboardService.GetApplicationDeals().ConfigureAwait(false);
               DealsCollection = new ObservableRangeCollection<DealsResponseModel.Deal>();
               DealsCollection.AddRange(deals);
            }
           catch (Exception e)
           {
               
               throw;
           }
        }

        private async void RemoveCard()
        {
            try
            {
                tips.RemoveAt(Position);
                TipCollection.ReplaceRange(tips);
                TipCount = TipCollection.Count;
                IsReloadVisible = TipCount == 0 ? true : false;
            }
            catch (Exception e)
            {
               await BusinessLogic.Log(e.ToString(), "Remove Card", string.Empty, string.Empty, string.Empty,
                    string.Empty);
            }   
        }

        public async Task GetCustomerAccountsbyPhoneNumber()
        {

            try
            {
                CheckConnectivity();
                List<CustomerAccount> customerAccounts = GlobalStaticFields.Customer.ListOfAllAccounts;  //await GlobalStaticFields.Customer.GetAccountsbyPhoneNumber(telephone);

                if (customerAccounts?.Count > 0)
                {
                    CustomerAccounts.ReplaceRange(customerAccounts);
                    GlobalStaticFields.CustomersTotalAccount = CustomerAccounts;
                    GlobalStaticFields.Customer.MyCards = await GlobalStaticFields.Customer.GetCards(false);
                }             

            }
            catch (Exception ex)
            {


            }
            finally
            {
                //IsAccountLoaded = true;
                //CardLoading = false;
            }
        }


    }
     
    public class Expense
    {
        public string Currency { get; set; }
        public decimal SumExpense { get; set; }
        public decimal SumIncome { get; set; }
        public float SumSavings { get; set; }
    }

    public class StringedExpense
    {
        public string Currency { get; set; }
        public string SumExpense { get; set; }
        public string SumIncome { get; set; }
        public string SumSavings { get; set; }
    }
}
