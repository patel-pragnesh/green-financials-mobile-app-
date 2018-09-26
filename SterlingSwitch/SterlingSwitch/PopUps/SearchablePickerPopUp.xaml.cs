using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.PopUps
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchablePickerPopUp : PopupPage
    {
		public SearchablePickerPopUp ()
		{
			InitializeComponent ();
		}

        public SearchablePickerPopUp(IList ItemsSource, string PickerTitle)
        {
            InitializeComponent();
            if (ItemsSource == null)
            {
                return;
            }
            this.ItemsSource = ItemsSource;
            this.PickerTitle = PickerTitle;

            this.BindingContext = this;

            CloseWhenBackgroundIsClicked = false;
        }
        public string PickerTitle { get; set; }

        public IList ItemsSource { get; set; }
        public event EventHandler<SearchablePickerItems> SelectedIndexChanged;

        public class SearchablePickerItems
        {
            public string DisplayText { get; set; }
            public int SelectedIndex { get; set; }
        }
        public List<SearchablePickerItems> Items { get => SetItems(); }
        public List<SearchablePickerItems> SetItems()
        {
            List<SearchablePickerItems> pickerItems = new List<SearchablePickerItems>();
           
                foreach (var item in ItemsSource)
                {
                    var cast = item.ToString();
                    pickerItems.Add(new SearchablePickerItems
                    {
                        DisplayText = cast.ToString(),
                        SelectedIndex = ItemsSource.IndexOf(item)
                    });
                }
            
        



            return pickerItems;
        }

        private void PickerList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (PickerList.SelectedItem == null)
                return;

            var item = (SearchablePickerItems)PickerList.SelectedItem;
            Navigation.PopPopupAsync();
            SelectedIndexChanged?.Invoke(this, item);
        }
        private void Cancel(object sender, EventArgs e)
        {
            Navigation.PopPopupAsync();
        }

        private void searchBarText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var v = searchBarText.Text.ToLowerInvariant();
            List<SearchablePickerItems> SearchList = new List<SearchablePickerItems>();

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
}