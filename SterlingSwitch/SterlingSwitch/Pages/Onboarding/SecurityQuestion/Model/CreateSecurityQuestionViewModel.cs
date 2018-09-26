using FormsControls.Base;
using Newtonsoft.Json;
using SterlingSwitch.Helper;
using SterlingSwitch.Pages.Onboarding.Login;
using SterlingSwitch.Pages.Onboarding.SecurityQuestion.Service;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.ViewModelBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.Onboarding.SecurityQuestion.Model
{
    public class CreateSecurityQuestionViewModel : BaseViewModel
    {
        private ICryptoService _crypto = DependencyService.Get<ICryptoService>();
        public CreateSecurityQuestionViewModel(INavigation navigation) : base(navigation)
        {
            // SecurityQuestionModel = new SecurityQuestionModel();
            QuestionAndAnswer = new ObservableCollection<QuestionAndAnswer>();
            Questions = new List<QuestionData>();
        }

        // public SecurityQuestionModel SecurityQuestionModel { get; set; }

        #region Properties
        public ObservableCollection<QuestionAndAnswer> QuestionAndAnswer { get; set; }

        private string username = default(string);

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        private string password = default(string);

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        private string uniqueKey = default(string);

        public string UniqueKey
        {
            get => uniqueKey;
            set => SetProperty(ref uniqueKey, value);
        }

        private string device = default(string);

        public string Device
        {
            get => device;
            set => SetProperty(ref device, value);
        }

        private string imei = default(string);

        public string IMEI
        {
            get => imei;
            set => SetProperty(ref imei, value);
        }

        private string os = default(string);

        public string OS
        {
            get => os;
            set => SetProperty(ref os, value);
        }

        private bool isquestionLoaded = false;

        public bool IsQuestionLoaded
        {
            get => isquestionLoaded;
            set => SetProperty(ref isquestionLoaded, value);
        }



        public List<QuestionData> Questions { get; set; }
        #endregion

        #region Events
        public async Task SetQuestions()
        {
            var pd = await ProgressDialog.Show("Please wait...");
            try
            {
                var _service = new SecurityQuestionService();
                var questions = await _service.GetQuestions();
                foreach (var question in questions)
                {
                    Questions.Add(new QuestionData { ID = question.ID, Question = question.Question });
                }
                IsQuestionLoaded = true;
                //if (questions.Any())
                //{
                //    var question = new List<string>();
                //    questions.ForEach(r =>
                //    {
                //        question.Add(r.Question);
                //    });
                //    BindableQuestions.AddRange(question);
                //    Questions.AddRange(questions);
                //}
            }
            catch (Exception ex) 
            {
                IsQuestionLoaded = false;
            }
            finally
            {
                await pd.Dismiss();
            }

        }

        public async Task SubmitSecurityQuestions(string username, string password)
        {
            var pd = await ProgressDialog.Show("Please wait...");
            try
            {
                var api = new ApiRequest();
                var key = await Utilities.GetUniqueKey();
                var iv = Security.GetKeyByte(8);
                var onboardedquestions = new OnBoardedUserSecurityQuestionRequest
                {
                    Device = GlobalStaticFields.Device(),
                    IMEI = GlobalStaticFields.DeviceIMEI(),
                    OS = GlobalStaticFields.DeviceOS(),
                    Password = _crypto.Encrypt(password),
                    Username = username,
                    QuestionAnswers = QuestionAndAnswer.ToList(),
                    UniqueKey = key
                };
                var request =await api.Post<OnBoardedUserSecurityQuestionRequest>(model: onboardedquestions, baseUrl: URLConstants.switchAPINewBaseURL, referenceUrl: "Switch/SetOnBoardedUserSecurityQuestions", key: Security.GetKey(16), iv: iv, pageOrViewModel: "CreateSecurityQuestionViewModel", isSensitive: true);
                APIResponse<string> apiresponse = null;
                if (request.IsSuccessStatusCode)
                {
                    var response = await request.Content.ReadAsStringAsync();
                     apiresponse = JsonConvert.DeserializeObject<APIResponse<string>>(response);
                    if (apiresponse.Status)
                    {
                        //login
                        
                        var req = new LoginInfo()
                        {
                            UserID = onboardedquestions.Username,
                            Password = onboardedquestions.Password
                        };
                        var loginResponse = await api.Login(req, URLConstants.SwitchNewApiBaseUrl, "Token", "UnProfiledLoginPage");
                        if (loginResponse.IsSuccessStatusCode)
                        {
                            response = await loginResponse.Content.ReadAsStringAsync();
                            if (!string.IsNullOrEmpty(response))
                            {
                                //Get Token and save token lifespan
                                
                                await LoginHelper.LogUserDetails(response, Username);   // get user details in abstract class
                                if (GlobalStaticFields.LoginTest == "Passed")
                                {
                                    await pd.Dismiss();
                                    GlobalStaticFields.Customer.IsLoggedOn = true;

                                    Application.Current.MainPage = new AnimationNavigationPage(new Dashboard.Dashboard());
                                }
                                else
                                {
                                    await pd.Dismiss();
                                    //  MessageDialog.Show("OOPS", "Sorry, an unknown error occurred, please try t.", PopUps.DialogType.Error, "OK", null);
                                    Application.Current.MainPage = new AnimationNavigationPage(new UnProfiledLoginPage());
                                }
                            }
                        }
                        else
                        {
                            await pd.Dismiss();
                            Application.Current.MainPage = new AnimationNavigationPage(new UnProfiledLoginPage());
                        }
                    }
                    else
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("OOPS", apiresponse.Message, PopUps.DialogType.Error, "OK", null);
                      //  Application.Current.MainPage = new AnimationNavigationPage(new CreateSecurityQuestion(username, password));
                    }
                }
                else
                {
                    var response = await request.Content.ReadAsStringAsync();
                    apiresponse = JsonConvert.DeserializeObject<APIResponse<string>>(response);
                    await pd.Dismiss();
                    MessageDialog.Show("OOPS",apiresponse.Message, PopUps.DialogType.Error, "OK", null);
                   // Application.Current.MainPage = new AnimationNavigationPage(new CreateSecurityQuestion(username,password));
                }
               
            }
            catch (Exception ex)
            {
                await pd.Dismiss();
                MessageDialog.Show("OOPS", "Sorry, an unknown error occurred, please try again", PopUps.DialogType.Error, "OK", null);
                
                // Application.Current.MainPage = new AnimationNavigationPage(new CreateSecurityQuestion(username, password));
            }
            
        }
        #endregion
    }

    public class OnBoardedUserSecurityQuestionRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string UniqueKey { get; set; }
        public string Device { get; set; }
        public string IMEI { get; set; }
        public string OS { get; set; }
        public List<QuestionAndAnswer> QuestionAnswers { get; set; }
    }
}
