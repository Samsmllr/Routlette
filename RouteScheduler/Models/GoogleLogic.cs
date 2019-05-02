using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RouteScheduler.Models
{
    public class GoogleLogic
    {
         private ApplicationDbContext Context = new ApplicationDbContext();
        APIKeys apikeys = new APIKeys();

        public void GeocodeAddress(string address, string city, string state)
        {
            string AddressIs = ParseString(address);
            string CityIs = ParseString(city);
            string StateIs = ParseString(state);
            var GeocodeAddress = ("https://maps.googleapis.com/maps/api/geocode/json?address=" + AddressIs + ",+" + CityIs + ",+" + StateIs + "&key=" + apikeys.ApiKey);
        }

        private string ParseString(string ParseLine)
        {
            List<string> ParsedIs = ParseLine.Split(' ').ToList();
            var CombinedAddress = string.Join("+", ParsedIs);
            return CombinedAddress;
        }
    }
}