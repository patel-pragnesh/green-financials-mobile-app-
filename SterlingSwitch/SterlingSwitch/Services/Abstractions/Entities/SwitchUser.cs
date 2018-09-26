using SterlingSwitch.Pages.Onboarding.SecurityQuestion.Model;
using SterlingSwitch.Pages.Onboarding.SecurityQuestion.ViewModel;
using System;
using System.Collections.Generic;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class SwitchUser
    {
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string AccessLocation { get; set; }
        public string CustomerTimeZone { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Title { get; set; }
        public string AccountType { get; set; }
        public string HomeAddress { get; set; }
        public bool IsTPIN { get; set; }
        public string ReferralCode { get; set; }
        public string RefferedBy { get; set; }
        public string SignupVerificationCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int Nationality { get; set; }
        public string UniqueKey { get; set; }
        public string Device { get; set; }
        public string IMEI { get; set; }
        public string OS { get; set; }
        public string TPIN { get; set; }
        public List<QuestionAnswerModel> SecurityQuestionAndAnswers { get; set; }
    }

    public class QuestionAndAnswer
    {
        public int QuestionID { get; set; }
        public string Answer { get; set; }
        public string Question { get; set; }
    }
}
