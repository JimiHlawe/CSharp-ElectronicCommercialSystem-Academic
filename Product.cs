using FinalProject;
using System;

namespace FinalProject
{
    internal class Product
    {
        private static int counter = 1000;
        private int idProduct;
        private string category;
        private string name;
        protected decimal price;
        public PackagingOptions packageOption { get; private set; }
        public decimal OriginalPrice { get; private set; }
        public Seller Seller { get; set; }

        public Product(ProductCategory category, string name, decimal price, PackagingOptions packageOption)
        {
            Category = category.ToString();
            Name = name;
            Price = price;
            this.packageOption = packageOption;
            OriginalPrice = price;
            ID = counter++;
        }

        public Product(Product originalProduct, PackagingOptions packageOption)
        {
            ID = originalProduct.ID;
            Category = originalProduct.Category;
            Name = originalProduct.Name;
            Price = originalProduct.OriginalPrice;
            OriginalPrice = originalProduct.OriginalPrice;
            this.packageOption = packageOption;
        }

        public string Category
        {
            get { return category; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Category cannot be empty.");
                }
                category = value;
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Name cannot be empty.");
                }
                name = value;
            }
        }

        public virtual decimal Price
        {
            get { return price; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Invalid price.");
                }
                price = value;
            }
        }

        public int ID
        {
            get { return idProduct; }
            set { idProduct = value; }
        }

        public ProductCategory GetCategoryEnum()
        {
            foreach (ProductCategory item in Enum.GetValues(typeof(ProductCategory)))
            {
                if (string.Compare(item.ToString(), category, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return item;
                }
            }
            return ProductCategory.Undefined;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Product other = (Product)obj;
            return idProduct == other.idProduct && category == other.category && name == other.name && price == other.price && packageOption == other.packageOption && OriginalPrice == other.OriginalPrice;
        }

        public override string ToString()
        {
            return $"Product ID: {idProduct}, Product: {name}, Category: {category}, Price: {OriginalPrice}";
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
