using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class SystemManager
    {
        public string SystemName { get; private set; }
        private int buyerCapacity = 2;
        private int sellerCapacity = 2;
        private int buyerCount = 0;
        private int sellerCount = 0;

        public List<Buyer> Buyers { get; private set; }
        public List<Seller> Sellers { get; private set; }

        public SystemManager(string systemName)
        {
            SystemName = systemName;
            Buyers = new List<Buyer>(buyerCapacity);
            Sellers = new List<Seller>(sellerCapacity);
        }

        public bool UserExists(string username)
        {
            foreach (Buyer buyer in Buyers)
            {
                if (buyer.UserName.Equals(username))
                {
                    return true;
                }
            }
            foreach (Seller seller in Sellers)
            {
                if (seller.UserName.Equals(username))
                {
                    return true;
                }
            }
            return false;
        }

        public void ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username) || !username.Any(char.IsLetter) || UserExists(username))
            {
                throw new ArgumentException("Username must contain letters and should be unique.");
            }
        }

        public void ValidatePassword(string password)
        {
            if (password.Length < 8 || !password.Any(char.IsDigit) || !password.Any(char.IsLetter))
            {
                throw new ArgumentException("Password must be at least 8 characters long and contain both letters and numbers.");
            }
        }

        public bool AddUser(string username, string password, Address address, bool isBuyer)
        {
            if (UserExists(username))
            {
                throw new ArgumentException("User already exists.");
            }

            if (isBuyer)
            {
                if (buyerCount == buyerCapacity)
                {
                    buyerCapacity *= 2;
                    List<Buyer> newBuyers = new List<Buyer>(buyerCapacity);
                    newBuyers.AddRange(Buyers);
                    Buyers = newBuyers;
                }
                Buyers.Add(new Buyer(username, password, address));
                buyerCount++;
            }
            else
            {
                if (sellerCount == sellerCapacity)
                {
                    sellerCapacity *= 2;
                    List<Seller> newSellers = new List<Seller>(sellerCapacity);
                    newSellers.AddRange(Sellers);
                    Sellers = newSellers;
                }
                Sellers.Add(new Seller(username, password, address));
                sellerCount++;
            }
            return true;
        }

        public bool AddProductToSeller(string sellerUsername, string productName, ProductCategory category, decimal price, bool isSpecialPackaging, decimal packagingPrice = 0)
        {
            foreach (Seller seller in Sellers)
            {
                if (seller.UserName.Equals(sellerUsername))
                {
                    Product product;
                    if (isSpecialPackaging)
                    {
                        product = new SpecialPackagedProduct(category, productName, price, packagingPrice);
                    }
                    else
                    {
                        product = new Product(category, productName, price, PackagingOptions.Regular);
                    }

                    product.Seller = seller;
                    seller.AddProduct(product);
                    return true;
                }
            }
            return false;
        }

        public bool ProductExistsForSeller(string sellerUsername, string productName)
        {
            foreach (Seller seller in Sellers)
            {
                if (seller.UserName.Equals(sellerUsername, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (Product product in seller.ProductsForSale)
                    {
                        if (product.Name.Equals(productName, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool SellerExists(string username)
        {
            foreach (Seller seller in Sellers)
            {
                if (seller.UserName.Equals(username))
                {
                    return true;
                }
            }
            return false;
        }

        public bool AddProductToBuyerCart(string buyerUsername, string sellerName, string productName, PackagingOptions packagingOption, decimal packagingPrice)
        {
            foreach (Buyer buyer in Buyers)
            {
                if (buyer.UserName.Equals(buyerUsername))
                {
                    foreach (Seller seller in Sellers)
                    {
                        if (seller.UserName.Equals(sellerName))
                        {
                            Product product = seller.FindProductByName(productName);
                            if (product != null)
                            {
                                buyer.AddProductToCart(product, packagingOption, packagingPrice);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool BuyerExists(string username)
        {
            foreach (Buyer buyer in Buyers)
            {
                if (buyer.UserName.Equals(username))
                {
                    return true;
                }
            }
            return false;
        }

        public Product FindProductById(int productId)
        {
            foreach (Seller seller in Sellers)
            {
                foreach (Product product in seller.ProductsForSale)
                {
                    if (product.ID == productId)
                    {
                        return product;
                    }
                }
            }
            return null;
        }

        public List<Product> FindProductsByName(string productName)
        {
            List<Product> matchingProducts = new List<Product>();
            foreach (Seller seller in Sellers)
            {
                foreach (Product product in seller.ProductsForSale)
                {
                    if (product.Name.Equals(productName, StringComparison.OrdinalIgnoreCase))
                    {
                        matchingProducts.Add(product);
                    }
                }
            }
            return matchingProducts;
        }

        public string CompleteOrderForBuyer(string buyerUsername)
        {
            foreach (Buyer buyer in Buyers)
            {
                if (buyer.UserName.Equals(buyerUsername))
                {
                    if (buyer.ShoppingCart.Count == 1)
                    {
                        throw new InvalidOperationException("Cannot complete order with only one item in the cart.");
                    }
                    if (buyer.ShoppingCart.Count == 0)
                    {
                        return "No products in the shopping cart.";
                    }

                    decimal totalPrice = 0m;
                    string productList = "";

                    foreach (Product product in buyer.ShoppingCart)
                    {
                        totalPrice += product.Price;
                        productList += $"{product} ";

                        if (product.packageOption == PackagingOptions.Special)
                        {
                            productList += "- Special Packaging";
                        }

                        productList += "\n";
                    }
                    buyer.AddShoppingCartToOrders();
                    buyer.ClearShoppingCart();

                    Order lastOrder = buyer.LastOrders[buyer.LastOrders.Count - 1];

                    return $"Order ID: {lastOrder.OrderID}\nProducts in the order:\n{productList}\nThe total price for the buyer's order is: {totalPrice}\nOrder completed and shopping cart cleared.";
                }
            }
            return "Buyer not found.";
        }

        public void ValidatePrice(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentException("Price must be a positive number.");
            }
        }

        public void ValidatePackagingOption(string option)
        {
            if (!option.Equals("Regular", StringComparison.OrdinalIgnoreCase) && !option.Equals("Special", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Invalid packaging option.");
            }
        }

        public PackagingOptions ParsePackagingOptionInput(string input)
        {
            if (input == "1")
            {
                return PackagingOptions.Regular;
            }
            else if (input == "2")
            {
                return PackagingOptions.Special;
            }
            else
            {
                throw new ArgumentException("Invalid packaging option.");
            }
        }

        public ProductCategory ParseProductCategoryInput(string input)
        {
            if (input == "1")
            {
                return ProductCategory.Clothing;
            }
            else if (input == "2")
            {
                return ProductCategory.Electronics;
            }
            else if (input == "3")
            {
                return ProductCategory.Kids;
            }
            else if (input == "4")
            {
                return ProductCategory.Office;
            }
            else
            {
                throw new ArgumentException("Invalid product category.");
            }
        }

        public ProductCategory ParseProductCategory(string category)
        {
            foreach (ProductCategory item in Enum.GetValues(typeof(ProductCategory)))
            {
                if (item.ToString().Equals(category, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }
            return ProductCategory.Undefined;
        }

        public Buyer GetBuyerByName(string username)
        {
            foreach (Buyer buyer in Buyers)
            {
                if (buyer.UserName.Equals(username))
                {
                    return buyer;
                }
            }
            return null;
        }

        public List<Seller> GetSellersWithProducts()
        {
            List<Seller> sellersWithProducts = new List<Seller>();
            foreach (Seller seller in Sellers)
            {
                if (seller.ProductsForSale.Count > 0)
                {
                    sellersWithProducts.Add(seller);
                }
            }
            return sellersWithProducts;
        }

        public static SystemManager operator +(SystemManager manager, Buyer buyer)
        {
            manager.Buyers.Add(buyer);
            return manager;
        }

        public static SystemManager operator +(SystemManager manager, Seller seller)
        {
            manager.Sellers.Add(seller);
            return manager;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            SystemManager other = (SystemManager)obj;
            return SystemName == other.SystemName;
        }
    }
}