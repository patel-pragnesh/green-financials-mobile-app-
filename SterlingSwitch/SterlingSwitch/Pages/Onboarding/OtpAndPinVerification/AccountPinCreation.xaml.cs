using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormsControls.Base;
using Newtonsoft.Json;
using SterlingSwitch.Custom.Controls;
using SterlingSwitch.Helper;
using SterlingSwitch.Pages.Onboarding.SecurityQuestion.Model;
using SterlingSwitch.Pages.Onboarding.SecurityQuestion.ViewModel;
using SterlingSwitch.Pages.Onboarding.Services;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Templates;
using switch_mobile.Services.Abstractions.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Onboarding.OtpAndPinVerification
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPinCreation : SwitchMasterPage
    {
        OnboardingViewModel _vm;
        List<QuestionAnswerModel> _qanda;
        public int Stage { get; set; } = 1;
        public string PinTracker = string.Empty;
        public string Pin = string.Empty;
        ICryptoService _crypto = DependencyService.Get<ICryptoService>();

        public AccountPinCreation(OnboardingViewModel model, List<QuestionAnswerModel> QandA)
        {
            
            _vm = model;
            _qanda = QandA;
            InitializeComponent();
        }

        private void PinCreation_Tapped(object sender, EventArgs e)
        {
            if (Pin.Length == 4 && Stage == 2)
            {
                if (!string.IsNullOrEmpty(Pin) && !string.IsNullOrEmpty(PinTracker))
                {
                    if (Pin.Length == 4 && PinTracker.Length == 4)
                    {
                        DoAccountCreation();
                    }
                }
            }
            PinStack.IsVisible = false;
            ConfirmPinStack.IsVisible = true;
            Pin = string.Empty;
            Stage = 2;
        }

        private void BackLabel_Tapped(object sender, EventArgs e)
        {
            PinStack.IsVisible = true;
            ConfirmPinStack.IsVisible = false;
            Pin = string.Empty;
            PinTracker = string.Empty;
            UpdateDots(Pin.Length);
            Stage = 1;
        }

        private void Cross_Tapped(object sender, EventArgs e)
        {
            // MessageDialog.Show("Warning", "You are about to terminate your onboarding process, would you like to proceed?", DialogType.Question, "Ok", ()=> Navigation.PopAsync(true), "Cancel",null);
            Navigation.PopAsync(true);
        }

        private void UpdateDots(int length)
        {
            Color fillColor = Color.Black;
            Color noColor = Color.Transparent;
            if (Stage == 1)
            {
                switch (length)
                {

                    case 0:
                        CellOne.Source = "Circle.png";
                        CellTwo.Source = "Circle.png";
                        CellThree.Source = "Circle.png";
                        CellFour.Source = "Circle.png";
                        break;
                    case 1:
                        CellOne.Source = "FilledCircle.png";
                        CellTwo.Source = "Circle.png";
                        CellThree.Source = "Circle.png";
                        CellFour.Source = "Circle.png";
                        break;
                    case 2:
                        CellOne.Source = "FilledCircle.png";
                        CellTwo.Source = "FilledCircle.png";
                        CellThree.Source = "Circle.png";
                        CellFour.Source = "Circle.png";
                        break;
                    case 3:
                        CellOne.Source = "FilledCircle.png";
                        CellTwo.Source = "FilledCircle.png";
                        CellThree.Source = "FilledCircle.png";
                        CellFour.Source = "Circle.png";
                        break;
                    case 4:
                        CellOne.Source = "FilledCircle.png";
                        CellTwo.Source = "FilledCircle.png";
                        CellThree.Source = "FilledCircle.png";
                        CellFour.Source = "FilledCircle.png";
                        break;
                }
            }
            else
            {
                switch (length)
                {

                    case 0:
                        ConfirmCellOne.Source = "Circle.png";
                        ConfirmCellTwo.Source = "Circle.png";
                        ConfirmCellThree.Source = "Circle.png";
                        ConfirmCellFour.Source = "Circle.png";
                        break;
                    case 1:
                        ConfirmCellOne.Source = "FilledCircle.png";
                        ConfirmCellTwo.Source = "Circle.png";
                        ConfirmCellThree.Source = "Circle.png";
                        ConfirmCellFour.Source = "Circle.png";
                        break;
                    case 2:
                        ConfirmCellOne.Source = "FilledCircle.png";
                        ConfirmCellTwo.Source = "FilledCircle.png";
                        ConfirmCellThree.Source = "Circle.png";
                        ConfirmCellFour.Source = "Circle.png";
                        break;
                    case 3:
                        ConfirmCellOne.Source = "FilledCircle.png";
                        ConfirmCellTwo.Source = "FilledCircle.png";
                        ConfirmCellThree.Source = "FilledCircle.png";
                        ConfirmCellFour.Source = "Circle.png";
                        break;
                    case 4:
                        ConfirmCellOne.Source = "FilledCircle.png";
                        ConfirmCellTwo.Source = "FilledCircle.png";
                        ConfirmCellThree.Source = "FilledCircle.png";
                        ConfirmCellFour.Source = "FilledCircle.png";
                        break;
                }
            }
        }

        private void GetInput(string value)
        {

            if (Pin.Length < 4)
            {
                Pin += value;
                if (Stage == 1)
                { PinTracker += value; }
                UpdateDots(Pin.Length);
            }

        }

        private void BackSpace()
        {
            if (!string.IsNullOrEmpty(Pin))
            {
                Pin = Pin.Remove(Pin.Length - 1);
                UpdateDots(Pin.Length);
            }
        }

        private void Delete_OnTapped(object sender, EventArgs e)
        {
            BackSpace();
        }

        private void InputClicked(object sender, object e)
        {
            var input = (PinItemView)sender;
            GetInput((string)input.CommandParameter);
        }

        async void DoAccountCreation()
        {
            string password = _crypto.Encrypt(Pin);
            try
            {
                if (Pin != PinTracker)
                {
                    MessageDialog.Show("OOPS", "PIN and Confirm PIN Mismatch. Kindly review and try again.", DialogType.Error, "OK", null);
                    return;
                }
                else
                {
                    var key = await Microsoft.AppCenter.AppCenter.GetInstallIdAsync();
                    DateTime _dob = _vm.DateOfBirth;
                    var model = new SwitchUser()
                    {
                        UserEmail = _vm.Email,
                        Password = password,
                        Gender = _vm.Gender == "Male" ? "M" : "F",
                        DateOfBirth =_dob,
                        FirstName = _vm.Firstname.Trim(),
                        LastName = _vm.Lastname.Trim(),
                        MiddleName = "N/A",
                        Title = _vm.Gender == "Male" ? "Mr" : "Mrs",
                        TPIN = password,
                        PhoneNumber = _vm.WalletPhone,
                        RefferedBy = _vm.ReferralCode,
                        ReferralCode = GlobalStaticFields.RandomString(8),
                        Device = GlobalStaticFields.Device(),
                        IMEI = GlobalStaticFields.DeviceIMEI(),
                        OS = GlobalStaticFields.DeviceOS(),
                        UniqueKey = key?.ToString() ?? Guid.NewGuid().ToString(),
                        AddressLine1 = string.Empty,
                        AddressLine2 = string.Empty,
                        Nationality = 0,
                        AccessLocation = GlobalStaticFields.GetUserLocation,
                        AccountType = "",
                        CustomerTimeZone = DateTime.Now.ToString("yymmddHHmmss"),
                        HomeAddress = "",
                        IsTPIN = false,
                        SignupVerificationCode = "",
                        SecurityQuestionAndAnswers = _qanda //new List<SecurityQuestionViewModel> {_qanda }
                    };
                    var pd = await ProgressDialog.Show("Sending Request..... Please wait.");
                    var response = await OnBoardingService.DoSwitchAccountCreation(model);
                    if (!string.IsNullOrEmpty(response))
                    {
                        var dt = JsonConvert.DeserializeObject<NewStatusMessage>(response);
                        if (dt.Status == true)
                        {
                            MessageDialog.Show("SUCCESS", "Account registration was successful.", DialogType.Success, "OK", null);
                            var upgradeModel = new UpgradeAccountForT24()
                            {
                                BVN = GlobalStaticFields.Customer.ListOfAllAccounts.FirstOrDefault(g => g.BVN != null || g.BVN != String.Empty)?.BVN ?? "",
                                CUSNUM = GlobalStaticFields.Customer.ListOfAllAccounts.FirstOrDefault(g => g.CustomerId != null || g.CustomerId != String.Empty)?.CustomerId ?? "",
                                HomeAddress = "",
                                NUBAN = GlobalStaticFields.Customer.ListOfAllAccounts.FirstOrDefault(g => g.nuban != null || g.nuban != string.Empty)?.nuban ?? "",
                                PhoneNumber = _vm.PhoneNumber
                            };
                            var upgraded = await OnBoardingService.UpgradeAccountForT24(upgradeModel);
                            if (!string.IsNullOrEmpty(upgraded))
                            {
                                if (upgraded.Contains("true"))
                                {
                                    string message = $"Dear Customer You have successfully Onboarded on Switch. kindly ignore this message if it was you, or report to the nearest Sterling bank office if otherwise.";
                                    DoLogin(_vm.Email, password);   // loguser in and navigate to dashboard.
                                    SendMail(message, _vm.Email);  // send mail to the specified email address
                                    // registration was successful
                                    await pd.Dismiss();
                                }
                            }
                        }
                        else
                        {
                           await pd.Dismiss();
                            MessageDialog.Show("OOPS", "Sorry, an error occured at our end. Kindly try again later.", DialogType.Error, "OK", null);
                            return;
                        }
                    }
                    else
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("OOPS", "Sorry, we are unable to create your account at the moment. Kindly try again.", DialogType.Error, "OK", null);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
            }
        }

        void SendMail(string message, string email)
        {
            BusinessLogic.SendEmail(message, email);
        }

        async void DoLogin(string email, string password)
        {
            var req = new LoginInfo()
            {
                UserID = email,
                Password = password
            };
            var response = await OnBoardingService.LoginAfterOnboarding(req);
            if (!string.IsNullOrEmpty(response))
            {
                //Get Token and save token lifespan
               
                await LoginHelper.LogUserDetails(response, email);   // get user details in abstract class
                if (GlobalStaticFields.LoginTest == "Passed")
                {
                    Application.Current.MainPage = new AnimationNavigationPage(new Dashboard.Dashboard());
                }
                else
                {                    
                    MessageDialog.Show("OOPS", "Sorry, your login attempt failed. Kindly review your credentials and try again.", PopUps.DialogType.Error, "OK", null);
                    return;
                }
            }
        }
    }
}