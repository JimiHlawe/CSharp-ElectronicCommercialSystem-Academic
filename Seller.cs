using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Seller : User, IComparable<Seller>, ICloneable
    {
        private List<Product> productsForSale;

        public Seller(string username, string password, Address address) : base(username, password, address)
        {
            productsForSale = new List<Product>();
        }

        public void AddProduct(Product product)
        {
            productsForSale.Add(product);
        }

        public Product FindProductByName(string productName)
        {
            foreach (Product product in productsForSale)
            {
                if (product.Name.Equals(productName, StringComparison.OrdinalIgnoreCase))
                {
                    return product;
                }
            }
            return null;
        }

        public List<Product> ProductsForSale
        {
            get { return productsForSale; }
        }

        public int CompareTo(Seller other)
        {
            if (other == null) return 1;

            int productCountComparison = other.ProductsForSale.Count.CompareTo(ProductsForSale.Count);
            if (productCountComparison == 0)
            {
                return string.Compare(UserName, other.UserName, StringComparison.OrdinalIgnoreCase);
            }
            return productCountComparison;
        }

        public object Clone()
        {
            Seller clone = (Seller)this.MemberwiseClone();
            clone.productsForSale = new List<Product>();
            foreach (Product product in this.productsForSale)
            {
                clone.productsForSale.Add((Product)product.Clone());
            }
            return clone;
        }

        public override string ToString()
        {
            return "ID: " + UserID + ", Username: " + username + ", Password: " + password + ", Address: " + address + ", Products for Sale: " + productsForSale.Count;
        }
    }
}
