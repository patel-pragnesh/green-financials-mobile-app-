using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.ViewModelBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.Onboarding.Services
{
    public class OnboardingViewModel: BaseViewModel
    {
        public string _firstname = string.Empty;
        public string _lastname = string.Empty;
        public string _gender = string.Empty;
        public string _email = string.Empty;
        public string _phone = string.Empty;
        public DateTime _dateOfBirth = default(DateTime);
        public string _countryCode = string.Empty;
        public string _referralCOde = string.Empty;
        public string _pin = string.Empty;
        public string _confirmPin = string.Empty;
        public string _defaultAccontNumber = string.Empty;
        public string _confirmAccountNumber = string.Empty;
        public string _selectedAccountNumber = string.Empty;
        public List<AllAccountOfCustomer> allAccountOfCustomers;
        public string _stashedAccountNumber = string.Empty;
        public string _walletPhone = default(string);
        //private static OnBoardingService _service;
        public OnboardingViewModel(INavigation navigation): base(navigation)
        {
           // _service = new OnBoardingService();
        }

        public string WalletPhone
        {
            get => _walletPhone;
            set => SetProperty(ref _walletPhone, value);
        }

        public string Firstname
        {
            get => _firstname;
            set => SetProperty(ref _firstname, value);
        }
        public string Lastname
        {
            get => _lastname;
            set => SetProperty(ref _lastname, value);
        }

        public string Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string PhoneNumber
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set => SetProperty(ref _dateOfBirth, value);
        }

        public string ReferralCode
        {
            get => _referralCOde;
            set => SetProperty(ref _referralCOde, value);
        }

        public string PIN
        {
            get => _pin;
            set => SetProperty(ref _pin, value);
        }

        public string ConfirmPIN
        {
            get => _confirmPin;
            set => SetProperty(ref _confirmPin, value);
        }

        public string DefaultAccountNumber
        {
            get => _defaultAccontNumber;
            set => SetProperty(ref _defaultAccontNumber, value);
        }

        public string CountryCode
        {
            get => _countryCode;
            set => SetProperty(ref _countryCode, value);
        }
        public string ConfirmAccountNumber
        {
            get => _confirmAccountNumber;
            set => SetProperty(ref _confirmAccountNumber, value);
        }

        public string SelectedAccountNumber
        {
            get => _selectedAccountNumber;
            set => SetProperty(ref _selectedAccountNumber, value);
        }

        public string StashedAccountNumber
        {
            get => _stashedAccountNumber;
            set => SetProperty(ref _stashedAccountNumber, value);
        }
    }
}
