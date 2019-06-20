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
using RouteScheduler.Logic;

namespace RouteScheduler.Models
{
    public class APILogic
    {
        private WebClient webClient = new WebClient();
        private readonly ApplicationDbContext Context = new ApplicationDbContext();
        private APIKeys apikeys = new APIKeys();

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

        public double DistanceBetweenTwoPlaces(double LatOne, double LongOne, double LatTwo, double LongTwo)
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

        public List<EventsHolder> GetEventsByIdAndDay(int id, DateTime day)
        {
            List<EventsHolder> list = new List<EventsHolder>();
            string getEvents = webClient.DownloadString("http://localhost:58619/api/events");
            var obj = JsonConvert.DeserializeObject<dynamic>(getEvents);
            EventsHolder events;
            foreach(var item in obj)
            {
                events = new EventsHolder();
                if (item.UserId == id && item.StartDate.Date == day.Date)
                {
                    int CustomerIdIs = item.CustomerId;
                    events.UserId = item.UserId;
                    events.StartDate = item.StartDate;
                    events.EndDate = item.EndDate;
                    events.Latitude = item.Latitude;
                    events.Longitude = item.Longitude;

                    list.Add(events);
                }
            }
            return list;
        }


    }
}