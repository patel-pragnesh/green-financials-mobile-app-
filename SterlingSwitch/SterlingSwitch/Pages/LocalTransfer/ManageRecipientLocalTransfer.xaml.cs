using SterlingSwitch.Extensions;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
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
	public partial class ManageRecipientLocalTransfer : SwitchMasterPage
    {
        SendMoneyViewModel vm;
        public ManageRecipientLocalTransfer ()
		{
			InitializeComponent ();
            this.BindingContext = vm = new SendMoneyViewModel();
            vm.ListOfSavedBeneficiaries = GlobalStaticFields.ListOfTransferbeneficiaries;
            RecipientListView.ItemsSource = vm.ListOfSavedBeneficiaries;



        }

        private void SearchRecipient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(SearchRecipient.Text))
                {
                    var oldV = e.OldTextValue;
                    var newV = e.NewTextValue;
                    if (newV.Length>oldV.Length)
                    {
                        var keyword = SearchRecipient.Text.ToLower().ToString();
                        var searchedList = vm.ListOfSavedBeneficiaries.Where(d => d.display.ToLower().Contains(keyword)).ToList();
                        var Conv = Extensions.ObservableExtensions.ToObservableCollection<GetSavedBeneficiaries>(searchedList);
                        vm.ListOfSavedBeneficiaries = Conv;
                        RecipientListView.ItemsSource = vm.ListOfSavedBeneficiaries;
                    }
                    else if(oldV.Length>newV.Length)
                    {
                        var keyword = SearchRecipient.Text.ToLower().ToString();
                        //get the ooriginal list 
                        var recentSearch = GlobalStaticFields.ListOfTransferbeneficiaries;
                        //search original list with present text
                        var searchedList = recentSearch.Where(d => d.display.ToLower().Contains(keyword)).ToList();
                        //convert list to observable collection
                        var Conv = Extensions.ObservableExtensions.ToObservableCollection<GetSavedBeneficiaries>(searchedList);
                        //bind observable collectioon to control itemssource
                        vm.ListOfSavedBeneficiaries = Conv;
                        RecipientListView.ItemsSource = vm.ListOfSavedBeneficiaries;


                    }
                    else if (string.IsNullOrEmpty(SearchRecipient.Text))
                    {
                        //bind everything back to the view
                        RecipientListView.ItemsSource = vm.ListOfSavedBeneficiaries;

                    }

                }
            }
            catch (Exception ex)
            {
                var log = ex;
                
            }

        }

  



        private void RecipientListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                //check for null first
                if ((e.Item == null))
                {
                    return;
                }
                //then cast it as a proper object
                var Recipient = e?.Item as GetSavedBeneficiaries;


                vm.ShowOrHideManageButton(Recipient);
            }
            catch (Exception ex)
            {

                var log = ex;
            }
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            if (RecipientListView.SelectedItem==null)
            {

                return;
            }
            var selected = RecipientListView.SelectedItem as GetSavedBeneficiaries;
            //now call delete beneficiary function
            var response =await BusinessLogic.DeleteTransferBeneficiary(selected);
            GlobalStaticFields.ListOfTransferbeneficiaries.Remove(selected);
            RecipientListView.ItemsSource = vm.ListOfSavedBeneficiaries = GlobalStaticFields.ListOfTransferbeneficiaries;
            MessageDialog.Show("Success", "That action was tentatively successful.", DialogType.Success, "OK", null, "", null);
        }
    }
}