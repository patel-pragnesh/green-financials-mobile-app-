using Newtonsoft.Json;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SterlingSwitch.Pages.BillsPayment.Service
{
    public static class BillerServices
    {       
        private static ApiRequest _apiService;


        public static async Task<List<Biller>> GetBillersAsync()
        {
            _apiService = new ApiRequest();
            try
            {
                var response = await _apiService.Get("", URLConstants.SwitchApiLiveBaseUrl, "Switch/GetBillers", "PayBillsPage");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var billers = JsonConvert.DeserializeObject<List<Biller>>(content);
                    return billers;
                }
            }
            catch (Exception ex)
            {
                
            }
            return new List<Biller>();
        }

        public static async Task<List<Biller>> GetDataBillerListAsync()
        {
            try
            {

            }
            catch (Exception ex)
            {

                return new List<Biller>();
            }
            return new List<Biller>();
        }

        public static async Task<List<BillerProduct>> GetBillerProducts()
        {
            _apiService = new ApiRequest();
            try
            {
                var response = await _apiService.Get("", URLConstants.SwitchApiLiveBaseUrl, "Switch/GetBillerProducts", "PayBillsPage");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var billerProducts = JsonConvert.DeserializeObject<List<BillerProduct>>(content);
                    return billerProducts;
                }
            }
            catch (Exception ex)
            {

                return new List<BillerProduct>();
            }
            return new List<BillerProduct>();
        }

        public static async Task<BillerPaymentResponse> PayBiller(PayBillerPost payBillerPost)
        {
            _apiService = new ApiRequest();
            try
            {
                var response = await _apiService.Post<PayBillerPost>(payBillerPost, "", URLConstants.SwitchApiBaseUrl, "Spay/BillPaymtAdviceRequestISW", "PayBillsPage");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var payResponse = JsonConvert.DeserializeObject<BillerPaymentResponse>(content);
                    return payResponse;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;          
            }
            return new BillerPaymentResponse();
        }

        public static async Task<List<Category>> GetBillerCategories()
        {
            _apiService = new ApiRequest();
            try
            {
                var response = await _apiService.Get("", URLConstants.SwitchApiLiveBaseUrl, "Switch/QuickTellerCategories", "PayBillsPage");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var category = JsonConvert.DeserializeObject<List<Category>>(content);
                    if(category.Count > 0)
                    {
                        GlobalStaticFields.BillerCategories = category;
                    }
                    return category;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
            }
            return new List<Category>();
        }

        public static async Task<List<BillerListResponse>> GetBillerBillersByCategory(BillerPost categoryId)
        {
            _apiService = new ApiRequest();
            try
            {
                var response = await _apiService.Post<BillerPost>(categoryId, "", URLConstants.SwitchApiBaseUrl, "Switch/QuickTellerBillers", "PayBillsPage");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var billers = JsonConvert.DeserializeObject<List<BillerListResponse>>(content);
                    return billers;
                }
            }
            catch(Exception ex) { }
            return new List<BillerListResponse>();
        }

        public static async Task<List<BillerItemList>> GetBillerProducts(BillerItemPost billerItemPost)
        {
            _apiService = new ApiRequest();
            try
            {
                var response = await _apiService.Post<BillerItemPost>(billerItemPost, "", URLConstants.SwitchApiBaseUrl, "Switch/QuickTellerBillerItems", "PayBillsPage");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var billersProducts = JsonConvert.DeserializeObject<List<BillerItemList>>(content);
                    return billersProducts;
                }
            }
            catch(Exception ex) { }
            return new List<BillerItemList>();
        }


        public static async Task<List<BillerListResponse>> GetBillerDataByCategory()
        {
            _apiService = new ApiRequest();
            try
            {

            }
            catch(Exception ex) { }

            return new List<BillerListResponse>();
        }
    }
}
