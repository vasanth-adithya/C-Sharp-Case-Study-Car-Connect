using System;

namespace CarConnect.Entity
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }

        public Customer() { }

        public Customer(int customerID, string firstName, string lastName, string email, string phoneNumber, string userName, string address, string password, DateTime registrationDate)
        {
            CustomerID = customerID;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            UserName = userName;
            Address = address;
            Password = password;
            RegistrationDate = registrationDate;
        }

        public override string ToString()
        {
            return $"  CustomerID       :  {CustomerID}\n" +
                   $"  FirstName        :  {FirstName}\n" +
                   $"  LastName         :  {LastName}\n" +
                   $"  Email            :  {Email}\n" +
                   $"  PhoneNumber      :  {PhoneNumber}\n" +
                   $"  UserName         :  {UserName}\n" +
                   $"  Password         :  {Password}\n" +
                   $"  RegistrationDate :  {RegistrationDate.ToShortDateString()}";
        }
    }
}
