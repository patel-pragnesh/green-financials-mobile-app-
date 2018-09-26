using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
    public class SpectaValidateBVN
    {
        public string bvn { get; set; }
        public string dob { get; set; }
        public string lastname { get; set; }
    }


    public class SpectaValidateNuban
    {
        public string nuban { get; set; }
        public string email { get; set; }
    }


    public class ProcessStatus
    {
        public int status { get; set; }
        public string message { get; set; }
    }



    public class SpectaProcess
    {
        public static string salaryAmt { get; set; }
        public static string reqAmt { get; set; }
        public static string eligibleAmt { get; set; }
        public static string accomodationType { get; set; }
        public static string maritalStatus { get; set; }
        public static string dateOfBirth { get; set; }
        public static string numberOfDependants { get; set; }
        public static string jobChangeCount { get; set; }
        public static string howLongInResidence { get; set; }
        public static string tenor { get; set; }
        public static string email { get; set; }
        public static string lastName { get; set; }
        public static string firstName { get; set; }
        public static string bvn { get; set; }
        public static string phoneNumber { get; set; }
        public static int channelId { get; set; }
        public static string gender { get; set; }
        public static string isCustomer { get; set; }
        public static string address { get; set; }
        public static string isTermsAccepted { get; set; }
        public static string otp { get; set; }
        public static string empName { get; set; }
        public static string empAddress { get; set; }
        public static string loanPurpose { get; set; }
        public static string acctNo { get; set; }
        public static string Salary_Act_No { get; set; }
        public static string monthlyRepayment { get; set; }
        public static string eligibleAmount { get; set; }
        public static string repaymentDay { get; set; }
    }

    public class SpectaProcess2
    {
        public string salaryAmt { get; set; }
        public string reqAmt { get; set; }
        public string eligibleAmt { get; set; }
        public string accomodationType { get; set; }
        public string maritalStatus { get; set; }
        public string dateOfBirth { get; set; }
        public string numberOfDependants { get; set; }
        public string jobChangeCount { get; set; }
        public string howLongInResidence { get; set; }
        public string tenor { get; set; }
        public string email { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string bvn { get; set; }
        public string phoneNumber { get; set; }
        public int channelId { get; set; }
        public string gender { get; set; }
        public string isCustomer { get; set; }
        public string address { get; set; }
        public string isTermsAccepted { get; set; }
        public string otp { get; set; }
        public string empName { get; set; }
        public string empAddress { get; set; }
        public string loanPurpose { get; set; }
        public string acctNo { get; set; }
        public string Salary_Act_No { get; set; }
    }


}
