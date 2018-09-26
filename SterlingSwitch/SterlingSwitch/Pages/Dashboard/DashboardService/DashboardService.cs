using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using switch_mobile.Services.Abstractions.Entities;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using static SterlingSwitch.Services.Abstractions.Entities.DailyTipsResponseModel;

namespace SterlingSwitch.Pages.Dashboard.DashboardService
{
    public class DashboardService
    {
        private ApiRequest _apiService;
       
        public List<Tip> ApplicationTips { get; private set; }

        public List<DealsResponseModel.Deal> ApplicationDeals { get; private set; }
        public AccountAdviceResponse Advise { get; private set; }

        public int ApplicationTipsCount { get; private set; }

        public int ApplicationDealsCount { get; private set; }

        public DashboardService()
        {
            _apiService = new ApiRequest();  
           // Parallel.Invoke(GetApplicationDeals, GetApplicationTips);
        }

        public async Task<List<Tip>> GetApplicationTips()
        {
            var url = URLConstants.SwitchApiTestBaseUrl + "Switch/GetDailyTips";
            //Get Application Usage Tips.
            try
            {
                
                var response = await _apiService.Get(string.Empty, url, url, "DashboardService").ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var contentInString = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<List<Tip>>(contentInString);
                    ApplicationTips = deserializedResponse;
                    ApplicationTipsCount = deserializedResponse.Count;
                    ApplicationTips.ForEach(r=> { r.Name = r.Name.Replace("\r\n", ""); r.ImagePath = r.Name.Replace(" ", "") + ".png"; });
                    return ApplicationTips;
                }
            }
            catch (Exception e)
            {
               await  BusinessLogic.Log(e.ToString(), "GetApplicationTips", url, "Request Type = GET",string.Empty , "Dashboard");
            }                
            return new List<Tip>();
        }



        public async Task<List<DealsResponseModel.Deal>> GetApplicationDeals()
        {
            var url = URLConstants.SwitchApiTestBaseUrl + "Switch/GetAdverts";
            
            try
            {

                var response = await _apiService.Get(string.Empty, url, url, "DashboardService").ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var contentInString = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<List<DealsResponseModel.Deal>>(contentInString);
                    ApplicationDeals = deserializedResponse;
                    return ApplicationDeals;
                }
            }
            catch (Exception e)
            {
                await BusinessLogic.Log(e.ToString(), "Get Deals", url, "Request Type = GET", string.Empty, "Dashboard");
            }
            return  new List<DealsResponseModel.Deal>();
        }

        //public static IList<T> Shuffle<T>(IList<T> list)
        //{
        //     Random rng = new Random();
        //      int n = list.Count;
        //    while (n > 1)
        //    {
        //        n--;
        //        int k = rng.Next(n + 1);
        //        var value = list[k];
        //        list[k] = list[n];
        //        list[n] = value;
        //    }

        //    return list.ToList();
        //}

        public async Task<AccountAdviceResponse> GetGustomerAccountAdvice(string userId)
        {
            //userId = userId == "MVODOUNOU@OUTLOOK.COM"? "nabbo247@yahoo.com" : "nabbo247@yahoo.com";
            var url = URLConstants.SwitchApiBaseUrl + $"Transaction/GetAccountsAdvice?userId={userId}";
            var contentInString = string.Empty;    
            try
            {
                var response = await _apiService.GetWithSwitchId(userId, string.Empty, url, url, "DashboardService").ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    contentInString = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<AccountAdviceResponse>(contentInString);
                    Advise = deserializedResponse;
                    return Advise;
                }
            }
            catch (Exception e)
            {
              await  BusinessLogic.Log(e.ToString(), "Getting Safe to spend", url, url, contentInString, "Dashboard");
            }
            return new AccountAdviceResponse();
        }
    }
}
