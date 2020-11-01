using Newtonsoft.Json;
using RestSharp;
using soa_01.CalculatorService;
using soa_01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace soa_01
{
    public class Program
    {
        private static readonly calcPortTypeClient calc = new calcPortTypeClient();

        public static void Main(string[] args)
        {
            while (true)
            {
                HandleConsoleActions();

                // za namene simuliranja lokacije je začetna lokacija v centru Ljubljane (46.054320, 14.504485)
                var myLocation = new Location()
                {
                    Name = "Ljubljana",
                    Latitude = 46.054320 + GetRandomNumber(),
                    Longitude = 14.504485 + GetRandomNumber()
                };

                var locationTime = GetLocationTime(myLocation.Name);
                var nearestParking = PrepareParkingData(myLocation);

                foreach (var parking in nearestParking)
                {
                    var (parkingData, distance) = parking;
                    Console.WriteLine(
                        "Naziv: {0}, Oddaljenost: {1}, Št. prostih parkirnih mest: {2}, Cena: {3}, Datum/Ura: {4}",
                        parkingData.Ime,
                        $"{(int) (distance / 1000)} km in {(int) (distance % 1000)} metrov",
                        parkingData.Zasedenost?.PKratkotrajniki ?? "Ni podatka",
                        locationTime.Datetime.Hour >= 19 || locationTime.Datetime.Hour <= 7
                            ? "ZASTONJ"
                            : !string.IsNullOrWhiteSpace(parkingData.Opis) && parkingData.Opis.IndexOf('€') > 0
                                ? parkingData.Opis.Substring(parkingData.Opis.IndexOf('€') - 5, 4) + " EUR/uro"
                                : "Ni podatka",
                        locationTime.Datetime
                    );
                }

                Console.WriteLine("--------------------------------------------------------");
            }
        }

        private static void HandleConsoleActions()
        {
            Console.Write("Press one (1) to get new location, or press two (2) for exit: ");
            var key = Console.ReadKey();
            Console.WriteLine();
            switch (key.KeyChar)
            {
                case '1':
                    break;
                case '2':
                    Environment.Exit(0);
                    break;
                default:
                    HandleConsoleActions();
                    break;
            }
        }

        private static Root LoadLjubljanaParkingData()
        {
            Console.WriteLine("Loading parking data...");
            var restClient = new RestClient("https://opendata.si/promet/parkirisca/lpt/");
            var request = new RestRequest(Method.GET);
            var response = restClient.Execute(request);

            return JsonConvert.DeserializeObject<Root>(response.Content);
        }

        private static WorldTime GetLocationTime(string location)
        {
            try
            {
                Console.WriteLine("Loading location time...");
                var restClient = new RestClient($"https://worldtimeapi.org/api/timezone/Europe/{location}");
                var request = new RestRequest(Method.GET);
                var response = restClient.Execute(request);

                return JsonConvert.DeserializeObject<WorldTime>(response.Content);
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong with WordTime API, we will try again after 5 seconds...");
                Thread.Sleep(5000);
                return GetLocationTime(location);
            }
        }

        private static List<Tuple<Parkirisca, double>> PrepareParkingData(Location location)
        {
            var data = LoadLjubljanaParkingData();

            // Filtriramo parkirišča, ki imajo podatke o lokaciji
            var parkings = data?.Parkirisca
                .Where(x => x.KoordinataXWgs != 0 && x.KoordinataYWgs != 0)
                .Select(parking =>
                    new Tuple<Parkirisca, Location>(
                        parking,
                        new Location()
                        {
                            Name = "Ljubljana",
                            Latitude = parking.KoordinataYWgs,
                            Longitude = parking.KoordinataXWgs
                        }
                    ));

            Console.WriteLine("Calculate distances...");
            var distanceToParking = new List<Tuple<Parkirisca, double>>();
            foreach (var parking in parkings ?? new List<Tuple<Parkirisca, Location>>())
            {
                Console.Write(".");
                var (parkingData, parkingLocation) = parking;
                distanceToParking.Add(new Tuple<Parkirisca, double>(
                    parkingData,
                    CalculateDistance(parkingLocation, location)
                ));
            }

            Console.WriteLine();

            // razvrstimo po razdalji in vzamemo prvih 5 najbližjih parkirišč
            return distanceToParking.OrderBy(x => x.Item2).Take(5).ToList();
        }

        // Haversine formula za izračun razdalje z upoštevanje ukrivljenosti zemlje
        private static double CalculateDistance(Location locationA, Location locationB)
        {
            try
            {
                const int R = 6378137; // obseg zemlje v metrih
                var dLat = ConvertToRadians(locationB.Latitude - locationA.Latitude);
                var dLong = ConvertToRadians(locationB.Longitude - locationA.Longitude);

                var haversineFirstStep =
                    calc.add(
                        Math.Sin(dLat / 2) * Math.Sin(dLat / 2),
                        Math.Cos(ConvertToRadians(locationA.Latitude)) *
                        Math.Cos(ConvertToRadians(locationB.Latitude)) *
                        Math.Sin(dLong / 2) *
                        Math.Sin(dLong / 2)
                    );
                var haversineSecondStep =
                    2 * Math.Atan2(Math.Sqrt(haversineFirstStep), Math.Sqrt(1 - haversineFirstStep));
                var distance = R * haversineSecondStep;
                return distance; // izračunana razdalja v metrih
            }
            catch (Exception)
            {
                Console.WriteLine(
                    "\nSomething went wrong with CalculatorService, we will try again after 5 seconds...");
                Thread.Sleep(5000);
                return CalculateDistance(locationA, locationB);
            }
        }

        private static double ConvertToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private static double GetRandomNumber()
        {
            var random = new Random();
            const double MAX_INTERVAL_VALUE = 10, MIN_INTERVAL_VALUE = -10;
            return (random.NextDouble() * Math.Abs(MAX_INTERVAL_VALUE - MIN_INTERVAL_VALUE) + MIN_INTERVAL_VALUE) / 100;
        }
    }
}