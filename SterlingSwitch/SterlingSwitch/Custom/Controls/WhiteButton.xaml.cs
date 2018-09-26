using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Custom.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WhiteButton : ContentView
	{
		public WhiteButton ()
		{
			InitializeComponent ();
            SetTap();
           // this.BindingContext = this;
            LabelTxt.Text = Text;
        }

        public event EventHandler Clicked;

        public static BindableProperty TextProperty = BindableProperty.Create(
                                                           propertyName: nameof(Text),
                                                           returnType: typeof(string),
                                                           declaringType: typeof(WhiteButton),
                                                           defaultValue: string.Empty,
                                                           defaultBindingMode: BindingMode.TwoWay,
                                                           propertyChanged: TextPropertyChanged);

        private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (WhiteButton)bindable;

            if (control != null || newValue != null)
            {
                control.Text = (string)newValue;
                control.LabelTxt.Text = (string)newValue;
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private void SetTap()
        {
            TapGestureRecognizer tgr = new TapGestureRecognizer();
            tgr.NumberOfTapsRequired = 1;
            tgr.Tapped += (s, e) =>
            {
                Clicked?.Invoke(this, new EventArgs());
            };

            this.GestureRecognizers.Add(tgr);
        }
    }
}