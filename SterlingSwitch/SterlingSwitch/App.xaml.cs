using FormsControls.Base;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using Plugin.Connectivity;
using SterlingSwitch.Pages.BankAccounts;
using SterlingSwitch.Pages.Dashboard;
using SterlingSwitch.Pages.Onboarding.Login;
using SterlingSwitch.Pages.QuickActions;
using SterlingSwitch.Services;
using System;
using System.Threading.Tasks;
using SterlingSwitch.Pages.Investments;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SterlingSwitch.Pages.Beneficiary;
using SterlingSwitch.Pages.CardlessWithdrawals;
using SterlingSwitch.Pages.Onboarding.OtpAndPinVerification;
using SterlingSwitch.Pages.Onboarding.SecurityQuestion;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SterlingSwitch
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
           InitializeAppStart();
         // MainPage = new  AnimationNavigationPage(new CreateSecurityQuestion("CHUKWUMAPRINCE08@GMAIL.COM", "1lIOaIGdOrc="));
        }

        protected override async void OnStart()
        {
          
            //var installId = await Microsoft.AppCenter.AppCenter.GetInstallIdAsync();
            //var id = installId?.ToString();
            // Handle when your app starts
            GlobalStaticFields._getDevice();
            Microsoft.AppCenter.AppCenter.Start("ios=c6a76a6d-f8f1-4555-85d1-62a545b651bc;android=026e3b37-e951-4347-8196-ba957754bd5b;uwp=44c96eca-062b-47a3-9626-b4e8524561e5", typeof(Analytics), typeof(Crashes), typeof(Push));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            App.Current.Properties["sleeper"] = DateTime.UtcNow;
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            GlobalStaticFields.SetSleepTimer();
            if (IsTokenExpired())
            {
                MainPage = new AnimationNavigationPage(new UnProfiledLoginPage());
                return;
            }
            //LogUserOut();
        }

        public bool CheckConnection()
        {
            if (CrossConnectivity.Current.IsConnected)
                return true;
            else
                return false;
        }

        public void InitializeAppStart()
        {

            GlobalStaticFields.Customer = new Services.Repository.Customer();
            Task.Run(async () => await GlobalStaticFields._setLifeStyle());
            //  Task.Run(() => GlobalStaticFields.GetToken());
            Task.Run(GlobalStaticFields.GetLocationFromPlugin);
            StoreUniqueID();
            if (IsTokenExpired())
            {
                //MainPage = new AnimationNavigationPage(new UnProfiledLoginPage());
                MainPage = new NavigationPage(new UnProfiledLoginPage());
                //MainPage = new NavigationPage(new Pages.BankAccounts.Views.TransactionDetails());

            }
            else
            {
                 MainPage = new AnimationNavigationPage(new UnProfiledLoginPage());
                //MainPage = new NavigationPage(new Dashboard());
                // MainPage = new NavigationPage(new Pages.AllTransactions.DetailedTransactionView());
            }

        }
        public void StartApp()
        {
            GlobalStaticFields._getDevice();
            GlobalStaticFields.getBalanceStatus();
           


        }

        public async void StoreUniqueID()
        {
           await GlobalStaticFields.StoreUniqueID();
        }
       

        public void StartRegistrationFlow()
        {
            // perform code for registration flow
        }

        private async void LogUserOut()
        {
            if (MainPage.Navigation.NavigationStack.Count > 0)
            {
                GlobalStaticFields.Customer.IsLoggedOn = false;
                Current.MainPage = new UnProfiledLoginPage();
                // await MainPage.Navigation.PopToRootAsync();
            }
            await GlobalStaticFields._getDevice();
           
        }

        public static Func<Task<bool?>> HardwareBackPressed
        {
            private get;
            set;
        }

        public static async Task<bool?> CallHardwareBackPressed()
        {
            Func<Task<bool?>> backPressed = HardwareBackPressed;
            if (backPressed != null)
            {
                return await backPressed();
            }

            return true;
        }
        public enum TransactionType
        {
            OneTimePayment,
            FuturePayment,
            StandingOrderPayment,
        }

        bool IsTokenExpired()
        {
            var tokenExpires = GlobalStaticFields.TokenExpired;
            var tokenIssued = GlobalStaticFields.TokenIssued;

            DateTime expiresDate;
            DateTime issuedDate;

            if (!DateTime.TryParse(tokenExpires, out expiresDate))
                return true;

            if (!DateTime.TryParse(tokenIssued, out issuedDate))
                return true;

            var diff = DateTime.Compare(expiresDate, issuedDate);

            return diff < 0;

        }
        void ResetImageDynamicResource()
        {

        }
        private void Home_Tapped(object sender, EventArgs e)
        {
            Current.MainPage = new NavigationPage(new Dashboard());
        }

        private void MyBank_Tapped(object sender, EventArgs e)
        {
            Current.MainPage = new NavigationPage(new BankAccountsView());
        }

        private void QuickAction_Tapped(object sender, EventArgs e)
        {
            Current.MainPage = new NavigationPage(new QuickActionsV2());
           // App.Current.MainPage.Navigation.PushModalAsync(new QuickActions(), true);
        }

        private void PaymentOndashboard_Tapped(object sender, EventArgs e)
        {
            Current.MainPage = new NavigationPage(new Pages.Pagelanding.PaymentsLanding());

        }

        private void More_Tapped(object sender, EventArgs e)
        {
            Current.MainPage = new NavigationPage(new Pages.More.MorePage());
        }
        private async void GoBack(object sender, EventArgs e)
        {
            await MainPage.Navigation.PopAsync();
        }
    }
}
