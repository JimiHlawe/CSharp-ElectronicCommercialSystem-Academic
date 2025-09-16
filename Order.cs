using FinalProject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Order : ICloneable
    {
        private static int orderCounter = 0;
        public string OrderID { get; private set; }
        private List<Product> products;
        public Buyer BuyerDetails { get; private set; }

        public Order(Buyer buyer)
        {
            OrderID = GenerateOrderID();
            products = new List<Product>();
            BuyerDetails = buyer;
        }

        private string GenerateOrderID()
        {
            string timestamp = DateTime.Now.ToString("dd/MM/yyyy,HH:mm");
            orderCounter++;
            return orderCounter.ToString("D4") + " \t" + timestamp;
        }

        public void AddProduct(Product product)
        {
            products.Add((Product)product.Clone());
        }

        public List<Product> GetProducts()
        {
            return products;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Order other = (Order)obj;
            if (OrderID != other.OrderID || !BuyerDetails.Equals(other.BuyerDetails))
            {
                return false;
            }

            if (products.Count != other.products.Count)
            {
                return false;
            }

            for (int i = 0; i < products.Count; i++)
            {
                if (!products[i].Equals(other.products[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder productsInfo = new StringBuilder();
            foreach (Product product in products)
            {
                productsInfo.AppendLine(product.ToString());
            }
            return "Order ID: " + OrderID + "\n" + productsInfo.ToString();
        }

        public object Clone()
        {
            Order clone = (Order)this.MemberwiseClone();
            clone.products = new List<Product>();
            foreach (Product product in this.products)
            {
                clone.products.Add((Product)product.Clone());
            }
            return clone;
        }
    }
}
