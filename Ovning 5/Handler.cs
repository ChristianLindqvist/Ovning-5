public class Handler
{
    private Garage<IVehicle> myGarage;

    // Konstruktor där garaget skapas med den storlek användaren valt
    public Handler()
    {
        int capacity = 0;

        while (true)
        {
            Console.Write("Please enter the capacity of the garage (0 to 100): ");
            string input = Console.ReadLine()?.Trim() ?? "";

            // TryParse returnerar true om texten är en giltig siffra
            if (int.TryParse(input, out capacity) && capacity >= 0 && capacity <= 100)
            {
                break; // Inmatningen är en giltig siffra, avbryt loopen
            }

            Console.WriteLine("Invalid input! Please enter a valid number (0 to 100).");
            Console.WriteLine();
        }

        myGarage = new Garage<IVehicle>(capacity);
    }

    public void SeedGarage()
    {
        myGarage.AddVehicle(new Car { RegistrationNumber = "ABC123", Color = "Black", NumberOfWheels = 4, FuelType = "Gasoline" });
        myGarage.AddVehicle(new Motorcycle { RegistrationNumber = "XYZ789", Color = "Red", NumberOfWheels = 2, CylinderVolume = 600 });
        myGarage.AddVehicle(new Airplane { RegistrationNumber = "FLY001", Color = "White", NumberOfWheels = 3, NumberOfEngines = 2 });
        myGarage.AddVehicle(new Bus { RegistrationNumber = "BUS456", Color = "Yellow", NumberOfWheels = 6, NumberOfSeats = 45 });
        myGarage.AddVehicle(new Boat { RegistrationNumber = "SEA777", Color = "Blue", NumberOfWheels = 0, Length = 12.5 });
    }


    public int Capacity
    {
        get { return myGarage.Capacity; }
    }

    public bool RemoveVehicle(string regNo)
    {
        return myGarage.RemoveVehicle(regNo);
    }

    public Dictionary<string, int> GetVehicleTypesCount()
    {
        return myGarage.GetVehicleTypesCount();
    }


    public IEnumerable<IVehicle> GetVehicles()
    {
        return myGarage;
    }

    public IEnumerable<IVehicle> SearchByProperties(string type, string color, int wheels, bool emptyWheelSearch)
    {
        return myGarage.SearchByProperties(type, color, wheels, emptyWheelSearch);
    }

    public int GetFreeSpotsCount()
    {
        return myGarage.GetFreeSpotsCount();
    }

    public bool RegistrationNumberExists(string regNo)
    {
        return myGarage.RegistrationNumberExists(regNo);
    }

    public bool AddVehicle(IVehicle vehicle)
    {
        return myGarage.AddVehicle(vehicle);
    }

    public IEnumerable<IVehicle> SearchByRegNo(string regNo)
    {
        return myGarage.SearchByRegNo(regNo);
    }
}
