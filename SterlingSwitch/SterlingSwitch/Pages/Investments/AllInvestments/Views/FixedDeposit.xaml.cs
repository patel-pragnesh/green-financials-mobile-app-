using SterlingSwitch.Pages.Investments.FixedDeposit;
using SterlingSwitch.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Investments.AllInvestments.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FixedDeposit : ContentView
    {
        public FixedDeposit()
        {
            InitializeComponent();
        }

        //private void DepositsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    var deposit = (FixedDepositValue) e.SelectedItem;
        //    ItemSelected = "DepositsListView_ItemSelected"
        //    Navigation.PushAsync(new TerminateFDPage(deposit), true);
        //    ((ListView) sender).SelectedItem = null;
        //}

        private void DepositsListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var deposit = (FixedDepositValue) e.Item;
            Navigation.PushAsync(new TerminateFDPage(deposit), true);
            ((ListView) sender).SelectedItem = null;
        }
    }
}