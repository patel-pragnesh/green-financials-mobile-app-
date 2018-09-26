using Newtonsoft.Json.Linq;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.ViewModelBase;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.More.Device
{
    public class DeviceViewModel : BaseViewModel
    {
        public DeviceViewModel(INavigation navigation) : base(navigation)
        {
            Devices = new ObservableCollection<MobileDevice>();
        }

        #region Properties

        public ObservableCollection<MobileDevice> Devices { get; set; }

        private bool isDevicesLoaded = false;

        public bool IsDevicesLoaded
        {
            get => isDevicesLoaded;
            set => SetProperty(ref isDevicesLoaded, value);
        }



        #endregion

        #region Events
        public async Task GetDevices()
        {
            var pdc = await ProgressDialog.Show("Please wait...");
            try
            {

                var apirequest = new ApiRequest();

                var request = await apirequest.Get(GlobalStaticFields.Token, URLConstants.switchAPINewBaseURL, $"Switch/GetUserDevices?userId={GlobalStaticFields.Customer.Email}", "DeviceViewModel");
                if (request.IsSuccessStatusCode)
                {
                    var jsonString = await request.Content.ReadAsStringAsync();
                    var response = JObject.Parse(jsonString);
                    var status = response.Value<bool>("Status");
                    var message = response.Value<string>("Message");
                    if (status)
                    {
                        var data = JArray.Parse(response["Data"].ToString());
                        for (int i = 0; i < data.Count; i++)
                        {
                            Devices.Add(new MobileDevice
                            {
                                ID = data[i].Value<int>("ID"),
                                DeviceName = data[i].Value<string>("Name"),
                                IMEI = data[i].Value<string>("IMEI"),
                                IsActive = data[i].Value<bool>("Enabled"),
                                IsEnabled = GlobalStaticFields.DeviceIMEI() == data[i].Value<string>("IMEI") ? false : true, //disable currently logged in device from being deactivated
                                OS = data[i].Value<string>("OS")
                            });
                        }

                        IsDevicesLoaded = true;
                    }
                }
                else
                {
                    var content = await request.Content.ReadAsStringAsync();
                }
                await pdc.Dismiss();
            }
            catch (WebException e)
            {
                await pdc.Dismiss();
                await  App.Current.MainPage.DisplayAlert("Error", e.Message, "Ok");
            }
            catch (Exception ex)
            {
                await pdc.Dismiss();
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }


        }

        public async Task<bool> UpdateDevice(MobileDevice mobiledevice)
        {
            var pdc = await ProgressDialog.Show("Please wait...");
            bool status = false;
            try
            {
                var apirequest = new ApiRequest();
                dynamic device = new JObject();
                device.ID = mobiledevice.ID;
                device.UserID = GlobalStaticFields.Customer.Email;
                device.Status = mobiledevice.IsActive;

                var request = await apirequest.Post(device, URLConstants.switchAPINewBaseURL, "Switch/UpdateUserDeviceStatus", Security.GetKey(16), Security.GetKeyByte(8), "DeviceViewModel");

                if (request.IsSuccessStatusCode)
                {
                    var jsonString = await request.Content.ReadAsStringAsync();
                    var response = JObject.Parse(jsonString);
                     status = response.Value<bool>("Status");
                    var message = response.Value<string>("Message");
                    if (status)
                    {
                        await pdc.Dismiss();
                        MessageDialog.Show("Info", message, DialogType.Info, "Ok",null);
                    }
                    else
                    {
                        var currentDevice = Devices.FirstOrDefault(x => x.ID == mobiledevice.ID);
                        if (currentDevice != null)
                        {
                            currentDevice.IsActive = !mobiledevice.IsActive;
                        }
                        await pdc.Dismiss();
                        MessageDialog.Show("Info", message, DialogType.Info, "Ok", null);
                    }
                }
                else
                {
                    var content = await request.Content.ReadAsStringAsync();
                    await pdc.Dismiss();
                }
                
            }
            catch (WebException)
            {
                var currentDevice = Devices.FirstOrDefault(x => x.ID == mobiledevice.ID);
                if (currentDevice != null)
                {
                    currentDevice.IsActive = !mobiledevice.IsActive;
                }
                await pdc.Dismiss();
            }
            catch (Exception)
            {
                var currentDevice = Devices.FirstOrDefault(x => x.ID == mobiledevice.ID);
                if (currentDevice != null)
                {
                    currentDevice.IsActive = !mobiledevice.IsActive;
                }
                await pdc.Dismiss();
            }

            return status;
            
        }
        async void DismisProcessDialog()
        {
           
        }
        #endregion
    }
}
