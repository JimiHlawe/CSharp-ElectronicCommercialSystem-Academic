using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Buyer : User, ICloneable
    {
        private List<Product> shoppingCart;
        private List<Order> lastOrders;

        public Buyer(string username, string password, Address address) : base(username, password, address)
        {
            shoppingCart = new List<Product>();
            lastOrders = new List<Order>();
        }

        public void AddProductToCart(Product product, PackagingOptions packagingOption, decimal packagingPrice = 0)
        {
            if (packagingOption == PackagingOptions.Special && product is SpecialPackagedProduct)
            {
                SpecialPackagedProduct newProduct = new SpecialPackagedProduct(product.GetCategoryEnum(), product.Name, product.OriginalPrice, packagingPrice);
                newProduct.ID = product.ID;
                ShoppingCart.Add(newProduct);
            }
            else
            {
                Product newProduct = new Product(product, packagingOption);
                newProduct.ID = product.ID;
                ShoppingCart.Add(newProduct);
            }
        }

        public List<Product> ShoppingCart
        {
            get { return shoppingCart; }
        }

        public decimal GetShoppingCartTotalPrice()
        {
            decimal total = 0;
            foreach (Product product in shoppingCart)
            {
                total += product.Price;
            }
            return total;
        }

        public void AddShoppingCartToOrders()
        {
            Order newOrder = new Order(this);
            foreach (Product product in shoppingCart)
            {
                newOrder.AddProduct(product);
            }
            lastOrders.Add(newOrder);
        }

        public void ClearShoppingCart()
        {
            shoppingCart.Clear();
        }

        public List<Order> LastOrders
        {
            get { return lastOrders; }
        }

        public void CreateNewCartFromHistory(int orderIndex)
        {
            if (orderIndex >= 0 && orderIndex < lastOrders.Count)
            {
                Order selectedOrder = lastOrders[orderIndex];
                shoppingCart.Clear();
                foreach (Product product in selectedOrder.GetProducts())
                {
                    shoppingCart.Add((Product)product.Clone());
                }
            }
        }

        public static bool operator >(Buyer buyer1, Buyer buyer2)
        {
            return buyer1.GetShoppingCartTotalPrice() > buyer2.GetShoppingCartTotalPrice();
        }

        public static bool operator <(Buyer buyer1, Buyer buyer2)
        {
            return buyer1.GetShoppingCartTotalPrice() < buyer2.GetShoppingCartTotalPrice();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Buyer other = (Buyer)obj;
            return GetShoppingCartTotalPrice() == other.GetShoppingCartTotalPrice();
        }

        public override string ToString()
        {
            return "ID: " + UserID + ", Username: " + username + ", Password: " + password + ", Address: " + address + ", Shopping Cart Total Price: " + GetShoppingCartTotalPrice();
        }

        public object Clone()
        {
            Buyer clone = (Buyer)this.MemberwiseClone();
            clone.shoppingCart = new List<Product>();
            foreach (Product product in this.shoppingCart)
            {
                clone.shoppingCart.Add((Product)product.Clone());
            }
            clone.lastOrders = new List<Order>();
            foreach (Order order in this.lastOrders)
            {
                clone.lastOrders.Add((Order)order.Clone());
            }
            return clone;
        }
    }
}
