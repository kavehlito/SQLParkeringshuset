using System;
using System.Diagnostics;

namespace SQLParkeringshuset
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                Console.WriteLine("Welcome to SQL Stuff\n");
                int menuSel = 15;
                do
                {
                    menuSel = MenuSelection();
                    MenuExecution(menuSel);

                } while (menuSel != 15);
            }
        }
        public static int MenuSelection()
        {
            int menuSel;
            Console.WriteLine($"\nPlease Select An Option");
            Console.WriteLine("Menu:");
            Console.WriteLine("1 - View Cars and Spots");
            Console.WriteLine("2 - Add Car");
            Console.WriteLine("3 - Park Car");
            Console.WriteLine("4 - View Cities");
            Console.WriteLine("5 - Add City");
            Console.WriteLine("6 - View Parkinghouses");
            Console.WriteLine("7 - Add Parkinghouse");
            Console.WriteLine("8 - Add Parkingslot");
            Console.WriteLine("9 - Show Available Slots");
            Console.WriteLine("10 - Take a car for a spin");
            Console.WriteLine("11 - Amount of Outlets per parkinghouse");
            Console.WriteLine("12 - Amount of Outlets per City");
            Console.WriteLine("13 - View Parked Cars");
            Console.WriteLine("14 - Empty Spots");
            Console.WriteLine("15 - Quit");

            string userInput = Console.ReadLine();
            int.TryParse(userInput, out menuSel);

            //Your code for menu selection

            return menuSel;
        }
        public static void MenuExecution(int menuSel)
        {
            try
            {
                //Your code for execution based on the menu selection
                switch (menuSel)
                {
                    case 1:
                        ViewCarsAndSpots();
                        break;
                    case 2:
                        AddCar();
                        break;
                    case 3:
                        ParkCar();
                        break;
                    case 4:
                        ViewCities();
                        break;
                    case 5:
                        AddCity();
                        break;
                    case 6:
                        ViewParkingHouse();
                        break;
                    case 7:
                        AddParkingHouse();
                        break;
                    case 8:
                        AddParkingSlot();
                        break;
                    case 9:
                        AvailableSlots();
                        break;
                    case 10:
                        DriveCar();
                        break;
                    case 11:
                        HousesWithOutlets();
                        break;
                    case 12:
                        CityOutletAmount();
                        break;
                    case 13:
                        ParkedCars();
                        break;
                    case 14:
                        EmptySlots();
                        break;
                    case 15:
                        Console.WriteLine("Bye Felicia");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void ViewCarsAndSpots()
        {
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("Alla bilar: ");
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

            var allParkingspots = DataBaseDapper.GetAllSpots();

            foreach (var house in allParkingspots)
            {
                Console.WriteLine($"{house.HouseName}\t{house.PlatserPerHus}\t{house.Slots}");
            }

            stopwatch.Stop();
            Console.WriteLine("Tid: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("----------------------------------------------");
        }
        public static void ViewCities()
        {
            Console.WriteLine("Alla städer: ");
            var cities = DataBaseDapper.GetAllCities();
            foreach (var city in cities)
            {
                Console.WriteLine($"{city.Id}\t{city.CityName}");
            }
            Console.WriteLine("----------------------------------------------");
        }
        public static void ViewParkingHouse()
        {
            Console.WriteLine("Alla parkeringshus: ");
            var parkingHouses = DataBaseDapper.GetAllParkingHouses();
            foreach (var parkingHouse in parkingHouses)
            {
                Console.WriteLine($"{parkingHouse.Id}\t{parkingHouse.HouseName}\t{parkingHouse.CityId}");
            }
            Console.WriteLine("----------------------------------------------");
        }
        public static void AddCar()
        {
            Console.WriteLine("Ange registernummer");
            string userPlateInput = Console.ReadLine().ToUpper();
            Console.WriteLine("Ange tillverkare");
            string userMakeInput = Console.ReadLine();
            Console.WriteLine("Ange färg");
            string userColorInput = Console.ReadLine();
            // Lägg till ny bil
            //Random rnd = new Random();
            //var rNumber = rnd.Next(100, 999);

            var newCar = new Models.Car
            {
                Plate = userPlateInput,
                Make = userMakeInput,
                Color = userColorInput
            };
            int rowsAffected = DataBaseDapper.InsertCar(newCar);
            Console.WriteLine("Antal bilar tillagda: " + rowsAffected);
            Console.WriteLine("----------------------------------------------");
        }
        public static void ParkCar()
        {
            // Parkera bil
            Console.WriteLine("Välj en bil från ID nummer");
            var cars = DataBaseDapper.GetAllCars();
            foreach (var car in cars)
            {
                Console.WriteLine($"ID:{car.Id}\t{car.Plate}\t{car.Make}\t{car.Color}\t{car.ParkingSlotsId}");
            }
            Console.WriteLine();
            int userCarInput = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            var parkingHouses = DataBaseDapper.GetAllParkingHouses();
            for (int i = 0; i < cars.Count; i++)
            {
                if (cars[i].Id == userCarInput)
                {
                    foreach (var parkingHouse in parkingHouses)
                    {
                        Console.WriteLine($"{parkingHouse.HouseName}");
                    }
                    string parkingHouseInput = Console.ReadLine();
                    for (int j = 0; j < parkingHouses.Count; j++)
                    {
                        if (parkingHouses[j].HouseName.ToLower() == parkingHouseInput)
                        {
                            Console.WriteLine("Lediga parkeringsplatser");
                            var availableSlots = DataBaseDapper.GetAvailableSlots(parkingHouseInput);
                            foreach (var emptySpot in availableSlots)
                            {
                                Console.WriteLine($"ID:{emptySpot.Id}\t ParkeringsNr:{emptySpot.SlotNumber}");
                            }
                            int pickEmptySpot = Convert.ToInt32(Console.ReadLine());
                            for (int k = 0; k < availableSlots.Count; k++)
                            {
                                if (availableSlots[k].Id == pickEmptySpot)
                                {
                                    int rowsAffected = DataBaseDapper.ParkCar(userCarInput, pickEmptySpot);
                                    Console.WriteLine("Antal bilar parkerade: " + rowsAffected);
                                    Console.WriteLine("----------------------------------------------");

                                }
                            }
                        }
                    }
                }
            }
        }
        public static void AddCity()
        {
            Console.WriteLine("Vilken stad vill du lägga till?");
            string userInput = Console.ReadLine();
            var newCity = new Models.City
            {
                CityName = userInput
            };
            int rowsAffected = DataBaseDapper.InsertCity(newCity);
            Console.WriteLine($"Antal städer tillagda: {rowsAffected}");
            Console.WriteLine("----------------------------------------------");
        }
        public static void AddParkingHouse()
        {
            Console.WriteLine("Ange Parkeringshus du vill lägga till");
            string userInput = Console.ReadLine();
            var newParkingHouse = new Models.ParkingHouse
            {
                HouseName = userInput,
                CityId = 4
            };
            int rowsAffected = DataBaseDapper.InsertParkingHouse(newParkingHouse);
            Console.WriteLine($"Antal parkeringshus tillagda: {rowsAffected}");
            Console.WriteLine("----------------------------------------------");
        }
        public static void AddParkingSlot()
        {
            var newParkingSlot = new Models.ParkingSlot
            {
                SlotNumber = 2,
                ElectricOutlet = 0,
                ParkingHouseId = 7
            };
            int rowsAffected = DataBaseDapper.InsertParkingSlots(newParkingSlot);
            Console.WriteLine($"Antal parkeringsplatser tillagda: {rowsAffected}");
        }
        public static void AvailableSlots()
        {
            Console.WriteLine("Välj en stad");
            var cities = DataBaseDapper.GetAllCities();
            var parkingHouses = DataBaseDapper.GetAllParkingHouses();
            foreach (var city in cities)
            {
                Console.WriteLine($"{city.CityName}");
            }
            Console.WriteLine();
            string userInput = Console.ReadLine().ToLower();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Välj ett parkeringshus");
            for (int i = 0; i < cities.Count; i++)
            {
                if (cities[i].CityName.ToLower() == userInput)
                {
                    foreach (var parkingHouse in parkingHouses)
                    {
                        if (parkingHouse.CityId == cities[i].Id)
                        {
                            Console.WriteLine($"{parkingHouse.HouseName}");
                        }
                    }
                    string parkingHouseInput = Console.ReadLine();
                    for (int j = 0; j < parkingHouses.Count; j++)
                    {
                        if (parkingHouses[j].HouseName.ToLower() == parkingHouseInput)
                        {
                            Console.WriteLine("Lediga parkeringsplatser");
                            var availableSlots = DataBaseDapper.GetAvailableSlots(parkingHouseInput);
                            foreach (var emptySpot in availableSlots)
                            {
                                Console.WriteLine($"ID:{emptySpot.Id}\t ParkeringsNr:{emptySpot.SlotNumber}");
                            }
                        }
                    }
                }
            }
        }
        public static void DriveCar()
        {
            Console.WriteLine("Välj en bil från ID nummer");
            var cars = DataBaseDapper.GetAllCars();
            foreach (var car in cars)
            {
                if (car.ParkingSlotsId != null)
                {
                Console.WriteLine($"ID:{car.Id}\t{car.Plate}\t{car.Make}\t{car.Color}\t{car.ParkingSlotsId}");
                }
            }
            int userInput = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < cars.Count; i++)
            {
                if (cars[i].Id == userInput)
                {
                    int rowsAffected = DataBaseDapper.DriveCar(userInput);
                    Console.WriteLine("Antal bilar som bränner gummi" + rowsAffected);
                }
            }
        }
        public static void HousesWithOutlets()
        {
            Console.WriteLine("Antal Elplatser per parkeringshus");
            var OutletsParkingHouse = DataBaseDapper.GetAmountOfOutlets();
            foreach (var outlets in OutletsParkingHouse)
            {
                Console.WriteLine($"{outlets.HouseName}\t{outlets.OutletAmount}");
            }
        }
        public static void CityOutletAmount()
        {
            Console.WriteLine("Antal Elplatser per stad");
            var cityOutlets = DataBaseDapper.GetAmountOfCityOutlets();
            foreach (var cityOutlet in cityOutlets)
            {
                Console.WriteLine($"{cityOutlet.CityName}\t{cityOutlet.AmountOutlets}");
            }
        }
        public static void ParkedCars()
        {
            Console.WriteLine("Parkerade bilar");
            var cars = DataBaseDapper.GetAllCars();
            foreach (var car in cars)
            {
                if (car.ParkingSlotsId != null)
                {
                    Console.WriteLine($"ID:{car.Id}\t{car.Plate}\t{car.Make}\t{car.Color}\t{car.ParkingSlotsId}");
                }
            }
        }
        public static void EmptySlots()
        {
            Console.WriteLine("Antal Lediga platser");
            var emptySlots = DataBaseDapper.GetEmptySpots();
            foreach (var emptySlot in emptySlots)
            {
                Console.WriteLine($"{emptySlot.Id}\t{emptySlot.SlotNumber}\t{emptySlot.HouseName}\t{emptySlot.CityName}");
            }
        }
    }
}
