using System.Collections.Generic;
using CarConnect.Entity;

namespace CarConnect.DAO
{
    internal interface IAdminService
    {
        bool Authenticate(string username, string password);

        List<Admin> GetAllAdmins();

        void GetAdminById(int adminId);

        void GetAdminByUserName(string userName);

        string RegisterAdmin(Admin adminData);

        string UpdateAdmin(Admin adminData);

        string DeleteAdmin(int adminId);
    }
}
