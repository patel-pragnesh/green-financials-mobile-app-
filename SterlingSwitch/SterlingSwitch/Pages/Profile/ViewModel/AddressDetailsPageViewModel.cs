using SterlingSwitch.Services;
using SterlingSwitch.ViewModelBase;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.Profile.ViewModel
{
  public  class AddressDetailsPageViewModel:BaseViewModel
    {
        public AddressDetailsPageViewModel(INavigation navigation):base(navigation)
        {
           // Nationality = GlobalStaticFields.Customer.
        }

        private string nationality;
        public string Nationality
        {
            get { return nationality; }
            set { SetProperty(ref nationality, value); }
        }

        private string countryofresidence;
        public string CountryOfResidence
        {
            get { return countryofresidence; }
            set { SetProperty(ref countryofresidence, value); }
        }

        private string addressline1;
        public string AddressLine1
        {
            get { return addressline1; }
            set { SetProperty(ref addressline1, value); }
        }
        private string addressline2;
        public string AddressLine2
        {
            get { return addressline2; }
            set { SetProperty(ref addressline2, value); }
        }
        private string townorcity;
        public string TownOrCity
        {
            get { return townorcity; }
            set { SetProperty(ref townorcity, value); }
        }

        private string state;
        public string State
        {
            get { return state; }
            set { SetProperty(ref state, value); }
        }
        private string postcode;
        public string PostCode
        {
            get { return postcode; }
            set { SetProperty(ref postcode, value); }
        }
    }
}
