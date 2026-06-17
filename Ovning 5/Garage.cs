using System.Collections;

public class Garage<T> : IEnumerable<T>, IGarage<T> where T : class, IVehicle // En generisk garageklass som kan lagra och stega igenom fordon.

{
    private T?[] vehicles; // Intern array som lagrar alla parkerade fordon i garaget.

    public int Capacity
    {
        get { return vehicles.Length; } // Hämtar kapaciteten direkt från fordons-arrayen
    }

    public Garage(int capacity) // Skapar ett nytt garage med ett bestämt antal parkeringsplatser.
    {
        vehicles = new T[capacity];
    }

    public bool AddVehicle(T vehicle) // Letar upp den första lediga platsen i garaget och parkerar fordonet där om det finns utrymme kvar.
    {
        for (int i = 0; i < vehicles.Length; i++)
        {
            if (vehicles[i] == null)
            {
                vehicles[i] = vehicle;
                return true;
            }
        }

        return false;
    }

    public int GetFreeSpotsCount()
    {
        return vehicles.Count(v => v == null);
    }

    // Kontrollerar om ett registreringsnummer redan är parkerat
    public bool RegistrationNumberExists(string regNo)
    {
        foreach (var vehicle in vehicles)
        {
            if (vehicle != null && vehicle.RegistrationNumber.Equals(regNo, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    public bool RemoveVehicle(string regNo)
    {
        for (int i = 0; i < vehicles.Length; i++)
        {
            if (vehicles[i] == null) continue;

            if (vehicles[i]?.RegistrationNumber.Equals(regNo, StringComparison.OrdinalIgnoreCase) == true)
            {
                vehicles[i] = default; // Ta bort fordonet
                return true;
            }
        }
        return false;
    }



    public Dictionary<string, int> GetVehicleTypesCount()
    {
        var typeCounts = new Dictionary<string, int>();

        foreach (var vehicle in vehicles)
        {
            if (vehicle == null) continue;

            string type = vehicle.GetType().Name;

            if (typeCounts.TryGetValue(type, out int value))
            {
                typeCounts[type] = value + 1;
            }
            else
            {
                typeCounts[type] = 1;
            }
        }

        return typeCounts;
    }



    public IEnumerable<T> SearchByProperties(string type, string color, int wheels, bool emptyWheelSearch)
    {
        var successfulResults = new List<T>();

        foreach (var vehicle in vehicles)
        {
            if (vehicle == null) continue;

            bool isTypeMatch = string.IsNullOrEmpty(type) ||
                               vehicle.GetType().Name.Equals(type, StringComparison.OrdinalIgnoreCase);

            bool isColorMatch = string.IsNullOrEmpty(color) ||
                                vehicle.Color.Equals(color, StringComparison.OrdinalIgnoreCase);

            bool isNumberOfWheelsMatch = emptyWheelSearch ||
                                         vehicle.NumberOfWheels == wheels;

            if (isTypeMatch && isColorMatch && isNumberOfWheelsMatch)
            {
                successfulResults.Add(vehicle);
            }
        }

        return successfulResults;
    }


    public IEnumerable<T> SearchByRegNo(string regNo)
    {
        var results = new List<T>();

        foreach (var vehicle in vehicles)
        {
            if (vehicle == null) continue;

            // Om söksträngen är tom visas alla, annars matchas registreringsnumret
            bool isMatch = string.IsNullOrEmpty(regNo) ||
                          vehicle.RegistrationNumber.Equals(regNo, StringComparison.OrdinalIgnoreCase);

            if (isMatch)
            {
                results.Add(vehicle);
            }
        }

        return results;
    }

    public IEnumerator<T> GetEnumerator() // Gör det möjligt att iterera över alla parkerade fordon i garaget med en foreach-loop.
    {
        foreach (var vehicle in vehicles)
        {
            if (vehicle != null)
            {
                yield return vehicle;
            }
        }
    }



    IEnumerator IEnumerable.GetEnumerator() // Skickar vidare till den nya varianten av GetEnumerator(), som är generisk till skillnad från den gamla. 
    {
        return GetEnumerator();
    }
}


//public Vehicle AddVehicleFromMenu() // Ber användaren knappa in ett nytt fordon om plats finns i garaget
//{
//    Vehicle tempVehicle = null;
//    int counter = 0;

//    for (int i = 0; i < vehicles.Length; i++)
//    {
//        if (vehicles[i] == null)
//        {
//            counter++;
//        }
//    }

//    Console.Clear();
//    Console.WriteLine($"There is currently {counter} spots left in the garage.");
//    Console.WriteLine();

//    if (counter <= 0)
//    {
//        Console.WriteLine("No available space in the garage. Please try again later.");
//        Console.WriteLine();
//        Console.WriteLine("Press any key to continue.");
//        Console.ReadKey();
//        return null;
//    }
//    else
//    {
//        Console.WriteLine("You are welcome to park your vehicle in the garage.");
//        Console.WriteLine();
//    }


//    // Typ av fordon
//    string vehicleType = "";
//    string[] allowedTypes = { "Car", "Motorcycle", "Airplane", "Bus", "Boat" };

//    while (true)
//    {
//        Console.Write("Please enter the type of vehicle (Car, Motorcycle, Airplane, Bus, Boat): ");
//        vehicleType = Console.ReadLine()?.Trim() ?? "";

//        if (allowedTypes.Any(t => t.ToLower() == vehicleType.ToLower()))
//        {
//            vehicleType = capitalFirstLetter(vehicleType);
//            break;
//        }

//        Console.WriteLine("Invalid vehicle type! You must choose a type from the list.");
//        Console.WriteLine();
//    }





//    // Registreringsnummer
//    string regNo = "";
//    string pattern = @"^[A-Z]{3}\d{3}$"; // Kräver exakt 3 bokstäver och 3 siffror

//    bool notAValidRegNo = true;

//    while (notAValidRegNo)
//    {
//        Console.Write("Please enter the registration number of vehicle in the format of 3 letters followed by 3 numbers: ");
//        regNo = Console.ReadLine()?.Trim().ToUpper() ?? "";

//        if (Regex.IsMatch(regNo, pattern))
//        {
//            // Formatet är korrekt här.

//            // Kolla i garaget om ett fordon med det inmatade registreringsnumret redan finns
//            foreach (var vehicle in vehicles)
//            {
//                if (vehicle == null) continue; // Hoppa över tomma fordon för att undvika krasch.

//                bool isRegNoMatch = vehicle.RegistrationNumber?.ToLower() == regNo.ToLower();

//                if (isRegNoMatch) // om det redan finns ett fordon med det inmatade registreringsnumret
//                {
//                    // skriv meddelande och be användaren skriva in ett nytt registreringsnummer
//                    Console.WriteLine("There is already a vehicle with the given registration number parked in the garage, please try again with a different one.");
//                    break; // Gå ur loopen att kolla efter matchande registreringsnummer
//                }
//                else
//                {
//                    // Formatet är korrekt OCH registreringsnumret är hittills unikt - gå vidare i loopen att kolla efter matchande registreringsnummer.
//                    notAValidRegNo = false;
//                }
//            }
//        }
//        else
//        {
//            Console.WriteLine("Invalid format! Please try again (e.g., ABC123).");
//        }
//    }




//    // Färg 
//    string color = "";
//    string[] allowedColors = { "Black", "Red", "Blue", "White", "Yellow" };

//    while (true)
//    {
//        Console.Write("Enter a color of vehicle (Black, Red, Blue, White, Yellow): ");
//        color = Console.ReadLine()?.Trim() ?? "";

//        if (allowedColors.Any(c => c.ToLower() == color.ToLower()))
//        {
//            color = capitalFirstLetter(color);
//            //color = char.ToUpper(color[0]) + color.Substring(1).ToLower();
//            break;
//        }

//        Console.WriteLine("Invalid color! You must choose a color from the list.");
//        Console.WriteLine();
//    }



//    // Antal hjul 
//    int numberOfWheels = 0;

//    while (true)
//    {
//        Console.Write("Please enter the number of wheels of vehicle (0 to 8): ");
//        string input = Console.ReadLine()?.Trim() ?? "";

//        // TryParse returnerar true om texten är en giltig siffra
//        if (int.TryParse(input, out numberOfWheels) && numberOfWheels >= 0 && numberOfWheels <= 8)
//        {
//            break; // Inmatningen är en giltig siffra, avbryt loopen
//        }

//        Console.WriteLine("Invalid input! Please enter a valid number (0 to 8).");
//        Console.WriteLine();
//    }






//    if (vehicleType == "Car")
//    {
//        // Drivmedelstyp
//        string fuelType = "";
//        string[] allowedFuelTypes = { "Gasoline", "Diesel" };

//        while (true)
//        {
//            Console.Write("Please enter the fuel type of vehicle (Gasoline / Diesel): ");
//            fuelType = Console.ReadLine()?.Trim() ?? "";

//            if (allowedFuelTypes.Any(c => c.ToLower() == fuelType.ToLower()))
//            {
//                fuelType = capitalFirstLetter(fuelType);
//                //fuelType = char.ToUpper(fuelType[0]) + fuelType.Substring(1).ToLower();
//                break;
//            }

//            Console.WriteLine("Invalid fuel type! You must choose a fuel type from the list.");
//            Console.WriteLine();
//        }
//        tempVehicle = (new Car { RegistrationNumber = regNo, Color = color, NumberOfWheels = numberOfWheels, FuelType = fuelType });
//    }




//    if (vehicleType == "Motorcycle")
//    {
//        // Cylindervolym
//        int cylinderVolume = 0;

//        while (true)
//        {
//            Console.Write("Please enter the cylinder volume of vehicle (0 to 9000): ");
//            string input = Console.ReadLine()?.Trim() ?? "";

//            // TryParse returnerar true om texten är en giltig siffra
//            if (int.TryParse(input, out cylinderVolume) && cylinderVolume >= 0 && cylinderVolume <= 9000)
//            {
//                break; // Inmatningen är en giltig siffra, avbryt loopen
//            }

//            Console.WriteLine("Invalid input! Please enter a valid number (0 to 9000).");
//            Console.WriteLine();
//        }
//        tempVehicle = (new Motorcycle { RegistrationNumber = regNo, Color = color, NumberOfWheels = numberOfWheels, CylinderVolume = cylinderVolume });
//    }

//    if (vehicleType == "Airplane")
//    {
//        // Antal motorer
//        int numberOfEngines = 0;

//        while (true)
//        {
//            Console.Write("Please enter the number of engines of vehicle (0 to 6): ");
//            string input = Console.ReadLine()?.Trim() ?? "";

//            // TryParse returnerar true om texten är en giltig siffra
//            if (int.TryParse(input, out numberOfEngines) && numberOfEngines >= 0 && numberOfEngines <= 6)
//            {
//                break; // Inmatningen är en giltig siffra, avbryt loopen
//            }

//            Console.WriteLine("Invalid input! Please enter a valid number (0 to 6).");
//            Console.WriteLine();
//        }
//        tempVehicle = (new Airplane { RegistrationNumber = regNo, Color = color, NumberOfWheels = numberOfWheels, NumberOfEngines = numberOfEngines });
//    }

//    if (vehicleType == "Bus")
//    {
//        // Antal säten
//        int numberOfSeats = 0;

//        while (true)
//        {
//            Console.Write("Please enter the number of seats of vehicle (0 to 52): ");
//            string input = Console.ReadLine()?.Trim() ?? "";

//            // TryParse returnerar true om texten är en giltig siffra
//            if (int.TryParse(input, out numberOfSeats) && numberOfSeats >= 0 && numberOfSeats <= 52)
//            {
//                break; // Inmatningen är en giltig siffra, avbryt loopen
//            }

//            Console.WriteLine("Invalid input! Please enter a valid number (0 to 52).");
//            Console.WriteLine();
//        }
//        tempVehicle = (new Bus { RegistrationNumber = regNo, Color = color, NumberOfWheels = numberOfWheels, NumberOfSeats = numberOfSeats });
//    }

//    if (vehicleType == "Boat")
//    {
//        // Längd
//        double length = 0.0;

//        while (true)
//        {
//            Console.Write("Please enter the length of vehicle (0,0 to 30,0): ");
//            string input = Console.ReadLine()?.Trim() ?? "";

//            // TryParse returnerar true om texten är en giltig siffra
//            if (double.TryParse(input, out length) && length >= 0 && length <= 30)
//            {
//                break; // Inmatningen är en giltig siffra, avbryt loopen
//            }

//            Console.WriteLine("Invalid input! Please enter a valid number (0,0 to 30,0).");
//            Console.WriteLine();
//        }
//        tempVehicle = (new Boat { RegistrationNumber = regNo, Color = color, NumberOfWheels = numberOfWheels, Length = length });
//    }


//    Console.WriteLine();
//    Console.WriteLine($"Vehicle with registration number {tempVehicle.RegistrationNumber} parked successfully in the garage.");
//    Console.WriteLine();
//    Console.WriteLine("Press any key to continue.");
//    Console.ReadKey();

//    return tempVehicle;


//}




// Räknar hur många lediga platser som finns kvar





//public void SearchByRegNo()
//{
//    Console.Clear();
//    Console.Write("Write the Reg No of the vehicle you are looking for. ");
//    //string regNoSearchParameter = Console.ReadLine()?.Trim() ?? "";
//    Console.WriteLine();


//    // Registreringsnummer
//    string regNoSearchParameter = "";
//    string pattern = @"^[A-Z]{3}\d{3}$"; // Kräver exakt 3 bokstäver och 3 siffror

//    while (true)
//    {
//        Console.Write("Please enter the registration number of vehicle in the format of 3 letters followed by 3 numbers: ");
//        regNoSearchParameter = Console.ReadLine()?.Trim().ToUpper() ?? "";
//        Console.WriteLine();

//        // Godkänn direkt om användaren bara tryckte på Enter (tom sträng)
//        if (string.IsNullOrEmpty(regNoSearchParameter))
//        {
//            break;
//        }

//        if (Regex.IsMatch(regNoSearchParameter, pattern))
//        {
//            break; // Om formatet är korrekt, avbryt loopen och gå vidare
//        }

//        Console.WriteLine("\"Invalid format! Please try again (e.g., ABC123).");
//        Console.WriteLine();
//    }





//    T[] successfulResults = new T[vehicles.Length]; // Intern array som lagrar alla fordon som matchar sökkriterierna.

//    int counter = 0;

//    foreach (var vehicle in vehicles)
//    {
//        if (vehicle == null) continue; // Hoppa över tomma fordon för att undvika krasch.

//        bool isRegNoMatch = string.IsNullOrEmpty(regNoSearchParameter) || vehicle.RegistrationNumber?.ToLower() == regNoSearchParameter.ToLower();

//        if (isRegNoMatch)
//        {
//            successfulResults[counter] = vehicle;
//            counter++;
//        }

//    }

//    Console.WriteLine("Search results:");

//    counter = 0;
//    foreach (var item in successfulResults) // Skriv ut alla matchande fordon.
//    {
//        if (item == null) continue; // Hoppa över tomma fordon för att undvika krasch.

//        Console.Write($"Type: {item.GetType().Name}, Reg No: {item.RegistrationNumber}, Color: {item.Color}, Wheels: {item.NumberOfWheels}");

//        if (item is Car c) Console.Write($", Fuel: {c.FuelType}");
//        else if (item is Motorcycle m) Console.Write($", Engine size: {m.CylinderVolume}cc");
//        else if (item is Airplane a) Console.Write($", Engines: {a.NumberOfEngines}");
//        else if (item is Bus b) Console.Write($", Seats: {b.NumberOfSeats}");
//        else if (item is Boat bo) Console.Write($", Length: {bo.Length}m");

//        counter++;
//        Console.WriteLine();

//    }

//    if (counter == 0)
//    {
//        Console.WriteLine("No vehicles matched your search.");
//    }

//    Console.WriteLine();
//    Console.WriteLine($"Number of search results: {counter}");

//    Console.WriteLine();
//    Console.WriteLine("Press any key to continue.");
//    Console.ReadKey();
//}







//public void SearchByProperties()
//{
//    Console.Clear();

//    // Typ av fordon
//    string typeSearchParameter = "";
//    string[] allowedTypes = { "Car", "Motorcycle", "Airplane", "Bus", "Boat" };

//    while (true)
//    {
//        Console.Write("Write the type of vehicle (Car, Motorcycle, Airplane, Bus, Boat) you are looking for (leave blank if vehicle type does not matter): ");
//        typeSearchParameter = Console.ReadLine()?.Trim() ?? "";
//        Console.WriteLine();

//        // Godkänn direkt om användaren bara tryckte på Enter (tom sträng)
//        if (string.IsNullOrEmpty(typeSearchParameter))
//        {
//            break;
//        }

//        if (allowedTypes.Any(t => t.ToLower() == typeSearchParameter.ToLower()))
//        {
//            typeSearchParameter = capitalFirstLetter(typeSearchParameter);
//            break;
//        }

//        Console.WriteLine("Invalid vehicle type! You must choose a type from the list.");
//        Console.WriteLine();
//    }



//    // Färg 
//    string colorSearchParameter = "";
//    string[] allowedColors = { "Black", "Red", "Blue", "White", "Yellow" };

//    while (true)
//    {
//        Console.Write("Write the color (Black, Red, Blue, White, Yellow) you are looking for (leave blank if color does not matter): ");
//        //Console.Write("Enter a color of vehicle (Black, Red, Blue, White, Yellow): ");
//        colorSearchParameter = Console.ReadLine()?.Trim() ?? "";
//        Console.WriteLine();

//        // Godkänn direkt om användaren bara tryckte på Enter (tom sträng)
//        if (string.IsNullOrEmpty(colorSearchParameter))
//        {
//            break;
//        }

//        if (allowedColors.Any(c => c.ToLower() == colorSearchParameter.ToLower()))
//        {
//            colorSearchParameter = capitalFirstLetter(colorSearchParameter);
//            //color = char.ToUpper(color[0]) + color.Substring(1).ToLower();
//            break;
//        }

//        Console.WriteLine("Invalid color! You must choose a color from the list.");
//        Console.WriteLine();
//    }

//    //string numberOfWheelsSearchParameter = Console.ReadLine()?.Trim() ?? "";
//    Console.WriteLine();

//    // Antal hjul 
//    int numberOfWheelsSearchParameter = 0;
//    bool userEnteredEmptyWheelSearch = false;

//    while (true)
//    {
//        Console.Write("Write the number of wheels you are looking for on the vehicle (0 to 8) (leave blank if the number of wheels does not matter): ");
//        string input = Console.ReadLine()?.Trim() ?? "";
//        Console.WriteLine();

//        // Godkänn direkt om användaren bara tryckte på Enter (tom sträng)
//        if (string.IsNullOrEmpty(input))
//        {
//            userEnteredEmptyWheelSearch = true;
//            break;
//        }

//        // TryParse returnerar true om texten är en giltig siffra
//        if (int.TryParse(input, out numberOfWheelsSearchParameter) && numberOfWheelsSearchParameter >= 0 && numberOfWheelsSearchParameter <= 8)
//        {
//            break; // Inmatningen är en giltig siffra, avbryt loopen
//        }

//        Console.WriteLine("Invalid input! Please enter a valid number (0 to 8).");
//        Console.WriteLine();
//    }







//    T[] successfulResults = new T[vehicles.Length]; // Intern array som lagrar alla fordon som matchar sökkriterierna.

//    int counter = 0;

//    foreach (var vehicle in vehicles)
//    {
//        if (vehicle == null) continue; // Hoppa över tomma fordon för att undvika krasch.

//        bool isTypeMatch = string.IsNullOrEmpty(typeSearchParameter) || vehicle.GetType().Name.ToLower() == typeSearchParameter.ToLower();
//        bool isColorMatch = string.IsNullOrEmpty(colorSearchParameter) || vehicle.Color?.ToLower() == colorSearchParameter.ToLower();
//        bool isNumberOfWheelsMatch = userEnteredEmptyWheelSearch || vehicle.NumberOfWheels == numberOfWheelsSearchParameter;

//        if (isTypeMatch && isColorMatch && isNumberOfWheelsMatch)
//        {
//            successfulResults[counter] = vehicle;
//            counter++;
//        }
//    }

//    Console.WriteLine("Search results:");

//    counter = 0;
//    foreach (var item in successfulResults) // Skriv ut alla matchande fordon.
//    {
//        if (item == null) continue; // Hoppa över tomma fordon för att undvika krasch.

//        Console.Write($"Type: {item.GetType().Name}, Reg No: {item.RegistrationNumber}, Color: {item.Color}, Wheels: {item.NumberOfWheels}");

//        if (item is Car c) Console.Write($", Fuel: {c.FuelType}");
//        else if (item is Motorcycle m) Console.Write($", Engine size: {m.CylinderVolume}cc");
//        else if (item is Airplane a) Console.Write($", Engines: {a.NumberOfEngines}");
//        else if (item is Bus b) Console.Write($", Seats: {b.NumberOfSeats}");
//        else if (item is Boat bo) Console.Write($", Length: {bo.Length}m");

//        counter++;
//        Console.WriteLine();

//    }

//    if (counter == 0)
//    {
//        Console.WriteLine("No vehicles matched your search.");
//        Console.WriteLine();
//        Console.WriteLine("Press any key to continue.");
//        Console.ReadKey();
//    }

//    Console.WriteLine();
//    Console.WriteLine($"Number of search results: {counter}");
//    Console.WriteLine();
//    Console.WriteLine("Press any key to continue.");
//    Console.ReadKey();
//}









//public bool RemoveVehicle()
//{
//    Console.Clear();
//    Console.Write("Write the Registration number of the vehicle you are looking to remove.");
//    Console.WriteLine();

//    // Registreringsnummer
//    string regNoSearchParameter = "";
//    string pattern = @"^[A-Z]{3}\d{3}$"; // Kräver exakt 3 bokstäver och 3 siffror

//    while (true)
//    {
//        Console.Write("Please enter the registration number of vehicle in the format of 3 letters followed by 3 numbers: ");
//        regNoSearchParameter = Console.ReadLine()?.Trim().ToUpper() ?? "";

//        if (Regex.IsMatch(regNoSearchParameter, pattern))
//        {
//            break; // Om formatet är korrekt, avbryt loopen och gå vidare
//        }

//        Console.WriteLine("\"Invalid format! Please try again (e.g., ABC123).");
//        Console.WriteLine();
//    }

//    int counter = 0;

//    for (int i = 0; i < vehicles.Length; i++)
//    {
//        if (vehicles[i] == null) continue; // Hoppa över tomma fordon för att undvika krasch.

//        bool isRegNoMatch = vehicles[i].RegistrationNumber?.ToLower() == regNoSearchParameter.ToLower();

//        if (isRegNoMatch)
//        {
//            vehicles[i] = null; // Ta bort fordonet
//            counter++; // Räkna antalet borttagna fordon
//            Console.WriteLine($"Vehicle with Registration number {regNoSearchParameter} was removed");
//            Console.WriteLine();
//            Console.WriteLine("Press any key to continue.");
//            Console.ReadKey();
//        }
//    }

//    if (counter > 0)
//    {
//        return true;
//    }
//    else
//    {
//        Console.WriteLine($"No vehicles with Registration number {regNoSearchParameter} was found and thus no vehicles were removed");
//        Console.WriteLine();
//        Console.WriteLine("Press any key to continue.");
//        Console.ReadKey();
//        return false;
//    }
//}










//public void ListVehicles()
//{
//    Console.Clear();
//    Console.WriteLine("Vehicles in the garage:");
//    Console.WriteLine();

//    int counter = 0;
//    foreach (var vehicle in vehicles)
//    {
//        if (vehicle == null) continue; // Hoppa över tomma parkeringsplatser för att undvika krasch

//        counter++;


//        Console.Write($"No: {counter} Type: {vehicle.GetType().Name}, Reg No: {vehicle.RegistrationNumber}, Color: {vehicle.Color}, Wheels: {vehicle.NumberOfWheels}");

//        if (vehicle is Car c) Console.Write($", Fuel: {c.FuelType}");
//        else if (vehicle is Motorcycle m) Console.Write($", Engine size: {m.CylinderVolume}cc");
//        else if (vehicle is Airplane a) Console.Write($", Engines: {a.NumberOfEngines}");
//        else if (vehicle is Bus b) Console.Write($", Seats: {b.NumberOfSeats}");
//        else if (vehicle is Boat bo) Console.Write($", Length: {bo.Length}m");
//        Console.WriteLine();
//    }

//    if (counter == 0)
//    {
//        Console.WriteLine("No vehicles in the garage.");
//    }

//    Console.WriteLine();
//    Console.WriteLine("Press any key to continue.");
//    Console.ReadKey();
//}








//public void ListVehicleTypesCount()
//{
//    var typeCounts = new Dictionary<string, int>();
//    int counter = 0;

//    // för varje vehicle i garaget: kolla vilken typ den har. lägg till typen i Dictionaryt (om den redan fanns där, öka bara med 1)
//    foreach (var vehicle in vehicles)
//    {
//        if (vehicle == null) continue; // Hoppa över tomma parkeringsplatser för att undvika krasch

//        counter++;
//        string type = vehicle.GetType().Name;

//        //if typen redan finns
//        //        plussa på

//        if (typeCounts.TryGetValue(type, out int value))
//        {
//            typeCounts[type] = value + 1;
//        }
//        else
//        {
//            //annars
//            //    skapa ny

//            typeCounts[type] = 1;

//        }
//    }


//    Console.Clear();

//    foreach (KeyValuePair<string, int> item in typeCounts)
//    {
//        Console.WriteLine($"Amount of vehicle type {item.Key}: {item.Value}");
//    }


//    if (counter == 0)
//    {
//        Console.WriteLine("No vehicles in the garage.");
//    }

//    Console.WriteLine();
//    Console.WriteLine("Press any key to continue.");
//    Console.ReadKey();
//}
