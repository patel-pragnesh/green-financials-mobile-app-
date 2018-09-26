using SterlingSwitch.Services;
using SterlingSwitch.ViewModelBase;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.Profile.ViewModel
{
  public  class PersonalDetailsPageViewModel:BaseViewModel
    {
        public PersonalDetailsPageViewModel(INavigation navigation):base(navigation)
        {
            Firstname = GlobalStaticFields.Customer.FirstName;
            Lastname = GlobalStaticFields.Customer.LastName;
            DateOfBirth = GlobalStaticFields.Customer.BirthDate.ToString("dd-MM-yyyy");
            Gender =  GlobalStaticFields.Customer.Gender == "M"?"Male":"Female";
            Email = GlobalStaticFields.Customer.Email;
            Phonenumber = GlobalStaticFields.Customer.PhoneNumber;
            BVN = GlobalStaticFields.Customer.BVN;
        }
        private string firstname;
        public string Firstname
        {
            get { return firstname; }
            set { SetProperty(ref firstname,value); }
        }

        private string lastname;
        public string Lastname
        {
            get { return lastname; }
            set { SetProperty(ref lastname, value); }
        }
        private string dateofbirth;
        public string DateOfBirth
        {
            get { return dateofbirth; }
            set { SetProperty(ref dateofbirth, value); }
        }

        private string gender;
        public string Gender
        {
            get { return gender; }
            set { SetProperty(ref gender, value); }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }
        private string bvn;
        public string BVN
        {
            get { return bvn; }
            set { SetProperty(ref bvn, value); }
        }

        private string phonenumber;
        public string Phonenumber
        {
            get { return phonenumber; }
            set { SetProperty(ref phonenumber, value); }
        }
    }
}
