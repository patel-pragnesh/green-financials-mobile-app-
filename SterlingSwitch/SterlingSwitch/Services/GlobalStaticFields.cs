using Microsoft.AppCenter.Push;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.DeviceInfo;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using SterlingSwitch.Helper;
using SterlingSwitch.Models;
using SterlingSwitch.Pages.BillsPayment.Service;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.Repository;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.ViewModelBase;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SterlingSwitch.Services
{
    public static class GlobalStaticFields
    {
        public static Customer Customer { get; set; }
        public static bool IsCreditCard { get; set; }
        public static int UserCreationOnParthian { get; set; }
        public static double ScreenWidth;
        public static double ScreenHeight;
        public static string AccountType { get; set; }
        public static string ExistingCustomerAccountNo { get; set; }
        public static string CurrentBalance { get; set; }
        public static string LoginTest { get; set; }
        public static bool LoginFromRegistration { get; set; }
        public static string ArrangementId { get; set; }
        public static string longtitude { get; set; }
        public static string latitude { get; set; }
        public static TokenResponse tokenFullobject { get; set; }
        public static string TransactionSelected { get; set; }
        public static bool IsBalanceHidden { get; set; } = false;
        public static string Token
        {
            get { return Acr.Settings.CrossSettings.Current.Get<string>("token"); }
            set { Acr.Settings.CrossSettings.Current.Set<string>("token", value); }
        }
        public static string TokenIssued
        {
            get { return Acr.Settings.CrossSettings.Current.Get<string>("tokenissued"); }
            set { Acr.Settings.CrossSettings.Current.Set<string>("tokenissued", value); }
        }
        public static string TokenExpired
        {
            get { return Acr.Settings.CrossSettings.Current.Get<string>("tokenexpired"); }
            set { Acr.Settings.CrossSettings.Current.Set<string>("tokenexpired", value); }
        }
        public static ObservableRangeCollection<CustomerAccount> CustomersTotalAccount { get; set; }

        public static string LoginSuccess { get; internal set; }
        public static ObservableCollection<SaveSwitchBeneficiary.GetSavedBeneficiaries> ListOfTransferbeneficiaries { get; set; }
        public static ObservableCollection<string> ParticipatingBankNameList { get; set; }
        public static ObservableCollection<Banklist> ParticipatingBankList { get; set; }
        public static ObservableCollection<string> BeneficiaryConvToString { get; set; }
        public static List<Category> BillerCategories { get; set; }
        public static SpectaDropdown SpectaDropdownList { get; set; }
        public static string PreviousPage { get; set; }
        public static string SelectedRecipientDetails { get; set; }
        public static string TokenAppID = "51";
        public static bool TPINToSet { get; set; }
        public static bool resetPin { get; set; }


        public static string GetUserLocation
        {
            get
            {
                try
                {
                    return latitude + "," + longtitude;
                }
                catch (Exception ex)
                {
                    string log = ex.ToString();
                    return "N/A";
                }
            }
        }

        public static async Task _getDevice()
        {
            try
            {

                if (App.Current.Properties.ContainsKey("email"))
                {
                    Customer.Email = App.Current.Properties["email"].ToString(); // as string;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
            }
        }
        public static string GetUniqueID { get; set; }
        public async static Task StoreUniqueID()
        {
            
            try
            {
                System.Guid? installId = await Microsoft.AppCenter.AppCenter.GetInstallIdAsync();
                GetUniqueID = installId.ToString();
               
            }
            catch (Exception)
            {
                GetUniqueID = "exception";
               
            }
         
        }
        public static async Task getBalanceStatus()
        {
            try
            {
                if (App.Current.Properties.ContainsKey("ShowBalanceStatus"))
                {
                    IsBalanceHidden = (bool)App.Current.Properties["ShowBalanceStatus"];
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                //  await BusinessLogic.LogExceptionOrRemarkAsync(ex.ToString(), "Exception on hidebalance check");
            }
        }

        public static async Task _setLifeStyle()
        { //
          // await LifeStyleRepository.SetMovieCollections();
          // await LifeStyleRepository.SetEventCollections();
        }


        public static bool IsLocationAvailable()
        {
            return CrossGeolocator.Current.IsGeolocationAvailable;
        }

        public static async Task<Position> GetLocationFromPlugin()
        {
            var pos = new Position();

            try
            {
                var location = IsLocationAvailable();

                if (!location)
                {
                    longtitude = "00";
                    latitude = "00";
                    return new Position();
                }
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5), null, true);
                if ((position != null))
                {
                    pos = position;
                    longtitude = position.Longitude.ToString();
                    latitude = position.Latitude.ToString();
                }
            }
            catch (Exception ex)
            {
                longtitude = "00";
                latitude = "00";
                string log = ex.ToString();
                return new Position();
            }

            return pos;
        }


        public static string Username
        {

            get
            {
                try
                {
                    return App.Current.Properties["username"] == null ? "N/A" : App.Current.Properties["username"].ToString();
                }
                catch (Exception v)
                {
                    string log = v.ToString();

                    return "N/A";

                }

            }
        }

        public static participatingBanks ListOfParticipatingBanks { get; set; }
        public static List<Banklist> ListOfbanks { get; set; }
        public static List<string> PagesVisited = new List<string>();
        public static List<PageName> PagesVisitedList = new List<PageName>();
        public static List<BAnkNameCode> BankNameAndCode = new List<BAnkNameCode>();//this is the calculated one
        public static void SetSleepTimer()
        {
            if (App.Current.Properties.ContainsKey("sleeper"))
            {
                DateTime sleepTime = (DateTime)App.Current.Properties["sleeper"];
                var seconds = System.Math.Abs((DateTime.UtcNow - sleepTime).TotalSeconds);
                if (seconds > 180 && Customer.IsLoggedOn)
                {
                    // App.ShowPopup("Session timeout", "You have been logged out after three minutes of inactivity", PopUps.DialogType.Info, "Got it");
                }
                else
                {
                    App.Current.Properties["sleeper"] = DateTime.UtcNow;
                }
            }
            else
            {
                App.Current.Properties["sleeper"] = DateTime.UtcNow;
            }
        }

        public enum TransactionType
        {
            OneTimePayment,
            FuturePayment,
            StandingOrderPayment,
        }

        public static void ConfigureAppCenter()
        {
            if (!Microsoft.AppCenter.AppCenter.Configured)
            {
                Push.PushNotificationReceived += (sender, e) =>
                {
                    var summary = $"Push notification received:" +
                                   $"\n\tNotification title: {e.Title ?? ""}" +
                                   $"\n\tMessage: {e.Message ?? ""}";
                    if (e.CustomData != null)
                    {
                        summary += "\n\tCustom data:\n";
                        foreach (var key in e.CustomData.Keys)
                        {
                            summary += $"\t\t{key} : {e.CustomData[key]}\n";
                        }
                    }
                    System.Diagnostics.Debug.WriteLine(summary);

                };
            }

        }
        public static async Task<bool> SendOTP()
        {
            bool result = false;
            try
            {
                var otpRequest = new OTPRequestMobile()
                {
                    mobile = Customer.PhoneNumber,
                    Referenceid = Utilities.GenerateReferenceId(),
                    RequestType = 142,
                    Translocation = GetUserLocation,
                    email = Customer.Email
                };

                string baseUrl = URLConstants.SwitchApiLiveBaseUrl;
                string endpoint = "spay/OTPRequestMobile";
                string url = baseUrl + endpoint;

                var apirequest = new ApiRequest();

                var request = await apirequest.Post(otpRequest, "", baseUrl, endpoint, "SendOTP()");
                var response = await request.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(response))
                {
                    var json = JsonConvert.DeserializeObject<StatusMessage>(response);
                    if (json.response == "00")
                        result = true;
                    else
                        result = false;
                }
            }
            catch (Exception ex)
            {
                await BusinessLogic.Log(ex.ToString(), "exception response gotten for calling api ", "OTPRequestMobile ", "", "", "SendOTP()");
                result = false;
            }

            return result;
        }

        public static async Task<bool> ValidateOTP(string OTP)
        {
            bool result = false;
            try
            {
                string baseUrl = URLConstants.SwitchApiLiveBaseUrl;
                string endpoint = "spay/ValOTPRequestMobile";
                string url = baseUrl + endpoint;

                var otpRequest = new ValOtpRequestMobile()
                {
                    mobile = Customer.PhoneNumber,
                    otp = OTP,
                    Referenceid = Utilities.GenerateReferenceId(),
                    RequestType = "143",
                    Translocation = GetUserLocation
                };
                var apirequest = new ApiRequest();
                var request = await apirequest.Post(otpRequest, "", baseUrl, endpoint, "ValidateOTP()");

                var response = await request.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(response))
                {
                    var json = JsonConvert.DeserializeObject<StatusMessage>(response);
                    if (json.response == "00")
                        result = true;
                    else
                        result = false;
                }
            }
            catch (Exception ex)
            {
                await BusinessLogic.Log(ex.ToString(), " exception response calling end point", "ValOTPRequestMobile", "", "", "ValidateOTP()");
                string log = ex.Message;
                result = false;
            }

            return result;
        }

        public static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        /// <summary>
        /// Modulus
        /// </summary>
        /// <returns></returns>
        public static string SHaredPublicKey()
        {
            string valueKey = "xKWS9hPrBRpcl4PYzbUWMQyLrcwiZAjR89z5PZk/JK3i6FuogpCdaSt1IEVoGV6i/X35ibnyKKJ7UNQDdy80w1dw9XQb/p3/MKExMROabm7MA/FJ7uBfQy8N34Exr2vbcf+lRPwZy1uC0O+aSH/w/k/zcCGFssdRdYM2tH2ndcwFuh5EHTpQvOX5CeG6Uf+ErIciz9CwKrawenGGNNMdK8lcyhO/IIlVYVk27uv/M3Hqi432Vzx8xXwC0Q/hc+2V2qdWUoWdK1901ulkRI9dqczX9cTh1lhOlav6pzpMMhfHg3qxWCuKIqR4oBLOmsI+Di90vtMlBrH9cnoMukn9pMhOQ2SLUsa+MA/z1nim9K5U27gMn3BwSIbaOoUVoNDhK16bdMx8dgnXPAnIUXnVwTSEbtAgEURpW7b/pky+48p5iyqnEm0qSw9jTtYlIc/55AEZaRgApSNRga+R80ZeNU8PF25SMc2QphpEgUOXng8aBU3kF/m0ALFhhIg7O+tw586i8y1EwSrQwoqSuxvkHfqB218AXbydiJwmYswEuu/tezwHiP5DgZDlUn4wt1ETl89mWOL0+gVffklt+WUAec1o36cAc0/J+muq07ysZzCSSifvvQXnVN304wcYK5Tzi0OcFUHwp885kFTFJLW2IOqdYGkzdxkPVIJ0V5mMRFU=";
            return valueKey;
        }
        public static string Exponent()
        {
            return "AQAB";
        }
        public static string DeviceIMEI()
        {
            string imei = CrossDevice.Device.DeviceId.ToUpper();
            return imei;
        }
        public static string DeviceOS()
        {
            string os = CrossDevice.Device.OperatingSystem.ToString();
            return os;
        }

        public static string Device()
        {
            string device = CrossDevice.Device.Model.ToString();
            return device;
        }
    }
}
