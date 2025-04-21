using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech
{
    public class TechshopException:Exception
    {
        public TechshopException(string message) : base(message) { }
    }
    public class InvalidDataException : TechshopException
    {
        public InvalidDataException(string message) : base(message) { }
    }

    public class InsufficientStockException : TechshopException
    {
        public InsufficientStockException(string message) : base(message) { }
    }

    public class IncompleteOrderException : TechshopException
    {
        public IncompleteOrderException(string message) : base(message) { }
    }

    public class PaymentFailedException : TechshopException
    {
        public PaymentFailedException(string message) : base(message) { }
    }

    public class CustomerNotFoundException : TechshopException
    {
        public CustomerNotFoundException(string message) : base(message) { }
    }

    public class ProductNotFoundException : TechshopException
    {
        public ProductNotFoundException(string message) : base(message) { }
    }

    public class OrderNotFoundException : TechshopException
    {
        public OrderNotFoundException(string message) : base(message) { }
    }
}
