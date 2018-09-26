using FormsControls.Base;
using SterlingSwitch.Pages.Specta.Service;
using SterlingSwitch.PopUps;
using SterlingSwitch.Templates;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Specta
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FinishQuickLoan : SwitchMasterPage
    {
        public string otp { get; set; }
        public bool IsAcceptLicence { get; set; }
         

        public FinishQuickLoan (string eligibleAmt)
		{
            this.BindingContext = this;
			InitializeComponent (); 
            lblEligibleAmount.Text = eligibleAmt;
		}

        private async void btnConfirm_Clicked(object sender, EventArgs e)
        {
            var pd = await ProgressDialog.Show("Sending Request. Please Wait...");
            try
            {
                if(IsEntryValidated() == true)
                {
                    SpectaProcess.otp = otp;
                    SpectaProcess.isTermsAccepted = IsAcceptLicence.ToString();

                    var processModel = new SpectaProcess2()
                    {
                        firstName = SpectaProcess.firstName,
                        lastName = SpectaProcess.lastName,
                        dateOfBirth = SpectaProcess.dateOfBirth,
                        isTermsAccepted = SpectaProcess.isTermsAccepted,
                        acctNo = SpectaProcess.acctNo,
                        accomodationType = SpectaProcess.accomodationType,
                        address = SpectaProcess.address,
                        bvn = SpectaProcess.bvn,
                        channelId = SpectaProcess.channelId,
                        eligibleAmt = SpectaProcess.eligibleAmt,
                        email = SpectaProcess.email,
                        empAddress = SpectaProcess.empAddress,
                        empName = SpectaProcess.empName,
                        gender = SpectaProcess.gender,
                        howLongInResidence = SpectaProcess.howLongInResidence,
                        isCustomer = SpectaProcess.isCustomer == "Yes" ? SpectaProcess.isCustomer = "1" : SpectaProcess.isCustomer = "0",
                        jobChangeCount = SpectaProcess.jobChangeCount,
                        loanPurpose = SpectaProcess.loanPurpose,
                        maritalStatus = SpectaProcess.maritalStatus,
                        numberOfDependants = SpectaProcess.numberOfDependants,
                        otp = SpectaProcess.otp,
                        phoneNumber = SpectaProcess.phoneNumber,
                        reqAmt = SpectaProcess.reqAmt,
                        salaryAmt = SpectaProcess.salaryAmt,
                        Salary_Act_No = SpectaProcess.Salary_Act_No == null ? SpectaProcess.Salary_Act_No = SpectaProcess.acctNo : SpectaProcess.Salary_Act_No,
                        tenor = SpectaProcess.tenor
                    };

                    var response = await SpectaService.SpectaFinal(processModel);
                    if (!string.IsNullOrEmpty(response))
                    {
                        if(response == "1")
                        {
                            // it is successful. Give a
                            MessageDialog.Show("SUCCESS", "Your Loan application was successful.", DialogType.Success, "OK", null);
                            Application.Current.MainPage = new AnimationNavigationPage(new Dashboard.Dashboard());

                        }
                        else
                        {
                            MessageDialog.Show("OOPS", "Sorry, we are unable to place your request at the moment.\n Kindly try again later or contact our customer care", DialogType.Success, "OK", null);
                            return;
                            // Unsuccessful
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                await pd.Dismiss();
                // catch any exception
            }
        }

        private bool IsEntryValidated()
        {
            if (string.IsNullOrEmpty(otp))
            {
                MessageDialog.Show("OOPS", "Please Enter the otp sent to your phone number and email", DialogType.Error, "DISMISS", null);
                txtOTP.Focus();
                return false;
            }
            if (switchLicenceAccepted.IsToggled == false)
            {
                MessageDialog.Show("OOPS", "Please accept our Terms and Conditions as stated", DialogType.Error, "DISMISS", null);
                txtOTP.Focus();
                return false;
            }
            else
                return true;
        }

            private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
    }
}