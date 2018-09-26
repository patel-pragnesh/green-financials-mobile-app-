using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SterlingSwitch.Helper;
using SterlingSwitch.Pages.Investments.ViewModel;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.Templates;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Investments.FixedDeposit
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmFixedDepositPage : SwitchMasterPage
    {
        private FixedDepositViewModel _vm;
        public string rate { get; set; }
        public string Amount { get; set; }
        public string Tenure { get; set; }
        public bool IsRollOverActivated { get; set; }
        public string InvestAccount { get; set; }
        public string ReInvest { get; set; }
        public List<string> ReinvestOptionList { get; set; }

        ApiRequest apirequest = new ApiRequest();

        public ConfirmFixedDepositPage(DateTime DateOfMaturity, string NewRate)
        {
            InitializeComponent();
            this.BindingContext = _vm = new FixedDepositViewModel(Navigation);

            lblDuration.Text = FixedDepositDataModel.Tenure;
            lblDepositAmount.Text = Convert.ToDecimal(FixedDepositDataModel.InvestAmount).ToString("N2");
            lblMaturityDate.Text = DateOfMaturity.ToString("MMM dd, yyyy");
            lblMaturityValue.Text = NewRate;
            GetAccounts();
            ReInvestOption();
        }

        public static string GetXMlString(string response)
        {
            string xml = response;
            int lenn = xml.Length;
            if (xml.Contains("Successful"))
            {
                int firstFindOurWord = xml.LastIndexOf("<arrangementid>");
                int lastFindOurWord = xml.IndexOf("</arrangementid>");
                string hhh = xml.Substring(firstFindOurWord, lastFindOurWord - firstFindOurWord);
                xml = hhh.Replace("<arrangementid>", "");
            }
            else
            {
                xml = "x011";
            }
            return xml;
        }

        public async void DoFixedDeposit()
        {
            var accSplitted = Utilities.SplitBeneficiaryDetailsByTab(InvestAccount.Trim());

           // string investAccount = accSplitted[0];
            if (GlobalStaticFields.Customer.IsTPin)
            {
                var ValidateFixDepositWithTPIN = new PopUps.VerifyPinPage("Confirmation", FixedDepositDataModel.InvestAmount, "NGN", null);
                ValidateFixDepositWithTPIN.Validated += ValidateFixDepositWithTPIN_Validated;
               await Navigation.PushAsync(ValidateFixDepositWithTPIN);
            }
            else
            {

                await NewMethod();
            }
        }

        private void ValidateFixDepositWithTPIN_Validated(object sender, bool e)
        {
            NewMethod();
        }

        private async Task NewMethod()
        {
            var pd = await ProgressDialog.Show("Sending Request. Please Wait...");
            try
            {
                var accSplitted = Utilities.SplitBeneficiaryDetailsByTab(InvestAccount.Trim());

                string investAccount = accSplitted[0];

                string currencyCode = "NGN";
                string capitalAcct = investAccount;
                string InvestAcct = investAccount.Trim();
                string InterestAcct = investAccount;

                string t = FixedDepositDataModel.Tenure;
                var t2 = t.Split(' ');

                var day = t2[1];
                var day2 = t2[2];
                var dd = day2.Substring(0, 1);
                string tenure = $"{day}{dd}";

                var deposit = new FixedDepositModel()
                {
                    ReferenceID = Utilities.GenerateReferenceId(),
                    amount = Convert.ToDecimal(FixedDepositDataModel.InvestAmount),
                    RequestType = 926.ToString(),
                    changeperiod = tenure,
                    payinacct = capitalAcct ?? "",
                    payoutacct3 = InvestAcct ?? capitalAcct,
                    payoutacct2 = InterestAcct ?? capitalAcct,
                    payoutacct1 = InvestAcct ?? capitalAcct,
                    customerid = GlobalStaticFields.Customer.CustomerId,
                    currency = currencyCode,
                    rate = FixedDepositDataModel.Rate,
                    effectivedate = DateTime.Now.ToString("yyyy-MM-dd")
                };
                var request = await apirequest.PostIBS<FixedDepositModel>(deposit, "", URLConstants.SwitchApiLiveBaseUrl, "IBSIntegrator/IBSBridge", "FixedDepositPage"); // modify this line

                var response = "";
                string arrangementId = "";
                if (request.IsSuccessStatusCode)
                {
                    response = await request.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(response) && !response.Contains("Unable to connect to the remote server") && !response.Contains("Connection refused"))
                    {
                        arrangementId = GetXMlString(response);
                        var invest = new SaveInvestment()
                        {
                            amount = FixedDepositDataModel.InvestAmount,
                            customerid = GlobalStaticFields.Customer.CustomerId,
                            Ref = arrangementId
                        };
                        if (arrangementId == "x011")
                        {
                            await pd.Dismiss();
                            MessageDialog.Show("OOPS", "Sorry, we are unable to process your request at the moment. Kindly try again later", DialogType.Error, "DISMISS", null);
                            return;
                        }
                        var resp = await apirequest.Post<SaveInvestment>(invest, "", URLConstants.SwitchApiLiveBaseUrl, "Switch/SaveInvest", "FixedDepositPage");
                        if (resp.IsSuccessStatusCode)
                        {
                            await pd.Dismiss();
                            var res = await resp.Content.ReadAsStringAsync();
                            if (!string.IsNullOrEmpty(res))
                            {
                                MessageDialog.Show("SUCCESS", $"Your Fixed Deposit placement worth of {FixedDepositDataModel.InvestAmount} was successful.", PopUps.DialogType.Success, "OK", null); // may need further check     
                                await Navigation.PushAsync(new AllInvestments.AllInvestments());
                            }
                        }
                    }
                    else
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("Internal Error", "Sorry, we are unable to process your request at the moment. Please try again later...", PopUps.DialogType.Error, "OK", null);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                await pd.Dismiss();
                await BusinessLogic.Log(ex.ToString(), "exception response buying fixed deposit for user " + GlobalStaticFields.Customer.Email, "/api/IBSIntegrator/IBSBridge", "", ex.ToString(), "Fixed DepositPage");
                MessageDialog.Show("OOPS", "Sorry, We are unable to process your request at the moment. Please try again later", DialogType.Error, "DISMISS", null);

            }
            await pd.Dismiss();
        }

        private void btnConfirm_Clicked(object sender, EventArgs e)
        {
            if ((dpdDebitAccount.SelectedIndex > -1))            
                DoFixedDeposit();                     
            else
                MessageDialog.Show("OOPS", "Please choose an account to debit and enable re-invest option to proceed", DialogType.Error, "DISMISS", null);
        }

        private void GetAccounts()
        {
            if (GlobalStaticFields.Customer.ListOfAllAccounts != null && GlobalStaticFields.Customer.ListOfAllAccounts.Count > 0)
            {
                List<string> acc = new List<string>();
                foreach (var item in GlobalStaticFields.Customer.ListOfAllAccounts)
                {
                    acc.Add(item.AccountNumberWithBalance);
                }
                dpdDebitAccount.ItemsSource = acc;
            }
        }

        private void ReInvestOption()
        {
            ReinvestOptionList = new List<string> { "Principal Only", "Principal + Interest" };          
        }

        private void dpdDebitAccount_SelectedIndexChanged(object sender, System.EventArgs e)
        {            
            InvestAccount = dpdDebitAccount.SelectedItem;            
        }
         
        private void grdTapped_Tapped(object sender, EventArgs e)
        {
            if(ReinvestOptionList != null && ReinvestOptionList.Count > 0)
            {
                var pickerPop = new ExtendedPickerPopup();

                pickerPop.ItemsSource = ReinvestOptionList;
                pickerPop.PickerTitle = "Re-Invest";              

                pickerPop.SelectedIndexChanged += (p, t) =>
                {
                    lblReinvestValue.Text = t.DisplayText;
                    toggleSelected.IsToggled = true;
                };

                Navigation.PushPopupAsync(pickerPop, true);
            }
        }

        private async void lblDismiss_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}