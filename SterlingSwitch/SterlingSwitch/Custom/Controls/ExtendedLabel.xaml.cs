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
	public partial class ExtendedLabel : ContentView
	{
		public ExtendedLabel ()
		{
			InitializeComponent ();
            var tapped = new TapGestureRecognizer() { NumberOfTapsRequired = 1 };
            tapped.Tapped += Tapped_Tapped;
            this.GestureRecognizers.Add(tapped);

            InitializeControls();
		}

        #region Methods
        private void Tapped_Tapped(object sender, EventArgs e)
        {
            var obj = (ExtendedLabel)sender;

            var ev = new ExtendedLabelTappedEvent();
            ev.Text = obj.Text;
            ItemTapped?.Invoke(sender, ev);
        }
        void InitializeControls()
        {
            bxLine.BackgroundColor = LineColor;
            lblText.Text = Text;
            lblText.TextColor = TextColor;
            contentGrid.Padding = ContentPadding;
            bxLine.Margin = LineMargin;
            bxLine.IsVisible = IsLineVisible;
        }
        #endregion
        #region Events
        public event EventHandler<ExtendedLabelTappedEvent> ItemTapped;

        #endregion

        #region BindableProperties


        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ExtendedLabel), 
            defaultValue: default(string), 
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnTextChanged);

        private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (ExtendedLabel)bindable;
            obj.lblText.Text = newValue as string;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ExtendedLabel),
            defaultValue: Color.FromHex("#545454"), 
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnTextColorChanged);

        private static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (ExtendedLabel)bindable;
            obj.lblText.TextColor = (Color)newValue;
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }


        public static readonly BindableProperty LineColorProperty = BindableProperty.Create(nameof(LineColor), typeof(Color), typeof(ExtendedLabel),
            defaultValue: Color.FromHex("#C7C7CC"), 
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnLineColorChanged);

        private static void OnLineColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (ExtendedLabel)bindable;
            obj.bxLine.BackgroundColor = (Color)newValue;
        }

        public Color LineColor
        {
            get { return (Color)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        public static readonly BindableProperty IsLineVisibleProperty = BindableProperty.Create(nameof(IsLineVisible), typeof(bool), typeof(ExtendedLabel),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnIsLineVisibleChanged);

        private static void OnIsLineVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;

            var obj = (ExtendedLabel)bindable;
            obj.bxLine.IsVisible = (bool)newValue;
        }

        public bool IsLineVisible
        {
            get { return (bool)GetValue(IsLineVisibleProperty); }
            set { SetValue(IsLineVisibleProperty, value); }
        }

        public static readonly BindableProperty ContentPaddingProperty = BindableProperty.Create(nameof(ContentPadding), typeof(Thickness), typeof(ExtendedLabel), defaultValue: default(Thickness),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnContentPaddingChanged);

        private static void OnContentPaddingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            var obj = (ExtendedLabel)bindable;
            obj.contentGrid.Padding = (Thickness)newValue;
        }

        public Thickness ContentPadding
        {
            get { return (Thickness)GetValue(ContentPaddingProperty); }
            set { SetValue(ContentPaddingProperty, value); }
        }


        public static readonly BindableProperty LineMarginProperty = BindableProperty.Create(nameof(LineMargin), typeof(Thickness), typeof(ExtendedLabel), defaultValue: default(Thickness), defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnLineMarginChanged);

        private static void OnLineMarginChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            var obj = (ExtendedLabel)bindable;
            obj.bxLine.Margin = (Thickness)newValue;
        }

        public Thickness LineMargin
        {
            get { return (Thickness)GetValue(LineMarginProperty); }
            set { SetValue(LineMarginProperty, value); }
        }
        
        #endregion
    }

    public class ExtendedLabelTappedEvent : EventArgs
    {
        public string Text { get; set; }
    }
}