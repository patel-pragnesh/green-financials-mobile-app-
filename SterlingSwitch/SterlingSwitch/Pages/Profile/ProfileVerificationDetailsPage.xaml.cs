using Plugin.Media;
using Rg.Plugins.Popup.Extensions;
using SterlingSwitch.Custom.Controls;
using SterlingSwitch.Pages.Profile.ViewModel;
using SterlingSwitch.PopUps;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Profile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfileVerificationDetailsPage : SwitchMasterPage
    {
        ProfileVerificationDetailsPageViewModel vm;
        public ProfileVerificationDetailsPage ()
		{
			InitializeComponent ();
            vm = new ProfileVerificationDetailsPageViewModel(Navigation);
            BindingContext = vm;
		}

       

        protected void ShowPickerForPhotoId(object sender, EventArgs e)
        {
            var pickerPop = new ExtendedPickerPopup();
            pickerPop.BindingContext = this;

            pickerPop.ItemsSource = new List<string> { "Passport", "Drivers license", "National ID card" };
            pickerPop.Title = "Photo ID";

            pickerPop.SelectedIndexChanged += PickerPop_SelectedIndexChanged;

            Navigation.PushPopupAsync(pickerPop, true);
        }

        protected void ShowPickerforAddressVerification(object sender, EventArgs e)
        {
            var pickerPop = new ExtendedPickerPopup();
            pickerPop.BindingContext = this;

            pickerPop.ItemsSource = new List<string> { "Utility Bill", "Rent Reciept", "Telephone Bill" };
            pickerPop.Title = "Address Verification";
            
            pickerPop.SelectedIndexChanged += PickerPop_SelectedIndexChanged;

            Navigation.PushPopupAsync(pickerPop, true);
        }

        protected async void ShowPickerForVideoSelfie(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                MessageDialog.Show("No Camera", " No camera available", DialogType.Info, "Ok", null);
                return;
            }

            var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
            {
                DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,
                DesiredLength = new TimeSpan(0,0,5)
                
            });

            if (file == null)
                return;

            var source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();

                file.Dispose();
                return stream;
            });
            vm.VideoIdStream = ConvertToMemoryStream(file);
        }

        private async void PickerPop_SelectedIndexChanged(object sender, PickerItems e)
        {
            //open camera

            var selectedItem = e.DisplayText;

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                MessageDialog.Show("No Camera", " No camera available", DialogType.Info, "Ok", null);
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {

                AllowCropping = true,
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear
            });

            if (file == null)
                return;


            var source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();

                file.Dispose();
                return stream;
            });

            //using (var ms = new MemoryStream())
            //{
            //    var imagestream = file.GetStream();
            //    imagestream.CopyTo(ms);
            //}

            switch (selectedItem)
            {               
                case "Utility Bill":
                    vm.DocumentStream = ConvertToMemoryStream(file);
                    await vm.UploadDocument("930", Models.DocumentType.UtilityBill);
                    break;
                case "Rent Reciept":
                    vm.DocumentStream = ConvertToMemoryStream(file);
                    await vm.UploadDocument("930", Models.DocumentType.RentReceipt);
                    break;
                case "Telephone Bill":
                    vm.DocumentStream = ConvertToMemoryStream(file);
                    await vm.UploadDocument("930",Models.DocumentType.TelephoneBill);
                    break;
                case "Passport":
                    vm.PhotoIdStream = ConvertToMemoryStream(file);
                    await vm.UploadPhotoID("929", Models.DocumentType.InternationalPassport);
                    break;
                case "Drivers license":
                    vm.PhotoIdStream = ConvertToMemoryStream(file);
                    await vm.UploadPhotoID("929", Models.DocumentType.DriversLicense);
                    break;
                case "National ID card":
                    vm.PhotoIdStream = ConvertToMemoryStream(file);
                    await vm.UploadPhotoID("929", Models.DocumentType.NationalIDCard);
                    break;
                default:
                    break;
            }
            
            //vm.ProfileImageSource = source;

        }

        MemoryStream ConvertToMemoryStream(Plugin.Media.Abstractions.MediaFile stream)
        {
            using (var ms = new MemoryStream())
            {
                var imagestream = stream.GetStream();
                imagestream.CopyTo(ms);

                return ms;
            }
        }

        protected async void Skip(object sender,EventArgs e)
        {
          await  Navigation.PopAsync();
        }
    }
}