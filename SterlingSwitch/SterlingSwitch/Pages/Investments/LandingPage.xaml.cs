using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SterlingSwitch.Pages.Investments.ViewModel;
using SterlingSwitch.Pages.Pagelanding;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SterlingSwitch.Models.SomeConstant;

namespace SterlingSwitch.Pages.Investments
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LandingPage : SwitchMasterPage
    {
	    private LandingPageViewModel _vm;
        public LandingPage ()
		{
			InitializeComponent ();
		    BindingContext = _vm = new LandingPageViewModel(Navigation);

            //get exact name of this present file as a page
            var x = (this.GetType().Name);
            //logic here
            BusinessLogic.LogFrequentPage(x, PageAliasConstant.InvestmentPage, ImageConstants.InvestmentIcon);
        }

        private void GetStarted_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InvestmentLanding(), true);
        }
    }
}