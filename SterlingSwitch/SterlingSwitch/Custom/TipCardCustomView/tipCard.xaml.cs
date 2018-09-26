using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Custom.TipCardCustomView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class tipCard : ContentView
	{
	    public event EventHandler feedTapped;
	    public event EventHandler crossTapped;
		public tipCard ()
		{
			InitializeComponent ();
		}

	    private void Feedcard_Tapped(object sender, EventArgs e)
	    {
	        feedTapped?.Invoke(this, null);
	    }

	    private void RemoveCard_Tapped(object sender, EventArgs e)
	    {
	        crossTapped?.Invoke(this, null);
	    }
	}
}