using SterlingSwitch.Pages.AllTransactions.ViewModel;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.AllTransactions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllTransactionsView : SwitchMasterPage
    {
        AllTransactionsViewModel vm;
        public AllTransactionsView()
        {
            InitializeComponent();
            vm = (AllTransactionsViewModel)this.BindingContext;
           
           // BindingContext = vm;

            if (tabView.SelectedIndex == 0)
                Resources["accounts"] = Application.Current.Resources["SelectedTabviewWhite"];
        }
        protected void GoBackEvent(object sender,EventArgs e)
        {
            Navigation.PopAsync();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (vm == null)
                vm = (AllTransactionsViewModel)BindingContext;

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);
            var startDate = new DateTime(currentYear, currentMonth, 1);
            var endDate = new DateTime(currentYear, currentMonth, daysInMonth);
            vm.SelectedMonth = vm.Months[currentMonth - 1];
            vm.MonthIndex = vm.Months.IndexOf(vm.SelectedMonth);
             
            await vm.GetAllTransactions(vm.SelectedAccount.nuban, vm.SelectedAccount.currencyCode, startDate, endDate);

         }



    }
}