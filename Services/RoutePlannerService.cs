using IzmitTransportationSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace IzmitTransportationSystem.Services
{
    public class RoutePlannerService
    {
        private readonly TransportationDataService _dataService;
        private readonly double _maxWalkingDistance = 3.0;
        private readonly double _walkingSpeedKmh = 5.0;

        public RoutePlannerService(TransportationDataService dataService)
        {
            _dataService = dataService;
        }

        private Passenger CreatePassenger(string passengerType)
        {
            switch (passengerType.ToLower())
            {
                case "student":
                    return new StudentPassenger();
                case "elderly":
                    return new ElderlyPassenger();
                default:
                    return new GeneralPassenger();
            }
        }

        public JourneyPlanResponse PlanJourney(JourneyRequest request)
        {
            var startLocation = new Coordinates(request.StartLatitude, request.StartLongitude);
            var destinationLocation = new Coordinates(request.DestinationLatitude, request.DestinationLongitude);
            var passenger = CreatePassenger(request.PassengerType);

            var nearestStartStop = _dataService.FindNearestStop(startLocation);
            var nearestEndStop = _dataService.FindNearestStop(destinationLocation);
            
            if (nearestStartStop == null || nearestEndStop == null)
            {
                // Durak bulunamadı, sadece taksi rotası döndür
                var taxiOnlyRoute = CalculateTaxiRoute(startLocation, destinationLocation, passenger);
                taxiOnlyRoute.RouteType = "Taxi Only";
                
                return new JourneyPlanResponse
                {
                    NearestStartStop = "Durak bulunamadı",
                    DistanceToStartStop = 0,
                    NearestEndStop = "Durak bulunamadı",
                    DistanceFromEndStop = 0,
                    OptimalRoute = taxiOnlyRoute,
                    AlternativeRoutes = new List<JourneyRoute>(),
                    StartLocation = startLocation,
                    EndLocation = destinationLocation,
                    StartStopLocation = startLocation,
                    EndStopLocation = destinationLocation,
                    StopLocations = new Dictionary<string, Coordinates>()
                };
            }

            var distanceToStartStop = nearestStartStop.DistanceTo(startLocation);
            var distanceFromEndStop = nearestEndStop.DistanceTo(destinationLocation);

            var busOnlyRoute = FindBusOnlyRoute(nearestStartStop, nearestEndStop, passenger);
            var tramOnlyRoute = FindTramOnlyRoute(nearestStartStop, nearestEndStop, passenger);
            var mixedRoute = FindMixedRoute(nearestStartStop, nearestEndStop, passenger);
            var taxiRoute = CalculateTaxiRoute(startLocation, destinationLocation, passenger);

            if (busOnlyRoute != null)
            {
                busOnlyRoute = AddAccessEgressWalkSegments(busOnlyRoute, startLocation, destinationLocation);
            }

            if (tramOnlyRoute != null)
            {
                tramOnlyRoute = AddAccessEgressWalkSegments(tramOnlyRoute, startLocation, destinationLocation);
            }

            if (mixedRoute != null)
            {
                mixedRoute = AddAccessEgressWalkSegments(mixedRoute, startLocation, destinationLocation);
            }

            var combinedRoutes = new List<JourneyRoute>();

            var directWalkDistance = startLocation.CalculateDistance(destinationLocation);
            if (directWalkDistance <= _maxWalkingDistance)
            {
                var walkRoute = new JourneyRoute();
                walkRoute.AddSegment(CreateWalkingSegment(startLocation, destinationLocation, "custom_start", "custom_end"));
                walkRoute.RouteType = "Walk Only";
                combinedRoutes.Add(walkRoute);
            }

            if (distanceToStartStop > _maxWalkingDistance || distanceFromEndStop > _maxWalkingDistance)
            {
                if (busOnlyRoute != null)
                {
                    var taxiBusRoute = AddTaxiSegments(busOnlyRoute, startLocation, destinationLocation, passenger);
                    taxiBusRoute.RouteType = "Taxi + Bus";
                    combinedRoutes.Add(taxiBusRoute);
                }

                if (tramOnlyRoute != null)
                {
                    var taxiTramRoute = AddTaxiSegments(tramOnlyRoute, startLocation, destinationLocation, passenger);
                    taxiTramRoute.RouteType = "Taxi + Tram";
                    combinedRoutes.Add(taxiTramRoute);
                }

                if (mixedRoute != null)
                {
                    var taxiMixedRoute = AddTaxiSegments(mixedRoute, startLocation, destinationLocation, passenger);
                    taxiMixedRoute.RouteType = "Taxi + Mixed";
                    combinedRoutes.Add(taxiMixedRoute);
                }
            }
            else
            {
                if (busOnlyRoute != null)
                {
                    busOnlyRoute.RouteType = "Bus Only";
                    combinedRoutes.Add(busOnlyRoute);
                }

                if (tramOnlyRoute != null)
                {
                    tramOnlyRoute.RouteType = "Tram Only";
                    combinedRoutes.Add(tramOnlyRoute);
                }

                if (mixedRoute != null)
                {
                    mixedRoute.RouteType = "Bus + Tram";
                    combinedRoutes.Add(mixedRoute);
                }
            }

            taxiRoute.RouteType = "Taxi Only";
            combinedRoutes.Add(taxiRoute);

            var sortedRoutes = combinedRoutes.OrderBy(r => r.TotalDuration).ToList();
            
            // Eğer hiç rota yoksa, boş bir taksi rotası oluştur
            if (!sortedRoutes.Any())
            {
                var emergencyTaxiRoute = CalculateTaxiRoute(startLocation, destinationLocation, passenger);
                emergencyTaxiRoute.RouteType = "Taxi Only";
                sortedRoutes.Add(emergencyTaxiRoute);
            }

            return new JourneyPlanResponse
            {
                NearestStartStop = nearestStartStop.Name ?? "Bilinmiyor",
                DistanceToStartStop = distanceToStartStop,
                NearestEndStop = nearestEndStop.Name ?? "Bilinmiyor",
                DistanceFromEndStop = distanceFromEndStop,
                OptimalRoute = sortedRoutes.First(),
                AlternativeRoutes = sortedRoutes.Skip(1).ToList(),
                StartLocation = startLocation,
                EndLocation = destinationLocation,
                StartStopLocation = nearestStartStop.Location,
                EndStopLocation = nearestEndStop.Location,
                StopLocations = BuildStopLocations(sortedRoutes)
            };
        }

        private RouteSegment CreateWalkingSegment(Coordinates start, Coordinates end, string fromId, string toId)
        {
            var distance = start.CalculateDistance(end);
            var duration = (int)System.Math.Ceiling((distance / _walkingSpeedKmh) * 60.0);

            return new RouteSegment
            {
                FromStopId = fromId,
                ToStopId = toId,
                VehicleType = "Walk",
                IsTransfer = false,
                Distance = distance,
                Duration = duration,
                Fare = 0
            };
        }

        private JourneyRoute AddAccessEgressWalkSegments(JourneyRoute route, Coordinates startLocation, Coordinates endLocation)
        {
            var newRoute = new JourneyRoute();

            if (route.Segments.Count == 0)
            {
                return route;
            }

            var firstSegment = route.Segments.First();
            var lastSegment = route.Segments.Last();

            var firstStop = _dataService.GetStopById(firstSegment.FromStopId);
            var lastStop = _dataService.GetStopById(lastSegment.ToStopId);

            if (firstStop != null)
            {
                var distanceToFirstStop = firstStop.DistanceTo(startLocation);
                if (distanceToFirstStop > 0.05)
                {
                    newRoute.AddSegment(CreateWalkingSegment(startLocation, firstStop.Location, "custom_start", firstStop.Id));
                }
            }

            foreach (var segment in route.Segments)
            {
                newRoute.AddSegment(segment);
            }

            if (lastStop != null)
            {
                var distanceFromLastStop = lastStop.DistanceTo(endLocation);
                if (distanceFromLastStop > 0.05)
                {
                    newRoute.AddSegment(CreateWalkingSegment(lastStop.Location, endLocation, lastStop.Id, "custom_end"));
                }
            }

            return newRoute;
        }

        private Dictionary<string, Coordinates> BuildStopLocations(IEnumerable<JourneyRoute> routes)
        {
            var locations = new Dictionary<string, Coordinates>();

            foreach (var route in routes)
            {
                foreach (var segment in route.Segments)
                {
                    if (!string.IsNullOrWhiteSpace(segment.FromStopId) && !segment.FromStopId.StartsWith("custom_"))
                    {
                        var stop = _dataService.GetStopById(segment.FromStopId);
                        if (stop != null && !locations.ContainsKey(stop.Id))
                        {
                            locations[stop.Id] = stop.Location;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(segment.ToStopId) && !segment.ToStopId.StartsWith("custom_"))
                    {
                        var stop = _dataService.GetStopById(segment.ToStopId);
                        if (stop != null && !locations.ContainsKey(stop.Id))
                        {
                            locations[stop.Id] = stop.Location;
                        }
                    }
                }
            }

            return locations;
        }

        private RouteSegment CreateTaxiSegment(Coordinates start, Coordinates end, Passenger passenger)
        {
            var taxiInfo = _dataService.GetTaxiInfo();
            var taxi = new Taxi(taxiInfo.OpeningFee, taxiInfo.CostPerKm);

            var distance = start.CalculateDistance(end);
            var fare = taxi.CalculateFare(distance, passenger);
            var duration = taxi.CalculateTravelTime(distance);

            return new RouteSegment
            {
                FromStopId = "custom_start",
                ToStopId = "custom_end",
                VehicleType = "Taxi",
                IsTransfer = false,
                Distance = distance,
                Duration = duration,
                Fare = fare
            };
        }

        private JourneyRoute CalculateTaxiRoute(Coordinates start, Coordinates end, Passenger passenger)
        {
            var route = new JourneyRoute();
            var segment = CreateTaxiSegment(start, end, passenger);
            route.AddSegment(segment);
            return route;
        }

        private JourneyRoute AddTaxiSegments(JourneyRoute route, Coordinates startLocation, Coordinates endLocation, Passenger passenger)
        {
            var newRoute = new JourneyRoute();

            if (route.Segments.Count > 0)
            {
                var firstSegment = route.Segments.First();
                var lastSegment = route.Segments.Last();

                var firstStop = _dataService.GetStopById(firstSegment.FromStopId);
                var lastStop = _dataService.GetStopById(lastSegment.ToStopId);

                if (firstStop != null)
                {
                    var distanceToFirstStop = firstStop.DistanceTo(startLocation);
                    if (distanceToFirstStop > _maxWalkingDistance)
                    {
                        var taxiSegment = CreateTaxiSegment(startLocation, firstStop.Location, passenger);
                        taxiSegment.ToStopId = firstStop.Id;
                        newRoute.AddSegment(taxiSegment);
                    }
                }

                foreach (var segment in route.Segments)
                {
                    newRoute.AddSegment(segment);
                }

                if (lastStop != null)
                {
                    var distanceFromLastStop = lastStop.DistanceTo(endLocation);
                    if (distanceFromLastStop > _maxWalkingDistance)
                    {
                        var taxiSegment = CreateTaxiSegment(lastStop.Location, endLocation, passenger);
                        taxiSegment.FromStopId = lastStop.Id;
                        newRoute.AddSegment(taxiSegment);
                    }
                }
            }

            return newRoute;
        }

        private JourneyRoute FindBusOnlyRoute(Stop start, Stop end, Passenger passenger)
        {
            var cityData = _dataService.GetCityData();
            var busStops = cityData.Stops.Where(s => s.Type == "bus").ToList();

            if (!busStops.Contains(start) || !busStops.Contains(end))
            {
                var startBusStop = start.Type == "bus" ? start : _dataService.FindNearestStop(start.Location, "bus");
                var endBusStop = end.Type == "bus" ? end : _dataService.FindNearestStop(end.Location, "bus");

                return FindShortestPath(startBusStop, endBusStop, passenger, "bus");
            }

            return FindShortestPath(start, end, passenger, "bus");
        }

        private JourneyRoute FindTramOnlyRoute(Stop start, Stop end, Passenger passenger)
        {
            var cityData = _dataService.GetCityData();
            var tramStops = cityData.Stops.Where(s => s.Type == "tram").ToList();

            if (!tramStops.Contains(start) || !tramStops.Contains(end))
            {
                var startTramStop = start.Type == "tram" ? start : _dataService.FindNearestStop(start.Location, "tram");
                var endTramStop = end.Type == "tram" ? end : _dataService.FindNearestStop(end.Location, "tram");

                return FindShortestPath(startTramStop, endTramStop, passenger, "tram");
            }

            return FindShortestPath(start, end, passenger, "tram");
        }

        private JourneyRoute FindMixedRoute(Stop start, Stop end, Passenger passenger)
        {
            var cityData = _dataService.GetCityData();

            if (start.Transfer != null || end.Transfer != null)
            {
                var route = new JourneyRoute();

                if (start.Transfer != null)
                {
                    var transferStop = _dataService.GetStopById(start.Transfer.TransferStopId);

                    route.AddSegment(new RouteSegment
                    {
                        FromStopId = start.Id,
                        ToStopId = transferStop.Id,
                        VehicleType = "Transfer",
                        IsTransfer = true,
                        Distance = 0.1,
                        Duration = start.Transfer.TransferDuration,
                        Fare = start.Transfer.TransferFare
                    });

                    start = transferStop;
                }

                var subRoute = FindShortestPath(start, end, passenger, null);

                if (subRoute != null)
                {
                    foreach (var segment in subRoute.Segments)
                    {
                        route.AddSegment(segment);
                    }
                    return route;
                }
            }

            foreach (var stop in cityData.Stops.Where(s => s.Transfer != null))
            {
                var route1 = FindShortestPath(start, stop, passenger, null);

                if (route1 != null)
                {
                    var transferStop = _dataService.GetStopById(stop.Transfer.TransferStopId);

                    var transferSegment = new RouteSegment
                    {
                        FromStopId = stop.Id,
                        ToStopId = transferStop.Id,
                        VehicleType = "Transfer",
                        IsTransfer = true,
                        Distance = 0.1,
                        Duration = stop.Transfer.TransferDuration,
                        Fare = stop.Transfer.TransferFare
                    };

                    var route2 = FindShortestPath(transferStop, end, passenger, null);

                    if (route2 != null)
                    {
                        var combinedRoute = new JourneyRoute();

                        foreach (var segment in route1.Segments)
                        {
                            combinedRoute.AddSegment(segment);
                        }

                        combinedRoute.AddSegment(transferSegment);

                        foreach (var segment in route2.Segments)
                        {
                            combinedRoute.AddSegment(segment);
                        }

                        return combinedRoute;
                    }
                }
            }

            return null;
        }

        private JourneyRoute FindShortestPath(Stop startStop, Stop endStop, Passenger passenger, string routeType)
        {
            if (startStop == null || endStop == null)
                return null;

            var cityData = _dataService.GetCityData();

            var stops = routeType != null
                ? cityData.Stops.Where(s => s.Type == routeType).ToList()
                : cityData.Stops;

            var distances = stops.ToDictionary(s => s.Id, s => double.MaxValue);
            var previous = stops.ToDictionary(s => s.Id, s => (string)null);
            var visited = new HashSet<string>();

            Vehicle vehicle = routeType == "bus" ? (Vehicle)new Bus() : routeType == "tram" ? (Vehicle)new Tram() : new Bus();

            distances[startStop.Id] = 0;

            while (visited.Count < stops.Count)
            {
                string currentId = null;
                double minDistance = double.MaxValue;

                foreach (var stop in stops)
                {
                    if (!visited.Contains(stop.Id) && distances[stop.Id] < minDistance)
                    {
                        minDistance = distances[stop.Id];
                        currentId = stop.Id;
                    }
                }

                if (currentId == null || currentId == endStop.Id)
                    break;

                visited.Add(currentId);

                var currentStop = stops.First(s => s.Id == currentId);

                foreach (var nextStop in currentStop.NextStops)
                {
                    var targetStop = stops.FirstOrDefault(s => s.Id == nextStop.StopId);

                    if (targetStop != null && !visited.Contains(targetStop.Id))
                    {
                        var distance = distances[currentId] + nextStop.Duration;
                        if (distance < distances[targetStop.Id])
                        {
                            distances[targetStop.Id] = distance;
                            previous[targetStop.Id] = currentId;
                        }
                    }
                }
            }

            if (previous[endStop.Id] != null || startStop.Id == endStop.Id)
            {
                var route = new JourneyRoute();

                var currentId = endStop.Id;

                while (currentId != startStop.Id)
                {
                    var prevId = previous[currentId];
                    if (prevId == null)
                        return null;

                    var fromStop = stops.First(s => s.Id == prevId);
                    var toStop = stops.First(s => s.Id == currentId);
                    var nextStop = fromStop.NextStops.First(ns => ns.StopId == currentId);

                    var segment = new RouteSegment
                    {
                        FromStopId = prevId,
                        ToStopId = currentId,
                        VehicleType = fromStop.Type,
                        IsTransfer = false,
                        Distance = nextStop.Distance,
                        Duration = nextStop.Duration,
                        Fare = vehicle.CalculateFare(nextStop.Distance, passenger)
                    };

                    route.Segments.Insert(0, segment);
                    route.TotalDistance += segment.Distance;
                    route.TotalDuration += segment.Duration;
                    route.TotalFare += segment.Fare;

                    currentId = prevId;
                }

                return route;
            }

            return null;
        }
    }
}
