using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Data;
using System.Globalization;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace RouteScheduler.Models
{
    public class GoogleLogic
    {
        private WebClient webClient = new WebClient();
        private readonly ApplicationDbContext Context = new ApplicationDbContext();
        APIKeys apikeys = new APIKeys();

        public List<double> GeocodeAddress(string Address, string City, string State)
        {
            List<double> list = new List<double>();
            var address = ParseAddressString(Address);
            var city = ParseAddressString(City);
            var state = ParseAddressString(State);
            string getGeocode = webClient.DownloadString($"https://maps.googleapis.com/maps/api/geocode/json?address={address},+{city},+{state}&key=" + apikeys.ApiKey);

            var obj = JsonConvert.DeserializeObject<dynamic>(getGeocode);
            var lat = obj.results[0].geometry.location.lat.Value;
            var lng = obj.results[0].geometry.location.lng.Value;
            list.Add(lat);
            list.Add(lng);
            return (list);
        }

        public double DistanceBetweenTwoPlaces(double LatOne, double LatTwo, double LongOne, double LongTwo)
        {
            string GetDistance = webClient.DownloadString($"https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins={LatOne},{LongOne}&destinations={LatTwo}%2C{LongTwo}&key=" + apikeys.ApiKey);
            var obj = JsonConvert.DeserializeObject<dynamic>(GetDistance);
            var DistanceString = obj.results[0].elements.distance.text;
            List<string> DistanceParse = DistanceString.Split(' ').ToList();
            double Distance = Convert.ToDouble(DistanceParse[0]);
            return Distance;
        }

        private string ParseAddressString(string ParseLine)
        {
            List<string> ParsedIs = ParseLine.Split(' ').ToList();
            var CombinedAddress = string.Join("+", ParsedIs);
            return CombinedAddress;
        }
    }
}