using Newtonsoft.Json.Linq;
using SterlingSwitch.Extensions;
using SterlingSwitch.Helper;
using SterlingSwitch.Models;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.ViewModelBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.CardlessWithdrawals.ViewModel
{
  public  class CardlessWithdrawViewModel:BaseViewModel
    {
        public CardlessWithdrawViewModel(INavigation navigation):base(navigation)
        {
            WithdrawalTypes = new List<string> { "Self", "Others" };
         //   this.PhoneNumber = GlobalStaticFields.Customer.PhoneNumber;
            WithDrawCommand = new Command(async ()=>await ExecuteCardLess(),()=>CanWithdrawal());
        }

        #region Properties

        public Page CardLessPage
        {
            get;
            set;
        }

        public List<string> WithdrawalTypes { get; set; }

        private string phoneNumber = "";

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { SetProperty(ref phoneNumber, value, onChanged: ((Command)WithDrawCommand).ChangeCanExecute); }
        }
        
        private string phoneNumberTitle = "Phone number";

        public string PhoneNumberTitle
        {
            get { return phoneNumberTitle; }
            set { SetProperty(ref phoneNumberTitle, value); }
        }
        
        private bool isRecipientVisible = false;

        public bool IsRecipientVisible
        {
            get { return isRecipientVisible; }
            set { SetProperty(ref isRecipientVisible, value); }
        }


        private string amount = default(string);

        public string Amount
        {
            get { return amount; }
            set { SetProperty(ref amount, value, onChanged: ((Command)WithDrawCommand).ChangeCanExecute); }
        }


        private string fromAccount = default(string);

        public string FromAccount
        {
            get { return fromAccount; }
            set { SetProperty(ref fromAccount, value, onChanged: ((Command)WithDrawCommand).ChangeCanExecute); }
        }

        private string recipient = default(string);

        public string Recipient
        {
            get { return recipient; }
            set { SetProperty(ref recipient, value); }
        }
        
        private string reference = default(string);

        public string Reference
        {
            get { return reference; }
            set { SetProperty(ref reference, value); }
        }


        private string oneTimePin = default(string);

        public string OneTimePin
        {
            get { return oneTimePin; }
            set { SetProperty(ref oneTimePin, value, onChanged: ((Command)WithDrawCommand).ChangeCanExecute); }
        }

        public ICommand WithDrawCommand { get; set; }
        #endregion

        #region Events
        public async Task ExecuteCardLess()
        {
           
            try
            {
                if (GlobalStaticFields.Customer.IsTPin)
                {
                    var CardLessWithdrawalWithTPIN = new PopUps.VerifyPinPage("Confirmation", Amount, "NGN", null);

                    CardLessWithdrawalWithTPIN.Validated +=  CardLessWithdrawalWithTPIN_Validated;
                  await  Navigation.PushAsync(CardLessWithdrawalWithTPIN);
                }
                else
                {
                  await  DoExecuteCardlessWithdrawal();
                }
               
            }
            catch (Exception ex)
            {

                MessageDialog.Show("OOPS", "An unknown error occurred while generating Token, please try again", DialogType.Error, "OK", null);

            }
            finally
            {
              // await prog.Dismiss();
            }
        }

        private async void CardLessWithdrawalWithTPIN_Validated(object sender, bool e)
        {
          await  DoExecuteCardlessWithdrawal();
        }

        public async Task DoExecuteCardlessWithdrawal()
        {
            var prog = await ProgressDialog.Show("please wait...");
            try
            {
              
                var refid = Utilities.GenerateReferenceId().Substring(4, 9);
                var cardless = new Cardless()
                {
                    Referenceid = refid,
                    RequestType = "209",
                    Translocation = GlobalStaticFields.GetUserLocation,
                    appid = URLConstants.AppId,
                    ttid = refid,
                    amount = double.Parse(Amount).ToString(),
                    codeGenerationChannel = URLConstants.AppId,
                    accountNo = Utilities.SplitBeneficiaryDetailsByTab(FromAccount)[0]?.Trim(),
                    transactionRef = refid,
                    subscriber = PhoneNumber.ToMSDN(),
                    oneTimePin = OneTimePin

                };

                var apirequest = new ApiRequest();
                var request = await apirequest.Post<Cardless>(cardless, "", URLConstants.SwitchApiLiveBaseUrl, "Spay/GenerateTokenReq", "CardlessWithdrawViewModel");
               
                if (request.IsSuccessStatusCode)
                {
                    var jsonString = await request.Content.ReadAsStringAsync();
                    jsonString = jsonString.JsonCleanUp();
                    var jObj = JObject.Parse(jsonString);
                    var resp = jObj.Value<string>("payWithMobileToken");
                    if (!string.IsNullOrWhiteSpace(resp))
                    {
                        //  MessageDialog.Show("Success", "Token successfully generated", DialogType.Success, "OK", null);
                        await Navigation.PushAsync(new CardlessWithdrawalSuccessful(), true);
                        Navigation.RemovePage(CardLessPage);
                    }
                    else
                    {

                        var error = JObject.Parse(jObj["error"].ToString());
                        var code = error.Value<string>("code");
                        if (string.Equals(code, "400900", StringComparison.OrdinalIgnoreCase))
                        {
                            var errorMsg = error.Value<string>("message");
                            MessageDialog.Show("OOPS", errorMsg, DialogType.Error, "OK", null);
                        }
                        else
                        {

                            MessageDialog.Show("OOPS", "Unable to generate token, please try again", DialogType.Error, "OK", null);

                        }

                    }

                }
                else
                {
                    var jsonString = await request.Content.ReadAsStringAsync();
                    jsonString = jsonString.JsonCleanUp();

                    MessageDialog.Show("OOPS", "An error occurred while generating Token, please try again", DialogType.Error, "OK", null);

                }
                await prog.Dismiss();
            }
            catch (Exception ex)
            {
                await prog.Dismiss();
                MessageDialog.Show("OOPS", "Unable to generate token, please try again", DialogType.Error, "OK", null);
            }
        }

        bool CanWithdrawal()
        {
           
            if (string.IsNullOrWhiteSpace(Amount))
                return false;
            else if (string.IsNullOrWhiteSpace(FromAccount))
                return false;
            else if (string.IsNullOrWhiteSpace(PhoneNumber))
                return false;
            else if (string.IsNullOrWhiteSpace(OneTimePin))
                return false;
            else
                return true;
        }
        #endregion
    }
}
