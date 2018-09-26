using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SterlingSwitch.Helper.Utilities;

namespace SterlingSwitch.PopUps
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchableExtendedPickerPopup : PopupPage
    {
        public string ActionTypeToPopUp { get; set; }
        public SearchableExtendedPickerPopup (IList ItemsSource, string PickerTitle, string ActionType)
		{
            ActionTypeToPopUp = ActionType;
            InitializeComponent();
            if (ItemsSource==null)
            {
                return;
            }
            this.ItemsSource = ItemsSource;
            this.PickerTitle = PickerTitle;

            this.BindingContext = this;
        
            CloseWhenBackgroundIsClicked = false;
        }

        public event EventHandler<PickerItems2> SelectedIndexChanged;
        public event EventHandler<PickerItems2> SelectedIndexChanged2;

        public string PickerTitle { get; set; }

        public IList ItemsSource { get; set; }

        public List<PickerItems2> Items { get => SetItems(ActionTypeToPopUp); }

        public List<PickerItems2> SetItems(string action)
        {
            List<PickerItems2> pickerItems = new List<PickerItems2>();
            action = ActionTypeToPopUp;
            if (action== "ListOfBanks")
            {
                foreach (var item in ItemsSource)
                {
                    var cast = item.ToString();
                    pickerItems.Add(new PickerItems2
                    {
                        DisplayText = cast.ToString(),
                        SelectedIndex = ItemsSource.IndexOf(item)
                    });
                }
            }
            else if (action=="Transferbeneficiaries")
            {
                foreach (var item in ItemsSource)
                {
                    var cast = (SaveSwitchBeneficiary.GetSavedBeneficiaries)item;
                    pickerItems.Add(new PickerItems2
                    {
                        DisplayText = cast.display.ToString(),
                        SelectedIndex = ItemsSource.IndexOf(item)
                    });
                }
            }
        


            return pickerItems;
        }

        private void PickerList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (PickerList.SelectedItem == null)
                return;

            var item = (PickerItems2)PickerList.SelectedItem;
            Navigation.PopPopupAsync();
            SelectedIndexChanged?.Invoke(this, item);
        }

        private void PickerList_OnItemSelected2(object sender, SelectedItemChangedEventArgs e)
        {
            if (PickerList.SelectedItem == null)
                return;

            var item = (PickerItems2)PickerList.SelectedItem;
            Navigation.PopPopupAsync();
            SelectedIndexChanged2?.Invoke(this, item);
        }
        private void Cancel(object sender, EventArgs e)
        {
            Navigation.PopPopupAsync();
        }

        private void searchBarText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var v = searchBarText.Text.ToLowerInvariant();
          List<PickerItems2>  SearchList = new List<PickerItems2>();

            foreach (var item in Items)
            {
                if (item.DisplayText.ToLowerInvariant().Contains(v))
                {
                    SearchList.Add(item);
                }
            }

            PickerList.ItemsSource = SearchList;

            if (string.IsNullOrEmpty(searchBarText.Text))
            {
                PickerList.ItemsSource = Items;
            }
        }
    }

    public class PickerItems2
    {
        public string DisplayText { get; set; }
        public int SelectedIndex { get; set; }
    }
}