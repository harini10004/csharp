using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Model;

namespace Tech.DAO
{
    public interface ICustomerService
    {
        bool AddCustomer(Customer customer);
        Customer GetCustomerById(int customerId);
        List<Customer> GetAllCustomers();
        bool UpdateCustomer(Customer customer);
        bool DeleteCustomer(int customerId);
        List<Customer> GetCustomersByCity(string city);
        int CalculateTotalOrders(int customerId);
    }
}
