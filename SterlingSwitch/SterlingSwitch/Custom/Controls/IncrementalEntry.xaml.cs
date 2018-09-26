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
	public partial class IncrementalEntry : ContentView
	{
		public IncrementalEntry ()
		{
			InitializeComponent ();
            this.Text = this.Minimum.ToString();
		}

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public static BindableProperty TextProperty = BindableProperty.Create(
                                                            propertyName: "TextProperty",
                                                            returnType: typeof(string),
                                                            declaringType: typeof(IncrementalEntry),
                                                            defaultValue: string.Empty,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: TextPropertyChanged);


        public static BindableProperty IncrementProperty = BindableProperty.Create(
                                                            propertyName: "IncrementProperty",
                                                            returnType: typeof(int),
                                                            declaringType: typeof(IncrementalEntry),
                                                            defaultValue: 50000,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: IncrementPropertyChanged);

        public static BindableProperty MinimumProperty = BindableProperty.Create(
                                                            propertyName: "MinimumProperty",
                                                            returnType: typeof(int),
                                                            declaringType: typeof(IncrementalEntry),
                                                            defaultValue: 100000,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: MinimumPropertyChanged);

        private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedEntry)bindable;

            if (control != null)
            {
                control.Text = (string)newValue;
            }
        }

        private static void IncrementPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (IncrementalEntry)bindable;

            if (control != null)
            {
                control.Increment = (int)newValue;
            }
        }

        private static void MinimumPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (IncrementalEntry)bindable;

            if (control != null)
            {
                control.Increment = (int)newValue;
            }
        }

        public int Increment
        {
            get { return (int)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public string Text
        {
            get
            {
                string txt = AmountToInvestTxt.Text;

                if (!string.IsNullOrWhiteSpace(txt))
                    txt = txt.Replace(",", "");

                return txt;
            }
            set
            {
                string s = value.Replace(",", "");

                if (double.TryParse(s, out double result) && s.Length > 3)
                    AmountToInvestTxt.Text = String.Format("{0:0,0}", result); //result.ToString("0:0,0");
                else
                    AmountToInvestTxt.Text = s;
            }
        }

       
        private void minus_Tapped(object sender, EventArgs e)
        {
            if (int.TryParse(this.Text, out int amt))
            {
                int newValue = amt - Increment;

                if (newValue < this.Minimum)
                    newValue = this.Minimum;

                this.Text = newValue.ToString();
            }
        }

        private void plus_Tapped(object sender, EventArgs e)
        {
            if (int.TryParse(this.Text, out int amt))
            {
                int newValue = amt + Increment;
                
                this.Text = newValue.ToString();
            }
        }

        private void AmountToInvestTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var tempValue = e.NewTextValue.Replace(",", "");

            //if (int.TryParse(tempValue, out int result) && result >= this.Minimum)
            //    this.Text = e.NewTextValue; 
            //else
            //    this.Text = this.Minimum.ToString();

            this.Text = e.NewTextValue;
            var args = new TextChangedEventArgs(e.OldTextValue, e.NewTextValue);
            TextChanged?.Invoke(sender, args);
        }
    }
}