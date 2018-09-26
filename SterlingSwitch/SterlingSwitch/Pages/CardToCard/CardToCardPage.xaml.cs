using Newtonsoft.Json;
using SterlingSwitch.Helper;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.Repository;
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
using static SterlingSwitch.Models.SomeConstant;

namespace SterlingSwitch.Pages.CardToCard
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CardToCardPage : SwitchMasterPage
    {
		public CardToCardPage ()
		{
			InitializeComponent ();
            LoadDetails();


            var x = (this.GetType().Name);
            //BusinessLogic.LogFrequentPage(x, PageAliasConstant.CardToCard, ImageConstants.CardToCardIcon);

            AccountPicker.RefreshContent = RefreshAccounts;

        }

        ApiRequest apiRequest = new ApiRequest();

        private string BaseURI = URLConstants.SwitchApiLiveBaseUrl;
        private Customer customer = GlobalStaticFields.Customer;


        public List<string> accountList { get; set; }
        public string SenderAccountNumber { get; set; }
        public string BeneficiaryCard { get; set; }
        public string Beneficiary { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }

        private void ValidateInfo()
        {
            if (AccountPicker.SelectedItem == null)
            {
                MessageDialog.Show("fvCard To Card Transfer", "Please select account to debit", DialogType.Error, "OK", null);
                return;
            }

            SenderAccountNumber = customer.ListOfAllAccounts.Where(x => x.AccountNumberWithBalance == AccountPicker.SelectedItem).SingleOrDefault().nuban;
            BeneficiaryCard = BeneficiaryCardTxt.Text;
            Beneficiary = BeneficiaryNameTxt.Text;
            Amount = AmountTxt.Text;
            Description = DescriptionTxt.Text;

            
            if (string.IsNullOrEmpty(BeneficiaryCard))
            {
                MessageDialog.Show("Card To Card Transfer", "Please enter beneficiary card details", DialogType.Error, "OK", null);
                return;
            }
            else if (string.IsNullOrEmpty(Beneficiary))
            {
                MessageDialog.Show("Card To Card Transfer", "Please enter Beneficiary's Fullname", DialogType.Error, "OK", null);
                return;
            }
            else if (string.IsNullOrEmpty(Amount))
            {
                MessageDialog.Show("Card To Card Transfer", "Please enter amount to send", DialogType.Error, "OK", null);
                return;
            }
            else if (string.IsNullOrEmpty(Description))
            {
                MessageDialog.Show("Card To Card Transfer", "Please enter a description", DialogType.Error, "OK", null);
                return;
            }
            else
            {
                MessageDialog.Show("Confirm Transaction", $"Please Confirm the transfer of: {Utilities.GetCurrency("NGN")}{Convert.ToDouble(Amount).ToString("N2")} to {Beneficiary}", DialogType.Question, "Proceed",
                    ()=> 
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (GlobalStaticFields.Customer.IsTPin)
                            {
                                var CardToCardVerifyPIN = new PopUps.VerifyPinPage("Confirmation", Convert.ToDouble(Amount).ToString("N2"), "NGN", null);
                                CardToCardVerifyPIN.Validated += CardToCardVerifyPIN_Validated;
                                Navigation.PushAsync(CardToCardVerifyPIN);
                            }
                            else
                            {
                                ProcessPayment();
                            }
                        });
                    }, "Dismiss", null);
            }
        }

        private void CardToCardVerifyPIN_Validated(object sender, bool e)
        {
            ProcessPayment();
        }

        private async void ProcessPayment()
        {
            var pd = await ProgressDialog.Show("Sending payment, please wait...");

            DateTime d = DateTime.Today;
            int yr = DateTime.Today.Year;
            string lastDigitOfYear = yr.ToString().Substring(3, 1);
            var da = lastDigitOfYear + d.DayOfYear.ToString("000");
            var systTraceAuditNo = "05" + GenNewPin().ToString();
            string destAmt = "";

            var curCode = customer.ListOfAllAccounts.Where(s => s.nuban.Equals(SenderAccountNumber)).FirstOrDefault().currency;

            
            var cardEnquiry = new MVISAEnquiryData()
            {
                Referenceid = Utilities.GenerateReferenceId(),
                RequestType = 138,
                Translocation = GlobalStaticFields.GetUserLocation,
                acquiringBin = "408999",
                acquirerCountryCode = "840",
                primaryAccountNumber = BeneficiaryCard,
                retrievalReferenceNumber = da + DateTime.Now.ToString("HH") + systTraceAuditNo,  //"0055" + _helperClass.GenNewPin(),
                systemsTraceAuditNumber = systTraceAuditNo
            };

            // call name Enquiry
            var market = await GetMarkup();   // get the markup rate

            MarkupRate markupRate = null;
            if (!string.IsNullOrEmpty(market))
            {
                markupRate = JsonConvert.DeserializeObject<MarkupRate>(market); // deserialise the markup rate
            }

            var enquiryResponse = await MVISAAccEnquiry(cardEnquiry);

            if (!string.IsNullOrEmpty(enquiryResponse))
            {
                var jsonData = JsonConvert.DeserializeObject<MVISAEnquiryResponse>(enquiryResponse);

                // perform some operations after name enquiry
                var replc = jsonData.responsedata.Replace('\\', ' ');
                var dd = JsonConvert.DeserializeObject<MVISAEnquiryResponseData>(replc);

                if (dd.geoRestrictionInd != "N")
                {
                    await pd.Dismiss();
                    MessageDialog.Show("Error", "Recipient issuer does not participate in fast funds.", DialogType.Error, "OK", null);
                    return;
                }
                if (dd.pushFundsBlockIndicator == "N")
                {
                    await pd.Dismiss();
                    MessageDialog.Show("Error", "Recipient issuer does not participate in fast funds.", DialogType.Error, "OK", null);
                    return;
                }

                if (dd.cardIssuerCountryCode != "566")
                {
                    Random gen0 = new Random();
                    var TraceNo = gen0.Next(100000, 999999);

                    // lets have a watchlist before 
                    var wl = new MVisaWatchListReq()
                    {
                        cardIssuerCountryCode = dd.cardIssuerCountryCode,
                        city = dd.billingCurrencyCode,
                        name = Beneficiary,
                        Translocation = GlobalStaticFields.GetUserLocation,
                        Referenceid = Utilities.GenerateReferenceId(),
                        RequestType = 140,
                        referenceNumber = da + DateTime.Now.ToString("HH") + TraceNo.ToString()
                    };

                    // call watchlist
                    var wldata = await MVisawatchListInquiry(wl);
                    if (!string.IsNullOrEmpty(wldata))
                    {
                        var wldd = JsonConvert.DeserializeObject<MVISAEnquiryResponse>(wldata);
                        var wldReplc = wldd.responsedata.Replace('\\', ' ');
                        var wlrplcdd = JsonConvert.DeserializeObject<WListData>(wldReplc);
                        if (Convert.ToInt32(wlrplcdd.ofacScore) >= 65)
                        {
                            await pd.Dismiss();
                            MessageDialog.Show("Transaction Failed", "Recipient issuer does not participate in fast funds.", DialogType.Error, "OK", null);
                            return;
                        }
                    }

                    // lets call exchange rate
                    var exrate = new MVISAExchangeRate()
                    {
                        destinationCurrencyCode = dd.billingCurrencyCode,
                        sourceAmount = Amount,
                        markUpRate = markupRate.markUpRate,
                        systemsTraceAuditNumber = systTraceAuditNo,
                        Referenceid = Utilities.GenerateReferenceId(),
                        RequestType = 137,
                        retrievalReferenceNumber = da + DateTime.Now.ToString("HH") + systTraceAuditNo,
                        sourceCurrencyCode = curCode,
                        Translocation = GlobalStaticFields.GetUserLocation
                    };

                    var exchrt = await MVISAExchangeRate(exrate);

                    if (!string.IsNullOrEmpty(exchrt))
                    {
                        var dt = JsonConvert.DeserializeObject<MVISAEnquiryResponse>(exchrt);
                        var dtrplc = dt.responsedata.Replace('\\', ' ');
                        var rplc = JsonConvert.DeserializeObject<ExchangeRateResponse>(dtrplc);
                        // destAmt = rplc.destinationAmount;

                        var newDestAmount = Convert.ToDecimal(Amount) / Convert.ToDecimal(rplc.destinationAmount);
                        var newDestAmount2 = newDestAmount * (Convert.ToDecimal(markupRate.markUpRate) / 100);
                        var newDestAmount3 = newDestAmount + newDestAmount2;
                        var actualDestinationAmount = Convert.ToDecimal(Amount) / newDestAmount3;
                        destAmt = actualDestinationAmount.ToString();
                    }

                }
                else
                    destAmt = Amount;

                var mVISA = new MVISACardModel()
                {
                    senderAccountNumber = SenderAccountNumber,
                    name = customer?.FirstName + " " + customer?.LastName,
                    recipientName = Beneficiary,
                    businessApplicationId = "PP",
                    merchantCategoryCode = "6012",
                    sourceOfFundsCode = "03",
                    country = dd.cardIssuerCountryCode,
                    county = dd.cardIssuerCountryCode,
                    zipCode = dd.cardIssuerCountryCode,
                    state = "",
                    Translocation = GlobalStaticFields.GetUserLocation,
                    Referenceid = Utilities.GenerateReferenceId(),
                    idCode = "SBPL" + GenNewPin(),
                    terminalId = "323" + GenNewPin(),
                    transactionIdentifier = GenNewPin().ToString(),
                    systemsTraceAuditNumber = systTraceAuditNo,
                    RequestType = 144,
                    retrievalReferenceNumber = da + DateTime.Now.ToString("HH") + systTraceAuditNo,
                    recipientPrimaryAccountNumber = BeneficiaryCard,
                    amount = destAmt,
                    transactionCurrencyCode = dd.billingCurrencyCode,
                    destinationAmount = destAmt
                };

                var response = await CardToCardMVISA(mVISA);

                if (!string.IsNullOrEmpty(response))
                {
                    var json = JsonConvert.DeserializeObject<CardToCardResponse>(response);
                    await pd.Dismiss();
                    if (json.response == "00")
                    {
                        if (dd.fastFundsIndicator == "B"
                            || dd.fastFundsIndicator == "D"
                            || dd.fastFundsIndicator == "E")
                        {
                            MessageDialog.Show("Info", "Your transaction was successful. Funds will be available to recipient within 30 mins of successful transfer", DialogType.Success, "OK", null);
                        }
                        else
                            MessageDialog.Show("Info", "YOur transaction was Successful. Funds will be available within 2 business days", DialogType.Success, "OK", null);
                        
                    }
                }
                else
                {
                    await pd.Dismiss();
                    MessageDialog.Show("Error", "Sorry, this operation was not successful. Please try again later.", DialogType.Error, "OK", null);
                    return;
                }
            }
            else
            {
                await pd.Dismiss();
                MessageDialog.Show("Error", "Sorry we are unable to validate the beneficiary account. Please review and try again.", DialogType.Error, "OK", null);
                return;
            }
        }

        public async Task<string> GetMarkup()
        {
            string url = BaseURI;
            string endpoint = "Switch/GetMarkup";

            try
            {
                var result = await apiRequest.Post<dynamic>(null, "", BaseURI, endpoint, "CardToCardPage");

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await result.Content.ReadAsStringAsync();
                    return jsonString;
                }
                else
                    return "";

            }
            catch (Exception ex)
            {
                await BusinessLogic.Log(ex.ToString(), "Error calling MVisa Markup", url, "", "", "CardToCardPage");
                return ex.Message;
            }
        }

        public async Task<string> MVISAAccEnquiry(MVISAEnquiryData model)
        {
            string url = BaseURI;
            string endpoint = "Spay/MVisaFundsTransferEnquir";

            try
            {
                
                var response = await apiRequest.Post(model, "", BaseURI, endpoint, "CardToCardPage");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonstring = await response.Content.ReadAsStringAsync();
                    return jsonstring;
                }
            }
            catch (Exception ex)
            {
                await BusinessLogic.Log(ex.ToString(), "Error calling MVisa Fund Enguiry", url, "", "", "CardToCardPage");
            }

            return "";
        }

        public async Task<string> MVISAExchangeRate(MVISAExchangeRate model)
        {
            string url = BaseURI;
            string endpoint = "Spay/MVisaForeignExchangeRates";

            try
            {
                
                var response = await apiRequest.Post(model, "", BaseURI, endpoint, "CardToCardPage");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonstring = await response.Content.ReadAsStringAsync();
                    return jsonstring;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                await BusinessLogic.Log(ex.ToString(), "Error calling MVisa Exchange Rate", url, "", "", "CardToCardPage");
            }

            return "";
        }
        public async Task<string> MVisawatchListInquiry(MVisaWatchListReq model)
        {
            string url = BaseURI;
            string endpoint = "Spay/MVisawatchListInquiry";

            try
            {                               
                var response = await apiRequest.Post(model, "", BaseURI, endpoint, "CardToCardPage");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonstring = await response.Content.ReadAsStringAsync();
                    return jsonstring;
                }
                else
                    return "";
            }
            catch (Exception ex)
            {
                await BusinessLogic.Log(ex.ToString(), "Error calling MVisa Watch List", url, "", "", "CardToCardPage");
                return "";
            }
        }

        public async Task<string> CardToCardMVISA(MVISACardModel model)
        {
            string url = BaseURI;
            string endpoint = "Spay/MVisaPushFundsTransactions";

            var response = await apiRequest.Post(model, "", BaseURI, endpoint, "CardToCardPage");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonstring = await response.Content.ReadAsStringAsync();
                return jsonstring;
            }

            return "";
        }

        public void LoadDetails()
        {
            accountList = new List<string>();
            foreach (var item in customer.ListOfAllAccounts)
            {
                accountList.Add(item.AccountNumberWithBalance);
            }

            AccountPicker.ItemsSource = accountList;

            List<string> cardType = new List<string>() { "Visa Card" };
            CardTypePicker.ItemsSource = cardType;
        }
        public void RefreshAccounts()
        {
            accountList = new List<string>();
            var accounts = GlobalStaticFields.Customer.GetAccountsbyPhoneNumber(GlobalStaticFields.Customer.PhoneNumber).Result;
            foreach (var item in accounts)
            {
                accountList.Add(item.AccountNumberWithBalance);
            }

            AccountPicker.ItemsSource = accountList;

        }
        public int GenNewPin()
        {
            Random r = new Random();
            return r.Next(1000, 9999);
        }

        private void ContinueButton_Clicked(object sender, EventArgs e)
        {
            ValidateInfo();
        }
    }
}