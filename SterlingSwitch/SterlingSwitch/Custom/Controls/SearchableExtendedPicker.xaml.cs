using Rg.Plugins.Popup.Extensions;
using SterlingSwitch.PopUps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Custom.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchableExtendedPicker : ContentView
	{
		public SearchableExtendedPicker ()
		{
			InitializeComponent ();
            SetTap();
            this.BindingContext = this;
        }


        TapGestureRecognizer tgr = new TapGestureRecognizer();
        public event EventHandler SelectedIndexChanged;

        public static BindableProperty ItemsSourceProperty = BindableProperty.Create(
                                                            propertyName: "ItemsSource",
                                                            returnType: typeof(IList),
                                                            declaringType: typeof(SearchableExtendedPicker),
                                                            defaultValue: null,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: ItemsSourcePropertyChanged);

        //public static BindableProperty ItemsProperty = BindableProperty.Create(
        //                                                    propertyName: "Items",
        //                                                    returnType: typeof(List<string>),
        //                                                    declaringType: typeof(ExtendedPicker),
        //                                                    defaultValue: null,
        //                                                    defaultBindingMode: BindingMode.TwoWay,
        //                                                    propertyChanged: ItemsPropertyChanged);

        public static BindableProperty SelectedItemProperty = BindableProperty.Create(
                                                            propertyName: "SelectedItem",
                                                            returnType: typeof(string),
                                                            declaringType: typeof(SearchableExtendedPicker),
                                                            defaultValue: "Select an item",
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: SelectedItemPropertyChanged);

        public static BindableProperty SelectedIndexProperty = BindableProperty.Create(
                                                            propertyName: "SelectedIndex",
                                                            returnType: typeof(int),
                                                            declaringType: typeof(SearchableExtendedPicker),
                                                            defaultValue: -1,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: SelectedIndexPropertyChanged);

        public static BindableProperty TitleProperty = BindableProperty.Create(
                                                            propertyName: "TitleProperty",
                                                            returnType: typeof(string),
                                                            declaringType: typeof(SearchableExtendedPicker),
                                                            defaultValue: string.Empty,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: TitlePropertyChanged);

        public static BindableProperty TextColorProperty = BindableProperty.Create(
                                                            propertyName: "TextColor",
                                                            returnType: typeof(Color),
                                                            declaringType: typeof(SearchableExtendedPicker),
                                                            defaultValue: Color.Black,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: TextColorPropertyChanged);


        private static void TextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchableExtendedPicker)bindable;

            if (control != null)
            {
                control.TextColor = (Color)newValue;
            }
        }

        private static void SelectedItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchableExtendedPicker)bindable;

            if (control != null)
            {
                control.SelectedItem = (string)newValue;
            }
        }

        private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchableExtendedPicker)bindable;

            if (control != null)
            {
                control.Title = (string)newValue;
                //control.HeaderTxt.Text = control.Title;
            }
        }

        private static void SelectedIndexPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchableExtendedPicker)bindable;

            if (control != null)
            {
                control.SelectedIndex = (int)newValue;
            }
        }

        private static void ItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchableExtendedPicker)bindable;

            if (control != null)
            {
                control.ItemsSource = (IList)newValue;
            }
        }

        //private static void ItemsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    var control = (ExtendedPicker)bindable;

        //    if (control != null)
        //    {
        //        control.Items = (List<string>)newValue;

        //        if(control.Items.Count > 0)
        //        {
        //            foreach (var item in control.Items)
        //                control.EmbeddedPicker.Items.Add(item);
        //        }
        //    }
        //}

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
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

        public IList ItemsSource
        {
            get { return (IList)base.GetValue(ItemsSourceProperty); }
            set { base.SetValue(ItemsSourceProperty, value); }
        }


        public void SetTap()
        {
            //tgr.Tapped += (s, e) =>
            //{
            if (ItemsSource==null)
            {
                return;
            }
                var pickerPop = new SearchableExtendedPickerPopup(ItemsSource, Title,"");

                pickerPop.SelectedIndexChanged2 += (p, t) =>
                {
                    //this.SelectedIndex = t.RequestType;
                    //this.SelectedItem = t.display;
                    SelectedTxt.TextColor = this.TextColor;

                    SelectedIndexChanged?.Invoke(this, null);
                };

                Navigation.PushPopupAsync(pickerPop, true);
            //};

            //this.GestureRecognizers.Add(tgr);
        }


    }
}