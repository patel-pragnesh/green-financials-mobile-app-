using SterlingSwitch.PopUps;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.More.Device
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceView : SwitchMasterPage
    {
        private DeviceViewModel vm;
        private MobileDevice mobileDevice;
        private bool IsToggledCanceled = true;
        public DeviceView()
        {
            InitializeComponent();
            vm = new DeviceViewModel(Navigation);
            BindingContext = vm;
            mobileDevice = new MobileDevice();
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var obj = (ListView)sender;
            obj.SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (vm.Devices.Count > 0) return;

            await vm.GetDevices();

        }

        private void ExtendedSwitch_OnToggled(object sender, bool e)
        {
            try
            {


                if (!vm.IsDevicesLoaded) return;

                if (!IsToggledCanceled)
                {
                    IsToggledCanceled = true;
                    return;
                }

                var obj = (Switch)sender;
                mobileDevice = (MobileDevice)obj.BindingContext;
                mobileDevice.IsActive = e;
                string message = e ? $"Are you sure you want to ACTIVATE {mobileDevice.DeviceName}?" : $"Are you sure you want to DEACTIVATE {mobileDevice.DeviceName}?";
                MessageDialog.Show("Toggle Device", message, DialogType.Question, "Yes", new Action(() => ToggleDevice(obj)), "No", () =>
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        var currentDevice = vm.Devices.FirstOrDefault(x => x.ID == mobileDevice.ID);
                        if (currentDevice != null)
                        {

                            IsToggledCanceled = false;
                            obj.IsToggled = !e;

                        }
                    });

                });
            }
            catch (Exception)
            {


            }
        }

        private void ToggleDevice(Switch obj)
        {
            try
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                {
                    var status = await vm.UpdateDevice(mobileDevice);


                    //Toggle back to original position if UpdateDevice fails
                    if (!status)
                        obj.IsToggled = !status;
                });
            }
            catch (Exception)
            {


            }
        }
    }
}