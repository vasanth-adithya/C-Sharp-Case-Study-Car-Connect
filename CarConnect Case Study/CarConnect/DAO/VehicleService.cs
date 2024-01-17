using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using CarConnect.Entity;
using CarConnect.Exceptions;
using CarConnect.Util;

namespace CarConnect.DAO
{
    public class VehicleService : IVehicleService
    {
        /// <summary>
        /// Retrieves a list of all vehicles from the database.
        /// </summary>
        /// <returns>A list of Vehicle objects representing all vehicles in the database.</returns>

        public List<Vehicle> GetAllVehicles()
        {
            // Create a list to store the retrieved vehicles
            List<Vehicle> vehicles = new List<Vehicle>();

            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Open the database connection
                    conn.Open();

                    // Execute a SELECT command to retrieve all vehicles
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Vehicle", conn))
                    {
                        // Execute the SELECT command and read the results
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            // Check if the Vehicle table has rows
                            if (dr.HasRows)
                            {
                                // Iterate through the rows and populate the list of vehicles
                                while (dr.Read())
                                {
                                    vehicles.Add(new Vehicle()
                                    {
                                        VehicleID = (int)dr["VehicleID"],
                                        Model = dr["Model"].ToString(),
                                        Make = dr["Make"].ToString(),
                                        Year = (int)dr["Year"],
                                        Color = dr["Color"].ToString(),
                                        RegistrationNumber = dr["RegistrationNumber"].ToString(),
                                        Availability = (bool)dr["Availability"],
                                        DailyRate = (decimal)dr["DailyRate"]
                                    });
                                }

                                // Close the data reader and return the list of vehicles
                                dr.Close();
                                return vehicles;
                            }
                            else
                            {
                                // Throw an exception if the Vehicle table is empty
                                throw new VehicleNotFoundException("Vehicle Table is empty.");
                            }
                        }
                    }
                }
            }
            catch (SqlException se)
            {
                // Handle SQL-related exceptions
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
            catch (VehicleNotFoundException vnfe)
            {
                // Handle custom exception related to the absence of vehicles
                Console.WriteLine(vnfe.Message);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                Console.WriteLine(ex.Message);
            }

            // Display a message and return the empty list if an exception occurs
            Console.Write("\nReturning to previous menu...");
            Thread.Sleep(4000);

            return vehicles;
        }



        /// <summary>
        /// Displays details of a vehicle based on its ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to retrieve details for.</param>

        public void GetVehicleById(int vehicleId)
        {
            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Execute a SELECT command to retrieve details of the vehicle with the specified ID
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Vehicle WHERE VehicleID = @vehicleId", conn))
                    {
                        // Add parameters to the SQL command
                        cmd.Parameters.AddWithValue("@vehicleId", vehicleId);

                        try
                        {
                            // Open the database connection
                            conn.Open();

                            // Execute the SELECT command and read the results
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                // Check if the Vehicle table has rows
                                if (dr.HasRows)
                                {
                                    // Clear the console and display details of the vehicle
                                    Console.Clear();
                                    Console.WriteLine($"\t\t\t\tDetails of Vehicle ID: {vehicleId}\n");

                                    while (dr.Read())
                                    {
                                        Console.WriteLine($"  Model                 = {dr["Model"]}");
                                        Console.WriteLine($"  Make                  = {dr["Make"]}");
                                        Console.WriteLine($"  Year                  = {dr["Year"]}");
                                        Console.WriteLine($"  Color                 = {dr["Color"]}");
                                        Console.WriteLine($"  RegistrationNumber    = {dr["RegistrationNumber"]}");
                                        Console.WriteLine($"  Availability          = {dr["Availability"]}");
                                        Console.WriteLine($"  DailyRate             = {dr["DailyRate"]}");
                                    }

                                    // Display a message and wait for user input before returning to the previous menu
                                    Console.Write("\n\n\n\nPress any key to return to previous menu...");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    // Throw an exception if no vehicle is found with the specified ID
                                    throw new VehicleNotFoundException($"No vehicle is present with ID: {vehicleId}.");
                                }
                            }
                        }
                        catch (SqlException se)
                        {
                            // Handle SQL-related exceptions
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
            catch (VehicleNotFoundException ex)
            {
                // Handle custom exception related to the absence of the vehicle
                Console.WriteLine(ex.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(2000);
            }
            catch (Exception e)
            {
                // Handle generic exceptions
                Console.WriteLine(e.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(2000);
            }
        }



        /// <summary>
        /// Retrieves a list of available vehicles.
        /// </summary>
        /// <returns>A list of available vehicles.</returns>

        public List<Vehicle> GetAvailableVehicles()
        {
            // Create a list to store available vehicles
            List<Vehicle> vehicles = new List<Vehicle>();

            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Execute a SELECT command to retrieve available vehicles
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Vehicle WHERE Availability = 1", conn))
                    {
                        // Open the database connection
                        conn.Open();

                        // Execute the SELECT command and read the results
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            // Check if the Vehicle table has rows
                            if (dr.HasRows)
                            {
                                // Iterate through the rows and add each vehicle to the list
                                while (dr.Read())
                                {
                                    vehicles.Add(new Vehicle()
                                    {
                                        VehicleID = (int)dr["VehicleID"],
                                        Model = dr["Model"].ToString(),
                                        Make = dr["Make"].ToString(),
                                        Year = (int)dr["Year"],
                                        Color = dr["Color"].ToString(),
                                        RegistrationNumber = dr["RegistrationNumber"].ToString(),
                                        Availability = (bool)dr["Availability"],
                                        DailyRate = (decimal)dr["DailyRate"]
                                    });
                                }

                                // Return the list of available vehicles
                                return vehicles;
                            }
                            else
                            {
                                // Throw an exception if the Vehicle table is empty
                                throw new VehicleNotFoundException("Vehicle Table is empty.");
                            }
                        }
                    }
                }
            }
            catch (SqlException se)
            {
                // Handle SQL-related exceptions
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
            catch (VehicleNotFoundException vnfe)
            {
                // Handle custom exception related to the absence of vehicles
                Console.WriteLine(vnfe.Message);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                Console.WriteLine(ex.Message);
            }

            // Display a message and wait for a few seconds before returning to the previous menu
            Console.Write("\nReturning to previous menu...");
            Thread.Sleep(4000);

            // Return the list of available vehicles (may be empty in case of exceptions)
            return vehicles;
        }



        /// <summary>
        /// Adds a new vehicle to the database.
        /// </summary>
        /// <param name="vehicleData">The vehicle information to be added.</param>
        /// <returns>A success message with the ID of the added vehicle.</returns>

        public string AddVehicle(Vehicle vehicleData)
        {
            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Execute an INSERT command to add a new vehicle
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Vehicle OUTPUT INSERTED.VehicleID VALUES (@Model, @Make, @Year, @Color, @RegistrationNumber, @Availability, @DailyRate);", conn))
                    {
                        // Set parameters for the SQL command
                        cmd.Parameters.AddWithValue("@Model", vehicleData.Model);
                        cmd.Parameters.AddWithValue("@Make", vehicleData.Make);
                        cmd.Parameters.AddWithValue("@Year", vehicleData.Year);
                        cmd.Parameters.AddWithValue("@Color", vehicleData.Color);
                        cmd.Parameters.AddWithValue("@RegistrationNumber", vehicleData.RegistrationNumber);
                        cmd.Parameters.AddWithValue("@Availability", vehicleData.Availability);
                        cmd.Parameters.AddWithValue("@DailyRate", vehicleData.DailyRate);

                        try
                        {
                            // Open the database connection
                            conn.Open();
                            // Execute the INSERT command and retrieve the ID of the added vehicle
                            object newId = cmd.ExecuteScalar();

                            // Check if the ID is not null (indicating success)
                            if (newId != null)
                            {
                                return $"Vehicle Added Successfully. ID: {newId}";
                            }
                        }
                        catch (SqlException se)
                        {
                            // Handle SQL-related exceptions
                            if (se.Class == 14)
                            {
                                // Display a message if a vehicle with the same registration number already exists
                                Console.WriteLine($"Vehicle with registration number \"{vehicleData.RegistrationNumber}\" already exists. Please enter a correct registration number.");
                                Console.Write("\nReturning to the previous menu...");
                                Thread.Sleep(3000);
                            }
                            else
                            {
                                // Throw the exception for other SQL-related issues
                                throw se;
                            }
                        }
                    }
                }
            }
            catch (SqlException se)
            {
                // Handle SQL-related exceptions
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
                    // Display a general error message for other SQL-related issues
                    Console.WriteLine("An error occurred: " + se.Message);
                    Console.Write("\n\n\n\nPress any key to return to previous menu...");
                    Console.ReadKey();
                }
            }
            catch (DatabaseConnectionException dce)
            {
                // Handle custom exception related to database connection
                Console.WriteLine(dce.Message);
                Console.Write("\nReturning to the previous menu...");
                Thread.Sleep(4000);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
                Console.Write("\nReturning to the previous menu...");
                Thread.Sleep(3000);
            }

            // Return null if the vehicle addition was not successful
            return null;
        }



        /// <summary>
        /// Updates the details of a vehicle in the database.
        /// </summary>
        /// <param name="vehicleData">The updated vehicle information.</param>
        /// <returns>A success message if the update is successful.</returns>

        public string UpdateVehicle(Vehicle vehicleData)
        {
            string response = null;

            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Execute an UPDATE command to update the details of a vehicle
                    using (SqlCommand cmd = new SqlCommand("UPDATE Vehicle SET Availability=@Availability, DailyRate=@DailyRate WHERE RegistrationNumber=@RegistrationNumber;", conn))
                    {
                        // Set parameters for the SQL command
                        cmd.Parameters.AddWithValue("@Availability", vehicleData.Availability);
                        cmd.Parameters.AddWithValue("@DailyRate", vehicleData.DailyRate);
                        cmd.Parameters.AddWithValue("@RegistrationNumber", vehicleData.RegistrationNumber);

                        try
                        {
                            // Open the database connection
                            conn.Open();
                            // Execute the UPDATE command and retrieve the number of updated rows
                            int updatedRows = cmd.ExecuteNonQuery();

                            // Check if the update is successful
                            if (updatedRows > 0)
                            {
                                response = "Vehicle details updated successfully.";
                            }
                            else
                            {
                                // Throw a custom exception if the registration number is not found
                                throw new VehicleNotFoundException("Invalid Registration Number. Please enter a valid registration number to update the details.");
                            }
                        }
                        catch (SqlException se)
                        {
                            // Handle SQL-related exceptions
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
                                // Display a general error message for other SQL-related issues
                                Console.WriteLine("An error occurred: " + se.Message);
                                Console.Write("\nReturning to the previous menu...");
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
                Console.Write("\nReturning to the previous menu...");
                Thread.Sleep(4000);
            }
            catch (VehicleNotFoundException vnfe)
            {
                // Handle custom exception related to a not found vehicle
                Console.WriteLine(vnfe.Message);
                Console.Write("\nReturning to the previous menu...");
                Thread.Sleep(4500);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                Console.WriteLine(ex.Message);
                Thread.Sleep(4500);
            }

            // Return the response (success message or null if unsuccessful)
            return response;
        }



        /// <summary>
        /// Removes a vehicle from the database based on the provided vehicle ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to be removed.</param>
        /// <returns>A success message if the removal is successful.</returns>

        public string RemoveVehicle(int vehicleId)
        {
            string response = null;

            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Execute a DELETE command to remove a vehicle based on its ID
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Vehicle WHERE VehicleID = @VehicleID", conn))
                    {
                        // Set parameters for the SQL command
                        cmd.Parameters.AddWithValue("@VehicleID", vehicleId);

                        try
                        {
                            // Open the database connection
                            conn.Open();
                            // Execute the DELETE command and retrieve the number of deleted rows
                            int deleteRows = cmd.ExecuteNonQuery();

                            // Check if the removal is successful
                            if (deleteRows > 0)
                            {
                                response = $"Data of Vehicle with ID: {vehicleId} has been deleted successfully.";
                            }
                            else
                            {
                                // Throw a custom exception if the vehicle ID is not found
                                throw new VehicleNotFoundException($"Vehicle by ID: {vehicleId} is not present.");
                            }
                        }
                        catch (SqlException se)
                        {
                            // Handle SQL-related exceptions
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
                                // Display a general error message for other SQL-related issues
                                Console.WriteLine("An error occurred: " + se.Message);
                                Console.Write("\nReturning to the previous menu...");
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
                Console.Write("\nReturning to the previous menu...");
                Thread.Sleep(4000);
            }
            catch (VehicleNotFoundException vnfe)
            {
                // Handle custom exception related to a not found vehicle
                Console.WriteLine(vnfe.Message);
                Console.Write("\nReturning to the previous menu...");
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                Console.WriteLine(ex.Message);
                Thread.Sleep(2000);
            }

            // Return the response (success message or null if unsuccessful)
            return response;
        }

    }
}
