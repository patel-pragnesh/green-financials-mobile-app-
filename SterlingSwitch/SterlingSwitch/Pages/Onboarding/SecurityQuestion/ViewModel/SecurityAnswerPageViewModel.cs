using Newtonsoft.Json.Linq;
using SterlingSwitch.Helper;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.ViewModelBase;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.Onboarding.SecurityQuestion.ViewModel
{
    public class SecurityAnswerPageViewModel : BaseViewModel
    {
        public SecurityAnswerPageViewModel(INavigation navigation) : base(navigation)
        {
            QuestionAndAnser = new ObservableCollection<QuestionAndAnswer>();
        }

        #region Properties
        public ObservableCollection<QuestionAndAnswer> QuestionAndAnser { get; set; }
        #endregion
        #region Events
        public async Task GetUserSecurityQuestionsAndAnswer(string email)
        {
            var pd = await ProgressDialog.Show("Please wait...");
            try
            {
                var api = new ApiRequest();
                var key = await Utilities.GetUniqueKey();
                var iv = Security.GetKeyByte(8);

                dynamic model = new JObject();
                model.UserId = email;
                model.Referenceid = Utilities.GenerateReferenceId();
                model.RequestType = 599;
                model.Translocation = GlobalStaticFields.GetUserLocation;


                var request = await api.Post(model: model, baseUrl: URLConstants.switchAPINewBaseURL, referenceUrl: "Spay/ValidateClientDevice", key: Security.GetKey(16), iv: iv, pageOrViewModel: "SecurityAnswerPageViewModel", isSensitive: false);

                if (request.IsSuccessStatusCode)
                {
                    string response = await request.Content.ReadAsStringAsync();
                    var apiresponse = JObject.Parse(response);
                    bool status = apiresponse.Value<bool>("Status");
                    string message = apiresponse.Value<string>("Message");
                    if (status)
                    {
                        QuestionAndAnser.Clear();
                        var data = JArray.Parse(apiresponse["Data"].ToString());
                        for (int i = 0; i < data.Count; i++)
                        {
                            QuestionAndAnser.Add(new QuestionAndAnswer
                            {
                                Answer = data[i].Value<string>("Answer"),
                                Question = data[i].Value<string>("Question"),
                                QuestionID = data[i].Value<int>("QuestionID"),
                            });
                        }
                        await pd.Dismiss();
                    }
                    else
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("OOPS", message, PopUps.DialogType.Error, "OK", null);
                    }

                }
                else
                {
                    string response = await request.Content.ReadAsStringAsync();
                    var apiresponse = JObject.Parse(response);
                    bool status = apiresponse.Value<bool>("Status");
                    string message = apiresponse.Value<string>("Message");
                    await pd.Dismiss();
                    MessageDialog.Show("OOPS", message, PopUps.DialogType.Error, "OK", null);
                }
               
            }
            catch (Exception ex)
            {
                await pd.Dismiss();
                MessageDialog.Show("OOPS", "Sorry, an unknown error occurred, please try again", PopUps.DialogType.Error, "OK", null);
                await BusinessLogic.Log(ex.ToString(), "Exception on GetUserSecurityQuestionsAndAnswer event", string.Empty, string.Empty, "Security Question Page", string.Empty);

            }


        }
        #endregion
    }
}
