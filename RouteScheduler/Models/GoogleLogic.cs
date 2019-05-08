using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;

namespace RouteScheduler.Models
{
    public class GoogleLogic
    {
         private ApplicationDbContext Context = new ApplicationDbContext();
        APIKeys apikeys = new APIKeys();

        public string GeocodeAddress(string address, string city, string state)
        {
            string AddressIs = ParseString(address);
            string CityIs = ParseString(city);
            string StateIs = ParseString(state);
            string getGeocode = ($"https://maps.googleapis.com/maps/api/geocode/json?address={AddressIs},+{CityIs},+{StateIs}&key=" + apikeys.ApiKey);

            return getGeocode;
        }

        private string ParseString(string ParseLine)
        {
            List<string> ParsedIs = ParseLine.Split(' ').ToList();
            var CombinedAddress = string.Join("+", ParsedIs);
            return CombinedAddress;
        }
    }
}