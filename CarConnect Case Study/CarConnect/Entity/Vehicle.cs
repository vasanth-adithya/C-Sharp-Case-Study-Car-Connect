using System.Runtime.InteropServices;

namespace CarConnect.Entity
{
    public class Vehicle
    {
        public int VehicleID { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public string RegistrationNumber { get; set; }
        public bool Availability { get; set; }
        public decimal DailyRate { get; set; }

        public Vehicle() { }

        public Vehicle(string model, string make, int year, string color, string registrationNumber, bool availability, decimal dailyRate, [Optional] int vehicleID)
        {
            Model = model;
            Make = make;
            Year = year;
            Color = color;
            RegistrationNumber = registrationNumber;
            Availability = availability;
            DailyRate = dailyRate;
            VehicleID = vehicleID;
        }
        public override string ToString()
        {
            return $"  VehicleID          :  {VehicleID}\n" +
                   $"  Model              :  {Model}\n" +
                   $"  Make               :  {Make}\n" +
                   $"  Year               :  {Year}\n" +
                   $"  Color              :  {Color}\n" +
                   $"  RegistrationNumber :  {RegistrationNumber}\n" +
                   $"  Availability       :  {Availability}\n" +
                   $"  DailyRate          :  {DailyRate}";
        }
    }
}
