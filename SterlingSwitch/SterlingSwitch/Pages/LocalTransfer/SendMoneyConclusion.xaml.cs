using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SterlingSwitch.Helper;
using SterlingSwitch.Models;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.Templates;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static switch_mobile.Services.Abstractions.Entities.SaveSwitchBeneficiary;

namespace SterlingSwitch.Pages.LocalTransfer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendMoneyConclusion : SwitchMasterPage
    {
        private SendMoneyViewModel svmx { get; set; }
        public int SelectedIndex { get; set; }
        public string Selecteditem { get; set; }
        public string scheduleDate { set; get; } 
        public string ScheduleEndDate { set; get; } 
        participatingBanks participatingBanksList = new participatingBanks();
        List<Banklist> BankList = new List<Banklist>();
        List<string> bankNameList = new List<string>();
        public CreateScheduleDebitPayment createScheduleDebitPayment { set; get; }
      
     


        public SendMoneyConclusion(SendMoneyViewModel svm)
        {
            InitializeComponent();
            svmx = new SendMoneyViewModel();
            if (svm == null)
            {
                return;
            }
            svmx = svm;

            IsSchdule.IsVisible = false;
            
            TransfertypePicker.ItemsSource = svm.TransferType;
            AccountListPicker.ItemsSource = svm.AccountListForPicker;
            AccountListPicker.RefreshContent = RefreshAccountList;
            RepeatSchedule.ItemsSource = svm.ListSchduleType;
            ScheduleDatePicker.MinimumDate = DateTime.Today;
           try
            {
                if (svm.IsExistingRecipient)
                {
                    this.PageTitle = $"Send money to {svm.SavedBeneficiaries.BenName.Split(' ')[0]}";
                }
            }
            catch (Exception ex)
            {
                string log = ex.ToString();
                BusinessLogic.Log(log, "exception at get first name on send money", "", "", "", "");
            }



            //BankNamePicker.ItemsSource = svm.ParticipatingBankNameList;



            this.BindingContext = svm;
        }

        private void RefreshAccountList()
        {
          svmx.BindAccount(GlobalStaticFields.Customer.PhoneNumber);
            AccountListPicker.ItemsSource = svmx.AccountListForPicker;
        }

        private async Task<ObservableCollection<string>> GetBankList()
        {

            BusinessLogic bl = new BusinessLogic();
            var getbanksList = new EmptyIBSCall()
            {
                ReferenceID = Utilities.GenerateReferenceId(),
                RequestType = "327"
            };
            var pd = await ProgressDialog.Show("Please wait.");
            var result = await bl.ParticipatingBankListAsync2(getbanksList);
            if (result != null && result.Count > 0)
            {
                GlobalStaticFields.ListOfbanks = result;
                BankList = result;
                participatingBanksList.bankList = bl.GetActualBankList(result);

                participatingBanksList.bankNameList = bl.GetParticipatingBankNameListing(result);
                await pd.Dismiss();
                return participatingBanksList.bankNameList;
            }
            await pd.Dismiss();
            return new ObservableCollection<string>();


        }

        public SendMoneyConclusion()
        {

            InitializeComponent();
           

        }

        private async void BtnPay_Clicked(object sender, EventArgs e)
        {
            try
            {




                if (IsValid())
                {
                    //var pd =  await ProgressDialog.Show("Validation. Please wait");
                    ConsumateTransfer();


                  
                }

                else
                {
                    MessageDialog.Show("Info", "You have to fill all fields before you can proceed", DialogType.Info, "OK", null, "", null);
                }
            }
            catch (Exception ex)
            {
                string log = ex.ToString();
                MessageDialog.Show("Error", "Unusual error occured. Our Tech Team has been alerted to this", DialogType.Error, "OK", null, "", null);
                await BusinessLogic.Log(ex.ToString(), "Error at Send money", "", "", "", "SendMoneyLocally");

            }

        }

       
        private void PaymentVerifyTPIN_Validated(object sender, bool e)
        {
            CreateScheduleDebitSteringBank(createScheduleDebitPayment);
        }

        private async void CreateScheduleDebitSteringBank(CreateScheduleDebitPayment createScheduleDebitPayment)
        {

            try
            {
                ApiRequest apirequest = new ApiRequest();
                var pddx = await ProgressDialog.Show("Initiating schedule. Please wait.");
                string endppoint = URLConstants.switchAPINewBaseURL + "Transaction/CreateScheduleDebitAccount";

                var response = await apirequest.Post(createScheduleDebitPayment, "", URLConstants.switchAPINewBaseURL, "Transaction/CreateScheduleDebitAccount", "SchedulePament");
                await pddx.Dismiss();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var JsonObjs = JsonConvert.DeserializeObject<APIResponse<int>>(result);
                        if (JsonObjs.Data > 0)
                        {
                            MessageDialog.Show("Success", "Your schedule Payment have been initaited.", DialogType.Success, "OK", () =>
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    GoToCategoryLanding();
                                });
                            }
                       , "", null);
                        }
                        else
                        {
                            MessageDialog.Show("Error", "Funds Transfer unsuccessful at this time. Please try again", DialogType.Error, "OK", null, "", null);
                        }
                    }
                    else
                    {
                        MessageDialog.Show("Error", "Funds Transfer unsuccessful at this time. Please try again", DialogType.Error, "OK", null, "", null);
                    }
                    //var JsonObj = JsonConvert.DeserializeObject<APIResponse<int>>(result);

                }
            }
            catch (Exception ex)
            {
               await Task.Run(()=> BusinessLogic.Log(ex.ToString(), "exception at createsehcduleDebit", "", "", "", ""));
                MessageDialog.Show("Error", "Funds Transfer unsuccessful at this time. Please try again", DialogType.Error, "OK", null, "", null);
            }
           
        }

        private bool IsValid()
        {
            if (SwitchSchedulePayment.IsToggled)
            {
                if (string.IsNullOrEmpty(scheduleDate))
                {
                    return false;
                }
                else if (string.IsNullOrEmpty(ScheduleEndDate))
                {
                    return false;
                }
                else if (RepeatSchedule.SelectedIndex < -1)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            if (svmx.IsNewOrExisting)//true for new
            {
                if (string.IsNullOrEmpty(txtAmount.Text) || string.IsNullOrEmpty(txtReference.Text) || string.IsNullOrEmpty(txtToAccount.Text))
                {
                    return false;
                }
                else if (!CheckIfDecimal(txtToAccount.Text))
                {
                    return false;
                }
                else if (txtToAccount.Text.Trim().Length != 10)
                {
                    return false;
                }
                else if (TransfertypePicker.SelectedItem == null)
                {
                    return false;
                }
                else if (TransfertypePicker.SelectedItem.ToString() == "To other Banks")
                {
                    if (string.IsNullOrEmpty(txtRecipientBank.Text))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }

            else
            {
                if (string.IsNullOrEmpty(txtAmount.Text) || string.IsNullOrEmpty(txtReference.Text))
                {
                    return false;
                }
                else if (string.IsNullOrEmpty(AccountListPicker.SelectedItem))
                {
                    return false;
                }

                else
                {
                    return true;
                }


            }



        }

        private bool CheckIfDecimal(string text)
        {
            try
            {
                Convert.ToDecimal(text);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        private async void ConsumateTransfer()
        {

            try
            {


                BusinessLogic bl = new BusinessLogic();
                var selectedDisplayedAcct = AccountListPicker.SelectedItem;
                var Nuban = GlobalStaticFields.Customer.ListOfAllAccounts.FirstOrDefault(k => k.AccountBalanceAccountType.ToLower().Trim() == selectedDisplayedAcct.ToLower().Trim()).nuban;
                svmx.AmountInputted = txtAmount.Text;
                svmx.MyAccount = Nuban;
                if (svmx.IsNewRecipient)
                {
                    svmx.BeneficiaryAccount = txtToAccount.Text;

                }
                else if (svmx.IsExistingRecipient)
                {
                    svmx.BeneficiaryAccount = svmx.SavedBeneficiaries.beneficiaryNuban;
                }
                //sterling to sterling transfer
                if (svmx.TransferTypeSelected.ToUpper().Trim() == SendMoneyConstantss.ToSterling.ToUpper().Trim())
                {


                    var nameVal = new IntraBankNameInquiry()
                    {
                        NUBAN = svmx.BeneficiaryAccount,
                        ReferenceID = Utilities.GenerateReferenceId(),
                        RequestType = "219"
                    };
                    //await pd.Message = "Name Enqiury. Please wait";
                   // var pdd = await ProgressDialog.Show("Name Enquiry. Please wait.");

                    var reply = await bl.NameInquiryIntrabank(nameVal);
                  //  await pdd.Dismiss();
                    if (reply != null)
                    {
                        if (reply.IBSResponse.ResponseCode == "00")
                        {
                            svmx.BeneficiaryName = reply.IBSResponse.ResponseText;
                            svmx.NameofBeneficiaryBank = "Sterling Bank";


                            svmx.AmountFormattedComma = string.Format("{0:n}", Convert.ToInt32(svmx.AmountInputted));

                            if (SwitchSchedulePayment.IsToggled)
                            {
                                var ScheduleID = svmx.paymentScheduleTypes.FirstOrDefault(x => x.SchedulePaymentText == RepeatSchedule.SelectedItem.Trim()).SchedulePaymenyID;

                                var FrequencyID = ScheduleFrequency(ScheduleDatePicker.Date, scheduleEndDatePicker.Date);

                                svmx.createScheduleDebitPayment = new CreateScheduleDebitPayment
                                {
                                    Amount = decimal.Parse(svmx.AmountInputted),
                                    bankCode = "0001",
                                    BillType = 1,
                                    FromAccount = svmx.MyAccount,
                                    remarks = txtReference.Text,
                                    Scheduled = ScheduleID,
                                    ScheduledCount = FrequencyID,
                                    StartDate = scheduleDate,
                                    ToAccount = svmx.BeneficiaryAccount
                                };

                                //svmx.createScheduleDebitPayment = s;

                                var msg = RepeatSchedule.SelectedItem.Trim() != "Never" ? RepeatSchedule.SelectedItem.Trim() : string.Empty;
                                MessageDialog.Show("Confimation", $"You have initiated a {msg} schedule payment of {svmx.AmountInputted} to {reply.IBSResponse.ResponseText} of sterling bank to start on {scheduleDate} for " +
                                    $"{svmx.createScheduleDebitPayment.ScheduledCount} times {Environment.NewLine} Do you want to continue", DialogType.Info, "Yes",
                                    () =>
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            if (GlobalStaticFields.Customer.IsTPin)
                                            {
                                                var SchedulePayTPIN = new PopUps.VerifyPinPage("Confirm schedule payment", svmx.AmountFormattedComma, "NGN", null);
                                                SchedulePayTPIN.Validated += SchedulePayTPIN_Validated;
                                            }
                                            else
                                            {

                                                CreateScheduleDebitSteringBank(svmx.createScheduleDebitPayment);
                                            }
                                        });
                                    }
                                  , "No", null);
                            }
                            else
                            {
                                svmx.SterlingToSterling = new TransferSterlingToSterling()
                                {
                                    Amount = txtAmount.Text,
                                    FromAccount = Nuban,
                                    ToAccount = svmx.BeneficiaryAccount,
                                    PaymentReference = txtReference.Text,
                                    ReferenceID = Utilities.GenerateReferenceId(),
                                    RequestType = "102"

                                };

                                svmx.AmountFormattedComma = string.Format("{0:n}", Convert.ToInt32(svmx.AmountInputted));
                                MessageDialog.Show("Confirmation", $"You have initiated a transfer of NGN {svmx.AmountFormattedComma} to {svmx.BeneficiaryName} of Sterling" + Environment.NewLine + "Do you want to continue?", DialogType.Info, "Yes", () =>
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        if (GlobalStaticFields.Customer.IsTPin)
                                        {
                                            var SendMoneyVerifySterlingTPIN = new PopUps.VerifyPinPage("Confirmation", svmx.AmountFormattedComma, "NGN", null);
                                            SendMoneyVerifySterlingTPIN.Validated += SendMoneyVerifyTPIN_Validated;
                                            Navigation.PushAsync(SendMoneyVerifySterlingTPIN);
                                        }
                                        else
                                        {
                                            ConsumateTransferToSterling(svmx.SterlingToSterling);
                                        }
                                    //ConsumateTransferToSterling2();
                                }
                                    );
                                }
                                  , "No", null
                              );

                            }
                        }
                        else
                        {
                            MessageDialog.Show("Info", "Name Enquiry failed at this time. Please try again", DialogType.Info, "OK", null, "", null);
                        }
                    }
                    else
                    {
                        MessageDialog.Show("Info", "Could not verify the name at this time. Please try again", DialogType.Info, "OK", null, "", null);
                    }


                }

                else if (svmx.TransferTypeSelected.ToUpper().Trim() == SendMoneyConstantss.ToOtherBank.ToUpper().Trim())//sterling to other banks
                {
                    try
                    {
                        if (svmx.IsNewRecipient)
                        {
                            svmx.NameofBeneficiaryBank = txtRecipientBank.Text;
                        }
                        else if (svmx.IsExistingRecipient)
                        {
                            svmx.NameofBeneficiaryBank = svmx.SavedBeneficiaries.Bank;
                            svmx.DestinationBankCode = svmx.SavedBeneficiaries.BankCode;
                        }

                        if (string.IsNullOrEmpty(svmx.DestinationBankCode))
                        {
                            svmx.DestinationBankCode = GetBankCode(txtRecipientBank.Text);

                        }
                        if (string.IsNullOrEmpty(svmx.DestinationBankCode))
                        {
                            MessageDialog.Show("Error", "Destination bank code error. Please try again", DialogType.Error, "OK", null, "", null);
                            return;
                        }
                        var nameVal = new InterbankNameInquiry()
                        {
                            DestinationBankCode = svmx.DestinationBankCode,
                            ToAccount = svmx.BeneficiaryAccount,
                            ReferenceID = Utilities.GenerateReferenceId(),
                            RequestType = "105"
                        };
                        var pdNameEnq = await ProgressDialog.Show("Name Enuiry. Please wait.");
                        var nameValResp = await bl.NameInquiryInterbank(nameVal);
                        await pdNameEnq.Dismiss();
                        if (nameValResp.IBSResponse.ResponseCode == "00")
                        {
                            svmx.BeneficiaryName = nameValResp.IBSResponse.AccountName;

                            if (SwitchSchedulePayment.IsToggled)
                            {
                                var ScheduleID = svmx.paymentScheduleTypes.FirstOrDefault(x => x.SchedulePaymentText == RepeatSchedule.SelectedItem.Trim()).SchedulePaymenyID;
                                var FrequencyID = ScheduleFrequency(ScheduleDatePicker.Date,scheduleEndDatePicker.Date);
                                svmx.createScheduleDebitPayment = new CreateScheduleDebitPayment
                                {
                                    Amount = decimal.Parse(svmx.AmountInputted),
                                    bankCode = svmx.DestinationBankCode,
                                    BillType = 2,
                                    FromAccount = svmx.MyAccount,
                                    remarks = txtReference.Text,
                                    Scheduled = ScheduleID,
                                    ScheduledCount = FrequencyID,
                                    StartDate = scheduleDate,
                                    ToAccount = svmx.BeneficiaryAccount
                                };

                                var msg = RepeatSchedule.SelectedItem.Trim() != "Never" ? RepeatSchedule.SelectedItem.Trim() : string.Empty;
                                MessageDialog.Show("Confimation", $"You have initiated a {msg} schedule payment of {svmx.AmountInputted} to {svmx.BeneficiaryName} of {svmx.NameofBeneficiaryBank} to start on {scheduleDate} for " +
                                    $"{svmx.createScheduleDebitPayment.ScheduledCount} times {Environment.NewLine} Do you want to continue", DialogType.Info, "Yes",
                                    () =>
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            if (GlobalStaticFields.Customer.IsTPin)
                                            {
                                                var SchedulePayTPIN = new PopUps.VerifyPinPage("Confirm schedule payment", svmx.AmountFormattedComma, "NGN", null);
                                                SchedulePayTPIN.Validated += SchedulePayTPIN_Validated;
                                            }
                                            else
                                            {

                                                CreateScheduleDebitSteringBank(svmx.createScheduleDebitPayment);
                                            }
                                        });
                                    }
                                  , "No", null);
                            }

                            else
                            {


                                svmx.OtherBanks = new OtherBanks()
                                {
                                    Amount = Convert.ToDouble(txtAmount.Text),
                                    ToAccount = svmx.BeneficiaryAccount,
                                    DestinationBankCode = svmx.DestinationBankCode,
                                    ReferenceID = Utilities.GenerateReferenceId(),
                                    RequestType = "101",
                                    PaymentReference = txtReference.Text,
                                    SessionID = nameValResp.IBSResponse.SessionID,
                                    NEResponse = nameValResp.IBSResponse.ResponseCode,
                                    BenefiName = nameValResp.IBSResponse.AccountName,
                                    FromAccount = Nuban


                                };
                                svmx.AmountFormattedComma = string.Format("{0:n}", Convert.ToInt32(svmx.AmountInputted));
                                MessageDialog.Show("Confirmation", $"You have initiated a transfer of {Utilities.GetCurrency("NGN")} {svmx.AmountFormattedComma} to {svmx.BeneficiaryName } of {svmx.NameofBeneficiaryBank}.{Environment.NewLine} You will charged {Utilities.GetCurrency("NGN")}52.50." + Environment.NewLine + "Do you want to continue?", DialogType.Info, "Yes", () =>
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        if (GlobalStaticFields.Customer.IsTPin)
                                        {
                                            var SendMoneyVerifyTPINOtherBanks = new PopUps.VerifyPinPage("Confirmation", svmx.AmountFormattedComma, "NGN", null);
                                            SendMoneyVerifyTPINOtherBanks.Validated += SendMoneyVerifyTPINOtherBanks_Validated;
                                            Navigation.PushAsync(SendMoneyVerifyTPINOtherBanks);
                                        }
                                        else
                                        {

                                            ConsumateTransferToOtherbanks(svmx.OtherBanks);
                                        }
                                    });
                                },
                                "No", null);
                            }
                        }
                        else
                        {
                            MessageDialog.Show("Error", "Error during name enquiry", DialogType.Error, "OK", null, "", null);
                            //await pdd.Dismiss();

                        }
                        
                    }
                    catch (Exception ex)
                    {

                        string log = ex.ToString();
                        await BusinessLogic.Log(log, "error at send money", "", "", "", "");

                    }

                }

                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MessageDialog.Show("Info", "Did not find the configured transfer type", DialogType.Error, "OK", null);
                    });
                }

            }
            catch (Exception ex)
            {
                var log = ex;
                await BusinessLogic.Log(ex.ToString(), "Error at Send money", "", "", "", "SendMoneyLocally");
                Device.BeginInvokeOnMainThread(() =>
                {
                    MessageDialog.Show("Info", "Sorry. Error Occured. Please try again", DialogType.Error, "OK", null);


                });

            }
            //await pdd.Dismiss();


        }

        private void SchedulePayTPIN_Validated(object sender, bool e)
        {
            CreateScheduleDebitSteringBank(svmx.createScheduleDebitPayment);
        }

        private void DebitAccount_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (GlobalStaticFields.Customer.ListOfAllAccounts != null && GlobalStaticFields.Customer.ListOfAllAccounts?.Count > 0)
            {
                
                var accountbalace = GlobalStaticFields.Customer.ListOfAllAccounts[AccountListPicker.SelectedIndex].balance;
                var currency = GlobalStaticFields.Customer.ListOfAllAccounts[AccountListPicker.SelectedIndex].currencyCode;
               // svmx.Balance = $"Balance {Utilities.GetCurrency(currency)} {accountbalace}";
                this.SubPageTitle = $"Balance {Utilities.GetCurrency(currency)} {accountbalace}";
            }

        }


        private void SendMoneyVerifyTPINOtherBanks_Validated(object sender, bool e)
        {
            ConsumateTransferToOtherbanks(svmx.OtherBanks);
        }

        private void SendMoneyVerifyTPIN_Validated(object sender, bool e)
        {
            ConsumateTransferToSterling(svmx.SterlingToSterling);
        }

        private async void ConsumateTransferToSterling(TransferSterlingToSterling sterlingToSterling)
        {
            var pddx = await ProgressDialog.Show("Funds Transfer. Please wait.");
            BusinessLogic bl = new BusinessLogic();
            var transferResult = await bl.SterlingToSterlingTransfer(sterlingToSterling);
            await pddx.Dismiss();
            if (transferResult.IBSResponse.ResponseCode == "00")
            //string testresult = "00";
            //if (testresult == "00")
            {
                ReloadAcct();
                if (svmx.IsSaveAsBeneficiary)
                {
                    svmx.SaveBeneficiary();
                }
                var trxntagid = transferResult.IBSResponse.TrnxTagID;
                await Navigation.PushModalAsync(new Pagelanding.CategoryTagLanding(trxntagid));
                //MessageDialog.Show("Success", "Funds Transfer was successful.", DialogType.Success, "OK", () =>
                // {
                //     Device.BeginInvokeOnMainThread(() =>
                //     {
                //         GoToCategoryLanding();
                //     });
                // }
                //, "", null);

               
           
            }
            else
            {
                MessageDialog.Show("Error", "Funds Transfer unsuccessful at this time. Please try again", DialogType.Error, "OK", null, "", null);
            }
        }


        private void ConsumateTransferToSterling2()
        {
            MessageDialog.Show("Info", "Test OK", DialogType.Success, "OK",
               () =>
               {
                   Device.BeginInvokeOnMainThread(() =>
                   {
                       GoToCategoryLanding();
                   });
               }

                ,
                "", null);


        }

        private void GoToCategoryLanding()
        {
            //BusinessLogic.RebindAccountDetails();
            Navigation.PushModalAsync(new Pages.Pagelanding.CategoryTagLanding());
        }
        private async void ConsumateTransferToOtherbanks(OtherBanks otherBanks)
        {
            try
            {
                BusinessLogic bl = new BusinessLogic();
                var pdc = await ProgressDialog.Show("Sending Money. Please wait.");

                 var result = await bl.SterlingToOtherBankTransfer(otherBanks);
                //string testresult = "00";
                await pdc.Dismiss();
                if (result.IBSResponse.ResponseCode == "00")
                //if (testresult == "00")
                {
                    ReloadAcct();
                    if (svmx.IsSaveAsBeneficiary)
                    {
                       await Task.Run(()=> svmx.SaveBeneficiary());
                    }
                    await Navigation.PushModalAsync(new Pagelanding.CategoryTagLanding());
          

                }
                else
                {
                    MessageDialog.Show("Error", "Funds transfer was not successful", DialogType.Error, "OK", null, "", null);

                }
            }
            catch (Exception ex)
            {

                string log = ex.ToString();
                Device.BeginInvokeOnMainThread(() =>
                {
                    MessageDialog.Show("Error", "Unusual Error", DialogType.Error, "OK", null, "", null);
                }
                );

            }

        }

        private void ReloadAcct()
        {
            BusinessLogic.RebindAccountDetails();
        }

        private void TransfertypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TransfertypePicker.SelectedItem == null)
            {
                return;
            }
            if (TransfertypePicker.SelectedItem.ToString() == "To other Banks")
            {
                svmx.TransferTypeSelected = SendMoneyConstantss.ToOtherBank;//  "To other Banks";
                StackForOtherbanksPicker.IsVisible = true;
            }
            else
            {
                svmx.TransferTypeSelected = SendMoneyConstantss.ToSterling;// "To Sterling";

                StackForOtherbanksPicker.IsVisible = false;

            }
        }

        private async void BankNamePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GlobalStaticFields.ParticipatingBankNameList != null && GlobalStaticFields.ParticipatingBankNameList.Count > 0)
            {
                LaunchPickerPopUp(GlobalStaticFields.ParticipatingBankNameList, "");
            }
            else
            {
                await GetBankList();
                LaunchPickerPopUp(participatingBanksList.bankNameList, "");
                //BankNamePicker.ItemsSource = GetBankList().Result;

            }

        }

        private async void LaunchPickerPopUp(ObservableCollection<string> participatingBankNameList, string v)
        {
            if (participatingBankNameList == null)
            {
                MessageDialog.Show("Info", "No Banks List gotten at this time", DialogType.Info, "OK", null);
                return;
            }
            //var pickerPop = new SearchableExtendedPickerPopup(participatingBankNameList, Title, "ListOfBanks");
            //pickerPop.SelectedIndexChanged2 += (p, t) =>
            //{
            //    SelectedIndex = t.SelectedIndex;
            //    Selecteditem = t.DisplayText;
            //    var selectedBank = participatingBankNameList.ElementAt(SelectedIndex);
            //    txtRecipientBank.Text = selectedBank;

            
            //};

            var searchableList = new SearchablePickerPopUp(participatingBankNameList, Title);
            searchableList.SelectedIndexChanged += SearchableList_SelectedIndexChanged;
            //await Navigation.PushPopupAsync(pickerPop, true);
            await Navigation.PushPopupAsync(searchableList, true);

        }


        public async void LaunchPickerBanks(ObservableCollection<string> participatingBankNameList, string v)
        {
            if (participatingBankNameList == null)
            {
                MessageDialog.Show("Info", "No Banks List gotten at this time", DialogType.Info, "OK", null);
                return;
            }
          

            var searchableList = new SearchablePickerPopUp(participatingBankNameList, Title);
            searchableList.SelectedIndexChanged += SearchableList_SelectedIndexChanged;
            //await Navigation.PushPopupAsync(pickerPop, true);
            await Navigation.PushPopupAsync(searchableList, true);

        }

        private void SearchableList_SelectedIndexChanged(object sender, SearchablePickerPopUp.SearchablePickerItems e)
        {
           
            SelectedIndex = e.SelectedIndex;
            Selecteditem = e.DisplayText;
            //var selectedBank = participatingBankNameList.ElementAt(SelectedIndex);
            //txtRecipientBank.Text = selectedBank;
           
            if (Selecteditem == SendMoneyConstantss.ShowMore)
            {
                txtRecipientBank.Text = "";
                svmx.NameofBeneficiaryBank = "";

                LaunchPickerBanks(GlobalStaticFields.ParticipatingBankNameList, "Select Banks");
                StackForOtherbanksPicker.IsEnabled = true;

            }
            else
            {
                txtRecipientBank.Text = Selecteditem;
                svmx.NameofBeneficiaryBank = Selecteditem;
                StackForOtherbanksPicker.IsEnabled = true;
            }


        }

    

        private string GetBankCode(string bankName)
        {
            string bankcode = "";
            //var code = ParticipatingBankList.Where(d => d.BANKNAME == bankName).FirstOrDefault().BANKCODE;
            // var code = page.ParticipatingBankList.Where(d => d.BANKNAME == bankName).FirstOrDefault().BANKCODE;
            foreach (var item in GlobalStaticFields.ListOfParticipatingBanks.bankList)
            {
                if (item.BANKNAME.ToUpper() == bankName.ToUpper())
                {
                    bankcode = item.BANKCODE;

                    continue;
                }
            }
            return bankcode;
        }

        private void SwitchSchedulePayment_Toggled(object sender, ToggledEventArgs e)
        {
            try
            {
                IsSchdule.IsVisible = false;
                //svmx.IsScheduleVisible = false;
                if (SwitchSchedulePayment.IsToggled)
                {
                    IsSchdule.IsVisible = true;
                    //svmx.IsScheduleVisible = true;
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void ScheduleDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            scheduleDate = e.NewDate.ToString("dd/MM/yyyy");

            scheduleEndDatePicker.MinimumDate = e.NewDate.AddDays(1);

           
        }

        private async void txtToAccount_TextChanged(object sender, TextChangedEventArgs e)
        {

            //txtToAccount_Completed(this, e);
            List<string> guessedBanks = new List<string>();
       
            if (!string.IsNullOrEmpty(txtToAccount.Text.Trim()))
            {
                var acct = txtToAccount.Text.Trim();
                if (acct.Length==10 && svmx.TransferTypeSelected==SendMoneyConstantss.ToOtherBank)
                {
                    txtToAccount.IsFocused = false;
                    txtRecipientBank.EnableActivityIndicator = true;
                    guessedBanks =await svmx.GetBanksFromNuban(acct);
                    LaunchPickerPopUpGuessBank(guessedBanks, "Select a bank"); 

                
                }
            }
        }

     

        private async void LaunchPickerPopUpGuessBank(List<string> guessedBanks, string v)
        {
            if (guessedBanks == null)
            {
                MessageDialog.Show("Info", "No Banks List gotten at this time", DialogType.Info, "OK", null);
                return;
            }

            txtRecipientBank.EnableActivityIndicator = false;
            guessedBanks.Add(SendMoneyConstantss.ShowMore);
            var searchableList = new SearchablePickerPopUp(guessedBanks, Title);
            searchableList.SelectedIndexChanged += SearchableList_SelectedIndexChanged;
            //await Navigation.PushPopupAsync(pickerPop, true);
            await Navigation.PushPopupAsync(searchableList, true); 
        }

        private void GuessBankPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void scheduleEndDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            ScheduleEndDate = e.NewDate.ToString("dd/MM/yyyy");
        }

        public int ScheduleFrequency(DateTime startDate, DateTime EndDate)
        {
            int FrequencyID = 0;
            var ScheduleID = svmx.paymentScheduleTypes.FirstOrDefault(x => x.SchedulePaymentText == RepeatSchedule.SelectedItem.Trim()).SchedulePaymenyID;
           
            var days = Convert.ToInt32((EndDate - startDate).TotalDays);
            if (ScheduleID == 0)
            {

                FrequencyID = 1;
            }

            if(ScheduleID == 1)
            {
                FrequencyID = days + 1;
            }

            if(ScheduleID == 2)
            {
                FrequencyID = (days / 7) + 1;
            }
            if (ScheduleID == 3)
            {
                FrequencyID = (days / 30) + 1;
            }

            if (ScheduleID == 4)
            {
                FrequencyID = (days / 365) + 1;
            }

            return FrequencyID;
        }
    }



    public enum BankType
    {
        Sterling, Others
    }

}