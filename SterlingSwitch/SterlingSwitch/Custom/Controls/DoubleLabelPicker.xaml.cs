using Rg.Plugins.Popup.Extensions;
using SterlingSwitch.PopUps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Custom.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DoubleLabelPicker : ContentView, INotifyPropertyChanged
    {
		public DoubleLabelPicker ()
		{
			InitializeComponent ();
            initializeProperty();
            SetUp();

            if (string.IsNullOrWhiteSpace(this.Placeholder))
                this.Placeholder = "Select an item";

            this.BindingContext = this;
        }

        private void initializeProperty()
        {
            ////LefContent.Text = LeftText;
            ////RightContent.Text = RightText;
            LeftContent.TextColor = Color.FromHex("#545454");
            xLineColor.BackgroundColor = Color.FromHex("#c7c7cc");
            RightContent.TextColor = Color.FromHex("#545454");
        }
        #region BindableProperties



        public static readonly BindableProperty LeftTextProperty = BindableProperty.Create(nameof(LeftText), typeof(string), typeof(DoubleLabelPicker), 
            defaultValue: default(string), 
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged:OnLeftTextChange);

        private static void OnLeftTextChange(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (DoubleLabelPicker)bindable;
            obj.LeftContent.Text = (string)newValue;
        }

        public string LeftText
        {
            get { return (string)GetValue(LeftTextProperty); }
            set { SetValue(LeftTextProperty, value); }
        }


        //public static readonly BindableProperty RightTextProperty = BindableProperty.Create(nameof(RightText), typeof(string), typeof(DoubleLabelPicker),
        //    defaultValue: default(string), 
        //    defaultBindingMode: BindingMode.TwoWay,
        //    propertyChanged:OnRighttextChanged);

        //private static void OnRighttextChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    var obj = (DoubleLabelPicker)bindable;
        //    obj.RightContent.Text = (string)newValue;
        //}

        //public string RightText
        //{
        //    get { return (string)GetValue(RightTextProperty); }
        //    set { SetValue(RightTextProperty, value); }
        //}



        public static readonly BindableProperty LineColorProperty = BindableProperty.Create(nameof(LineColor), typeof(Color), typeof(DoubleLabelPicker), 
            defaultValue: default(Color),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: onLineColorChanged);

        private static void onLineColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (DoubleLabelPicker)bindable;
            obj.xLineColor.Color = (Color)newValue;
        }

        public Color LineColor
        {
            get { return (Color)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        //public static readonly BindableProperty RightTextColorProperty = BindableProperty.Create(nameof(RightTextColor), typeof(Color), typeof(DoubleLabelPicker),
        //  defaultValue: default(Color),
        //  defaultBindingMode: BindingMode.TwoWay,
        //  propertyChanged: OnRightTextColorChanged);

        //private static void OnRightTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    var obj = (DoubleLabelPicker)bindable;
        //    obj.RightContent.TextColor = (Color)newValue;
        //}

        //public Color RightTextColor
        //{
        //    get { return (Color)GetValue(RightTextColorProperty); }
        //    set { SetValue(RightTextColorProperty, value); }
        //}


        public static readonly BindableProperty LeftTextColorProperty = BindableProperty.Create(nameof(LeftTextColor), typeof(Color), typeof(DoubleLabelPicker),
            defaultValue: default(Color),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnLeftTextColorChanged);

        private static void OnLeftTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (DoubleLabelPicker)bindable;
            obj.LeftContent.TextColor = (Color)newValue;
        }

        public Color LeftTextColor
        {
            get { return (Color)GetValue(LeftTextColorProperty); }
            set { SetValue(LeftTextColorProperty, value); }
        }



        public static BindableProperty ItemsSourceProperty = BindableProperty.Create(
                                                            propertyName: "ItemsSource",
                                                            returnType: typeof(IEnumerable<string>),
                                                            declaringType: typeof(DoubleLabelPicker),
                                                            defaultValue: null,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: ItemsSourcePropertyChanged);

        public static BindableProperty SelectedItemProperty = BindableProperty.Create(
                                                            propertyName: "SelectedItem",
                                                            returnType: typeof(string),
                                                            declaringType: typeof(DoubleLabelPicker),
                                                            defaultValue: null,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: SelectedItemPropertyChanged);

        public static BindableProperty SelectedIndexProperty = BindableProperty.Create(
                                                            propertyName: "SelectedIndex",
                                                            returnType: typeof(int),
                                                            declaringType: typeof(DoubleLabelPicker),
                                                            defaultValue: -1,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: SelectedIndexPropertyChanged);

        public static BindableProperty TitleProperty = BindableProperty.Create(
                                                            propertyName: "TitleProperty",
                                                            returnType: typeof(string),
                                                            declaringType: typeof(DoubleLabelPicker),
                                                            defaultValue: string.Empty,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: TitlePropertyChanged);

        public static BindableProperty PlaceholderProperty = BindableProperty.Create(
                                                            propertyName: "PlaceholderProperty",
                                                            returnType: typeof(string),
                                                            declaringType: typeof(DoubleLabelPicker),
                                                            defaultValue: string.Empty,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: PlaceholderPropertyChanged);


        public static BindableProperty TextColorProperty = BindableProperty.Create(
                                                           propertyName: "TextColor",
                                                           returnType: typeof(Color),
                                                           declaringType: typeof(DoubleLabelPicker),
                                                           defaultValue: Color.Black,
                                                           defaultBindingMode: BindingMode.TwoWay,
                                                           propertyChanged: TextColorPropertyChanged);


        #endregion BindableProperties
        #region Properties
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public string SelectedItem
        {
            get
            {
                return (string)GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        public int SelectedIndex
        {
            get
            {
                return (int)GetValue(SelectedIndexProperty);
            }
            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public IEnumerable<string> ItemsSource
        {
            get { return (IEnumerable<string>)base.GetValue(ItemsSourceProperty); }
            set { base.SetValue(ItemsSourceProperty, value); }
        }

        public event EventHandler SelectedIndexChanged;

        public Action RefreshContent { get; set; }

        public ICommand ReloadItemsSourceCommand { get; set; }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty ItemsProperty = BindableProperty.Create("Items", typeof(IEnumerable<string>), typeof(ExtendedEntry), null);

        public IEnumerable<string> Items
        {
            get => (IEnumerable<string>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }
        #endregion Properties
        #region Events
        private static void TextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedPicker)bindable;

            if (control != null)
            {
                control.TextColor = (Color)newValue;
            }
        }
        private static void SelectedItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DoubleLabelPicker)bindable;

            if (control != null)
            {
                control.SelectedItem = (string)newValue;
                control.RightContent.Text = control.SelectedItem;
            }
        }

        private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DoubleLabelPicker)bindable;

            if (control != null)
            {
                control.Title = (string)newValue;
                control.LeftContent.Text = control.Title;
            }
        }

        private static void PlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DoubleLabelPicker)bindable;

            if (control != null)
            {
                control.Placeholder = (string)newValue;
                control.RightContent.Text = control.Placeholder;
            }
        }

        private static void SelectedIndexPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DoubleLabelPicker)bindable;

            if (control != null)
            {
                control.SelectedIndex = (int)newValue;
            }
        }



        private static void ItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DoubleLabelPicker)bindable;

            if (control != null)
            {
                control.ItemsSource = (IEnumerable<string>)newValue;
            }
        }
        #endregion Events

        private void SetUp()
        {
            try
            {
                TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.NumberOfTapsRequired = 1;
                tapGestureRecognizer.Tapped += (s, e) =>
                {
                    var PickerPop = new ExtendedPickerPopup();
                    PickerPop.BindingContext = this;

                    PickerPop.SetBinding(ExtendedPickerPopup.ItemsSourceProperty, "ItemsSource");
                    PickerPop.SetBinding(ExtendedPickerPopup.TitleProperty, "Title");

                    PickerPop.SelectedIndexChanged += (p, t) =>
                     {
                         this.SelectedIndex = t.SelectedIndex;
                         this.SelectedItem = t.DisplayText;
                         RightContent.TextColor = this.TextColor;

                         SelectedIndexChanged?.Invoke(this, null);
                     };

                    Navigation.PushPopupAsync(PickerPop, true);
                };

                this.GestureRecognizers.Add(tapGestureRecognizer);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                //throw;
            }
        }
    }
}