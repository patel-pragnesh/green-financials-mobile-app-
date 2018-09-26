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
	public partial class OptionCell : ContentView
	{
		public OptionCell ()
		{
			InitializeComponent ();

            img.Source = ImageSource.FromFile("forwardarrow.png");
        }

        private void Tapped_Tapped(object sender, EventArgs e)
        {
            var obj = (OptionCell)sender;
            var optioncelleventargs = new OptionCellEventArgs
            {
                HeaderText = obj.HeaderText,
                SubHeaderText = obj.SubHeaderText
            };
            ItemTapped?.Invoke(obj, optioncelleventargs);
        }

        public event EventHandler<OptionCellEventArgs> ItemTapped;

        #region BindableProperties

        public static readonly BindableProperty BoxViewColorProperty = BindableProperty.Create(nameof(BoxViewColor), typeof(Color), 
            typeof(OptionCell), defaultValue: Color.Default,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnBoxViewColorChanged);

        private static void OnBoxViewColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
           
        }

        public Color BoxViewColor
        {
            get { return (Color)GetValue(BoxViewColorProperty); }
            set { SetValue(BoxViewColorProperty, value); }
        }


        public static readonly BindableProperty HeaderTextProperty = BindableProperty.Create(nameof(HeaderText), typeof(string), typeof(OptionCell), defaultValue: string.Empty, defaultBindingMode: BindingMode.TwoWay);

        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }


        public static readonly BindableProperty SubHeaderTextProperty = BindableProperty.Create(nameof(SubHeaderText), typeof(string), typeof(OptionCell), defaultValue: string.Empty, defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty IsBoxLineViewVisibleProperty = BindableProperty.Create(nameof(isBoxViewLineVisible), typeof(bool), typeof(OptionCell), defaultValue: true, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnIsBoxViewLineVisibleChanged);

        private static void OnIsBoxViewLineVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue==null)
            {
                return;
            }
            var d = (OptionCell)bindable;
            d.bxVwLine.IsVisible =(bool)newValue;
        }

        public string SubHeaderText
        {
            get { return (string)GetValue(SubHeaderTextProperty); }
            set { SetValue(SubHeaderTextProperty, value); }
        }

        public bool isBoxViewLineVisible
        {
            get {return(bool) GetValue(IsBoxLineViewVisibleProperty) ; }
            set{ SetValue(IsBoxLineViewVisibleProperty,value);}
        }

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(OptionCell), defaultValue: ImageSource.FromFile("forwardarrow.png"), defaultBindingMode: BindingMode.TwoWay);

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }




        #endregion

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if(propertyName == BoxViewColorProperty.PropertyName)
            {
                bxv.BackgroundColor = BoxViewColor;
            }
            if(propertyName == HeaderTextProperty.PropertyName)
            {
                lblHeader.Text = HeaderText;
            }
            if(propertyName == SubHeaderTextProperty.PropertyName)
            {
                lblSubHeader.Text = SubHeaderText;
            }
            if(propertyName == ImageSourceProperty.PropertyName)
            {
                img.Source = ImageSource;
            }
        }
    }

    public class OptionCellEventArgs : EventArgs
    {
        public string HeaderText { get; set; }
        public string SubHeaderText { get; set; }
    }
}