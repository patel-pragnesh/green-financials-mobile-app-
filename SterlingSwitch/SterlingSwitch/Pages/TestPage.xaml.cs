using SterlingSwitch.Helper;
using SterlingSwitch.PopUps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TestPage : ContentPage
	{
		public TestPage ()
		{
			InitializeComponent ();
            //List<string> list = new List<string>() {"Lagos", "Abuja", "Port Harcourt", "Sokoto" };
            //lblContent.Text = Utilities.GetCurrency("USD", Models.CurrencyReturnType.CurrencyName);
		}

        private void TestPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (TestPicker.SelectedItem == null)
            //    return;

            //MessageDialog.Show("Test Result", $"SelectedItem: {TestPicker.SelectedItem}, SelectedIndex: {TestPicker.SelectedIndex}", DialogType.Info, "OK", null);
        }

        private void TestEntry_Completed(object sender, EventArgs e)
        {
            Debug.WriteLine("=================");
            Debug.WriteLine("Text Completed Event Fired");
            Debug.WriteLine("=================");
        }

        private void TestEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Debug.WriteLine("=================");
            Debug.WriteLine($"Old: {e.OldTextValue}, New: {e.NewTextValue}");
            Debug.WriteLine("=================");
        }

        private async void ShowProgress_Clicked(object sender, EventArgs e)
        {
            var pd = await ProgressDialog.Show("Setting things up, please wait.");

            await Task.Delay(5000).ConfigureAwait(false);

            await pd.Dismiss();
        }
        
        private async void ShowProgressWithEnding_Clicked(object sender, EventArgs e)
        {
            var pd = await ProgressDialog.Show("Setting things up, please wait.");

            await Task.Delay(5000).ConfigureAwait(false);

            await pd.Dismiss("All is now set.");
        }

        private async void ShowProgressWithMessage_Clicked(object sender, EventArgs e)
        {
            var pd = await ProgressDialog.Show("Setting things up, please wait.");

            await Task.Delay(5000).ConfigureAwait(false);

            pd.Message = "Almost done, thank you for your patience";

            await Task.Delay(5000).ConfigureAwait(false);

            await pd.Dismiss("All is now set.");
        }

        private void ShowContent_Clicked(object sender, EventArgs e)
        {
            MessageDialog.Show("Transfer Successful", "You have successfully sent the sum of N200,000 to Chukwuma Precious." + Environment.NewLine + "Do you want to save as beneficiary?", DialogType.Info, "Yes", DoSomething, "No", () => { Navigation.PushAsync(new TestPage()); });
        }

        private void DoSomething()
        {

        }
    }
}