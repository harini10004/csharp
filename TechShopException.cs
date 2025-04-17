using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop
{
    internal class TechShopException:Exception
    {
        public TechShopException(string message) : base(message) { }

        public class InvalidDataException : TechShopException
        {
            public InvalidDataException(string message) : base(message) { }
        }

        public class InsufficientStockException : TechShopException
        {
            public InsufficientStockException(string message) : base(message) { }
        }

        public class IncompleteOrderException : TechShopException
        {
            public IncompleteOrderException(string message) : base(message) { }
        }

        public class DuplicateEntityException : TechShopException
        {
            public DuplicateEntityException(string message) : base(message) { }
        }

        public class EntityNotFoundException : TechShopException
        {
            public EntityNotFoundException(string message) : base(message) { }
        }
    }
}
