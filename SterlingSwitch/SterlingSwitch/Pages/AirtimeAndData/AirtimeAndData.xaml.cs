using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SterlingSwitch.Helper;
using SterlingSwitch.Pages.AirtimeAndData.ViewModel;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SterlingSwitch.Models.SomeConstant;

namespace SterlingSwitch.Pages.AirtimeAndData
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AirtimeAndData : SwitchMasterPage
	{
	    private AirtimeDataViewModel _vm;
	    private List<NetworkProviderModel.Provider> providers;
        public AirtimeAndData ()
		{
			InitializeComponent ();
		    this.BindingContext = _vm = new AirtimeDataViewModel(Navigation);
            DebitAccount.RefreshContent = RefreshCustomerAccount;

            //get name of this present page

            var x = (this.GetType().Name);
            BusinessLogic.LogFrequentPage(x, PageAliasConstant.airtimedata, ImageConstants.AirtimeDataIcon);

         

        }

        protected override async void OnAppearing()
        {
            
            base.OnAppearing();
            await SetServices();
        }
        void RefreshCustomerAccount()
        {
            if (GlobalStaticFields.Customer != null)
            {
                if (GlobalStaticFields.Customer.ListOfAllAccounts != null)
                {
                    var accountPreview = new List<string>();
                    GlobalStaticFields.Customer.ListOfAllAccounts.ForEach(a => { accountPreview.Add(a.AccountBalanceAccountType); });
                    DebitAccount.ItemsSource = accountPreview;
                }

            }
        }
	    private void Category_OnSelectedIndexChanged(object sender, EventArgs e)
	    {
	        _vm.SelectedCategory = Category.SelectedIndex;
	        _vm.SelectedCategoryItem = Category.SelectedItem;
	        if (_vm.SelectedCategory == 1)
	        {
	            _vm.IsListofBundleVisible = true;
	            Amount.IsEnabled = false;

	        }
	        else
	        {
	            _vm.IsListofBundleVisible = false;
	            Amount.IsEnabled = true;
            }
	    }

	    public async Task  SetServices()
	    {
	        try
	        {
	            Category.ItemsSource = _vm.Categories;

	            providers = await _vm.SetServiceProviders();
	            if (providers.Any())
	            {
	                var listOfProviders = new List<string>();
	                providers.ForEach(p => { listOfProviders.Add(p.MobileNetwork); });
	                Providers.ItemsSource = listOfProviders;
	            }
	            else
	            {
	                var noProviderList = new List<string> { "No NetWork Providers" };
	                Providers.ItemsSource = noProviderList;
	            }

	            if (GlobalStaticFields.Customer != null )
	            {
	                if (GlobalStaticFields.Customer.ListOfAllAccounts != null)
	                {
	                    var accountPreview = new List<string>();
	                    GlobalStaticFields.Customer.ListOfAllAccounts.ForEach(a => { accountPreview.Add(a.AccountBalanceAccountType); });
	                    DebitAccount.ItemsSource = accountPreview;
                    }
	                
	            }
	            else
	            {
	                DebitAccount.IsEnabled = false;
                    DebitAccount.SelectedItem = "No Account to Debit";
	                ContinueButton.IsEnabled = false;
	            }
            }
	        catch (Exception e)
	        {
	            await BusinessLogic.Log(e.Message, "On Page", string.Empty, "Setting Up services", string.Empty, "AirtimeData Page");
            }
	       
	    }

	    private void Amount_OnTextChanged(object sender, TextChangedEventArgs e)
	    {
            _vm.EnteredAmount = Amount.Text;
	    }

	    private void ExtendedEntry_OnTextChanged(object sender, TextChangedEventArgs e)
	    {
            
            _vm.EnteredPhoneNumber = BeneficiaryNumber.Text;

	    }

	   

        private void PhoneBook_SelectedPhone(object sender, string e)
        {
            if (e == null) return;
            BeneficiaryNumber.Text = e;
        }

        private async void Providers_OnSelectedIndexChanged(object sender, EventArgs e)
	    {
	        if (providers.Any())
	        {
	            _vm.SelectedProvider = providers[Providers.SelectedIndex]?.NetworkID;
            }

            //Get Data Bundle Items
	        var provider = Providers.SelectedItem;
	        var providerId = _vm.DataProviders.FirstOrDefault(r => r.Name.ToLower().Contains(provider.ToLower()))?.ID;
            //Get Items
	        AvailableBundle.SelectedItem = "Getting Data Bundles....";
	        AvailableBundle.IsEnabled = false;
            var response = await _vm.GetDataItems(providerId);
	        if (!response.Any()) return;
	        var bundles = new List<string>();
	        response.ForEach(p => {bundles.Add(p.Name);});
	        AvailableBundle.ItemsSource = bundles;
	        AvailableBundle.IsEnabled = true;
	        AvailableBundle.SelectedItem = "Select a Data Bundle";
        }

	    private async void AvailableBundle_OnSelectedIndexChanged(object sender, EventArgs e)
	    {
	        try
	        {
	            _vm.PaymentCode = _vm.DataBunbleItems[AvailableBundle.SelectedIndex].PaymentCode;
	            Amount.Text = $"{_vm.DataBunbleItems[AvailableBundle.SelectedIndex].Amount}";
	            // _vm.EnteredAmount = _vm.DataBunbleItems[AvailableBundle.SelectedIndex].Amount;
	        }
	        catch (Exception exception)
	        {
	            await BusinessLogic.Log(exception.Message, "On Page", string.Empty, "Setting Data Amount and crud.", string.Empty, "AirtimeData Page");
            }
	    }

	    private void DebitAccount_OnSelectedIndexChanged(object sender, EventArgs e)
	    {
	        if (GlobalStaticFields.Customer.ListOfAllAccounts != null && GlobalStaticFields.Customer.ListOfAllAccounts?.Count > 0)
	        {
	            _vm.SelectedAccount = GlobalStaticFields.Customer.ListOfAllAccounts[DebitAccount.SelectedIndex].nuban;
                var accountbalace = GlobalStaticFields.Customer.ListOfAllAccounts[DebitAccount.SelectedIndex].balance;
                var currency = GlobalStaticFields.Customer.ListOfAllAccounts[DebitAccount.SelectedIndex].currencyCode;
                _vm.Balance =$"Balance {Utilities.GetCurrency(currency)} {accountbalace}";
               
	        }
           
        }

	    private void BeneficiaryNumber_OnIconTapped(object sender, EventArgs e)
	    {
	        var phoneBook = new PhonebookList();
	        phoneBook.SelectedPhone += PhoneBook_SelectedPhone;
	        Navigation.PushAsync(phoneBook, true);
        }
	}
}