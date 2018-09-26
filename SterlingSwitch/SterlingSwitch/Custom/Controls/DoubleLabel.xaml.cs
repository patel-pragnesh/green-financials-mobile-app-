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
	public partial class DoubleLabel : ContentView
	{
		public DoubleLabel ()
        {
            InitializeComponent();
            initializeProperty();
        }

        private void initializeProperty()
        {
            LefContent.Text = LeftText;
            RightContent.Text = RightText;
            LefContent.FontSize = 17;
            RightContent.FontSize = 17;
            LefContent.TextColor = Color.FromHex("#545454");
            RightContent.TextColor = Color.FromHex("#545454");
            xLineColor.BackgroundColor = Color.FromHex("#c7c7cc");
           // xLineColor.IsVisible = true;
        }

        #region BindableProperties

        public static readonly BindableProperty LeftTextProperty = BindableProperty.Create(nameof(LeftText), typeof(string), typeof(DoubleLabel),
            defaultValue: default(string),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnLeftTextChanged);

        private static void OnLeftTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (DoubleLabel)bindable;
            obj.LefContent.Text = (string)newValue;
        }

        public string LeftText
        {
            get { return (string)GetValue(LeftTextProperty); }
            set { SetValue(LeftTextProperty, value); }
        }


        public static readonly BindableProperty RightTextProperty = BindableProperty.Create(nameof(RightText), typeof(string), typeof(DoubleLabel), 
            defaultValue: default(string), defaultBindingMode
            : BindingMode.TwoWay,
            propertyChanged:OnrightTextChanged);

        private static void OnrightTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (DoubleLabel)bindable;
            obj.RightContent.Text = (string)newValue;
        }

        public string RightText
        {
            get { return (string)GetValue(RightTextProperty); }
            set { SetValue(RightTextProperty, value); }
        }


        public static readonly BindableProperty LineColorProperty = BindableProperty.Create(nameof(LineColor), typeof(Color), typeof(DoubleLabel),
            defaultValue: default(Color),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:onLineColorChanged);

        private static void onLineColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (DoubleLabel)bindable;
            obj.xLineColor.Color = (Color)newValue;
        }

        public Color LineColor
        {
            get { return (Color)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }


        public static readonly BindableProperty RightTextColorProperty = BindableProperty.Create(nameof(RightTextColor), typeof(Color), typeof(DoubleLabel), 
            defaultValue: default(Color), 
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnRightTextColorChanged);

        private static void OnRightTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (DoubleLabel)bindable;
            obj.RightContent.TextColor = (Color)newValue;
        }

        public Color RightTextColor
        {
            get { return (Color)GetValue(RightTextColorProperty); }
            set { SetValue(RightTextColorProperty, value); }
        }


        public static readonly BindableProperty LeftTextColorProperty = BindableProperty.Create(nameof(LeftTextColor), typeof(Color), typeof(DoubleLabel), 
            defaultValue: default(Color),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnLeftTextColorChanged);

        private static void OnLeftTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (DoubleLabel)bindable;
            obj.LefContent.TextColor = (Color)newValue;
        }

        public Color LeftTextColor
        {
            get { return (Color)GetValue(LeftTextColorProperty); }
            set { SetValue(LeftTextColorProperty, value); }
        }




        public static readonly BindableProperty IsLineVisibleProperty = BindableProperty.Create(nameof(IsLineVisible), typeof(bool), typeof(DoubleLabel),
            defaultValue: true, defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnLinevisibleChanged);

        private static void OnLinevisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (DoubleLabel)bindable;
            obj.xLineColor.IsVisible = (bool)newValue;
        }

        public bool IsLineVisible
        {
            get { return (bool)GetValue(IsLineVisibleProperty); }
            set { SetValue(IsLineVisibleProperty, value); }
        }


        public static readonly BindableProperty LeftTextFontSizeProperty = BindableProperty.Create(nameof(LeftTextFontSize), typeof(double), typeof(DoubleLabel), 
            defaultValue: default(double),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnLeftTextFontSizeChanged);

        private static void OnLeftTextFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (DoubleLabel)bindable;
            obj.LefContent.FontSize = (double)newValue;
        }

        public double LeftTextFontSize
        {
            get { return (double)GetValue(LeftTextFontSizeProperty); }
            set { SetValue(LeftTextFontSizeProperty, value); }
        }


        public static readonly BindableProperty RightTextFontSizeProperty = BindableProperty.Create(nameof(RightTextFontSize), typeof(double), typeof(DoubleLabel), 
            defaultValue: default(double), 
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnRightTextFontSizeChanged);

        private static void OnRightTextFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (DoubleLabel)bindable;
            obj.RightContent.FontSize = (double)newValue;
        }

        public double RightTextFontSize
        {
            get { return (double)GetValue(RightTextFontSizeProperty); }
            set { SetValue(RightTextFontSizeProperty, value); }
        }



        #endregion BindableProperties
    }
}