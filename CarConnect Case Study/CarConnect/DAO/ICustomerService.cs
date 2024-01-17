using System.Collections.Generic;
using CarConnect.Entity;

namespace CarConnect.DAO
{
    public interface ICustomerService
    {
        bool Authenticate(string username, string password);

        List<Customer> GetAllCustomers();

        void GetCustomerById(int customerId);

        void GetCustomerByUserName(string username);

        string RegisterCustomer(Customer customerData);

        string UpdateCustomer(Customer customerData);

        string DeleteCustomer(int customerId);
    }
}
