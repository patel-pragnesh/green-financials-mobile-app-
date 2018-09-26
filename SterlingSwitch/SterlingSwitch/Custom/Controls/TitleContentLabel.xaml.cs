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
	public partial class TitleContentLabel : ContentView
	{
		public TitleContentLabel ()
		{
			InitializeComponent ();
            InitializeProperties();
		}

        void InitializeProperties()
        {
            lblHeader.Text = HeaderText;
            lblContent.Text = ContentText;
            lblHeader.TextColor = HeaderTextColor;
            lblContent.TextColor = ContentTextColor;
            bxLine.BackgroundColor = LineColor;
            lblContent.LineBreakMode = LineBreakMode;
        }


        public static readonly BindableProperty HeaderTextProperty = BindableProperty.Create(nameof(HeaderText), typeof(string), typeof(TitleContentLabel), 
            defaultValue: default(string),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnHeaderTextChanged);

        private static void OnHeaderTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (TitleContentLabel)bindable;
            obj.lblHeader.Text = (string)newValue;
        }

        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }


        public static readonly BindableProperty HeaderTextColorProperty = BindableProperty.Create(nameof(HeaderTextColor), typeof(Color), typeof(TitleContentLabel),
            defaultValue: Color.FromHex("#a7a7a7"),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnHeaderTextColorChanged);

        private static void OnHeaderTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (TitleContentLabel)bindable;
            obj.lblHeader.TextColor = (Color)newValue;
        }

        public Color HeaderTextColor
        {
            get { return (Color)GetValue(HeaderTextColorProperty); }
            set { SetValue(HeaderTextColorProperty, value); }
        }



        public static readonly BindableProperty ContentTextProperty = BindableProperty.Create(nameof(ContentText), typeof(string), typeof(TitleContentLabel), 
            defaultValue: default(string), 
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnContextTextChanged);

        private static void OnContextTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (TitleContentLabel)bindable;
            obj.lblContent.Text = (string)newValue;
        }

        public string ContentText
        {
            get { return (string)GetValue(ContentTextProperty); }
            set { SetValue(ContentTextProperty, value); }
        }


        public static readonly BindableProperty ContentTextColorProperty = BindableProperty.Create(nameof(ContentTextColor), typeof(Color), typeof(TitleContentLabel),
            defaultValue: Color.FromHex("#4a4a4a"), 
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnContentTextColorChanged);

        private static void OnContentTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (TitleContentLabel)bindable;
            obj.lblContent.TextColor = (Color)newValue;
        }

        public Color ContentTextColor
        {
            get { return (Color)GetValue(ContentTextColorProperty); }
            set { SetValue(ContentTextColorProperty, value); }
        }


        public static readonly BindableProperty LineColorProperty = BindableProperty.Create(nameof(LineColor), typeof(Color), typeof(TitleContentLabel),
            defaultValue: Color.FromHex("#C7C7CC"), 
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnLineColorChanged);

        private static void OnLineColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (TitleContentLabel)bindable;
            obj.bxLine.BackgroundColor = (Color)newValue;
        }

        public Color LineColor
        {
            get { return (Color)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }



        public static readonly BindableProperty LineBreakModeProperty = BindableProperty.Create(nameof(LineBreakMode), typeof(LineBreakMode), typeof(TitleContentLabel), 
            defaultValue: LineBreakMode.NoWrap, 
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnLineBreakModeChanged);

        private static void OnLineBreakModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null)
                return;

            var obj = (TitleContentLabel)bindable;
            obj.lblContent.LineBreakMode = (LineBreakMode)newValue;
        }

        public LineBreakMode LineBreakMode
        {
            get { return (LineBreakMode)GetValue(LineBreakModeProperty); }
            set { SetValue(LineBreakModeProperty, value); }
        }



    }
}