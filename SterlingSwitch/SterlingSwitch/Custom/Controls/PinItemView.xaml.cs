using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Custom.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PinItemView : ContentView
	{
		public PinItemView ()
		{
			InitializeComponent ();
            PinTxt.BindingContext = this;
            SetTap();
        }

        public event EventHandler Clicked;

        public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(PinItemView), null);


        /// <summary>
        /// Gets or sets the item text.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

      

        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(PinItemView), null);

        /// <summary>
        /// Gets or sets the item command.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(PinItemView), null);

        /// <summary>
        /// Gets or sets the item command parameter.
        /// </summary>
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
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