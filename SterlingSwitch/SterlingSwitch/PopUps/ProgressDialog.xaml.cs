using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.PopUps
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProgressDialog : PopupPage
    {
		public ProgressDialog (string message)
		{
			InitializeComponent ();
            this.Message = message;
            CloseWhenBackgroundIsClicked = false;
		}

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        public static BindableProperty MessageProperty = BindableProperty.Create(
                                                            propertyName: "MessageProperty",
                                                            returnType: typeof(string),
                                                            declaringType: typeof(ProgressDialog),
                                                            defaultValue: string.Empty,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: MessagePropertyChanged);

        private static void MessagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ProgressDialog)bindable;

            if (control != null)
            {
                control.Message = (string)newValue;
                //control.HeaderTxt.Text = control.Message;
            }
        }

        public async Task Dismiss(string doneMsg)
        {
            DialogImg.Source = "End.gif";
            Message = doneMsg;
            Device.BeginInvokeOnMainThread(() => TitleTxt.Text = "DONE");
            await Task.Delay(2500);
            await Dismiss();
        }
        
        public async Task Dismiss()
        {
            await Navigation.PopPopupAsync();
        }
        
        public string Message
        {
            get => MessageTxt.Text;
            set => Device.BeginInvokeOnMainThread(() => MessageTxt.Text = value);
        }

        public async static Task<ProgressDialog> Show(string message)
        {
            ProgressDialog progressDialog = new ProgressDialog(message);
            await App.Current.MainPage.Navigation.PushPopupAsync(progressDialog, true);
            return progressDialog;
        }
    }
}