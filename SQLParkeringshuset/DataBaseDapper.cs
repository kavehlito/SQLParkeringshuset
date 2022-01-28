using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace SQLParkeringshuset
{
    class DataBaseDapper
    {
        static string connString = "data source=.\\SQLEXPRESS; initial catalog = Parking2; persist security info = True; Integrated Security = True;";

        public static List<Models.Car> GetAllCars()
        {
            var sql = "SELECT * FROM Cars";
            var cars = new List<Models.Car>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                cars = connection.Query<Models.Car>(sql).ToList();
            }
            return cars;
        }

        public static List<Models.AllSpots> GetAllSpots()
        {
            var sql = @"
                        SELECT
                            count(*) AS PlatserPerHus,
                            ph.HouseName,
	                        STRING_AGG(ps.SlotNumber, ', ') AS Slots
                        FROM
                            ParkingHouses ph
                        JOIN
                            ParkingSlots ps ON ph.Id = ps.ParkingHouseId
                        GROUP BY
                            ph.HouseName";

            var spotsPerHouse = new List<Models.AllSpots>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                spotsPerHouse = connection.Query<Models.AllSpots>(sql).ToList();

            }
            return spotsPerHouse;
        }

        public static int InsertCar(Models.Car car)
        {
            int affectedRows = 0;

            var sql = $"insert into Cars(Plate, Make, Color) values ('{car.Plate}', '{car.Make}', '{car.Color}')";

            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    affectedRows = connection.Execute(sql);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return affectedRows;
        }

        public static int ParkCar(int carId, int spotId)
        {
            int affectedRows = 0;
            var sql = $"update Cars set ParkingSlotsId = {spotId} where Id = {carId}";
            using (var connection = new SqlConnection(connString))
            {
                affectedRows = connection.Execute(sql);
            }
            return affectedRows;
        }
        public static List<Models.City> GetAllCities()
        {
            var sql = "SELECT * FROM Cities";
            var cities = new List<Models.City>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                cities = connection.Query<Models.City>(sql).ToList();
            }
            return cities;
        }
        public static int InsertCity(Models.City city)
        {
            int affectedRows = 0;

            var sql = $"insert into Cities(CityName) values ('{city.CityName}')";

            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    affectedRows = connection.Execute(sql);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return affectedRows;
        }

        public static List<Models.ParkingHouse> GetAllParkingHouses()
        {
            var sql = "SELECT * FROM ParkingHouses";
            var parkingHouses = new List<Models.ParkingHouse>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                parkingHouses = connection.Query<Models.ParkingHouse>(sql).ToList();
            }
            return parkingHouses;
        }
        public static List<Models.ParkingSlot> GetAllParkingSlots()
        {
            var sql = "SELECT * FROM ParkingSlots";
            var parkingSlots = new List<Models.ParkingSlot>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                parkingSlots = connection.Query<Models.ParkingSlot>(sql).ToList();
            }
            return parkingSlots;
        }
        public static int InsertParkingHouse(Models.ParkingHouse parkingHouse)
        {
            int affectedRows = 0;

            var sql = $"insert into ParkingHouses(HouseName, CityId) values ('{parkingHouse.HouseName}',{parkingHouse.CityId})";

            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    affectedRows = connection.Execute(sql);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return affectedRows;
        }
        public static int InsertParkingSlots(Models.ParkingSlot parkingSlot)
        {
            int affectedRows = 0;

            var sql = $"insert into ParkingSlots(SlotNumber, ElectricOutlet, ParkingHouseId) values ({parkingSlot.SlotNumber},{parkingSlot.ElectricOutlet},{parkingSlot.ParkingHouseId})";

            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    affectedRows = connection.Execute(sql);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return affectedRows;
        }
        public static List<Models.AvailableSlots> GetAvailableSlots(string city)
        {
            var availableSlots = new List<Models.AvailableSlots>();

            var sql = @"SELECT
                            ParkingSlots.Id
                            ,SlotNumber
                        FROM ParkingSlots
                        LEFT JOIN ParkingHouses
                        ON ParkingHouses.Id = ParkingHouseId
                        LEFT JOIN Cars
                        ON Cars.ParkingSlotsId = ParkingSlots.Id
                        LEFT JOIN Cities
                        ON Cities.Id = ParkingHouses.CityId
                        WHERE Cars.ParkingSlotsId IS NULL AND HouseName = '" + city + "'";

            using (var connection = new SqlConnection(connString))
            {
                availableSlots = connection.Query<Models.AvailableSlots>(sql).ToList();
            }
            return availableSlots;
        }
        public static int DriveCar(int carId)
        {
            int affectedRows = 0;
            var sql = $"update Cars set ParkingSlotsId = NULL where Id = {carId}";
            using (var connection = new SqlConnection(connString))
            {
                affectedRows = connection.Execute(sql);
            }
            return affectedRows;
        }
        public static List<Models.AmountOfOutlets> GetAmountOfOutlets()
        {
            var sql = @"SELECT
                            HouseName
                            ,COUNT(ElectricOutlet) AS 'OutletAmount'
                        FROM ParkingHouses
                        LEFT JOIN ParkingSlots
                        ON ParkingSlots.ParkingHouseId = ParkingHouses.Id
                        WHERE ParkingSlots.ElectricOutlet = 1
                        GROUP BY HouseName";
            var outletsPerHouse = new List<Models.AmountOfOutlets>();
            using(var connection = new SqlConnection(connString))
            {
                outletsPerHouse = connection.Query<Models.AmountOfOutlets>(sql).ToList();
            }
            return outletsPerHouse;
        }
        public static List<Models.CityOutlets> GetAmountOfCityOutlets()
        {
            var sql = @"SELECT
                            CityName
                            ,COUNT(ElectricOutlet) AS AmountOutlets
                        FROM ParkingSlots
                        LEFT JOIN ParkingHouses
                        ON ParkingHouses.Id = ParkingSlots.ParkingHouseId
                        LEFT JOIN Cities
                        ON Cities.Id =  ParkingHouses.CityId
                        WHERE ElectricOutlet = 1
                        GROUP BY CityName";
            var cityOutlets = new List<Models.CityOutlets>();
            using (var connection = new SqlConnection(connString))
            {
                cityOutlets = connection.Query<Models.CityOutlets>(sql).ToList();
            }
            return cityOutlets;
        }
        public static List<Models.EmptySpots> GetEmptySpots()
        {
            var sql = @"SELECT
                            ParkingSlots.Id
                            ,SlotNumber
                            ,HouseName
                            ,CityName
                        FROM ParkingSlots
                        LEFT JOIN Cars ON Cars.ParkingSlotsId = ParkingSlots.Id
                        LEFT JOIN ParkingHouses ON ParkingHouses.Id = ParkingSlots.ParkingHouseId
                        JOIN Cities ON ParkingHouses.CityId = Cities.Id
                        WHERE Cars.Id IS NULL";
            var emptySlots = new List<Models.EmptySpots>();
            using (var connection = new SqlConnection(connString))
            {
                emptySlots = connection.Query<Models.EmptySpots>(sql).ToList();
            }
            return emptySlots;
        }
    }
}
