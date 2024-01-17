namespace CarConnectUnitTests
{
    /// <summary>
    /// Unit tests for the VehicleService class.
    /// </summary>
    [TestFixture]
    public class VehicleServicesTests
    {
        private IVehicleService _vehicleService;

        /// <summary>
        /// Setup method to initialize necessary resources before each test.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Set the APP_CONFIG_FILE for the test environment.
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", String.Format("{0}\\App.config", AppDomain.CurrentDomain.BaseDirectory));

            // Instantiate the IVehicleService implementation for testing.
            _vehicleService = new VehicleService();
        }


        /// <summary>
        /// Test for GetAllVehicles method to ensure it returns a non-null result.
        /// </summary>
        [Test]
        public void GetAllVehiclesTestWhenNotNull()
        {
            Assert.IsNotNull(_vehicleService.GetAllVehicles());
        }


        /// <summary>
        /// Test for GetAvailableVehicles method to ensure it returns a non-null result.
        /// </summary>
        [Test]
        public void GetAvailableVehiclesTestWhenNotNull()
        {
            Assert.IsNotNull(_vehicleService.GetAvailableVehicles());
        }


        /// <summary>
        /// Test for UpdateVehicle method to ensure it returns a non-null result.
        /// </summary>
        [Test]
        public void UpdateVehiclesTestWhenNotNull()
        {
            // Create a sample vehicle for testing.
            Vehicle vehicle = new Vehicle() { Availability = true, DailyRate = 50, RegistrationNumber = "ts36l5555" };

            // Assert that the result is not null.
            Assert.IsNotNull(_vehicleService.UpdateVehicle(vehicle));
        }


        /// <summary>
        /// Test for AddVehicle method to ensure it returns a non-null result.
        /// </summary>
        [Test]
        public void AddVehiclesTestWhenNotNull()
        {
            // Create a sample vehicle for testing.
            Vehicle vehicle = new Vehicle() { Model = "Rohan", Make = "Bhaskar", Year = 2002, Color = "Blue", Availability = true, DailyRate = 100, RegistrationNumber = "JKL023" };

            // Assert that the result is not null.
            Assert.IsNotNull(_vehicleService.AddVehicle(vehicle));
        }
    }
}