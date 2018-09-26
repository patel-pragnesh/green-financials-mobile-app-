using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SterlingSwitch.Pages.Onboarding.SecurityQuestion.Model;
using SterlingSwitch.Pages.Onboarding.SecurityQuestion.ViewModel;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using Xamanimation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Onboarding.SecurityQuestion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SecurityAnswerPage : SwitchMasterPage
    {
        SecurityAnswerPageViewModel vm;
        string email;
        string pass;
        public SecurityAnswerPage(string username,string password)
        {
            InitializeComponent();
            vm = new SecurityAnswerPageViewModel(Navigation);
            BindingContext = vm;
            email = username;
            pass = password;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (vm.QuestionAndAnser.Count > 0) return;
            await vm.GetUserSecurityQuestionsAndAnswer(email);

            QuestionOne.Text = vm.QuestionAndAnser.FirstOrDefault()?.Question;
            QuestionTwo.Text = vm.QuestionAndAnser.LastOrDefault()?.Question;
        }
        private void Cross_OnTapped(object sender, EventArgs e)
        {
            Navigation.PopAsync(true);
        }

        private void BtnStep1_OnClicked(object sender, EventArgs e)
        {
            try
            {

                var ans = stepOneAnswer.Text;
                var question = QuestionOne.Text;
                if(!ValidateQuestion(question,ans))
                {
                    MessageDialog.Show("OOPS", "Incorrect answer", PopUps.DialogType.Error, "OK", null);
                    return;
                }

                grdStep1.IsVisible = false;
                grdStep2.Animate(new FadeInAnimation());
                grdStep2.IsVisible = true;

                //grdStep1.Animate(new FadeOutAnimation());
            }
            catch (Exception ex)
            {
                BusinessLogic.Log(ex.ToString(), "Exception on security question picker event", string.Empty, string.Empty, "Security Question Page", string.Empty);
            }

        }

        /// <summary>
        /// Previous Question
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Next_OnClicked(object sender, EventArgs e)
        {
            //grdStep2.Animate(new FadeOutAnimation());
            grdStep2.IsVisible = false;
            grdStep1.Animate(new FadeInAnimation());
            grdStep1.IsVisible = true;
        }

        private async void BtnStep2_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var ans = stepTwoAnswer.Text;
                var question = QuestionTwo.Text;
                if (!ValidateQuestion(question, ans))
                {
                    MessageDialog.Show("OOPS", "Incorrect answer", PopUps.DialogType.Error, "OK", null);
                    return;
                }

                //Add device to user
                var questModel = new CreateSecurityQuestionViewModel(Navigation);
                foreach (var item in vm.QuestionAndAnser)
                {
                    questModel.QuestionAndAnswer.Add(new QuestionAndAnswer
                    {
                        Answer = item.Answer,
                        Question = item.Question,
                        QuestionID = item.QuestionID
                    });
                }
                await questModel.SubmitSecurityQuestions(email, pass);
            }
            catch (Exception ex)
            {
              await  BusinessLogic.Log(ex.ToString(), "Exception on security question picker event", string.Empty, string.Empty, "Security Question Page", string.Empty);
            }

        }

        bool ValidateQuestion(string question,string answer)
        {
            return vm.QuestionAndAnser.Any(x => x.Question == question && x.Answer == answer);
        }
    }
}