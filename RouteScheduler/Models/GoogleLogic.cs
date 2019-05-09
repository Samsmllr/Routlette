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
        private ApplicationDbContext Context = new ApplicationDbContext();
        APIKeys apikeys = new APIKeys();

        public List<double> GeocodeAddress(string Address, string City, string State)
        {
            List<double> list = new List<double>();
            var address = ParseString(Address);
            var city = ParseString(City);
            var state = ParseString(State);
            string getGeocode = webClient.DownloadString($"https://maps.googleapis.com/maps/api/geocode/json?address={address},+{city},+{state}&key=" + apikeys.ApiKey);

            var obj = JsonConvert.DeserializeObject<dynamic>(getGeocode);
            var lat = obj.results[0].geometry.location.lat.Value;
            var lng = obj.results[0].geometry.location.lng.Value;
            list.Add(lat);
            list.Add(lng);
            return (list);
        }

        private string ParseString(string ParseLine)
        {
            List<string> ParsedIs = ParseLine.Split(' ').ToList();
            var CombinedAddress = string.Join("+", ParsedIs);
            return CombinedAddress;
        }
    }
}