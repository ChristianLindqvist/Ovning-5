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