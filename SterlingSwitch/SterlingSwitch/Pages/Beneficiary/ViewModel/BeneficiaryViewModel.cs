using System;
using System.Collections.Generic;
using System.Text;
using SterlingSwitch.ViewModelBase;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.Beneficiary.ViewModel
{
    public class BeneficiaryViewModel : BaseViewModel
    {
        private ObservableRangeCollection<string> _beneficiaries;

        public ObservableRangeCollection<string> Beneficiaries
        {
            get => _beneficiaries;
            set => SetProperty(ref _beneficiaries, value);
        }
        public BeneficiaryViewModel(INavigation navigation) : base(navigation)
        {
            Beneficiaries = new ObservableRangeCollection<string>();
            Beneficiaries.AddRange(new List<string>() { string.Empty, string.Empty, string.Empty, string.Empty});
        }
    }
}
