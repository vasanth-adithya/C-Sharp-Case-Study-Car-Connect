using System;

namespace CarConnect.Entity
{
    internal class Admin
    {
        public int AdminID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime JoinDate { get; set; }

        public Admin() { }

        public Admin(int adminID, string firstName, string lastName, string email, string phoneNumber, string userName, string password, string role, DateTime joinDate)
        {
            AdminID = adminID;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            UserName = userName;
            Password = password;
            Role = role;
            JoinDate = joinDate;
        }

        public override string ToString()
        {
            return $"AdminID        :  {AdminID}\n" +
                   $"FirstName      :  {FirstName}\n" +
                   $"LastName       :  {LastName}\n" +
                   $"Email          :  {Email}\n" +
                   $"PhoneNumber    :  {PhoneNumber}\n" +
                   $"UserName       :  {UserName}\n" +
                   $"Password       :  {Password}\n" +
                   $"Role           :  {Role}\n" +
                   $"JoinDate       :  {JoinDate.ToShortDateString()}";
        }
    }
}
