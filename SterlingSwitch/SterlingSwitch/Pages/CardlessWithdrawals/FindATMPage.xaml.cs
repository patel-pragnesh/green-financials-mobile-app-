using Newtonsoft.Json;
using SterlingSwitch.Pages.CardlessWithdrawals.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SterlingSwitch.Services;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace SterlingSwitch.Pages.CardlessWithdrawals
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FindATMPage : SwitchMasterPage
    {
		public FindATMPage ()
		{
			InitializeComponent ();
            map.UiSettings.CompassEnabled = false;
            map.UiSettings.ZoomControlsEnabled = false;
            UpdateMap();
        }

        List<ATMLocation> atmLocations = new List<ATMLocation>();
        Position UserPosition { get; set; }

        private async void UpdateMap()
        {
            try
            {
                
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);


                if (status == PermissionStatus.Granted)
                {
                    DoUpdate();
                }
                else
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        MessageDialog.Show("Find ATM", "Switch need your location to get ATM around you.", DialogType.Info, "OK", null);
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = results[Permission.Location];

                    if (status == PermissionStatus.Granted)
                    {
                        DoUpdate();
                    }
                    else if (status != PermissionStatus.Unknown)
                    {
                        MessageDialog.Show("Location Denied", "Can not continue with ATM search, kindly permit location access to continue.", DialogType.Error, "OK", null);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageDialog.Show("Find ATM", "Error getting your GPS location at this time.", DialogType.Error, "OK", null);
            }
        }

        private async void DoUpdate()
        {
            var position = await GlobalStaticFields.GetLocationFromPlugin().ConfigureAwait(false);

            if (position != null)
            {
                var pos = new Position(position.Latitude, position.Longitude);
                UserPosition = pos;
                map.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Distance.FromMeters(100)));

                GetATMs($"{position.Latitude},{position.Longitude}");
            }
            else
            {
                MessageDialog.Show("Find ATM", "Error getting your GPS location at this time.", DialogType.Error, "Retry", UpdateMap, "Cancel", null);
            }
        }

        private async void GetATMs(string location)
        {
            string url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={location}&radius=1500&type=bank&keyword=sterling&key=AIzaSyDYdbG_jHO5TOZcIPkKHsF0uD6po9AWvik";

            var client = new HttpClient();

            try
            {
                string response = await client.GetStringAsync(url);
                var resultObject = JsonConvert.DeserializeObject<ATMs>(response);

                var coordSplit = location.Split(',');

                double lat1 = double.Parse(coordSplit[0]);
                double lon1 = double.Parse(coordSplit[1]);

                if (resultObject.status.ToLower() == "ok")
                {
                    foreach (var place in resultObject.results)
                    {
                        if (place.name.ToLower().Contains("sterling"))
                        {
                            atmLocations.Add(new ATMLocation
                            {
                                PlaceName = place.name,
                                Address = place.vicinity,
                                Location = place.geometry.location,
                                Distance = $"{ATMLocation.GetDistance(lat1, lon1, place.geometry.location.lat, place.geometry.location.lng, DistanceUnit.Kiliometers).ToString("N2")}km",
                                OpenNow = GetOpenHours(place?.opening_hours?.open_now)
                            });
                        }
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ATMPlacesListView.ItemsSource = atmLocations;

                        //Update pin on the map
                        foreach (var item in atmLocations)
                            map.Pins.Add(new Pin
                            {
                                Position = new Position(item.Location.lat, item.Location.lng),
                                Label = item.PlaceName,
                                Address = item.Address,
                                Type = PinType.Place
                            });

                        map.SelectedPin = map.Pins.ElementAt(0);
                        map.MoveToRegion(MapSpan.FromCenterAndRadius(map.Pins.ElementAt(0).Position, Distance.FromMeters(500)), true);
                        LoadingView.IsVisible = false;
                    });
                }
            }
            catch (Exception ex)
            {
                LoadingView.IsVisible = false;
                MessageDialog.Show("ATM Places", "Error getting ATM locations at this time.", DialogType.Error, "Retry", () => { GetATMs(location); }, "Cancel", null);
                
                var log = ex.Message;
                await BusinessLogic.Log(ex.ToString(), "Error getting ATM Locations", url, "", "", "FindATMPage");
            }
        }

        private string GetOpenHours(bool? open_now)
        {
            if (open_now == null)
                return "N/A";

            if (open_now.Value)
                return "Yes";

            return "No";

        }

        private void GetDirection(string start, string destination)
        {
            string url = "";

            if (Device.RuntimePlatform == Device.iOS)
                url = String.Format("http://maps.apple.com/maps?saddr={0}&daddr={1}", start, destination);
            if (Device.RuntimePlatform == Device.Android)
                url = String.Format("http://maps.google.com/maps?saddr={0}&daddr={1}", start, destination);

            Device.OpenUri(new Uri(url));
        }

        public async void GeoCode(String address)
        {
            var geocoder = new Xamarin.Forms.GoogleMaps.Geocoder();
            var positions = await geocoder.GetPositionsForAddressAsync(address);
            if (positions.Count() > 0)
            {
                var pos = positions.First();
                map.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Distance.FromMeters(5000)));
                var reg = map.VisibleRegion;
                //var format = "0.00";
                //labelStatus.Text = $"Center = {reg.Center.Latitude.ToString(format)}, {reg.Center.Longitude.ToString(format)}";
            }
            else
            {
                //await this.DisplayAlert("Not found", "No results", "Close");
                //Debug.WriteLine("Geocoder returns no results");
            }
        }

        private void map_SelectedPinChanged(object sender, SelectedPinChangedEventArgs e)
        {
            if (e.SelectedPin == null)
                return;

            var index = map.Pins.IndexOf(e.SelectedPin);

            ATMPlacesListView.SelectedItem = atmLocations[index];
        }

        private void ATMPlacesListView_SelectedItemChanged(object sender, EventArgs e)
        {
            if (ATMPlacesListView.SelectedItem == null)
                return;

            map.SelectedPin = map.Pins.ElementAt(atmLocations.IndexOf((ATMLocation)ATMPlacesListView.SelectedItem));
            map.MoveToRegion(MapSpan.FromCenterAndRadius(map.Pins.ElementAt(0).Position, Distance.FromMeters(500)), true);
        }

        private void GetDirectionTapped(object sender, EventArgs e)
        {
            if (ATMPlacesListView.SelectedItem == null)
                return;

            var item = ((ATMLocation)ATMPlacesListView.SelectedItem).Location;

            string start = $"{UserPosition.Latitude},{UserPosition.Longitude}";
            string end = $"{item.lat},{item.lng}";

            GetDirection(start, end);
        }
    }

    public class EqualsParameterContextConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            return value == ((View)parameter).BindingContext;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}