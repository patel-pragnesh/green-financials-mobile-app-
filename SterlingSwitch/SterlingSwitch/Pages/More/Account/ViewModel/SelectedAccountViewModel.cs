using SterlingSwitch.Models;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.ViewModelBase;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.More.Account.ViewModel
{
    public class SelectedAccountViewModel : BaseViewModel
    {
        public SelectedAccountViewModel(INavigation navigation) : base(navigation)
        {
            CustomerAccounts = new CustomerAccount();
        }

        #region Properties
        public CustomerAccount CustomerAccounts { get; set; }
        public MyCards CustomerCards { get; set; }

        private bool _isAccountLoaded;
        public bool IsAccountLoaded
        {
            get { return _isAccountLoaded; }
            set { SetProperty(ref _isAccountLoaded, value); }
        }
        #endregion Properties
    }
}
