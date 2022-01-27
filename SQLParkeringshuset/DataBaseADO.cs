using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLParkeringshuset
{
    class DataBaseADO
    {
        public static List<Models.Car> GetAllCars()
        {
            var connString = "data source=.\\SQLEXPRESS; initial catalog = Parking2; persist security info = True; Integrated Security = True;";
            var sql = "SELECT * FROM Cars";
            var cars = new List<Models.Car>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var x = reader.GetOrdinal("Make");

                            var car = new Models.Car
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Plate = reader.GetString(reader.GetOrdinal("Plate")),
                                Make = reader.GetString(reader.GetOrdinal("Make")),
                                // Make = reader.GetString(2),
                                Color = reader.GetString(reader.GetOrdinal("Color")),
                                ParkingSlotsId = reader.IsDBNull(reader.GetOrdinal("ParkingSlotsId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ParkingSlotsId")),
                            };
                            cars.Add(car);
                        }
                    }
                }
            }
            return cars;
        }


        public static List<Models.AllSpots> GetAllSpots()
        {
            var connString = "data source=.\\SQLEXPRESS; initial catalog = Parking2; persist security info = True; Integrated Security = True;";
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
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var house = new Models.AllSpots
                            {
                                PlatserPerHus = reader.GetInt32(reader.GetOrdinal("PlatserPerHus")),
                                HouseName = reader.GetString(reader.GetOrdinal("HouseName")),
                                Slots = reader.GetString(reader.GetOrdinal("Slots"))

                            };
                            spotsPerHouse.Add(house);
                        }
                    }
                }
            }
            return spotsPerHouse;
        }


    }
}
