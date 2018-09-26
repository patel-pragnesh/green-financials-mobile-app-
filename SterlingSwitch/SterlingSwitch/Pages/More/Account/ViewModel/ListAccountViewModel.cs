using SterlingSwitch.Helper;
using SterlingSwitch.Models;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.ViewModelBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.More.Account.ViewModel
{
  public class ListAccountViewModel : BaseViewModel
    {
         public ListAccountViewModel(INavigation navigation) : base(navigation)
        {
            CustomerAccounts = new ObservableRangeCollection<CustomerAccount>();
            CustomerCards = new ObservableRangeCollection<MyCards>();
            profileListAccount = new ObservableRangeCollection<ProfileListAccount>();
        }

        #region Properties
        public ObservableRangeCollection<CustomerAccount> CustomerAccounts { get; set; }
        public ObservableRangeCollection<MyCards> CustomerCards { get; set; }

        public ObservableRangeCollection<ProfileListAccount> profileListAccount { set; get; }

        private bool _isAccountLoaded;
        public bool IsAccountLoaded
        {
            get { return _isAccountLoaded; }
            set { SetProperty(ref _isAccountLoaded, value); }
        }
        #endregion Properties

        #region Events
        public async Task GetCustomerAccountsbyPhoneNumber(string telephone)
        {

            try
            {
                CheckConnectivity();
                //CardLoading = true;

                List<CustomerAccount> customerAccounts = await GlobalStaticFields.Customer.GetAccountsbyPhoneNumber(telephone);
                List<ProfileListAccount> profileListAccounts = new List<ProfileListAccount>();

                if (customerAccounts?.Count > 0)
                {
                    CustomerAccounts.ReplaceRange(customerAccounts);

                    if (CustomerAccounts?.Count > 0)
                    {
                        foreach (var item in CustomerAccounts)
                        {
                            var profileAcc = new ProfileListAccount
                            {
                                nuban = item.nuban,
                                AccountType = item.accountType,
                                BalanceWithCurrency = $"{Utilities.GetCurrency(item.currencyCode)}{item.balance}"
                            };

                            profileListAccounts.Add(profileAcc);

                        }

                        profileListAccount.ReplaceRange(profileListAccounts);
                    }
                    //await GlobalStaticFields.Customer.GetCards(false);
                }

                //var apirequest = new ApiRequest();

                //dynamic accts = new JObject();
                //accts.phone = telephone;
                //var request = await apirequest.Post<dynamic>(accts, "", URLConstants.SwitchApiLiveBaseUrl, "Switch/GetAllOtherAcctDetailsByNumber", "BankAccountsViewModel");

                //if (request.IsSuccessStatusCode)
                //{
                //    var response = await request.Content.ReadAsStringAsync();
                //    List<CustomerAccount> content = JsonConvert.DeserializeObject<List<CustomerAccount>>(response);
                //    if (content?.Count > 0)
                //        CustomerAccounts.ReplaceRange(content);

                //}
                //else
                //{
                //    var response = await request.Content.ReadAsStringAsync();
                //}

            }
            catch (Exception ex)
            {


            }
            finally
            {
                IsAccountLoaded = true;
               // CardLoading = false;
            }
        }
        #endregion Events
    }

    public class ProfileListAccount
    {
        public string nuban { get; set; }
        public string AccountType { get; set; }
        public string BalanceWithCurrency { set; get; }
    }
}
