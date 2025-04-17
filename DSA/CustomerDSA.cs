using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Model;

namespace TechShop.DSA
{
    internal class CustomerDSA
    {
        private List<Customer> customers;

        public CustomerDSA()
        {
            customers = new List<Customer>();
        }

        public void AddCustomer(Customer customer)
        {
            if (customers.Any(c => c.CustomerId == customer.CustomerId))
            {
                throw new TechShopException.DuplicateEntityException($"Customer with ID {customer.CustomerId} already exists.");
            }
            customers.Add(customer);
        }

        public Customer GetCustomerById(int customerId)
        {
            var customer = customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (customer == null)
            {
                throw new TechShopException.EntityNotFoundException($"Customer with ID {customerId} not found.");
            }
            return customer;
        }

        public List<Customer> GetAllCustomers()
        {
            return customers.ToList();
        }

        public void UpdateCustomer(Customer updatedCustomer)
        {
            var existingCustomer = GetCustomerById(updatedCustomer.CustomerId);
            existingCustomer.FirstName = updatedCustomer.FirstName;
            existingCustomer.LastName = updatedCustomer.LastName;
            existingCustomer.Email = updatedCustomer.Email;
            existingCustomer.Phone = updatedCustomer.Phone;
            existingCustomer.Address = updatedCustomer.Address;
        }

        public void DeleteCustomer(int customerId)
        {
            var customer = GetCustomerById(customerId);
            customers.Remove(customer);
        }

        public int CalculateTotalOrders(int customerId, OrderDSA orderDSA)
        {
            return orderDSA.GetOrdersByCustomer(customerId).Count;
        }
    }
}
