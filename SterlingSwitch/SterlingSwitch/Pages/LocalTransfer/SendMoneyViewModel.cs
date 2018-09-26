using SterlingSwitch.Helper;
using SterlingSwitch.Models;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.ViewModelBase;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static switch_mobile.Services.Abstractions.Entities.SaveSwitchBeneficiary;

namespace SterlingSwitch.Pages.LocalTransfer
{
    public class SendMoneyViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> ParticipatingBankNameList { get; set; }
        public List<string> accountListForPicker { get; set; }
        public bool IsNewOrExisting { get; set; }
        public bool IsScheduleVisible { set; get; } = false;
        public List<string> AccountListForPicker { get; set; }
       
        public bool IsNewRecipient { get; set; }
        public bool IsExistingRecipient { get; set; }
        public ObservableCollection<Banklist> ParticipatingBankList { get; set; }
        public GetSavedBeneficiaries OldSavedBeneficiarySelected { get; set; }
        internal void ShowOrHideManageButton(GetSavedBeneficiaries beneficiaries)
        {
            if (OldSavedBeneficiarySelected == beneficiaries)
            {
                // click twice on the same item will hide it
                beneficiaries.IsVisible = !beneficiaries.IsVisible;
                ToogleList(beneficiaries);
            }
            else
            {
                if (OldSavedBeneficiarySelected != null)
                {
                    // hide previous selected item
                    OldSavedBeneficiarySelected.IsVisible = false;
                    ToogleList(OldSavedBeneficiarySelected);
                }
                // show selected item
                beneficiaries.IsVisible = true;
                ToogleList(beneficiaries);
            }

            OldSavedBeneficiarySelected = beneficiaries;
        }

        private void ToogleList(GetSavedBeneficiaries beneficiaries)
        {
            var index = ListOfSavedBeneficiaries.IndexOf(beneficiaries);
            if (index<=0)
            {
                return;
            }
            ListOfSavedBeneficiaries.Remove(beneficiaries);
            ListOfSavedBeneficiaries.Insert(index, beneficiaries);
        }

        private bool _issave;
        participatingBanks participatingBanksList = new participatingBanks();
        List<Banklist> BankList = new List<Banklist>();
        List<string> BankNameList = new List<string>();
        public OtherBanks OtherBanks { get; set; }
        public CreateScheduleDebitPayment createScheduleDebitPayment { get; set; }
        public TransferSterlingToSterling SterlingToSterling { set; get; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberNameAttribute] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }


        public bool IsSaveAsBeneficiary { get; set; }
        public ICommand BeginSendMoney { get; set; }

        public TransferModel Funds { get; set; }
        public GetSavedBeneficiaries SavedBeneficiaries { get; set; }
        public ObservableCollection<GetSavedBeneficiaries> ListOfSavedBeneficiaries { get; set; }
        public ObservableCollection<AllAccountOfCustomer> AccountDetails { get; set; }
        public List<string> TransferType { get; set; }
        public List<string> ListSchduleType { get; set; }
        public ObservableCollection<PaymentScheduleType> paymentScheduleTypes { get; set; }
        public string AmountFormattedComma { get; set; }
        private string _AmountInputted;
        public string AmountInputted { get;  set;        }
        public string ReferenceInputted { get; set; }
        public string BeneficiaryAccount { get; set; }
        public string BeneficiaryName { get; set; }
        public string MyAccount { get; set; }
        private string _NameofBeneficiaryBank;
        public string NameofBeneficiaryBank
        {
            get { return _NameofBeneficiaryBank; }

            set
            {
                if (_NameofBeneficiaryBank != value)
                {
                    _NameofBeneficiaryBank = value;

                    OnPropertyChanged(nameof(NameofBeneficiaryBank));
                    try
                    {
                     
                        if (GlobalStaticFields.BankNameAndCode!=null && GlobalStaticFields.BankNameAndCode.Count>0)
                        {
                            //get the bank code looping at banknameandcode
                            var bankcode = GlobalStaticFields.BankNameAndCode.Where(w => w.Name == NameofBeneficiaryBank).FirstOrDefault().Code;
                            DestinationBankCode = bankcode;
                            if (string.IsNullOrEmpty(bankcode))
                            {
                                //now get this from the other BankCode
                                foreach (var item in GlobalStaticFields.ListOfParticipatingBanks.bankList)
                                {
                                    if (item.BANKNAME.ToUpper() == NameofBeneficiaryBank.ToUpper())
                                    {
                                        bankcode = item.BANKCODE;
                                        DestinationBankCode = bankcode;
                                        continue;
                                    }
                                }
                            }





                        }
                    }
                    catch (Exception ex)
                    {

                        string log = ex.ToString();
                    }

                }
            }
        }
        public string DestinationBankCode { get; set; }
        public bool HasUserSavedNewBeneficiary { get; set; } = false;

        public string TransferTypeSelected { get;  set; }

       
        public string Balance { get; set; }

        public SendMoneyViewModel() 
        {
            Funds = new TransferModel();
            SavedBeneficiaries = new GetSavedBeneficiaries();
            ParticipatingBankList = new ObservableCollection<Banklist>();
            ParticipatingBankNameList = new ObservableCollection<string>();
            //BeginSendMoney = new Command(BeginSendMoneyNow);
            BindAccount(GlobalStaticFields.Customer.PhoneNumber);
            SetTransferTypes();
            GetparticipatingBanks();
            setListSchdule();
        }

        private void setListSchdule()
        {
            if (paymentScheduleTypes!=null && paymentScheduleTypes.Count>0 ) 
            {
                return;
            }
            paymentScheduleTypes = new ObservableCollection<PaymentScheduleType>()
            {
                new PaymentScheduleType
                {
                    SchedulePaymenyID = 0,
                    SchedulePaymentText = "Never"
                },
                new PaymentScheduleType
                {
                    SchedulePaymenyID = 1,
                    SchedulePaymentText = "Daily"
                },
                new PaymentScheduleType
                {
                    SchedulePaymenyID = 2,
                    SchedulePaymentText = "Weekly"
                },
                new PaymentScheduleType
                {
                    SchedulePaymenyID = 3,
                    SchedulePaymentText  = "Monthly"
                },
                new PaymentScheduleType
                {
                    SchedulePaymenyID = 4,
                    SchedulePaymentText = "Yearly"
                }
                
            };

            if(paymentScheduleTypes.Count > 0)
            {
                ListSchduleType = new List<string>();

                foreach (var item in paymentScheduleTypes)
                {
                    ListSchduleType.Add(item.SchedulePaymentText);
                }
            }
        }

        
        private async void GetparticipatingBanks()
        {
            if (GlobalStaticFields.ParticipatingBankNameList!=null && GlobalStaticFields.ParticipatingBankNameList.Count>0)
            {
                return;
            }
            BusinessLogic bl = new BusinessLogic();
            var getbanksList = new EmptyIBSCall()
            {
                ReferenceID = Utilities.GenerateReferenceId(),
                RequestType = "327"
            };
            //var pd = await ProgressDialog.Show("Gettings Banks. Please wait.");

            //var response2 = await bl.ParticipatingBankListAsync2(getbanksList);
            var response2 = await bl.GetParticipatingBanksFromFile();
            if (response2 != null && response2.Count > 0)
            {
                participatingBanksList.bankList = bl.GetActualBankList(response2);
                participatingBanksList.bankNameList = bl.GetParticipatingBankNameListing(response2);
                GlobalStaticFields.ListOfParticipatingBanks = participatingBanksList;
                GlobalStaticFields.ListOfbanks = response2;
                GlobalStaticFields.ParticipatingBankNameList = participatingBanksList.bankNameList;


            }
            //await pd.Dismiss();


            #region MyRegion
            //if (response2 != null)
            //{


            //    var query = from b in response2
            //                group b by new { b.BANKNAME }
            //               into mySortedBank
            //                select mySortedBank.FirstOrDefault();

            //    foreach (var item in query)
            //    {
            //        BankList.Add(item);
            //        bankNameList.Add(item.BANKNAME.ToUpper());
            //    }

            //    var ParticipatingBankListing = new ObservableCollection<Banklist>(BankList.OrderBy(s => s.BANKNAME.ToUpper()));
            //    participatingBanksList.bankList = ParticipatingBankListing;
            //    // ParticipatingBankList = new ObservableCollection<Banklist>(BankList.OrderBy(s => s.BANKNAME));

            //    var ParticipatingBankNameListing = new ObservableCollection<string>(bankNameList.OrderBy(s => s).Distinct());
            //    //ParticipatingBankNameList = new ObservableCollection<string>(bankNameList.OrderBy(s => s));
            //    participatingBanksList.bankNameList = ParticipatingBankNameListing;
            //} 
            #endregion


        }

        private void SetTransferTypes()
        {
            if (TransferType!=null && TransferType.Count>0)
            {
                return;
            }
            TransferType = new List<string>()
            {
               SendMoneyConstantss.ToSterling,SendMoneyConstantss.ToOtherBank
            };
        }

        //public SendMoneyViewModel()
        //{
        //    Funds = new TransferModel();
        //    SavedBeneficiaries = new GetSavedBeneficiaries();
        //    ParticipatingBankList = new ObservableCollection<Banklist>();
        //    ParticipatingBankNameList = new ObservableCollection<string>();
        //    BeginSendMoney = new Command(BeginSendMoneyNow);
        //}
        private void BeginSendMoneyNow()
        {
            if (IsValid())
            {
                MessageDialog.Show("Confirmation", $"You have initiated a transfer of NGN {Funds.Amount } to {SavedBeneficiaries.BenName} of {SavedBeneficiaries.Bank}" + Environment.NewLine + "Do you want to continue?", DialogType.Success, "Yes", ConsumateTransfer, "No", null);

            }
            else
            {

            }
        }
        private void ConsumateTransfer()
        {

        }
        private bool IsValid()
        {
            if (string.IsNullOrEmpty(Funds.Amount) || string.IsNullOrEmpty(Funds.PaymentReference))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async void SaveBeneficiary()
        {
            if (IsSaveAsBeneficiary)
            {
                var beneficiary = new SaveSwitchBeneficiary()
                {

                    beneficiaryMobile = "",
                    beneficiaryNuban = BeneficiaryAccount,
                    dateAdded = DateTime.Now,
                    dateModified = DateTime.Now,
                    Referenceid = Utilities.GenerateReferenceId(),
                    StatusFlag = true,
                    switchuserid = GlobalStaticFields.Customer.Email,
                    Translocation = GlobalStaticFields.GetUserLocation,
                    RequestType = 0,
                    BenName =  BeneficiaryName,
                    Bank = NameofBeneficiaryBank,
                    BenNickName = "",
                    BankCode = DestinationBankCode,
                    TransactionType = TransferTypeSelected
                };
                BusinessLogic bl = new BusinessLogic();
               var ben = await bl.SaveFundsTrfBeneficiary(beneficiary);  // call method to save beneficiary
                HasUserSavedNewBeneficiary = true;

            }
        }

        public async Task BindAccount(string phone)
        {

            try
            {

                if (GlobalStaticFields.Customer.ListOfAllAccounts != null && GlobalStaticFields.Customer.ListOfAllAccounts.Count > 0)
                {
                    AccountListForPicker = new List<string>();
                    foreach (var acct in GlobalStaticFields.Customer.ListOfAllAccounts)
                    {
                        AccountListForPicker.Add(acct.AccountBalanceAccountType);

                    }
                }

                else
                {
                    BusinessLogic.RebindAccountDetails();
                }


            }
            catch (Exception ex)
            {


                string log = ex.Message;
            }


        }

        internal async Task<List<string>> GetBanksFromNuban(string acct)
        {
            BusinessLogic bl = new BusinessLogic();
            List<string> banks = new List<string>();
            banks = await bl.GetBanksFromNuban(acct);
          
            return banks;
        }
    }

    public class CreateScheduleDebitPayment
    {
        public string StartDate { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public decimal Amount { get; set; }
        public int Scheduled { get; set; }
        public string remarks { get; set; }
        public int ScheduledCount { get; set; }
        public int BillType { get; set; }
        public string bankCode { get; set; }
    }

    public class PaymentScheduleType
    {
        public int SchedulePaymenyID { get; set; }
        public string SchedulePaymentText { get; set; }
    }

}
