using SterlingSwitch.Pages.LiveChat;
using SterlingSwitch.Pages.More.Device;
using SterlingSwitch.Pages.Onboarding.ChangePIN;
using SterlingSwitch.Pages.Onboarding.Login;
using SterlingSwitch.Pages.Onboarding.TPin;
using SterlingSwitch.Services;
using SterlingSwitch.Templates;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.More
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MorePage : SwitchMasterPage
    {
        private bool IsFirstLoad = false;
        public MorePage ()
		{
			InitializeComponent ();
           // SetTPIN();
		}


        protected override async void OnAppearing()
        {
            SetTPIN();
        }

        private void SetTPIN()
        {
            if (GlobalStaticFields.Customer.IsTPin)
            {
                IsTPIN.IsToggled = true;
                IsFirstLoad = true;
                GlobalStaticFields.resetPin = true;               
            }
            else
            {
                IsTPIN.IsToggled = false;
                IsFirstLoad = true;
                GlobalStaticFields.TPINToSet = true;
            }
        }
        private void ExtendedLabel_ItemTapped(object sender, Custom.Controls.ExtendedLabelTappedEvent e)
        {
            var text = e.Text;

            switch (text)
            {
                case "Your Profile":
                    Navigation.PushAsync(new Profile.ProfilePage());
                    break;
                case "Privacy Settings":
                    // case "Notifications":
                    break;
                case "Accounts & Cards":
                    Navigation.PushAsync(new Accounts());
                    break; 
                case "Change Pin":
                    Navigation.PushAsync(new ChangePINPage());
                    break;
                case "Live Chat":
                    Navigation.PushAsync(new LiveChatPage());
                    break;
                case "Contact Us":                    
                    break;
                case "Devices":
                    Navigation.PushAsync(new DeviceView());
                    break;
                case "FAQs":
                    Navigation.PushAsync(new FAQ.FrequenceyQuestion());
                    break;
                case "Terms and Conditions":
                    break;
                case "Privacy Policy":
                    break;
                case "About Switch":
                    break;
                case "Log out":
                    App.Current.MainPage = new NavigationPage(new UnProfiledLoginPage());
                    break;
                default:
                    break;
            }
             //  DisplayAlert("Item Tapped", text, "Ok");
        }

        private void ExtendedSwitch_OnToggled(object sender, bool e)
        {
          if(IsFirstLoad)
            {               
                IsFirstLoad = false;
                Navigation.PushAsync(new TransactionPinPage());
            } 
        }
    }
}