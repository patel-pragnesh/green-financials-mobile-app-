using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace switch_mobile.Services.Abstractions.Entities
{
   public class TransferForex
    {      
           
            public string AccountDebit { get; set; }
           
            public string ChargesAccount { get; set; }
            public string Amount { get; set; }
      
            public string Beneficiary { get; set; }
     
            public string BeneficiaryName { get; set; }
         
            public string BeneficiaryAddress { get; set; }
         
            public string BeneficiaryBank { get; set; }
          
            public string BranchAddress { get; set; }
            public string CurrencyCode { get; set; }
        
            public string BeneficiaryAccount { get; set; }
           
            public string SWIFTCode { get; set; }
          
            public string SortCodeChips { get; set; }
           
            public string IntermediaryBank { get; set; }
           
            public string IntermediaryAddress { get; set; }
          
            public string IntermediaryAccountNo { get; set; }
    
            public string IntermediarySWIFTCode { get; set; }
           
            public string IntermediarySortCodeClips { get; set; }
    
            public string OffshoreCharges { get; set; }
           
            public string TransactionRate { get; set; }
           
            public string SourceOfFund { get; set; }
       
            public string CustomerEmail { get; set; }
   
            public string TransactionNarration { get; set; }
            public bool save { get; set; }
        
    }
}
