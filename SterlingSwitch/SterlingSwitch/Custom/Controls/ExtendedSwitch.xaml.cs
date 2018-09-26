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
	public partial class ExtendedSwitch : ContentView
	{
		public ExtendedSwitch ()
		{

			InitializeComponent ();
            sw.Toggled += Sw_Toggled;

            InitializeControls();
        }

        private void Sw_Toggled(object sender, ToggledEventArgs e)
        {
            var obj = (Switch)sender;

            OnToggled?.Invoke(sender, obj.IsToggled);
        }

        #region Methods
        
        void InitializeControls()
        {
            bxLine.BackgroundColor = LineColor;
            lblText.Text = Text;
            lblText.TextColor = TextColor;
            contentGrid.Padding = ContentPadding;
            bxLine.Margin = LineMargin;
        }
        #endregion
        #region Events
        public event EventHandler<bool> OnToggled;

        #endregion

        #region BindableProperties


        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ExtendedSwitch),
            defaultValue: default(string),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnTextChanged);

        private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (ExtendedSwitch)bindable;
            obj.lblText.Text = newValue as string;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ExtendedSwitch),
            defaultValue: Color.FromHex("#545454"),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnTextColorChanged);

        private static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (ExtendedSwitch)bindable;
            obj.lblText.TextColor = (Color)newValue;
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }


        public static readonly BindableProperty LineColorProperty = BindableProperty.Create(nameof(LineColor), typeof(Color), typeof(ExtendedSwitch),
            defaultValue: Color.FromHex("#C7C7CC"),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnLineColorChanged);

        private static void OnLineColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (ExtendedSwitch)bindable;
            obj.bxLine.BackgroundColor = (Color)newValue;
        }

        public Color LineColor
        {
            get { return (Color)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }


        public static readonly BindableProperty ContentPaddingProperty = BindableProperty.Create(nameof(ContentPadding), typeof(Thickness), typeof(ExtendedSwitch), defaultValue: default(Thickness),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnContentPaddingChanged);

        private static void OnContentPaddingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            var obj = (ExtendedSwitch)bindable;
            obj.contentGrid.Padding = (Thickness)newValue;
        }

        public Thickness ContentPadding
        {
            get { return (Thickness)GetValue(ContentPaddingProperty); }
            set { SetValue(ContentPaddingProperty, value); }
        }


        public static readonly BindableProperty LineMarginProperty = BindableProperty.Create(nameof(LineMargin), typeof(Thickness), typeof(ExtendedSwitch), defaultValue: default(Thickness), defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnLineMarginChanged);

        private static void OnLineMarginChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            var obj = (ExtendedSwitch)bindable;
            obj.bxLine.Margin = (Thickness)newValue;
        }

        public Thickness LineMargin
        {
            get { return (Thickness)GetValue(LineMarginProperty); }
            set { SetValue(LineMarginProperty, value); }
        }


        public static readonly BindableProperty IsToggledProperty = BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(ExtendedSwitch), defaultValue: default(bool),
            defaultBindingMode: BindingMode.TwoWay,propertyChanged:OnIsToggledChanged);

        private static void OnIsToggledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            var obj = (ExtendedSwitch)bindable;
            obj.sw.IsToggled = (bool)newValue;
        }

        public bool IsToggled
        {
            get { return (bool)GetValue(IsToggledProperty); }
            set { SetValue(IsToggledProperty, value); }
        }


        #endregion
    }
}