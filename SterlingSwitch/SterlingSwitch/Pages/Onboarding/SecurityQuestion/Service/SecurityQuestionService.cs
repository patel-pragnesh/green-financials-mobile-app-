using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SterlingSwitch.Pages.Onboarding.SecurityQuestion.Model;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;

namespace SterlingSwitch.Pages.Onboarding.SecurityQuestion.Service
{
    public class SecurityQuestionService
    {
        private ApiRequest _apiService;
        public SecurityQuestionService()
        {
              _apiService = new ApiRequest();
        }

        public async Task<List<QuestionData>> GetQuestions()
        {
            try
            {
                var response = await _apiService.Get(string.Empty, URLConstants.switchAPINewBaseURL,
                    "Switch/GetSecurityQuestions", "Security question service");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var contentInString = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<SecurityQuestionModel>(contentInString);
                    return deserializedResponse.Data.ToList();
                }
            }
            catch (Exception e)
            {
                var log = e.Message;
            }
            return new List<QuestionData>();
        }
    }

    
}
