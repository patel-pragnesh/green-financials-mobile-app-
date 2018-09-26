using SterlingSwitch.Pages.WalletAllTransaction.ViewModel;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.WalletAllTransaction
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WalletAllTransactionPage : SwitchMasterPage
    {
        WalletAllTransactionPageViewModel vm;
        public WalletAllTransactionPage ()
		{
			InitializeComponent ();

            vm = (WalletAllTransactionPageViewModel)this.BindingContext;

            // BindingContext = vm;

            if (tabView.SelectedIndex == 0)
                Resources["accounts"] = Application.Current.Resources["SelectedTabviewWhite"];
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (vm == null)
                vm = (WalletAllTransactionPageViewModel)BindingContext;

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);
            var startDate = new DateTime(currentYear, currentMonth, 1);
            var endDate = new DateTime(currentYear, currentMonth, daysInMonth);
            vm.SelectedMonth = vm.Months[currentMonth - 1];
            await vm.GetWalletTransactions( startDate, endDate);

        }
    }
}