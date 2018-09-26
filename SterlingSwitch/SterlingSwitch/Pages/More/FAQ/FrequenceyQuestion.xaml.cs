using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.More.FAQ
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FrequenceyQuestion : SwitchMasterPage
	{
        FAQViewModel vm;
		public FrequenceyQuestion ()
		{
			InitializeComponent ();
            this.BindingContext = vm = new FAQViewModel(Navigation);
		}

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var FAQs = e.Item as FAQ;

            vm? .ShowHiddenFAQS(FAQs);
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (vm == null)
                vm = new FAQViewModel(Navigation);

            await vm.LoadFAQs();
        }
    }
}