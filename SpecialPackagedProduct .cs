using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class SpecialPackagedProduct : Product
    {
        private decimal packagingPrice;

        public SpecialPackagedProduct(ProductCategory category, string name, decimal price, decimal packagingPrice) : base(category, name, price, PackagingOptions.Special)
        {
            PackagingPrice = packagingPrice;
        }

        public decimal PackagingPrice
        {
            get { return packagingPrice; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Invalid packaging price.");
                }
                packagingPrice = value;
            }
        }

        public override decimal Price
        {
            get { return base.Price + packagingPrice; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            SpecialPackagedProduct other = (SpecialPackagedProduct)obj;
            return base.Equals(other) && packagingPrice == other.packagingPrice;
        }

        public override string ToString()
        {
            return base.ToString() + ", Packaging Price: " + packagingPrice;
        }

        public new object Clone()
        {
            SpecialPackagedProduct clone = (SpecialPackagedProduct)base.Clone();
            clone.packagingPrice = packagingPrice;
            return clone;
        }
    }
}
