using SterlingSwitch.Custom.Controls;
using SterlingSwitch.Pages.CardlessWithdrawals.ViewModel;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SterlingSwitch.Models.SomeConstant;

namespace SterlingSwitch.Pages.CardlessWithdrawals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardlessWithdrawView : SwitchMasterPage
    {
        CardlessWithdrawViewModel vm;
        public CardlessWithdrawView()
        {
            InitializeComponent();
            vm = new CardlessWithdrawViewModel(navigation: Navigation);
            BindingContext = vm;
            vm.PhoneNumber = GlobalStaticFields.Customer.PhoneNumber;
            epaccount.RefreshContent = RefreshAccountsPicker;
            epwithdrawal.RefreshContent = RefreshWithdrawalPicker;
            vm.CardLessPage = this;

            // this.gettype().name gets the file type name, taking it as page name.
            BusinessLogic.LogFrequentPage(this.GetType().Name,PageAliasConstant.cardlesswithdrawal,ImageConstants.WithdrawalIcon);
         
        }

        protected void RefreshAccountsPicker()
        {
            List<string> accounts = GlobalStaticFields.Customer.ListOfAllAccounts.Select(x => x.AccountNumberWithBalance).ToList();
            epaccount.ItemsSource = accounts;
        }
        protected void RefreshWithdrawalPicker()
        {
            epwithdrawal.ItemsSource = vm.WithdrawalTypes;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            epwithdrawal.ItemsSource = vm.WithdrawalTypes;
            List<string> accounts = GlobalStaticFields.Customer.ListOfAllAccounts.Select(x => x.AccountNumberWithBalance).ToList();
            epaccount.ItemsSource = accounts;
        }

        private void epwithdrawal_SelectedIndexChanged(object sender, EventArgs e)
        {
            var obj = (ExtendedPicker)sender;
            if (obj.SelectedItem == "Self")
            {
                vm.PhoneNumber = GlobalStaticFields.Customer.PhoneNumber;
                vm.PhoneNumberTitle = "Phone number";
                vm.IsRecipientVisible = false;

            }
            else
            {
                vm.PhoneNumber = "";
                vm.PhoneNumberTitle = "Recipients Phone number";
                vm.IsRecipientVisible = true;
            }
        }

        private void epaccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            var obj = (ExtendedPicker)sender;
            vm.FromAccount = obj.SelectedItem;
        }

        protected async void Continue(object sender, EventArgs e)
        {

            await vm.ExecuteCardLess();

            //var title = epwithdrawal.SelectedItem == "Self" ? "Self" : vm.Recipient;
            //    var tpinPage = new PopUps.VerifyPinPage(title, vm.Amount, "NGN", vm.Reference);

            //    tpinPage.Validated += TpinPage_Validated;

            //    Navigation.PushAsync(tpinPage);
           
        }

        private void TpinPage_Validated(object sender, bool e)
        {
            //await vm.ExecuteCardLess();
        }
    }
}