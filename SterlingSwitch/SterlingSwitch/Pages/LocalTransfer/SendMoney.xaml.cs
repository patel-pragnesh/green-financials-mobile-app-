using Rg.Plugins.Popup.Extensions;
using SterlingSwitch.Custom.Controls;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using switch_mobile.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SterlingSwitch.Models.SomeConstant;

namespace SterlingSwitch.Pages.LocalTransfer
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SendMoney : SwitchMasterPage
    {
        private SendMoneyViewModel svm;
        public int selectedindex;
        public string selectedItem;
		public SendMoney ()
		{
			InitializeComponent ();
            svm = new SendMoneyViewModel();

            var x = this.GetType().Name;//get name of this present page

            BusinessLogic.LogFrequentPage(x, PageAliasConstant.SendMoney, ImageConstants.SendMoneyIcon);


           }

        private void tapRecipientType_Tapped(object sender, EventArgs e)
        {
            var lb = (StackLayout)sender;
            var lbx = lb.Children[0];
            var lbxx = (LabelUnderline)lbx;

            if (NewOrExisting.IsVisible == true)
            {
                NewOrExisting.IsVisible = false;

                //lbxx.ImageSource = ImageSource.FromFile("arrowDown.png");
            }
            else
            {
                NewOrExisting.Opacity = 0;
                NewOrExisting.IsVisible = true;
                NewOrExisting.FadeTo(1, 200, Easing.SinIn);
                //lbxx.ImageSource = ImageSource.FromFile("BlackBackArrow.png");
            }
        }

        private void tapNew_Tapped(object sender, EventArgs e)
        {
            svm.IsNewRecipient = true;
            svm.IsExistingRecipient = false;
            
            Navigation.PushAsync(new Pages.LocalTransfer.SendMoneyConclusion(svm));
        }

        private async void tapExisting_Tapped(object sender, EventArgs e)
        {
            svm.IsExistingRecipient = true;
            svm.IsNewRecipient = false;
            if (GlobalStaticFields.ListOfTransferbeneficiaries!=null)
            {
                RecipientPicker.ItemsSource = GlobalStaticFields.ListOfTransferbeneficiaries;
                LaunchPickerPopUp(GlobalStaticFields.ListOfTransferbeneficiaries, "");
        
            }

            else
            {
                var pd = await ProgressDialog.Show("Beneficiaries, Please wait.");
                var mybeneficiary = await BusinessLogic.GetMyBeneficiaries(GlobalStaticFields.Customer.Email);
                await pd.Dismiss();
                if (mybeneficiary.Any())
                {
                   

                    RecipientPicker.ItemsSource = mybeneficiary;
                    GlobalStaticFields.ListOfTransferbeneficiaries = mybeneficiary;
                    LaunchPickerPopUp(mybeneficiary, "");
            
                }
                else
                {
                   

                    await DisplayAlert("Info", "No record found", "OK");
                }
            }
       
          
        }

        private void LaunchPickerPopUp(ObservableCollection<SaveSwitchBeneficiary.GetSavedBeneficiaries> listOfTransferbeneficiaries, string v)
        {
            
           
            var pickerPop = new SearchableExtendedPickerPopup(listOfTransferbeneficiaries, Title, "Transferbeneficiaries");
            pickerPop.SelectedIndexChanged2 += (p, t) =>
            {
                 selectedindex = t.SelectedIndex;
               selectedItem = t.DisplayText;
                var selectedbeneficiary = listOfTransferbeneficiaries.ElementAt(selectedindex);
                svm.SavedBeneficiaries = selectedbeneficiary;
                // bankcode was previously used but this was changed to transfer type by ramon upon request on the below date. this wil contain values like "To Sterling" or "To other banks"
                string dateNewPropCreated = "2018-08-03";
                if (selectedbeneficiary.dateAdded<=Convert.ToDateTime(dateNewPropCreated))
                {
                    svm.TransferTypeSelected = selectedbeneficiary.BankCode??"NA";
                }
                else if(selectedbeneficiary.dateAdded>=Convert.ToDateTime(dateNewPropCreated))
                {
                    svm.TransferTypeSelected = selectedbeneficiary.TransactionType??"NA";

                }
                
                Navigation.PushAsync(new Pages.LocalTransfer.SendMoneyConclusion(svm));
            };
            Navigation.PushPopupAsync(pickerPop, true);

        }


        private void RecipientPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}