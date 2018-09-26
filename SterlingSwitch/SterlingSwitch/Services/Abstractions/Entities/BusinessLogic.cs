using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using SterlingSwitch.Extensions;
using SterlingSwitch.Helper;
using SterlingSwitch.Models;
using SterlingSwitch.Pages.LocalTransfer;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class BusinessLogic
    {
        static ApiRequest httpService = new ApiRequest();
        participatingBanks participatingBanksList = new participatingBanks();
        List<Banklist> BankList = new List<Banklist>();
        List<string> bankNameList = new List<string>();
        public BusinessLogic()
        {

        }


        public static string JsonizePageVisited(List<string> PagesVisited,string icon, string pageAlias)
        {
            string response = "";
            response = JsonConvert.SerializeObject(PagesVisited);
            var r = new PageName()
            {
                PageAlias=pageAlias,
                PageIcon=icon,


            };
            return response;
        }



        public static string JsonizePageVisitedV2(PageName json)
        {
            string response = "";
           GlobalStaticFields.PagesVisitedList.Add(json);
            var ff = GlobalStaticFields.PagesVisitedList;
            response = JsonConvert.SerializeObject(ff);
          
            return response;
        }

        internal static void LogFrequentPage(string name, string alias, string icon)
        {
            //first model it
            var pg = new SterlingSwitch.Models.PageName()
            {
                PageAlias = alias,
                Page = name,
                PageIcon = icon
            };
            //then persist it after jsonizing it
            Application.Current.Properties["PagesVisitedListV2"] = JsonizePageVisitedV2(pg);
            App.Current.SavePropertiesAsync();//brute saves it so it will persisit even when debugging/appcrash
        }

        public async Task<IbsresponseRoot> NameInquiryIntrabank(IntraBankNameInquiry intrabankNameInquiry)
        {
            var pdd = await ProgressDialog.Show("Name Enquiry. Please wait.");
            try
            {
                string postResponsex = "";
                var request = await httpService.PostIBS<IntraBankNameInquiry>(intrabankNameInquiry, "", URLConstants.SwitchApiLiveBaseUrl, "IBSIntegrator/IBSBridgeJSON", "NameEnquiryIntraBank");
                if (request.IsSuccessStatusCode)
                {
                    postResponsex = await request.Content.ReadAsStringAsync();
                    postResponsex = postResponsex.JsonCleanUp();
                    var jobj = JsonConvert.DeserializeObject<IbsresponseRoot>(postResponsex);

                    return jobj;
                }

                else
                {
                    return new IbsresponseRoot();
                }
            }
            catch (Exception ex)
            {
               await Task.Run(()=>( BusinessLogic.Log(ex.ToString(), "exception at name enquiry","", "", "", "nameenquiry")));
                string log = ex.ToString();
                return new IbsresponseRoot();

            }
            finally
            {
                await pdd.Dismiss();
            }


        }

        internal async static void SaveTaggedTrnx(int selectedCategoryTag, string trnxTagID)
        {
            var tag = new Tagging()
            {
                CategoryId=selectedCategoryTag.ToString(),
                TnnxID=trnxTagID,
                UserEmail=GlobalStaticFields.Customer.Email
            };

            try
            {
                string postResponsex = "";
                var request = await httpService.Post<Tagging>(tag, "", URLConstants.SwitchApiLiveBaseUrl, "Switch/TagTransaction", "TagTransactions");
                if (request.IsSuccessStatusCode)
                {
                    postResponsex = await request.Content.ReadAsStringAsync();
                    postResponsex = postResponsex.JsonCleanUp();
                    //var jobj = JsonConvert.DeserializeObject<IbsresponseRoot>(postResponsex);

                   
                }

              
            }
            catch (Exception ex)
            {
                await Task.Run(() => (BusinessLogic.Log(ex.ToString(), "exception at tag transaction", "", "", "", "tagtransactions")));
                string log = ex.ToString();
            

            }
        }

        public static async Task<List<AllAccountOfCustomer>> allAcctInfoAsync(string mobile)
        {
            var response = "";
            List<AllAccountOfCustomer> listOfAcct = new List<AllAccountOfCustomer>();
            try
            {
                AllAccountOfCustomer alc = new AllAccountOfCustomer();
                dynamic allAccount = new JObject();
                allAccount.mobile = mobile;
                var request = await httpService.Post<dynamic>(allAccount, "", URLConstants.SwitchApiBaseUrl, "Switch/GetAllOtherAcctDetailsByNumber", "BusinessLogic/allAcctInfoAsync");
                if (request.IsSuccessStatusCode)
                {
                    response = await request.Content.ReadAsStringAsync();
                    listOfAcct = JsonConvert.DeserializeObject<List<AllAccountOfCustomer>>(response);
                    return listOfAcct;
                }
            }
            catch (Exception rf)
            {
                List<AllAccountOfCustomer> nolistOfAcct = new List<AllAccountOfCustomer>();
                return nolistOfAcct;
            }

            return new List<AllAccountOfCustomer>();

        }
        public async Task<IbsresponseRoot> NameInquiryInterbank(InterbankNameInquiry interbankNameInquiry)
        {
            string postResponsex = "";
            try
            {
                var request = await httpService.PostIBS<InterbankNameInquiry>(interbankNameInquiry, "", URLConstants.SwitchApiLiveBaseUrl, "IBSIntegrator/IBSBridgeJSON", "NameEnquiryInterBank");
                if (request.IsSuccessStatusCode)
                {
                    postResponsex = await request.Content.ReadAsStringAsync();
                    postResponsex = postResponsex.JsonCleanUp();
                    var jobj = JsonConvert.DeserializeObject<IbsresponseRoot>(postResponsex);

                    return jobj;
                }

                else
                {
                    return new IbsresponseRoot();
                }
            }
            catch (Exception ex)
            {

                await Task.Run(()=> Log(ex.ToString(), "exception at name enquiry", "", "", "", "NameEnquiryInterBank"));
                return new IbsresponseRoot();
            }



        }
        public static async Task Log(string exception, string ExceptionRemark, string endPoint, string requestSentToAPI, string responseFromApi, string pageVisited)
        {
            try
            {
                var exc = new ExceptionModel()
                {
                    AppName = "SterlingSwitch",
                    Exception = exception.ToString(),
                    ExceptionRemark = ExceptionRemark,
                    Endpoint = endPoint,
                    PageVisited = pageVisited,
                    parameters = requestSentToAPI,
                    ResponseFromAPI = responseFromApi,
                    UserName = GlobalStaticFields.Username
                };
                var request = await Task.Run(()=> httpService.Log<ExceptionModel>(exc, "", URLConstants.ExceptionURL, URLConstants.ExceptionEndPoint));
            }
            catch (Exception ex)
            {
                string log = ex.Message;
            }
        }

        internal async static Task<string> DeleteTransferBeneficiary(SaveSwitchBeneficiary.GetSavedBeneficiaries selected)
        {
            var request = new DeleteTransferBenef()
            {
                SwitchID = GlobalStaticFields.Customer.Email,
                nuban = selected.beneficiaryNuban
            };
            var requestx = await httpService.Post<dynamic>(request, "", URLConstants.SwitchApiBaseUrl, "Switch/DeleteSwitchBeneficiary", "ManageLocalTrfRecipient");
            if (requestx.IsSuccessStatusCode)
            {
                var req = await requestx.Content.ReadAsStringAsync();
                return req;
            }
            else
            {
                return "";
            }
        }

        internal ObservableCollection<string> GetParticipatingBankNameListing(List<Banklist> response2)
        {



            var query = from b in response2
                        group b by new { b.BANKNAME }
                       into mySortedBank
                        select mySortedBank.FirstOrDefault();

            foreach (var item in query)
            {
                BankList.Add(item);
                bankNameList.Add(item.BANKNAME.ToUpper());
            }



            var ParticipatingBankNameListing = new ObservableCollection<string>(bankNameList.OrderBy(s => s).Distinct());

            participatingBanksList.bankNameList = ParticipatingBankNameListing;
            return ParticipatingBankNameListing;
        }

        internal ObservableCollection<Banklist> GetActualBankList(List<Banklist> response2)
        {



            var query = from b in response2
                        group b by new { b.BANKNAME }
                       into mySortedBank
                        select mySortedBank.FirstOrDefault();

            foreach (var item in query)
            {
                BankList.Add(item);
                bankNameList.Add(item.BANKNAME.ToUpper());
            }

            var ParticipatingBankListing = new ObservableCollection<Banklist>(BankList.OrderBy(s => s.BANKNAME.ToUpper()));
            participatingBanksList.bankList = ParticipatingBankListing;
            return ParticipatingBankListing;


        }

        public async Task<IbsresponseRoot> SterlingToSterlingTransfer(TransferSterlingToSterling transferToBankAccount)
        {

            string postResponsex = "";
            var request = await httpService.PostIBS<TransferSterlingToSterling>(transferToBankAccount, "", URLConstants.SwitchApiLiveBaseUrl, "IBSIntegrator/IBSBridgeJSON", "sterlingToSterlingTransfer");
            if (request.IsSuccessStatusCode)
            {
                postResponsex = await request.Content.ReadAsStringAsync();
                postResponsex = postResponsex.JsonCleanUp();
                var jobj = JsonConvert.DeserializeObject<IbsresponseRoot>(postResponsex);

                return jobj;
            }

            else
            {
                return new IbsresponseRoot();
            }
        }

        internal async Task<List<Banklist>> GetParticipatingBanksFromFile()
        {
            List<Banklist> bk = new List<Banklist>();
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(BusinessLogic)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("SterlingSwitch.Pages.LocalTransfer.File.GetParticipatingBanks.json");
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                json = json.JsonCleanUp();
                var data = JsonConvert.DeserializeObject<List<Banklist>>(json);
                if (data.Any())
                {
                   
                    bk = data;
                    return bk;
                }
            }

            return bk;
        }

        public async Task<IbsresponseRoot> SterlingToOtherBankTransfer(OtherBanks otherBanks)
        {

            string postResponsex = "";
            var request = await httpService.PostIBS<OtherBanks>(otherBanks, "", URLConstants.SwitchApiLiveBaseUrl, "IBSIntegrator/IBSBridgeJSON", "sterlingtoOtherBanktransfer");
            if (request.IsSuccessStatusCode)
            {
                postResponsex = await request.Content.ReadAsStringAsync();
                postResponsex = postResponsex.JsonCleanUp();
                var jobj = JsonConvert.DeserializeObject<IbsresponseRoot>(postResponsex);

                return jobj;
            }

            else
            {
                return new IbsresponseRoot();
            }
        }

        public async Task<List<IbsresponseRoot>> ParticipatingBankListAsync(EmptyIBSCall empty)
        {
            string postResponsex = "";

            participatingBanks pbList = new participatingBanks();
            List<Banklist> BankList = new List<Banklist>();
            List<string> bankNameList = new List<string>();

            var request = await httpService.PostIBS<EmptyIBSCall>(empty, "", URLConstants.SwitchApiLiveBaseUrl, "IBSIntegrator/GetPerticipatingBanksJSON", "GetParticipatingBanks");
            if (request.IsSuccessStatusCode)
            {
                postResponsex = await request.Content.ReadAsStringAsync();
                postResponsex = postResponsex.JsonCleanUp();
                var jobj = JsonConvert.DeserializeObject<List<IbsresponseRoot>>(postResponsex);

                return jobj;
            }
            else
            {
                return new List<IbsresponseRoot>();

            }



        }

       

        public async Task<List<Banklist>> ParticipatingBankListAsync2(EmptyIBSCall empty)
        {
            try
            {
                string postResponsex = "";

                participatingBanks pbList = new participatingBanks();
                List<Banklist> BankList = new List<Banklist>();
                List<string> bankNameList = new List<string>();

                var request = await httpService.PostIBS<EmptyIBSCall>(empty, "", URLConstants.SwitchApiLiveBaseUrl, "IBSIntegrator/GetPerticipatingBanksJSON", "GetParticipatingBanks");
                if (request.IsSuccessStatusCode)
                {
                    postResponsex = await request.Content.ReadAsStringAsync();
                    postResponsex = postResponsex.JsonCleanUp();
                    postResponsex = postResponsex.TrimStart('"');
                    var jobj = JsonConvert.DeserializeObject<List<Banklist>>(postResponsex);

                    return jobj;
                }
                else
                {
                    return new List<Banklist>();

                }
            }
            catch (Exception ex)
            {
               await BusinessLogic.Log(ex.ToString(), "Exception at get participating bank", "", "", "", "Business Logic");
                return new List<Banklist>();
            }



        }
        internal string ExtractTotalAmountcalculatedFromCurrencySwap(RateResponseModel data)
        {
            var split = data.message.Split('|');
            var split2 = split[0];
            string amount = split2;
            return amount;
        }

        internal string ExtractExchangeRateFromCurrencyRate(RateResponseModel data)
        {
            var split = data.message.Split(':');
            var split2 = split[1].Split('|');
            string rate = split2[0];
            return rate;
        }

        public static async void SendEmail(string email, string message)
        {
            try
            {
                var emailx = new sendEmai()
                {
                    Body = message,
                    DestinationEmail = email,
                    SourceEmail = "noreply@switch-ng.com",
                    Subject = "Switch Logon"
                };
                var request = await httpService.Post<sendEmai>(emailx, "", URLConstants.SwitchApiLiveBaseUrl, "Switch/SendMail", "BusinessLogic/SendEmail");

            }
            catch (Exception ex)
            {

            }
        }

        internal async Task<List<string>> GetBanksFromNuban(string acct)
        {
            List<string> bk = new List<string>();
            string url = $"Transaction/GetBankFromNUBAN?nuban={acct}";
            var banks = await httpService.Get("", URLConstants.SwitchApiBaseUrl, url, "GetBankFromNUBAN");
            if (banks.IsSuccessStatusCode)
            {
                var result =await banks.Content.ReadAsStringAsync();
                var result2 = JsonConvert.DeserializeObject<GetBankFromNuban>(result);
                if (result2.Status)
                {
                    foreach (var item in result2.Data)
                    {
                        bk.Add(item.Name);//loop through each bank name and add to a list to make the list
                      
                    }
                    //bk.Add("Choose another bank"); // will add this later
                    GlobalStaticFields.BankNameAndCode = null;
                    GlobalStaticFields.BankNameAndCode = result2.Data;

                    return bk;
                }
                else
                {
                    return bk;
                }
                
            }
            else
            {
                return bk;
            }

        }

        public static async void SendEmailGeneric(string email, string message, string subject)
        {
            try
            {
                var emailx = new sendEmai()
                {
                    Body = message,
                    DestinationEmail = email,
                    SourceEmail = "noreply@switch-ng.com",
                    Subject = subject
                };
                var request = await httpService.Post<sendEmai>(emailx, "", URLConstants.SwitchApiLiveBaseUrl, "Switch/SendMail", "BusinessLogic/SendEmail");

            }
            catch (Exception ex)
            {

            }
        }

        internal async Task<int> SaveFundsTrfBeneficiary(SaveSwitchBeneficiary beneficiary)
        {
            int result = 0;

            var saveList = await httpService.Post2<dynamic>(beneficiary, URLConstants.SwitchApiBaseUrl, "Switch/SaveSwitchBeneficiary", "Savebeneficiary");

            if (saveList.IsSuccessStatusCode)
            {
                var resp =await saveList.Content.ReadAsStringAsync();

            }

            return result;
        }

     

        public static bool IsConnectionOK()
        {
            if (CrossConnectivity.Current.IsConnected)
                return true;
            else
                return false;
        }


        internal async static Task<ObservableCollection<SaveSwitchBeneficiary.GetSavedBeneficiaries>> GetMyBeneficiaries(string email)
        {
            string response = "";
            ObservableCollection<SaveSwitchBeneficiary.GetSavedBeneficiaries> beneficiaryLists = new ObservableCollection<SaveSwitchBeneficiary.GetSavedBeneficiaries>();
            SaveSwitchBeneficiary.GetBeneRequest ben = new SaveSwitchBeneficiary.GetBeneRequest()
            {
                switchuserid = email,
                Referenceid = Utilities.GenerateReferenceId(),
                RequestType = 702,
                Translocation = GlobalStaticFields.GetUserLocation
            };

            var benList = await httpService.Post2<dynamic>(ben, URLConstants.SwitchApiBaseUrl, "Switch/GetSwitchBeneficiaryInNew", "GetBeneficiaryAPICall");
            if (benList.IsSuccessStatusCode)
            {
                response = await benList.Content.ReadAsStringAsync();

            }
            var conv = JsonConvert.DeserializeObject<ObservableCollection<SaveSwitchBeneficiary.GetSavedBeneficiaries>>(response);
            return conv;

        }


        public async static void RebindAccountDetails()
        {
            await Task.Run(()=>LoginHelper.ReBindPrimaryAccountDetail());
        }

    }
}
