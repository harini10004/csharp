using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.DSA;
using TechShop.Model;

namespace TechShop
{
    internal class TechshopUi
    {
        CustomerDSA customerDSA = new CustomerDSA();
        ProductDSA productDSA = new ProductDSA();
        OrderDSA orderDSA = new OrderDSA();
        OrderDetailDSA orderDetailDSA = new OrderDetailDSA();
        InventoryDSA inventoryDSA = new InventoryDSA();

        public void Run()
        {
            string input;
            do
            {
                Console.WriteLine("\nTechShop Management System");
                Console.WriteLine("1. Customer Management");
                Console.WriteLine("2. Product Management");
                Console.WriteLine("3. Order Management");
                Console.WriteLine("4. Inventory Management");
                Console.WriteLine("5. Exit");
                Console.Write("Select option: ");

                input = Console.ReadLine();
                switch (input)
                {
                    case "1": CustomerMenu(); break;
                    case "2": ProductMenu(); break;
                    case "3": OrderMenu(); break;
                    case "4": InventoryMenu(); break;
                    case "5": return;
                    default: Console.WriteLine("Invalid option"); break;
                }
            } while (input != "5");
        }

        private void CustomerMenu()
        {
            string input;
            do
            {
                Console.WriteLine("\nCustomer Management");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. View Customer");
                Console.WriteLine("3. View All Customers");
                Console.WriteLine("4. Update Customer");
                Console.WriteLine("5. Delete Customer");
                Console.WriteLine("6. View Customer Orders");
                Console.WriteLine("7. Back to Main Menu");
                Console.Write("Select option: ");

                input = Console.ReadLine();
                try
                {
                    switch (input)
                    {
                        case "1": AddCustomer(); break;
                        case "2": ViewCustomer(); break;
                        case "3": ViewAllCustomers(); break;
                        case "4": UpdateCustomer(); break;
                        case "5": DeleteCustomer(); break;
                        case "6": ViewCustomerOrders(); break;
                        case "7": break;
                        default: Console.WriteLine("Invalid option"); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            } while (input != "7");
        }

        private void AddCustomer()
        {
            Console.Write("Customer ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("First Name: ");
            string firstName = Console.ReadLine();
            Console.Write("Last Name: ");
            string lastName = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Phone: ");
            string phone = Console.ReadLine();
            Console.Write("Address: ");
            string address = Console.ReadLine();

            customerDSA.AddCustomer(new Customer(id, firstName, lastName, email, phone, address));
            Console.WriteLine("Customer added successfully!");
        }

        private void ViewCustomer()
        {
            Console.Write("Enter Customer ID: ");
            int id = int.Parse(Console.ReadLine());
            var customer = customerDSA.GetCustomerById(id);
            Console.WriteLine(customer);
            Console.WriteLine($"Total Orders: {customerDSA.CalculateTotalOrders(id, orderDSA)}");
        }

        private void ViewAllCustomers()
        {
            foreach (var customer in customerDSA.GetAllCustomers())
            {
                Console.WriteLine(customer);
            }
        }

        private void UpdateCustomer()
        {
            Console.Write("Enter Customer ID to update: ");
            int id = int.Parse(Console.ReadLine());
            var customer = customerDSA.GetCustomerById(id);

            Console.Write("First Name");
            string firstName = Console.ReadLine();
            Console.Write("Last Name ");
            string lastName = Console.ReadLine();
            Console.Write("Email ");
            string email = Console.ReadLine();
            Console.Write("Phone");
            string phone = Console.ReadLine();
            Console.Write("Address ");
            string address = Console.ReadLine();

            var updated = new Customer(
                id,
                string.IsNullOrEmpty(firstName) ? customer.FirstName : firstName,
                string.IsNullOrEmpty(lastName) ? customer.LastName : lastName,
                string.IsNullOrEmpty(email) ? customer.Email : email,
                string.IsNullOrEmpty(phone) ? customer.Phone : phone,
                string.IsNullOrEmpty(address) ? customer.Address : address
            );

            customerDSA.UpdateCustomer(updated);
            Console.WriteLine("Customer updated successfully!");
        }

        private void DeleteCustomer()
        {
            Console.Write("Enter Customer ID to delete: ");
            int id = int.Parse(Console.ReadLine());
            customerDSA.DeleteCustomer(id);
            Console.WriteLine("Customer deleted successfully!");
        }

        private void ViewCustomerOrders()
        {
            Console.Write("Enter Customer ID: ");
            int id = int.Parse(Console.ReadLine());
            var orders = orderDSA.GetOrdersByCustomer(id);
            foreach (var order in orders)
            {
                Console.WriteLine(order);
            }
        }

        private void ProductMenu()
        {
            string input;
            do
            {
                Console.WriteLine("\nProduct Management");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. View Product");
                Console.WriteLine("3. View All Products");
                Console.WriteLine("4. Update Product");
                Console.WriteLine("5. Delete Product");
                Console.WriteLine("6. Search Products");
                Console.WriteLine("7. Back to Main Menu");
                Console.Write("Select option: ");

                input = Console.ReadLine();
                try
                {
                    switch (input)
                    {
                        case "1": AddProduct(); break;
                        case "2": ViewProduct(); break;
                        case "3": ViewAllProducts(); break;
                        case "4": UpdateProduct(); break;
                        case "5": DeleteProduct(); break;
                        case "6": SearchProducts(); break;
                        case "7": break;
                        default: Console.WriteLine("Invalid option"); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            } while (input != "7");
        }

        private void AddProduct()
        {
            Console.WriteLine("Product ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Product Name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Description: ");
            string desc = Console.ReadLine();
            Console.WriteLine("Category");
            string category = Console.ReadLine();
            Console.WriteLine("Price: ");
            decimal price = decimal.Parse(Console.ReadLine());

            var product = new Product(id, name, category, price, desc);
            productDSA.AddProduct(product);

            inventoryDSA.AddInventoryItem(new Inventory(
                inventoryDSA.GetAllInventoryItems().Count + 1,
                product,
                0,
                DateTime.Now
            ));

            Console.WriteLine("Product added successfully!");
        }

        private void ViewProduct()
        {
            Console.Write("Enter Product ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine(productDSA.GetProductById(id));
        }

        private void ViewAllProducts()
        {
            foreach (var product in productDSA.GetAllProducts())
            {
                Console.WriteLine(product);
            }
        }

        private void UpdateProduct()
        {
            Console.Write("Enter Product ID to update: ");
            int id = int.Parse(Console.ReadLine());
            var product = productDSA.GetProductById(id);
            Console.WriteLine("Product name");
            string name = Console.ReadLine();
            Console.WriteLine("Description");
            string desc = Console.ReadLine();
            Console.WriteLine("Category");
            string category = Console.ReadLine();
            Console.WriteLine("Price");
            decimal priceInput = Convert.ToDecimal(Console.ReadLine());

            var updated = new Product(
                id,
                string.IsNullOrEmpty(name) ? product.ProductName : name,
                string.IsNullOrEmpty(category) ? product.Category : category,
                product.Price = priceInput,
                string.IsNullOrEmpty(desc) ? product.Description : desc
            );

            productDSA.UpdateProduct(updated);
            Console.WriteLine("Product updated successfully!");
        }

        private void DeleteProduct()
        {
            Console.Write("Enter Product ID to delete: ");
            int id = int.Parse(Console.ReadLine());
            productDSA.DeleteProduct(id);
            Console.WriteLine("Product deleted successfully!");
        }

        private void SearchProducts()
        {
            Console.Write("Enter search term: ");
            string term = Console.ReadLine();
            var results = productDSA.SearchProducts(term);
            foreach (var product in results)
            {
                Console.WriteLine(product);
            }
        }

        private void OrderMenu()
        {
            string input;
            do
            {
                Console.WriteLine("\nOrder Management");
                Console.WriteLine("1. Create Order");
                Console.WriteLine("2. View Order");
                Console.WriteLine("3. View All Orders");
                Console.WriteLine("4. Update Order Status");
                Console.WriteLine("5. Cancel Order");
                Console.WriteLine("6. Add Product to OrderDetail");
                Console.WriteLine("7. View Order Details");
                Console.WriteLine("8. Back to Main Menu");
                Console.Write("Select option: ");

                input = Console.ReadLine();
                try
                {
                    switch (input)
                    {
                        case "1": CreateOrder(); break;
                        case "2": ViewOrder(); break;
                        case "3": ViewAllOrders(); break;
                        case "4": UpdateOrderStatus(); break;
                        case "5": CancelOrder(); break;
                        case "6": AddOrderDetail(); break;
                        case "7": ViewOrderDetails(); break;
                        case "8": break;
                        default: Console.WriteLine("Invalid option"); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            } while (input != "8");
        }

        private void CreateOrder()
        {
            Console.Write("Order ID: ");
            int orderId = int.Parse(Console.ReadLine());
            Console.Write("Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());

            var customer = customerDSA.GetCustomerById(customerId);
            var order = new Order(orderId, customer, DateTime.Now, 0, "Pending");

            orderDSA.AddOrder(order);
            Console.WriteLine("Order created successfully!");
        }

        private void ViewOrder()
        {
            Console.Write("Enter Order ID: ");
            int id = int.Parse(Console.ReadLine());
            var order = orderDSA.GetOrderById(id);
            Console.WriteLine(order);
            Console.WriteLine($"Total Amount: {orderDSA.CalculateTotalAmount(id, orderDetailDSA):C}");
        }

        private void ViewAllOrders()
        {
            foreach (var order in orderDSA.GetAllOrders())
            {
                Console.WriteLine(order);
            }
        }

        private void UpdateOrderStatus()
        {
            Console.Write("Enter Order ID to update: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Enter new status (e.g., Completed, Canceled): ");
            string status = Console.ReadLine();

            orderDSA.UpdateOrderStatus(id, status);
            Console.WriteLine("Order status updated successfully!");
        }

        private void CancelOrder()
        {
            Console.Write("Enter Order ID to cancel: ");
            int id = int.Parse(Console.ReadLine());
            orderDSA.CancelOrder(id);
            Console.WriteLine("Order canceled successfully!");
        }

        private void AddOrderDetail()
        {
            Console.Write("Enter Order ID: ");
            int orderId = int.Parse(Console.ReadLine());
            Console.Write("Enter Product ID: ");
            int productId = int.Parse(Console.ReadLine());
            Console.Write("Enter Quantity: ");
            int quantity = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter discount");
            decimal discount = decimal.Parse(Console.ReadLine());

            var product = productDSA.GetProductById(productId);
            var order = orderDSA.GetOrderById(orderId);
            var orderDetail = new OrderDetail(orderId, order, product, quantity, discount);

            orderDetailDSA.AddOrderDetail(orderDetail);
            Console.WriteLine("Product added to order successfully!");
        }

        private void ViewOrderDetails()
        {
            Console.Write("Enter Order ID: ");
            int orderId = int.Parse(Console.ReadLine());
            var orderDetails = orderDetailDSA.GetOrderDetailById(orderId);
            Console.Write(orderDetails.ToString());
        }

        private void InventoryMenu()
        {
            string input;
            do
            {
                Console.WriteLine("\nInventory Management");
                Console.WriteLine("1. Add Inventory");
                Console.WriteLine("2. View Inventory");
                Console.WriteLine("3. View All Inventory");
                Console.WriteLine("4. Update Inventory");
                Console.WriteLine("5. Delete Inventory");
                Console.WriteLine("6. View Low/Out of Stock Products");
                Console.WriteLine("7. Back to Main Menu");
                Console.Write("Select option: ");

                input = Console.ReadLine();
                try
                {
                    switch (input)
                    {
                        case "1": AddInventory(); break;
                        case "2": ViewInventory(); break;
                        case "3": ViewAllInventory(); break;
                        case "4": UpdateInventory(); break;
                        case "5": DeleteInventory(); break;
                        case "6": ViewLowStockProducts(); break;
                        case "7": break;
                        default: Console.WriteLine("Invalid option"); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            } while (input != "7");
        }

        private void AddInventory()
        {
            Console.Write("Product ID: ");
            int productId = int.Parse(Console.ReadLine());
            Console.Write("Stock Quantity: ");
            int quantity = int.Parse(Console.ReadLine());

            var product = productDSA.GetProductById(productId);
            var inventory = new Inventory(
                inventoryDSA.GetAllInventoryItems().Count + 1,
                product,
                quantity,
                DateTime.Now
            );

            inventoryDSA.AddInventoryItem(inventory);
            Console.WriteLine("Inventory added successfully!");
        }

        private void ViewInventory()
        {
            Console.Write("Enter Inventory ID: ");
            int id = int.Parse(Console.ReadLine());
            var inventory = inventoryDSA.GetInventoryById(id);
            Console.WriteLine(inventory);
        }

        private void ViewAllInventory()
        {
            foreach (var inventory in inventoryDSA.GetAllInventoryItems())
            {
                Console.WriteLine(inventory);
            }
        }

        private void UpdateInventory()
        {
            Console.Write("Enter Inventory ID to update: ");
            int id = int.Parse(Console.ReadLine());
            var inventory = inventoryDSA.GetInventoryById(id);

            Console.Write($"Quantity ({inventory.QuantityInStock}): ");
            int quantity = int.Parse(Console.ReadLine());

            var updated = new Inventory(
                id,
                inventory.Product,
                quantity,
                inventory.LastStockUpdate
            );

            inventoryDSA.AddInventoryItem(updated);
            Console.WriteLine("Inventory updated successfully!");
        }

        private void DeleteInventory()
        {
            Console.Write("Enter Inventory ID to delete: ");
            int id = int.Parse(Console.ReadLine());
            inventoryDSA.DeleteInventoryItem(id);
            Console.WriteLine("Inventory deleted successfully!");
        }

        private void ViewLowStockProducts()
        {
            foreach (var inventory in inventoryDSA.ListOutOfStockProducts())
            {
                Console.WriteLine(inventory);
            }
        }
    }
}
