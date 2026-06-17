public interface IGarage<T> where T : class, IVehicle
{
    bool AddVehicle(T vehicle);
    IEnumerator<T> GetEnumerator();
    //void ListVehicleTypesCount();
    bool RemoveVehicle(string regNo);
    IEnumerable<T> SearchByProperties(string type, string color, int wheels, bool emptyWheelSearch);
    int GetFreeSpotsCount();
    bool RegistrationNumberExists(string regNo);
    IEnumerable<T> SearchByRegNo(string regNo);
    Dictionary<string, int> GetVehicleTypesCount();
}