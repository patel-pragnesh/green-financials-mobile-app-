using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.DeviceInfo;
using SterlingSwitch.Extensions;
using SterlingSwitch.Helper;
using SterlingSwitch.Models;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SterlingSwitch.Services.Repository
{
    public class Customer
    {
        public Customer()
        {
            ListOfAllAccounts = new List<CustomerAccount>();
            MyCards = new List<MyCards>();
        }
        public string Email { get; set; }
        public string AccountNumber { get; set; }
        public string AccountID { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserID { get; set; }
        public List<MyCards> MyCards { get; set; }
        public List<string> UniqueCustomerIDs { get; set; }
        public bool IsExistingCustomer { get; set; }
        public bool IsLoggedIn { get; set; }
        public List<CustomerAccount> ListOfAllAccounts { get; set; }       
        public double WalletBalance { get; set; }
        public string WalletAcctNo { get; set; }
        public string CustomerId { get; set; }
        public DateTime BirthDate { get; set; }
        public string MiddleName { get; set; }
        public string Gender { get; set; }
        public string ProfilePix { get; set; }
        public string BVN { get; set; }
        public string HomeAddress { get; set; }
        public string ValidMeansOfId { get; set; }
        public string ReferralCode { get; set; }
        public bool IsLoggedOn { get; set; } = false;
        public bool IsTPin { get; set; }
        public bool IsAccountLock { get; set; }
        public async Task<List<MyCards>> GetCards(bool showActivity)
        {
            ProgressDialog pd = null;
            List<MyCards> allCards = new List<MyCards>();
            try
            {

                //if (showActivity)
                //    pd = await ProgressDialog.Show("Getting all your cards, please wait.");

                var apirequest = new ApiRequest();

                foreach (var item in UniqueCustomerIDs)
                {
                    var cardModel = new CardModel()
                    {
                        CustomerID = item,
                        ReferenceID = Utilities.GenerateReferenceId(),
                        RequestType = "944"
                    };

                    var request = await apirequest.PostIBS<CardModel>(cardModel, "", URLConstants.SwitchApiLiveBaseUrl, "IBSIntegrator/IBSBridgeJSONForCard", "Customer.cs");

                    if (request.IsSuccessStatusCode)
                    {
                        var resTxt = await request.Content.ReadAsStringAsync();
                        await BusinessLogic.Log("", "response of all cards request...", URLConstants.SwitchApiLiveBaseUrl + "IBSIntegrator/IBSBridgeJSONForCard", cardModel.ToString(), resTxt, "DashboardV3ViewModel");
                        resTxt = resTxt.JsonCleanUp();
                        resTxt = JsonConvert.DeserializeObject<CardObject>(resTxt).IBSResponse.ResponseText;
                        if (!string.IsNullOrWhiteSpace(resTxt) && resTxt.Length > 8)
                        {
                            if (resTxt.Split('|')[0].Length > 2)
                            {
                                string[] pans = resTxt.Split('~');


                                foreach (string pan in pans)
                                {
                                    var cardInfo = pan.Split('|');
                                    var pand = cardInfo[0];
                                    var Exp = $"{cardInfo[1].Substring(2, 2)} / {cardInfo[1].Substring(0, 2)}";
                                    allCards.Add(new MyCards()
                                    {
                                        CardName = $"{FirstName} {LastName}",
                                        CardPan = pand,
                                        MaskedPan = $"**** **** **** {pand.Substring(pand.Length - 4)}",
                                        LastDigits = pand.Substring(pand.Length - 4),
                                        ExpiryYear = cardInfo[1].Substring(0, 2),
                                        ExpiryMonth = cardInfo[1].Substring(2, 2),
                                        CVV = "***",
                                        FormattedExp = Exp,
                                        SequenceNumber = cardInfo[2],
                                        CardType = cardInfo[3],
                                        CardProgram = cardInfo[3],
                                        CardLogo = GetLogo(cardInfo[3]),
                                        HolderColor = Color.DeepSkyBlue
                                    });
                                }
                            }

                        }
                        else
                        {
                            // no cards found
                        }
                    }
                }

                //if (showActivity)
                //    pd?.Dismiss();
            }
            catch (System.Net.WebException ex)
            {
                //if (showActivity)
                //    pd?.Dismiss();

                await BusinessLogic.Log(ex.ToString(), "Web Exception....Data Connection Error.", "", "", "", "Customer.cs");
            }
            catch (Exception ex)
            {
                //if (showActivity)
                //    pd?.Dismiss();

                await BusinessLogic.Log(ex.ToString(), "Exception on getting all cards.", "", "", "", "Customer.cs");
            }

            return allCards;
        }
        private string GetCardType(string type)
        {
            if (type.ToUpper().Contains("VISA"))
                return "Visa Card";
            if (type.ToUpper().Contains("VERVE"))
                return "Verve Card";

            return "Master Card";
        }
        private ImageSource GetLogo(string type)
        {
            if (type.ToUpper().Contains("VISA"))
                return ImageSource.FromFile("visa");

            if (type.ToUpper().Contains("VERVE"))
                return ImageSource.FromFile("verve");

            return ImageSource.FromFile("master");
        }
        public async Task<List<CustomerAccount>> GetAccountsbyPhoneNumber(string telephone)
        {
            try
            {
                if (ListOfAllAccounts == null) ListOfAllAccounts = new List<CustomerAccount>();

                ListOfAllAccounts.Clear();
                var apirequest = new ApiRequest();
                List<CustomerAccount> customerActs = new List<CustomerAccount>();
                dynamic accts = new JObject();
                accts.phone = telephone;
                var request = await apirequest.Post<dynamic>(accts, "", URLConstants.SwitchApiLiveBaseUrl, "Switch/GetAllOtherAcctDetailsByNumber", "BankAccountsViewModel");

                if (request.IsSuccessStatusCode)
                {
                    var response = await request.Content.ReadAsStringAsync();
                    customerActs = (List<CustomerAccount>)JsonConvert.DeserializeObject<List<CustomerAccount>>(response);
                    customerActs = customerActs.Distinct(new CustomerAccountEquality()).ToList();
                    foreach (CustomerAccount acc in customerActs)
                    {
                        ListOfAllAccounts.Add(new CustomerAccount
                        {
                            nuban = acc.nuban,
                            currency = acc.currencyCode == null ? "" : Utilities.GetCurrency(acc.currencyCode),
                            currencyCode = acc.currencyCode,
                            balance = acc.balance,
                            AccountNumberWithBalance = $"{acc.nuban} | {Utilities.GetCurrency(acc.currencyCode ?? "")} {acc.balance} | {acc.accountType}",
                            CustomerId = acc.CustomerId,
                            AccountName = acc.AccountName,
                            AccountGroup = acc.AccountGroup,
                            accountType = acc.accountType,
                            BVN = acc.BVN
                        });
                    }
                    if (UniqueCustomerIDs == null) UniqueCustomerIDs = new List<string>();

                    UniqueCustomerIDs = ListOfAllAccounts.Select(x => x.CustomerId).Distinct<string>().ToList();
                    
                    //Add the CustomerID for wallet account
                    UniqueCustomerIDs.Add($"VT{PhoneNumber.Replace("+", "")}");
                }
                var cards = await GetCards(true);
                MyCards = cards;
                return ListOfAllAccounts;

            }
            catch (Exception ex)
            {
                return default(List<CustomerAccount>);
            }

        }

        // get the current device the user is using
     
    }
}
