using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using CarConnect.Entity;
using CarConnect.Exceptions;
using CarConnect.Util;

namespace CarConnect.DAO
{
    internal class AdminService : IAdminService
    {
        /// <summary>
        /// Authenticates an admin using the provided username and password.
        /// </summary>
        /// <param name="username">Username of the admin.</param>
        /// <param name="password">Password of the admin.</param>
        /// <returns>True if authentication is successful, false otherwise.</returns>

        public bool Authenticate(string username, string password)
        {
            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Open the database connection
                    conn.Open();

                    // Prepare SQL command for selecting admin based on the provided username
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Admin WHERE username = @username", conn))
                    {
                        // Set parameter for the SQL command
                        cmd.Parameters.AddWithValue("@username", username);

                        // Execute the SQL command and get the results using a DataReader
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            // Check if there are rows and read the first row
                            if (dr.HasRows && dr.Read())
                            {
                                // Get the stored password from the database
                                string storedPassword = dr["Password"].ToString(); // Assuming it's stored as a hashed value

                                // Check if the provided password matches the stored password
                                if (password == storedPassword)
                                {
                                    Console.Write("Admin authentication successful! Logged IN.");
                                    Thread.Sleep(2000);
                                    return true;
                                }
                                else
                                {
                                    // Throw an exception if the password is invalid
                                    throw new AuthenticationException("Invalid password.");
                                }
                            }
                            else
                            {
                                // Throw an exception if the admin username is invalid
                                throw new AuthenticationException("Invalid admin username.");
                            }
                        }
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
            }
            catch (AuthenticationException ae)
            {
                // Handle exception for authentication failure
                Console.WriteLine(ae.Message);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                Console.WriteLine($"Authentication Error: {ex.Message}");
            }

            // Display a message and return false if authentication fails
            Console.Write("\nReturning to previous menu...");
            Thread.Sleep(3000);
            return false;
        }



        /// <summary>
        /// Retrieves a list of all admins from the database.
        /// </summary>
        /// <returns>A list of Admin objects representing all admins in the database.</returns>

        public List<Admin> GetAllAdmins()
        {
            // Create a list to store admin objects
            List<Admin> admins = new List<Admin>();

            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Prepare SQL command for selecting all admins
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Admin", conn))
                    {
                        // Open the database connection
                        conn.Open();

                        // Execute the SQL command and get the results using a DataReader
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            // Check if there are rows in the result set
                            if (dr.HasRows)
                            {
                                // Loop through each row and create Admin objects
                                while (dr.Read())
                                {
                                    admins.Add(new Admin()
                                    {
                                        AdminID = (int)dr["AdminID"],
                                        FirstName = dr["FirstName"].ToString(),
                                        LastName = dr["LastName"].ToString(),
                                        Email = dr["Email"].ToString(),
                                        PhoneNumber = dr["PhoneNumber"].ToString(),
                                        UserName = dr["UserName"].ToString(),
                                        Password = dr["Password"].ToString(),
                                        Role = dr["Role"].ToString(),
                                        JoinDate = (DateTime)dr["JoinDate"]
                                    });
                                }

                                // Return the list of admins
                                return admins;
                            }
                            else
                            {
                                // Throw an exception if the Admin table is empty
                                throw new AdminNotFoundException("Admin Table is empty.");
                            }
                        }
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
                    throw new DatabaseConnectionException("Error connecting to Database.\nIncorrect Server Name.");
                }
                else
                {
                    // Handle other SQL exceptions
                    Console.WriteLine("An error occurred: " + se.Message);
                }
            }
            catch (DatabaseConnectionException dce)
            {
                // Handle custom exception related to database connection
                Console.WriteLine(dce.Message);
            }
            catch (AdminNotFoundException cnfe)
            {
                // Handle exception if no admins are found in the database
                Console.WriteLine(cnfe.Message);
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                Console.WriteLine(ex.Message);
            }

            // Display a message and return the list of admins (even if it's empty)
            Console.Write("\nReturning to previous menu...");
            Thread.Sleep(4000);

            return admins;
        }



        /// <summary>
        /// Retrieves and displays details of an admin based on the provided admin ID.
        /// </summary>
        /// <param name="adminId">The ID of the admin to retrieve.</param>

        public void GetAdminById(int adminId)
        {
            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Prepare SQL command for selecting admin details by ID
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Admin WHERE AdminID = @adminId", conn))
                    {
                        // Add adminId as a parameter to the SQL command
                        cmd.Parameters.AddWithValue("@adminId", adminId);

                        try
                        {
                            // Open the database connection
                            conn.Open();

                            // Execute the SQL command and get the results using a DataReader
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                // Check if there are rows in the result set
                                if (dr.HasRows)
                                {
                                    // Clear the console and display admin details
                                    Console.Clear();
                                    Console.WriteLine($"\t\t\t\tDetails of Admin ID : {adminId}\n");

                                    // Loop through each row and display admin details
                                    while (dr.Read())
                                    {
                                        DateTime joinDate = (DateTime)dr["JoinDate"];

                                        Console.WriteLine($"  FirstName         = {dr["FirstName"]}");
                                        Console.WriteLine($"  LastName          = {dr["LastName"]}");
                                        Console.WriteLine($"  Email             = {dr["Email"]}");
                                        Console.WriteLine($"  PhoneNumber       = {dr["PhoneNumber"]}");
                                        Console.WriteLine($"  UserName          = {dr["UserName"]}");
                                        Console.WriteLine($"  Password          = {dr["Password"]}");
                                        Console.WriteLine($"  Role              = {dr["Role"]}");
                                        Console.WriteLine($"  JoinDate          = {joinDate.ToShortDateString()}");
                                    }

                                    // Display a message and wait for a key press to return to the previous menu
                                    Console.Write("\n\n\n\nPress any key to return to the previous menu...");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    // Throw an exception if no admin is found with the provided ID
                                    throw new AdminNotFoundException($"No Admin is present with ID: {adminId}.");
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
            catch (AdminNotFoundException anfe)
            {
                // Handle exception if no admin is found with the provided ID
                Console.WriteLine(anfe.Message);
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
        /// Retrieves and displays details of an admin based on the provided username.
        /// </summary>
        /// <param name="username">The username of the admin to retrieve.</param>

        public void GetAdminByUserName(string username)
        {
            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Prepare SQL command for selecting admin details by username
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Admin WHERE UserName = @username", conn))
                    {
                        // Add username as a parameter to the SQL command
                        cmd.Parameters.AddWithValue("@username", username);

                        try
                        {
                            // Open the database connection
                            conn.Open();

                            // Execute the SQL command and get the results using a DataReader
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                // Check if there are rows in the result set
                                if (dr.HasRows)
                                {
                                    // Clear the console and display admin details
                                    Console.Clear();
                                    Console.WriteLine($"\t\t\t\tDetails of Admin: {username}\n");

                                    // Loop through each row and display admin details
                                    while (dr.Read())
                                    {
                                        DateTime joinDate = (DateTime)dr["JoinDate"];

                                        Console.WriteLine($"  AdminID           = {dr["AdminID"]}");
                                        Console.WriteLine($"  FirstName         = {dr["FirstName"]}");
                                        Console.WriteLine($"  LastName          = {dr["LastName"]}");
                                        Console.WriteLine($"  Email             = {dr["Email"]}");
                                        Console.WriteLine($"  PhoneNumber       = {dr["PhoneNumber"]}");
                                        Console.WriteLine($"  Password          = {dr["Password"]}");
                                        Console.WriteLine($"  Role              = {dr[7]}"); // Assuming "Role" is at index 7
                                        Console.WriteLine($"  JoinDate          = {joinDate.ToShortDateString()}");
                                    }

                                    // Display a message and wait for a key press to return to the previous menu
                                    Console.Write("\n\n\n\nPress any key to return to the previous menu...");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    // Throw an exception if no admin is found with the provided username
                                    throw new AdminNotFoundException($"No admin is present with the username: {username}.");
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
            catch (AdminNotFoundException ex)
            {
                // Handle exception if no admin is found with the provided username
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
        /// Registers a new admin in the system with the provided admin data.
        /// </summary>
        /// <param name="adminData">The data of the admin to be registered.</param>
        /// <returns>A string indicating the success of the registration along with the new admin ID.</returns>

        public string RegisterAdmin(Admin adminData)
        {
            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Prepare SQL command for inserting admin data and retrieving the new admin ID
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO admin OUTPUT INSERTED.adminid VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @UserName, @Password, @Role, @JoinDate);", conn))
                    {
                        // Add parameters for admin data to the SQL command
                        cmd.Parameters.AddWithValue("@FirstName", adminData.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", adminData.LastName);
                        cmd.Parameters.AddWithValue("@Email", adminData.Email);
                        cmd.Parameters.AddWithValue("@PhoneNumber", adminData.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Role", adminData.Role);
                        cmd.Parameters.AddWithValue("@UserName", adminData.UserName);
                        cmd.Parameters.AddWithValue("@Password", adminData.Password);
                        cmd.Parameters.AddWithValue("@JoinDate", adminData.JoinDate);

                        try
                        {
                            // Open the database connection
                            conn.Open();
                            // Execute the SQL command and get the result (new admin ID)
                            object newId = cmd.ExecuteScalar();

                            // Check if a new admin ID is obtained
                            if (newId != null)
                            {
                                // Return a success message with the new admin ID
                                return $"Admin Registration Successful. ID: {newId}";
                            }
                        }
                        catch (SqlException se)
                        {
                            // Handle specific SQL exceptions related to duplicate admin username
                            if (se.Class == 14)
                            {
                                Console.WriteLine("Admin Username already exists. Please enter another username");
                                Console.Write("\nReturning to the previous menu...");
                                Thread.Sleep(3000);
                            }
                            else
                            {
                                // Throw other SQL exceptions
                                throw se;
                            }
                        }
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
                Thread.Sleep(3000);
            }

            // Return null if the registration is not successful
            return null;
        }



        /// <summary>
        /// Updates the details of an admin in the system based on the provided admin data.
        /// </summary>
        /// <param name="adminData">The updated data of the admin.</param>
        /// <returns>A string indicating the success of the update.</returns>

        public string UpdateAdmin(Admin adminData)
        {
            string response = null;

            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Prepare SQL command for updating admin details
                    using (SqlCommand cmd = new SqlCommand("UPDATE admin SET PhoneNumber=@PhoneNumber, Role=@Role, FirstName=@FirstName, LastName=@LastName WHERE Username=@Username;", conn))
                    {
                        // Add parameters for admin data to the SQL command
                        cmd.Parameters.AddWithValue("@PhoneNumber", adminData.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Role", adminData.Role);
                        cmd.Parameters.AddWithValue("@Username", adminData.UserName);
                        cmd.Parameters.AddWithValue("@FirstName", adminData.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", adminData.LastName);

                        try
                        {
                            // Open the database connection
                            conn.Open();
                            // Execute the SQL command and get the number of updated rows
                            int updatedRows = cmd.ExecuteNonQuery();

                            // Check if any rows were updated
                            if (updatedRows > 0)
                            {
                                // Set success response
                                response = "Admin details updated successfully.";
                            }
                            else
                            {
                                // Throw an exception if no rows were updated (admin not found)
                                throw new AdminNotFoundException("Invalid Admin Username. Please enter a valid username to update the details.");
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
            catch (AdminNotFoundException e)
            {
                // Handle custom exception related to admin not found
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

            // Return the response indicating the success of the update
            return response;
        }



        /// <summary>
        /// Deletes an admin from the system based on the provided admin ID.
        /// </summary>
        /// <param name="adminId">The ID of the admin to be deleted.</param>
        /// <returns>A string indicating the success of the deletion.</returns>

        public string DeleteAdmin(int adminId)
        {
            string response = null;

            try
            {
                // Establish a connection to the database
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    // Prepare SQL command for deleting admin based on admin ID
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Admin WHERE AdminID = @adminId", conn))
                    {
                        // Add parameter for admin ID to the SQL command
                        cmd.Parameters.AddWithValue("@adminId", adminId);

                        try
                        {
                            // Open the database connection
                            conn.Open();
                            // Execute the SQL command and get the number of deleted rows
                            int deleteRows = cmd.ExecuteNonQuery();

                            // Check if any rows were deleted
                            if (deleteRows > 0)
                            {
                                // Set success response
                                response = $"Data of admin having ID: {adminId} has been deleted successfully.";
                            }
                            else
                            {
                                // Throw an exception if no rows were deleted (admin not found)
                                throw new AdminNotFoundException($"Admin by ID: {adminId} is not present.");
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
            catch (AdminNotFoundException e)
            {
                // Handle custom exception related to admin not found
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

            // Return the response indicating the success of the deletion
            return response;
        }

    }
}
