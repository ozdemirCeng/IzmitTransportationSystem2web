using IzmitTransportationSystem.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;

namespace IzmitTransportationSystem.Services
{
    public class TransportationDataService
    {
        private static TransportationDataService _instance;
        private CityData _cityData;

        public static TransportationDataService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TransportationDataService();
                return _instance;
            }
        }

        private TransportationDataService()
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string jsonData = File.ReadAllText("veri.json");
                JObject jObject = JObject.Parse(jsonData);

                _cityData = new CityData
                {
                    City = jObject["city"].ToString(),
                    Taxi = new TaxiInfo
                    {
                        OpeningFee = double.Parse(jObject["taxi"]["openingFee"].ToString()),
                        CostPerKm = double.Parse(jObject["taxi"]["costPerKm"].ToString())
                    },
                    Stops = new List<Stop>()
                };

                foreach (var stopJson in jObject["duraklar"])
                {
                    var stop = new Stop
                    {
                        Id = stopJson["id"].ToString(),
                        Name = stopJson["name"].ToString(),
                        Type = stopJson["type"].ToString(),
                        Location = new Coordinates(
                            double.Parse(stopJson["lat"].ToString()),
                            double.Parse(stopJson["lon"].ToString())
                        ),
                        IsTerminal = bool.Parse(stopJson["sonDurak"].ToString()),
                        NextStops = new List<NextStop>()
                    };

                    foreach (var nextStopJson in stopJson["nextStops"])
                    {
                        stop.NextStops.Add(new NextStop
                        {
                            StopId = nextStopJson["stopId"].ToString(),
                            Distance = double.Parse(nextStopJson["mesafe"].ToString()),
                            Duration = int.Parse(nextStopJson["sure"].ToString()),
                            Fare = double.Parse(nextStopJson["ucret"].ToString())
                        });
                    }

                    if (stopJson["transfer"] != null && stopJson["transfer"].Type != JTokenType.Null)
                    {
                        stop.Transfer = new Transfer
                        {
                            TransferStopId = stopJson["transfer"]["transferStopId"].ToString(),
                            TransferDuration = int.Parse(stopJson["transfer"]["transferSure"].ToString()),
                            TransferFare = double.Parse(stopJson["transfer"]["transferUcret"].ToString())
                        };
                    }

                    _cityData.Stops.Add(stop);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                throw;
            }
        }

        public CityData GetCityData() => _cityData;

        public Stop GetStopById(string id)
        {
            return _cityData.Stops.FirstOrDefault(s => s.Id == id);
        }

        public Stop FindNearestStop(Coordinates location, string type = null)
        {
            var stops = string.IsNullOrEmpty(type)
                ? _cityData.Stops
                : _cityData.Stops.Where(s => s.Type == type);

            return stops.OrderBy(s => s.DistanceTo(location)).FirstOrDefault();
        }

        public TaxiInfo GetTaxiInfo() => _cityData.Taxi;
    }
}
