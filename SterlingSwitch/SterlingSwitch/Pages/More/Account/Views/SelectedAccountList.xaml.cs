using SterlingSwitch.Pages.More.Account.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.More.Account.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectedAccountList : ContentView
	{
        SelectedAccountViewModel vm;
        public SelectedAccountList ()
		{
			InitializeComponent ();
		}
	}
}