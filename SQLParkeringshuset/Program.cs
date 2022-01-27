using System;
using System.Diagnostics;

namespace SQLParkeringshuset
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("Alla bilar: ");
            //var cars = DataBaseADO.GetAllCars();
            var cars = DataBaseDapper.GetAllCars();

            stopwatch.Start();
            foreach (var car in cars)
            {
                Console.WriteLine($"{car.Id}\t{car.Plate}\t{car.Make}\t{car.Color}\t{car.ParkingSlotsId}");
            }
            stopwatch.Stop();

            Console.WriteLine("Tid: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("---------------------------------------------");
            stopwatch.Restart();
            //var allParkingspots = DataBaseADO.GetAllSpots();
            var allParkingspots = DataBaseDapper.GetAllSpots();

            foreach (var house in allParkingspots)
            {
                Console.WriteLine($"{house.HouseName}\t{house.PlatserPerHus}\t{house.Slots}");
            }

            stopwatch.Stop();
            Console.WriteLine("Tid: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("----------------------------------------------");


            // Lägg till ny bil
           /* Random rnd = new Random();
            var rNumber = rnd.Next(100, 999);

            var newCar = new Models.Car
            {
                Plate = "XPW" + rNumber,
                Make = "Tesla",
                Color = "Röd"
            };
            int rowsAffected = DataBaseDapper.InsertCar(newCar);
            Console.WriteLine("Antal bilar tillagda: " + rowsAffected);
            Console.WriteLine("----------------------------------------------");

            // Parkera bil
            rowsAffected = DataBaseDapper.ParkCar(6, 12);
            Console.WriteLine("Antal bilar parkerade: " + rowsAffected);*/
            Console.WriteLine("----------------------------------------------");

            Console.WriteLine("Alla städer: ");
            var cities = DataBaseDapper.GetAllCities();
            foreach (var city in cities)
            {
                Console.WriteLine($"{city.Id}\t{city.CityName}");
            }
            Console.WriteLine("----------------------------------------------");

            /*var newCity = new Models.City
            {
                CityName = "Märsta"
            };
            int rowsAffected = DataBaseDapper.InsertCity(newCity);
            Console.WriteLine($"Antal städer tillagda: {rowsAffected}"); 
            Console.WriteLine("----------------------------------------------");*/

            Console.WriteLine("Alla parkeringshus: ");
            var parkingHouses = DataBaseDapper.GetAllParkingHouses();
            foreach (var parkingHouse in parkingHouses)
            {
                Console.WriteLine($"{parkingHouse.Id}\t{parkingHouse.HouseName}\t{parkingHouse.CityId}");
            }
            Console.WriteLine("----------------------------------------------");

            /*var newParkingHouse = new Models.ParkingHouse
            {
                HouseName = "Märta Garaget",
                CityId = 4
            };
            int rowsAffected = DataBaseDapper.InsertParkingHouse(newParkingHouse);
            Console.WriteLine($"Antal parkeringshus tillagda: {rowsAffected}");
            Console.WriteLine("----------------------------------------------"); */

            /*var newParkingSlot = new Models.ParkingSlot
            {
                SlotNumber = 1,
                ElectricOutlet = 1,
                ParkingHouseId = 7
            };
            int rowsAffected = DataBaseDapper.InsertParkingSlots(newParkingSlot);
            Console.WriteLine($"Antal parkeringsplatser tillagda: {rowsAffected}");*/
        }
    }
}
