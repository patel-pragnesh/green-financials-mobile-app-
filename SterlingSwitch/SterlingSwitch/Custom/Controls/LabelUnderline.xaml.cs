using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Custom.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LabelUnderline : ContentView
	{
		public LabelUnderline ()
		{
			InitializeComponent ();
            initializeProperty();
        }

        private void initializeProperty()
        {
            xLineColor.BackgroundColor = Color.Black;
            ArrowImage.IsVisible = IsArrowVisible;
            ArrowImage.Source = ImageSource;
        }


        #region BindableProperties

        public static readonly BindableProperty BoxViewColorProperty = BindableProperty.Create(nameof(BoxViewColor), typeof(Color),
            typeof(LabelUnderline), defaultValue: Color.Default,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnBoxViewColorChanged);

        private static void OnBoxViewColorChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }

        public Color BoxViewColor
        {
            get { return (Color)GetValue(BoxViewColorProperty); }
            set { SetValue(BoxViewColorProperty, value); }
        }


        public static readonly BindableProperty HeaderTextProperty = BindableProperty.Create(nameof(HeaderText), typeof(string), typeof(LabelUnderline), defaultValue: string.Empty, defaultBindingMode: BindingMode.TwoWay);

        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }
        public bool IsArrowVisible
        {
            get { return (bool)GetValue(IsArrowVisibleProperty); }
            set { SetValue(IsArrowVisibleProperty, value); }
        }
        public static readonly BindableProperty IsArrowVisibleProperty = BindableProperty.Create(nameof(IsArrowVisible), typeof(bool), typeof(LabelUnderline), defaultValue: false, defaultBindingMode: BindingMode.TwoWay,propertyChanged: IsArrowVisiblePropertyChanged);

        private static void IsArrowVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue==newValue)
            {
                return;
            }
            if (newValue!=null)
            {
                var ss = (LabelUnderline)bindable;
                ss.ArrowImage.IsVisible = (bool)newValue;
            }

        }

        public static readonly BindableProperty SubHeaderTextProperty = BindableProperty.Create(nameof(SubHeaderText), typeof(string), typeof(LabelUnderline), defaultValue: string.Empty, defaultBindingMode: BindingMode.TwoWay);

        public string SubHeaderText
        {
            get { return (string)GetValue(SubHeaderTextProperty); }
            set { SetValue(SubHeaderTextProperty, value); }
        }


        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(LabelUnderline), defaultValue: ImageSource.FromFile("arrowDown.png"), defaultBindingMode: BindingMode.TwoWay);

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }


        public static readonly BindableProperty LableLineColorProperty = BindableProperty.Create(nameof(LableLineColor), typeof(Color), typeof(LabelUnderline), defaultValue: default(Color), defaultBindingMode: BindingMode.TwoWay
            ,propertyChanged:OnLaelLineColorChanged);

        private static void OnLaelLineColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (LabelUnderline)bindable;
            obj.xLineColor.Color = (Color)newValue;
        }

        public Color LableLineColor
        {
            get { return (Color)GetValue(LableLineColorProperty); }
            set { SetValue(LableLineColorProperty, value); }
        }




        #endregion

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
         
            if (propertyName == HeaderTextProperty.PropertyName)
            {
                lblHeader.Text = HeaderText;
            }
            if (propertyName == SubHeaderTextProperty.PropertyName)
            {
                lblSubHeader.Text = SubHeaderText;
            }
            if (propertyName == ImageSourceProperty.PropertyName)
            {
                ArrowImage.Source = ImageSource;
            }
        }
    }
}