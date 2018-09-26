using Newtonsoft.Json;
using SterlingSwitch.Helper;
using SterlingSwitch.Pages.Investments.ViewModel;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Investments.TreasuryBills
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TreasuryBillsPage : SwitchMasterPage
    {
		public TreasuryBillsPage ()
		{
			InitializeComponent ();

          
            des.Text = $"Choose your preferred investment \n  option below.";
            GetTBills();
        }

        ApiRequest apiRequest = new ApiRequest();
        List<TreasuryBill> treasuryBills;

        private async void GetTBills()
        {
            //var pd = await ProgressDialog.Show("Getting all available Treasury Bills");
            LoadingIndicator.IsVisible = true;
            TreasuryBillViewModel tbillModel = new TreasuryBillViewModel()
            {

                Referenceid = Utilities.GenerateReferenceId(),
                RequestType = 121,
                TransactionReference = Utilities.GenerateReferenceId(),
                Translocation = GlobalStaticFields.GetUserLocation
            };

            var BaseURL = URLConstants.SwitchApiLiveBaseUrl;
            var endpoint = "Spay/ParthianInstrumentsReq";

            try
            {
                var response = await apiRequest.Post(tbillModel, "", BaseURL, endpoint, "TreasuryBillsPage");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var tbills = JsonConvert.DeserializeObject<SpayResponse>(jsonString);

                    var allTBills = JsonConvert.DeserializeObject<List<TreasuryBill>>(tbills.responsedata);

                    List<TreasuryBill> treasuryBills = new List<TreasuryBill>();
                    foreach (var item in allTBills)
                    {
                        item.Maturity = FormatDate(item.Maturity);
                    }

                    TreasuryListView.ItemsSource = allTBills.OrderBy(x => x.Coupon);

                    if (allTBills.Count < 1)
                        RefreshView.IsVisible = true;
                    else
                        RefreshView.IsVisible = false;
                }
                else
                    RefreshView.IsVisible = true;

                LoadingIndicator.IsVisible = false;
            }
            catch (Exception ex)
            {
                LoadingIndicator.IsVisible = false;
                RefreshView.IsVisible = true;
                string log = ex.ToString();
                await BusinessLogic.Log(ex.ToString(), "Excepton on getting all parthian investments", BaseURL + endpoint, "", "", "TreasuryBillsPage");
            }
        }

        private string FormatDate(string s)
        {
            try
            {
                DateTime date = DateTime.Parse(s);
                s = date.ToString("MMM dd, yyy");
            }
            catch (Exception)
            {
                ;
            }

            return s;
        }



        private void TreasuryListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (TreasuryListView.SelectedItem == null)
                return;

            if(int.TryParse(AmountTxt.Text, out int amount))
            {
                if(amount < 100000)
                {
                    MessageDialog.Show("Treasury Bills", $"The minimum buyable amount of Treasury Bills is {Utilities.GetCurrency("NGN")}100,000", DialogType.Info, "OK", null);
                    return;
                }

                var selectedTBills = (TreasuryBill)e.SelectedItem;
                selectedTBills.Price = (float)amount;
                Navigation.PushAsync(new ConfirmTreasuryBillsPage(selectedTBills));
            }
        }

        private void Refresh(object sender, EventArgs e)
        {
            GetTBills();
        }

        private void GoBack(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}