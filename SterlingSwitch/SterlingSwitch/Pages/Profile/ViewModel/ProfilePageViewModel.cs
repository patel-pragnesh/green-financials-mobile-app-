using SterlingSwitch.Extensions;
using SterlingSwitch.Helper;
using SterlingSwitch.Models;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.ViewModelBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.Profile.ViewModel
{
   public class ProfilePageViewModel:BaseViewModel
    {
        public ProfilePageViewModel(INavigation navigation):base(navigation)
        {

        }

        #region Properties
        private ImageSource profileImageSource = ImageSource.FromUri(new Uri($"{URLConstants.SwitchUrl}upload/{GlobalStaticFields.Customer.UserID}.jpg"));

        public ImageSource ProfileImageSource
        {
            get { return profileImageSource; }
            set { SetProperty(ref profileImageSource, value); }
        }

        private MemoryStream imageStream = default(MemoryStream);

        public MemoryStream ImageStream
        {
            get { return imageStream; }
            set { SetProperty(ref imageStream, value); }
        }



        private bool isSaving = default(bool);
        public bool IsSaving
        {
            get { return isSaving; }
            set { SetProperty(ref isSaving, value); }
        }


        private bool canUpload = true;

        public bool CanUpload
        {
            get { return canUpload; }
            set { SetProperty(ref canUpload, value); }
        }



        #endregion


        #region Events
        public async Task SaveImage()
        {
            try
            {
                CheckConnectivity();
               
                IsSaving = true;
                CanUpload = false;

                // var profileimage = new ProfileImageModel
                //{
                //    ImageByte = Convert.ToBase64String(ImageStream.ToArray()),
                //    NUBAN = GlobalStaticFields.Customer.ListOfAllAccounts.FirstOrDefault().nuban,
                //    ReferenceID = Utilities.GenerateReferenceId(),
                //    RequestType = "928"
                //};
                var profileimage = new ProfileImageModel
                {
                   Base64Image = Convert.ToBase64String(ImageStream.ToArray()),
                   Email = GlobalStaticFields.Customer.Email,
                   FileType = "jpg"
                };
                var apirequest = new ApiRequest();
                string msg = "";
                string content = "";
                var request = await apirequest.Post<ProfileImageModel>(profileimage, "", URLConstants.SwitchApiLiveBaseUrl, "Switch/SaveImage", "ProfilePageViewModel");
                if(request.IsSuccessStatusCode)
                {
                     content = await request.Content.ReadAsStringAsync();     
                    ProfileImageSource= ImageSource.FromUri(new Uri($"{URLConstants.SwitchUrl}upload/{GlobalStaticFields.Customer.UserID}.jpg"));
                }
                else
                {
                     content = await request.Content.ReadAsStringAsync();
                }
                if (!string.IsNullOrWhiteSpace(content))
                {
                    content = content.JsonCleanUp();
                  
                    Utilities.ShowToast(content);
                }
                    
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsSaving = false;
                CanUpload = true;
            }
        }
        #endregion






    }
}
