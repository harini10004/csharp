using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tech.DAO;
using System.Threading.Tasks;
using Tech.Model;

namespace Tech
{
    public class TechShopUI
    {
        private CustomerService customerService;
        private ProductService productService;
        private InventoryService inventoryService;
        private PaymentService paymentService;
        private OrderService orderService;

        public TechShopUI()
        {
            customerService = new CustomerService();
            productService = new ProductService();
            inventoryService = new InventoryService();
            paymentService = new PaymentService();

            // ✅ Now that dependencies are initialized, we can pass them in
            orderService = new OrderService(productService, inventoryService);
        }

        public static class InputHelper
        {
            public static string GetInput(string str)
            {
                Console.Write(str);
                return Console.ReadLine();
            }

            public static int GetIntegerInput(string str)
            {
                while (true)
                {
                    try
                    {
                        Console.Write(str);
                        return int.Parse(Console.ReadLine());
                    }
                    catch (TechshopException)
                    {
                        Console.WriteLine("Invalid input. Please enter a valid integer.");
                    }
                }
            }

            public static decimal GetDecimalInput(string str)
            {
                while (true)
                {
                    try
                    {
                        Console.Write(str);
                        return decimal.Parse(Console.ReadLine());
                    }
                    catch (TechshopException)
                    {
                        Console.WriteLine("Invalid input. Please enter a valid decimal number.");
                    }
                }
            }

            public static DateTime GetDateInput(string str)
            {
                while (true)
                {
                    try
                    {
                        Console.Write(str + " (YYYY-MM-DD): ");
                        return DateTime.Parse(Console.ReadLine());
                    }
                    catch (TechshopException)
                    {
                        Console.WriteLine("Invalid date format. Please use YYYY-MM-DD.");
                    }
                }
            }

            public static bool GetYesNoInput(string str)
            {
                while (true)
                {
                    Console.Write(str + " (Y/N): ");
                    string input = Console.ReadLine().ToUpper();
                    if (input == "Y") return true;
                    if (input == "N") return false;
                    Console.WriteLine("Please enter Y or N.");
                }
            }
        }
        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("TechShop Management System");
                Console.WriteLine("==========================");
                Console.WriteLine("1. Customer Management");
                Console.WriteLine("2. Product Management");
                Console.WriteLine("3. Order Management");
                Console.WriteLine("4. Inventory Management");
                Console.WriteLine("5. Payment Processing");
                Console.WriteLine("6. Reports");
                Console.WriteLine("0. Exit");
                Console.WriteLine("==========================");

                int choice = InputHelper.GetIntegerInput("Enter your choice: ");

                try
                {
                    switch (choice)
                    {
                        case 1:
                            CustomerMenu();
                            break;
                        case 2:
                            ProductMenu();
                            break;
                        case 3:
                            OrderMenu();
                            break;
                        case 4:
                            InventoryMenu();
                            break;
                        case 5:
                            PaymentMenu();
                            break;
                        case 6:
                            ReportsMenu();
                            break;
                        case 0:
                            Console.WriteLine("Exiting TechShop Management System. Goodbye!");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (TechshopException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private void CustomerMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Customer Management");
                Console.WriteLine("==================");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. View Customer Details");
                Console.WriteLine("3. Update Customer Information");
                Console.WriteLine("4. Delete Customer");
                Console.WriteLine("5. List All Customers");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine("==================");

                int choice = InputHelper.GetIntegerInput("Enter your choice: ");

                try
                {
                    switch (choice)
                    {
                        case 1:
                            AddCustomer();
                            break;
                        case 2:
                            ViewCustomer();
                            break;
                        case 3:
                            UpdateCustomer();
                            break;
                        case 4:
                            DeleteCustomer();
                            break;
                        case 5:
                            ListAllCustomers();
                            break;
                        case 0:
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (TechshopException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private void AddCustomer()
        {
            Console.Clear();
            Console.WriteLine("Register New Customer");
            Console.WriteLine("=====================");

            int customerId = InputHelper.GetIntegerInput("Enter Customer ID: ");
            string firstName = InputHelper.GetInput("First Name: ");
            string lastName = InputHelper.GetInput("Last Name: ");
            string email = InputHelper.GetInput("Email: ");
            string phone = InputHelper.GetInput("Phone  ");
            string address = InputHelper.GetInput("Address  ");

            Customer newCustomer = new Customer(firstName, lastName, email, phone, address);
            bool success = customerService.AddCustomer(newCustomer);

            if (success)
            {
                Console.WriteLine("Customer registered successfully!");
            }
        }

        private void ViewCustomer()
        {
            Console.Clear();
            Console.WriteLine("View Customer Details");
            Console.WriteLine("=====================");

            int customerId = InputHelper.GetIntegerInput("Enter Customer ID: ");
            Customer customer = customerService.GetCustomerById(customerId);

            Console.WriteLine("\nCustomer Details:");
            Console.WriteLine($"ID: {customer.CustomerID}");
            Console.WriteLine($"Name: {customer.FirstName} {customer.LastName}");
            Console.WriteLine($"Email: {customer.Email}");
            Console.WriteLine($"Phone: {customer.Phone ?? "Not provided"}");
            Console.WriteLine($"Address: {customer.Address ?? "Not provided"}");
        }

        private void UpdateCustomer()
        {
            Console.Clear();
            Console.WriteLine("Update Customer Information");
            Console.WriteLine("===========================");

            int customerId = InputHelper.GetIntegerInput("Enter Customer ID to update: ");
            Customer existingCustomer = customerService.GetCustomerById(customerId);

            Console.WriteLine("\nCurrent Details:");
            Console.WriteLine($"1. First Name: {existingCustomer.FirstName}");
            Console.WriteLine($"2. Last Name: {existingCustomer.LastName}");
            Console.WriteLine($"3. Email: {existingCustomer.Email}");
            Console.WriteLine($"4. Phone: {existingCustomer.Phone ?? "Not provided"}");
            Console.WriteLine($"5. Address: {existingCustomer.Address ?? "Not provided"}");
            Console.WriteLine("0. Cancel");

            int fieldToUpdate = InputHelper.GetIntegerInput("\nEnter number of field to update (0 to cancel): ");

            if (fieldToUpdate == 0) return;

            Customer updatedCustomer = new Customer
            {
                CustomerID = existingCustomer.CustomerID,
                FirstName = existingCustomer.FirstName,
                LastName = existingCustomer.LastName,
                Email = existingCustomer.Email,
                Phone = existingCustomer.Phone,
                Address = existingCustomer.Address
            };

            switch (fieldToUpdate)
            {
                case 1:
                    updatedCustomer.FirstName = InputHelper.GetInput("Enter new First Name: ");
                    break;
                case 2:
                    updatedCustomer.LastName = InputHelper.GetInput("Enter new Last Name: ");
                    break;
                case 3:
                    updatedCustomer.Email = InputHelper.GetInput("Enter new Email: ");
                    break;
                case 4:
                    updatedCustomer.Phone = InputHelper.GetInput("Enter new Phone: ");
                    break;
                case 5:
                    updatedCustomer.Address = InputHelper.GetInput("Enter new Address: ");
                    break;
                default:
                    Console.WriteLine("Invalid field selection.");
                    return;
            }

            bool success = customerService.UpdateCustomer(updatedCustomer);
            if (success)
            {
                Console.WriteLine("Customer information updated successfully!");
            }
        }

        private void DeleteCustomer()
        {
            Console.Clear();
            Console.WriteLine("Delete Customer");
            Console.WriteLine("==============");

            int customerId = InputHelper.GetIntegerInput("Enter Customer ID to delete: ");

            if (InputHelper.GetYesNoInput("Are you sure you want to delete this customer?"))
            {
                bool success = customerService.DeleteCustomer(customerId);
                if (success)
                {
                    Console.WriteLine("Customer deleted successfully!");
                }
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }

        private void ListAllCustomers()
        {
            Console.Clear();
            Console.WriteLine("All Customers");
            Console.WriteLine("=============");

            List<Customer> customers = customerService.GetAllCustomers();

            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
                return;
            }

            foreach (var customer in customers)
            {
                Console.WriteLine($"ID: {customer.CustomerID}, Name: {customer.FirstName} {customer.LastName}, Email: {customer.Email},Phone:{customer.Phone},Address:{customer.Address}");
            }
        }

        private void ProductMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Product Management");
                Console.WriteLine("=================");
                Console.WriteLine("1. Add New Product");
                Console.WriteLine("2. View Product Details");
                Console.WriteLine("3. Update Product Information");
                Console.WriteLine("4. Remove Product");
                Console.WriteLine("5. List All Products");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine("=================");

                int choice = InputHelper.GetIntegerInput("Enter your choice: ");

                try
                {
                    switch (choice)
                    {
                        case 1:
                            AddProduct();
                            break;
                        case 2:
                            ViewProduct();
                            break;
                        case 3:
                            UpdateProduct();
                            break;
                        case 4:
                            RemoveProduct();
                            break;
                        case 5:
                            ListAllProducts();
                            break;
                        case 0:
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (TechshopException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private void AddProduct()
        {
            Console.Clear();
            Console.WriteLine("Add New Product");
            Console.WriteLine("===============");

            string productName = InputHelper.GetInput("Product Name: ");
            string description = InputHelper.GetInput("Description (optional): ");
            decimal price = InputHelper.GetDecimalInput("Price: ");
            string category = InputHelper.GetInput("Category (optional): ");

            Product newProduct = new Product(productName, description, price, category);
            bool success = productService.AddProduct(newProduct);

            if (success)
            {
                Console.WriteLine("Product added successfully!");
            }
        }

        private void ViewProduct()
        {
            Console.Clear();
            Console.WriteLine("View Product Details");
            Console.WriteLine("===================");

            int productId = InputHelper.GetIntegerInput("Enter Product ID: ");
            Product product = productService.GetProductById(productId);

            Console.WriteLine("\nProduct Details:");
            Console.WriteLine($"ID: {product.ProductID}");
            Console.WriteLine($"Name: {product.ProductName}");
            Console.WriteLine($"Description: {product.Description ?? "Not provided"}");
            Console.WriteLine($"Price: {product.Price:C}");
            Console.WriteLine($"Category: {product.Category ?? "Not categorized"}");

            // Show inventory status
            try
            {
                Inventory inventory = inventoryService.GetInventoryByProductId(productId);
                Console.WriteLine($"Quantity in Stock: {inventory.QuantityInStock}");
                Console.WriteLine($"Last Stock Update: {inventory.LastStockUpdate}");
            }
            catch (TechshopException ex)
            {
                Console.WriteLine($"Inventory information: {ex.Message}");
            }
        }

        private void UpdateProduct()
        {
            Console.Clear();
            Console.WriteLine("Update Product Information");
            Console.WriteLine("=========================");

            int productId = InputHelper.GetIntegerInput("Enter Product ID to update: ");
            Product existingProduct = productService.GetProductById(productId);

            Console.WriteLine("\nCurrent Details:");
            Console.WriteLine($"1. Product Name: {existingProduct.ProductName}");
            Console.WriteLine($"2. Description: {existingProduct.Description ?? "Not provided"}");
            Console.WriteLine($"3. Price: {existingProduct.Price:C}");
            Console.WriteLine($"4. Category: {existingProduct.Category ?? "Not categorized"}");
            Console.WriteLine("0. Cancel");

            int fieldToUpdate = InputHelper.GetIntegerInput("\nEnter number of field to update (0 to cancel): ");

            if (fieldToUpdate == 0) return;

            Product updatedProduct = new Product
            {
                ProductID = existingProduct.ProductID,
                ProductName = existingProduct.ProductName,
                Description = existingProduct.Description,
                Price = existingProduct.Price,
                Category = existingProduct.Category
            };

            switch (fieldToUpdate)
            {
                case 1:
                    updatedProduct.ProductName = InputHelper.GetInput("Enter new Product Name: ");
                    break;
                case 2:
                    updatedProduct.Description = InputHelper.GetInput("Enter new Description: ");
                    break;
                case 3:
                    updatedProduct.Price = InputHelper.GetDecimalInput("Enter new Price: ");
                    break;
                case 4:
                    updatedProduct.Category = InputHelper.GetInput("Enter new Category: ");
                    break;
                default:
                    Console.WriteLine("Invalid field selection.");
                    return;
            }

            bool success = productService.UpdateProduct(updatedProduct);
            if (success)
            {
                Console.WriteLine("Product information updated successfully!");
            }
        }

        private void RemoveProduct()
        {
            Console.Clear();
            Console.WriteLine("Remove Product");
            Console.WriteLine("==============");

            int productId = InputHelper.GetIntegerInput("Enter Product ID to remove: ");

            if (InputHelper.GetYesNoInput("Are you sure you want to remove this product?"))
            {
                bool success = productService.DeleteProduct(productId);
                if (success)
                {
                    Console.WriteLine("Product removed successfully!");
                }
            }
            else
            {
                Console.WriteLine("Removal cancelled.");
            }
        }

        private void ListAllProducts()
        {
            Console.Clear();
            Console.WriteLine("All Products");
            Console.WriteLine("============");

            List<Product> products = productService.GetAllProducts();

            if (products.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }

            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.ProductID}, Name: {product.ProductName}, Price: {product.Price:C}, Category: {product.Category ?? "N/A"}");
            }
        }

       

        private void OrderMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Order Management");
                Console.WriteLine("================");
                Console.WriteLine("1. Place New Order");
                Console.WriteLine("2. View Order Details");
                Console.WriteLine("3. Update Order Status");
                Console.WriteLine("4. Cancel Order");
                Console.WriteLine("5. View Customer Orders");
                Console.WriteLine("6. View Orders by Date Range");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine("================");

                int choice = InputHelper.GetIntegerInput("Enter your choice: ");

                try
                {
                    switch (choice)
                    {
                        case 1:
                            PlaceOrder();
                            break;
                        case 2:
                            ViewOrder();
                            break;
                        case 3:
                            UpdateOrderStatus();
                            break;
                        case 4:
                            CancelOrder();
                            break;
                        case 5:
                            ViewCustomerOrders();
                            break;
                        case 6:
                            ViewOrdersByDateRange();
                            break;
                        case 0:
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (TechshopException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private void PlaceOrder()
        {
            try
            {
                // Get customer ID
                int customerId = InputHelper.GetIntegerInput("Enter Customer ID: ");
                Customer customer = customerService.GetCustomerById(customerId);

                Console.WriteLine($"\nPlacing order for: {customer.FirstName} {customer.LastName}");

                // Create order
                Order newOrder = new Order
                {
                    CustomerID = customerId,
                    OrderDate = DateTime.Now,
                    TotalAmount = 0,
                    Status = "Pending"
                };

                // Collect order details
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                bool addMoreProducts = true;

                while (addMoreProducts)
                {
                    Console.WriteLine("\nAdd Product to Order:");
                    int productId = InputHelper.GetIntegerInput("Enter Product ID: ");
                    Product product = productService.GetProductById(productId);

                    Console.WriteLine($"Selected Product: {product.ProductName}, Price: {product.Price:C}");
                    Inventory inventory = inventoryService.GetInventoryByProductId(productId);
                    Console.WriteLine($"Available in Stock: {inventory.QuantityInStock}");

                    int quantity = InputHelper.GetIntegerInput("Enter Quantity: ");

                    orderDetails.Add(new OrderDetail
                    {
                        ProductID = productId,
                        Quantity = quantity,
                        UnitPrice = product.Price
                    });

                    newOrder.TotalAmount += product.Price * quantity;

                    addMoreProducts = InputHelper.GetYesNoInput("Add another product to this order?");
                }

                // Confirm and place order
                Console.WriteLine($"\nOrder Total: {newOrder.TotalAmount:C}");
                if (InputHelper.GetYesNoInput("Confirm and place this order?"))
                {
                    bool success = orderService.PlaceOrder(newOrder, orderDetails);
                    if (success)
                    {
                        Console.WriteLine("Order placed successfully!");

                       
                    }
                }
            }
            catch (TechshopException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Details: {ex.InnerException.Message}");
                }
            }
        }
        private void ViewOrder()
        {
            Console.Clear();
            Console.WriteLine("View Order Details");
            Console.WriteLine("=================");

            int orderId = InputHelper.GetIntegerInput("Enter Order ID: ");
            Order order = orderService.GetOrderDetails(orderId);
            Customer customer = customerService.GetCustomerById(order.CustomerID);

            Console.WriteLine("\nOrder Summary:");
            Console.WriteLine($"Order ID: {order.OrderID}");
            Console.WriteLine($"Customer: {customer.FirstName} {customer.LastName}");
            Console.WriteLine($"Order Date: {order.OrderDate}");
            Console.WriteLine($"Total Amount: {order.TotalAmount:C}");
            Console.WriteLine($"Status: {order.Status}");

            // Get order details
            Console.WriteLine("\nOrder Items:");
            // Note: In a real implementation, we would have a method to get order details by order ID
            // For this example, we'll simulate it
            Console.WriteLine("(Order details would be displayed here)");

            // Show payments if any
            try
            {
                List<Payment> payments = paymentService.GetPaymentsByOrder(orderId);
                if (payments.Count > 0)
                {
                    Console.WriteLine("\nPayments:");
                    foreach (var payment in payments)
                    {
                        Console.WriteLine($"- {payment.PaymentDate}: {payment.Amount:C} via {payment.PaymentMethod} ({payment.Status})");
                    }
                }
            }
            catch (TechshopException)
            {
                throw new("no Payment found");
            }
        }

        private void UpdateOrderStatus()
        {
            Console.Clear();
            Console.WriteLine("Update Order Status");
            Console.WriteLine("==================");

            int orderId = InputHelper.GetIntegerInput("Enter Order ID: ");
            Order order = orderService.GetOrderDetails(orderId);

            Console.WriteLine($"\nCurrent Status: {order.Status}");
            Console.WriteLine("Available Statuses: Pending, Processing, Shipped, Delivered, Cancelled");
            string newStatus = InputHelper.GetInput("Enter new Status: ");

            bool success = orderService.UpdateOrderStatus(orderId, newStatus);
            if (success)
            {
                Console.WriteLine("Order status updated successfully!");
            }
        }

        private void CancelOrder()
        {
            Console.Clear();
            Console.WriteLine("Cancel Order");
            Console.WriteLine("============");

            int orderId = InputHelper.GetIntegerInput("Enter Order ID to cancel: ");
            Order order = orderService.GetOrderDetails(orderId);

            if (order.Status == "Cancelled")
            {
                Console.WriteLine("This order is already cancelled.");
                return;
            }

            Console.WriteLine($"\nOrder ID: {order.OrderID}");
            Console.WriteLine($"Customer ID: {order.CustomerID}");
            Console.WriteLine($"Total Amount: {order.TotalAmount:C}");
            Console.WriteLine($"Current Status: {order.Status}");

            if (InputHelper.GetYesNoInput("\nAre you sure you want to cancel this order?"))
            {
                bool success = orderService.CancelOrder(orderId);
                if (success)
                {
                    Console.WriteLine("Order cancelled successfully!");
                }
            }
            else
            {
                Console.WriteLine("Order cancellation aborted.");
            }
        }

        private void ViewCustomerOrders()
        {
            Console.Clear();
            Console.WriteLine("Customer Orders");
            Console.WriteLine("==============");

            int customerId = InputHelper.GetIntegerInput("Enter Customer ID: ");
            Customer customer = customerService.GetCustomerById(customerId);

            Console.WriteLine($"\nOrders for: {customer.FirstName} {customer.LastName}");
            List<Order> orders = orderService.GetOrdersByCustomer(customerId);

            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found for this customer.");
                return;
            }

            foreach (var order in orders)
            {
                Console.WriteLine($"\nOrder ID: {order.OrderID}");
                Console.WriteLine($"Date: {order.OrderDate}, Total: {order.TotalAmount:C}, Status: {order.Status}");
            }
        }

        private void ViewOrdersByDateRange()
        {
            Console.Clear();
            Console.WriteLine("Orders by Date Range");
            Console.WriteLine("===================");

            DateTime startDate = InputHelper.GetDateInput("Enter Start Date");
            DateTime endDate = InputHelper.GetDateInput("Enter End Date");

            List<Order> orders = orderService.GetOrdersByDateRange(startDate, endDate);

            if (orders.Count == 0)
            {
                Console.WriteLine($"No orders found between {startDate.ToShortDateString()} and {endDate.ToShortDateString()}.");
                return;
            }

            Console.WriteLine($"\nOrders between {startDate.ToShortDateString()} and {endDate.ToShortDateString()}:");
            foreach (var order in orders)
            {
                Customer customer = customerService.GetCustomerById(order.CustomerID);
                Console.WriteLine($"\nOrder ID: {order.OrderID}");
                Console.WriteLine($"Customer: {customer.FirstName} {customer.LastName}");
                Console.WriteLine($"Date: {order.OrderDate}, Total: {order.TotalAmount:C}, Status: {order.Status}");
            }
        }

        private void InventoryMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Inventory Management");
                Console.WriteLine("===================");
                Console.WriteLine("1. View Inventory Status");
                Console.WriteLine("2. Update Product Stock");
                Console.WriteLine("3. View Low Stock Products");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine("===================");

                int choice = InputHelper.GetIntegerInput("Enter your choice: ");

                try
                {
                    switch (choice)
                    {
                        case 1:
                            ViewInventoryStatus();
                            break;
                        case 2:
                            UpdateProductStock();
                            break;
                        case 3:
                            ViewLowStockProducts();
                            break;
                        case 0:
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (TechshopException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private void ViewInventoryStatus()
        {
            Console.Clear();
            Console.WriteLine("Inventory Status");
            Console.WriteLine("================");

            List<Inventory> inventory = inventoryService.GetAllInventory();

            if (inventory.Count == 0)
            {
                Console.WriteLine("No inventory records found.");
                return;
            }

            foreach (var item in inventory)
            {
                Product product = productService.GetProductById(item.ProductID);
                Console.WriteLine($"\nProduct ID: {item.ProductID}, Name: {product.ProductName}");
                Console.WriteLine($"Quantity in Stock: {item.QuantityInStock}");
                Console.WriteLine($"Last Updated: {item.LastStockUpdate}");
            }
        }

        private void UpdateProductStock()
        {
            Console.Clear();
            Console.WriteLine("Update Product Stock");
            Console.WriteLine("===================");

            int productId = InputHelper.GetIntegerInput("Enter Product ID: ");
            Product product = productService.GetProductById(productId);
            Inventory inventory = inventoryService.GetInventoryByProductId(productId);

            Console.WriteLine($"\nCurrent Stock for {product.ProductName}: {inventory.QuantityInStock}");

            int quantityChange = InputHelper.GetIntegerInput("Enter quantity to add (positive) or remove (negative): ");

            bool success = inventoryService.UpdateStockQuantity(productId, quantityChange);
            if (success)
            {
                Console.WriteLine("Inventory updated successfully!");
                Inventory updatedInventory = inventoryService.GetInventoryByProductId(productId);
                Console.WriteLine($"New stock level: {updatedInventory.QuantityInStock}");
            }
        }

        private void ViewLowStockProducts()
        {
            Console.Clear();
            Console.WriteLine("Low Stock Products");
            Console.WriteLine("=================");

            int threshold = InputHelper.GetIntegerInput("Enter low stock threshold: ");

            try
            {
                List<Inventory> inventories = inventoryService.GetLowStockProducts(threshold);

                if (inventories.Count == 0)
                {
                    Console.WriteLine($"No products with stock below {threshold}.");
                    return;
                }

                Console.WriteLine($"\nProducts with stock below {threshold}:");

                foreach (var inventory in inventories)
                {
                    Product product = productService.GetProductById(inventory.ProductID); // Assuming you have this service
                    Console.WriteLine($"- {product.ProductName}: {inventory.QuantityInStock} in stock");
                }
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void PaymentMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Payment Processing");
                Console.WriteLine("=================");
                Console.WriteLine("1. Process Payment for Order");
                Console.WriteLine("2. View Payment Details");
                Console.WriteLine("3. Update Payment Status");
                Console.WriteLine("4. View Payments for Order");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine("=================");

                int choice = InputHelper.GetIntegerInput("Enter your choice: ");

                try
                {
                    switch (choice)
                    {
                        case 1:
                            ProcessPayment();
                            break;
                        case 2:
                            ViewPayment();
                            break;
                        case 3:
                            UpdatePaymentStatus();
                            break;
                        case 4:
                            ViewPaymentsForOrder();
                            break;
                        case 0:
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (TechshopException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private void ProcessPayment()
        {
            Console.Clear();
            Console.WriteLine("Process Payment");
            Console.WriteLine("==============");

            int orderId = InputHelper.GetIntegerInput("Enter Order ID: ");
            Order order = orderService.GetOrderDetails(orderId);

            if (order.Status == "Cancelled")
            {
                Console.WriteLine("Cannot process payment for cancelled order.");
                return;
            }

            Console.WriteLine($"\nOrder Total: {order.TotalAmount:C}");
            Console.WriteLine("Available Payment Methods: Credit Card, Debit Card, PayPal, Bank Transfer");
            string paymentMethod = InputHelper.GetInput("Enter Payment Method: ");

            ProcessPaymentForOrder(orderId, order.TotalAmount, paymentMethod);
        }

        private void ProcessPaymentForOrder(int orderId, decimal amount, string paymentMethod = null)
        {
            if (paymentMethod == null)
            {
                Console.WriteLine("Available Payment Methods: Credit Card, Debit Card, PayPal, Bank Transfer");
                paymentMethod = InputHelper.GetInput("Enter Payment Method: ");
            }

            bool success = paymentService.ProcessPayment(orderId, amount, paymentMethod);
            if (success)
            {
                Console.WriteLine("Payment processed successfully!");
                orderService.UpdateOrderStatus(orderId, "Paid");
            }
        }

        private void ViewPayment()
        {
            Console.Clear();
            Console.WriteLine("View Payment Details");
            Console.WriteLine("===================");

            int paymentId = InputHelper.GetIntegerInput("Enter Payment ID: ");
            Payment payment = paymentService.GetPaymentDetails(paymentId);
            Order order = orderService.GetOrderDetails(payment.OrderID);

            Console.WriteLine("\nPayment Details:");
            Console.WriteLine($"Payment ID: {payment.PaymentID}");
            Console.WriteLine($"Order ID: {payment.OrderID}");
            Console.WriteLine($"Order Total: {order.TotalAmount:C}");
            Console.WriteLine($"Amount Paid: {payment.Amount:C}");
            Console.WriteLine($"Payment Method: {payment.PaymentMethod}");
            Console.WriteLine($"Payment Date: {payment.PaymentDate}");
            Console.WriteLine($"Status: {payment.Status}");
        }

        private void UpdatePaymentStatus()
        {
            Console.Clear();
            Console.WriteLine("Update Payment Status");
            Console.WriteLine("====================");

            int paymentId = InputHelper.GetIntegerInput("Enter Payment ID: ");
            Payment payment = paymentService.GetPaymentDetails(paymentId);

            Console.WriteLine($"\nCurrent Status: {payment.Status}");
            Console.WriteLine("Available Statuses: Pending, Completed, Failed, Refunded");
            string newStatus = InputHelper.GetInput("Enter new Status: ");

            bool success = paymentService.UpdatePaymentStatus(paymentId, newStatus);
            if (success)
            {
                Console.WriteLine("Payment status updated successfully!");
            }
        }

        private void ViewPaymentsForOrder()
        {
            Console.Clear();
            Console.WriteLine("Payments for Order");
            Console.WriteLine("=================");

            int orderId = InputHelper.GetIntegerInput("Enter Order ID: ");
            Order order = orderService.GetOrderDetails(orderId);

            Console.WriteLine($"\nOrder ID: {order.OrderID}");
            Console.WriteLine($"Order Total: {order.TotalAmount:C}");
            Console.WriteLine($"Order Status: {order.Status}");

            List<Payment> payments = paymentService.GetPaymentsByOrder(orderId);

            if (payments.Count == 0)
            {
                Console.WriteLine("\nNo payments found for this order.");
                return;
            }

            Console.WriteLine("\nPayment History:");
            foreach (var payment in payments)
            {
                Console.WriteLine($"\nPayment ID: {payment.PaymentID}");
                Console.WriteLine($"Amount: {payment.Amount:C}");
                Console.WriteLine($"Method: {payment.PaymentMethod}");
                Console.WriteLine($"Date: {payment.PaymentDate}");
                Console.WriteLine($"Status: {payment.Status}");
            }
        }

        private void ReportsMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Reports");
                Console.WriteLine("=======");
                Console.WriteLine("1. Sales by Date Range");
                Console.WriteLine("2. Customer Order History");
                Console.WriteLine("3. Product Sales Report");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine("=======");

                int choice = InputHelper.GetIntegerInput("Enter your choice: ");

                try
                {
                    switch (choice)
                    {
                        case 1:
                            SalesByDateRange();
                            break;
                        case 2:
                            CustomerOrderHistory();
                            break;
                        case 3:
                            ProductSalesReport();
                            break;
                        case 0:
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (TechshopException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private void SalesByDateRange()
        {
            Console.Clear();
            Console.WriteLine("Sales by Date Range");
            Console.WriteLine("===================");

            DateTime startDate = InputHelper.GetDateInput("Enter Start Date");
            DateTime endDate = InputHelper.GetDateInput("Enter End Date");

            List<Order> orders = orderService.GetOrdersByDateRange(startDate, endDate);

            if (orders.Count == 0)
            {
                Console.WriteLine($"No orders found between {startDate.ToShortDateString()} and {endDate.ToShortDateString()}.");
                return;
            }

            decimal totalSales = 0;
            int orderCount = 0;

            Console.WriteLine($"\nOrders between {startDate.ToShortDateString()} and {endDate.ToShortDateString()}:");
            foreach (var order in orders)
            {
                if (order.Status != "Cancelled")
                {
                    totalSales += order.TotalAmount;
                    orderCount++;
                }

                Customer customer = customerService.GetCustomerById(order.CustomerID);
                Console.WriteLine($"\nOrder ID: {order.OrderID}");
                Console.WriteLine($"Customer: {customer.FirstName} {customer.LastName}");
                Console.WriteLine($"Date: {order.OrderDate}, Total: {order.TotalAmount:C}, Status: {order.Status}");
            }

            Console.WriteLine($"\nSummary:");
            Console.WriteLine($"Total Orders: {orderCount}");
            Console.WriteLine($"Total Sales: {totalSales:C}");
            Console.WriteLine($"Average Order Value: {(orderCount > 0 ? totalSales / orderCount : 0):C}");
        }

        private void CustomerOrderHistory()
        {
            Console.Clear();
            Console.WriteLine("Customer Order History");
            Console.WriteLine("======================");

            int customerId = InputHelper.GetIntegerInput("Enter Customer ID: ");
            Customer customer = customerService.GetCustomerById(customerId);

            Console.WriteLine($"\nOrder History for: {customer.FirstName} {customer.LastName}");
            List<Order> orders = orderService.GetOrdersByCustomer(customerId);

            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found for this customer.");
                return;
            }

            decimal totalSpent = 0;
            int orderCount = 0;

            foreach (var order in orders)
            {
                if (order.Status != "Cancelled")
                {
                    totalSpent += order.TotalAmount;
                    orderCount++;
                }

                Console.WriteLine($"\nOrder ID: {order.OrderID}");
                Console.WriteLine($"Date: {order.OrderDate}, Total: {order.TotalAmount:C}, Status: {order.Status}");
            }

            Console.WriteLine($"\nSummary:");
            Console.WriteLine($"Total Orders: {orderCount}");
            Console.WriteLine($"Total Spent: {totalSpent:C}");
            Console.WriteLine($"Average Order Value: {(orderCount > 0 ? totalSpent / orderCount : 0):C}");
        }

        private void ProductSalesReport()
        {
            Console.Clear();
            Console.WriteLine("Product Sales Report");
            Console.WriteLine("====================");

            // In a real implementation, we would have a method to get product sales data
            // For this example, we'll simulate it by showing all products with their inventory status

            List<Product> products = productService.GetAllProducts();

            if (products.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }

            Console.WriteLine("\nProduct Sales and Inventory Status:");
            foreach (var product in products)
            {
                Inventory inventory = inventoryService.GetInventoryByProductId(product.ProductID);
                Console.WriteLine($"\nProduct ID: {product.ProductID}");
                Console.WriteLine($"Name: {product.ProductName}");
                Console.WriteLine($"Price: {product.Price:C}");
                Console.WriteLine($"In Stock: {inventory.QuantityInStock}");
                Console.WriteLine($"Last Stock Update: {inventory.LastStockUpdate}");
            }
        }
    }
}
