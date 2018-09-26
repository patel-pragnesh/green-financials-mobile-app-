using FormsControls.Base;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
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
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Investments.FixedDeposit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FixedDepositPage : SwitchMasterPage, IAnimationPage
    {
        public IPageAnimation PageAnimation { get; } = new FlipPageAnimation { Duration = AnimationDuration.Long, Subtype = AnimationSubtype.FromTop };
        private FixedDepositViewModel _vm;
        ApiRequest apirequest = new ApiRequest();
        public string rate { get; set; }
        public string NewRate { get; set; }
        string splitedDate { get; set; }
        

        public FixedDepositPage()
        {
            InitializeComponent();
            this.BindingContext = _vm = new FixedDepositViewModel(Navigation);

     
          
            
            //this is how to get last page. commented. saved for future
            ////var xy = App.Current.MainPage.Navigation.NavigationStack.Last();
     
       
            sliderValue.Value = 30;
            GetRateAsync();
        }

        private async void btnTryFixedDeposit_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                if (Convert.ToDecimal(txtAmountToInvest.Text) < 100000) // to be modified just for tests
                {
                    MessageDialog.Show("OOPS", "Sorry, the minimum amount to invest is 100,000. Please review and try again", DialogType.Error, "DISMISS", null);
                    return;
                }
                var pd = await ProgressDialog.Show("Sending Request. Please Wait...");
                FixedDepositDataModel.InvestAmount = txtAmountToInvest.Text;
                FixedDepositDataModel.Tenure = lblDays.Text;

                string t = FixedDepositDataModel.Tenure;
                var t2 = t.Split(' ');

                var day = t2[1];
                splitedDate = day;
                var day2 = t2[2];
                var dd = day2.Substring(0, 1);
                string tenure = $"{day}{dd}";
                // lets make a final call to get the rate based on the amount and number of days 
                var model = new FixedDepositNewRate
                {
                    amt = txtAmountToInvest.Text,
                    day = tenure
                };

                var request = await apirequest.Post<FixedDepositNewRate>(model, "", URLConstants.SwitchApiLiveBaseUrl, "Switch/NewGetRate", "FixedDepositPage").ConfigureAwait(true);
                await pd.Dismiss();

                if (request.IsSuccessStatusCode)
                {
                    var result = await request.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<string>(result);
                    if (!string.IsNullOrEmpty(data))
                        NewRate = data;
                    else
                        NewRate = rate;
                }
                DoNavigateFixedDeposit();
            }
            catch (Exception ex)
            {
                 // log your exception
            }
        }

        private async void DoNavigateFixedDeposit()
        {
            var days = Convert.ToInt16(splitedDate);

            FixedDepositDataModel.Rate = NewRate;
            var summary = $"Deposit N{Convert.ToDouble(txtAmountToInvest.Text).ToString("N2")} \nfor {lblDays.Text} to earn";
            var expectedAmount = $"";
            var duration = $"On {DateTime.Now.AddDays(days)} \n at {NewRate}% interest";

            await Navigation.PushAsync(new ConfirmFixedDepositPage(DateTime.Now.AddDays(days), NewRate));           

        }

        public async Task<string> GetRateAsync()
        {
            try
            {
                var result = await apirequest.Post<object>(model:null,bearerToken: "",baseUrl: URLConstants.SwitchApiLiveBaseUrl,referenceUrl: "Switch/GetRate",pageOrViewModel: "FixdDepositPage");
                var jsonString = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var jData = JsonConvert.DeserializeObject<string>(jsonString);
                    rate = jData;
                }
            }
            catch (Exception ex)
            {

            }
            return rate;
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            var summary = $"Deposit N500,000 \nfor 180 days to earn";
            var expectedAmount = $"N1,000,000";
            var duration = $"on June 22, 2018 \n at 6% interest";
            var md = new FixedDepositTryDialog(summary, expectedAmount, duration);
            App.Current.MainPage.Navigation.PushPopupAsync(md, true);
        }

        private void sliderValue_ValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            var obj = (Slider)sender;
            if (obj.Value >= 0 && obj.Value <= 30)
                obj.Value = 30;
            else if (obj.Value >= 31 && obj.Value <= 60)
                obj.Value = 60;
            else if (obj.Value >= 61 && obj.Value <= 90)
                obj.Value = 90;
            else if (obj.Value >= 91 && obj.Value <= 120)
                obj.Value = 120;
            else 
                obj.Value = 180;
            
        }

        public void OnAnimationStarted(bool isPopAnimation)
        {
            // Put your code here but leaving empty works just fine
        }

        public void OnAnimationFinished(bool isPopAnimation)
        {
            // Put your code here but leaving empty works just fine
        }

        private void BackButton_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync(true);
        }
    }
}