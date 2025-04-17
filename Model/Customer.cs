using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TechShop.Model
{
    internal class Customer
    {
        private int customerId;
        private string firstName;
        private string lastName;
        private string email;
        private string phone;
        private string address;

        public Customer(int customerId, string firstName, string lastName, string email, string phone, string address)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Address = address;
        }

        public int CustomerId
        {
            get { return customerId; }
            set
            {
                if (value <= 0)
                    throw new TechShopException.InvalidDataException("Customer ID must be positive.");
                customerId = value;
            }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new TechShopException.InvalidDataException("First name cannot be empty.");
                firstName = value;
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new TechShopException.InvalidDataException("Last name cannot be empty.");
                lastName = value;
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new TechShopException.InvalidDataException("Email cannot be empty.");

                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(value, pattern))
                    throw new TechShopException.InvalidDataException("Invalid email format.");

                email = value;
            }
        }

        public string Phone
        {
            get { return phone; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new TechShopException.InvalidDataException("Phone cannot be empty.");

                if (!Regex.IsMatch(value, @"^\d{10}$"))
                    throw new TechShopException.InvalidDataException("Phone number must be 10 digits.");

                phone = value;
            }
        }

        public string Address
        {
            get { return address; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new TechShopException.InvalidDataException("Address cannot be empty.");
                address = value;
            }
        }

        public override string ToString()
        {
            return $"Customer ID: {CustomerId}\t First Name: {FirstName}\t Last Name: {LastName}\t Email: {Email}\t Phone: {Phone}\t Address: {Address}";
        }

        public override bool Equals(object obj)
        {
            return obj is Customer other && this.CustomerId == other.CustomerId;
        }

        public override int GetHashCode()
        {
            return CustomerId.GetHashCode();
        }
    }
}
