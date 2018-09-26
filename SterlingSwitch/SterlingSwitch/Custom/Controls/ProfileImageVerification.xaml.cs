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
	public partial class ProfileImageVerification : ContentView
	{
		public ProfileImageVerification ()
		{
			InitializeComponent ();
            initializeProperty();
        }

        private void initializeProperty()
        {
            lblheaderText.Text = HeaderText;
            lblsubheaderText.Text = SubHeaderText;
            lblheaderText.TextColor = HeaderTextColor;
            lblsubheaderText.TextColor = SubHeaderTextColor;
            frmIndicator.BackgroundColor = FrameColor;
        }

        #region BindableProperties

       

        public static readonly BindableProperty HeaderTextProperty = BindableProperty.Create(nameof(HeaderText), typeof(string), typeof(ProfileImageVerification), 
            defaultValue: default(string), 
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnHeaderTextChanged);

        private static void OnHeaderTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (ProfileImageVerification)bindable;
            obj.lblheaderText.Text = (string)newValue;
        }

        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }


        public static readonly BindableProperty SubHeaderTextProperty = BindableProperty.Create(nameof(SubHeaderText), 
            typeof(string), typeof(ProfileImageVerification), 
            defaultValue: default(string), 
            defaultBindingMode: BindingMode.TwoWay,propertyChanged:OnSubHeaderTextChanged);

        private static void OnSubHeaderTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (ProfileImageVerification)bindable;
            obj.lblsubheaderText.Text = (string)newValue;
        }

        public string SubHeaderText
        {
            get { return (string)GetValue(SubHeaderTextProperty); }
            set { SetValue(SubHeaderTextProperty, value); }
        }


        public static readonly BindableProperty HeaderTextColorProperty = BindableProperty.Create(nameof(HeaderTextColor), typeof(Color), typeof(ProfileImageVerification), 
            defaultValue: Color.FromHex("#4a4a4a"), 
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnHeaderTextColorChanged);

        private static void OnHeaderTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (ProfileImageVerification)bindable;
            obj.lblheaderText.TextColor = (Color)newValue;
        }

        public Color HeaderTextColor
        {
            get { return (Color)GetValue(HeaderTextColorProperty); }
            set { SetValue(HeaderTextColorProperty, value); }
        }


        public static readonly BindableProperty SubHeaderTextColorProperty = BindableProperty.Create(nameof(SubHeaderTextColor), typeof(Color),
            typeof(ProfileImageVerification), defaultValue: Color.FromHex("#1f3958"), 
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnSubHeaderTextColorChanged);

        private static void OnSubHeaderTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (ProfileImageVerification)bindable;
            obj.lblsubheaderText.TextColor = (Color)newValue;
        }

        public Color SubHeaderTextColor
        {
            get { return (Color)GetValue(SubHeaderTextColorProperty); }
            set { SetValue(SubHeaderTextColorProperty, value); }
        }


        public static readonly BindableProperty FrameColorProperty = BindableProperty.Create(nameof(FrameColor),
            typeof(Color), typeof(ProfileImageVerification),
            defaultValue: Color.FromHex("#d8d8d8"), defaultBindingMode: BindingMode.TwoWay
            ,propertyChanged:OnFrameColorChanged);

        private static void OnFrameColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (ProfileImageVerification)bindable;
            obj.frmIndicator.BackgroundColor = (Color)newValue;
        }

        public Color FrameColor
        {
            get { return (Color)GetValue(FrameColorProperty); }
            set { SetValue(FrameColorProperty, value); }
        }




        #endregion BindableProperties
    }
}