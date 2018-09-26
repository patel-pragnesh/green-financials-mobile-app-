using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using SterlingSwitch.Custom.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.PopUps
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExtendedPickerPopup : PopupPage
    {
		public ExtendedPickerPopup ()
		{
			InitializeComponent ();

            //this.BindingContext = this;
            HeaderTxt.BindingContext = this;
            CloseWhenBackgroundIsClicked = false;
		}


        void Reload()
        {
            var context = (ExtendedPicker)this.BindingContext;
            context.ReloadItemsSourceCommand.Execute(null);
        }

        public event EventHandler<PickerItems> SelectedIndexChanged;
        
        public ICommand ReloadCommand { get; set; }

        public static BindableProperty ItemsSourceProperty = BindableProperty.Create(
                                                            propertyName: "ItemsSource",
                                                            returnType: typeof(IEnumerable<string>),
                                                            declaringType: typeof(ExtendedPickerPopup),
                                                            defaultValue: null,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: ItemsSourceChanged);

        public static BindableProperty IsRefreshVisibleProperty = BindableProperty.Create(
                                                            propertyName: "IsRefreshVisible",
                                                            returnType: typeof(bool),
                                                            declaringType: typeof(ExtendedPickerPopup),
                                                            defaultValue: false,
                                                            defaultBindingMode: BindingMode.TwoWay);
        
        public static BindableProperty PickerTitleProperty = BindableProperty.Create(
                                                            propertyName: "PickerTitleProperty",
                                                            returnType: typeof(string),
                                                            declaringType: typeof(ExtendedPickerPopup),
                                                            defaultValue: string.Empty,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: PickerTitleChanged);
        

        public List<PickerItems> Items { get => SetItems(); }

        public IEnumerable<string> ItemsSource
        {
            get { return (IEnumerable<string>)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);

                if (ItemsSource == null || ItemsSource.Count() == 0)
                    IsRefreshVisible = true;
                else
                    IsRefreshVisible = false;
            }
        }

        public string PickerTitle
        {
            get { return (string)GetValue(PickerTitleProperty); }
            set { SetValue(PickerTitleProperty, value); }
        }

        public bool IsRefreshVisible
        {
            get { return (bool)GetValue(IsRefreshVisibleProperty); }
            set { SetValue(IsRefreshVisibleProperty, value); }
        }


        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedPickerPopup)bindable;

            var items = control.ItemsSource;

            List<PickerItems> pickerItems = new List<PickerItems>();

            foreach (var item in items)
            {
                pickerItems.Add(new PickerItems
                {
                    DisplayText = item,
                    SelectedIndex = items.IndexOf(item)
                });
            }

            control.PickerList.ItemsSource = pickerItems;

            if (control.ItemsSource == null || control.ItemsSource.Count() == 0)
                control.RefreshView.IsVisible = true;
            else
                control.RefreshView.IsVisible = false;
        }

        private static void PickerTitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedPickerPopup)bindable;

            control.PickerTitle = (string)newValue;
            control.HeaderTxt.Text = (string)newValue;
        }

        public List<PickerItems> SetItems()
        {
            List<PickerItems> pickerItems = new List<PickerItems>();

            foreach (var item in ItemsSource)
            {
                pickerItems.Add(new PickerItems
                {
                    DisplayText = item,
                    SelectedIndex = ItemsSource.IndexOf(item)
                });
            }


            return pickerItems;
        }

        private void PickerList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (PickerList.SelectedItem == null)
                return;

            var item = (PickerItems)PickerList.SelectedItem;
            Navigation.PopPopupAsync();
            SelectedIndexChanged?.Invoke(this, item);
        }

        private void Cancel(object sender, EventArgs e)
        {
            Navigation.PopPopupAsync();
        }

        private void Refresh(object sender, EventArgs e)
        {
            var context = (ExtendedPicker)this.BindingContext;

            Task.Run(async () => {
                while (RefreshBtn.IsVisible)
                {
                    await RefreshBtn.RelRotateTo(360, 500);
                    await Task.Delay(100);
                }
            });

            context.RefreshContent?.Invoke();
        }
    }

    public class PickerItems
    {
        public string DisplayText { get; set; }
        public int SelectedIndex { get; set; }
    }
}