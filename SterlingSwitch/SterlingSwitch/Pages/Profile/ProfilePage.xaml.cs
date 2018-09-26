using FormsControls.Base;
using Plugin.Media;
using SterlingSwitch.Custom.Controls;
using SterlingSwitch.Pages.Profile.ViewModel;
using SterlingSwitch.PlatformSpecs;
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
    public partial class ProfilePage : SwitchMasterPage,IAnimationPage
    {
        ProfilePageViewModel vm;
        public ProfilePage()
        {
            InitializeComponent();
            vm = new ProfilePageViewModel(Navigation);
            BindingContext = vm;
        }

 

        private void ExtendedLabel_ItemTapped(object sender, Custom.Controls.ExtendedLabelTappedEvent e)
        {

            var obj = (ExtendedLabel)sender;
            switch (obj.Text)
            {
                case "PERSONAL DETAILS":
                    Navigation.PushAsync(new PersonalDetailsPage());
                    break;
                case "RESIDENTIAL DETAILS":
                    Navigation.PushAsync(new AddressDetailsPage());
                    //Navigation.PushAsync(new ProfileVerificationDetailsPage());
                    break;
                default:
                    break;
            }

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

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
                DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front
            });

            if (file == null)
                return;

            
           var source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                
                file.Dispose();
                return stream;
            });

            using (var ms = new MemoryStream())
            {
                var imagestream = file.GetStream();
                imagestream.CopyTo(ms);
                vm.ImageStream = ms;
            }
           // vm.ImageStream = (MemoryStream)file.GetStream(); 
            await vm.SaveImage();
            vm.ProfileImageSource = source;
        }

       
    }
}