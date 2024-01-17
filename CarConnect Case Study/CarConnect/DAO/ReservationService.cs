using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using CarConnect.Entity;
using CarConnect.Exceptions;
using CarConnect.Util;

namespace CarConnect.DAO
{
    internal class ReservationService : IReservationService
    {
        /// <summary>
        /// Retrieves a list of all reservations from the system.
        /// </summary>
        /// <returns>A list of Reservation objects representing all reservations.</returns>

        public List<Reservation> GetAllReservations()
        {
            // Initialize a list to store Reservation objects
            List<Reservation> reservations = new List<Reservation>();

            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Prepare SQL command for selecting all reservations
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Reservation", conn))
                    {
                        try
                        {
                            // Open the database connection
                            conn.Open();
                            // Execute the SQL command and get the result set
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                // Check if the result set has rows
                                if (dr.HasRows)
                                {
                                    // Iterate through the result set and populate Reservation objects
                                    while (dr.Read())
                                    {
                                        reservations.Add(new Reservation()
                                        {
                                            ReservationID = (int)dr["ReservationID"],
                                            CustomerID = (int)dr["CustomerID"],
                                            VehicleID = (int)dr["VehicleID"],
                                            StartDate = (DateTime)dr["StartDate"],
                                            EndDate = (DateTime)dr["EndDate"],
                                            TotalCost = (decimal)dr["TotalCost"],
                                            Status = dr["Status"].ToString()
                                        });
                                    }
                                    // Close the SqlDataReader
                                    dr.Close();
                                    // Return the list of reservations
                                    return reservations;
                                }
                                else
                                {
                                    // Throw an exception if the Reservation table is empty
                                    throw new ReservationException("Reservation Table is empty.");
                                }
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
                                throw new DatabaseConnectionException("Incorrect Server Name.");
                            }
                            else
                            {
                                // Handle other SQL exceptions
                                Console.WriteLine("An error occurred: " + se.Message);
                            }
                        }
                    }
                }
            }
            catch (DatabaseConnectionException dce)
            {
                // Handle custom exception related to database connection
                Console.WriteLine(dce.Message);
            }
            catch (ReservationException re)
            {
                // Handle custom exception related to reservations
                Console.WriteLine(re.Message);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                Console.WriteLine(ex.Message);
            }

            // Return the list of reservations
            Console.Write("\nReturning to previous menu...");
            Thread.Sleep(4000);
            return reservations;
        }



        /// <summary>
        /// Retrieves details of a reservation based on the provided reservation ID.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation to retrieve details for.</param>

        public void GetReservationById(int reservationId)
        {
            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Prepare SQL command for selecting reservation details by ID
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Reservation WHERE ReservationID = @ReservationID", conn))
                    {
                        // Set the parameter for the reservation ID
                        cmd.Parameters.AddWithValue("@ReservationID", reservationId);

                        try
                        {
                            // Open the database connection
                            conn.Open();
                            // Execute the SQL command and get the result set
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                // Check if the result set has rows
                                if (dr.HasRows)
                                {
                                    // Clear the console and display reservation details
                                    Console.Clear();
                                    Console.WriteLine($"\t\t\t\tDetails of Reservation ID: {reservationId}\n");

                                    while (dr.Read())
                                    {
                                        // Extract and format date values
                                        DateTime startDate = (DateTime)dr["StartDate"];
                                        DateTime endDate = (DateTime)dr["EndDate"];

                                        // Display reservation details
                                        Console.WriteLine($"  CustomerID     = {dr["CustomerID"]}");
                                        Console.WriteLine($"  VehicleID      = {dr["VehicleID"]}");
                                        Console.WriteLine($"  StartDate      = {startDate.ToShortDateString()}");
                                        Console.WriteLine($"  EndDate        = {endDate.ToShortDateString()}");
                                        Console.WriteLine($"  TotalCost      = {dr["TotalCost"]}");
                                        Console.WriteLine($"  Status         = {dr["Status"]}");
                                    }

                                    // Prompt to return to the previous menu
                                    Console.Write("\n\n\n\nPress any key to return to the previous menu...");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    // Throw an exception if no reservation is found with the given ID
                                    throw new ReservationException($"No reservation is present with ID: {reservationId}.");
                                }
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
                                throw new DatabaseConnectionException("Incorrect Server Name.");
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
            catch (ReservationException re)
            {
                // Handle custom exception related to reservations
                Console.WriteLine(re.Message);
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
        /// Retrieves details of reservations for a specific customer based on the provided customer ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer for whom reservations are to be retrieved.</param>

        public void GetReservationByCustomerId(int customerId)
        {
            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Prepare SQL command for selecting reservations by customer ID
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Reservation WHERE CustomerID = @CustomerID", conn))
                    {
                        // Set the parameter for the customer ID
                        cmd.Parameters.AddWithValue("@CustomerID", customerId);

                        try
                        {
                            // Open the database connection
                            conn.Open();
                            // Execute the SQL command and get the result set
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                // Check if the result set has rows
                                if (dr.HasRows)
                                {
                                    // Clear the console and display reservation details
                                    Console.Clear();
                                    Console.WriteLine($"\t\t\t\tDetails of Reservations for Customer ID: {customerId}\n");

                                    while (dr.Read())
                                    {
                                        // Extract and format date values
                                        DateTime startDate = (DateTime)dr["StartDate"];
                                        DateTime endDate = (DateTime)dr["EndDate"];

                                        // Display reservation details
                                        Console.WriteLine($"  ReservationID     = {dr["ReservationID"]}");
                                        Console.WriteLine($"  VehicleID         = {dr["VehicleID"]}");
                                        Console.WriteLine($"  StartDate         = {startDate.ToShortDateString()}");
                                        Console.WriteLine($"  EndDate           = {endDate.ToShortDateString()}");
                                        Console.WriteLine($"  TotalCost         = {dr["TotalCost"]}");
                                        Console.WriteLine($"  Status            = {dr["Status"]}");
                                        Console.WriteLine();
                                    }

                                    // Prompt to return to the previous menu
                                    Console.Write("\n\n\n\nPress any key to return to the previous menu...");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    // Throw an exception if no reservations are found for the given customer ID
                                    throw new ReservationException($"No reservations found for customer with ID: {customerId}.");
                                }
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
                                throw new DatabaseConnectionException("Incorrect Server Name.");
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
            catch (ReservationException re)
            {
                // Handle custom exception related to reservations
                Console.WriteLine(re.Message);
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
        /// Creates a new reservation based on the provided reservation data.
        /// </summary>
        /// <param name="reservationData">The reservation data to be used for creating the reservation.</param>
        /// <returns>A string message indicating the success of the reservation creation along with the reservation ID, or null if unsuccessful.</returns>

        public string CreateReservation(Reservation reservationData)
        {
            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Prepare SQL command for inserting a new reservation and retrieving the inserted ReservationID
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Reservation OUTPUT INSERTED.ReservationID VALUES (@CustomerID, @VehicleID, @StartDate, @EndDate, @TotalCost, @Status);", conn))
                    {
                        // Set parameters for the reservation data
                        cmd.Parameters.AddWithValue("@CustomerID", reservationData.CustomerID);
                        cmd.Parameters.AddWithValue("@VehicleID", reservationData.VehicleID);
                        cmd.Parameters.AddWithValue("@StartDate", reservationData.StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", reservationData.EndDate);
                        cmd.Parameters.AddWithValue("@TotalCost", reservationData.TotalCost);
                        cmd.Parameters.AddWithValue("@Status", reservationData.Status);

                        try
                        {
                            // Open the database connection
                            conn.Open();
                            // Execute the SQL command and retrieve the newly inserted ReservationID
                            object newId = cmd.ExecuteScalar();

                            // Check if the newId is not null
                            if (newId != null)
                            {
                                // Return a success message with the newly created ReservationID
                                return $"Reservation Successful with ID: {newId}";
                            }
                        }
                        catch (SqlException se)
                        {
                            // Handle specific SQL exceptions
                            throw se;
                        }
                    }
                }
            }
            catch (SqlException se)
            {
                // Handle specific SQL exceptions related to database connection and other issues
                if (se.Class == 11)
                {
                    throw new DatabaseConnectionException("Error connecting to Database.\nName of the database is not present in the SQL server.");
                }
                else if (se.Class == 16)
                {
                    throw new DatabaseConnectionException("Invalid vehicle or customer ID.");
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

            // Return null if the reservation creation was unsuccessful
            return null;
        }



        /// <summary>
        /// Updates the status of a reservation based on the provided reservation data.
        /// </summary>
        /// <param name="reservationData">The reservation data containing the updated status and the reservation ID.</param>
        /// <returns>A string message indicating the success of the reservation update, or null if unsuccessful.</returns>

        public string UpdateReservation(Reservation reservationData)
        {
            string response = null;

            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Prepare SQL command for updating the status of a reservation based on ReservationID
                    using (SqlCommand cmd = new SqlCommand("UPDATE Reservation SET Status=@Status WHERE ReservationID=@ReservationID;", conn))
                    {
                        // Set parameters for the reservation data
                        cmd.Parameters.AddWithValue("@Status", reservationData.Status);
                        cmd.Parameters.AddWithValue("@ReservationID", reservationData.ReservationID);

                        try
                        {
                            // Open the database connection
                            conn.Open();
                            // Execute the SQL command and get the number of updated rows
                            int updatedRows = cmd.ExecuteNonQuery();

                            // Check if any rows were updated
                            if (updatedRows > 0)
                            {
                                // Set the response message for successful update
                                response = "Reservation details updated successfully.";
                            }
                            else
                            {
                                // Throw an exception if no rows were updated
                                throw new ReservationException("Invalid Reservation ID. Please enter a valid reservation ID to update the details.");
                            }
                        }
                        catch (SqlException se)
                        {
                            // Handle specific SQL exceptions related to database connection and other issues
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
            catch (ReservationException re)
            {
                // Handle custom exception related to reservation
                Console.WriteLine(re.Message);
                Console.Write("\nReturning to previous menu...");
                Thread.Sleep(4500);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                Console.WriteLine(ex.Message);
                Thread.Sleep(4500);
            }

            // Return the response message
            return response;
        }



        /// <summary>
        /// Cancels a reservation and updates the status to 'Cancelled' and vehicle availability if applicable.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation to be cancelled.</param>
        /// <returns>A string message indicating the success of the reservation cancellation, or null if unsuccessful.</returns>

        public string CancelReservation(int reservationId)
        {
            string response = null;

            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Open the database connection
                    conn.Open();

                    // Check the current status and vehicle ID of the reservation
                    using (SqlCommand selectCmd = new SqlCommand("SELECT Status, VehicleID FROM Reservation WHERE ReservationID = @ReservationID", conn))
                    {
                        selectCmd.Parameters.AddWithValue("@ReservationID", reservationId);

                        // Execute the SELECT command and read the results
                        using (SqlDataReader dr = selectCmd.ExecuteReader())
                        {
                            // Check if the reservation exists
                            if (dr.HasRows)
                            {
                                dr.Read();
                                string status = dr["Status"].ToString();
                                int vehicleId = (int)dr["VehicleID"];

                                dr.Close();

                                // Check if the status allows cancellation
                                if (status.ToLower() == "pending" || status.ToLower() == "confirmed")
                                {
                                    // Update the reservation status to 'Cancelled'
                                    using (SqlCommand updateReservationCmd = new SqlCommand("UPDATE Reservation SET Status = 'Cancelled' WHERE ReservationID = @ReservationID", conn))
                                    {
                                        updateReservationCmd.Parameters.AddWithValue("@ReservationID", reservationId);

                                        // Execute the UPDATE command and get the number of updated rows for reservations
                                        int updatedRowsReservation = updateReservationCmd.ExecuteNonQuery();

                                        // Update the vehicle availability
                                        using (SqlCommand updateVehicleCmd = new SqlCommand("UPDATE Vehicle SET Availability = 1 WHERE VehicleID = @VehicleID", conn))
                                        {
                                            updateVehicleCmd.Parameters.AddWithValue("@VehicleID", vehicleId);

                                            // Execute the UPDATE command and get the number of updated rows for vehicles
                                            int updatedRowsVehicle = updateVehicleCmd.ExecuteNonQuery();

                                            // Check if both reservation and vehicle updates were successful
                                            if (updatedRowsReservation > 0 && updatedRowsVehicle > 0)
                                            {
                                                // Set the response message for successful cancellation
                                                response = "Reservation Cancelled successfully.";
                                            }
                                        }
                                    }
                                }
                                else if (status.ToLower() == "completed" || status.ToLower() == "cancelled")
                                {
                                    // Throw an exception if attempting to cancel a reservation with invalid status
                                    throw new ReservationException($"Cannot cancel a reservation where status is {status}.");
                                }
                                else
                                {
                                    // Throw an exception if the status is corrupted or unrecognized
                                    throw new ReservationException($"Corrupted status.");
                                }
                            }
                            else
                            {
                                // Throw an exception if the reservation does not exist
                                throw new ReservationException($"Reservation by ID : {reservationId} is not present.");
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
            catch (ReservationException re)
            {
                // Handle custom exception related to reservation
                Console.WriteLine(re.Message);
                Console.Write("\nReturning to the previous menu...");
                Thread.Sleep(4500);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                Console.WriteLine(ex.Message);
                Thread.Sleep(4500);
            }

            // Return the response message
            return response;
        }



        /// <summary>
        /// Calculates the total cost for a reservation based on the daily rate of the vehicle and the duration of the reservation.
        /// </summary>
        /// <param name="startDate">The start date of the reservation.</param>
        /// <param name="endDate">The end date of the reservation.</param>
        /// <param name="vehicleId">The ID of the vehicle for which the total cost is calculated.</param>
        /// <returns>The calculated total cost for the reservation.</returns>
        public decimal CalculateTotalCost(DateTime startDate, DateTime endDate, int vehicleId)
        {
            decimal totalCost = 0;

            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Open the database connection
                    conn.Open();

                    // Retrieve the daily rate of the vehicle
                    using (SqlCommand cmd = new SqlCommand("SELECT DailyRate FROM Vehicle WHERE VehicleID = @VehicleID", conn))
                    {
                        cmd.Parameters.AddWithValue("@VehicleID", vehicleId);

                        // Execute the SELECT command and read the results
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            // Check if the vehicle exists
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    decimal dailyRate = (decimal)dr["DailyRate"];
                                    var numberOfDays = (endDate - startDate).Days;

                                    // Calculate the total cost ensuring it is non-negative
                                    totalCost = Math.Max(0, dailyRate * numberOfDays);
                                }
                            }
                        }
                    }
                }

                return totalCost;
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
                Console.WriteLine(ex.Message);
                Thread.Sleep(4500);
            }

            // Return the calculated total cost
            return totalCost;
        }
    }
}