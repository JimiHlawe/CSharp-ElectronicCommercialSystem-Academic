using FinalProject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    enum ProductCategory { Clothing, Electronics, Kids, Office, Undefined };
    enum PackagingOptions { Regular, Special };

    internal class Program
    {
        static void Main(string[] args)
        {
            SystemManager manager = new SystemManager("Electronic Commercial System");

            while (true)
            {
                DisplayMenu();
                try
                {
                    int choice = int.Parse(Console.ReadLine());
                    ExecuteChoice(choice, manager);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input format. Please enter a valid number.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("\t********************");
            Console.WriteLine("Welcome to the Electronic Commercial System!");
            Console.WriteLine("1. Add Buyer");
            Console.WriteLine("2. Add Seller");
            Console.WriteLine("3. Add Product to Seller");
            Console.WriteLine("4. Add Product to Buyer's Cart");
            Console.WriteLine("5. Complete Order for Buyer");
            Console.WriteLine("6. Display All Buyers");
            Console.WriteLine("7. Display All Sellers");
            Console.WriteLine("8. Create New Cart from Order History");
            Console.WriteLine("9. Compare Two Buyers");
            Console.WriteLine("10. Exit");
            Console.Write("Enter a choice: ");
        }

        static void ExecuteChoice(int choice, SystemManager manager)
        {
            if (choice == 1)
            {
                AddUser(manager, true);
            }
            else if (choice == 2)
            {
                AddUser(manager, false);
            }
            else if (choice == 3)
            {
                AddProductToSeller(manager);
            }
            else if (choice == 4)
            {
                AddProductToBuyerCart(manager);
            }
            else if (choice == 5)
            {
                CompleteOrderForBuyer(manager);
            }
            else if (choice == 6)
            {
                DisplayAllUsers(manager, true, true);
            }
            else if (choice == 7)
            {
                DisplayAllUsers(manager, false, false);
            }
            else if (choice == 8)
            {
                CreateNewCartFromOrderHistory(manager);
            }
            else if (choice == 9)
            {
                CompareTwoBuyers(manager);
            }
            else if (choice == 10)
            {
                Console.WriteLine("Exiting the system...");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid choice. Enter a number between 1-10.");
            }
        }

        static void AddUser(SystemManager manager, bool isBuyer)
        {
            string userType = isBuyer ? "buyer" : "seller";

            string username;
            while (true)
            {
                try
                {
                    Console.WriteLine($"Please Enter {userType}'s username:");
                    username = Console.ReadLine();
                    manager.ValidateUsername(username);
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            string password;
            while (true)
            {
                try
                {
                    Console.WriteLine("Please Enter password:");
                    password = Console.ReadLine();
                    manager.ValidatePassword(password);
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            string country;
            while (true)
            {
                try
                {
                    Console.WriteLine("Please Enter country:");
                    country = Console.ReadLine();
                    Address.ValidateCountry(country);
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            string city;
            while (true)
            {
                try
                {
                    Console.WriteLine("Please Enter city:");
                    city = Console.ReadLine();
                    Address.ValidateCity(city);
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            string street;
            while (true)
            {
                try
                {
                    Console.WriteLine("Please Enter street:");
                    street = Console.ReadLine();
                    Address.ValidateStreet(street);
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            int buildingNumber;
            while (true)
            {
                try
                {
                    Console.WriteLine("Enter building number:");
                    buildingNumber = int.Parse(Console.ReadLine());
                    Address.ValidateBuildingNumber(buildingNumber);
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid building number. Please enter a valid number.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            Address address = new Address(country, city, street, buildingNumber);
            if (manager.AddUser(username, password, address, isBuyer))
            {
                Console.WriteLine($"{userType} successfully added.");
            }
            else
            {
                Console.WriteLine("Failed to add user.");
            }
        }

        static void AddProductToSeller(SystemManager manager)
        {
            Console.WriteLine("Enter the seller's username:");
            string username = Console.ReadLine();
            if (!manager.SellerExists(username))
            {
                Console.WriteLine("Seller not found.");
                return;
            }

            Console.WriteLine("Enter the product name:");
            string productName = Console.ReadLine();

            if (manager.ProductExistsForSeller(username, productName))
            {
                Console.WriteLine("Product already exists in the seller's inventory.");
                return;
            }

            ProductCategory category;
            while (true)
            {
                try
                {
                    Console.WriteLine("Enter the product category (1: Clothing, 2: Electronics, 3: Kids, 4: Office):");
                    string categoryInput = Console.ReadLine();
                    category = manager.ParseProductCategoryInput(categoryInput);
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            decimal price;
            while (true)
            {
                try
                {
                    Console.WriteLine("Enter the product price:");
                    price = decimal.Parse(Console.ReadLine());
                    manager.ValidatePrice(price);
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid price format. Please enter a valid number.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            PackagingOptions packagingOption;
            while (true)
            {
                try
                {
                    Console.WriteLine("Is the product special? (1: Regular, 2: Special):");
                    string specialPackage = Console.ReadLine();
                    packagingOption = manager.ParsePackagingOptionInput(specialPackage);
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            decimal packagingPrice = 0;
            if (packagingOption == PackagingOptions.Special)
            {
                while (true)
                {
                    try
                    {
                        Console.WriteLine("Enter the special packaging price:");
                        packagingPrice = decimal.Parse(Console.ReadLine());
                        manager.ValidatePrice(packagingPrice);
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid price format. Please enter a valid number.");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }

            if (manager.AddProductToSeller(username, productName, category, price, packagingOption == PackagingOptions.Special, packagingPrice))
            {
                Console.WriteLine("Product added to the seller's inventory successfully.");
            }
            else
            {
                Console.WriteLine("Product could not be added.");
            }
        }

        static void AddProductToBuyerCart(SystemManager manager)
        {
            Console.WriteLine("Enter the buyer's username:");
            string username = Console.ReadLine();
            if (!manager.BuyerExists(username))
            {
                Console.WriteLine("Buyer not found.");
                return;
            }

            List<Seller> sellersWithProducts = manager.GetSellersWithProducts();
            if (sellersWithProducts.Count == 0)
            {
                Console.WriteLine("No sellers have products for sale.");
                return;
            }

            Console.WriteLine("Sellers with products for sale:");
            foreach (Seller seller in sellersWithProducts)
            {
                Console.WriteLine($"Seller: {seller.UserName}");
                foreach (Product product in seller.ProductsForSale)
                {
                    string packagingInfo;
                    if (product.packageOption == PackagingOptions.Special)
                    {
                        packagingInfo = $", Packaging Price: {((SpecialPackagedProduct)product).PackagingPrice} - Packaging: Special";
                    }
                    else
                    {
                        packagingInfo = " - Packaging: Regular";
                    }
                    Console.WriteLine($"Product ID: {product.ID}, Product: {product.Name}, Category: {product.Category}, Price: {product.OriginalPrice}{packagingInfo}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Enter the product ID or name:");
            string productInput = Console.ReadLine();
            try
            {
                int productId = int.Parse(productInput);
                Product selectedProduct = manager.FindProductById(productId);
                if (selectedProduct != null)
                {
                    AddProductByIdToCart(manager, username, selectedProduct);
                }
                else
                {
                    Console.WriteLine("Product not found.");
                }
            }
            catch (FormatException)
            {
                List<Product> matchingProducts = manager.FindProductsByName(productInput);
                if (matchingProducts.Count == 0)
                {
                    Console.WriteLine("Product not found.");
                    return;
                }

                if (matchingProducts.Count == 1)
                {
                    AddProductByIdToCart(manager, username, matchingProducts[0]);
                }
                else
                {
                    Console.WriteLine("Multiple sellers have this product. Enter the seller's name from whom you want to buy:");
                    string sellerName = Console.ReadLine();
                    foreach (Product product in matchingProducts)
                    {
                        if (product.Seller.UserName.Equals(sellerName, StringComparison.OrdinalIgnoreCase))
                        {
                            AddProductByIdToCart(manager, username, product);
                            return;
                        }
                    }
                    Console.WriteLine("The seller does not have the requested product.");
                }
            }
        }

        static void AddProductByIdToCart(SystemManager manager, string buyerUsername, Product selectedProduct)
        {
            PackagingOptions packagingOption = PackagingOptions.Regular;
            decimal packagingPrice = 0;
            if (selectedProduct is SpecialPackagedProduct)
            {
                while (true)
                {
                    try
                    {
                        Console.WriteLine("Do you want special package? (1: Regular, 2: Special):");
                        string buyerPackage = Console.ReadLine();
                        packagingOption = manager.ParsePackagingOptionInput(buyerPackage);
                        if (packagingOption == PackagingOptions.Special)
                        {
                            packagingPrice = ((SpecialPackagedProduct)selectedProduct).PackagingPrice;
                        }
                        else
                        {
                            packagingPrice = 0;
                        }
                        break;
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }

            manager.AddProductToBuyerCart(buyerUsername, selectedProduct.Seller.UserName, selectedProduct.Name, packagingOption, packagingPrice);
            Console.WriteLine($"Selected product '{selectedProduct.Name}'");
            Console.WriteLine("Product added to the buyer's cart successfully.");
        }

        static void CompleteOrderForBuyer(SystemManager manager)
        {
            Console.WriteLine("Enter the buyer's username:");
            string username = Console.ReadLine();
            Buyer buyer = manager.GetBuyerByName(username);
            if (buyer == null)
            {
                Console.WriteLine("Buyer not found.");
                return;
            }

            Console.WriteLine("Current Shopping Cart:");
            foreach (Product product in buyer.ShoppingCart)
            {
                string packagingInfo;
                if (product.packageOption == PackagingOptions.Special)
                {
                    packagingInfo = $", Packaging Price: {((SpecialPackagedProduct)product).PackagingPrice} - Special Packaging";
                }
                else
                {
                    packagingInfo = " - Regular Packaging";
                }
                Console.WriteLine($"Product ID: {product.ID}, Product: {product.Name}, Category: {product.Category}, Price: {product.OriginalPrice}{packagingInfo}");
            }

            Console.WriteLine($"Total Shopping Cart Price: {buyer.GetShoppingCartTotalPrice()}");

            Console.WriteLine("Are you sure you want to complete the order? (1: Yes, 2: No):");
            string input = Console.ReadLine();
            if (input == "1")
            {
                try
                {
                    string result = manager.CompleteOrderForBuyer(username);
                    Console.WriteLine(result);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Order not completed.");
            }
        }

        static void DisplayAllUsers(SystemManager manager, bool displayBuyers, bool displayOrderHistory)
        {
            if (displayBuyers)
            {
                List<Buyer> buyers = manager.Buyers;
                if (buyers.Count == 0)
                {
                    Console.WriteLine("No buyers found.");
                    return;
                }

                foreach (Buyer buyer in buyers)
                {
                    Console.WriteLine(buyer);
                    Console.WriteLine("Shopping Cart:");
                    if (buyer.ShoppingCart.Count == 0)
                    {
                        Console.WriteLine("Shopping cart is currently empty.");
                    }
                    else
                    {
                        foreach (Product product in buyer.ShoppingCart)
                        {
                            string packagingInfo;
                            if (product.packageOption == PackagingOptions.Special)
                            {
                                packagingInfo = $", Packaging Price: {((SpecialPackagedProduct)product).PackagingPrice} - Special Packaging";
                            }
                            else
                            {
                                packagingInfo = " - Regular Packaging";
                            }
                            Console.WriteLine($"Product ID: {product.ID}, Product: {product.Name}, Category: {product.Category}, Price: {product.OriginalPrice}{packagingInfo}");
                        }
                        Console.WriteLine($"Total Shopping Cart Price: {buyer.GetShoppingCartTotalPrice()}");
                    }

                    if (displayOrderHistory)
                    {
                        Console.WriteLine("Order History:");
                        List<Order> lastOrders = buyer.LastOrders;
                        if (lastOrders.Count == 0)
                        {
                            Console.WriteLine("No orders have been made yet.");
                        }
                        else
                        {
                            foreach (Order order in lastOrders)
                            {
                                if (order != null)
                                {
                                    Console.WriteLine(order);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                List<Seller> sellers = manager.Sellers;
                if (sellers.Count == 0)
                {
                    Console.WriteLine("No sellers found.");
                    return;
                }

                sellers.Sort(new SellerComparer());
                foreach (Seller seller in sellers)
                {
                    Console.WriteLine(seller);
                    Console.WriteLine("Products for Sale: ");
                    if (seller.ProductsForSale.Count == 0)
                    {
                        Console.WriteLine("No products available for sale.");
                    }
                    else
                    {
                        foreach (Product product in seller.ProductsForSale)
                        {
                            string packagingInfo;
                            if (product.packageOption == PackagingOptions.Special)
                            {
                                packagingInfo = $", Packaging Price: {((SpecialPackagedProduct)product).PackagingPrice} - Packaging: Special";
                            }
                            else
                            {
                                packagingInfo = " - Packaging: Regular";
                            }
                            Console.WriteLine($"Product ID: {product.ID}, Product: {product.Name}, Category: {product.Category}, Price: {product.OriginalPrice}{packagingInfo}");
                        }
                    }
                    Console.WriteLine();
                }
            }
        }

        static void CreateNewCartFromOrderHistory(SystemManager manager)
        {
            Console.WriteLine("Enter the buyer's username:");
            string username = Console.ReadLine();
            Buyer buyer = manager.GetBuyerByName(username);
            if (buyer == null)
            {
                Console.WriteLine("Buyer not found.");
                return;
            }

            Console.WriteLine("Order History:");
            List<Order> lastOrders = buyer.LastOrders;
            if (lastOrders.Count == 0)
            {
                Console.WriteLine("No orders have been made yet.");
                return;
            }

            for (int i = 0; i < lastOrders.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Order ID: {lastOrders[i].OrderID}");
                foreach (Product product in lastOrders[i].GetProducts())
                {
                    string packagingInfo;
                    if (product.packageOption == PackagingOptions.Special)
                    {
                        packagingInfo = $", Packaging Price: {((SpecialPackagedProduct)product).PackagingPrice} - Special Packaging";
                    }
                    else
                    {
                        packagingInfo = " - Regular Packaging";
                    }
                    Console.WriteLine($"Product ID: {product.ID}, Product: {product.Name}, Category: {product.Category}, Price: {product.OriginalPrice}{packagingInfo}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Enter the number of the order to create a new cart from (or type 'cancel' to exit):");
            string input = Console.ReadLine();
            if (input.ToLower() == "cancel")
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }

            int orderNumber;
            try
            {
                orderNumber = int.Parse(input);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid order number.");
                return;
            }

            try
            {
                buyer.CreateNewCartFromHistory(orderNumber - 1);
                Console.WriteLine("New cart created from the selected order.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void CompareTwoBuyers(SystemManager manager)
        {
            Console.WriteLine("Enter the first buyer's username:");
            string buyer1Username = Console.ReadLine();
            Buyer buyer1 = manager.GetBuyerByName(buyer1Username);
            if (buyer1 == null)
            {
                Console.WriteLine("First buyer not found.");
                return;
            }

            Console.WriteLine("Enter the second buyer's username:");
            string buyer2Username = Console.ReadLine();
            Buyer buyer2 = manager.GetBuyerByName(buyer2Username);
            if (buyer2 == null)
            {
                Console.WriteLine("Second buyer not found.");
                return;
            }

            if (buyer1.GetShoppingCartTotalPrice() > buyer2.GetShoppingCartTotalPrice())
            {
                Console.WriteLine($"Buyer {buyer1.UserName} has a higher cart total:");
                foreach (Product product in buyer1.ShoppingCart)
                {
                    string packagingInfo;
                    if (product.packageOption == PackagingOptions.Special)
                    {
                        packagingInfo = $", Packaging Price: {((SpecialPackagedProduct)product).PackagingPrice} - Special Packaging";
                    }
                    else
                    {
                        packagingInfo = " - Regular Packaging";
                    }
                    Console.WriteLine($"Product ID: {product.ID}, Product: {product.Name}, Category: {product.Category}, Price: {product.OriginalPrice}{packagingInfo}");
                }
                Console.WriteLine($"Total Shopping Cart Price: {buyer1.GetShoppingCartTotalPrice()}");
            }
            else if (buyer1.GetShoppingCartTotalPrice() < buyer2.GetShoppingCartTotalPrice())
            {
                Console.WriteLine($"Buyer {buyer2.UserName} has a higher cart total:");
                foreach (Product product in buyer2.ShoppingCart)
                {
                    string packagingInfo;
                    if (product.packageOption == PackagingOptions.Special)
                    {
                        packagingInfo = $", Packaging Price: {((SpecialPackagedProduct)product).PackagingPrice} - Special Packaging";
                    }
                    else
                    {
                        packagingInfo = " - Regular Packaging";
                    }
                    Console.WriteLine($"Product ID: {product.ID}, Product: {product.Name}, Category: {product.Category}, Price: {product.OriginalPrice}{packagingInfo}");
                }
                Console.WriteLine($"Total Shopping Cart Price: {buyer2.GetShoppingCartTotalPrice()}");
            }
            else
            {
                Console.WriteLine("Both buyers have the same cart total:");
                foreach (Product product in buyer1.ShoppingCart)
                {
                    string packagingInfo;
                    if (product.packageOption == PackagingOptions.Special)
                    {
                        packagingInfo = $", Packaging Price: {((SpecialPackagedProduct)product).PackagingPrice} - Special Packaging";
                    }
                    else
                    {
                        packagingInfo = " - Regular Packaging";
                    }
                    Console.WriteLine($"Product ID: {product.ID}, Product: {product.Name}, Category: {product.Category}, Price: {product.OriginalPrice}{packagingInfo}");
                }
                Console.WriteLine($"Total Shopping Cart Price: {buyer1.GetShoppingCartTotalPrice()}");

                foreach (Product product in buyer2.ShoppingCart)
                {
                    string packagingInfo;
                    if (product.packageOption == PackagingOptions.Special)
                    {
                        packagingInfo = $", Packaging Price: {((SpecialPackagedProduct)product).PackagingPrice} - Special Packaging";
                    }
                    else
                    {
                        packagingInfo = " - Regular Packaging";
                    }
                    Console.WriteLine($"Product ID: {product.ID}, Product: {product.Name}, Category: {product.Category}, Price: {product.OriginalPrice}{packagingInfo}");
                }
                Console.WriteLine($"Total Shopping Cart Price: {buyer2.GetShoppingCartTotalPrice()}");
            }
        }
    }

    internal class SellerComparer : IComparer<Seller>
    {
        public int Compare(Seller seller1, Seller seller2)
        {
            if (seller1 == null || seller2 == null)
            {
                throw new ArgumentException("Both objects being compared must be of type Seller.");
            }

            int productCountComparison = seller2.ProductsForSale.Count.CompareTo(seller1.ProductsForSale.Count);
            if (productCountComparison == 0)
            {
                return string.Compare(seller1.UserName, seller2.UserName, StringComparison.OrdinalIgnoreCase);
            }
            return productCountComparison;
        }
    }
}

