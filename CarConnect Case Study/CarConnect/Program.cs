using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using CarConnect.DAO;
using CarConnect.Entity;
using CarConnect.Exceptions;

namespace CarConnect
{
    internal class Program
    {
        /// <summary>
        /// The main entry point for the Car Connect application.
        /// </summary>
        static void Main(string[] args)
        {
            // Main application loop
            while (true)
            {
                Console.Clear();

                // Display the home page menu
                Console.WriteLine("\t\t\t\tCar Connect Home Page");
                Console.WriteLine("1. Customer");
                Console.WriteLine("2. Admin");
                Console.WriteLine("3. Vehicle");
                Console.WriteLine("4. Reservation");
                Console.WriteLine("0. exit");
                Console.Write("\nPlease enter your choice : ");

                // Read user input for menu selection
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    // Navigate to the Customer home screen
                    case "1":
                        CustomerHomeScreen();
                        break;

                    // Navigate to the Admin home screen
                    case "2":
                        AdminHomeScreen();
                        break;

                    // Navigate to the Vehicle menu
                    case "3":
                        VehicleMenu();
                        break;

                    // Navigate to the Reservation menu
                    case "4":
                        ReservationMenu();
                        break;

                    // Exit the application
                    case "0":
                        Exit();
                        break;

                    // Display an error message for invalid choices
                    default:
                        Console.Write("Invalid choice. Please enter a number as displayed.");
                        Thread.Sleep(2000);
                        break;
                }
            }
        }



        /// <summary>
        /// Displays the Customer Home Screen and provides options for login, registration, and returning to the main menu.
        /// </summary>
        public static void CustomerHomeScreen()
        {
            // Flag to control return to the main menu
            bool customerHomeScreenReturnToMainMenu = false;

            // Main loop for the Customer Home Screen
            while (!customerHomeScreenReturnToMainMenu)
            {
                // Instantiate the CustomerService for handling customer-related operations
                ICustomerService customerService;
                customerService = new CustomerService();

                // Clear the console and display the Customer Home Screen menu
                Console.Clear();
                Console.WriteLine("\t\t\t\tCustomer Home Screen");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Return to Main Menu");
                Console.WriteLine("0. Exit");
                Console.Write("\nPlease enter your choice : ");

                // Read user input for menu selection
                string customerHomeScreenChoice = Console.ReadLine();
                Console.WriteLine();

                // Process user choice using a switch statement
                switch (customerHomeScreenChoice)
                {
                    // Initiate customer authentication process
                    case "1":
                        AuthenticateCustomer(customerService);
                        break;

                    // Initiate customer registration process
                    case "2":
                        RegisterCustomer(customerService);
                        break;

                    // Set flag to return to the main menu
                    case "3":
                        customerHomeScreenReturnToMainMenu = true;
                        break;

                    // Exit the application
                    case "0":
                        Exit();
                        break;

                    // Display an error message for invalid choices
                    default:
                        Console.Write("Invalid choice. Please enter a number as displayed.");
                        Thread.Sleep(2000);
                        break;
                }
            }
        }

        /// <summary>
        /// Initiates the customer authentication process by taking the username and password from the user.
        /// If authentication is successful, navigates to the Customer Menu.
        /// </summary>
        /// <param name="customerService">The customer service instance for handling customer-related operations.</param>
        public static void AuthenticateCustomer(ICustomerService customerService)
        {
            try
            {
                // Clear the console and display the Customer Login Page
                Console.Clear();
                Console.WriteLine("\t\t\t\tCustomer Login Page");

                // Read user input for username
                Console.Write("Enter Username: ");
                string uname = Console.ReadLine();
                Console.WriteLine();

                // Read user input for password
                Console.Write("Enter Password: ");
                string pwd = Console.ReadLine();
                Console.WriteLine();

                // Perform customer authentication
                bool AuthToken = customerService.Authenticate(uname, pwd);

                // If authentication is successful, navigate to the Customer Menu
                if (AuthToken)
                {
                    CustomerMenu();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and display error messages
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }

        /// <summary>
        /// Displays the Customer Menu, allowing users to perform various operations related to customer management.
        /// </summary>
        public static void CustomerMenu()
        {
            // Initialize the customer service
            ICustomerService customerService;
            customerService = new CustomerService();

            // Flag to control returning to the main menu
            bool customerReturnToMainMenu = false;
            while (!customerReturnToMainMenu)
            {
                // Clear the console and display the Customer Menu options
                Console.Clear();
                Console.WriteLine("\t\t\t\tCustomer Menu");
                Console.WriteLine("1. Get all customers");
                Console.WriteLine("2. Get customer details using customer ID");
                Console.WriteLine("3. Get customer details using customer user-name");
                Console.WriteLine("4. Register a new customer");
                Console.WriteLine("5. Update Customer");
                Console.WriteLine("6. Delete an existing customer data");
                Console.WriteLine("7. Logout");
                Console.WriteLine("0. Exit");
                Console.Write("\nPlease enter your choice : ");

                // Read user input for menu choice
                string customerMenuChoice = Console.ReadLine();
                Console.WriteLine();

                // Process user choice and perform corresponding actions
                switch (customerMenuChoice)
                {
                    // Case for getting all customers
                    case "1":
                        GetAllCustomers(customerService);
                        break;

                    // Case for getting customer details by ID
                    case "2":
                        GetCustomerById(customerService);
                        break;

                    // Case for getting customer details by username
                    case "3":
                        GetCustomerByUserName(customerService);
                        break;

                    // Case for registering a new customer
                    case "4":
                        RegisterCustomer(customerService);
                        break;

                    // Case for updating customer details
                    case "5":
                        UpdateCustomer(customerService);
                        break;

                    // Case for deleting an existing customer
                    case "6":
                        DeleteCustomer(customerService);
                        break;

                    // Case for logging out
                    case "7":
                        Console.Write("Logged out successfully.");
                        Thread.Sleep(2000);
                        customerReturnToMainMenu = true;
                        break;

                    // Case for exiting the application
                    case "0":
                        Exit();
                        break;

                    // Default case for handling invalid choices
                    default:
                        Console.Write("Invalid choice. Please enter a number as displayed.");
                        Thread.Sleep(2000);
                        break;
                }
            }
        }

        /// <summary>
        /// Retrieves and displays details of all customers in the system.
        /// </summary>
        /// <param name="customerService">The customer service instance used for data retrieval.</param>
        public static void GetAllCustomers(ICustomerService customerService)
        {
            try
            {
                // Clear the console and display the heading
                Console.Clear();
                Console.WriteLine("\t\t\t\tDeatils of All CUSTOMERS\n");

                // Retrieve a list of all customers from the service
                List<Customer> customers = customerService.GetAllCustomers();

                // Check if there are any customers
                if (customers.Count > 0)
                {
                    // Display details for each customer
                    foreach (Customer customer in customers)
                    {
                        Console.WriteLine(customer);
                        Console.WriteLine();
                    }

                    // Prompt to return to the previous menu
                    Console.Write("\n\n\n\nPress any key to return to previous menu...");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and return to the previous menu
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }

        /// <summary>
        /// Retrieves and displays details of a customer based on the provided customer ID.
        /// </summary>
        /// <param name="customerService">The customer service instance used for data retrieval.</param>
        public static void GetCustomerById(ICustomerService customerService)
        {
            try
            {
                // Clear the console and display the heading
                Console.Clear();
                Console.WriteLine("\t\t\t\tCustomer Details using Customer ID\n");

                // Prompt the user to enter the ID of the customer
                Console.Write("Enter the ID of the customer you want to retrive the data: ");

                // Parse the user input as an integer
                int customerId = int.Parse(Console.ReadLine());
                Console.WriteLine();

                // Retrieve and display details of the customer by ID
                customerService.GetCustomerById(customerId);
            }
            catch (Exception ex)
            {
                // Handle exceptions related to invalid input and return to the previous menu
                Console.WriteLine();
                Console.WriteLine(ex.Message + " The ID should be a numeric value.");
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }

        /// <summary>
        /// Retrieves and displays details of a customer based on the provided customer username.
        /// </summary>
        /// <param name="customerService">The customer service instance used for data retrieval.</param>
        public static void GetCustomerByUserName(ICustomerService customerService)
        {
            try
            {
                // Clear the console and display the heading
                Console.Clear();
                Console.WriteLine("\t\t\t\tCustomer Details using Customer Username\n");

                // Prompt the user to enter the username of the customer
                Console.Write("Enter the username of the customer you want to retrive the data: ");

                // Read the user input for the customer username
                string customerName = Console.ReadLine();
                Console.WriteLine();

                // Retrieve and display details of the customer by username
                customerService.GetCustomerByUserName(customerName);
            }
            catch (Exception ex)
            {
                // Handle exceptions related to data retrieval and return to the previous menu
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }

        /// <summary>
        /// Registers a new customer by collecting and validating user input for various details.
        /// </summary>
        /// <param name="customerService">The customer service instance used for customer registration.</param>
        public static void RegisterCustomer(ICustomerService customerService)
        {
            try
            {
                // Clear the console and create a new customer instance for registration
                Console.Clear();
                Customer registerCustomer = new Customer();

                // Prompt the user to enter the first name
                Console.WriteLine("\t\t\t\tNew User Regesitration");
                Console.WriteLine("Enter First Name: ");
                registerCustomer.FirstName = Console.ReadLine();

                // Validate the first name using a regular expression
                if (Regex.IsMatch(registerCustomer.FirstName, "^[a-zA-Z ]+$"))
                {
                    // Prompt the user to enter the last name
                    Console.WriteLine("Enter Last Name: ");
                    registerCustomer.LastName = Console.ReadLine();

                    // Validate the last name using a regular expression
                    if (Regex.IsMatch(registerCustomer.LastName, "^[a-zA-Z ]+$"))
                    {
                        // Prompt the user to enter the phone number
                        Console.WriteLine("Enter Phone Number: ");
                        registerCustomer.PhoneNumber = Console.ReadLine();

                        // Validate the phone number using a regular expression and length check
                        if (Regex.IsMatch(registerCustomer.PhoneNumber, @"^\d+$"))
                        {
                            if (registerCustomer.PhoneNumber.Length == 10)
                            {
                                // Prompt the user to enter the address
                                Console.WriteLine("Enter Address: ");
                                registerCustomer.Address = Console.ReadLine();

                                // Prompt the user to enter the email address
                                Console.WriteLine("Enter Email-id: ");
                                registerCustomer.Email = Console.ReadLine();

                                // Validate the email address
                                if (registerCustomer.Email.Contains("@"))
                                {
                                    // Prompt the user to enter the username
                                    Console.WriteLine("Enter Username: ");
                                    registerCustomer.UserName = Console.ReadLine();

                                    // Prompt the user to enter the password
                                    Console.WriteLine("Password");
                                    registerCustomer.Password = Console.ReadLine();

                                    // Set the registration date to the current date
                                    registerCustomer.RegistrationDate = DateTime.Now.Date;
                                    Console.WriteLine();

                                    // Register the customer and display the result message
                                    string res = customerService.RegisterCustomer(registerCustomer);
                                    if (res != null)
                                    {
                                        Console.WriteLine(res);
                                        Console.Write("\nReturning to previous menu...");
                                        Thread.Sleep(2000);
                                    }
                                }
                                else throw new InvalidInputException("Invalid E-mail address.");
                            }
                            else throw new InvalidInputException("Phone number should be of 10 digits.");
                        }
                        else throw new InvalidInputException("Phone number should only contain digits.");
                    }
                    else throw new InvalidInputException("Cannot use special characters and numbers.");
                }
                else throw new InvalidInputException("Cannot use special characters and numbers.");
            }
            catch (InvalidInputException ex)
            {
                // Handle exceptions related to invalid input and return to the previous menu
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions and display the error message
                Console.WriteLine();
                Console.Write(ex.Message);
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// Updates customer details by collecting and validating user input for various details.
        /// </summary>
        /// <param name="customerService">The customer service instance used for updating customer details.</param>
        public static void UpdateCustomer(ICustomerService customerService)
        {
            try
            {
                // Clear the console and create a new customer instance for updating details
                Console.Clear();
                Console.WriteLine("\t\t\t\tCustomer Details Updation");
                Customer updateCustomer = new Customer();

                // Prompt the user to enter the new first name
                Console.WriteLine("Enter new First Name: ");
                updateCustomer.FirstName = Console.ReadLine();

                // Validate the new first name using a regular expression
                if (Regex.IsMatch(updateCustomer.FirstName, "^[a-zA-Z ]+$"))
                {
                    // Prompt the user to enter the new last name
                    Console.WriteLine("Enter Last Name: ");
                    updateCustomer.LastName = Console.ReadLine();

                    // Validate the new last name using a regular expression
                    if (Regex.IsMatch(updateCustomer.LastName, "^[a-zA-Z ]+$"))
                    {
                        // Prompt the user to enter the new phone number
                        Console.WriteLine("Enter new phone number: ");
                        updateCustomer.PhoneNumber = Console.ReadLine();

                        // Validate the new phone number using a regular expression and length check
                        if (Regex.IsMatch(updateCustomer.PhoneNumber, @"^\d+$"))
                        {
                            if (updateCustomer.PhoneNumber.Length == 10)
                            {
                                // Prompt the user to enter the new address
                                Console.WriteLine("Enter new Address: ");
                                updateCustomer.Address = Console.ReadLine();

                                // Prompt the user to enter the customer username for which the details need to be updated
                                Console.WriteLine("Enter the customer username for which the details need to be updated: ");
                                updateCustomer.UserName = Console.ReadLine();
                                Console.WriteLine();

                                // Update the customer details and display the result message
                                string updateCustomerRes = customerService.UpdateCustomer(updateCustomer);
                                if (updateCustomerRes != null)
                                {
                                    Console.WriteLine(updateCustomerRes);
                                    Console.Write("\nReturning to previous menu...");
                                    Thread.Sleep(2000);
                                }
                            }
                            else throw new InvalidInputException("Phone number should be of 10 digits.");
                        }
                        else throw new InvalidInputException("Phone number should only contain digits.");
                    }
                    else throw new InvalidInputException("Cannot use special characters and numbers.");
                }
                else throw new InvalidInputException("Cannot use special characters and numbers.");
            }
            catch (InvalidInputException ex)
            {
                // Handle exceptions related to invalid input and return to the previous menu
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions and display the error message
                Console.WriteLine();
                Console.Write(ex.Message);
                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// Deletes a customer by collecting and validating user input for the customer ID.
        /// </summary>
        /// <param name="customerService">The customer service instance used for customer deletion.</param>
        public static void DeleteCustomer(ICustomerService customerService)
        {
            try
            {
                // Clear the console and prompt the user for the customer ID to be deleted
                Console.Clear();
                Console.WriteLine("\t\t\t\tCustomer Details Deletion\n");
                Console.Write("Enter the ID of the customer you want to delete: ");
                int deleteCustomerID = int.Parse(Console.ReadLine());
                Console.WriteLine();

                // Delete the customer using the provided customer ID and display the result message
                string deleteCustomerRes = customerService.DeleteCustomer(deleteCustomerID);
                if (deleteCustomerRes != null)
                {
                    Console.WriteLine(deleteCustomerRes);
                    Console.Write("\nReturning to previous menu...");
                    Thread.Sleep(3500);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions related to invalid input and return to the previous menu
                Console.WriteLine();
                Console.WriteLine(ex.Message + " The ID should be a numeric value.");
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }



        /// <summary>
        /// Displays the admin home screen menu, allowing login, registration, and returning to the main menu.
        /// </summary>
        public static void AdminHomeScreen()
        {
            try
            {
                // Initialize admin service
                IAdminService adminService;
                adminService = new AdminService();

                // Loop to display admin home screen until the user returns to the main menu
                bool adminHomeScreenReturnToMainMenu = false;
                while (!adminHomeScreenReturnToMainMenu)
                {
                    // Clear the console and display admin home screen options
                    Console.Clear();
                    Console.WriteLine("\t\t\t\tAdmin Home Screen");
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Register");
                    Console.WriteLine("3. Return to Main Menu");
                    Console.WriteLine("0. Exit");
                    Console.Write("\nPlease enter your choice : ");

                    // Collect and process user choice
                    string adminHomeScreenChoice = Console.ReadLine();
                    Console.WriteLine();

                    // Switch based on user choice
                    switch (adminHomeScreenChoice)
                    {
                        // Option to log in as an admin
                        case "1":
                            AuthenticateAdmin(adminService);
                            break;

                        // Option to register a new admin
                        case "2":
                            RegisterAdmin(adminService);
                            break;

                        // Return to the main menu
                        case "3":
                            adminHomeScreenReturnToMainMenu = true;
                            break;

                        // Exit the program
                        case "0":
                            Exit();
                            break;

                        // Display error message for invalid choices
                        default:
                            Console.Write("Invalid choice. Please enter a number as displayed.");
                            Thread.Sleep(2000);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions and return to the main menu
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.Write("\nReturning to the main menu...");
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// Displays the admin login page, prompting for admin username and password.
        /// Authenticates the admin and opens the admin menu upon successful login.
        /// </summary>
        /// <param name="adminService">The service for admin-related functionalities.</param>
        public static void AuthenticateAdmin(IAdminService adminService)
        {
            try
            {
                // Clear the console and display the admin login page
                Console.Clear();
                Console.WriteLine("\t\t\t\tAdmin Login Page");
                Console.Write("Enter Admin Username: ");
                string uname = Console.ReadLine();
                Console.WriteLine();
                Console.Write("Enter Password: ");
                string pwd = Console.ReadLine();
                Console.WriteLine();

                // Authenticate admin using the provided credentials
                bool AuthToken = adminService.Authenticate(uname, pwd);

                // If authentication is successful, open the admin menu
                if (AuthToken)
                {
                    AdminMenu();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and return to the previous menu
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }

        /// <summary>
        /// Displays the admin menu and handles admin-related functionalities based on user input.
        /// </summary>
        public static void AdminMenu()
        {
            // Initialize admin service
            IAdminService adminService;
            adminService = new AdminService();

            // Flag to control returning to the main menu
            bool adminReturnToMainMenu = false;
            while (!adminReturnToMainMenu)
            {
                // Clear the console and display the Admin Menu options
                Console.Clear();
                Console.WriteLine("\t\t\t\tAdmin Menu");
                Console.WriteLine("1. Get all admins");
                Console.WriteLine("2. Get admin details using admin ID");
                Console.WriteLine("3. Get admin details using admin user-name");
                Console.WriteLine("4. Register a new admin");
                Console.WriteLine("5. Update admin");
                Console.WriteLine("6. Delete an existing admin data");
                Console.WriteLine("7. Logout");
                Console.WriteLine("0. Exit");
                Console.Write("\nPlease enter your choice : ");

                // Read user input for menu choice
                string adminMenuChoice = Console.ReadLine();
                Console.WriteLine();

                // Process user choice and perform corresponding actions
                switch (adminMenuChoice)
                {
                    // Case for getting all admins
                    case "1":
                        GetAllAdmins(adminService);
                        break;

                    // Case for getting admin details by ID
                    case "2":
                        GetAdminById(adminService);
                        break;

                    // Case for getting admin details by username
                    case "3":
                        GetAdminByUserName(adminService);
                        break;

                    // Case for registering a new admin
                    case "4":
                        RegisterAdmin(adminService);
                        break;

                    // Case for updating admin details
                    case "5":
                        UpdateAdmin(adminService);
                        break;

                    // Case for deleting an existing admin
                    case "6":
                        DeleteAdmin(adminService);
                        break;

                    // Case for logging out
                    case "7":
                        Console.Write("Logged out successfully.");
                        Thread.Sleep(2000);
                        adminReturnToMainMenu = true;
                        break;

                    // Case for exiting the application
                    case "0":
                        Exit();
                        break;

                    // Default case for handling invalid choices
                    default:
                        Console.Write("Invalid choice. Please enter a number as displayed.");
                        Thread.Sleep(2000);
                        break;
                }
            }
        }

        /// <summary>
        /// Retrieves and displays details of all administrators using the provided IAdminService.
        /// </summary>
        /// <param name="adminService">The IAdminService implementation to fetch the administrators.</param>
        public static void GetAllAdmins(IAdminService adminService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Display header for the list of administrators.
                Console.WriteLine("\t\t\t\tDeatils of All ADMINS\n");

                // Retrieve a list of all administrators using the provided service.
                List<Admin> admins = adminService.GetAllAdmins();

                // Check if there are any administrators to display.
                if (admins.Count > 0)
                {
                    // Iterate through each administrator and print details.
                    foreach (Admin admin in admins)
                    {
                        Console.WriteLine(admin);
                        Console.WriteLine();
                    }

                    // Prompt user to return to the previous menu.
                    Console.Write("\n\n\n\nPress any key to return to previous menu...");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions by displaying an error message.
                Console.WriteLine();
                Console.WriteLine(ex.Message);

                // Inform the user about returning to the previous menu and introduce a delay.
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }

        /// <summary>
        /// Retrieves and displays details of an administrator based on the provided Admin ID using the given IAdminService.
        /// </summary>
        /// <param name="adminService">The IAdminService implementation to fetch the administrator details.</param>
        public static void GetAdminById(IAdminService adminService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Display header for the administrator details based on Admin ID.
                Console.WriteLine("\t\t\t\tAdmin Details using Admin ID\n");

                // Prompt the user to enter the ID of the Admin to retrieve the data.
                Console.Write("Enter the ID of the Admin you want to retrive the data: ");
                int adminId = int.Parse(Console.ReadLine());
                Console.WriteLine();

                // Retrieve and display the details of the administrator based on the provided Admin ID.
                adminService.GetAdminById(adminId);
            }
            catch (Exception ex)
            {
                // Handle exceptions by displaying an error message.
                Console.WriteLine();
                Console.WriteLine(ex.Message + " The ID should be a numeric value.");

                // Inform the user about returning to the previous menu and introduce a delay.
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }

        /// <summary>
        /// Retrieves and displays details of an administrator based on the provided Admin Username using the given IAdminService.
        /// </summary>
        /// <param name="adminService">The IAdminService implementation to fetch the administrator details.</param>
        public static void GetAdminByUserName(IAdminService adminService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Display header for the administrator details based on Admin Username.
                Console.WriteLine("\t\t\t\tAdmin Details using Admin Username\n");

                // Prompt the user to enter the username of the admin to retrieve the data.
                Console.Write("Enter the username of the admin you want to retrive the data: ");
                string adminName = Console.ReadLine();
                Console.WriteLine();

                // Retrieve and display the details of the administrator based on the provided Admin Username.
                adminService.GetAdminByUserName(adminName);
            }
            catch (Exception ex)
            {
                // Handle exceptions by displaying an error message.
                Console.WriteLine();
                Console.WriteLine(ex.Message);

                // Inform the user about returning to the previous menu and introduce a delay.
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }

        /// <summary>
        /// Registers a new administrator using the provided IAdminService, prompting the user to input various details.
        /// </summary>
        /// <param name="adminService">The IAdminService implementation to handle the admin registration.</param>
        public static void RegisterAdmin(IAdminService adminService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Create a new Admin instance for registration.
                Admin registerAdmin = new Admin();

                // Display header for new admin registration.
                Console.WriteLine("\t\t\t\tNew Admin Regesitration");

                // Prompt user to input the First Name.
                Console.WriteLine("Enter First Name: ");
                registerAdmin.FirstName = Console.ReadLine();
                if (Regex.IsMatch(registerAdmin.FirstName, "^[a-zA-Z ]+$"))
                {
                    // Prompt user to input the Last Name.
                    Console.WriteLine("Enter Last Name: ");
                    registerAdmin.LastName = Console.ReadLine();
                    if (Regex.IsMatch(registerAdmin.LastName, "^[a-zA-Z ]+$"))
                    {
                        // Prompt user to input the Phone Number.
                        Console.WriteLine("Enter Phone Number: ");
                        registerAdmin.PhoneNumber = Console.ReadLine();
                        if (Regex.IsMatch(registerAdmin.PhoneNumber, @"^\d+$"))
                        {
                            if (registerAdmin.PhoneNumber.Length == 10)
                            {
                                // Prompt user to input the Email.
                                Console.WriteLine("Enter Email-id: ");
                                registerAdmin.Email = Console.ReadLine();
                                if (registerAdmin.Email.Contains("@"))
                                {
                                    // Prompt user to input the Username.
                                    Console.WriteLine("Enter Username: ");
                                    registerAdmin.UserName = Console.ReadLine();

                                    // Prompt user to input the Password.
                                    Console.WriteLine("Password");
                                    registerAdmin.Password = Console.ReadLine();

                                    // Prompt user to input the Role.
                                    Console.WriteLine("Enter Role: ");
                                    registerAdmin.Role = Console.ReadLine();

                                    // Set the Join Date to the current date.
                                    registerAdmin.JoinDate = DateTime.Now.Date;
                                    Console.WriteLine();

                                    // Register the new admin and display the result.
                                    string res = adminService.RegisterAdmin(registerAdmin);
                                    if (res != null)
                                    {
                                        Console.WriteLine(res);
                                        Console.Write("\nReturning to previous menu...");
                                        Thread.Sleep(2000);
                                    }
                                }
                                else throw new InvalidInputException("Invalid E-mail address.");
                            }
                            else throw new InvalidInputException("Phone number should be of 10 digits.");
                        }
                        else throw new InvalidInputException("Phone number should only contain digits.");
                    }
                    else throw new InvalidInputException("Cannot use special characters and numbers.");
                }
                else throw new InvalidInputException("Cannot use special characters and numbers.");
            }
            catch (InvalidInputException ex)
            {
                // Handle specific input-related exceptions by displaying an error message.
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                // Handle other exceptions by displaying an error message.
                Console.WriteLine();
                Console.Write(ex.Message);
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// Updates details of an existing administrator using the provided IAdminService.
        /// </summary>
        /// <param name="adminService">The IAdminService implementation to handle the admin details update.</param>
        public static void UpdateAdmin(IAdminService adminService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Display header for admin details updation.
                Console.WriteLine("\t\t\t\tAdmin Details Updation");

                // Create a new Admin instance for updating details.
                Admin updateAdmin = new Admin();

                // Prompt user to input the new First Name.
                Console.WriteLine("Enter new First Name: ");
                updateAdmin.FirstName = Console.ReadLine();
                if (Regex.IsMatch(updateAdmin.FirstName, "^[a-zA-Z ]+$"))
                {
                    // Prompt user to input the new Last Name.
                    Console.WriteLine("Enter Last Name: ");
                    updateAdmin.LastName = Console.ReadLine();
                    if (Regex.IsMatch(updateAdmin.LastName, "^[a-zA-Z ]+$"))
                    {
                        // Prompt user to input the new Phone Number.
                        Console.WriteLine("Enter new phone number: ");
                        updateAdmin.PhoneNumber = Console.ReadLine();
                        if (Regex.IsMatch(updateAdmin.PhoneNumber, @"^\d+$"))
                        {
                            if (updateAdmin.PhoneNumber.Length == 10)
                            {
                                // Prompt user to input the new Role.
                                Console.WriteLine("Enter new Role: ");
                                updateAdmin.Role = Console.ReadLine();

                                // Prompt user to input the admin username for which details need to be updated.
                                Console.WriteLine("Enter the admin username for which the details need to be updated: ");
                                updateAdmin.UserName = Console.ReadLine();
                                Console.WriteLine();

                                // Update the admin details and display the result.
                                string updateAdminRes = adminService.UpdateAdmin(updateAdmin);
                                if (updateAdminRes != null)
                                {
                                    Console.WriteLine(updateAdminRes);
                                    Console.Write("\nReturning to previous menu...");
                                    Thread.Sleep(2000);
                                }
                            }
                            else throw new InvalidInputException("Phone number should be of 10 digits.");
                        }
                        else throw new InvalidInputException("Phone number should only contain digits.");
                    }
                    else throw new InvalidInputException("Cannot use special characters and numbers.");
                }
                else throw new InvalidInputException("Cannot use special characters and numbers.");
            }
            catch (InvalidInputException ex)
            {
                // Handle specific input-related exceptions by displaying an error message.
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                // Handle other exceptions by displaying an error message.
                Console.WriteLine();
                Console.Write(ex.Message);
                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// Deletes details of an existing administrator based on the provided Admin ID using the given IAdminService.
        /// </summary>
        /// <param name="adminService">The IAdminService implementation to handle the admin details deletion.</param>
        public static void DeleteAdmin(IAdminService adminService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Display header for admin details deletion.
                Console.WriteLine("\t\t\t\tAdmin Details Deletion\n");

                // Prompt user to input the ID of the admin to delete.
                Console.Write("Enter the ID of the admin you want to delete: ");
                int deleteAdminID = int.Parse(Console.ReadLine());
                Console.WriteLine();

                // Delete the admin details based on the provided Admin ID and display the result.
                string deleteAdminRes = adminService.DeleteAdmin(deleteAdminID);
                if (deleteAdminRes != null)
                {
                    Console.WriteLine(deleteAdminRes);
                    Console.Write("\nReturning to previous menu...");
                    Thread.Sleep(3500);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions by displaying an error message.
                Console.WriteLine();
                Console.WriteLine(ex.Message + " The ID should be a numeric value.");

                // Inform the user about returning to the previous menu and introduce a delay.
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }



        /// <summary>
        /// Displays the Vehicle Menu and allows users to perform various operations related to vehicles.
        /// </summary>
        public static void VehicleMenu()
        {
            // Initialize the vehicle service.
            IVehicleService vehicleService;
            vehicleService = new VehicleService();

            // Variable to control returning to the main menu.
            bool vehicleReturnToMainMenu = false;

            // Display the Vehicle Menu in a loop until the user chooses to return to the main menu or exit.
            while (!vehicleReturnToMainMenu)
            {
                Console.Clear();

                // Display the options in the Vehicle Menu.
                Console.WriteLine("\t\t\t\tVehicle Menu");
                Console.WriteLine("1. Get all vehicles");
                Console.WriteLine("2. Get vehicle details using vehicle ID");
                Console.WriteLine("3. Get Available vehicles");
                Console.WriteLine("4. Register a new vehicle");
                Console.WriteLine("5. Update vehicle details");
                Console.WriteLine("6. Delete an existing vehicle data");
                Console.WriteLine("7. Return to Main Menu");
                Console.WriteLine("0. Exit");
                Console.Write("\nPlease enter your choice : ");

                // Read the user's choice.
                string vehicleMenuChoice = Console.ReadLine();
                Console.WriteLine();

                // Process the user's choice with a switch statement.
                switch (vehicleMenuChoice)
                {
                    // Option 1: Get all vehicles.
                    case "1":
                        GetAllVehices(vehicleService);
                        break;

                    // Option 2: Get vehicle details using vehicle ID.
                    case "2":
                        GetVehicleById(vehicleService);
                        break;

                    // Option 3: Get Available vehicles.
                    case "3":
                        GetAvailableVehicles(vehicleService);
                        break;

                    // Option 4: Register a new vehicle.
                    case "4":
                        AddVehicle(vehicleService);
                        break;

                    // Option 5: Update vehicle details.
                    case "5":
                        UpdateVehicle(vehicleService);
                        break;

                    // Option 6: Delete an existing vehicle data.
                    case "6":
                        RemoveVehicle(vehicleService);
                        break;

                    // Option 7: Return to Main Menu.
                    case "7":
                        Console.Write("\nReturning to previous menu...");
                        Thread.Sleep(2000);
                        vehicleReturnToMainMenu = true;
                        break;

                    // Option 0: Exit.
                    case "0":
                        Exit();
                        break;

                    // Handle invalid choices.
                    default:
                        Console.Write("Invalid choice. Please enter a number as displayed.");
                        Thread.Sleep(2000);
                        break;
                }
            }
        }

        /// <summary>
        /// Retrieves and displays details of all vehicles using the provided IVehicleService.
        /// </summary>
        /// <param name="vehicleService">The IVehicleService implementation to fetch the vehicle details.</param>
        public static void GetAllVehices(IVehicleService vehicleService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Display header for the list of vehicles.
                Console.WriteLine("\t\t\t\tDeatils of All VEHICLES\n");

                // Retrieve a list of all vehicles using the provided service.
                List<Vehicle> vehicles = vehicleService.GetAllVehicles();

                // Check if there are any vehicles to display.
                if (vehicles.Count > 0)
                {
                    // Iterate through each vehicle and print details.
                    foreach (Vehicle vehicle in vehicles)
                    {
                        Console.WriteLine(vehicle);
                        Console.WriteLine();
                    }

                    // Prompt user to return to the previous menu.
                    Console.Write("\n\n\n\nPress any key to return to previous menu...");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions by displaying an error message.
                Console.WriteLine();
                Console.WriteLine(ex.Message);

                // Inform the user about returning to the previous menu and introduce a delay.
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }

        /// <summary>
        /// Retrieves and displays details of a vehicle based on the provided Vehicle ID using the given IVehicleService.
        /// </summary>
        /// <param name="vehicleService">The IVehicleService implementation to fetch the vehicle details.</param>
        public static void GetVehicleById(IVehicleService vehicleService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Display header for the vehicle details based on Vehicle ID.
                Console.WriteLine("\t\t\t\tVehicle Details using Vehicle ID\n");

                // Prompt the user to enter the ID of the vehicle to retrieve the data.
                Console.Write("Enter the ID of the vehicle you want to retrive the data: ");
                int vehicleId = int.Parse(Console.ReadLine());
                Console.WriteLine();

                // Retrieve and display the details of the vehicle based on the provided Vehicle ID.
                vehicleService.GetVehicleById(vehicleId);
            }
            catch (Exception ex)
            {
                // Handle exceptions by displaying an error message.
                Console.WriteLine();
                Console.WriteLine(ex.Message + " The ID should be a numeric value.");

                // Inform the user about returning to the previous menu and introduce a delay.
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }

        /// <summary>
        /// Retrieves and displays details of available vehicles using the provided IVehicleService.
        /// </summary>
        /// <param name="vehicleService">The IVehicleService implementation to fetch available vehicle details.</param>
        public static void GetAvailableVehicles(IVehicleService vehicleService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Display header for the list of available vehicles.
                Console.WriteLine("\t\t\t\tDeatils of Available Vehicles\n");

                // Retrieve a list of available vehicles using the provided service.
                List<Vehicle> vehicles = vehicleService.GetAvailableVehicles();

                // Check if there are any available vehicles to display.
                if (vehicles.Count > 0)
                {
                    // Iterate through each available vehicle and print details.
                    foreach (Vehicle vehicle in vehicles)
                    {
                        Console.WriteLine(vehicle);
                        Console.WriteLine();
                    }

                    // Prompt user to return to the previous menu.
                    Console.Write("\n\n\n\nPress any key to return to previous menu...");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions by displaying an error message.
                Console.WriteLine();
                Console.WriteLine(ex.Message);

                // Inform the user about returning to the previous menu and introduce a delay.
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }

        /// <summary>
        /// Registers a new vehicle using the provided IVehicleService, prompting the user to input various details.
        /// </summary>
        /// <param name="vehicleService">The IVehicleService implementation to handle the vehicle registration.</param>
        public static void AddVehicle(IVehicleService vehicleService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Create a new Vehicle instance for registration.
                Vehicle addVehicle = new Vehicle();

                // Display header for new vehicle registration.
                Console.WriteLine("\t\t\t\tNew Vehicle Regesitration");

                // Prompt user to input the Vehicle Model.
                Console.WriteLine("Enter Vehicle Model: ");
                addVehicle.Model = Console.ReadLine();

                // Prompt user to input the Vehicle Manufacturer (Make).
                Console.WriteLine("Enter Vehicle Manufacturer (Make) : ");
                addVehicle.Make = Console.ReadLine();

                // Prompt user to input the Vehicle Color.
                Console.WriteLine("Enter Vehicle Color: ");
                addVehicle.Color = Console.ReadLine();

                // Validate that the color contains only letters and spaces.
                if (Regex.IsMatch(addVehicle.Color, "^[a-zA-Z ]+$"))
                {
                    // Prompt user to input the Vehicle Manufacturing year (YYYY).
                    Console.WriteLine("Enter Vehicle Manufacturing year (YYYY) : ");

                    // Validate that the input is a valid integer.
                    if (int.TryParse(Console.ReadLine(), out int year))
                    {
                        addVehicle.Year = year;

                        // Validate that the year is a four-digit number.
                        if (Regex.IsMatch(addVehicle.Year.ToString(), @"^\d{4}$"))
                        {
                            // Prompt user to input the Vehicle Registration Number.
                            Console.WriteLine("Enter Vehicle Registration Number: ");
                            addVehicle.RegistrationNumber = Console.ReadLine();

                            // Validate that the registration number contains alphanumeric characters and spaces.
                            if (Regex.IsMatch(addVehicle.RegistrationNumber, @"^[a-zA-Z0-9 ]+$"))
                            {
                                // Prompt user to input the Vehicle Availability (true / false).
                                Console.WriteLine("Enter Vehicle Availability (true / false): ");

                                // Validate that the input is a valid boolean.
                                if (bool.TryParse(Console.ReadLine(), out bool availability))
                                {
                                    addVehicle.Availability = availability;

                                    // Prompt user to input the Daily Rate of the Vehicle.
                                    Console.WriteLine("Enter Daily Rate of the Vehicle: ");

                                    // Validate that the input is a valid decimal value.
                                    if (decimal.TryParse(Console.ReadLine(), out decimal dailyRate))
                                    {
                                        addVehicle.DailyRate = dailyRate;
                                        Console.WriteLine();

                                        // Register the new vehicle and display the result.
                                        string res = vehicleService.AddVehicle(addVehicle);
                                        if (res != null)
                                        {
                                            Console.WriteLine(res);
                                            Console.Write("\nReturning to previous menu...");
                                            Thread.Sleep(2000);
                                        }
                                    }
                                    else throw new InvalidInputException("Input should only contain numeric values.");
                                }
                                else throw new InvalidInputException("Input should only be - True / False.");
                            }
                            else throw new InvalidInputException("Cannot use special characters");
                        }
                        else throw new InvalidInputException("Year should be of 4 digits.");
                    }
                    else throw new InvalidInputException("Year should only contain digits.");
                }
                else throw new InvalidInputException("Cannot use special characters and numbers");
            }
            catch (InvalidInputException ex)
            {
                // Handle specific input-related exceptions by displaying an error message.
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                // Handle other exceptions by displaying an error message.
                Console.WriteLine();
                Console.Write(ex.Message);
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// Updates details of a vehicle based on user input using the provided IVehicleService.
        /// </summary>
        /// <param name="vehicleService">The IVehicleService implementation to handle the vehicle details update.</param>
        public static void UpdateVehicle(IVehicleService vehicleService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Display header for vehicle details updation.
                Console.WriteLine("\t\t\t\tVehicle Details Updation");

                // Create a new Vehicle instance for updating details.
                Vehicle updateVehicle = new Vehicle();

                // Prompt user to input the availability status (true or false).
                Console.WriteLine("Enter availability status (true or false): ");

                // Validate that the input is a valid boolean.
                if (bool.TryParse(Console.ReadLine(), out bool availability))
                {
                    updateVehicle.Availability = availability;

                    // Prompt user to input the new Daily Rate.
                    Console.WriteLine("Enter new Daily Rate: ");

                    // Validate that the input is a valid decimal value.
                    if (decimal.TryParse(Console.ReadLine(), out decimal dailyRate))
                    {
                        updateVehicle.DailyRate = dailyRate;

                        // Prompt user to input the Vehicle Registration Number for which details need to be updated.
                        Console.WriteLine("Enter the Vehicle Registration Number for which the details need to be updated: ");
                        updateVehicle.RegistrationNumber = Console.ReadLine();

                        // Validate that the registration number contains alphanumeric characters and spaces.
                        if (Regex.IsMatch(updateVehicle.RegistrationNumber, @"^[a-zA-Z0-9 ]+$"))
                        {
                            Console.WriteLine();

                            // Update the vehicle details and display the result.
                            string updateVehicleRes = vehicleService.UpdateVehicle(updateVehicle);
                            if (updateVehicleRes != null)
                            {
                                Console.WriteLine(updateVehicleRes);
                                Console.Write("\nReturning to previous menu...");
                                Thread.Sleep(2000);
                            }
                        }
                        else throw new InvalidInputException("Cannot use special characters");
                    }
                    else throw new InvalidInputException("Input should only contain numeric values.");
                }
                else throw new InvalidInputException("Input should only be - True / False.");
            }
            catch (InvalidInputException ex)
            {
                // Handle specific input-related exceptions by displaying an error message.
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                // Handle other exceptions by displaying an error message.
                Console.WriteLine();
                Console.Write(ex.Message);
                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// Deletes details of a vehicle based on user input using the provided IVehicleService.
        /// </summary>
        /// <param name="vehicleService">The IVehicleService implementation to handle the vehicle details deletion.</param>
        public static void RemoveVehicle(IVehicleService vehicleService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Display header for vehicle details deletion.
                Console.WriteLine("\t\t\t\tVehicle Details Deletion\n");

                // Prompt the user to enter the ID of the vehicle to delete.
                Console.Write("Enter the ID of the vehicle you want to delete: ");
                int deleteVehicleID = int.Parse(Console.ReadLine());
                Console.WriteLine();

                // Delete the vehicle details based on the provided ID and display the result.
                string deleteCustomerRes = vehicleService.RemoveVehicle(deleteVehicleID);
                if (deleteCustomerRes != null)
                {
                    Console.WriteLine(deleteCustomerRes);
                    Console.Write("\nReturning to previous menu...");
                    Thread.Sleep(3500);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions by displaying an error message.
                Console.WriteLine();
                Console.WriteLine(ex.Message + " The ID should be a numeric value.");

                // Inform the user about returning to the previous menu and introduce a delay.
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }



        /// <summary>
        /// Displays the Reservation Menu and allows users to perform various operations related to reservations.
        /// </summary>
        public static void ReservationMenu()
        {
            // Initialize the reservation service.
            IReservationService reservationService;
            reservationService = new ReservationService();

            // Variable to control returning to the main menu.
            bool reservationReturnToMainMenu = false;

            // Display the Reservation Menu in a loop until the user chooses to return to the main menu or exit.
            while (!reservationReturnToMainMenu)
            {
                Console.Clear();

                // Display options in the Reservation Menu.
                Console.WriteLine("\t\tReservation Menu");
                Console.WriteLine("1. Get all reservations");
                Console.WriteLine("2. Get reservation details using reservation ID");
                Console.WriteLine("3. Get reservation details using customer ID");
                Console.WriteLine("4. Register a new reservation");
                Console.WriteLine("5. Update reservation");
                Console.WriteLine("6. Cancel an existing reservation data");
                Console.WriteLine("7. Return to Main Menu");
                Console.WriteLine("0. Exit");
                Console.Write("\nPlease enter your choice : ");

                // Read the user's choice.
                string reservationMenuChoice = Console.ReadLine();
                Console.WriteLine();

                // Process the user's choice with a switch statement.
                switch (reservationMenuChoice)
                {
                    // Option 1: Get all reservations.
                    case "1":
                        GetAllReservations(reservationService);
                        break;

                    // Option 2: Get reservation details using reservation ID.
                    case "2":
                        GetReservationById(reservationService);
                        break;

                    // Option 3: Get reservation details using customer ID.
                    case "3":
                        GetReservationByCustomerId(reservationService);
                        break;

                    // Option 4: Register a new reservation.
                    case "4":
                        CreateReservation(reservationService);
                        break;

                    // Option 5: Update reservation.
                    case "5":
                        UpdateReservation(reservationService);
                        break;

                    // Option 6: Cancel an existing reservation data.
                    case "6":
                        CancelReservation(reservationService);
                        break;

                    // Option 7: Return to Main Menu.
                    case "7":
                        reservationReturnToMainMenu = true;
                        break;

                    // Option 0: Exit.
                    case "0":
                        Exit();
                        break;

                    // Handle invalid choices.
                    default:
                        Console.Write("Invalid choice. Please enter a number as displayed.");
                        Thread.Sleep(2000);
                        break;
                }
            }
        }

        /// <summary>
        /// Retrieves and displays details of all reservations using the provided IReservationService.
        /// </summary>
        /// <param name="reservationService">The IReservationService implementation to fetch reservation details.</param>
        public static void GetAllReservations(IReservationService reservationService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Display header for the list of all reservations.
                Console.WriteLine("\t\t\t\tDeatils of All RESERVATIONS\n");

                // Retrieve a list of all reservations using the provided service.
                List<Reservation> reservations = reservationService.GetAllReservations();

                // Check if there are any reservations to display.
                if (reservations.Count > 0)
                {
                    // Iterate through each reservation and print details.
                    foreach (Reservation reservation in reservations)
                    {
                        Console.WriteLine(reservation);
                        Console.WriteLine();
                    }

                    // Prompt user to return to the previous menu.
                    Console.Write("\n\n\n\nPress any key to return to previous menu...");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions by displaying an error message.
                Console.WriteLine();
                Console.WriteLine(ex.Message);

                // Inform the user about returning to the previous menu and introduce a delay.
                Console.Write("Returning to previous menu.");
                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// Retrieves and displays details of a reservation using the provided IReservationService based on Reservation ID.
        /// </summary>
        /// <param name="reservationService">The IReservationService implementation to fetch reservation details.</param>
        public static void GetReservationById(IReservationService reservationService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Display header for reservation details retrieval by Reservation ID.
                Console.WriteLine("\t\t\t\tReservtion Details using Reservation ID\n");

                // Prompt user to input the ID of the reservation to retrieve data.
                Console.Write("Enter the ID of the Reservation you want to retrive the data: ");
                int reservationId = int.Parse(Console.ReadLine());
                Console.WriteLine();

                // Retrieve and display details of the reservation using the provided service.
                reservationService.GetReservationById(reservationId);
            }
            catch (Exception ex)
            {
                // Handle exceptions by displaying an error message.
                Console.WriteLine();
                Console.WriteLine(ex.Message + " The ID should be a numeric value.");

                // Inform the user about returning to the previous menu and introduce a delay.
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }

        /// <summary>
        /// Retrieves and displays details of reservations using the provided IReservationService based on Customer ID.
        /// </summary>
        /// <param name="reservationService">The IReservationService implementation to fetch reservation details.</param>
        public static void GetReservationByCustomerId(IReservationService reservationService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Display header for reservation details retrieval by Customer ID.
                Console.WriteLine("\t\t\t\tReservtion Details using Customer ID\n");

                // Prompt user to input the ID of the customer to retrieve reservation data.
                Console.Write("Enter the ID of the Customer you want to retrive the data: ");
                int customerId = int.Parse(Console.ReadLine());
                Console.WriteLine();

                // Retrieve and display details of reservations associated with the provided customer ID.
                reservationService.GetReservationByCustomerId(customerId);
            }
            catch (Exception ex)
            {
                // Handle exceptions by displaying an error message.
                Console.WriteLine();
                Console.WriteLine(ex.Message + " The ID should be a numeric value.");

                // Inform the user about returning to the previous menu and introduce a delay.
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }

        /// <summary>
        /// Registers a new reservation by gathering user input and utilizing the provided IReservationService.
        /// </summary>
        /// <param name="reservationService">The IReservationService implementation to handle reservation operations.</param>
        public static void CreateReservation(IReservationService reservationService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Create a new Reservation instance for registration.
                Reservation registerReservation = new Reservation();

                // Display header for new reservation registration.
                Console.WriteLine("\t\t\t\tNew Reservation");

                // Prompt user to input Customer ID.
                Console.WriteLine("Enter Customer ID: ");
                registerReservation.CustomerID = int.Parse(Console.ReadLine());

                // Prompt user to input Start Date (dd-MM-yyyy).
                Console.WriteLine("Enter Vehicle ID: ");
                registerReservation.VehicleID = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter Start Date (dd-MM-yyyy): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", null, DateTimeStyles.None, out DateTime parsedStartDate))
                {
                    registerReservation.StartDate = parsedStartDate;

                    // Check if the Start Date is valid (not in the past).
                    if (registerReservation.StartDate >= DateTime.Now.Date)
                    {
                        // Prompt user to input End Date (dd-MM-yyyy).
                        Console.WriteLine("Reservation must be made for at least 1 day.\nEnter End Date (dd-MM-yyyy): ");
                        if (DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", null, DateTimeStyles.None, out DateTime parsedEndDate))
                        {
                            registerReservation.EndDate = parsedEndDate;

                            // Check if End Date is greater than Start Date.
                            if (registerReservation.EndDate > registerReservation.StartDate)
                            {
                                // Calculate and set the Total Cost for the reservation.
                                registerReservation.TotalCost = reservationService.CalculateTotalCost(registerReservation.StartDate, registerReservation.EndDate, registerReservation.VehicleID);

                                // Prompt user to input reservation status.
                                Console.WriteLine("Enter status: ");
                                registerReservation.Status = Console.ReadLine();
                                Console.WriteLine();

                                // Create the reservation and display the result.
                                string res = reservationService.CreateReservation(registerReservation);
                                if (res != null)
                                {
                                    Console.WriteLine(res);
                                    Console.Write("\nReturning to previous menu...");
                                    Thread.Sleep(2000);
                                }
                            }
                            else throw new InvalidInputException($"Reservation must be made for at least 1 day.");
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Invalid date format.");
                            Console.Write("\nReturning to previous menu...");
                            Thread.Sleep(2000);
                        }
                    }
                    else throw new InvalidInputException($"Start Date cannot be lesser than {DateTime.Now.ToShortDateString()}");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid date format.");
                    Console.Write("\nReturning to previous menu...");
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions by displaying an error message.
                Console.WriteLine();
                Console.Write(ex.Message);
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// Updates the status of a reservation by gathering user input and utilizing the provided IReservationService.
        /// </summary>
        /// <param name="reservationService">The IReservationService implementation to handle reservation operations.</param>
        public static void UpdateReservation(IReservationService reservationService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Create a new Reservation instance for update.
                Reservation updateReservation = new Reservation();

                // Display header for reservation status update.
                Console.WriteLine("\t\t\t\tUpdate Reservation");

                // Prompt user to input the new status of the reservation.
                Console.WriteLine("Enter Status of the reservation: ");
                updateReservation.Status = Console.ReadLine();

                // Prompt user to input the reservation ID.
                Console.Write("Enter reservation id: ");
                updateReservation.ReservationID = int.Parse(Console.ReadLine());
                Console.WriteLine();

                // Update the reservation status and display the result.
                string updateReservationRes = reservationService.UpdateReservation(updateReservation);
                if (updateReservationRes != null)
                {
                    Console.WriteLine(updateReservationRes);
                    Console.Write("\nReturning to previous menu...");
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions by displaying an error message.
                Console.WriteLine();
                Console.Write(ex.Message);
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// Cancels a reservation by gathering user input and utilizing the provided IReservationService.
        /// </summary>
        /// <param name="reservationService">The IReservationService implementation to handle reservation operations.</param>
        public static void CancelReservation(IReservationService reservationService)
        {
            try
            {
                // Clear the console for a clean display.
                Console.Clear();

                // Display header for reservation cancellation page.
                Console.WriteLine("\t\t\t\tReservation Cacnellation Page\n");

                // Prompt user to input the ID of the reservation to cancel.
                Console.Write("Enter the ID of the reservation you want to cancel: ");
                int deleteReservationID = int.Parse(Console.ReadLine());
                Console.WriteLine();

                // Cancel the reservation and display the result.
                string deleteReservationRes = reservationService.CancelReservation(deleteReservationID);
                if (deleteReservationRes != null)
                {
                    Console.WriteLine(deleteReservationRes);
                    Console.Write("\nReturning to previous menu...");
                    Thread.Sleep(3500);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions by displaying an error message.
                Console.WriteLine();
                Console.WriteLine(ex.Message + " The ID should be a numeric value.");

                // Inform the user about returning to the previous menu and introduce a delay.
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }



        /// <summary>
        /// Exits the application with a brief message and a short delay.
        /// </summary>
        public static void Exit()
        {
            // Display an exit message.
            Console.Write("Exiting...");

            // Introduce a delay for a smoother exit experience.
            Thread.Sleep(2000);
            Console.WriteLine();

            // Terminate the application.
            Environment.Exit(0);
        }
    }
}
