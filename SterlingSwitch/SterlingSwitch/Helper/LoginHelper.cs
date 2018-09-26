using Newtonsoft.Json;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SterlingSwitch.Helper
{
    public static class LoginHelper
    {
       
        public async static Task LogUserDetails(string loginResponse, string userID)
        {
            ApiRequest httpService = new ApiRequest();
            IDevice _deviceId = DependencyService.Get<IDevice>();
            var response = "";
            try
            {               
                if (!string.IsNullOrEmpty(loginResponse))
                {
                    if (loginResponse == "true")
                    {
                        var userObj = new preferencesCustomer()
                        {
                            UserID = userID
                        };
                        var request = await httpService.Post<preferencesCustomer>(userObj, "", URLConstants.SwitchApiBaseUrl, "Switch/GetUserInfo", "LoginHelper");
                        if (request.IsSuccessStatusCode)
                        {
                            response = await request.Content.ReadAsStringAsync();

                            if (!string.IsNullOrEmpty(response))
                            {
                                var json2 = JsonConvert.DeserializeObject<LoginResponse>(response);
                                if (json2.IsAccountLock == false)
                                {
                                    GlobalStaticFields.Customer.AccountNumber = json2.WalletAcct;
                                    GlobalStaticFields.Customer.PhoneNumber = json2.PhoneNumber;
                                    GlobalStaticFields.Customer.Email = json2.UserEmail;
                                    GlobalStaticFields.Customer.IsLoggedIn = true;
                                    GlobalStaticFields.Customer.FirstName = json2.FirstName;
                                    GlobalStaticFields.Customer.LastName = json2.LastName;
                                    GlobalStaticFields.Customer.BirthDate = json2.DateOfBirth;
                                    GlobalStaticFields.Customer.MiddleName = json2.MiddleName;
                                    GlobalStaticFields.Customer.Gender = json2.Gender;
                                    GlobalStaticFields.Customer.ProfilePix = json2.ProfilePix;
                                    GlobalStaticFields.Customer.BVN = json2.BVN == null ? "" : json2.BVN;
                                    GlobalStaticFields.Customer.HomeAddress = json2.HomeAddress ?? "N/A";
                                    GlobalStaticFields.Customer.WalletAcctNo = json2.WalletAcct;
                                    GlobalStaticFields.Customer.CustomerId = json2.customerID;
                                    GlobalStaticFields.Customer.IsTPin = json2.IsTPIN;
                                    GlobalStaticFields.Customer.IsAccountLock = json2.IsAccountLock;
                                    if (json2.Parthian == "true")
                                    {
                                        GlobalStaticFields.UserCreationOnParthian = 1;
                                    }
                                    else
                                    {
                                        GlobalStaticFields.UserCreationOnParthian = 0;
                                    }
                                    GlobalStaticFields.LoginTest = "Passed";
                                    string message = $"Dear {GlobalStaticFields.Customer.FirstName} There has been a recent login with your switch profile. If this was done by you, please ignore this notification. However if it wasnt you, we urge you to login and change your PIN";
                                    BusinessLogic.SendEmail(GlobalStaticFields.Customer.Email, message);
                                    //ReBindPrimaryAccountDetail();
                                    BusinessLogic.RebindAccountDetails();
                                }
                                else
                                {
                                    GlobalStaticFields.LoginTest = "Failed";
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        var accessTokeResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(loginResponse);

                        var json = JsonConvert.DeserializeObject<LoginResponse>(accessTokeResponse["AccountInfo"]);
                        if (json.Status == true && json.IsAccountLock == false)
                        {
                            //Save Token
                            GlobalStaticFields.Token = accessTokeResponse["access_token"];
                            GlobalStaticFields.TokenExpired = accessTokeResponse[".expires"];
                            GlobalStaticFields.TokenIssued = accessTokeResponse[".issued"];
                            GlobalStaticFields.Customer.AccountNumber = json.NUBAN ?? json.WalletAcct;
                            GlobalStaticFields.Customer.PhoneNumber = json.PhoneNumber;
                            GlobalStaticFields.Customer.Email = json.UserEmail;
                            GlobalStaticFields.Customer.IsLoggedIn = true;
                            GlobalStaticFields.Customer.FirstName = json.FirstName;
                            GlobalStaticFields.Customer.LastName = json.LastName;
                            GlobalStaticFields.Customer.BirthDate = json.DateOfBirth;
                            GlobalStaticFields.Customer.MiddleName = json.MiddleName;
                            GlobalStaticFields.Customer.Gender = json.Gender;
                            GlobalStaticFields.Customer.ProfilePix = json.ProfilePix;
                            GlobalStaticFields.Customer.BVN = json.BVN == null ? "" : json.BVN;
                            GlobalStaticFields.Customer.HomeAddress = json.HomeAddress ?? "N/A";
                            GlobalStaticFields.Customer.ReferralCode = json.ReferralCode;
                            GlobalStaticFields.Customer.UserID = json.UserId;
                            GlobalStaticFields.Customer.WalletAcctNo = json.WalletAcct;
                            GlobalStaticFields.Customer.CustomerId = json.customerID ?? "";
                            GlobalStaticFields.Customer.IsTPin = json.IsTPIN;
                            GlobalStaticFields.Customer.IsAccountLock = json.IsAccountLock;
                            if (json.Parthian == "true")
                            {
                                GlobalStaticFields.UserCreationOnParthian = 1;
                            }
                            else
                            {
                                GlobalStaticFields.UserCreationOnParthian = 0;
                            }
                            GlobalStaticFields.LoginTest = "Passed";
                            string message = $"Dear {GlobalStaticFields.Customer.FirstName}\n There has been a recent login with your switch profile. If this was done by you, please ignore this notification. However if it wasnt you, we urge you to login and change your PIN";
                            BusinessLogic.SendEmail(GlobalStaticFields.Customer.Email, message);
                            // ReBindPrimaryAccountDetail();
                            BusinessLogic.RebindAccountDetails();
                        }
                        else
                        {
                            GlobalStaticFields.LoginTest = "Failed";
                            return;                            
                        }
                    }
                   
                }
            }
            catch (Exception ex)
            {
                await BusinessLogic.Log(ex.ToString(), "Exception on Logon", "Switch/GetUserInfo", userID, response, "LoginPage");
            }
        }

        public static async void ReBindPrimaryAccountDetail()
        {
            GlobalStaticFields.Customer.ListOfAllAccounts = await GlobalStaticFields.Customer.GetAccountsbyPhoneNumber(GlobalStaticFields.Customer.PhoneNumber);
        }

        
    }
}
