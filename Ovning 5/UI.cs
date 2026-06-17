using System.Text.RegularExpressions;

public class UI : IUI
{
    private Handler handler;

    // Konstruktor som tar emot handlern från Program.cs
    public UI(Handler handler)
    {
        this.handler = handler;
    }

    // Huvudmetoden som startar igång hela gränssnittet
    public void Run()
    {
        int menuOption = 0;
        bool runProgram = true;

        while (runProgram)
        {
            Console.Clear();
            ShowMenu();

            if (!int.TryParse(Console.ReadLine(), out menuOption))
            {
                // Switchen hoppar till 'default' vid felaktig input
                menuOption = -1;
            }

            switch (menuOption)
            {
                case 0: // Exit
                    Console.Clear();
                    Console.WriteLine("Exiting...");
                    runProgram = false;
                    break;

                case 1:
                    ListVehiclesFromMenu();
                    break;

                case 2:
                    AddVehicleFromMenu();
                    break;

                case 3:
                    RemoveVehicleFromMenu();
                    break;

                case 4:
                    ListVehicleTypesCountFromMenu();
                    break;

                case 5:
                    SearchByPropertiesFromMenu();
                    break;

                case 6:
                    SearchByRegNoFromMenu();
                    break;


                default:
                    Console.Clear();
                    Console.WriteLine("Wrong input. Please try again.");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void ShowMenu()
    {
        //En huvudmeny för programmet som håller det vid liv och informerar användaren.
        Console.Clear();
        Console.WriteLine("Welcome to the garage!" + Environment.NewLine);
        Console.WriteLine("--- MENU ---" + Environment.NewLine);
        Console.WriteLine("Please choose an option below by entering a digit as shown." + Environment.NewLine);
        Console.WriteLine("0. Exit program");
        Console.WriteLine("1. List vehicles");
        Console.WriteLine("2. Add vehicle");
        Console.WriteLine("3. Remove vehicle");
        Console.WriteLine("4. List vehicle types");
        Console.WriteLine("5. Search by properties");
        Console.WriteLine("6. Search by registration number");
    }

    public void RemoveVehicleFromMenu()
    {
        Console.Clear();
        string regNoSearchParameter = "";
        string pattern = @"^[A-Z]{3}\d{3}$";

        // Kolla om lediga platser är lika med totala kapaciteten
        if (handler.GetFreeSpotsCount() == handler.Capacity)
        {
            Console.WriteLine("The garage is empty. There are no vehicles to remove.");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            return; 
        }


        while (true)
        {
            Console.Write("Please enter the registration number to remove (e.g., ABC123): ");
            regNoSearchParameter = Console.ReadLine()?.Trim().ToUpper() ?? "";

            if (Regex.IsMatch(regNoSearchParameter, pattern))
            {
                break;
            }

            Console.WriteLine("Invalid format! Please try again.");
            Console.WriteLine();
        }

        // Anropa handlern och spara resultatet
        bool wasRemoved = handler.RemoveVehicle(regNoSearchParameter);

        // Visa resultatet för användaren
        if (wasRemoved)
        {
            Console.WriteLine($"Vehicle with Registration number {regNoSearchParameter} was removed.");
        }
        else
        {
            Console.WriteLine($"No vehicle with Registration number {regNoSearchParameter} was found.");
        }

        Console.WriteLine();
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    public void ListVehiclesFromMenu()
    {
        Console.Clear();
        Console.WriteLine("Vehicles in the garage:");
        Console.WriteLine();

        int counter = 0;

        // Hämta listan från handlern och loopa igenom den
        foreach (var vehicle in handler.GetVehicles())
        {
            if (vehicle == null) continue; // Hoppa över tomma platser

            counter++;

            Console.Write($"No: {counter} Type: {vehicle.GetType().Name}, Reg No: {vehicle.RegistrationNumber}, Color: {vehicle.Color}, Wheels: {vehicle.NumberOfWheels}");

            // Skriv ut de unika egenskaperna baserat på typ
            if (vehicle is Car c) Console.Write($", Fuel: {c.FuelType}");
            else if (vehicle is Motorcycle m) Console.Write($", Engine size: {m.CylinderVolume}cc");
            else if (vehicle is Airplane a) Console.Write($", Engines: {a.NumberOfEngines}");
            else if (vehicle is Bus b) Console.Write($", Seats: {b.NumberOfSeats}");
            else if (vehicle is Boat bo) Console.Write($", Length: {bo.Length}m");
            Console.WriteLine();
        }

        if (counter == 0)
        {
            Console.WriteLine("No vehicles in the garage.");
        }

        Console.WriteLine();
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    public void AddVehicleFromMenu()
    {
        int freeSpots = handler.GetFreeSpotsCount();

        Console.Clear();
        Console.WriteLine($"There is currently {freeSpots} spots left in the garage.\n");

        if (freeSpots <= 0)
        {
            Console.WriteLine("No available space in the garage. Please try again later.\n");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("You are welcome to park your vehicle in the garage.\n");

        // 1. Inmatning: Fordonstyp
        string vehicleType = "";
        string[] allowedTypes = { "Car", "Motorcycle", "Airplane", "Bus", "Boat" };

        while (true)
        {
            Console.Write("Please enter the type of vehicle (Car, Motorcycle, Airplane, Bus, Boat): ");
            vehicleType = Console.ReadLine()?.Trim() ?? "";

            if (allowedTypes.Any(t => t.ToLower() == vehicleType.ToLower()))
            {
                vehicleType = capitalFirstLetter(vehicleType);
                break;
            }

            Console.WriteLine("Invalid vehicle type! You must choose a type from the list.\n");
        }

        // 2. Inmatning: Registreringsnummer
        string regNo = "";
        string pattern = @"^[A-Z]{3}\d{3}$";

        while (true)
        {
            Console.Write("Please enter the registration number (e.g., ABC123): ");
            regNo = Console.ReadLine()?.Trim().ToUpper() ?? "";

            if (!Regex.IsMatch(regNo, pattern))
            {
                Console.WriteLine("Invalid format! Please try again.\n");
                continue;
            }

            // Fråga handlern om numret redan finns taggat i garaget
            if (handler.RegistrationNumberExists(regNo))
            {
                Console.WriteLine("There is already a vehicle with that registration number parked here, try another.\n");
                continue;
            }

            break; // Formatet är rätt och det är unikt!
        }

        // 3. Inmatning: Färg
        string color = "";
        string[] allowedColors = { "Black", "Red", "Blue", "White", "Yellow" };

        while (true)
        {
            Console.Write("Enter a color of vehicle (Black, Red, Blue, White, Yellow): ");
            color = Console.ReadLine()?.Trim() ?? "";

            if (allowedColors.Any(c => c.ToLower() == color.ToLower()))
            {
                color = capitalFirstLetter(color);
                break;
            }

            Console.WriteLine("Invalid color! You must choose a color from the list.\n");
        }

        // 4. Inmatning: Hjul
        int numberOfWheels = 0;

        while (true)
        {
            Console.Write("Please enter the number of wheels of vehicle (0 to 8): ");
            string input = Console.ReadLine()?.Trim() ?? "";

            if (int.TryParse(input, out numberOfWheels) && numberOfWheels >= 0 && numberOfWheels <= 8)
            {
                break;
            }

            Console.WriteLine("Invalid input! Please enter a valid number (0 to 8).\n");
        }

        // 5. Hantera unika egenskaper baserat på fordonstyp
        IVehicle? tempVehicle = null;

        if (vehicleType == "Car")
        {
            string fuelType = "";
            string[] allowedFuelTypes = { "Gasoline", "Diesel" };

            while (true)
            {
                Console.Write("Please enter the fuel type of vehicle (Gasoline / Diesel): ");
                fuelType = Console.ReadLine()?.Trim() ?? "";

                if (allowedFuelTypes.Any(c => c.ToLower() == fuelType.ToLower()))
                {
                    fuelType = capitalFirstLetter(fuelType);
                    break;
                }
                Console.WriteLine("Invalid fuel type! Choose from the list.\n");
            }
            tempVehicle = new Car { RegistrationNumber = regNo, Color = color, NumberOfWheels = numberOfWheels, FuelType = fuelType };
        }
        else if (vehicleType == "Motorcycle")
        {
            int cylinderVolume = 0;
            while (true)
            {
                Console.Write("Please enter the cylinder volume (0 to 9000): ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (int.TryParse(input, out cylinderVolume) && cylinderVolume >= 0 && cylinderVolume <= 9000) break;
                Console.WriteLine("Invalid input! Enter a valid number (0 to 9000).\n");
            }
            tempVehicle = new Motorcycle { RegistrationNumber = regNo, Color = color, NumberOfWheels = numberOfWheels, CylinderVolume = cylinderVolume };
        }
        else if (vehicleType == "Airplane")
        {
            int numberOfEngines = 0;
            while (true)
            {
                Console.Write("Please enter the number of engines (0 to 6): ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (int.TryParse(input, out numberOfEngines) && numberOfEngines >= 0 && numberOfEngines <= 6) break;
                Console.WriteLine("Invalid input! Enter a valid number (0 to 6).\n");
            }
            tempVehicle = new Airplane { RegistrationNumber = regNo, Color = color, NumberOfWheels = numberOfWheels, NumberOfEngines = numberOfEngines };
        }
        else if (vehicleType == "Bus")
        {
            int numberOfSeats = 0;
            while (true)
            {
                Console.Write("Please enter the number of seats (0 to 52): ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (int.TryParse(input, out numberOfSeats) && numberOfSeats >= 0 && numberOfSeats <= 52) break;
                Console.WriteLine("Invalid input! Enter a valid number (0 to 52).\n");
            }
            tempVehicle = new Bus { RegistrationNumber = regNo, Color = color, NumberOfWheels = numberOfWheels, NumberOfSeats = numberOfSeats };
        }
        else if (vehicleType == "Boat")
        {
            double length = 0;
            while (true)
            {
                Console.Write("Please enter the length of the boat in meters: ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (double.TryParse(input, out length) && length > 0) break;
                Console.WriteLine("Invalid input! Enter a valid number above 0.\n");
            }
            tempVehicle = new Boat { RegistrationNumber = regNo, Color = color, NumberOfWheels = numberOfWheels, Length = length };
        }

        // 6. Skicka det färdiga fordonet till handlern för parkering
        if (tempVehicle != null)
        {
            handler.AddVehicle(tempVehicle);
            Console.WriteLine($"\nSuccessfully parked your {vehicleType} ({regNo}) in the garage!");
        }

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }

    private string capitalFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }

    public void SearchByRegNoFromMenu()
    {
        Console.Clear();
        Console.WriteLine("Write the Reg No of the vehicle you are looking for.\n");

        string regNoSearchParameter = "";
        string pattern = @"^[A-Z]{3}\d{3}$";

        // 1. Inmatning och validering
        while (true)
        {
            Console.Write("Please enter the registration number (3 letters, 3 numbers, or leave blank to see all): ");
            regNoSearchParameter = Console.ReadLine()?.Trim().ToUpper() ?? "";
            Console.WriteLine();

            if (string.IsNullOrEmpty(regNoSearchParameter))
            {
                break; // Godkänn tomt svar direkt
            }

            if (Regex.IsMatch(regNoSearchParameter, pattern))
            {
                break; // Godkänn korrekt format
            }

            Console.WriteLine("Invalid format! Please try again (e.g., ABC123).\n");
        }

        // 2. Anropa handlern för att få sökresultaten
        var searchResults = handler.SearchByRegNo(regNoSearchParameter);

        // 3. Skriv ut resultaten till användaren
        Console.WriteLine("Search results:");
        int counter = 0;

        foreach (var item in searchResults)
        {
            if (item == null) continue;

            Console.Write($"Type: {item.GetType().Name}, Reg No: {item.RegistrationNumber}, Color: {item.Color}, Wheels: {item.NumberOfWheels}");

            // Unika egenskaper per fordonstyp
            if (item is Car c) Console.Write($", Fuel: {c.FuelType}");
            else if (item is Motorcycle m) Console.Write($", Engine size: {m.CylinderVolume}cc");
            else if (item is Airplane a) Console.Write($", Engines: {a.NumberOfEngines}");
            else if (item is Bus b) Console.Write($", Seats: {b.NumberOfSeats}");
            else if (item is Boat bo) Console.Write($", Length: {bo.Length}m");

            counter++;
            Console.WriteLine();
        }

        if (counter == 0)
        {
            Console.WriteLine("No vehicles matched your search.");
        }

        Console.WriteLine();
        Console.WriteLine($"Number of search results: {counter}\n");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }



    public void SearchByPropertiesFromMenu()
    {
        Console.Clear();

        // 1. Typ av fordon
        string typeSearchParameter = "";
        string[] allowedTypes = { "Car", "Motorcycle", "Airplane", "Bus", "Boat" };

        while (true)
        {
            Console.Write("Write the type of vehicle (Car, Motorcycle, Airplane, Bus, Boat) you are looking for (leave blank if vehicle type does not matter): ");
            typeSearchParameter = Console.ReadLine()?.Trim() ?? "";
            Console.WriteLine();

            if (string.IsNullOrEmpty(typeSearchParameter))
            {
                break; // Godkänn tomt svar direkt
            }

            if (allowedTypes.Any(t => t.ToLower() == typeSearchParameter.ToLower()))
            {
                typeSearchParameter = capitalFirstLetter(typeSearchParameter);
                break;
            }

            Console.WriteLine("Invalid vehicle type! You must choose a type from the list.");
            Console.WriteLine();
        }

        // 2. Färg 
        string colorSearchParameter = "";
        string[] allowedColors = { "Black", "Red", "Blue", "White", "Yellow" };

        while (true)
        {
            Console.Write("Write the color (Black, Red, Blue, White, Yellow) you are looking for (leave blank if color does not matter): ");
            colorSearchParameter = Console.ReadLine()?.Trim() ?? "";
            Console.WriteLine();

            if (string.IsNullOrEmpty(colorSearchParameter))
            {
                break; // Godkänn tomt svar direkt
            }

            if (allowedColors.Any(c => c.ToLower() == colorSearchParameter.ToLower()))
            {
                colorSearchParameter = capitalFirstLetter(colorSearchParameter);
                break;
            }

            Console.WriteLine("Invalid color! You must choose a color from the list.");
            Console.WriteLine();
        }

        // 3. Antal hjul 
        int numberOfWheelsSearchParameter = 0;
        bool userEnteredEmptyWheelSearch = false;

        while (true)
        {
            Console.Write("Write the number of wheels you are looking for on the vehicle (0 to 8) (leave blank if the number of wheels does not matter): ");
            string input = Console.ReadLine()?.Trim() ?? "";
            Console.WriteLine();

            if (string.IsNullOrEmpty(input))
            {
                userEnteredEmptyWheelSearch = true;
                break; // Godkänn tomt svar direkt
            }

            if (int.TryParse(input, out numberOfWheelsSearchParameter) && numberOfWheelsSearchParameter >= 0 && numberOfWheelsSearchParameter <= 8)
            {
                break; // Giltig siffra, gå vidare
            }

            Console.WriteLine("Invalid input! Please enter a valid number (0 to 8).");
            Console.WriteLine();
        }

        // Anropa handlern med alla fyra insamlade parametrar
        var searchResults = handler.SearchByProperties(
            typeSearchParameter,
            colorSearchParameter,
            numberOfWheelsSearchParameter,
            userEnteredEmptyWheelSearch
        );

        Console.WriteLine("Search results:");
        int counter = 0;

        // Loopa igenom och formatera sökresultaten
        foreach (var item in searchResults)
        {
            if (item == null) continue;

            Console.Write($"Type: {item.GetType().Name}, Reg No: {item.RegistrationNumber}, Color: {item.Color}, Wheels: {item.NumberOfWheels}");

            if (item is Car c) Console.Write($", Fuel: {c.FuelType}");
            else if (item is Motorcycle m) Console.Write($", Engine size: {m.CylinderVolume}cc");
            else if (item is Airplane a) Console.Write($", Engines: {a.NumberOfEngines}");
            else if (item is Bus b) Console.Write($", Seats: {b.NumberOfSeats}");
            else if (item is Boat bo) Console.Write($", Length: {bo.Length}m");

            counter++;
            Console.WriteLine();
        }

        if (counter == 0)
        {
            Console.WriteLine("No vehicles matched your search.");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Number of search results: {counter}");
        Console.WriteLine();
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    public void ListVehicleTypesCountFromMenu()
    {
        Console.Clear();

        // Hämta ordlistan från handlern
        Dictionary<string, int> typeCounts = handler.GetVehicleTypesCount();

        // Skriv ut alla typer och antal
        foreach (KeyValuePair<string, int> item in typeCounts)
        {
            Console.WriteLine($"Amount of vehicle type {item.Key}: {item.Value}");
        }

        // Om ordlistan är tom betyder det att garaget var tomt
        if (typeCounts.Count == 0)
        {
            Console.WriteLine("No vehicles in the garage.");
        }

        Console.WriteLine();
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }
}
