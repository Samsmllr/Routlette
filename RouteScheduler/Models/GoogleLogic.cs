using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Data;
using System.Globalization;
using System.Web.Script.Serialization;

namespace RouteScheduler.Models
{
    public class GoogleLogic
    {
         private ApplicationDbContext Context = new ApplicationDbContext();
        APIKeys apikeys = new APIKeys();

        public dynamic GeocodeAddress(string Address, string City, string State)
        {
            var address = ParseString(Address);
            var city = ParseString(City);
            var state = ParseString(State);
            var getGeocode = String.Format($"https://maps.googleapis.com/maps/api/geocode/json?address={address},+{city},+{state}&key=" + apikeys.ApiKey);

            var result = new System.Net.WebClient().DownloadString(getGeocode);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Deserialize<dynamic>(result);
        }

        private string ParseString(string ParseLine)
        {
            List<string> ParsedIs = ParseLine.Split(' ').ToList();
            var CombinedAddress = string.Join("+", ParsedIs);
            return CombinedAddress;
        }
    }
}