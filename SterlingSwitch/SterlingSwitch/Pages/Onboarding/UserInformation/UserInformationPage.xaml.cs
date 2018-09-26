using Newtonsoft.Json;
using SterlingSwitch.Pages.Onboarding.OtpAndPinVerification;
using SterlingSwitch.Pages.Onboarding.Services;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using System;
using FormsControls.Base;
using Xamanimation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Linq;
using SterlingSwitch.Models;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using SterlingSwitch.Custom.Controls;
using SterlingSwitch.Pages.Onboarding.Login;

namespace SterlingSwitch.Pages.Onboarding.UserInformation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserInformationPage : SwitchMasterPage
    {
        OnboardingViewModel _vm;
        string phone = string.Empty;

        public UserInformationPage()
        {
            InitializeComponent();
            _vm = new OnboardingViewModel(Navigation);
            BindingContext = _vm;
            GenderList();
        }

        private void Cross_Tapped(object sender, EventArgs e)
        {
            // MessageDialog.Show("Warning", "You are about to terminate your onboarding process, would you like to proceed?", DialogType.Question, "Ok", () => App.Current.MainPage = new UnProfiledLoginPage(), "Cancel", null);
            Navigation.PopAsync(true);
        }

        private void btnStep1_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_vm.Firstname))
            {
                if (grdStep1.IsVisible == true)
                {
                    //grdStep1.Animate(new FadeOutAnimation());
                    grdStep1.IsVisible = false;
                    grdStep2.Animate(new FadeInAnimation());
                    grdStep2.IsVisible = true;
                }
            }
            else
            {
                MessageDialog.Show("OOPS", "Please enter your Firstname", DialogType.Error, "DISMISS", null);
                return;
            }
        }

        private void btnStep2_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_vm.Lastname))
            {
                if (grdStep2.IsVisible == true)
                {
                    //grdStep2.Animate(new FadeOutAnimation());
                    grdStep2.IsVisible = false;
                    grdStep3.Animate(new FadeInAnimation());
                    grdStep3.IsVisible = true;
                    
                }
            }
            else
            {
                MessageDialog.Show("OOPS", "Please enter your Lastname", DialogType.Error, "DISMISS", null);                
            }
        }

        private void btnStep3_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_vm.DateOfBirth.ToString()))
            {
                if (grdStep3.IsVisible == true)
                {
                    //grdStep3.Animate(new FadeOutAnimation());
                    grdStep3.IsVisible = false;
                    grdStep4.Animate(new FadeInAnimation());
                    grdStep4.IsVisible = true;
                }                
            }
            else
            {
                MessageDialog.Show("OOPS", "Please enter your Date of Birth", DialogType.Error, "DISMISS", null);
            }

        }

        private void btnStep4_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_vm.Gender))
            {
                if (grdStep4.IsVisible == true)
                {
                    //grdStep4.Animate(new FadeOutAnimation());
                    grdStep4.IsVisible = false;
                    grdStep5.Animate(new FadeInAnimation());
                    grdStep5.IsVisible = true;
                }
            }
            else
            {
                MessageDialog.Show("OOPS", "Please select your gender", DialogType.Error, "DISMISS", null);
                return;
            }

        }

        private void btnStep5_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_vm.ReferralCode))
            {
                if (grdStep5.IsVisible == true)
                {
                    //grdStep5.Animate(new FadeOutAnimation());
                    grdStep5.IsVisible = false;
                    grdStep6.Animate(new FadeInAnimation());
                    grdStep6.IsVisible = true;
                }
            }
            else
            {
                MessageDialog.Show("OOPS", "Please enter your Referral code.", DialogType.Error, "DISMISS", null);
                return;
            }

        }
        private void lblSkip_Tapped(object sender, EventArgs e)
        {
            if (grdStep5.IsVisible == true)
            {
                //grdStep5.Animate(new FadeOutAnimation());
                grdStep5.IsVisible = false;
                grdStep6.Animate(new FadeInAnimation());
                grdStep6.IsVisible = true;
            }
        }

        private void btnStep6_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_vm.Email))
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(_vm.Email);
                if (!match.Success)
                {
                    MessageDialog.Show("OOPS", "The Email address you entered is not valid. Kindly review and try again.", DialogType.Error, "DISMISS", null);                  
                    return;
                }
                else
                {
                    if (grdStep6.IsVisible == true)
                    {
                        //grdStep6.Animate(new FadeOutAnimation());
                        grdStep6.IsVisible = false;
                        grdStep7.Animate(new FadeInAnimation());
                        grdStep7.IsVisible = true;
                    }
                }              
            }
            else
            {
                MessageDialog.Show("OOPS", "Please enter your email address", DialogType.Error, "DISMISS", null);
                return;
            }

        }

        private async void btnStep7_Clicked(object sender, EventArgs e)
        {


            List<CustomerAccount> listOfAcct = new List<CustomerAccount>();
            if (!string.IsNullOrEmpty(_vm.CountryCode) && !string.IsNullOrEmpty(_vm.PhoneNumber))
            {
                if (_vm.PhoneNumber.StartsWith("0"))
                {
                    _vm.PhoneNumber = _vm.PhoneNumber.Substring(1);
                }

                phone = _vm.CountryCode + _vm.PhoneNumber;
                _vm.WalletPhone = phone;
                if (grdStep7.IsVisible == true)
                {
                    var pd = await ProgressDialog.Show("Processing Request..... Please wait.");
                    var accountlst = await OnBoardingService.GetAllCustomerAccountsByMobile(phone);
                    await pd.Dismiss();
                    if (!string.IsNullOrEmpty(accountlst))
                    {
                        listOfAcct = JsonConvert.DeserializeObject<List<CustomerAccount>>(accountlst);                      
                        if (listOfAcct.Count > 0)
                        {
                            GlobalStaticFields.Customer.ListOfAllAccounts = listOfAcct;
                            _vm.DefaultAccountNumber = listOfAcct.FirstOrDefault().nuban;
                            lblAccountVerification.Text = _vm.DefaultAccountNumber.Substring(0, 5) + "#####";
                            grdStep7.Animate(new FadeOutAnimation());
                            grdStep7.IsVisible = false;
                            grdStep8.IsVisible = true;
                        }
                        else
                        {
                            var val = await DoAccountVerification();
                            if (val)
                            {
                                Navigation.PushAsync(new OtpVerification(_vm));
                               
                            }
                        }
                    }
                }
            }
            else
            {
                MessageDialog.Show("OOPS", "Country code and phone number are required", DialogType.Error, "DISMISS", null);
                return;
            }
        }

        private async void btnStep8_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_vm.ConfirmAccountNumber))
            {
                if (_vm.DefaultAccountNumber != _vm.ConfirmAccountNumber)
                {
                    MessageDialog.Show("OOPS!", "Sorry, Account number mismatch. Kindly review and try again", DialogType.Error, "DISMISS", null);
                    return;
                }
                else
                {
                    var val = await DoAccountVerification();
                    if (val)
                    {
                       Navigation.PushAsync(new OtpVerification(_vm));
                    }
                    else
                        _vm.PhoneNumber = string.Empty; 
                }
            }
            else
            {
                MessageDialog.Show("OOPS!", "Kindly confirm the account number stated above.", DialogType.Error, "DISMISS", null);
                return;
            }

        }

        async Task<bool> DoAccountVerification()
        {
            bool status = false;
            var pd = await ProgressDialog.Show("Processing Request..... Please wait.");
            Random r = new Random();
            r.Next(1000, 9999);
            var otpRequest = new OTPRequestMobile()
            {
                mobile =_vm.WalletPhone,
                Referenceid = "00055" + DateTime.Now.ToString("yymmddHHmmss") + r.Next().ToString(),
                RequestType = 142,
                Translocation = GlobalStaticFields.GetUserLocation,
                email = _vm.Email
            };
            // lets check if the user exist before now.
            var user = new UserAlreadyExist()
            {
                Phone = _vm.WalletPhone,
                UserEmail = _vm.Email
            };
            var userResponse = await OnBoardingService.VerifyRegistrationStatus(user);
            if (userResponse == true) // value should be compared against true. this condition was changed on purpose for test
            {
                
                await pd.Dismiss();
                MessageDialog.Show("OOPS", "Sorry, It appears that you already onboarded. Kindly review your details and try again, or contact our switch support.", DialogType.Error, "DISMISS", null);                
            }
            else
            {
                var otpresponse = await OnBoardingService.IsOtpSent(otpRequest);
                if (otpresponse)
                {
                    await pd.Dismiss();
                    MessageDialog.Show("SUCCESS", $"An OTP has been sent to your mobile number {phone} and Email {_vm.Email}", DialogType.Success, "OK", null);
                    
                    status = true;                 
                }
            }
            return status;
        }

        void GenderList()
        {
            var lst = new List<string>()
            {
                "Male", "Female"
            };
            dpdGender.ItemsSource = lst;
        }

        
        private void PreviousStepOne_Tapped(object sender, EventArgs e)
        {
            //grdStep2.Animate(new FadeOutAnimation());
            grdStep2.IsVisible = false;
            grdStep1.Animate(new FadeInAnimation());
            grdStep1.IsVisible = true;
        }

        private void PreviousStepTwo_Tapped(object sender, EventArgs e)
        {
            //grdStep3.Animate(new FadeOutAnimation());
            grdStep3.IsVisible = false;
            grdStep2.Animate(new FadeInAnimation());
            grdStep2.IsVisible = true;
        }

        private void PreviousStepThree_Tapped(object sender, EventArgs e)
        {
            //grdStep4.Animate(new FadeOutAnimation());
            grdStep4.IsVisible = false;
            grdStep3.Animate(new FadeInAnimation());
            grdStep3.IsVisible = true;
        }

        private void PreviousStepFour_Tapped(object sender, EventArgs e)
        {
            //grdStep5.Animate(new FadeOutAnimation());
            grdStep5.IsVisible = false;
            grdStep4.Animate(new FadeInAnimation());
            grdStep4.IsVisible = true;
        }

        private void PreviousStepFive_Tapped(object sender, EventArgs e)
        {
            //grdStep6.Animate(new FadeOutAnimation());
            grdStep7.IsVisible = false;
            grdStep6.Animate(new FadeInAnimation());
            grdStep6.IsVisible = true;
        }
        private void dpdGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            _vm.Gender = dpdGender.SelectedItem;
        }

        private async void txtCountryCode_Focused(object sender, FocusEventArgs e)
        {
            var popPage = new CallingCode();
            popPage.SelectionSucceeded += (s, arg) =>
            {
                txtCountryCode.Text = "+" + arg.Code;
            };
            txtCountryCode.Unfocus();
            await Navigation.PushModalAsync(popPage);
        }
    }
}