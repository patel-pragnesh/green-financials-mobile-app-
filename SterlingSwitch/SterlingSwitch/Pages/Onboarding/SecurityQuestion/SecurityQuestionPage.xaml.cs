using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SterlingSwitch.Pages.Onboarding.OtpAndPinVerification;
using SterlingSwitch.Pages.Onboarding.SecurityQuestion.Model;
using SterlingSwitch.Pages.Onboarding.SecurityQuestion.ViewModel;
using SterlingSwitch.Pages.Onboarding.Services;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using Xamanimation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Onboarding.SecurityQuestion
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SecurityQuestionPage : SwitchMasterPage
	{
	    private SecurityQuestionViewModel _vm;
        private OnboardingViewModel _ovm;
        private List<QuestionAnswerModel> Answer { get; set; }
	    private bool isBinded = false;
		public SecurityQuestionPage (OnboardingViewModel ovm)
		{
			InitializeComponent ();
		    BindingContext = _vm = new SecurityQuestionViewModel(Navigation);
		    Answer = new List<QuestionAnswerModel>();
            _ovm = ovm;
        }

	    protected override void OnAppearing()
	    {
	        base.OnAppearing();
	        if (isBinded != false) return;
	        stepOnePicker.IsEnabled = false;
	        stepOnePicker.SelectedItem = "Fetching security questions";
	        stepOnePicker.ItemsSource = _vm.BindableQuestions;
	        stepOnePicker.SelectedItem = "Select a security Question 1 of 2.";
	        stepOnePicker.IsEnabled = true;
	    }

	    private void Cross_OnTapped(object sender, EventArgs e)
	    {
	        Navigation.PopAsync(true);
	    }

        private  void BtnStep1_OnClicked(object sender, EventArgs e)
        {
            try {
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

                var questionId = _vm.Questions.FirstOrDefault(f => f.Question.ToLower().Equals(stepOnePicker.SelectedItem.ToLower())).ID;
                Answer.Add(new QuestionAnswerModel() { Answer = stepOneAnswer.Text, QuestionID = questionId });
                grdStep1.IsVisible = false;
                grdStep2.Animate(new FadeInAnimation());
                grdStep2.IsVisible = true;
                stepTwoPicker.ItemsSource = _vm.BindableQuestions.Where(x => !x.ToLower().Equals(stepOnePicker?.SelectedItem.ToLower())).ToList(); //_vm.BindableQuestions;

                //grdStep1.Animate(new FadeOutAnimation());
            }
            catch (Exception ex)
            {
                BusinessLogic.Log(ex.ToString(), "Exception on security question picker event", string.Empty, string.Empty,"Security Question Page", string.Empty);
            }
            

        }

        private void Next_OnClicked(object sender, EventArgs e)
        {
            //grdStep2.Animate(new FadeOutAnimation());
            grdStep2.IsVisible = false;
            grdStep1.Animate(new FadeInAnimation());
            grdStep1.IsVisible = true;
        }

	    private void BtnStep2_OnClicked(object sender, EventArgs e)
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
                var questionId = _vm.Questions.FirstOrDefault(f => f.Question.ToLower().Equals(stepOnePicker.SelectedItem.ToLower())).ID;
                Answer.Add(new QuestionAnswerModel() { Answer = stepTwoAnswer.Text, QuestionID = questionId });
                Navigation.PushAsync(new AccountPinCreation(_ovm, Answer));
            }
            catch (Exception ex)
            {
                BusinessLogic.Log(ex.ToString(), "Exception on security question picker event", string.Empty, string.Empty, "Security Question Page", string.Empty);
            }

        }
	}
}