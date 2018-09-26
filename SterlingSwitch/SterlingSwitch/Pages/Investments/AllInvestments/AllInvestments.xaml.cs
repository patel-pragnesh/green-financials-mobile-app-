using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SterlingSwitch.Pages.Investments.ViewModel;
using SterlingSwitch.Templates;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts;
using SkiaSharp;
using SterlingSwitch.PopUps;

namespace SterlingSwitch.Pages.Investments.AllInvestments
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AllInvestments : SwitchMasterPage
    {
        private AllInvestmentViewModel _vm;
        public AllInvestments ()
		{
			InitializeComponent ();
            BindingContext = _vm = new AllInvestmentViewModel(Navigation);
            DefaultSelectedTab();
		    SetChart();
            GetAllIvestment();
        }


        async void GetAllIvestment()
        {
            var pd = await ProgressDialog.Show("Fetching your investments...");
            try
            {

                await _vm.SetRunningFixedDeposits();
                await _vm.SetRunningTreasuryBills();
                await pd.Dismiss();
            }
            catch (Exception ex)
            {
                await pd.Dismiss();
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
           
        }

        private void SfTabView_TabItemTapped(object sender, Syncfusion.XForms.TabView.TabItemTappedEventArgs e)
        {

        }

        private void SfTabView_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            
        }

        private void SetChart()
        {
            var entries = new[]
            {
                new Microcharts.Entry(216)
                {
                    Color = SKColor.Parse("#7ed321")
                },
                new Microcharts.Entry(144)
                {
                    Color = SKColor.Parse("#e7e7e7")
                } };

            var chart = new DonutChart() { Entries = entries, HoleRadius = 0.9f, MaxValue = 360, BackgroundColor = SKColor.Parse("#FFFFFF"), MinValue = 0};
           this.chartView.Chart = chart;
        }

        private void Fixed_DepositTapped(object sender, EventArgs e)
        {
            var obj = (StackLayout)sender;
            tabView.SelectedIndex = 0;
            ResetSelectedTab();
          //  Resources["fixed"] = Resources["Selected"];
            bxfixedDepo.BackgroundColor = (Color)Application.Current.Resources["SelectedTab"];
        }
        private void Treasury_Tapped(object sender, EventArgs e)
        {
            var obj = (StackLayout)sender;
            tabView.SelectedIndex = 1;
            ResetSelectedTab();
           // Resources["treasury"] = Resources["Selected"];
            bxTreasury.BackgroundColor = (Color)Application.Current.Resources["SelectedTab"];
        }

        void ResetSelectedTab()
        {
           // Resources["fixed"] = Resources["UnSelectedTab"];
           // Resources["treasury"] = Resources["UnSelectedTab"];
            bxfixedDepo.BackgroundColor = (Color)Application.Current.Resources["UnSelectedTab"];
            bxTreasury.BackgroundColor = (Color)Application.Current.Resources["UnSelectedTab"];
        }

        void DefaultSelectedTab()
        {
            tabView.SelectedIndex = 0;
            ResetSelectedTab();
           // Resources["fixed"] = Resources["Selected"];
            if (tabView.SelectedIndex == 0)
            {
                bxfixedDepo.BackgroundColor = (Color)Application.Current.Resources["SelectedTab"];
            }
        }
    }
}