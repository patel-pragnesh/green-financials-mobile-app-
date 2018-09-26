using FormsControls.Base;
using Newtonsoft.Json.Linq;
using SterlingSwitch.Helper;
using SterlingSwitch.Pages.Onboarding.ForgotPin;
using SterlingSwitch.Pages.Onboarding.SecurityQuestion;
using SterlingSwitch.Pages.Onboarding.UserInformation;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.Templates;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Onboarding.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnProfiledLoginPage : SwitchMasterPage, IAnimationPage
    {
        public LoginInfo Customer { get; set; }
        public bool EmailValidity { get; set; }
        public bool PwdValidity { get; set; }
        public bool IsRememberPasswordChecked { get; set; }
        public bool EnableStatus { get; set; }
        public string LoginButtonText { get; set; }

        private string _enteredPassCode = string.Empty;
        private ICryptoService _crypto = DependencyService.Get<ICryptoService>();
        private static ApiRequest httpService = new ApiRequest();
        private string username;
        private string pass;

        public UnProfiledLoginPage()
        {

            InitializeComponent();
            BindingContext = this;
            LoginButton.Text = "Login";
            EnableStatus = true;
            LoginButton.IsEnabled = false;

            Customer = new LoginInfo();

        }

        private async Task LoginButton_Tapped(object sender, System.EventArgs e)
        {
            if (BusinessLogic.IsConnectionOK() == true)
            {
                Customer.UserID = txtUsername.Text.Trim();
                Customer.Password = txtPassword.Text;
                username = txtUsername.Text.Trim();
                pass = txtPassword.Text;
                string imei = GlobalStaticFields.DeviceIMEI(); // calls same method to get device info

                var btn = (Button)sender;
                try
                {
                    var response = "";
                    if (BusinessLogic.IsConnectionOK() == true)
                    {

                        try
                        {
                            btn.IsEnabled = false;
                            if (!string.IsNullOrEmpty(Customer.UserID.Trim()))
                            {
                                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                                Match match = regex.Match(Customer.UserID.Trim());
                                if (!match.Success)
                                {
                                    MessageDialog.Show("OOPS", "Sorry, The email address you entered is not incorrect. Please verify and try again.", DialogType.Error, "DISMISS", null);
                                    return;
                                }
                            }
                            else if (string.IsNullOrEmpty(Customer.UserID))
                            {
                                MessageDialog.Show("OOPS", "Sorry, Username (Email address) field is required", DialogType.Error, "DISMISS", null);
                                return;
                            }
                            if (string.IsNullOrEmpty(Customer.Password.Trim()))
                            {
                                MessageDialog.Show("OOPS", "Sorry, Password field is required", DialogType.Error, "DISMISS", null);
                                return;
                            }

                            string Username = Customer.UserID;
                            App.Current.Properties["username"] = Username;
                            EnableStatus = false;
                            if (!string.IsNullOrEmpty(Customer.UserID) && !string.IsNullOrEmpty(Customer.Password))
                            {
                                LoginButton.Text = "Validating Credentials...";
                                var encPassword = _crypto.Encrypt(Customer.Password);
                                var req = new LoginInfo()
                                {
                                    UserID = Customer.UserID,
                                    Password = encPassword
                                };
                                //  var loginResponse = await httpService.Post<dynamic>(req, "", URLConstants.SwitchApiBaseUrl, "Switch/Login2", "UnProfiledLoginPage");
                                var loginResponse = await httpService.Login(req, URLConstants.SwitchNewApiBaseUrl, "Token", "UnProfiledLoginPage");
                                if (loginResponse.IsSuccessStatusCode)
                                {
                                    response = await loginResponse.Content.ReadAsStringAsync();
                                }
                                if (!string.IsNullOrEmpty(response))
                                {
                                    //Get Token and save token lifespan
                                    await LoginHelper.LogUserDetails(response, Username);   // get user details in abstract class
                                    if (GlobalStaticFields.LoginTest == "Passed")
                                    {
                                        LoginButton.Text = "Login Success...";
                                        GlobalStaticFields.Customer.IsLoggedOn = true;
                                        if (IsRememberPasswordChecked == true)
                                        {
                                            if (!App.Current.Properties.ContainsKey("email"))
                                            {
                                                App.Current.Properties["email"] = Username;
                                            }
                                        }
                                        Application.Current.MainPage = new AnimationNavigationPage(new Dashboard.Dashboard());
                                    }
                                    else
                                    {
                                        EnableStatus = true;
                                        LoginButton.Text = "Login";
                                        MessageDialog.Show("OOPS", "Sorry, your login attempt failed. Kindly review your credentials and try again.", PopUps.DialogType.Error, "OK", null);
                                        return;
                                    }
                                }
                                else
                                {
                                    var msg = await loginResponse.Content.ReadAsStringAsync();
                                    var jsResponse = JObject.Parse(msg);
                                    var err = jsResponse.Value<string>("error");
                                    var errormsg = jsResponse.Value<string>("error_description");
                                    EnableStatus = true;
                                    LoginButton.Text = "Login";
                                    if (err == "02" || err == "03")
                                    {
                                        //option to register device
                                        MessageDialog.Show("OOPS", $"{errormsg}{Environment.NewLine} Do you want to register this device?", PopUps.DialogType.Error, "Yes", new Action(()=> RegisterDevice(err)), "No",null);
                                    }
                                    else
                                    {
                                        MessageDialog.Show("OOPS", errormsg, PopUps.DialogType.Error, "OK", null);

                                    }

                                    return;
                                }
                            }
                            else
                            {
                                MessageDialog.Show("OOPS", "Username and Pin fields are required", PopUps.DialogType.Error, "OK", null);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            string log = ex.Message;
                            EnableStatus = true;
                            LoginButton.Text = "Login";
                            await BusinessLogic.Log(ex.ToString(), "Exception on Login Attempt", URLConstants.SwitchApiLiveBaseUrl + "Login2", $"Username: {txtUsername.Text} Password {txtPassword.Text}", response, "UnProfiledLoginPage");
                            MessageDialog.Show("OOPS", "Sorry, an error occurred, please try again", PopUps.DialogType.Error, "OK", null);

                        }
                    }
                    else
                    {
                        MessageDialog.Show("OOPS", "Sorry, it appears you do not have internet connectivity on your device. Kindly reconnect and try again", DialogType.Error, "DISMISS", null);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageDialog.Show("OOPS", "An error occurred please try again", PopUps.DialogType.Error, "OK", null);

                }
                finally
                {
                    btn.IsEnabled = true;
                }
            }
        }

        private void RegisterDevice(string code)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                //option to register device
                if (code == "02")
                {                    
                    Navigation.PushAsync(new SecurityAnswerPage(username, pass));
                }
                else
                {
                    //03, create security question
                    Navigation.PushAsync(new CreateSecurityQuestion(username, pass));

                }
            });
        }

        private bool CheckEmailValid()
        {
            bool isValid = IsValidEmail(txtUsername.Text);
            var state = isValid ? "Valid" : "Invalid";
            VisualStateManager.GoToState(txtUsername, state);
            return isValid;
        }

        private bool IsValidEmail(string userID)
        {
            try
            {
                //var addr = new System.Net.Mail.MailAddress(userID);
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(userID.Trim());
                if (match.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                // return addr.Address == userID;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void btnJoinSwitch_Tapped(object sender, EventArgs e)
        {
            // navigate to onboarding process
            Navigation.PushAsync(new UserInformationPage());
            //Navigation.PushAsync(new AccountPinCreation());
        }

        private async void btnForgotPin_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPasswordPage());
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            EmailValidity = CheckEmailValid();
            CheckIfToEnableLogInButton();

        }

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {

            PwdValidity = CheckPasswordValid();
            CheckIfToEnableLogInButton();

            //Not clean but gets the job done for now
            if (e.NewTextValue.Length == ((Entry)sender).MaxLength)
            {

                if (((Entry)sender).Text != "cont" && ((Entry)sender).Text != _enteredPassCode)
                {
                    _enteredPassCode = e.NewTextValue;
                    RemoveFocus((Entry)sender);
                    ((Entry)sender).Text = "cont";
                }


                if (((Entry)sender).Text == "cont")
                {
                    ((Entry)sender).Text = _enteredPassCode.ToString();
                }

            }
        }

        private void RemoveFocus(Entry entry)
        {
            entry.Unfocus();
        }

        private void CheckIfToEnableLogInButton()
        {
            if (EmailValidity && PwdValidity)
            {
                //enable
                LoginButton.IsEnabled = true;
                //txtUsername.TextColor = Color.Black;
                //txtPassword.TextColor = Color.Black;
            }

            else
            {
                //disable
                LoginButton.IsEnabled = false;
            }
        }

        private bool CheckPasswordValid()
        {
            var pwdlength = (txtPassword.Text ?? "").Length;
            bool IsValid = pwdlength == 4;
            //set visual state
            var state = IsValid ? "Valid" : "Invalid";
            VisualStateManager.GoToState(txtPassword, state);
            return IsValid;
        }


    }
}