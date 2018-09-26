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
	public partial class MessageDialog : PopupPage
    {
		public MessageDialog (string title, string message, DialogType dialogType, string OkText, Action OkAction = null, string CancelText = null,  Action CancelAction = null)
		{
			InitializeComponent ();

            CloseWhenBackgroundIsClicked = false;

            TitleTxt.Text = title.ToUpper();
            MessageTxt.Text = message;

            YesBtn.Text = string.IsNullOrWhiteSpace(OkText)? "OK" : OkText;

            YesBtn.Clicked += async(a, b) =>
            {
                await Navigation.PopPopupAsync().ConfigureAwait(false);
                OkAction?.Invoke();
            };

            NoBtn.IsVisible = !string.IsNullOrWhiteSpace(CancelText) ? true : false;

            //if(!NoBtn.IsVisible)
            //{
            //    YesBtn.Margin = new Thickness(0, 0, 0, -30);
            //}

            NoBtn.Clicked += async(a, b) =>
            {
                await Navigation.PopPopupAsync().ConfigureAwait(false);
                CancelAction?.Invoke();
            };


            switch (dialogType)
            {
                case DialogType.Success:
                    icon.Source = "success.gif";
                    break;
                case DialogType.Info:
                    icon.Source = "info.png";
                    break;
                case DialogType.Error:
                    icon.Source = "error.png";
                    break;
                case DialogType.Question:
                    icon.Source = "question.png";
                    break;
                default:
                    break;
            }

        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        public static void Show(string title, string message, DialogType dialogType, string OkText, Action OkAction)
        {
            MessageDialog md = new MessageDialog(title, message, dialogType, OkText, OkAction, null, null);
            App.Current.MainPage.Navigation.PushPopupAsync(md, true);
        }

        public static void  Show(string title, string message, DialogType dialogType, string OkText, Action OkAction, string CancelText, Action CancelAction = null)
        {
            MessageDialog md = new MessageDialog(title, message, dialogType, OkText, OkAction, CancelText, CancelAction);
            App.Current.MainPage.Navigation.PushPopupAsync(md, true);
        }
    }

    public enum DialogType
    {
        Success, Error, Question, Info 
    }
}