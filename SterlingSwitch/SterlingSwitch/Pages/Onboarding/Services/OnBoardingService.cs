using Newtonsoft.Json;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SterlingSwitch.Pages.Onboarding.Services
{
    public static class OnBoardingService
    {
        private static ApiRequest _apiService;
        public static async Task<bool> VerifyRegistrationStatus(UserAlreadyExist model)
        {
            bool IsExisting = false;
            try
            {
                _apiService = new ApiRequest();
                var response = await _apiService.Post<UserAlreadyExist>(model, "", URLConstants.SwitchApiBaseUrl, "Switch/CheckIfUserAlreadyExist", "Onboarding");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var resp = JsonConvert.DeserializeObject<string>(content);
                    if (resp == "true")
                    {
                        IsExisting = true;
                    }
                    else
                        IsExisting = false;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
            }
            return IsExisting;
        }

        public static async Task<bool> IsOtpSent(OTPRequestMobile otpModel)
        {
            bool isSent = false;
            _apiService = new ApiRequest();
            try
            {
                Random r = new Random();
                var response = await _apiService.Post<OTPRequestMobile>(otpModel, "", URLConstants.SwitchApiBaseUrl, "Spay/OTPRequestMobile", "Onboarding");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var val = JsonConvert.DeserializeObject<StatusMessage>(content);
                    if (val.response == "00")
                        isSent = true;
                    else
                        isSent = false;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
            }
            return isSent;
        }

        public static async Task<string> GetAllCustomerAccountsByMobile(string phone)
        {
            string responseData = string.Empty;
            try
            {
              
                _apiService = new ApiRequest();
                var ph = new Customerphone() { phone = phone };
                var response = await _apiService.Post<Customerphone>(ph, "", URLConstants.SwitchApiBaseUrl, "Switch/GetAllOtherAcctDetailsByNumber", "OnBoarding");
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();                    
                }
            }
            catch(Exception ex)
            {
                string log = ex.Message;
            }
            return responseData;
        }

        public static async Task<string> ValidateOTP(ValOtpRequestMobile valOtpRequestMobile)
        {
            string responseData = string.Empty;
            try
            {
                _apiService = new ApiRequest();
                var response = await _apiService.Post<ValOtpRequestMobile>(valOtpRequestMobile, "", URLConstants.SwitchApiBaseUrl, "Spay/ValOTPRequestMobile", "OnBoardingOTPVerification");
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            catch(Exception ex)
            {
                string log = ex.Message;
            }
            return responseData;
        }

        public static async Task<string> CreateWalletAccount(CustomerWalletAccount customerWalletAccount)
        {
            string responseData = string.Empty;
            try
            {
                _apiService = new ApiRequest();
                var response = await _apiService.Post<CustomerWalletAccount>(customerWalletAccount, "", URLConstants.SwitchApiBaseUrl, "Spay/SBPMWalletAccountReq", "OnboardingOTPVerificationPage");
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            catch(Exception ex)
            {
                string log = ex.Message;
            }
            return responseData;
        }

        public static async Task<string> DoSwitchAccountCreation(SwitchUser customerCreation)
        {
            string responseData = string.Empty;
            try
            {
                _apiService = new ApiRequest();
                var response = await _apiService.OnBoarding(customerCreation, "", URLConstants.switchAPINewBaseURL, "Switch/CompleteSignUp", "OnboardingAccountPINCreation");
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
            }
            return responseData;
        }

        public static async Task<string> UpgradeAccountForT24(UpgradeAccountForT24 upgradeAccountForT24)
        {
            string responseData = string.Empty;
            try
            {
                _apiService = new ApiRequest();
                var response = await _apiService.Post<UpgradeAccountForT24>(upgradeAccountForT24, "", URLConstants.SwitchApiBaseUrl, "Switch/UpgradeSwitchProfileForT24Acct", "OnboardingAccountPINVerificationPage");
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            catch(Exception ex)
            {
                string log = ex.Message;
            }
            return responseData;
        }

        public static async Task<string> LoginAfterOnboarding(LoginInfo model)
        {
            string responseData = string.Empty;
            try
            {
                _apiService = new ApiRequest();
                //var loginResponse = await _apiService.Post<dynamic>(model, "", URLConstants.SwitchApiBaseUrl, "Switch/Login2", "UnProfiledLoginPage");
                var loginResponse = await _apiService.Login(model, URLConstants.SwitchNewApiBaseUrl, "Token", "UnProfiledLoginPage");
                if (loginResponse.IsSuccessStatusCode)
                {
                    responseData = await loginResponse.Content.ReadAsStringAsync();
                }
            }
            catch(Exception ex) { string log = ex.Message; }
            return responseData;
        }
    }
}
