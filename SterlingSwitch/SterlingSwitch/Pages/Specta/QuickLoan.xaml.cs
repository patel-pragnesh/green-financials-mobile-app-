using SterlingSwitch.Helper;
using SterlingSwitch.Models;
using SterlingSwitch.Pages.Specta.Service;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SterlingSwitch.Models.SomeConstant;

namespace SterlingSwitch.Pages.Specta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuickLoan : SwitchMasterPage
    {
        public List<string> loanPurposeList { get; set; }
        public List<CustomerAccount> AccountDetails { get; set; }
        public List<string> accountList { get; set; }
        public List<Value> loan { get; set; }
        public List<Value> AccomodationTypeList { get; set; }
        public List<string> accomodationTypeList { get; set; }
        public List<string> jobChangeCountList { get; set; }
        public List<Value> JobChangeCountList { get; set; }
        public List<string> maritalStatusList { get; set; }
        public List<Value> MaritalStatusList { get; set; }
        public List<string> numberOfDependantsList { get; set; }
        public List<Value> NumberOfDependantsList { get; set; }
        public List<string> tenorList { get; set; }
        public List<Value> TenorList { get; set; }
        public List<string> howLongInResidenceList { get; set; }
        public List<Value> HowLongInResidenceList { get; set; }
        public string accomodationType { get; set; }
        public string jobChangeCount { get; set; }
        public string maritalStatus { get; set; }
        public string numberOfDependants { get; set; }
        public string tenor { get; set; }
        public string howLongInResidence { get; set; }
        public string isCustomer { get; set; }
        public string acctNo { get; set; }
        public double amount { get; set; }
        public string loanPurpose { get; set; }
        public string empAddress { get; set; }
        public string address { get; set; }
        public string empName { get; set; }
        public string empEmail { get; set; }
        public string salaryAmt { get; set; }
        public QuickLoan()
        {
            InitializeComponent();
            this.BindingContext = this;

            //get name of this present page

            var x = (this.GetType().Name);
            BusinessLogic.LogFrequentPage(x, PageAliasConstant.quickloan, ImageConstants.QuickLoanIcon);

            GetLoanPurpose();
            GetaccomodationTypeList();
            GetjobChangeCountList();
            GetmaritalStatusList();
            Getdependant();
            Gettenor();
            GethowLongInResidenceList();
            GetCurrentAccount();
        }


        private async Task GetLoanPurpose()
        {
            try
            {
                var loanp = new List<string>();
                var content = GlobalStaticFields.SpectaDropdownList.dropdownList;
                var response = content.Where(s => s.dropdownListName
                                    .Equals("loanPurpose", StringComparison.OrdinalIgnoreCase))
                                    .Select(s => s.value).ToList();
                if (response != null && response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        foreach (var i in item)
                        {
                            loanp.Add(i.descrip);
                        }
                        loanPurposeList = new List<string>(loanp);
                        dpdLoanPurpose.ItemsSource = loanPurposeList;
                    }
                }
            }
            catch (Exception)
            {
                // log your exceptions
            }
        }

        private async Task GetaccomodationTypeList()
        {
            try
            {
                var accomodation = new List<string>();
                var content = GlobalStaticFields.SpectaDropdownList.dropdownList;
                var response = content.Where(s => s.dropdownListName
                               .Equals("accommodationType", StringComparison.OrdinalIgnoreCase))
                               .Select(s => s.value).ToList();
                if (response != null && response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        foreach (var i in item)
                        {
                            accomodation.Add(i.descrip);
                        }
                        accomodationTypeList = new List<string>(accomodation);
                        AccomodationTypeList = new List<Value>(item);
                        dpdAccomodation.ItemsSource = accomodationTypeList;
                    }
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
            }
        }

        private async Task GetjobChangeCountList()
        {
            try
            {
                var content = GlobalStaticFields.SpectaDropdownList.dropdownList;
                var job = new List<string>();
                var response = content.Where(s => s.dropdownListName
                               .Equals("numberOfJobChanged", StringComparison.OrdinalIgnoreCase))
                               .Select(s => s.value).ToList();
                if (response != null && response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        foreach (var i in item)
                        {
                            job.Add(i.descrip);
                        }
                        jobChangeCountList = new List<string>(job);
                        JobChangeCountList = new List<Value>(item);
                        dpdJobChangeCount.ItemsSource = jobChangeCountList;
                    }
                }

            }
            catch (Exception ex) { }
        }
        private async Task GetmaritalStatusList()
        {
            try
            {
                var content = GlobalStaticFields.SpectaDropdownList.dropdownList;
                var job = new List<string>();
                var response = content.Where(s => s.dropdownListName
                               .Equals("maritalStatus", StringComparison.OrdinalIgnoreCase))
                               .Select(s => s.value).ToList();
                if (response != null && response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        foreach (var i in item)
                        {
                            job.Add(i.descrip);
                        }
                        maritalStatusList = new List<string>(job);
                        MaritalStatusList = new List<Value>(item);
                        dpdMaritalStatus.ItemsSource = maritalStatusList;
                    }
                }

            }
            catch (Exception ex) { }
        }

        private async Task Getdependant()
        {
            var content = GlobalStaticFields.SpectaDropdownList.dropdownList;
            var job = new List<string>();
            var response = content.Where(s => s.dropdownListName
                           .Equals("dependant", StringComparison.OrdinalIgnoreCase))
                           .Select(s => s.value).ToList();
            if (response != null && response.Count > 0)
            {
                foreach (var item in response)
                {
                    foreach (var i in item)
                    {
                        job.Add(i.descrip);
                    }
                    numberOfDependantsList = new List<string>(job);
                    NumberOfDependantsList = new List<Value>(item);
                    dpdDependants.ItemsSource = numberOfDependantsList;
                }
            }
        }

        private async Task Gettenor()
        {
            var content = GlobalStaticFields.SpectaDropdownList.dropdownList;
            var job = new List<string>();
            var response = content.Where(s => s.dropdownListName
                           .Equals("tenor", StringComparison.OrdinalIgnoreCase))
                           .Select(s => s.value).ToList();
            if (response != null && response.Count > 0)
            {
                foreach (var item in response)
                {
                    foreach (var i in item)
                    {
                        job.Add(i.descrip);
                    }
                    tenorList = new List<string>(job);
                    TenorList = new List<Value>(item);
                    dpdPaybackPeriod.ItemsSource = tenorList;
                }
            }
        }

        private async Task GethowLongInResidenceList()
        {
            var content = GlobalStaticFields.SpectaDropdownList.dropdownList;
            var job = new List<string>();
            var response = content.Where(s => s.dropdownListName
                           .Equals("numberOfYearsInCurrentResidence", StringComparison.OrdinalIgnoreCase))
                           .Select(s => s.value).ToList();
            if (response != null && response.Count > 0)
            {
                foreach (var item in response)
                {
                    foreach (var i in item)
                    {
                        job.Add(i.descrip);
                    }
                    howLongInResidenceList = new List<string>(job);
                    HowLongInResidenceList = new List<Value>(item);
                    dpdResidencyPeriod.ItemsSource = howLongInResidenceList;
                }
            }
        }

        public async Task GetCurrentAccount()
        {
            try
            {
                var allAcc = GlobalStaticFields.Customer.ListOfAllAccounts;
                AccountDetails = new List<CustomerAccount>(allAcc);
                accountList = new List<string>();
                foreach (var item in AccountDetails.Where(x => x.accountType.Contains("Salary")))
                {
                    accountList.Add(item.AccountNumberWithBalance);
                }
                dpdDebitAccount.ItemsSource = accountList;
            }
            catch (Exception ex)
            {

            }
        }


        // filter and return the accomodation Id
        public string getAccomodationId(string name)
        {
            var accomodationId = AccomodationTypeList.Where(d => d.descrip == name).FirstOrDefault().refid;
            return accomodationId;
        }

        // filter and retunrn the number of job change count Id
        public string getjobChangeCount(string name)
        {
            var jobchangecount = JobChangeCountList.Where(d => d.descrip == name).FirstOrDefault().refid;
            return jobchangecount;
        }

        // filter and return M/Status Id
        public string getMaritalStatus(string name)
        {
            var mstatus = MaritalStatusList.Where(s => s.descrip == name).FirstOrDefault().refid;
            return mstatus;
        }

        // filter and return Dependants Id
        public string getDependent(string name)
        {
            var dp = NumberOfDependantsList.Where(d => d.descrip == name).FirstOrDefault().refid;
            return dp;
        }

        // filter and returns tenor Id
        public string getTenor(string name)
        {
            var tt = TenorList.Where(d => d.descrip == name).FirstOrDefault().refid;
            return tt;
        }

        // filters and returns residence Id
        public string getResidence(string name)
        {
            var res = HowLongInResidenceList.Where(d => d.descrip == name).FirstOrDefault().refid;
            return res;
        }


        private void BtnContinue_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (IsEntryValidated() == true)
                {
                    // if all entries are validated correctly, go ahead and do processing

                    MessageDialog.Show("Transaction Confirmation", $"You are about to book a loan that is worth \n {Utilities.GetCurrency("NGN")}{txtAmount.Text:##,###}", DialogType.Question, " Do you want to Proceed",
                  DoLoanBooking, "Cancel", null);
                    //
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                // throw;
            }
        }

        public void DoLoanBooking()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var pd = await ProgressDialog.Show("Sending Request. Please Wait...");
                try
                {

                    var accSplitted = Utilities.SplitBeneficiaryDetailsByTab(acctNo);
                    string AcctToDebit = accSplitted[0].Trim(); 
                    SpectaProcess.firstName = GlobalStaticFields.Customer?.FirstName;
                    SpectaProcess.lastName = GlobalStaticFields.Customer?.LastName;
                    SpectaProcess.gender = GlobalStaticFields.Customer?.Gender;
                    SpectaProcess.empName = empName;
                    SpectaProcess.bvn = GlobalStaticFields.Customer?.BVN;
                    SpectaProcess.dateOfBirth = GlobalStaticFields.Customer?.BirthDate.ToString("yyyy-MM-dd"); //model.dob;
                    SpectaProcess.email = empEmail;
                    if (!string.IsNullOrEmpty(GlobalStaticFields.Customer.BVN))
                        isCustomer = "Yes";
                    else
                        isCustomer = "No";
                    SpectaProcess.isCustomer = isCustomer;
                    SpectaProcess.phoneNumber = GlobalStaticFields.Customer?.PhoneNumber;
                    SpectaProcess.acctNo = AcctToDebit;
                    SpectaProcess.reqAmt = amount.ToString();
                    SpectaProcess.loanPurpose = loanPurpose;
                    SpectaProcess.empAddress = empAddress;
                    SpectaProcess.address = address;
                    SpectaProcess.salaryAmt = salaryAmt;
                    SpectaProcess.accomodationType = getAccomodationId(accomodationType);
                    SpectaProcess.jobChangeCount = getjobChangeCount(jobChangeCount);
                    SpectaProcess.maritalStatus = getMaritalStatus(maritalStatus);
                    SpectaProcess.numberOfDependants = getDependent(numberOfDependants);
                    SpectaProcess.tenor = getTenor(tenor);
                    SpectaProcess.howLongInResidence = getResidence(howLongInResidence);

                    // there wont be need to validate NUBAN, since its coming from a list of all customer account....

                    // since Nuban Passed, lets get Customer's Eligible Amount
                    var eResponse = await SpectaService.GetEligibleAmount(SpectaProcess.accomodationType, SpectaProcess.maritalStatus,
                                                        SpectaProcess.numberOfDependants, SpectaProcess.jobChangeCount,
                                                        SpectaProcess.howLongInResidence, SpectaProcess.empName, SpectaProcess.empAddress,
                                                        SpectaProcess.gender, SpectaProcess.tenor, SpectaProcess.reqAmt, SpectaProcess.firstName,
                                                        SpectaProcess.lastName, SpectaProcess.address, SpectaProcess.dateOfBirth,
                                                        SpectaProcess.salaryAmt, SpectaProcess.phoneNumber, SpectaProcess.bvn,
                                                        SpectaProcess.email, SpectaProcess.isCustomer, SpectaProcess.acctNo);

                    if (!string.IsNullOrEmpty(eResponse) && eResponse == "Success")
                    {

                        var otpResponse = await SpectaService.GetOtpSpectaOTP(SpectaProcess.acctNo, SpectaProcess.email);   // call otp method.                       
                        if (otpResponse == true)
                        {
                            await pd.Dismiss();
                            MessageDialog.Show("SUCCESS", $"An OTP has been sent to your registered phone number {GlobalStaticFields.Customer.PhoneNumber.Substring(0,5)}##### and email {SpectaProcess.email}", DialogType.Error, "DISMISS", null);
                            await Navigation.PushAsync(new FinishQuickLoan(SpectaProcess.eligibleAmount));
                        }
                        else
                        {
                            await pd.Dismiss();
                            MessageDialog.Show("OOPS", "Sorry, we are unable to process your request at the moment. Kindly try again later", DialogType.Error, "DISMISS", null);
                            return;
                        }
                    }
                    else
                    {
                        await pd.Dismiss();
                        MessageDialog.Show("OOPS", "Sorry, you are not eligible for this request. Kindly contact our customer care", DialogType.Error, "DISMISS", null);
                        return;
                    }
                    await pd.Dismiss();
                }
                catch (Exception ex)
                {
                    await pd.Dismiss();
                }
            });
        }

        private bool IsEntryValidated()
        {
            if (string.IsNullOrEmpty(acctNo))
            {
                MessageDialog.Show("OOPS", "Please select a salary account", DialogType.Error, "DISMISS", null);
                dpdDebitAccount.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(loanPurpose))
            {
                MessageDialog.Show("OOPS", "Please select purpose of loan", DialogType.Error, "DISMISS", null);
                dpdLoanPurpose.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtEmployer.Text))
            {
                MessageDialog.Show("OOPS", "Please enter the name of your employer", DialogType.Error, "DISMISS", null);
                txtEmployer.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtEmployerAddress.Text))
            {
                MessageDialog.Show("OOPS", "Please enter the address of your place of work", DialogType.Error, "DISMISS", null);
                txtEmployerAddress.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtOfficialEmailAddress.Text))
            {
                MessageDialog.Show("OOPS", "Please enter your official email address", DialogType.Error, "DISMISS", null);
                txtOfficialEmailAddress.Focus();
                return false;
            }
            if (!string.IsNullOrEmpty(txtOfficialEmailAddress.Text))
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(txtOfficialEmailAddress.Text);
                if (!match.Success)
                {
                    MessageDialog.Show("OOPS", "Sorry, The email address you entered is not incorrect. Please verify and try again.", DialogType.Error, "DISMISS", null);
                    return false;
                }
            }
            if (string.IsNullOrEmpty(jobChangeCount))
            {
                MessageDialog.Show("OOPS", "Please select total number of job changes", DialogType.Error, "DISMISS", null);
                dpdJobChangeCount.Focus();
                dpdJobChangeCount.BackgroundColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(salaryAmt))
            {
                MessageDialog.Show("OOPS", "How much do you earn per month ?", DialogType.Error, "DISMISS", null);
                txtMonthlySalary.Focus();
                txtMonthlySalary.BackgroundColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(txtAmount.Text))
            {
                MessageDialog.Show("OOPS", "How much do you want to borrow ?", DialogType.Error, "DISMISS", null);
                txtAmount.Focus();
                txtAmount.BackgroundColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(accomodationType))
            {
                MessageDialog.Show("OOPS", "What type of accomodation do you live in?", DialogType.Error, "DISMISS", null);
                dpdAccomodation.Focus();
                dpdAccomodation.BackgroundColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(maritalStatus))
            {
                MessageDialog.Show("OOPS", "What is your marital status ?", DialogType.Error, "DISMISS", null);
                dpdMaritalStatus.Focus();
                dpdMaritalStatus.BackgroundColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(numberOfDependants))
            {
                MessageDialog.Show("OOPS", "How many people depend on you ?", DialogType.Error, "DISMISS", null);
                dpdDependants.Focus();
                dpdDependants.BackgroundColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(tenor))
            {
                MessageDialog.Show("OOPS", "In how many months do you want to pay back ?", DialogType.Error, "DISMISS", null);
                dpdPaybackPeriod.Focus();
                dpdPaybackPeriod.BackgroundColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(address))
            {
                MessageDialog.Show("OOPS", "Kindly provide us your home address ?", DialogType.Error, "DISMISS", null);
                txtResidentialAddress.Focus();
                txtResidentialAddress.BackgroundColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(howLongInResidence))
            {
                MessageDialog.Show("OOPS", "How long have you stayed at your current residence ?", DialogType.Error, "DISMISS", null);
                txtResidentialAddress.Focus();
                txtResidentialAddress.BackgroundColor = Color.Red;
                return false;
            }            
            else
            {
                return true;
            }
        }

        private void dpdDebitAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            acctNo = dpdDebitAccount.SelectedItem;
        }

        private void dpdLoanPurpose_SelectedIndexChanged(object sender, EventArgs e)
        {
            loanPurpose = dpdLoanPurpose.SelectedItem;
        }

        private void dpdJobChangeCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            jobChangeCount = dpdJobChangeCount.SelectedItem;
        }

        private void dpdAccomodation_SelectedIndexChanged(object sender, EventArgs e)
        {
            accomodationType = dpdAccomodation.SelectedItem;
        }

        private void dpdMaritalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            maritalStatus = dpdMaritalStatus.SelectedItem;
        }

        private void dpdDependants_SelectedIndexChanged(object sender, EventArgs e)
        {
            numberOfDependants = dpdDependants.SelectedItem;
        }

        private void dpdPaybackPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            tenor = dpdPaybackPeriod.SelectedItem;
        }

        private void dpdResidencyPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            howLongInResidence = dpdResidencyPeriod.SelectedItem;
        }
    }
}