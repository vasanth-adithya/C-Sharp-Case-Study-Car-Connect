using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using CarConnect.Entity;
using CarConnect.Exceptions;
using CarConnect.Util;

namespace CarConnect.DAO
{
    public class CustomerService : ICustomerService
    {
        /// <summary>
        /// Authenticates a customer based on the provided username and password.
        /// </summary>
        /// <param name="username">The username of the customer.</param>
        /// <param name="password">The password of the customer.</param>
        /// <returns>True if authentication is successful, otherwise false.</returns>

        public bool Authenticate(string username, string password)
        {
            try
            {
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    conn.Open();

                    // Prepare SQL command to retrieve customer information
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Customer WHERE username = @username", conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            // Check if the customer exists and read their details
                            if (dr.HasRows && dr.Read())
                            {
                                // Retrieve the stored password (assuming it's stored as a hashed value)
                                string storedPassword = dr["Password"].ToString();

                                // Compare the provided password with the stored password
                                if (password == storedPassword)
                                {
                                    Console.Write("Customer authentication successful! Logged IN.");
                                    Thread.Sleep(2000);
                                    return true;
                                }
                                else throw new AuthenticationException("Invalid password.");
                            }
                            else
                            {
                                throw new AuthenticationException("Invalid Customer username.");
                            }
                        }
                    }
                }
            }
            catch (SqlException se)
            {
                // Handle SQL exceptions related to database connection
                if (se.Class == 11)
                {
                    throw new DatabaseConnectionException("Error connecting to Database.\nName of the database is not present in the SQL server.");
                }
                else if (se.Class == 20)
                {
                    throw new DatabaseConnectionException("Error connecting to Database.\nIncorrect Server Name.");
                }
                else
                {
                    Console.WriteLine("An error occurred: " + se.Message);
                    Console.Write("\n\n\n\nPress any key to return to previous menu...");
                    Console.ReadKey();
                }
            }
            catch (DatabaseConnectionException dce)
            {
                // Handle custom exception related to database connection
                Console.WriteLine(dce.Message);
            }
            catch (AuthenticationException ae)
            {
                // Handle custom exception related to authentication
                Console.WriteLine(ae.Message);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                Console.WriteLine($"Authentication Error: {ex.Message}");
            }

            // Inform user and wait before returning to the previous menu
            Console.Write("\nReturning to previous menu...");
            Thread.Sleep(3000);
            return false;
        }



        /// <summary>
        /// Retrieves a list of all customers from the database.
        /// </summary>
        /// <returns>A list of Customer objects representing all customers in the database.</returns>

        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            try
            {
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Prepare and execute SQL command to select all customers
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Customer", conn))
                    {
                        conn.Open();

                        // Read data using SqlDataReader
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            // Check if there are rows in the result set
                            if (dr.HasRows)
                            {
                                // Iterate through each row and create Customer objects
                                while (dr.Read())
                                {
                                    customers.Add(new Customer()
                                    {
                                        CustomerID = (int)dr["CustomerID"],
                                        FirstName = dr["FirstName"].ToString(),
                                        LastName = dr["LastName"].ToString(),
                                        Email = dr["Email"].ToString(),
                                        PhoneNumber = dr["PhoneNumber"].ToString(),
                                        Address = dr["Address"].ToString(),
                                        UserName = dr["UserName"].ToString(),
                                        Password = dr["Password"].ToString(),
                                        RegistrationDate = (DateTime)dr["RegistrationDate"]
                                    });
                                }
                                return customers;
                            }
                            else
                            {
                                // Throw exception if the Customer table is empty
                                throw new CustomerNotFoundException("Customer Table is empty.");
                            }
                        }
                    }
                }
            }
            catch (SqlException se)
            {
                // Handle SQL exceptions related to database connection
                if (se.Class == 11)
                {
                    throw new DatabaseConnectionException("Error connecting to Database.\nName of the database is not present in the SQL server.");
                }
                else if (se.Class == 20)
                {
                    throw new DatabaseConnectionException("Error connecting to Database.\nIncorrect Server Name.");
                }
                else
                {
                    Console.WriteLine("An error occurred: " + se.Message);
                }
            }
            catch (DatabaseConnectionException dce)
            {
                // Handle custom exception related to database connection
                Console.WriteLine(dce.Message);
            }
            catch (CustomerNotFoundException cnfe)
            {
                // Handle custom exception related to empty Customer table
                Console.WriteLine(cnfe.Message);
            }
            catch (Exception ex)
            {
                // Handle other unexpected exceptions
                Console.WriteLine(ex.Message);
            }

            // Inform user and wait before returning to the previous menu
            Console.Write("\nReturning to the previous menu...");
            Thread.Sleep(4000);

            return customers;
        }



        /// <summary>
        /// Retrieves and displays details of a customer based on the provided customer ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer to retrieve.</param>

        public void GetCustomerById(int customerId)
        {
            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Prepare SQL command to retrieve customer details by ID
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Customer WHERE CustomerID = @customerId", conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);

                        try
                        {
                            conn.Open();

                            // Execute SQL command and retrieve results
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                // Check if customer with the given ID exists
                                if (dr.HasRows)
                                {
                                    // Clear console and display customer details
                                    Console.Clear();
                                    Console.WriteLine($"\t\t\t\tDetails of Customer ID : {customerId}\n");

                                    while (dr.Read())
                                    {
                                        DateTime registrationDate = (DateTime)dr["RegistrationDate"];

                                        Console.WriteLine($"  FirstName         = {dr["FirstName"]}");
                                        Console.WriteLine($"  LastName          = {dr["LastName"]}");
                                        Console.WriteLine($"  Email             = {dr["Email"]}");
                                        Console.WriteLine($"  PhoneNumber       = {dr["PhoneNumber"]}");
                                        Console.WriteLine($"  UserName          = {dr["UserName"]}");
                                        Console.WriteLine($"  Address           = {dr["Address"]}");
                                        Console.WriteLine($"  Password          = {dr["Password"]}");
                                        Console.WriteLine($"  RegistrationDate  = {registrationDate.ToShortDateString()}");
                                    }

                                    // Inform user and wait before returning to the previous menu
                                    Console.Write("\n\n\n\nPress any key to return to the previous menu...");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    // Throw exception if customer with the given ID is not found
                                    throw new CustomerNotFoundException($"No customer is present with ID: {customerId}.");
                                }
                            }
                        }
                        catch (SqlException se)
                        {
                            // Handle SQL exceptions related to database connection
                            if (se.Class == 11)
                            {
                                throw new DatabaseConnectionException("Error connecting to Database.\nName of the database is not present in the SQL server.");
                            }
                            else if (se.Class == 20)
                            {
                                throw new DatabaseConnectionException("Error connecting to Database.\nIncorrect Server Name.");
                            }
                            else
                            {
                                Console.WriteLine("An error occurred: " + se.Message);
                                Console.Write("\n\n\n\nPress any key to return to previous menu...");
                                Console.ReadKey();
                            }
                        }
                    }
                }
            }
            catch (DatabaseConnectionException dce)
            {
                // Handle custom exception related to database connection
                Console.WriteLine(dce.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
            catch (CustomerNotFoundException cnfe)
            {
                // Handle custom exception related to customer not found
                Console.WriteLine(cnfe.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                Console.WriteLine(ex.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }



        /// <summary>
        /// Retrieves and displays details of a customer based on the provided username.
        /// </summary>
        /// <param name="username">The username of the customer to retrieve.</param>

        public void GetCustomerByUserName(string username)
        {
            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Prepare SQL command to retrieve customer details by username
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Customer WHERE UserName = @username", conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        try
                        {
                            conn.Open();

                            // Execute SQL command and retrieve results
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                // Check if customer with the given username exists
                                if (dr.HasRows)
                                {
                                    // Clear console and display customer details
                                    Console.Clear();
                                    Console.WriteLine($"\t\t\t\tDetails of User: {username}\n");

                                    while (dr.Read())
                                    {
                                        DateTime registrationDate = (DateTime)dr["RegistrationDate"];

                                        Console.WriteLine($"  CustomerID        = {dr["CustomerID"]}");
                                        Console.WriteLine($"  FirstName         = {dr["FirstName"]}");
                                        Console.WriteLine($"  LastName          = {dr["LastName"]}");
                                        Console.WriteLine($"  Email             = {dr["Email"]}");
                                        Console.WriteLine($"  PhoneNumber       = {dr["PhoneNumber"]}");
                                        Console.WriteLine($"  Address           = {dr["Address"]}");
                                        Console.WriteLine($"  Password          = {dr["Password"]}");
                                        Console.WriteLine($"  RegistrationDate  = {registrationDate.ToShortDateString()}");
                                    }

                                    // Inform user and wait before returning to the previous menu
                                    Console.Write("\n\n\n\nPress any key to return to the previous menu...");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    // Throw exception if customer with the given username is not found
                                    throw new CustomerNotFoundException($"No customer is present with the username: {username}.");
                                }
                            }
                        }
                        catch (SqlException se)
                        {
                            // Handle SQL exceptions related to database connection
                            if (se.Class == 11)
                            {
                                throw new DatabaseConnectionException("Error connecting to Database.\nName of the database is not present in the SQL server.");
                            }
                            else if (se.Class == 20)
                            {
                                throw new DatabaseConnectionException("Error connecting to Database.\nIncorrect Server Name.");
                            }
                            else
                            {
                                Console.WriteLine("An error occurred: " + se.Message);
                                Console.Write("\n\n\n\nPress any key to return to previous menu...");
                                Console.ReadKey();
                            }
                        }
                    }
                }
            }
            catch (DatabaseConnectionException dce)
            {
                // Handle custom exception related to database connection
                Console.WriteLine(dce.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
            catch (CustomerNotFoundException ex)
            {
                // Handle custom exception related to customer not found
                Console.WriteLine(ex.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                Console.Write(ex.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
        }



        /// <summary>
        /// Registers a new customer in the database.
        /// </summary>
        /// <param name="customerData">Customer data to be registered.</param>
        /// <returns>A string indicating the success of the registration along with the new customer ID.</returns>

        public string RegisterCustomer(Customer customerData)
        {
            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Prepare SQL command for customer registration
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Customer OUTPUT INSERTED.CustomerID VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Address, @UserName, @Password, @RegistrationDate);", conn))
                    {
                        // Set parameters for the SQL command
                        cmd.Parameters.AddWithValue("@FirstName", customerData.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", customerData.LastName);
                        cmd.Parameters.AddWithValue("@Email", customerData.Email);
                        cmd.Parameters.AddWithValue("@PhoneNumber", customerData.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Address", customerData.Address);
                        cmd.Parameters.AddWithValue("@UserName", customerData.UserName);
                        cmd.Parameters.AddWithValue("@Password", customerData.Password);
                        cmd.Parameters.AddWithValue("@RegistrationDate", customerData.RegistrationDate);

                        try
                        {
                            // Open the database connection
                            conn.Open();

                            // Execute the SQL command and retrieve the new customer ID
                            object newId = cmd.ExecuteScalar();
                            if (newId != null)
                            {
                                // Return a success message with the new customer ID
                                return $"Customer Registration Successful. ID: {newId}";
                            }
                        }
                        catch (SqlException se)
                        {
                            // Handle specific SQL exceptions, e.g., if the username already exists
                            if (se.Class == 14)
                            {
                                Console.WriteLine("Customer Username already exists. Please enter another username");
                                Console.Write("\nReturning to the previous menu...");
                                Thread.Sleep(3000);
                            }
                            else
                            {
                                // Rethrow the exception if it's not a specific case
                                throw se;
                            }
                        }
                    }
                }
            }
            catch (SqlException se)
            {
                // Handle SQL exceptions related to database connection
                if (se.Class == 11)
                {
                    throw new DatabaseConnectionException("Error connecting to Database.\nName of the database is not present in the SQL server.");
                }
                else if (se.Class == 20)
                {
                    throw new DatabaseConnectionException("Error connecting to Database.\nIncorrect Server Name.");
                }
                else
                {
                    Console.WriteLine("An error occurred: " + se.Message);
                    Console.Write("\n\n\n\nPress any key to return to previous menu...");
                    Console.ReadKey();
                }
            }
            catch (DatabaseConnectionException dce)
            {
                // Handle custom exception related to database connection
                Console.WriteLine(dce.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                Console.Write("An error occurred: " + ex.Message);
                Console.Write("\nReturning to the previous menu...");
                Thread.Sleep(3000);
            }

            return null; // Return null if registration is unsuccessful
        }



        /// <summary>
        /// Updates customer details in the database.
        /// </summary>
        /// <param name="customerData">Customer data to be updated.</param>
        /// <returns>A string indicating the success of the update operation.</returns>

        public string UpdateCustomer(Customer customerData)
        {
            string response = null;

            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Prepare SQL command for updating customer details
                    using (SqlCommand cmd = new SqlCommand("UPDATE Customer SET PhoneNumber=@PhoneNumber, Address=@Address, FirstName=@FirstName, LastName=@LastName WHERE Username=@Username;", conn))
                    {
                        // Set parameters for the SQL command
                        cmd.Parameters.AddWithValue("@PhoneNumber", customerData.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Address", customerData.Address);
                        cmd.Parameters.AddWithValue("@Username", customerData.UserName);
                        cmd.Parameters.AddWithValue("@FirstName", customerData.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", customerData.LastName);

                        try
                        {
                            // Open the database connection
                            conn.Open();

                            // Execute the SQL command and get the number of updated rows
                            int updatedRows = cmd.ExecuteNonQuery();
                            if (updatedRows > 0)
                            {
                                // Set response message for a successful update
                                response = "Customer details updated successfully.";
                            }
                            else
                            {
                                // Throw an exception if no rows were updated
                                throw new CustomerNotFoundException("Invalid Customer Username. Please enter a valid username to update the details.");
                            }
                        }
                        catch (SqlException se)
                        {
                            // Handle specific SQL exceptions related to database connection
                            if (se.Class == 11)
                            {
                                throw new DatabaseConnectionException("Error connecting to Database.\nName of the database is not present in the SQL server.");
                            }
                            else if (se.Class == 20)
                            {
                                throw new DatabaseConnectionException("Error connecting to Database.\nIncorrect Server Name.");
                            }
                            else
                            {
                                // Handle other SQL exceptions
                                Console.WriteLine("An error occurred: " + se.Message);
                                Console.Write("\n\n\n\nPress any key to return to previous menu...");
                                Console.ReadKey();
                            }
                        }
                    }
                }
            }
            catch (DatabaseConnectionException dce)
            {
                // Handle custom exception related to database connection
                Console.WriteLine(dce.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
            catch (CustomerNotFoundException e)
            {
                // Handle exception for an invalid customer username
                Console.WriteLine(e.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4500);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                Console.WriteLine(ex.Message);
                Thread.Sleep(4500);
            }

            return response; // Return the response message
        }


        /// <summary>
        /// Deletes a customer from the database based on the provided customer ID.
        /// </summary>
        /// <param name="customerId">ID of the customer to be deleted.</param>
        /// <returns>A string indicating the success of the deletion operation.</returns>

        public string DeleteCustomer(int customerId)
        {
            string response = null;

            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Prepare SQL command for deleting a customer
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Customer WHERE CustomerID = @customerId", conn))
                    {
                        // Set parameter for the SQL command
                        cmd.Parameters.AddWithValue("@customerId", customerId);

                        try
                        {
                            // Open the database connection
                            conn.Open();

                            // Execute the SQL command and get the number of deleted rows
                            int deleteRows = cmd.ExecuteNonQuery();

                            if (deleteRows > 0)
                            {
                                // Set response message for a successful deletion
                                response = $"Data of customer with ID: {customerId} has been deleted successfully.";
                            }
                            else
                            {
                                // Throw an exception if no rows were deleted
                                throw new CustomerNotFoundException($"Customer by ID: {customerId} is not present.");
                            }
                        }
                        catch (SqlException se)
                        {
                            // Handle specific SQL exceptions related to database connection
                            if (se.Class == 11)
                            {
                                throw new DatabaseConnectionException("Error connecting to Database.\nName of the database is not present in the SQL server.");
                            }
                            else if (se.Class == 20)
                            {
                                throw new DatabaseConnectionException("Error connecting to Database.\nIncorrect Server Name.");
                            }
                            else
                            {
                                // Handle other SQL exceptions
                                Console.WriteLine("An error occurred: " + se.Message);
                                Console.Write("\n\n\n\nPress any key to return to previous menu...");
                                Console.ReadKey();
                            }
                        }
                    }
                }
            }
            catch (DatabaseConnectionException dce)
            {
                // Handle custom exception related to database connection
                Console.WriteLine(dce.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4000);
            }
            catch (CustomerNotFoundException e)
            {
                // Handle exception for a customer not found
                Console.WriteLine(e.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                Console.WriteLine(ex.Message);
                Thread.Sleep(2000);
            }

            return response; // Return the response message
        }
    }
}
