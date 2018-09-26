using SterlingSwitch.Helper;
using SterlingSwitch.Pages.BillsPayment.Service;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SterlingSwitch.Models.SomeConstant;

namespace SterlingSwitch.Pages.BillsPayment
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PayBillsPage : SwitchMasterPage
    {
        public string SelectedAccount { get; set; }
        public List<Category> BillerCategories { get; set; }
        public List<string> BillerCategoryName { get; set; }
        public string categoryId { get; set; }
        public int CategoryselectedIndex = -1;
        public int BillerselectedIndex = -1;
        public int SelectedProductId = -1;
        public List<BillerListResponse> BillerList;
        public List<string> billerList;
        public string selectedbillerField;
        public List<BillerItemList> products;
        private List<string> product;

        public string BillerName { get; set; }
        public List<string> SelectedProduct { get; set; }
        public string SelectedBiller { get; set; }
        public string Subcharge { get; set; }
        public string SelectedProductAmount { get; set; }
        public string SelectedPaymentCode { get; set; }
        double totalAmount { get; set; }

        public PayBillsPage()
        {
            InitializeComponent();
            this.BindingContext = this;

            DebitAccount.RefreshContent = GetAccounts().Wait;
            Category.RefreshContent = RefreshCategories().Wait;
            var x = (this.GetType().Name);
            BusinessLogic.LogFrequentPage(x, PageAliasConstant.PayBills, ImageConstants.PayBillsIcon);

        }

        private async Task GetAccounts()
        {
            List<string> acc = new List<string>();
            if (GlobalStaticFields.Customer.ListOfAllAccounts != null && GlobalStaticFields.Customer.ListOfAllAccounts.Count > 0)
            {
                acc.Clear();
                foreach (var item in GlobalStaticFields.Customer.ListOfAllAccounts)
                {
                    acc.Add(item.AccountNumberWithBalance);
                }
                DebitAccount.ItemsSource = acc;
            }
            else
            {
                acc.Clear();
                var accounts = await GlobalStaticFields.Customer.GetAccountsbyPhoneNumber(GlobalStaticFields.Customer.PhoneNumber);
                foreach (var item in accounts)
                {
                    acc.Add(item.AccountNumberWithBalance);
                }
                DebitAccount.ItemsSource = acc;
            }
        }

    

        private async Task RefreshCategories()
        {           
            BillerCategories =  await BillerServices.GetBillerCategories();
            if (BillerCategories != null && BillerCategories.Count > 0)
            {
                BillerCategoryName = new List<string>();
                foreach (var item in BillerCategories)
                {
                    BillerCategoryName.Add(item.Name);
                }
                Category.ItemsSource = BillerCategoryName;
            }          
        }
        private void Category_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            CategoryselectedIndex = Category.SelectedIndex;
            ResetPickers();

            if (CategoryselectedIndex >= 0)
            {
                categoryId = BillerCategories[CategoryselectedIndex]?.ID;
                GetBillers();
            }
        }
        
        private async void GetBillers()
        {

            var pd = await ProgressDialog.Show("Getting Billers. Please Wait...");
            var biller = new BillerPost();
            biller.billerId = categoryId;
            BillerList = await BillerServices.GetBillerBillersByCategory(biller);
            SetBillerNames();
            await pd.Dismiss();
        }
        private void SetBillerNames()
        {
            billerList = new List<string>();
            if (BillerList.Count > 0)
            {
                foreach (var item in BillerList)
                {
                    billerList.Add(item.Name);
                }
                Billers.ItemsSource = billerList;
            }
        }

        private void Billers_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            BillerProduct.SelectedIndex = -1;
            BillerProduct.SelectedItem = string.Empty;

            try
            {
                BillerselectedIndex = Billers.SelectedIndex;
                SelectedBiller = BillerList[BillerselectedIndex]?.ID;
                SetBillerField();
                GetBillerProducts();
            }
            catch (Exception ex)
            { }
        }

        private string SetSubCharge()
        {
            if (SelectedProductId != -1)
                return Subcharge = BillerList[BillerselectedIndex].Surcharge;
            else
                return "";
        }
        private string SetBillerField()
        {
            if (BillerselectedIndex != -1)
            {
                selectedbillerField = BillerList[BillerselectedIndex].CustomerFieldLabel;
                BillerLabel.Placeholder = selectedbillerField;
                return selectedbillerField;
            }
            else
                return "";
        }

        private void GetBillerProducts()
        {
            _GetBillerItems();
        }

        private async void _GetBillerItems()
        {
            var pd = await ProgressDialog.Show("Getting Products. Please Wait...");
            var billerItems = new BillerItemPost();
            billerItems.ItemID = SelectedBiller;
            products = await BillerServices.GetBillerProducts(billerItems);
            SetProductNames();
            await pd.Dismiss();
        }

        private List<string> SetProductNames()
        {
            product = new List<string>();
            try
            {
                if (products.Count > 0)
                {
                    if (product.Count > 0)
                    {
                        product = new List<string>();
                    }
                    foreach (var item in products)
                    {
                        product.Add(item.Name);
                    }
                    BillerProduct.ItemsSource = product;
                    return SelectedProduct;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
            }
            return product;
        }

        private void BillerProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedProductId = BillerProduct.SelectedIndex;
            try
            {
                SetSubCharge();
                SelectedProductAmount = setAmount();
                txtAmount.Text = SelectedProductAmount;
                SelectedPaymentCode = setPaymentCode();
            }
            catch (Exception ex) { }
        }

        public string setAmount()
        {
            if (SelectedProductId != -1)
            {
                var amount = products[SelectedProductId].Amount;
                if (amount == "0")
                    grdAmount.InputTransparent = false;
                else
                    grdAmount.InputTransparent = true;

                return amount;
            }
            return "";
        }

        public string setPaymentCode()
        {
            if (SelectedProductId != -1)
            {
                return products[SelectedProductId].PaymentCode;
            }
            return "";
        }

        private async void ContinueButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Category.SelectedItem))
            {
                MessageDialog.Show("OOPS", "Kindly select biller category to proceed", DialogType.Error, "DISMISS", null);
                return;
            }
            if (string.IsNullOrEmpty(Billers.SelectedItem))
            {
                MessageDialog.Show("OOPS", "Kindly select a biller to proceed", DialogType.Error, "DISMISS", null);
                return;
            }
            if (string.IsNullOrEmpty(BillerProduct.SelectedItem))
            {
                MessageDialog.Show("OOPS", "Kindly select a biller product to proceed", DialogType.Error, "DISMISS", null);
                return;
            }
            if (string.IsNullOrEmpty(BillerLabel.Text))
            {
                MessageDialog.Show("OOPS", $"Kindly enter your {selectedbillerField}  to proceed", DialogType.Error, "DISMISS", null);
                return;
            }
            if (string.IsNullOrEmpty(txtAmount.Text))
            {
                MessageDialog.Show("OOPS", "Enter an amount", DialogType.Error, "DISMISS", null);
                return;
            }
            if (string.IsNullOrEmpty(DebitAccount.SelectedItem))
            {
                MessageDialog.Show("OOPS", "Please select account to debit", DialogType.Error, "DISMISS", null);
                return;
            }
            try
            {
                MessageDialog.Show("Transaction Confirmation", $"You are about to make payment for {Billers.SelectedItem} worth \n {Utilities.GetCurrency("NGN")}{txtAmount.Text:##,###}. Payment will attract a sub charge of {Utilities.GetCurrency("NGN")}{Convert.ToDecimal(Subcharge).ToString("N2")}", DialogType.Question, "Do you want to Proceed ?",
               MakeBillsPayment, "Cancel", null);
            }
            catch (Exception ex)
            {
                MessageDialog.Show("SUCCESS", $"Sorry we are unable to process your request at the moment. Kindly try again later", DialogType.Error, "DISMISS", null);
            }
        }

        async void MakeBillsPayment()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                totalAmount = (Convert.ToDouble(txtAmount.Text) + Convert.ToDouble(Subcharge));
                if (GlobalStaticFields.Customer.IsTPin)
                {
                    var IsTPINValidate = new PopUps.VerifyPinPage("Confirmation",string.Format("{0:N1}",totalAmount),"NGN",null);
                    IsTPINValidate.Validated += IsTPINValidate_Validated;
                    await  Navigation.PushAsync(IsTPINValidate);
                }
                else
                {
                    DoBillsPaymentTransaction();
                }
                
            });
        }

        private void IsTPINValidate_Validated(object sender, bool e)
        {
            DoBillsPaymentTransaction();
        }

        public async void DoBillsPaymentTransaction()
        {
            var pd = await ProgressDialog.Show("Sending Request. Please Wait...");

            var accSplitted = Utilities.SplitBeneficiaryDetailsByTab(DebitAccount.SelectedItem.Trim());
            string AcctToDebit = accSplitted[0];
            // totalAmount = (Convert.ToDouble(SelectedProductAmount) + Convert.ToDouble(Subcharge));
            
            string amt = totalAmount.ToString("##");

            var payBills = new PayBillerPost
            {
                ActionType = "",
                Referenceid = Utilities.GenerateReferenceId(),
                RequestType = "108",
                SubscriberInfo1 = BillerLabel.Text,
                Translocation = GlobalStaticFields.GetUserLocation,
                amt = amt,
                email = GlobalStaticFields.Customer.Email,
                mobile = GlobalStaticFields.Customer.PhoneNumber,
                nuban = AcctToDebit.Trim(),
                paymentcode = SelectedPaymentCode
            };
            var response = await BillerServices.PayBiller(payBills);
            await pd.Dismiss();
            if (string.Equals(response.message, "Not successful", StringComparison.OrdinalIgnoreCase))
            {
                MessageDialog.Show("ERROR", $"Sorry we are unable to process your request at the moment. Kindly try again later", DialogType.Error, "DISMISS", null);
                return;

            }
            else
            {
                MessageDialog.Show("SUCCESS", $"BILLS PAYMENT FOR {BillerProduct.SelectedItem} WAS SUCCESSFUL", DialogType.Success, "DISMISS", null);
                SaveBillPayment();
            }
        }        

        private void ResetPickers()
        {
             
            Billers.SelectedIndex = -1;
            Billers.SelectedItem = string.Empty;
            Billers.Placeholder = "Select Biller";
            BillerProduct.SelectedIndex = -1;
            BillerProduct.SelectedItem = string.Empty;
            BillerProduct.Placeholder = "Select a Product";
        }

        private  void SaveBillPayment()
        {
            // implement save bills payment
        }



        protected override  void OnAppearing()
        {
            base.OnAppearing();          
        }
    }
}