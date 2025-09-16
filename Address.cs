using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Address : ICloneable
    {
        private string country;
        private string city;
        private string street;
        private int buildingNumber;

        public Address(string country, string city, string street, int buildingNumber)
        {
            Country = country;
            City = city;
            Street = street;
            BuildingNumber = buildingNumber;
        }

        public string Country
        {
            get { return country; }
            set
            {
                ValidateCountry(value);
                country = value;
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                ValidateCity(value);
                city = value;
            }
        }

        public string Street
        {
            get { return street; }
            set
            {
                ValidateStreet(value);
                street = value;
            }
        }

        public int BuildingNumber
        {
            get { return buildingNumber; }
            set
            {
                ValidateBuildingNumber(value);
                buildingNumber = value;
            }
        }

        public static void ValidateCountry(string country)
        {
            if (string.IsNullOrEmpty(country) || !AllLetters(country))
                throw new ArgumentException("Invalid country name.");
        }

        public static void ValidateCity(string city)
        {
            if (string.IsNullOrEmpty(city) || !AllLetters(city))
                throw new ArgumentException("Invalid city name.");
        }

        public static void ValidateStreet(string street)
        {
            if (string.IsNullOrEmpty(street) || !AllLetters(street))
                throw new ArgumentException("Invalid street name.");
        }

        public static void ValidateBuildingNumber(int buildingNumber)
        {
            if (buildingNumber <= 0)
                throw new ArgumentException("Invalid building number.");
        }

        private static bool AllLetters(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Address other = (Address)obj;
            return country == other.country && city == other.city && street == other.street && buildingNumber == other.buildingNumber;
        }

        public override string ToString()
        {
            return country + ", " + city + ", " + street + ", " + buildingNumber;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
