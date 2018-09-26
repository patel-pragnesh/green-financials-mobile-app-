using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SterlingSwitch.Pages.Onboarding.SecurityQuestion.Model;
using SterlingSwitch.Pages.Onboarding.SecurityQuestion.Service;
using SterlingSwitch.ViewModelBase;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.Onboarding.SecurityQuestion.ViewModel
{
    public class SecurityQuestionViewModel:BaseViewModel
    {
        private readonly SecurityQuestionService _service;
        private ObservableRangeCollection<QuestionData> _questions;
        private ObservableRangeCollection<string> _bindableQuestions;
        private bool _isPickerEnabled = false;
        public ObservableRangeCollection<QuestionData>  Questions
        {
            get => _questions;
            set => SetProperty(ref _questions, value);
        }

        public ObservableRangeCollection<string> BindableQuestions
        {
            get => _bindableQuestions;
            set => SetProperty(ref _bindableQuestions, value);
        }

        public bool IsPickerEnabled
        {
            get => _isPickerEnabled;
            set => SetProperty(ref _isPickerEnabled, value);
        }
        public SecurityQuestionViewModel(INavigation navigation) : base(navigation)
        {
            _service = new SecurityQuestionService();
            Questions = new ObservableRangeCollection<QuestionData>();
            BindableQuestions = new ObservableRangeCollection<string>();
            Task.Run(async () => SetQuestions());
        }

        private async Task SetQuestions()
        {
            try
            {
                var questions = await _service.GetQuestions();
                if (questions.Any())
                {
                    var question = new List<string>();
                    questions.ForEach(r =>
                    {
                        question.Add(r.Question);
                    });
                    BindableQuestions.AddRange(question);
                    Questions.AddRange(questions);
                }      
            }
            catch (Exception e)
            {
               
            }
            
        }
    }
}
