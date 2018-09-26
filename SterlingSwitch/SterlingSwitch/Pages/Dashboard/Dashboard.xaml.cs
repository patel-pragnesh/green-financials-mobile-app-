using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FormsControls.Base;
using SterlingSwitch.Models;
using SterlingSwitch.Pages.BillsPayment;
using SterlingSwitch.Pages.Dashboard.ViewModel;
using SterlingSwitch.Pages.Specta;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Templates;
using Xamanimation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SterlingSwitch.Models.SomeConstant;

namespace SterlingSwitch.Pages.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : SwitchMasterPage, IAnimationPage
    {
        private DashboardViewModel _vm;

        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Long, Subtype = AnimationSubtype.FromLeft, BounceEffect = true };

        public ObservableCollection<PageName> PageNames { get; set; }
        public List<PageName> TopThreePages { get; set; }
        public string PageUserFriendlyName { get; set; }
        public int NavigationCount { get; set; }

        public Dashboard()
        {
            InitializeComponent();
            this.BindingContext = _vm = new DashboardViewModel(Navigation);         
        }

        private void GetFrequentlyUsedFromCurrentPropV2()
        {
            PageNames = new ObservableCollection<PageName>();
            var animation = new Xamanimation.FadeInAnimation();
            
            try
            {
                if (Application.Current.Properties.ContainsKey("PagesVisitedListV2"))
                {


                    var freq = Application.Current.Properties["PagesVisitedListV2"];
                    if (freq != null)
                    {
                        //DeserializeObject into a list first
                        var ee = freq.ToString();
                        var ff = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PageName>>(ee);

                        //find counts of re-occurrence with dictionary
                        //var dictionary = ff.GroupBy(str => str).ToDictionary(group => group.Key, group => group.Count());

                        //find counts of re-occurrence with dictionary
                        var vv = ff.GroupBy(u => u.PageAlias).Select(g => g.ToList()).ToList();

                        #region old
                        //    foreach (var item in dictionary)
                        //{
                        //    //pick each key and value in dictionary and assign into the model called PageName. this will simly show the pages and the number of occurences of that page. this will also be bindable
                        //    var gg = new PageName 
                        //    { 
                        //        Page = item.Key.Page, 
                        //        PageCount = item.Value, 
                        //        PageAlias = item.Key.PageAlias,
                        //         PageIcon = item.Key.PageIcon


                        //    };

                        //    PageNames.Add(gg);
                        //} 
                        #endregion

                        foreach (var item in vv)
                        {

                            //pick each key and value in dictionary and assign into the model called PageName. this will simly show the pages and the number of occurences of that page. this will also be bindable
                            var gg = new PageName
                            {
                                Page = item.FirstOrDefault().Page.ToString(),
                                PageCount = item.Select(i => i.Page).Count(),
                                PageAlias = item[0].PageAlias,
                                PageIcon = item.FirstOrDefault().PageIcon
                            };

                            PageNames.Add(gg);
                        }

                     

                        //get top 3 pages order by count
                        TopThreePages = PageNames.OrderByDescending(q => q.PageCount).Take(3).ToList();
                        
                        FirstFrequentImage.Source = TopThreePages[0]?.PageIcon;
                        PageNameFormatted(TopThreePages[0]?.PageAlias);
                        FirstFrequentLabel.Text = PageUserFriendlyName;
                        var firstCommands = new Command<string>(NavigateToPage);
                        FirstStack.GestureRecognizers.Clear();
                        NavigationCount = 0;//null it here so you increment it in the switch-case to avoid new command<string>navigating thrice
                        FirstStack.GestureRecognizers.Add(new TapGestureRecognizer()
                        {
                            NumberOfTapsRequired = 1,
                            Command = firstCommands,
                            CommandParameter = TopThreePages[0]?.PageAlias
                        });
                        //_vm.IsFrequentlyUsedVisible = true;
                        
                        

                        FrequentlyUsedFrame.Animate(animation);
                        FrequentlyUsedFrame.IsVisible = true;
                        try
                        {
                            SecondFrequentImage.Source = TopThreePages[1]?.PageIcon;
                            PageNameFormatted(TopThreePages[1]?.PageAlias);
                            SecondFrequentLabel.Text = PageUserFriendlyName;
                            var secondCommands = new Command<string>(NavigateToPage);
                            SeconStack.GestureRecognizers.Clear();
                            SeconStack.GestureRecognizers.Add(new TapGestureRecognizer()
                            {
                                NumberOfTapsRequired = 1,
                                Command = secondCommands,
                                CommandParameter = TopThreePages[1]?.PageAlias
                            });
                           // _vm.IsFrequentlyUsedVisible = true;
                            FrequentlyUsedFrame.Animate(animation);
                            FrequentlyUsedFrame.IsVisible = true;
                        }
                        catch (Exception ex)
                        {
                            //mostly errors are due to index out of range. meaning this has no value. hence, hide it
                            SeconStack.IsVisible = false;
                            string log = ex.ToString();
                        }

                        try
                        {
                            ThirdFrequentImage.Source = TopThreePages[2]?.PageIcon;
                            PageNameFormatted(TopThreePages[2]?.PageAlias);
                            ThirdFrequentLabel.Text = PageUserFriendlyName;
                            var thirdCommands = new Command<string>(NavigateToPage);
                            ThirdStack.GestureRecognizers.Clear();
                            ThirdStack.GestureRecognizers.Add(new TapGestureRecognizer()
                            {
                                NumberOfTapsRequired = 1,
                                Command = thirdCommands,
                                CommandParameter = TopThreePages[2]?.PageAlias
                            });
                            //_vm.IsFrequentlyUsedVisible = true;
                            FrequentlyUsedFrame.Animate(animation);
                            FrequentlyUsedFrame.IsVisible = true;
                        }
                        catch (Exception ex)
                        {
                            //mostly errors are due to index out of range. meaning this has no value. hence, hide it
                            ThirdStack.IsVisible = false;
                            string log = ex.ToString();
                        }
                        

                        //Make card visible here
                       
                    }
                }
            }
            catch (Exception ex)
            {
               // MessageDialog.Show("Info", ex.Message, DialogType.Info, "OK", null);
            }
            finally
            {
                
                
            }


        }

        private void GetFrequentlyUsedFromCurrentProp()
        {

            PageNames = new ObservableCollection<PageName>();
            try
            {
                if (Application.Current.Properties.ContainsKey("PagesVisitedList"))
                {
                    //retrieve the persisted value
                    var freq = Application.Current.Properties["PagesVisitedList"];
                    if (freq != null)
                    {
                        //DeserializeObject into a list first
                        var ee = freq.ToString();
                        var ff = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(ee);

                        //find counts of re-occurrence with dictionary
                        var dictionary = ff.GroupBy(str => str)
                                           .ToDictionary(group => group.Key, group => group.Count());

                        foreach (var item in dictionary)
                        {
                            //pick each key and value in dictionary and assign into the model called PageName. this will simly show the pages and the number of occurences of that page. this will also be bindable
                            var gg = new PageName { Page = item.Key, PageCount = item.Value };

                            PageNames.Add(gg);
                        }

                        //get top 3 pages order by count
                        TopThreePages = PageNames.OrderByDescending(q => q.PageCount).Take(3).ToList();

                        FirstFrequentImage.Source = TopThreePages[0]?.PageIcon;
                        FirstFrequentLabel.Text = TopThreePages.Select(u => u.PageAlias)?.FirstOrDefault();
                        var Commands = new Command<string>(NavigateToPage);
                        FirstStack.GestureRecognizers.Add(new TapGestureRecognizer()
                        {
                            NumberOfTapsRequired = 1,
                            Command = Commands,
                            CommandParameter = FirstFrequentLabel.Text
                        });

                        

                    }

                }
            }
            catch (Exception ex)
            {

                string l = ex.ToString();
                //will throw if you dont log out. the key PagesVisitedList has not been registered yet

            }

        }

        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            var rf = e;
            if (rf.ScrollY == 0)
            {
                _vm.GreetingonMaster = string.Empty;
            }
            else
            {
                _vm.GreetingonMaster = _vm.Greeting;
            }
        }

        private void tipCard_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (_vm.feedCommand.CanExecute(this))
                {
                    if (!_vm.DealsCollection.Any())
                    {
                        _vm.feedCommand.Execute(this);
                    }
                    //get position
                    var position = _vm.Position;
                    //now use position to get the object selected from the list tipCount
                    var selectedTips = _vm.TipCollection.ElementAt(position);
                    //extract the name, lower it, delete spaces in between
                    var keyname = selectedTips.Name.Replace(" ", string.Empty).ToLower();
                    //navigate to page based on the name in selected tips
                    NavigateToPage(keyname);


                }
            }
            catch (Exception ex)
            {
               
            }

        }


        private void NavigateToPage(string pagename)
        {
            
            if (NavigationCount > 0) return;
            //for unusual reasons, the new Command<string>(NavigateToPage); fires thrice. hence page navigates thrice.
            //i am setting a count so that it returns after one tap
            switch (pagename)
            {
                case "currencyswap":

                    NavigationCount = 1;// set nav count to 1
                    Navigation.PushAsync(new CurrencySwap.CurrencySwapPage());
                    break;

                case "fxtransfer":
                    NavigationCount = 1;// set nav count to 1

                    MessageDialog.Show("Info", "Page not available yet", DialogType.Info, "OK", null);
                    break;

                case "treasurybills":
                    NavigationCount = 1;// set nav count to 1

                    Navigation.PushAsync(new Pages.Investments.TreasuryBills.TreasuryBillsPage());
                    break;

                case "fixeddeposit":
                    NavigationCount = 1;// set nav count to 1

                    Navigation.PushAsync(new Pages.Investments.FixedDeposit.FixedDepositPage());
                    break;

                case "investment":
                    NavigationCount = 1;// set nav count to 1

                    Navigation.PushAsync(new Investments.LandingPage());
                    break;



                case "compareit":
                    NavigationCount = 1;// set nav count to 1

                    MessageDialog.Show("Info", "Page not available yet", DialogType.Info, "OK", null);
                    break;

                case "switchgive":
                    NavigationCount = 1;// set nav count to 1

                    MessageDialog.Show("Info", "Page not available yet", DialogType.Info, "OK", null);
                    break;

                case "flighttickets":
                    NavigationCount = 1;// set nav count to 1

                    MessageDialog.Show("Info", "Page not available yet", DialogType.Info, "OK", null);
                    break;


                case "safetospend":
                    NavigationCount = 1;// set nav count to 1

                    MessageDialog.Show("Info", "Page not available yet", DialogType.Info, "OK", null);
                    break;


                case "budgettracker":
                    NavigationCount = 1;// set nav count to 1

                    MessageDialog.Show("Info", "Page not available yet", DialogType.Info, "OK", null);
                    break;


                case "cardlesswithdrawal":
                    NavigationCount = 1;// set nav count to 1

                    Navigation.PushAsync(new Pages.CardlessWithdrawals.CardlessWithdrawView());
                    break;

                case PageAliasConstant.SendMoney:
                    NavigationCount = 1;// set nav count to 1

                    Navigation.PushAsync(new Pages.LocalTransfer.SendMoney());
                    break;

                case "whatisaqrcode":
                    NavigationCount = 1;// set nav count to 1

                    MessageDialog.Show("Info", "Page not available yet", DialogType.Info, "OK", null);
                    break;

                case "airtimedata":
                    NavigationCount = 1;// set nav count to 1

                    Navigation.PushAsync(new AirtimeAndData.AirtimeAndData());
                    break;
                case "billspayment":
                    NavigationCount = 1;// set nav count to 1

                    Navigation.PushAsync(new PayBillsPage());
                    break;

                case PageAliasConstant.quickloan:
                    NavigationCount = 1;// set nav count to 1
                    Navigation.PushAsync(new QuickLoan());
                    break;


                default:
                    NavigationCount = 1;// set nav count to 1

                    MessageDialog.Show("Info", "Page not available yet", DialogType.Info, "OK", null);
                    break;
            }
        }

        private string PageNameFormatted(string pagename)
        {
            if(string.IsNullOrEmpty(pagename)) return  string.Empty;

            switch (pagename)
            {
                case PageAliasConstant.currencyswap:
                    PageUserFriendlyName = "Currency Swap";
                    break;

                case PageAliasConstant.InvestmentPage:
                    PageUserFriendlyName = "Investment";
                    break;

                case "fxtransfer":
                    PageUserFriendlyName =  "FX Transfer";
                    break;

                case "treasurybills":
                    PageUserFriendlyName =  "Treasury Bills";
                    break;

                case "fixeddeposit":
                    PageUserFriendlyName =  "Fixed Deposit";
                    break;

                case "compareit":
                    PageUserFriendlyName =  "Compare IT";
                    break;

                case "switchgive":
                    PageUserFriendlyName =  "Switch Give";
                    break;

                case "flighttickets":
                    PageUserFriendlyName =  "Flight Tickets";
                    break;


                case "safetospend":
                    PageUserFriendlyName =  "Safe To Spend";
                    break;


                case "budgettracker":
                    PageUserFriendlyName =  "Budget Tracker";
                    break;


                case "cardlesswithdrawal":
                    PageUserFriendlyName =  "Withdrawals";
                    break;


                case "whatisaqrcode":
                    PageUserFriendlyName =  "Pay With QR";
                    break;

                case "airtimedata":
                    PageUserFriendlyName =  $"Airtime & Data";
                    break;

                case "sendmoney":
                    PageUserFriendlyName =  "Send Money";
                    break;

                case "billspayment":
                    PageUserFriendlyName =  "Bill Payment";
                    break;

                case PageAliasConstant.CardToCard:
                    PageUserFriendlyName = "Card To Card";
                    break;

                case PageAliasConstant.quickloan:
                    PageUserFriendlyName = "Quick Loan";
                    break;


                default:
                    PageUserFriendlyName =  string.Empty;
                    break;
            }

            return string.Empty;
        }



        private void Oncross_Tapped(object sender, EventArgs e)
        {
            if (_vm.crossCommand.CanExecute(this))
            {
                _vm.crossCommand.Execute(this);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // await Task.Run(async () => await _vm.GetAccountAdvice());
            await _vm.GetAccountAdvice();
            GetFrequentlyUsedFromCurrentPropV2();
        }

        private void TapSendMoney_Tapped(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new Pages.LocalTransfer.SendMoney());
            }
            catch (Exception ex)
            {


            }
        }

        public void OnAnimationStarted(bool isPopAnimation)
        {

        }

        public void OnAnimationFinished(bool isPopAnimation)
        {

        }

        private void TapBills_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PayBillsPage());
        }
    }
}