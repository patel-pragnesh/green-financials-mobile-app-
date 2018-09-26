using SterlingSwitch.Models;
using SterlingSwitch.Pages.LocalTransfer;
using SterlingSwitch.Pages.Pagelanding;
using SterlingSwitch.Templates;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.AllTransactions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailedTransactionView : SwitchMasterPage
    {
        private  RefinedTransactions TransactionDetails {get;set;}
        public string TransactionID { get; set; }
        public string History { get; set; }
        public string AmountSent { get; set; }
        public string AmountReceived { get; set; }
        public string InitiateTransaction { get; set; }
        private SendMoneyViewModel svm;

        public DetailedTransactionView (RefinedTransactions obj)
		{
            TransactionDetails = obj;
			InitializeComponent ();
            this.BindingContext = obj;
            LoadDetails();
		}

        private void LoadDetails()
        {
            try
            {
                TransactionID = TransactionDetails.ReferenceID;
                lblHistory.Text = $"History with {TransactionDetails.BeneficiaryName}";
                lblTotalAmountSent.RightText = TransactionDetails.Amount;
                lblTotalReceived.RightText = TransactionDetails.Amount;
                optCell.HeaderText = TransactionDetails.BeneficiaryName;
                optCell.SubHeaderText = TransactionDetails.TransactionDate;
                lblAmount.Text = TransactionDetails.Amount;
                optCell.isBoxViewLineVisible = false;
                // = $"Send Money to {TransactionDetails.BeneficiaryName}";
            }
            catch (Exception ex)
            {
                string log = ex.ToString();
            }
        }

        void SendMoney_Tapped(object sender, System.EventArgs e)
        {
            svm = new SendMoneyViewModel();
            svm.IsExistingRecipient = true;
            svm.IsNewRecipient = false;
            Navigation.PushAsync(new Pages.LocalTransfer.SendMoney());
        }

        private void TagCategory_Tapped(object sender, EventArgs e)
        {
            try
            {
          
                var trnx = TransactionDetails.ID.ToString();

                Navigation.PushModalAsync(new CategoryTagLanding(trnx));
            }
            catch (Exception)
            {

               
            }
        }
    }
}