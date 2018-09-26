using SterlingSwitch.Custom.Controls;
using SterlingSwitch.Helper;
using SterlingSwitch.PopUps;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Onboarding.Login
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : SwitchMasterPage
    {
		public LoginPage ()
		{
			InitializeComponent ();
		}
        ICryptoService _crypto = DependencyService.Get<ICryptoService>();
        private void InputClicked(object sender, object e)
        {
            var input = (PinItemView)sender;
            GetInput((string)input.CommandParameter);
        }
        public string Pin = string.Empty;
        private void GetInput(string value)
        {
            if (Pin.Length < 4)
            {
                Pin += value;
                UpdateDots(Pin.Length); 
                if(Pin.Length == 4)
                {
                    // trigger login 
                    MessageDialog.Show("SUCCESS", "Login you In.........", PopUps.DialogType.Success, "OK", null);
                }
            }
        }

        private void BackSpaceClicked(object sender, object e)
        {
            BackSpace();
        }
        private void BackSpace()
        {
            if (!string.IsNullOrEmpty(Pin))
            {
                Pin = Pin.Remove(Pin.Length - 1);
                UpdateDots(Pin.Length);
            }
        }

        private void DoLogin()
        {
            var encPassword = _crypto.Encrypt(Pin);
        }
        private void UpdateDots(int length)
        {
            Color fillColor = Color.Black;
            Color noColor = Color.Transparent;
            switch (length)
            {
                case 0:
                    CellOne.Source = "Circle.png";
                    CellTwo.Source = "Circle.png";
                    CellThree.Source = "Circle.png";
                    CellFour.Source = "Circle.png";
                    break;
                case 1:
                    CellOne.Source = "FilledCircle.png";
                    CellTwo.BackgroundColor = noColor;
                    CellThree.BackgroundColor = noColor;
                    CellFour.BackgroundColor = noColor;
                    break;
                case 2:
                    CellOne.Source = "FilledCircle.png";
                    CellTwo.Source = "FilledCircle.png";
                    CellThree.BackgroundColor = noColor;
                    CellFour.BackgroundColor = noColor;
                    break;
                case 3:
                    CellOne.Source = "FilledCircle.png";
                    CellTwo.Source = "FilledCircle.png";
                    CellThree.Source = "FilledCircle.png";
                    CellFour.BackgroundColor = noColor;
                    break;
                case 4:
                    CellOne.Source = "FilledCircle.png";
                    CellTwo.Source = "FilledCircle.png";
                    CellThree.Source = "FilledCircle.png";
                    CellFour.Source = "FilledCircle.png";
                    break;
            }
        }
    }
}