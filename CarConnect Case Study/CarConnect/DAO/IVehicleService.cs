using System.Collections.Generic;
using CarConnect.Entity;

namespace CarConnect.DAO
{
    public interface IVehicleService
    {
        List<Vehicle> GetAllVehicles();

        void GetVehicleById(int vehicleId);

        List<Vehicle> GetAvailableVehicles();

        string AddVehicle(Vehicle vehicleData);

        string UpdateVehicle(Vehicle vehicleData);

        string RemoveVehicle(int vehicleId);
    }
}
