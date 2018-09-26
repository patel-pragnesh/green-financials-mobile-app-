using SterlingSwitch.Pages.Cards.Models;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.Repository;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Cards
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RequestCardPage : SwitchMasterPage
    {
		public RequestCardPage ()
		{
			InitializeComponent ();
            customer = GlobalStaticFields.Customer;
            Setup();
        }

        private string Address = string.Empty;
        private Customer customer;
        ApiRequest apiRequest = new ApiRequest();


        List<string> virtualCardCurrency = new List<string>() { "NGN", "USD" };

        private void Setup()
        {
            CardTypePicker.SelectedIndex = 0;
            AddressPicker.SelectedIndex = 0;
            CurrencyPicker.SelectedIndex = 0;

            StatePicker.SelectedIndex = 0;
            CountryPicker.SelectedIndex = 0;


            if (customer.ListOfAllAccounts != null && customer.ListOfAllAccounts.Count > 0)
            {
                List<string> acc = new List<string>();
                foreach (var item in customer.ListOfAllAccounts)
                {
                    acc.Add(item.AccountNumberWithBalance);
                }

                AccountPicker.ItemsSource = acc;
            }

            NameTxt.Text = $"{customer.FirstName} {customer.LastName}";

            List<string> cardTypeList = new List<string>() { "MasterCard", "Visa Card", "Virtual Card" };
            CardTypePicker.ItemsSource = cardTypeList;

            List<string> addressList = new List<string>() { "Default Address", "New Address" };
            AddressPicker.ItemsSource = addressList;

            List<string> countryList = new List<string>() { "Nigeria" };
            CountryPicker.ItemsSource = countryList;

            List<string> stateList = new List<string>() { "Abia", "Adamawa", "Akwa Ibom", "Anambra", "Bauchi", "Bayelsa", "Benue", "Borno", "Cross River", "Delta", "Ebonyi", "Enugu", "Edo", "Ekiti", "Gombe", "Imo", "Jigawa", "Kaduna", "Kano", "Katsina", "Kebbi", "Kogi", "Kwara", "Lagos", "Nasarawa", "Niger", "Ogun", "Ondo", "Osun", "Oyo", "Plateau", "Rivers", "Sokoto", "Taraba", "Yobe", "Zamfara", "FCT, Abuja" };
            StatePicker.ItemsSource = stateList;
        }

        private string GetProductID(string cardType)
        {
            if (cardType == "MasterCard")
                cardType = "53";
            else if (cardType == "Visa Card")
                cardType = "46";
            return cardType;
        }

        private RequestCardModel CreateModel(string Account, string Address, string CardType, string CurrencyCode)
        {
            ////Remove Later==================
            //page.CustomerId = "268325";
            ////==============================

            RequestCardModel rcm = new RequestCardModel();
            rcm.Address = Address;
            rcm.Cardfirstname = customer.FirstName;
            rcm.Cardsurname = customer.LastName;
            rcm.Title = GetTitle();
            rcm.Account = Account;
            rcm.Dateissued = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";
            rcm.PasExpDat = " ";
            rcm.Email = customer.Email;
            rcm.Customernumber = customer.CustomerId;
            rcm.RegionReg = " ";
            rcm.Resident = " ";
            rcm.ResAddress = Address;
            rcm.SecretAnsw = " ";
            rcm.SecretQuer = " ";
            rcm.RequestChannelID = "1";
            rcm.productID = GetProductID(CardType);
            rcm.Pindelivery = "NG0020006";
            rcm.Phone = customer.PhoneNumber;
            rcm.PasPlace = " ";
            rcm.PASNOM = " ";
            rcm.PasExpDat = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";
            rcm.Ledgercode = " ";
            rcm.GroupCmd = " ";
            rcm.FinProf = "0";
            rcm.Cusregion = " ";
            rcm.CusName = customer.FirstName + " " + customer.LastName;
            rcm.Cuscity = " ";
            rcm.Currencycode = CurrencyCode;
            rcm.CntryReg = "566";
            rcm.CountryRes = "566";
            rcm.CntryLive = " ";
            rcm.CityReg = " ";
            rcm.Cellphone = customer.PhoneNumber;
            rcm.Cardsurname = customer.LastName;
            rcm.Cardmidname = " ";
            rcm.Carddelivery = "NG0020006";
            rcm.Birthday = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";
            rcm.Sex = customer.Gender.Substring(0, 1);


            return rcm;
        }

        private async void Request()
        {
            var pd = await ProgressDialog.Show("Processing...please wait!");
            string Account = customer.ListOfAllAccounts.FirstOrDefault(k => k.AccountNumberWithBalance == AccountPicker.SelectedItem).nuban;
            string cardType = CardTypePicker.SelectedItem.ToString();
            string currencyCode = CurrencyPicker.SelectedItem.ToString();

            string BaseURL = URLConstants.SwitchApiLiveBaseUrl;
            string endpoint = "Switch/SubmitCardRequest";
            string url = BaseURL + endpoint;

            var requestModel = CreateModel(Account, Address, cardType, currencyCode);

            try
            {
                var response = await apiRequest.Post(requestModel, "", BaseURL, endpoint, "RequestCardPage");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    if (jsonString.ToLower().Contains("request submitted successfully"))
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("Card Request", "Card Request was submitted successfully.", DialogType.Success, "OK", null);
                    }
                    else
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("Card Request", "Error submitting card request, please try again.", DialogType.Error, "OK", null);
                        await BusinessLogic.Log(jsonString.ToString(), "Error submitting card request, please try again.", url, "", jsonString, "RequestCardPage");
                    }
                }
                else
                {
                    await pd.Dismiss();
                    MessageDialog.Show("Card Request", "Error submitting card request, please try again.", DialogType.Error, "OK", null);
                    await BusinessLogic.Log(response.ToString(), "Error submitting card request, please try again.", url, "", response.ToString(), "RequestCardPage");
                }
            }
            catch (Exception ex)
            {
                await pd.Dismiss();
                MessageDialog.Show("Card Request", "Error submitting card request, please try again.", DialogType.Error, "OK", null);
                await BusinessLogic.Log(ex.ToString(), "Error submitting card request, please try again.", url, "", "", "CardRequestPage");
            }

        }

        private string GetTitle()
        {
            string title = "";
            if (customer.Gender.Substring(0, 1) == "M")
                title = "Mr";
            else
                title = "Mrs";

            return title;
        }

        private async void VirtualCardRequest()
        {
            var pd = await ProgressDialog.Show("Processing...please wait!");


            int currencyCode = GetCurrencyCode(CurrencyPicker.SelectedItem.ToString());

            string BaseURL = URLConstants.SwitchApiLiveBaseUrl;
            string endpoint = "Switch/SubmitVirtualCardrequest";
            string url = BaseURL + endpoint;

            var virtualCardModel = new VirtualCard();

            //Remove later===============
            //virtualCardModel.UserEmail = "sunnymexy@gmail.com";
            //============================

            virtualCardModel.IsVirtualCard = true;
            virtualCardModel.IsVirtualCardActive = 1;
            virtualCardModel.UserEmail = customer.Email;
            virtualCardModel.VirtualCardRequestChannelID = 1;
            virtualCardModel.VirtualCardCurrency = currencyCode;
            virtualCardModel.ProductID = 121;
            virtualCardModel.Account = customer.PhoneNumber.Replace("+", "");

            try
            {

                var response = await apiRequest.Post(virtualCardModel, "", BaseURL, endpoint, "RequestCardPage");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    if (jsonString.ToLower().Contains("submitted successfully"))
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("Card Request", "Card Request was submitted successfully.", DialogType.Success, "OK", null);
                    }
                    else
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("Card Request", "Error submitting card request, please try again.", DialogType.Error, "OK", null);
                        await BusinessLogic.Log(jsonString.ToString(), "Error submitting virtual card request, please try again.", url, "", jsonString, "RequestCardPage");
                    }
                }
                else
                {
                    await pd.Dismiss();
                    MessageDialog.Show("Card Request", "Error submitting card request, please try again.", DialogType.Error, "OK", null);
                    await BusinessLogic.Log(response.ToString(), "Error submitting virtual card request, please try again.", url, "", response.ToString(), "RequestCardPage");
                }
            }
            catch (Exception ex)
            {
                await pd.Dismiss();
                MessageDialog.Show("Card Request", "Error submitting card request, please try again.", DialogType.Error, "OK", null);
                await BusinessLogic.Log(ex.ToString(), "Error submitting virtual card request, please try again.", url, "", "", "RequestCardPage");
            }
        }

        private int GetCurrencyCode(string v)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("SterlingSwitch.Resources.CountryInfo.xml");
            string text = "";
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            XDocument xdoc = XDocument.Parse(text);
            var item = xdoc.Descendants("country").Where(i => i.Attribute("currency").Value != null && i.Attribute("currency").Value.Trim() == v.Trim()).Single();

            if (item != null)
            {
                string currency = item.Attribute("ccn3").Value;

                if (!string.IsNullOrWhiteSpace(currency))
                    return int.Parse(currency);
            }

            return 566;
        }

        private void CardTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CardTypePicker.SelectedItem == null)
                return;

            if (CardTypePicker.SelectedItem.ToString().Contains("Virtual"))
            {
                CurrencyPicker.ItemsSource = virtualCardCurrency;
                CurrencyPicker.SelectedIndex = 0;

                AccountPicker.IsVisible = false;
                AddressPicker.IsVisible = false;
                NewAddressView.IsVisible = false;
            }
            else
            {
                AccountPicker.IsVisible = true;
                AddressPicker.IsVisible = true;

                AddressPicker.SelectedIndex = 0;
            }
        }

        private void AccountPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AccountPicker.SelectedItem == null)
                return;

            if (GlobalStaticFields.Customer.ListOfAllAccounts != null && GlobalStaticFields.Customer.ListOfAllAccounts.Count > 0)
            {
                var currency = GlobalStaticFields.Customer.ListOfAllAccounts[AccountPicker.SelectedIndex].currencyCode;

                List<string> curList = new List<string>() { currency };

                CurrencyPicker.ItemsSource = curList;
                CurrencyPicker.SelectedIndex = 0;
            }
            else
                CurrencyPicker.ItemsSource = null;
        }

        private void AddressPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AddressPicker.SelectedItem == null)
                return;

            if (AddressPicker.SelectedItem.ToString() == "New Address")
                NewAddressView.IsVisible = true;
            else
                NewAddressView.IsVisible = false;
        }

        private void RequestBtn_Clicked(object sender, EventArgs e)
        {
            if (CardTypePicker.SelectedItem.ToString().Contains("Virtual"))
            {
                VirtualCardRequest();
            }
            else
            {
                if (AccountPicker.SelectedItem == null)
                {
                    MessageDialog.Show("Account Number", "Kindly select an account to continue.", DialogType.Error, "OK", null);
                    return;
                }

                if (AddressPicker.SelectedItem.ToString() == "New Address")
                {
                    if (string.IsNullOrWhiteSpace(AddressOneTxt.Text) | string.IsNullOrWhiteSpace(AddressTwoTxt.Text))
                    {
                        MessageDialog.Show(" Delivery Address", "Kindly fill out the delivery address to continue.", DialogType.Error, "OK", null);
                        return;
                    }

                    Address = $"{AddressOneTxt.Text}, {AddressTwoTxt.Text}, {StatePicker.SelectedItem.ToString()}, {CountryPicker.SelectedItem.ToString()}";
                }
                else
                    Address = "DEFAULT ADDRESS";

                Request();
            }
        }
    }
}