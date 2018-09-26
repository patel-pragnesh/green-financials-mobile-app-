using Newtonsoft.Json;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SterlingSwitch.Pages.Specta.Service
{
    public static class SpectaService
    {
        private static ApiRequest _apiService;

        public static async Task<SpectaDropdown> GetSpectaDropdoown()
        {
            _apiService = new ApiRequest();
            try
            {
                var response = await _apiService.Get("", URLConstants.SpectaAPILiveBaseUrl, "GetAllDropdownList", "QuickLoan");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var dropdownItems = JsonConvert.DeserializeObject<SpectaDropdown>(content);
                    if(dropdownItems != null)
                    {
                        GlobalStaticFields.SpectaDropdownList = dropdownItems;
                    }
                    return dropdownItems;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
            }
            return new  SpectaDropdown();
        }

        public static async Task<string> GetEligibleAmount(string accomdationId, string maritalStatus, string numberOfDependants, string jobChangeCount, string howLongInResidence, string empName, string empAddress, string gender, string tenor, string reqAmtval, string fname, string lname, string address, string dob, string salaryAmt, string phoneNumber, string bvn, string email, string isCustomer, string acctNo)
        {
            string eligibleStatus = "";
            _apiService = new ApiRequest();
            try
            {
                string url = $"GetEligibleamt?accomdationId={accomdationId}&maritalStatus={maritalStatus}&numberOfDependants={numberOfDependants}&jobChangeCount={jobChangeCount}&howLongInResidence={howLongInResidence}&empName={empName}&empAddress={empAddress}&gender={gender}&tenor={tenor}&reqAmtval={reqAmtval}&fname={fname}&lname={lname}&address={address}&dob={dob}&salaryAmt={salaryAmt}&phoneNumber={phoneNumber}&bvn={bvn}&email={email}&isCustomer={isCustomer}&acctNo={acctNo}";
                var response = await _apiService.Get("", URLConstants.SpectaAPILiveBaseUrl, url, "QuickLoan");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var eligibility = JsonConvert.DeserializeObject<SpectaEligibleAmountResponse>(content);
                    if (eligibility.message.ToLower() == "Successful".ToLower())
                    {
                        SpectaProcess.eligibleAmt = eligibility.eligibleAmount.ToString();
                        SpectaProcess.monthlyRepayment = eligibility.monthlyRepayment.ToString();
                        SpectaProcess.eligibleAmount = eligibility.eligibleAmount.ToString();
                        SpectaProcess.repaymentDay = eligibility.repaymentDay;
                        eligibleStatus = "Success";
                    }
                    else
                        eligibleStatus = "Failed";
                    
                }
                return eligibleStatus;
               
            }
            catch (Exception ex)
            {
                await BusinessLogic.Log(ex.ToString(), "Exception gotten from GetEligibleAMount specta", "GetEligibleAmount", "", "", "QuickLoan");
                return eligibleStatus;
            }
        }

        public static async Task<bool> GetOtpSpectaOTP(string nuban, string email)
        {
            bool isOTPSent = false;
            _apiService = new ApiRequest();
            try
            {
                string url = $"GetOtp?nuban={nuban}&email={email}";
                var response = await _apiService.Get("", URLConstants.SpectaAPILiveBaseUrl, url, "QuickLoan");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var IsSuccessful = JsonConvert.DeserializeObject<spectaOtp>(content);
                    if (IsSuccessful.isOtpSent == true)
                    {
                        isOTPSent = true;
                    }
                    else
                        isOTPSent = false;
                }
                return isOTPSent;
            }
            catch (Exception ex)
            {
                await BusinessLogic.Log(ex.ToString(), "Exception gotten when sending OTP", "GetEligibleAmount", "", "", "QuickLoan");
                return isOTPSent;
            }
        }

        public static async Task<string> SpectaFinal(SpectaProcess2 spectaProcess)
        {
            string IsSuccessful = "";
            _apiService = new ApiRequest();
            try
            {
                var response = await _apiService.Post<SpectaProcess2>(spectaProcess, "", URLConstants.SpectaAPILiveBaseUrl, "Process", "FinishQuickLoan");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var resp = JsonConvert.DeserializeObject<ProcessStatus>(content);
                    IsSuccessful = resp.status.ToString();
                }
                return IsSuccessful;
            }
            catch(Exception ex)
            {
                return IsSuccessful;
            }
        }

        // calls omitted:: NUBAN validator (Correct NUBAN was gotten from user's account), BVN validator (Correct BVN was gotten from customer's profile)
    }
}
