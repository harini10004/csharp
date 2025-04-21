using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tech.Model
{
    public class Customer
    {
        private int customerID;
        private string firstName;
        private string lastName;
        private string email;
        private string phone;
        private string address;
        public Customer() { }
        public Customer(string firstName, string lastName, string email, string phone, string address)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Address = address;
        }

        public int CustomerID
        {
            get { return customerID; }
            set { customerID = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("First name cannot be empty");
                firstName = value;
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Last name cannot be empty");
                lastName = value;
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (!IsValidEmail(value))
                    throw new ArgumentException("Invalid email format");
                email = value;
            }
        }

        public string Phone
        {
            get { return phone; }
            set
            {
                if (!string.IsNullOrEmpty(value) && !IsValidPhone(value))
                    throw new ArgumentException("Invalid phone number format");
                phone = value;
            }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^\d{10}$");
        }

        public override string ToString()
        {
            return $"CustomerID: {CustomerID}, Name: {FirstName} {LastName}, Email: {Email}, Phone: {Phone}, Address: {Address}";
        }
    }
}
