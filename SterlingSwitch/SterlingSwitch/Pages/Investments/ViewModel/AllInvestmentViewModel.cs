using Newtonsoft.Json;
using SterlingSwitch.Helper;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.ViewModelBase;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.Investments.ViewModel
{
    public class AllInvestmentViewModel : BaseViewModel
    {
        private ObservableRangeCollection<FixedDepositValue> _runningFixedDeposit;
        private ObservableRangeCollection<ParthianInnerArrayOfBillDetails> _runningTreasuryBills;
        ApiRequest apirequest = new ApiRequest();
        private string totalInvestments = "";
        private string allFixedDepositInvestments = string.Empty;
        private string allTbillsDepositInvestments = string.Empty;
        private double totalTbills { get; set; }
        private double totalFixedDeposit { get; set; }
        private string allgoals = $"Goal: 0";

        public ObservableRangeCollection<FixedDepositValue> RunningFixedDeposit
        {
            get => _runningFixedDeposit;
            set => SetProperty(ref _runningFixedDeposit, value);
        }

        public ObservableRangeCollection<ParthianInnerArrayOfBillDetails> RunningTreasuryBills
        {
            get => _runningTreasuryBills;
            set => SetProperty(ref _runningTreasuryBills, value);
        }
        public string TotalInvestments
        {
            get => totalInvestments;
            set => SetProperty(ref totalInvestments, value);
        }

        public string AllGoals
        {
            get => allgoals;
            set => SetProperty(ref allgoals, value);
        }
        public string AllFixedInvestments
        {
            get => allFixedDepositInvestments;
            set => SetProperty(ref allFixedDepositInvestments, value);
        }

        public string AllTbillsInvestments
        {
            get => allTbillsDepositInvestments;
            set => SetProperty(ref allTbillsDepositInvestments, value);
        }

        public AllInvestmentViewModel(INavigation navigation) : base(navigation)
        {
             
        }

        public async Task SetRunningFixedDeposits()
        {
          //  var pd = await ProgressDialog.Show("Sending Request. Please Wait...");
            RunningFixedDeposit = new ObservableRangeCollection<FixedDepositValue>();
            try
            {
                double totalAmtInvested = 0;
                if (!string.IsNullOrEmpty(GlobalStaticFields.Customer.CustomerId))
                {
                    var cust = new GetInvestments()
                    {
                        customerid = GlobalStaticFields.Customer.CustomerId,
                        amount = "",
                        message = "",
                        Ref = "",
                        response = "",
                        responseCode = ""
                    };

                    var response = await apirequest.Post<GetInvestments>(cust, "", URLConstants.SwitchApiLiveBaseUrl, "Switch/GetInvestment", "AllInvestments/FixedDeposit");
                    var jsonString = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(jsonString))
                    {
                        //await pd.Dismiss();
                        var jsonData = JsonConvert.DeserializeObject<List<FixedDepositValue>>(jsonString);
                        if (jsonData.Count > 0)
                        {
                            foreach (var item in jsonData)
                            {
                                //totalAmtInvested += Convert.ToDouble(item.Amount);
                                totalFixedDeposit += Convert.ToDouble(item.Amount);
                                TotalInvestments = Utilities.GetCurrency("NGN") + totalFixedDeposit.ToString("N2");
                            }
                            RunningFixedDeposit.AddRange(jsonData);
                        }
                    }
                  //  await pd.Dismiss();
                }

            }
            catch (Exception ex)
            {
                string log = ex.Message;
              //  await pd.Dismiss();
            }
        }

        public async Task SetRunningTreasuryBills()
        {
            double totalAmtInvested = 0;
            string refid = DateTime.Now.Ticks.ToString();
            RunningTreasuryBills = new ObservableRangeCollection<ParthianInnerArrayOfBillDetails>();        
            try
            {
                if (!string.IsNullOrEmpty(GlobalStaticFields.Customer.PhoneNumber))
                {
                    var cust = new GetListOfParthiansBills()
                    {
                        PhoneNumber = GlobalStaticFields.Customer.PhoneNumber,
                        Referenceid = refid,
                        RequestType = 129,
                        TransactionReference = refid,
                        Translocation = "",
                    };
                   
                    var response = await apirequest.Post<GetListOfParthiansBills>(cust, "", URLConstants.SwitchApiLiveBaseUrl, "SPay/GetParthianInvestReq", "AllInvestments/TBills");
                    var jsonString = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(jsonString))
                    {
                       // await pd.Dismiss();

                        var jsonData = JsonConvert.DeserializeObject<ParthianInvestReq>(jsonString);
                        if(jsonData.response == "00")
                        {
                            var parthian = JsonConvert.DeserializeObject<List<ParthianInnerArrayOfBillDetails>>(jsonData.responsedata);
                            foreach (var item in parthian)
                            {                               
                                totalTbills += Convert.ToDouble(item?.AmountInvested);
                                TotalInvestments = "N" + totalTbills ?? totalTbills.ToString("N2");
                            }
                            RunningTreasuryBills.AddRange(parthian);
                        }

                    }
                }
              //  await pd.Dismiss();
            }
            catch (Exception ex)
            {
               //  await pd.Dismiss();
                string log = ex.Message;
            }finally{
               // await pd.Dismiss();
            }
            AllFixedInvestments = $"Fixed Deposit: {Utilities.GetCurrency("NGN") }{totalFixedDeposit.ToString("N2")}";
            AllTbillsInvestments = $"T-Bills: {Utilities.GetCurrency("NGN") }{totalTbills.ToString("N2")}";
        }
    }
}
