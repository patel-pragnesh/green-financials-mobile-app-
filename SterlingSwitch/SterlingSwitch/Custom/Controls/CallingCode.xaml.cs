using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Custom.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CallingCode : ContentPage
	{
		public CallingCode ()
		{
			InitializeComponent ();
		}
        public List<CountryCode> PopList { get; private set; }
        public List<CountryCode> SearchList = new List<CountryCode>();
        public event EventHandler<CountryCode> SelectionSucceeded;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetCallingCodes();
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var v = SearchBox.Text.ToLowerInvariant();
            SearchList = new List<CountryCode>();

            foreach (var item in PopList)
            {
                if (item.CountryName.ToLowerInvariant().Contains(v) | item.Code.ToLowerInvariant().Contains(v))
                {
                    SearchList.Add(item);
                }
            }

            CurrencyListView.ItemsSource = SearchList;

            if (string.IsNullOrEmpty(SearchBox.Text))
            {
                CurrencyListView.ItemsSource = PopList;
            }

        }

        private List<CountryCode> LoadCodes(Assembly assembly)
        {            
            Stream stream = assembly.GetManifestResourceStream("SterlingSwitch.Custom.Controls.CountryInfo.xml");
            string text = "";
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            XDocument xdoc = XDocument.Parse(text);
            var popList = new List<CountryCode>();

            popList = (from item in xdoc.Descendants("country").Where(i => i.Attribute("callingCode").Value != null && i.Attribute("callingCode").Value.Trim() != string.Empty)
                       select new CountryCode()
                       {
                           CountryName = item.Attribute("name").Value.Split(',').FirstOrDefault(),
                           Code = item.Attribute("callingCode").Value.Split(',').FirstOrDefault(),
                           FlagCode = item.Attribute("cca3").Value.Split(',').FirstOrDefault().ToLower()
                       }).OrderBy(x => x.CountryName).ToList();

            return popList;
        }

        private async void GetCallingCodes()
        {
            LoadingProgressBar.IsVisible = true;
            await Task.Run(() =>    // by putting this Task.Run only the Activity Indicator is shown otherwise its not shown.  So we have added this.
            {
                PopList = LoadCodes(this.GetType().GetTypeInfo().Assembly);

                Device.BeginInvokeOnMainThread(() => {

                    CurrencyListView.ItemsSource = PopList;
                    LoadingProgressBar.IsVisible = false;
                });
            });
        }
             

        private void CurrencyListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (CurrencyListView.SelectedItem == null)
                return;

            var item = CurrencyListView.SelectedItem as CountryCode;

            SelectionSucceeded?.Invoke(this, item);
            Navigation.PopModalAsync(true);
        }

        public class CountryCode
        {
            public string CountryName { get; set; }
            public string Code { get; set; }
            public string FlagCode { get; set; }
        }
    }
}