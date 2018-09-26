using SterlingSwitch.Models;
using SterlingSwitch.Pages.BankAccounts;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Pagelanding
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoryTagLanding : ContentPage
    {
        public List<string> MyList { get; set; }
        public ObservableCollection<Categories> CategoriesList { get; set; }
        public string TrnxTagID { get; set; }
        public CategoryTagLanding ()
		{
			InitializeComponent ();
            SetUp();
          
        }

        private void SetUp()
        {
            MyList = new List<string>()
            {
                "Bills","Transfers","Entertainment","Eating Out","Shopping","Groceries","Travel","Transportation","Charity","Education","Investment","Income"
            };
            CategoriesList = new ObservableCollection<Categories>()
            {
                new Categories{ImageSource="billssmall.png",Text="Bills",id=7},
                new Categories{ImageSource="transfers.png",Text="Transfers",id=9},
                new Categories{ImageSource="entertainment.png",Text="Entertainment"},
                new Categories{ImageSource="eatingOut.png",Text="Eating Out",id=3},
                new Categories{ImageSource="shopping.png",Text="Shopping",id=5},

                new Categories{ImageSource="travel.png",Text="Travel"},
                new Categories{ImageSource="groceries.png",Text="Groceries",id=1},
                new Categories{ImageSource="transportation.png",Text="Transportation",id=2},
                new Categories{ImageSource="give.png",Text="Charity",id=6},
                new Categories{ImageSource="education.png",Text="Education",id=8},
                new Categories{ImageSource="investment.png",Text="Investment"},
                new Categories{ImageSource="income.png",Text="Income"},
                new Categories{ImageSource="shopping.png",Text="Domestic Errands",id=4},
            };
            this.BindingContext = this;
        }

        public CategoryTagLanding(string transactionID)
        {
            InitializeComponent();
            SetUp();
            //already checked that this would be successful in previous page
            TrnxTagID = transactionID;
            
        }


        private  void GoToPaymentLanding()
        {
            Device.BeginInvokeOnMainThread(()=> {
                Navigation.PopModalAsync();
                Application.Current.MainPage.Navigation.PushAsync(new PaymentsLanding());

            });
        }
        private void GoToBankView()
        {
            Device.BeginInvokeOnMainThread(() => {
                Navigation.PopModalAsync();
                Application.Current.MainPage.Navigation.PushAsync(new BankAccountsView());

            });
        }


        private void GoToDashBoard()
        {
            Device.BeginInvokeOnMainThread(() => {
                Navigation.PopModalAsync();
                Application.Current.MainPage.Navigation.PushAsync(new Dashboard.Dashboard());

            });
        }
        private void FlowListView_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(TrnxTagID))
            {
                return;
            }
            var selectedItem = e.Item as Categories;
            var selectedCategoryTag = selectedItem.id;
            BusinessLogic.SaveTaggedTrnx(selectedCategoryTag, TrnxTagID);
            MessageDialog.Show("Info", $"You have tagged this transaction as {selectedItem.Text.ToString()}. ", DialogType.Success, "OK", NavigatePage, "", null);
        }

        public class Categories
        {
            public string ImageSource { get; set; }
            public string Text { get; set; }
            public int id { get; set; }
        }
        private async void Cancel(object sender, EventArgs e)
        {
           await Navigation.PopModalAsync();
            //get previous page and navigate based on previous page
            string p = GetPreviousPage();
            if (p.ToLower().Contains("detailedtransaction"))//coming fom statement page tagging
            {
                //do nothing. just pop off the modal new BankAccountsView()
                await Application.Current.MainPage.Navigation.PushAsync(new BankAccountsView());

            }
            else if (p.ToLower().Contains("sendmoney"))//coming from  funds transfer tagging
            {
                await Application.Current.MainPage.Navigation.PushAsync(new PaymentsLanding());

            }
            else
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Dashboard.Dashboard());

            }
        }

        private string GetPreviousPage()
        {
            var p = Application.Current.MainPage.Navigation.NavigationStack.Last().ToString();
            return p;
        }

        private async void NavigatePage()
        {
            string p = GetPreviousPage();
            if (p.ToLower().Contains("detailedtransaction"))//coming fom statement page tagging
            {
                //do nothing. just pop off the modal new BankAccountsView()
                GoToBankView();
            }
            else if (p.ToLower().Contains("sendmoney"))//coming from  funds transfer tagging
            {
                GoToPaymentLanding();
            }
            else
            {
                GoToDashBoard();
            }
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}