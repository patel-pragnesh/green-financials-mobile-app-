using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Pages.Onboarding.SecurityQuestion.Model
{
    public class SecurityQuestionModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public QuestionData[] Data { get; set; }
    }

    public class QuestionData
    {
        public int ID { get; set; }
        public string Question { get; set; }
    }



}
