using SterlingSwitch.Pages.Profile.ViewModel;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Profile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PersonalDetailsPage : SwitchMasterPage
    {
        PersonalDetailsPageViewModel vm;
        public PersonalDetailsPage ()
		{
			InitializeComponent ();
            vm = new PersonalDetailsPageViewModel(Navigation);
            BindingContext = vm;
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
    }
}