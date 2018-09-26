using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Pages.CardlessWithdrawals.ViewModel
{
    public class ATMLocation
    {
        public string PlaceName { get; set; }
        public string Address { get; set; }
        public string Distance { get; set; }
        public string OpenNow { get; set; }
        public Location Location { get; set; }

        public static double GetDistance(double lat1, double lon1, double lat2, double lon2, DistanceUnit unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;

            if (unit == DistanceUnit.Kiliometers)
            {
                dist = dist * 1.609344;
            }
            else if (unit == DistanceUnit.NauticalMiles)
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }

        //:::This function converts decimal degrees to radians:::
        private static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }


        //:::This function converts radians to decimal degrees:::
        private static double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }

    public enum DistanceUnit
    {
        Miles,
        Kiliometers,
        NauticalMiles
    }

    public class ATMs
    {
        public object[] html_attributions { get; set; }
        public Result[] results { get; set; }
        public string status { get; set; }
    }

    public class Result
    {
        public Geometry geometry { get; set; }
        public string icon { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public Opening_Hours opening_hours { get; set; }
        public Photo[] photos { get; set; }
        public string place_id { get; set; }
        public Plus_Code plus_code { get; set; }
        public float rating { get; set; }
        public string reference { get; set; }
        public string scope { get; set; }
        public string[] types { get; set; }
        public string vicinity { get; set; }
    }

    public class Geometry
    {
        public Location location { get; set; }
        public Viewport viewport { get; set; }
    }

    public class Location
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Viewport
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Northeast
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Southwest
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Opening_Hours
    {
        public bool? open_now { get; set; }
    }

    public class Plus_Code
    {
        public string compound_code { get; set; }
        public string global_code { get; set; }
    }

    public class Photo
    {
        public int height { get; set; }
        public string[] html_attributions { get; set; }
        public string photo_reference { get; set; }
        public int width { get; set; }
    }
}
