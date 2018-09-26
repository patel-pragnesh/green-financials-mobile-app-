using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;

namespace SterlingSwitch.Pages.AirtimeAndData.Service
{
    public class AirtimeDataService
    {
        private ApiRequest _apiService;

        public AirtimeDataService()
        {
            _apiService = new ApiRequest();
        }

        public async Task<List<NetworkProviderModel.Provider>> GetAvailableProviders()
        {
            var url = URLConstants.SwitchApiBaseUrl + "Switch/GetNetworkProvider";
            try
            {
                var response = await _apiService.Get(string.Empty, url, url, "AirtimeDate Service");
                if (response.IsSuccessStatusCode)
                {
                    var contentInString = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<List<NetworkProviderModel.Provider>>(contentInString);
                    deserializedResponse.ForEach(t =>
                    {
                        if (t.MobileNetwork == "ETISALAT")
                        {
                            t.MobileNetwork = "9MOBILE";
                        }
                    });
                    return deserializedResponse;
                }
            }
            catch (Exception e)
            {
                await BusinessLogic.Log(e.ToString(), "GetAvailableProviders", url, "Request Type = GET", string.Empty, "AirtimeDate Service");
            }
            return new List<NetworkProviderModel.Provider>();
        }

        public async Task<List<BillerPostResponseModel.Biller>> GetDateBundleProviders()
        {
            var url = URLConstants.SwitchApiBaseUrl + "Switch/QuickTellerBillers";
            var request = new BillerPostRequestModel {billerId = "5"};
            try
            {
                var response = await _apiService.Post(request, string.Empty, url, url, "Airtime Service");
                if (response.IsSuccessStatusCode)
                {
                    var contentInString = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<List<BillerPostResponseModel.Biller>>(contentInString);
                    deserializedResponse = deserializedResponse.Where(b =>
                           b.CustomerFieldLabel.ToLower().Contains("phone number")
                        || b.CustomerFieldLabel.ToLower().Contains("telephone number")
                        || b.CustomerFieldLabel.ToLower().Contains("mobile number")
                        || b.CustomerFieldLabel.ToLower().Contains("username")).ToList();

                    return deserializedResponse;
                }
            }
            catch (Exception e)
            {
                await BusinessLogic.Log(e.ToString(), "QuickTellerBillers", url, "Request Type = POST", string.Empty, "AirtimeDate Service");
            }
            return new List<BillerPostResponseModel.Biller>();
        }

        public async Task<List<BillerItemResponse.Item>> GetBillerItem(string itemId)
        {
            var url = URLConstants.SwitchApiBaseUrl + "Switch/QuickTellerBillerItems";
            var request = new BillerItemRequestModel {ItemId = itemId};
            try
            {
                var response = await _apiService.Post(request, string.Empty, url, url, "AirtimeData Service");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var contentInString = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<List<BillerItemResponse.Item>>(contentInString);
                    return deserializedResponse;
                }
            }
            catch (Exception e)
            {
                await BusinessLogic.Log(e.ToString(), "QuickTellerBillerItems", url, "Request Type = POST", string.Empty, "AirtimeDate Service");
            }
            return new List<BillerItemResponse.Item>();
        }

        public async Task<BuyDataResponseModel> BuyDatabundle(BuyDataRequestModel request)
        {
            try
            {
                var response = await _apiService.Post(request, string.Empty, URLConstants.SwitchApiBaseUrl, "spay/BillPaymtAdviceRequestISW", "AirtimeData Service");
                if (response.IsSuccessStatusCode)
                {
                    var contentInString = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<BuyDataResponseModel>(contentInString);
                    return deserializedResponse;
                }
            }
            catch (Exception e)
            {
                await BusinessLogic.Log(e.ToString(), "QuickTellerBillerItems", URLConstants.SwitchApiBaseUrl+"spay/BillPaymtAdviceRequestISW", "Request Type = POST", string.Empty, "AirtimeDate Service");
            }
            return new BuyDataResponseModel();
        }

        public async Task<string> BuyAirtime(AirtimeRequest request)
        {      
            try
            {
                var response = await _apiService.PostIBS(request, string.Empty, URLConstants.SwitchApiBaseUrl, "IBSIntegrator/IBSBridge", "AirtimeData Service");
                if (response.IsSuccessStatusCode)
                {
                    var contentInString = await response.Content.ReadAsStringAsync();
                    //var deserializedResponse = JsonConvert.DeserializeObject<BuyDataResponseModel>(contentInString);
                    return contentInString;
                }
            }
            catch (Exception e)
            {
                await BusinessLogic.Log(e.ToString(), "QuickTellerBillerItems", URLConstants.SwitchApiBaseUrl+ "IBSIntegrator/IBSBridge", "Request Type = POST", string.Empty, "AirtimeDate Service");
            }
            return string.Empty;
        }

        

    }
}
