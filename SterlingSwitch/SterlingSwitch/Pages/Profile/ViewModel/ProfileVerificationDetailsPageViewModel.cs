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
using Xamarin.Forms;

namespace SterlingSwitch.Pages.Profile.ViewModel
{
    public class ProfileVerificationDetailsPageViewModel : BaseViewModel
    {
        public ProfileVerificationDetailsPageViewModel(INavigation navigation) : base(navigation)
        {
            PhotoIdButtonText = "Take Picture";
            DocumentButtonText = "Take Picture";
        }
        #region Properties
        private ImageSource profileImageSource = ImageSource.FromUri(new Uri($"{URLConstants.SwitchUrl}upload/{GlobalStaticFields.Customer.UserID}.jpg"));

        public ImageSource ProfileImageSource
        {
            get { return profileImageSource; }
            set { SetProperty(ref profileImageSource, value); }

        }


        private MemoryStream documentStream = default(MemoryStream);
        public MemoryStream DocumentStream
        {
            get { return documentStream; }
            set { SetProperty(ref documentStream, value); }
        }

        private MemoryStream photoIdStream = default(MemoryStream);
        public MemoryStream PhotoIdStream
        {
            get { return photoIdStream; }
            set { SetProperty(ref photoIdStream, value); }
        }

        private MemoryStream videoStream = default(MemoryStream);
        public MemoryStream VideoIdStream
        {
            get { return videoStream; }
            set { SetProperty(ref videoStream, value); }
        }

        private bool isSaving = default(bool);
        public bool IsSaving
        {
            get { return isSaving; }
            set { SetProperty(ref isSaving, value); }
        }


        private bool isUploadingPhotoId = false;
        public bool IsUploadingPhotoId
        {
            get { return isUploadingPhotoId; }
            set { SetProperty(ref isUploadingPhotoId, value); }
        }
        private bool isAddressVerifying = false;
        public bool IsAddressVerifying
        {
            get { return isAddressVerifying; }
            set { SetProperty(ref isAddressVerifying, value); }
        }

        private bool canUploadDocument =true;

        public bool CanUploadDocument
        {
            get { return canUploadDocument; }
            set { SetProperty(ref canUploadDocument, value); }
        }

        private bool canUploadPhoto = true;

        public bool CanUploadPhoto
        {
            get { return canUploadPhoto; }
            set { SetProperty(ref canUploadPhoto, value); }
        }


        private Color photoIDUploadedColor = Color.FromRgb(216, 216, 216);

        public Color PhotoIDUploadedColor
        {
            get { return photoIDUploadedColor; }
            set { SetProperty(ref photoIDUploadedColor, value); }
        }

        private Color documentUploadedColor = Color.FromRgb(216, 216, 216);

        public Color DocumentUploadedColor
        {
            get { return documentUploadedColor; }
            set { SetProperty(ref documentUploadedColor, value); }
        }


        private string photoIdButtonText = "";
        public string PhotoIdButtonText
        {
            get { return photoIdButtonText; }
            set { SetProperty(ref photoIdButtonText, value); }
        }

        private string documentButtonText = "";
        public string DocumentButtonText
        {
            get { return documentButtonText; }
            set { SetProperty(ref documentButtonText, value); }
        }


        #endregion

        #region Events
        public async Task SaveUpload(string requestType, DocumentType documentType)
        {
            try
            {
                CheckConnectivity();
                string imageByte = "";
                switch (documentType)
                {
                    case DocumentType.InternationalPassport:
                    case DocumentType.DriversLicense:
                    case DocumentType.NationalIDCard:
                        imageByte = Convert.ToBase64String(PhotoIdStream.ToArray());
                        IsUploadingPhotoId = true;
                        break;
                    case DocumentType.UtilityBill:
                    case DocumentType.RentReceipt:
                    case DocumentType.TelephoneBill:
                        imageByte = Convert.ToBase64String(DocumentStream.ToArray());
                        IsAddressVerifying = true;
                        break;
                    default:
                        break;
                }



                var profileimage = new DocumentUploadModel
                {
                    ImageByte = imageByte,
                    NUBAN = GlobalStaticFields.Customer.ListOfAllAccounts?.FirstOrDefault()?.nuban,
                    FileType = "jpg",
                    ReferenceID = Utilities.GenerateReferenceId(),
                    RequestType = requestType,
                    DocumentType = documentType
                };
                var apirequest = new ApiRequest();
                string msg = "";
                string content = "";
                var request = await apirequest.Post<DocumentUploadModel>(profileimage, "", URLConstants.SwitchApiLiveBaseUrl, "Switch/UploadMandate", "ProfileVerificationDetailsPageViewModel");
                if (request.IsSuccessStatusCode)
                {
                    content = await request.Content.ReadAsStringAsync();
                    switch (documentType)
                    {
                        case DocumentType.InternationalPassport:
                        case DocumentType.DriversLicense:
                        case DocumentType.NationalIDCard:
                            // IsUploadingPhotoId = true;
                            break;
                        case DocumentType.UtilityBill:
                        case DocumentType.RentReceipt:
                        case DocumentType.TelephoneBill:
                            //  IsAddressVerifying = true;
                            break;
                        default:
                            break;
                    }
                    // ProfileImageSource = ImageSource.FromUri(new Uri($"{URLConstants.SwitchUrl}upload/{GlobalStaticFields.Customer.UserID}.jpg"));
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
                switch (documentType)
                {
                    case DocumentType.InternationalPassport:
                    case DocumentType.DriversLicense:
                    case DocumentType.NationalIDCard:
                        IsUploadingPhotoId = false;
                        break;
                    case DocumentType.UtilityBill:
                    case DocumentType.RentReceipt:
                    case DocumentType.TelephoneBill:
                        IsAddressVerifying = false;
                        break;
                    default:
                        break;
                }
            }
        }

        public async Task UploadPhotoID(string requestType, DocumentType documentType)
        {
            try
            {
                CheckConnectivity();
                string imageByte = "";
                imageByte = Convert.ToBase64String(PhotoIdStream.ToArray());
                IsUploadingPhotoId = true;
                CanUploadPhoto = false;


                var profileimage = new DocumentUploadModel
                {
                    ImageByte = imageByte,
                    NUBAN = GlobalStaticFields.Customer.ListOfAllAccounts?.FirstOrDefault()?.nuban,
                    FileType = "jpg",
                    ReferenceID = Utilities.GenerateReferenceId(),
                    RequestType = requestType,
                    DocumentType = documentType
                };
                var apirequest = new ApiRequest();
                string msg = "";
                string content = "";
                var request = await apirequest.Post<DocumentUploadModel>(profileimage, "", URLConstants.SwitchApiLiveBaseUrl, "Switch/UploadMandate", "ProfileVerificationDetailsPageViewModel");
                if (request.IsSuccessStatusCode)
                {
                    content = await request.Content.ReadAsStringAsync();
                    PhotoIDUploadedColor = Color.FromRgb(90 ,200 ,250);
                    PhotoIdButtonText = "Retake";
                    // ProfileImageSource = ImageSource.FromUri(new Uri($"{URLConstants.SwitchUrl}upload/{GlobalStaticFields.Customer.UserID}.jpg"));
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
                IsUploadingPhotoId = false;
                CanUploadPhoto = true;
            }
        }
        
        public async Task UploadDocument(string requestType, DocumentType documentType)
        {
            try
            {
                CheckConnectivity();
                string imageByte = "";
                imageByte = Convert.ToBase64String(DocumentStream.ToArray());
                IsAddressVerifying = true;
                CanUploadDocument = false;


                var profileimage = new DocumentUploadModel
                {
                    ImageByte = imageByte,
                    NUBAN = GlobalStaticFields.Customer.ListOfAllAccounts?.FirstOrDefault()?.nuban,
                    FileType = "jpg",
                    ReferenceID = Utilities.GenerateReferenceId(),
                    RequestType = requestType,
                    DocumentType = documentType
                };
                var apirequest = new ApiRequest();
                string msg = "";
                string content = "";
                var request = await apirequest.Post<DocumentUploadModel>(profileimage, "", URLConstants.SwitchApiLiveBaseUrl, "Switch/UploadMandate", "ProfileVerificationDetailsPageViewModel");
                if (request.IsSuccessStatusCode)
                {
                    content = await request.Content.ReadAsStringAsync();
                    DocumentUploadedColor = Color.FromRgb(90, 200, 250);
                    DocumentButtonText = "Retake";
                    // ProfileImageSource = ImageSource.FromUri(new Uri($"{URLConstants.SwitchUrl}upload/{GlobalStaticFields.Customer.UserID}.jpg"));
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
                IsAddressVerifying = false;
                CanUploadDocument = true;
            }
        }
        #endregion
    }
}
