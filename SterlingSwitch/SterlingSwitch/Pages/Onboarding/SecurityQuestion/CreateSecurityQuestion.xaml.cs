using SterlingSwitch.Pages.Onboarding.SecurityQuestion.Model;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using System;
using System.Linq;
using Xamanimation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Onboarding.SecurityQuestion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateSecurityQuestion : SwitchMasterPage
    {
        private CreateSecurityQuestionViewModel vm;
        string _username = "";
        string _password = "";
        public CreateSecurityQuestion(string username,string password)
        {
            InitializeComponent();
            vm = new CreateSecurityQuestionViewModel(Navigation);
            BindingContext = vm;
            _username = username;
            _password = password;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!vm.IsQuestionLoaded)
                await vm.SetQuestions();

            stepOnePicker.ItemsSource = vm.Questions.Select(x => x.Question).ToList();

            
        }

      
        private void btnStep1_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(stepOnePicker.SelectedItem))
            {
                MessageDialog.Show(string.Empty, "Please select a question to proceed", DialogType.Error, "Ok", null);
                return;
            }
            if (string.IsNullOrEmpty(stepOneAnswer.Text))
            {
                MessageDialog.Show(string.Empty, "Please provide an answer to the selected question to proceed", DialogType.Error, "Ok", null);
                return;
            }

            var question = vm.Questions.FirstOrDefault(x => x.Question == stepOnePicker.SelectedItem);
            vm.QuestionAndAnswer.Add(new QuestionAndAnswer() { Answer = stepOneAnswer.Text.Trim(), QuestionID = question.ID });
            grdStep1.IsVisible = false;
            grdStep2.Animate(new FadeInAnimation());
            grdStep2.IsVisible = true;
            stepTwoPicker.ItemsSource = vm.Questions.Where(x=> !string.Equals(x.Question,question.Question)).Select(x=>x.Question).ToList();
        }

        private async void btnSubmit_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(stepTwoPicker.SelectedItem))
                {
                    MessageDialog.Show(string.Empty, "Please select a question to proceed", DialogType.Error, "Ok", null);
                    return;
                }
                if (string.IsNullOrEmpty(stepTwoAnswer.Text))
                {
                    MessageDialog.Show(string.Empty, "Please provide an answer to the selected question to proceed", DialogType.Error, "Ok", null);
                    return;
                }
                var question = vm.Questions.FirstOrDefault(x => x.Question == stepTwoPicker.SelectedItem);
                vm.QuestionAndAnswer.Add(new QuestionAndAnswer() { Answer = stepTwoAnswer.Text.Trim(), QuestionID = question.ID });

                await vm.SubmitSecurityQuestions(_username, _password);
            }
            catch (Exception ex)
            {

                
            }

        }

    }
}