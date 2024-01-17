namespace CarConnectUnitTests
{
    /// <summary>
    /// Unit tests for the CustomerService class.
    /// </summary>
    [TestFixture]
    public class CustomerServicesTests
    {
        private ICustomerService _customerService;

        /// <summary>
        /// Setup method to initialize necessary resources before each test.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Set the APP_CONFIG_FILE for the test environment.
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", String.Format("{0}\\App.config", AppDomain.CurrentDomain.BaseDirectory));

            // Instantiate the ICustomerService implementation for testing.
            _customerService = new CustomerService();
        }


        /// <summary>
        /// Test for Authenticate method when authentication fails.
        /// </summary>
        [Test]
        public void CustomerAuthenticationTestWhenFail()
        {
            // Assert that the authentication fails for the provided credentials.
            Assert.IsFalse(_customerService.Authenticate("1", "fcgvha"));
        }

        /// <summary>
        /// Test for UpdateCustomer method to ensure it returns a non-null result.
        /// </summary>
        [Test]
        public void UpdateCustomerTestWhenNotNull()
        {
            // Create a sample customer for testing.
            Customer updateCustomer = new Customer() { FirstName = "Rohan", LastName = "Bhaskar", PhoneNumber = "1234567890", UserName = "vasanth", Address = "Hyderabad" };

            // Assert that the result is not null.
            Assert.IsNotNull(_customerService.UpdateCustomer(updateCustomer));
        }
    }
}
