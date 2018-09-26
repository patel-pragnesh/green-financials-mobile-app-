using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SterlingSwitch.Pages.Beneficiary.ViewModel;
using SterlingSwitch.Templates;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Beneficiary
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BeneficiaryPage : SwitchMasterPage
    {
        private BeneficiaryViewModel _vm;
        public BeneficiaryPage ()
		{
			InitializeComponent ();
            BindingContext = _vm = new BeneficiaryViewModel(Navigation);
		}
	}
}